using System.Collections.Generic;
using UnityEngine;

namespace Kotono.Code
{
    public class CodeNodePort 
    {
        public string portName;             // 端口名称
        public List<EdgeData> connectedEdges; // 与该端口相连的所有Edge

        public CodeNodePort(string name)
        {
            portName = name;
            connectedEdges = new List<EdgeData>();
        }

        public void AddEdge(EdgeData edge)
        {
            connectedEdges.Add(edge);
        }

        public List<EdgeData> GetConnectedEdges()
        {
            return connectedEdges;
        }
    }
    
    [System.Serializable]
    public class EdgeData
    {
        public string fromNodeId;      // 起点节点 ID
        public string fromPortName;    // 起点端口名称
        public string toNodeId;        // 终点节点 ID
        public string toPortName;      // 终点端口名称
    }
}
