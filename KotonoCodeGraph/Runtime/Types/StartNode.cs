using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("Start","Process/Start",false,true)]
    public class StartNode : CodeGraphNode
    {
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            Debug.Log("Start");
            return base.OnProcess(currentGraph);
        }
    }
}
