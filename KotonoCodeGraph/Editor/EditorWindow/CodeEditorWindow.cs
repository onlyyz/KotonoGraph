using UnityEditor;
using UnityEngine;

namespace Kotono.Code.Editor
{
    
    public class CodeEditorWindow : EditorWindow
    {
        public static void Open(CodeGraphAsset target)
        {
            CodeEditorWindow[] windows = Resources.FindObjectsOfTypeAll<CodeEditorWindow>();
            foreach (var window in windows)
            {
                if (window.m_currentGraph == target)
                {
                    window.Focus();
                    return;
                }
            }
            
            CodeEditorWindow codeWindow = CreateWindow<CodeEditorWindow>(typeof(CodeEditorWindow),typeof(SceneView));
            codeWindow.titleContent = new GUIContent($"{target.name}",EditorGUIUtility.ObjectContent(null,typeof(CodeEditorWindow)).image);
            codeWindow.Load(target);
        }

        [SerializeField]
        private CodeGraphAsset m_currentGraph;
        [SerializeField]
        private SerializedObject m_serializedGraphAsset;
        [SerializeField]
        private CodeGraphView m_currentGraphView;

        #region Load

        public void Load(CodeGraphAsset target)
        {
            m_currentGraph=  target;
            DrawGraph();
        }

        public void DrawGraph()
        {
            m_serializedGraphAsset = new SerializedObject(m_currentGraph);
            m_currentGraphView = new CodeGraphView(m_serializedGraphAsset,this);
            rootVisualElement.Add(m_currentGraphView);
        }
        #endregion
    }
}
