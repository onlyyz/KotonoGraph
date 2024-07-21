using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "Dialogue File";
        private TextField fileNameTextField;
        private Button saveButton;
        [MenuItem("Tool/Kotono Graph")]
        public static void ShowExample()
        {
            GetWindow<DSEditorWindow>("Kotono Dialogue Graph");
        }


        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            
            AddStyles();
        }




        #region Elements Addtion

        private void AddGraphView()
        {
            DSGraphView GraphView = new DSGraphView(this);
            GraphView.StretchToParentSize();
            rootVisualElement.Add(GraphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            //the TextField include the Call Back Function
            fileNameTextField = DSElementUtility.CreateTextField(defaultFileName,"File Name:",
                callback =>
                {
                    fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                });
          
            
            //fileNameTextField.style.flexDirection = FlexDirection.Column;
            saveButton = DSElementUtility.CreateButton("Save");
            
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            
            
            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles");
            
            rootVisualElement.Add(toolbar);
        }
        
        #endregion

        
        
        
        #region Styles
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables");
        }
        #endregion

        #region Utility Methods

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }
        
        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }

        #endregion
    }
}