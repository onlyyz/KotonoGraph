using UnityEngine;

namespace Kotono.Code
{
    [System.Serializable]
    public class CodePortEdge 
    {
       
    }
    
    [System.Serializable]
    public struct CodeCodePortEdgeStruct
    {
        public string nodeID;
        public int portIndex;

        public CodeCodePortEdgeStruct(string nodeID, int portIndex)
        {
            this.nodeID = nodeID;
            this.portIndex = portIndex;
        }
    }
}
