

using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("EventBase","",false,true)]
    public class EventBase : CodeGraphNode
    {
        public EventManager eventMag
        {
            get
            {
                if (EventManager.Self == null)
                {
                    Debug.LogError("EventManager 抓取不到,你需要启动项目随后就可以抓取到");
                    return null;
                }
                else
                {
                    return EventManager.Self;
                }
            }
        }
        
        protected bool isCoolingDown = false;
        public virtual async void Register()
        {
            
        }
        public virtual async void UnRegister()
        {
            
        }
    }
    
}
