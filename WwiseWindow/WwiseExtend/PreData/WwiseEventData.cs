using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WS.DataClass
{


    [CreateAssetMenu(fileName = "WwiseEventData", menuName = "Wwise/Event Data", order = 1)]
    public class WwiseEventData : ScriptableObject
    {
        public int state;

        [Button]
        public void setttt() => state = eventDatas.Count;
        
        [System.Serializable]
        public class EventData
        {
            public string eventName;
            public float maxAttenuation;
            public float maxDuration = -1;
            public float minDuration = -1;


            public List<string> rtpcs = new List<string>();
            public List<string> switches = new List<string>();
            public List<string> states = new List<string>();
            public List<string> triggers = new List<string>();
        }

        // Dictionary of events and their associated data
        [ShowInInspector] 
        public Dictionary<System.Guid, EventData> eventDatas = new Dictionary<System.Guid, EventData>();
    }
}
