using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    public class CodeGraphView : GraphView
    {
        private CodeGraphAsset m_codeGraph;
        private SerializedObject m_serializedObject;
        
        
        public CodeEditorWindow m_EditorWindow;
        
        //
        public List<CodeEditorNode>  m_graphNodes;
        public Dictionary<string, CodeEditorNode> m_nodeDictionary;
        
        private CodeGraphWindowSearchProvider m_searchProvider;
        
        public CodeGraphView(SerializedObject serializedGraphAsset,CodeEditorWindow CodeEditorWindow)
        {
            m_serializedObject = serializedGraphAsset;
            m_codeGraph =(CodeGraphAsset) m_serializedObject.targetObject;
            m_EditorWindow = CodeEditorWindow;
            
            m_graphNodes = new List<CodeEditorNode>();
            m_nodeDictionary = new Dictionary<string, CodeEditorNode>();
            
            m_searchProvider = ScriptableObject.CreateInstance<CodeGraphWindowSearchProvider>();
            m_searchProvider.graph = this;
            this.nodeCreationRequest = ShowSearchWindow;

            AddManipulatorToGraph();
            
            GridBackground background = new GridBackground();
            background.name = "Background";
            Add(background);
            
            background.SendToBack();
            
            this.styleSheets.Add(Resources.Load<StyleSheet>("USS/CodeGraphEditor"));

            //绘制节点
            DrawNodes();
        }

   
        #region 搜索树
        private void ShowSearchWindow(NodeCreationContext obj)
        {
            m_searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), m_searchProvider);
        }

        #endregion
        public void AddManipulatorToGraph()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
        }

        public void Add(CodeGraphNode node)
        {
            Undo.RecordObject(m_serializedObject.targetObject, "Add Graph Node");
            //添加进资源中
            m_codeGraph.Nodes.Add(node);
            m_serializedObject.Update();
            
            //转换为Node
            AddNodeToGraph(node);
        }

        public void AddNodeToGraph(CodeGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;
            // Debug.Log($"节点类型: {node.typeName}");
            //传递数据
            CodeEditorNode editorNode = new CodeEditorNode(node);
            editorNode.SetPosition(node.position);
            
            //保存
            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.id, editorNode);
            
            
            AddElement(editorNode);
        }



        #region Draw Node

        private void DrawNodes()
        {
            foreach (var node in m_codeGraph.Nodes)
            {
                AddNodeToGraph(node);
            }
        }


        #endregion
    }
    
}