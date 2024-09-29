using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("MoveNode","Action/Move")]
    public class MoveNode : CodeGraphNode
    {
        [UPROPERTY]
        public Vector3 Direction = Vector3.forward;
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            currentGraph.self.transform.position += Direction;
            return base.OnProcess(currentGraph);
        }
    }
}
