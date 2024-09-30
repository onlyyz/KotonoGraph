using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Kotono.Code
{
    [UCLASS("CustomEvent","Event/CustomEvent",false,true)]
    public class CustomEvent : EventBase
    {
        [UFUNCTIONREGISTER]
        public override async void Register()
        {
            if (eventMag == null)
            {
                return;
            }
            eventMag.RegisterHandler("Invoke",Invoke);
        }
        
        public virtual void UnRegister()
        {
            eventMag.UnregisterHandler("Invoke",Invoke);
        }
        public void Invoke()
        {
            Debug.Log("CustomEvent.Invoke");
        }
    }
}