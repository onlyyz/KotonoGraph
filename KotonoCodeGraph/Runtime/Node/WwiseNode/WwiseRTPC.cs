using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("WwiseRTPC","Wwise/RTPC")]
    public class WwiseRTPC : WwiseNode
    {
        [WWISE]
        public AK.Wwise.RTPC RTPC = new AK.Wwise.RTPC();
       
      
        
        [UPROPERTY] 
        public float Value;
        [UPROPERTY]
        public GameObject Target;
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            RTPC.SetValue(Target,Value);
            return base.OnProcess(currentGraph);
        }
    }
}
