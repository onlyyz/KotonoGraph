using System;
using System.Collections.Generic;
using UnityEngine;


namespace WS.Extend
{
    public static class WwiseExtend
    {
        public delegate void DataEventHandler(object sender, WwiseObjectReference reference, Dictionary<System.Type, WwiseObjectType> scriptTypeMap);

        public static event DataEventHandler DataEvent;
        public static void KotonoExtend(WwiseObjectReference reference,Dictionary<System.Type, WwiseObjectType> ScriptTypeMap)
        {
            
            DataEvent?.Invoke(null, reference,ScriptTypeMap);
            // if (reference.WwiseObjectType == WwiseObjectType.Event)
            // {
            //     DataEevent?.Invoke(null, reference);
            // }
            // if (reference.WwiseObjectType == WwiseObjectType.Event)
            // {
            //     DataEevent?.Invoke(null, reference);
            // }
            // if (reference.WwiseObjectType == WwiseObjectType.Event)
            // {
            //     DataEevent?.Invoke(null, reference);
            // }
        }
    }

   
}