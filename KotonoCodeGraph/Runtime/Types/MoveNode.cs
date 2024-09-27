using UnityEngine;

namespace Kotono.Code
{
    [NodeInfo("MoveNode","Action/Move")]
    public class MoveNode : CodeGraphNode
    {
        [ExposedProperty]
        public Vector3 Direction = Vector3.forward;
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            Debug.Log("MoveNode.OnProcess");
            Debug.Log(currentGraph.self.name);
            currentGraph.self.transform.position += Direction;
            return base.OnProcess(currentGraph);
        }
    }
}
