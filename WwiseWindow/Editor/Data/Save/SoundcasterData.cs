using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "ScriptableObjects/DataContainer", order = 1)]
public class SoundcasterData : ScriptableObject
{
    public Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
}