using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("Wwise Global RTPC","Wwise/Global RTPC")]
    public class WwiseGlobalRTPC : WwiseNode
    {
        [WWISE]
        public AK.Wwise.RTPC RTPC = new AK.Wwise.RTPC();
       
     
        
        [UPROPERTY] 
        public float Value;
      
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            RTPC.SetGlobalValue(Value);
            return base.OnProcess(currentGraph);
        }
    }
}
