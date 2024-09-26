using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Kotono.Code
{
    [Serializable]
    public class CodeGraphNode 
    {
      [SerializeField]
      public string m_guid;
      [SerializeField]
      public Rect m_Position;

      public string typeName;

      public string id => m_guid;
      public Rect position => m_Position;

      public CodeGraphNode()
      {
          NewGUID();
      }

      public void NewGUID()
      {
          m_guid = System.Guid.NewGuid().ToString();
          
      }

      public void SetPosition(Rect position)
      {
          m_Position = position;
      }
    }
}