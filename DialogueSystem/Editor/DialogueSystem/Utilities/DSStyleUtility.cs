using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Utilities
{
    public static class DSStyleUtility
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
                //Asset -> need .uss
                // StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load(styleSheetName);
                //Resources -> not need .uss
                StyleSheet styleSheet =  Resources.Load<StyleSheet>(styleSheetName);
                
                if(styleSheet != null)
                    Debug.Log(" 加载成功 " + styleSheetName);
                else
                    Debug.Log(" 加载失败 " + styleSheetName);
                
                Debug.Log(element);
                element.styleSheets.Add(styleSheet);
            }
          
            return element;
        }
    }
}