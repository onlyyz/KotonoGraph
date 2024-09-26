using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;


namespace Kotono.Code.Editor
{
    public class CodeEditorNode : Node
    {
       private CodeGraphNode m_node;
        public CodeEditorNode(CodeGraphNode node)
        {
            this.AddToClassList("code-graph-code");
            
            m_node = node;
            Type typeInfo = node.GetType();
            NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();
           
            title = info.title;

            string[] depths = info.menuItem.Split('/');
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(' ','-'));
            }
            
            name = typeInfo.Name;
            
            this.AddToClassList("code-graph-code");
        }
    }
}
