using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("Debug Log","Debug/Log Message")]
    public class DebugLogNode : CodeGraphNode
    {
        [UPROPERTY]
        public string logMessage;
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            Debug.Log(logMessage);
            return base.OnProcess(currentGraph);
        }
    }
}
