using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kotono.Code
{
    [CreateAssetMenu(menuName = "Kotono/Code Graph Asset")]
    public class CodeGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> m_nodes;
        [SerializeReference]
        private List<CodeGraphConnection> m_connections;

        private Dictionary<string, CodeGraphNode> m_nodeDictionary;

        public GameObject self;
        
        
        public List<CodeGraphNode> Nodes => m_nodes;
        public List<CodeGraphConnection> Connections => m_connections;
        
        public CodeGraphAsset()
        {
            m_nodes = new List<CodeGraphNode>();
            m_connections = new List<CodeGraphConnection>();
        }

        public void Init(GameObject gameObject)
        {
            this.self = gameObject;
            m_nodeDictionary = new Dictionary<string, CodeGraphNode>();
            foreach (var node in Nodes)
            {
                m_nodeDictionary.Add(node.id, node);
            }
        }
        
        public CodeGraphNode GetStartNode()
        {
            StartNode[] startNodes = Nodes.OfType<StartNode>().ToArray();
            if (startNodes.Length == 0)
            {
                Debug.LogError("this not the StartNode");
                return null;
            }
            return startNodes.FirstOrDefault();
        }

        public CodeGraphNode GetNode(string nextNodeID)
        {
            if (m_nodeDictionary.TryGetValue(nextNodeID, out CodeGraphNode node))
            {
                return node;
            }
            return null;
        }
        
        public CodeGraphNode GetNodeFromOutput(string outputNodeId, int index)
        {
            foreach (CodeGraphConnection Connection in Connections)
            {
                if (Connection.outputPort.nodeID == outputNodeId && Connection.outputPort.portIndex == index)
                {
                    string nodeID = Connection.inputPort.nodeID;
                    CodeGraphNode inputNode = m_nodeDictionary[nodeID];
                    return inputNode;
                }
            }

            return null;
        }
    }
}