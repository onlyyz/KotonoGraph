using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands.BranchExplorer;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using Port = UnityEditor.Experimental.GraphView.Port;

namespace Kotono.Code.Editor
{
    public class CodeGraphView : GraphView
    {
        //保存的资源
        private CodeGraphAsset m_codeGraph;
        private SerializedObject m_serializedObject;
       
        public CodeEditorWindow m_EditorWindow;
        
        //
        public List<CodeEditorNode>  m_graphNodes;
        //保存方便删除和获取。不为保存实体
        public Dictionary<string, CodeEditorNode> m_nodeDictionary;
        public Dictionary<Edge, CodeGraphConnection> m_CodeGraphConnectionDictionary;
        
        private CodeGraphWindowSearchProvider m_searchProvider;
        
        public CodeGraphView(SerializedObject serializedGraphAsset,CodeEditorWindow CodeEditorWindow)
        {
            m_serializedObject = serializedGraphAsset;
            m_codeGraph =(CodeGraphAsset) m_serializedObject.targetObject;
            m_EditorWindow = CodeEditorWindow;
            
            m_graphNodes = new List<CodeEditorNode>();
            m_nodeDictionary = new Dictionary<string, CodeEditorNode>();
            m_CodeGraphConnectionDictionary = new Dictionary<Edge, CodeGraphConnection>();
            
            m_searchProvider = ScriptableObject.CreateInstance<CodeGraphWindowSearchProvider>();
            m_searchProvider.graph = this;
            nodeCreationRequest = ShowSearchWindow;

            AddManipulatorToGraph();
            
            GridBackground background = new GridBackground();
            background.name = "Background";
            Add(background);
            
            background.SendToBack();
            
            this.styleSheets.Add(Resources.Load<StyleSheet>("USS/CodeGraphEditor"));

            //绘制节点
            DrawNodes();
            DrawConnections();
            OnGraphViewChanged();
            
        }

  

        #region Port

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();

            foreach (var node in m_graphNodes)
            {
                allPorts.AddRange(node.Ports);
            }

            foreach (Port p in allPorts)
            {
                if(p == startPort) {continue;}
                if (p.node == startPort.node) {continue;}
                if(p.direction == startPort.direction) {continue;}

                if (p.portType == startPort.portType)
                {
                    ports.Add(p);
                }  
            }
            
          
            return ports;
        }

        #endregion        
        
        #region OnGraphViewChanged
        
        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes =>
            {
                //移动
                if (changes.movedElements != null)
                {
                    Undo.RecordObject(m_serializedObject.targetObject, "Move Node");
                
                    List<CodeEditorNode> nodesToMove = changes.movedElements.OfType<CodeEditorNode>().ToList();
                
                    if (nodesToMove.Count > 0)
                        foreach (var node in nodesToMove)
                            node.SvaPosition();
                }
            
                //移除物体
                if (changes.elementsToRemove != null)
                {
                    Undo.RecordObject(m_serializedObject.targetObject, "Remove Node");
                
                    List<CodeEditorNode> nodesToRemove = changes.elementsToRemove.OfType<CodeEditorNode>().ToList();
             
                    if (nodesToRemove.Count > 0)
                        foreach (var node in nodesToRemove)
                            RemoveNode(node);
                
                    List<Edge> edges = changes.elementsToRemove.OfType<Edge>().ToList();
                    if (edges.Count > 0)
                        foreach (Edge edge in edges)
                            RemoveConnection(edge);
                }

            
                if (changes.edgesToCreate != null)
                {
                    Undo.RecordObject(m_serializedObject.targetObject, "Connection");
                
                    var connections = changes.edgesToCreate;

                    if (connections.Count > 0)
                        foreach (var edge in connections)
                        {
                            CreateEdge(edge);
                           
                        }
                }
                
                return changes;
            });
          
        }
        
        #endregion
        
        #region Graph View Change 删除 增加
        
        private void CreateEdge(Edge edge)
        {
            CodeEditorNode inputNode = (CodeEditorNode)edge.input.node;
            int inputIndex = inputNode.Ports.IndexOf(edge.input);
            CodeEditorNode outputNode = (CodeEditorNode)edge.output.node;
            int outputIndex = outputNode.Ports.IndexOf(edge.output);
            
            
            CodeGraphConnection connection = new  CodeGraphConnection(inputNode.Node.id, inputIndex, outputNode.Node.id,outputIndex);
            m_codeGraph.Connections.Add(connection);
        }


        private void RemoveNode(CodeEditorNode NodeData)
        {
           
            m_codeGraph.Nodes.Remove(NodeData.Node);
            m_nodeDictionary.Remove(NodeData.Node.id);
            m_graphNodes.Remove(NodeData);
            m_serializedObject.Update();
        }


        private void RemoveConnection(Edge e)
        {
            if (m_CodeGraphConnectionDictionary.TryGetValue(e, out CodeGraphConnection connection))
            {
                m_codeGraph.Connections.Remove(connection);
                m_CodeGraphConnectionDictionary.Remove(e);
            }
          
        }
        #endregion

        #region 控制器 搜索树
        
        public void AddManipulatorToGraph()
        {
            //放大缩小
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
        }
        
        private void ShowSearchWindow(NodeCreationContext obj)
        {
            m_searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), m_searchProvider);
        }

        #endregion

        #region Node
        
        public void Add(CodeGraphNode node)
        {
            Undo.RecordObject(m_serializedObject.targetObject, "Add Graph Node");
            //添加进资源中
            m_codeGraph.Nodes.Add(node);
            m_serializedObject.Update();
            
            //转换为Node
            AddNodeToGraph(node);
            BindSerializedObject();
        }

        public void AddNodeToGraph(CodeGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;
            // Debug.Log($"节点类型: {node.typeName}");
            //传递数据
            CodeEditorNode editorNode = new CodeEditorNode(node,m_serializedObject);
            editorNode.SetPosition(node.position);
            
            //保存
            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.id, editorNode);
            
            
            AddElement(editorNode);
        }

        private void BindSerializedObject()
        {
            this.m_serializedObject.Update();
            this.Bind(m_serializedObject);
        }
        

        #endregion
        
        #region Load Draw Node

        private void DrawNodes()
        {
            foreach (var node in m_codeGraph.Nodes)
            {
                AddNodeToGraph(node);
            }

            BindSerializedObject();
        }


        private void DrawConnections()
        {
            if (m_codeGraph.Connections == null)
            {
                return;
            }

            foreach (CodeGraphConnection connetction in m_codeGraph.Connections)
            {
                DrawConnection(connetction);
            }
        }

        private void DrawConnection(CodeGraphConnection connetction)
        {
            CodeEditorNode inputNode = GetNode(connetction.inputPort.nodeID);
            CodeEditorNode outputNode = GetNode(connetction.outputPort.nodeID);

            if (inputNode == null || outputNode == null)
            {
                return;
            }

            Port inPort = inputNode.Ports[connetction.inputPort.portIndex];
            Port outPort = outputNode.Ports[connetction.outputPort.portIndex];

            Edge edge = inPort.ConnectTo(outPort);
            AddElement(edge);

            m_CodeGraphConnectionDictionary.Add(edge,connetction);
        }

        private CodeEditorNode GetNode(string nodeID)
        {
            CodeEditorNode node = null;
            m_nodeDictionary.TryGetValue(nodeID, out node);
            return node;
        }

        
        
        
        #endregion

        
    }
    
}