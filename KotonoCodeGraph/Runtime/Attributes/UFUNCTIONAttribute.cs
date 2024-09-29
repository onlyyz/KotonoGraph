using System;

namespace Kotono.Code
{
    public class UFUNCTIONAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;
        private bool m_hasFlowInput;
        private bool m_hasFlowOutput;
        public string title =>this.m_nodeTitle;
        public string menuItem =>this.m_menuItem;
        public bool hasFlowInput => this.m_hasFlowInput;
        public bool hasFlowOutput => this.m_hasFlowOutput;

        public UFUNCTIONAttribute(string title, string menuItem = " ",bool hasFlowInput = true , bool hasFlowOutput = true)
        {
            this.m_nodeTitle = title;
            this.m_menuItem = menuItem;
            this.m_hasFlowInput = hasFlowInput;
            this.m_hasFlowOutput = hasFlowOutput;
        }
    }
}
