using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RTPCFinder : MonoBehaviour
{
    // 查询 Event 与哪些 RTPC 相关联
    [Button]
    static List<string> FindRTPCsInEvent(uint eventId)
    {
        // 获取 Wwise 项目数据
        AkWwiseProjectData wwiseProjectData = AkWwiseProjectInfo.GetData();

        // 使用 LINQ 查找与该 EventId 相关的 RTPC 并提取其名称
        return wwiseProjectData.RtpcWwu
            .SelectMany(rtpcWorkUnit => rtpcWorkUnit.List) // 展开 RtpcWwu 的 RTPC 列表
            .Where(rtpc => rtpc.Id == eventId) // 筛选与 EventId 相关的 RTPC
            .Select(rtpc => rtpc.Name) // 选择 RTPC 名称
            .ToList();
    }

    void Start()
    {
        // 假设你想查询 eventId 为 1234 的 RTPC 关联
        uint eventId = 1234;
        List<string> rtpcNames = FindRTPCsInEvent(eventId);

        // 输出 RTPC 名称
        foreach (var rtpcName in rtpcNames)
        {
            Debug.Log("RTPC Name: " + rtpcName);
        }
    }
}

