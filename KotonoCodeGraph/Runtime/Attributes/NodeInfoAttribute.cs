

using System;

namespace Kotono.Code
{
    public class NodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;
    
        public string title =>this.m_nodeTitle;
        public string menuItem =>this.m_menuItem;

        public NodeInfoAttribute(string title, string menuItem = " ")
        {
            this.m_nodeTitle = title;
            this.m_menuItem = menuItem;
        }
    }
}
