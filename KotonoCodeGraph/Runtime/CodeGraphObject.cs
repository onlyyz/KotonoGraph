using System;
using UnityEngine;

namespace Kotono.Code
{
    public class CodeGraphObject : MonoBehaviour
    {
        [SerializeField]
        private CodeGraphAsset m_graphAsset;

        private CodeGraphAsset graphInstance;
        private void OnEnable()
        {
            graphInstance = Instantiate(m_graphAsset);
            ExecuteAsset();
        }

        private void ExecuteAsset()
        {
            graphInstance.Init(this.gameObject);
            
            CodeGraphNode startNode =  graphInstance.GetStartNode();
            processAndMoveToNextNode(startNode);
        }

        private void processAndMoveToNextNode(CodeGraphNode startNode)
        {
            string nextNodeID =  startNode.OnProcess(graphInstance);
            if (!string.IsNullOrEmpty(nextNodeID))
            {
                CodeGraphNode node = graphInstance.GetNode(nextNodeID);
                processAndMoveToNextNode(node);
            }
        }
    }
}
