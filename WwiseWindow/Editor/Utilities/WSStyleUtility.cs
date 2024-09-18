using UnityEngine;
using UnityEngine.UIElements;

namespace WS.StyleUtilities
{
    public static class WSStyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                StyleSheet styleSheet =  Resources.Load<StyleSheet>(styleSheetName);
             
                element.styleSheets.Add(styleSheet);
            }
          
            return element;
        }
    }
}