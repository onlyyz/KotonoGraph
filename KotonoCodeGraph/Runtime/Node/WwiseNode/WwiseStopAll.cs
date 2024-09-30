using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("StopAll","Wwise/StopAll")]
    public class WwiseStopAll : WwiseNode
    {
        [UTITLE]
        public string name = " stop all majiko ";
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            AkSoundEngine.StopAll();    
            return base.OnProcess(currentGraph);
        }
    }
}