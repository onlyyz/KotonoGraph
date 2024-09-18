using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using WS.DataClass;

public class WwiseDataFetcherToSO : EditorWindow
{
    // Reference to the ScriptableObject where we will store the data
    public WwiseEventData wwiseEventData;

    [MenuItem("Tools/Wwise Data Fetcher to SO")]
    public static void ShowWindow()
    {
        GetWindow<WwiseDataFetcherToSO>("Wwise Data Fetcher to SO");
    }

    void OnGUI()
    {
        // Field to assign the ScriptableObject
        wwiseEventData = (WwiseEventData)EditorGUILayout.ObjectField("Wwise Event Data SO", wwiseEventData, typeof(WwiseEventData), false);

        if (GUILayout.Button("Fetch Wwise Data"))
        {
            if (wwiseEventData != null)
            {
                StartFetchingData();
            }
            else
            {
                Debug.LogError("Please assign a Wwise Event Data SO.");
            }
        }
    }

    private void StartFetchingData()
    {
        // Clear existing data
        wwiseEventData.eventDatas.Clear();

        // Start fetching Wwise data (mock implementation, replace with actual Wwise fetching logic)
        Task.Run(() =>
        {
            FetchEvents();
        });
    }

    private void FetchEvents()
    {
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

        foreach (var eventWorkUnit in wwiseProjectData.EventWwu)
        {
            foreach (var eventInfo in eventWorkUnit.List)
            {
                // Create a new EventData object for each event
                WwiseEventData.EventData eventData = new WwiseEventData.EventData
                {
                    eventName = eventInfo.Name,
                    maxAttenuation = eventInfo.maxAttenuation,
                    maxDuration = eventInfo.maxDuration,
                    minDuration = eventInfo.minDuration
                };

                // Fetch associated data
                GetRtpc(eventInfo, eventData);
                GetSwitch(eventInfo, eventData);
                GetState(eventInfo, eventData);
                GetTrigger(eventInfo, eventData);

                // Add the event data to the SO
                wwiseEventData.eventDatas.Add(eventInfo.Guid, eventData);
            }
        }

        // Mark the ScriptableObject as dirty to ensure changes are saved
        EditorUtility.SetDirty(wwiseEventData);

        Debug.Log("Wwise data fetching complete!");
    }


    private void GetRtpc(AkWwiseProjectData.Event eventInfo, WwiseEventData.EventData eventData)
    {
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();
    
        // Fetch associated RTPCs
        foreach (var rtpcWwu in wwiseProjectData.RtpcWwu)
        {
            foreach (var rtpcInfo in rtpcWwu.List)
            {
                if (rtpcInfo.Id == eventInfo.Id)
                {
                    // Instead of creating RTPC with Id, use the RTPC name or path in Wwise
                    AK.Wwise.RTPC rtpc = new AK.Wwise.RTPC();
                    eventData.rtpcs.Add(rtpcInfo.Name);  // You can store RTPC names in the ScriptableObject
                }
            }
        }
    }
    
    private void GetSwitch(AkWwiseProjectData.Event eventInfo, WwiseEventData.EventData eventData)
    {
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

        // Fetch associated Switches
        foreach (var switchWwu in wwiseProjectData.SwitchWwu)
        {
            foreach (var switchInfo in switchWwu.List)
            {
                if (switchInfo.Id == eventInfo.Id)
                {
                    // Instead of using Id, use the Switch name
                   
                    eventData.switches.Add(switchInfo.Name);  // Store the Switch name in the ScriptableObject
                }
            }
        }
    }
    
    private void GetState(AkWwiseProjectData.Event eventInfo, WwiseEventData.EventData eventData)
    {
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

        // Fetch associated States
        foreach (var stateWwu in wwiseProjectData.StateWwu)
        {
            foreach (var stateInfo in stateWwu.List)
            {
                if (stateInfo.Id == eventInfo.Id)
                {
                    // Instead of using Id, use the State name
                 
                    eventData.states.Add(stateInfo.Name);  // Store the State name in the ScriptableObject
                }
            }
        }
    }

    private void GetTrigger(AkWwiseProjectData.Event eventInfo, WwiseEventData.EventData eventData)
    {
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

        // Fetch associated Triggers
        foreach (var triggerWwu in wwiseProjectData.TriggerWwu)
        {
            foreach (var triggerInfo in triggerWwu.List)
            {
                if (triggerInfo.Id == eventInfo.Id)
                {
                    // Instead of using Id, use the Trigger name
                    eventData.triggers.Add(triggerInfo.Name);  // Store the Trigger name in the ScriptableObject
                }
            }
        }
    }


}

