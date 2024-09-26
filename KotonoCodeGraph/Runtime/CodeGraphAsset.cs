using System.Collections.Generic;
using Kotono.PCG;
using UnityEngine;

namespace Kotono.Code
{
    [CreateAssetMenu(menuName = "Kotono/Code Graph Asset")]
    public class CodeGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> m_nodes;
        public List<CodeGraphNode> Nodes => m_nodes;

        public CodeGraphAsset()
        {
            m_nodes = new List<CodeGraphNode>();
        }
    }
}