using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace WS.Window
{

    using Extend;
    using StyleUtilities;

    public class WSWindow : EditorWindow
    {
        WSGraphView graphView;
        
        [MenuItem("Tools/Wwise Window")]
        public static void ShowWindow()
        {
            var window = GetWindow<WSWindow>("Kotono Wwise Window");
            window.minSize = new Vector2(800, 600);
        }

        #region UnityEvent

        private void OnEnable()
        {
            rootVisualElement.Clear();
            
            InitGraphView();
            ExtendWwisePicker();
            
            AddStyles();
          

        }
        
        #endregion

        #region Init

        
        
        private void InitGraphView()
        {
            graphView = new WSGraphView(this)
            {
                name = "Wwise Graph View",
                style =
                {
                    flexGrow = 1
                }
            };
            rootVisualElement.Add(graphView);
        }

        #endregion

        #region delegate

        private void ExtendWwisePicker()
        {
            WwiseExtend.DataEvent += OnDataChanged;
        }
        private void OnDataChanged(object Handle, WwiseObjectReference reference,Dictionary<System.Type, WwiseObjectType> ScriptTypeMap)
        {
            graphView.DragEvent(reference,ScriptTypeMap);
        }
        
        #endregion
        
        #region Styles
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("WwiseSystem/WSVariables");
        }
        #endregion
        
    }
    
    
    
}