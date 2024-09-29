using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("State","Wwise/State")]
    public class WwiseState : WwiseNode
    {
        [WWISE]
        public AK.Wwise.State State = new AK.Wwise.State();
      
        
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            State.SetValue();
            return base.OnProcess(currentGraph);
        }
    }
}
