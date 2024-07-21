using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    public class DSGroup : Group
    {
        public string ID { get; set; }
        private Color defaultBackgroundColor;
        private float defaultBorderWidth;
        public string OldTitle{ get; set; }
        public DSGroup(string groupTitle, Vector2 position)
        {
            defaultBackgroundColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;

            ID = Guid.NewGuid().ToString();
            title = groupTitle;
            OldTitle = groupTitle;
            SetPosition(new Rect(position,Vector2.zero));
        }
        
        #region Style
        public void SetErrorStyle(Color color)
        {
            contentContainer.style.backgroundColor = color;
            contentContainer.style.borderBottomWidth = 2.0f;
        }
        public void ResetStyle()
        {
            contentContainer.style.backgroundColor = defaultBackgroundColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }
        #endregion
    }
}
