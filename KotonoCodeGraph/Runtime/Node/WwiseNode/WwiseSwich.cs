using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("Event","Wwise/Swich")]
    public class WwiseSwich : WwiseNode
    {
        [WWISE]
        public AK.Wwise.Switch Switch = new AK.Wwise.Switch();
        
        [UPROPERTY]
        public GameObject Target;
        
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            Switch.SetValue(Target);
            return base.OnProcess(currentGraph);
        }
    }
}
