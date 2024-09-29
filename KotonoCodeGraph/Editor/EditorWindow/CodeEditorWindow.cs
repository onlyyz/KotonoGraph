using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        
        //启动判断 不用加载操作
        private void OnEnable()
        {
            if (m_currentGraph != null)
            {
                DrawGraph();
            }
        }
        
        public void Load(CodeGraphAsset target)
        {
            m_currentGraph=  target;
            DrawGraph();
        }

        
       
        public void DrawGraph()
        {
            
            CodeLibrary.Core.InstanceCodeLibrary();
            
            m_serializedGraphAsset = new SerializedObject(m_currentGraph);
            m_currentGraphView = new CodeGraphView(m_serializedGraphAsset,this);
            
            //脏标记
            m_currentGraphView.graphViewChanged += OnGraphViweChanged;
            rootVisualElement.Add(m_currentGraphView);
        }

        private void OnGUI()
        {
            if (m_currentGraph != null)
            {
                if( EditorUtility.IsDirty(m_currentGraph))
                {
                    this.hasUnsavedChanges = true;
                }
                else
                {
                    this.hasUnsavedChanges = false;
                }
                
            }
        }
        
        private GraphViewChange OnGraphViweChanged(GraphViewChange graphviewchange)
        {
            EditorUtility.SetDirty(m_currentGraph);
            return graphviewchange;
        }
 

        #endregion
    }
}