using UnityEngine;

namespace Kotono.Code
{
    [NodeInfo("Debug Log","Debug/Log Message")]
    public class DebugLogNode : CodeGraphNode
    {
        [ExposedProperty]
        public string logMessage;
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            Debug.Log(logMessage);
            return base.OnProcess(currentGraph);
        }
    }
}
