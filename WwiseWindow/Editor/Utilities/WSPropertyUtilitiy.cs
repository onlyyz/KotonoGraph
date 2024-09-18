using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using WS.DataElement;
using WS.Selection;

namespace WS.Utilitiy 
{
   
    using DataClass;
    
    public enum propertyType
    {
        State = 1,
        Switch = 2,
        RTPC = 3,
        Trigger = 4,
    }
    public static class WSPropertyUtilitiy 
    {
        public static void AddProperty(this WSSelectionSection Selection,WwiseObjectReference reference, Dictionary<System.Type, WwiseObjectType> ScriptTypeMap)
        {
            switch (reference.WwiseObjectType)
            {
                case WwiseObjectType.Event:
               
                    WwiseEventReference eventReference = CastToObjectReference<WwiseEventReference>(reference);
                    Selection.RTPCsScrollView.AddRTPCs(
                        FindRTPCsInEvent(eventReference.Id));
                    break;
                
                
                
                case WwiseObjectType.State:
               
                    WwiseStateReference stateReference = CastToObjectReference<WwiseStateReference>(reference);
                    Selection.statesScrollView.AddStates(stateReference.GroupObjectReference.ObjectName,
                        FindStatesInGroup(stateReference.GroupObjectReference.Id),stateReference.ObjectName);
                 
                    break;
                case WwiseObjectType.Switch:
                    WwiseSwitchReference switchReference = CastToObjectReference<WwiseSwitchReference>(reference);
                    
                    DebugLog(switchReference.GroupObjectReference.ObjectName,
                        switchReference.ObjectName);
                    Selection.SwitchsScrollView.AddSwitches(switchReference.GroupObjectReference.ObjectName,
                        FindSwitchsInGroup(switchReference.GroupObjectReference.Id),switchReference.ObjectName);
                    break;
                case WwiseObjectType.Bus:
                    // scrollView.AddRTPCs(data);
                    break;
                case WwiseObjectType.Trigger:
                    // scrollView.AddTriggers(data);
                    break;
            }
         
            // AkSoundEngine.SetState(stateunit);
       
            // Selection.statesScrollView.CheckListNull(data.states,propertyType.State);
            // Selection.SwitchsScrollView.CheckListNull(data.states,propertyType.Switch);
            // Selection.RTPCsScrollView.CheckListNull(data.states,propertyType.RTPC);
            // Selection.TriggersScrollView.CheckListNull(data.states,propertyType.Trigger);

            // Selection.statesScrollView.Add(WSDataElement.CrateSwitchElement("state"));
            // Selection.SwitchsScrollView.Add(WSDataElement.CrateSwitchElement("state"));
            // Selection.RTPCsScrollView.Add(WSDataElement.CrateSwitchElement("state"));
            // Selection.TriggersScrollView.Add(WSDataElement.CrateSwitchElement("state"));



            #region MyRegion

            // WwiseStateReference Statereference = CastToObjectReference<WwiseStateReference>(reference);
            //
            //
            // Debug.Log("guid ： " + Statereference.ObjectName + "  " + Statereference.GroupObjectReference.ObjectName);
            //
            // Debug.Log("guid ： " + Statereference.Id + "  " + Statereference.GroupObjectReference.Id);
            //
            //
            // Selection.statesScrollView.AddStates(Statereference.GroupObjectReference.ObjectName,
            //     FindStatesInGroup(Statereference.GroupObjectReference.Id),Statereference.ObjectName);

            #endregion
        }





        public static T CastToObjectReference<T>(WwiseObjectReference reference) where T : WwiseObjectReference
        {
            if (reference is T stateReference)
            {
                return stateReference;
            }

            return null;
        }
        
        
        // public static void CheckListNull(this ScrollView scrollView,List<string> data,propertyType type)
        // {
        //     if (data.Count == 0)
        //     {
        //         return;
        //     }
        //     else
        //     {
        //         Debug.Log("长度 ： " + data.Count);
        //         scrollView.AddStates(data);
        //         switch (type)
        //         {
        //             case propertyType.State:
        //                 scrollView.AddStates(data);
        //                 break;
        //             case propertyType.Switch:
        //                 scrollView.AddSwitches(data);
        //                 break;
        //             case propertyType.RTPC:
        //                 scrollView.AddRTPCs(data);
        //                 break;
        //             case propertyType.Trigger:
        //                 scrollView.AddTriggers(data);
        //                 break;
        //         }
        //     }
        // }
        
        public static void AddStates(this ScrollView scrollView,string name, List<string> states,string selection)
        {
            scrollView.CrateStatesElement(name,states, selection);
        }
        public static void AddSwitches(this ScrollView scrollView,string name, List<string> switchs,string selection)
        {
            scrollView.CrateSwitchElement(name,switchs, selection);
        }
        public static void AddRTPCs(this ScrollView scrollView,List<AkWwiseProjectData.AkInformation> rtpcs)
        {
            foreach (var rtpc in rtpcs)
            {
                scrollView.CrateRTPCElement(rtpc);
            }
        }
        // public static void AddTriggers(this ScrollView scrollView,List<string> triggers)
        // {
        //     triggers.ForEach(state => scrollView.Add(WSDataElement.CrateSwitchElement(state)));
        // }


        #region GetData


        static List<string> FindStatesInGroup(uint groupId)
        {
            // 获取 Wwise 项目数据
            AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

            // 使用 LINQ 查找符合条件的 GroupValue 并提取其 values 中的 Name
            return wwiseProjectData.StateWwu
                .SelectMany(groupValWorkUnit => groupValWorkUnit.List)
                .Where(groupValue => groupValue.Id == groupId)
                .SelectMany(groupValue => groupValue.values)
                .Select(value => value.Name)
                .ToList();
        }
        
        static List<string> FindSwitchsInGroup(uint groupId)
        {
            // 获取 Wwise 项目数据
            AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

            // 使用 LINQ 查找符合条件的 GroupValue 并提取其 values 中的 Name
            return wwiseProjectData.SwitchWwu
                .SelectMany(groupValWorkUnit => groupValWorkUnit.List)
                .Where(groupValue => groupValue.Id == groupId)
                .SelectMany(groupValue => groupValue.values)
                .Select(value => value.Name)
                .ToList();
        }
        
        static List<AkWwiseProjectData.AkInformation> FindRTPCsInEvent(uint eventId)
        {
            // 获取 Wwise 项目数据
            AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

            // 使用 LINQ 查找与该 EventId 相关的 RTPC 并提取其名称
            return wwiseProjectData.RtpcWwu
                .SelectMany(rtpcWorkUnit => rtpcWorkUnit.List) // 展开 RtpcWwu 的 RTPC 列表
                .Where(rtpc => rtpc.Id == eventId) // 筛选与 EventId 相关的 RTPC
                // .Select(rtpc => rtpc.Name) // 选择 RTPC 名称
                .ToList();
        }

        public static void DebugLog(params string[] strings)
        {
            Debug.Log(string.Join(" ", strings));
        }
        #endregion
        
        
    }
}