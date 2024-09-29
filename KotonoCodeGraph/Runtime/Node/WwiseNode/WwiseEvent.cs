using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("Event","Wwise/Event")]
    public class WwiseEvent : WwiseNode
    {
        [WWISE]
        public AK.Wwise.Event scriptEvent = new AK.Wwise.Event();
        [WWISE]
        public AK.Wwise.Event script1Event = new AK.Wwise.Event();
        
        public override string OnProcess(CodeGraphAsset currentGraph)
        {
            scriptEvent.Post(new GameObject("aaa"));
            return base.OnProcess(currentGraph);
        }
    }
}
