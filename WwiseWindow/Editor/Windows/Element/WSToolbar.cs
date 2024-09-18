using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WS.Library;
using WS.StyleUtilities;
using WS.Utilities;


namespace WS.Window
{
    using WS.WSToolbar;
    public class WSToolbar : Toolbar
    {
        private string selectedKey;

        WSLibrary Library =  WSLibrary.Core;
        private GameObject selectedObject;
        // {
        //     get { return WwiseSoundcasterWindow.SoundingBody; }
        //     set
        //     {
        //         WwiseSoundcasterWindow.SoundingBody = value;
        //     }
        // }


        private WSGraphView graphView;
        private Button saveButton;
        private Button miniMapButton;
        
        
        private Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        private PopupField<string> keyDropdown;
        private TextField newStringField;
        private TextField fileNameTextField;
        
        
        #region Toolbar


        public WSToolbar(WSGraphView graphView)
        {
            this.graphView = graphView;
            CreatePanel();

            // CreateButton();
            AddGameObject();
            
            AddStyles();
            graphView.Add(this);
          
        }

        #endregion

        #region InitPanel

        public void CreatePanel()
        {
            // 添加字典键的下拉菜单
            AddKeyDropdown();
            AddNewPanelTextField();
            AddKeyControlButtons(keyDropdown);
        }
        
        private PopupField<string> AddKeyDropdown()
        {
            // 动态生成字典Key的下拉菜单
            List<string> keyOptions = new List<string>(dict.Keys); // 获取字典中的所有键
            if (keyOptions.Count == 0) keyOptions.Add(" 没有界面 "); // 当字典为空时显示默认值
    
            keyDropdown = new PopupField<string>("当前界面：", keyOptions, 0);
            keyDropdown.RegisterValueChangedCallback(evt =>
            {
                selectedKey = evt.newValue; // 保存用户选择的键
            });
            
            keyDropdown.labelElement.AddToClassList("labelMinWidth");
            
            Add(keyDropdown);
        
            return keyDropdown;
        }
        
        private void  AddNewPanelTextField()
        {
            // 新建Key的文本输入框
            newStringField = new TextField("新界面:");
          
            newStringField.labelElement.AddToClassList("labelMinWidth");
            Add(newStringField);
        }
        
        private void AddKeyControlButtons(PopupField<string> keyDropdown)
        {
            // 添加键按钮
            Button addButton = new Button(() =>
            {
                if (!string.IsNullOrEmpty(newStringField.value) && !dict.ContainsKey(newStringField.value))
                {
                    dict.Add(newStringField.value, new List<string>());
                    RefreshPanelKeys(keyDropdown); // 更新下拉菜单选项
                }
            })
            {
                text = "添加界面"
            };
    
            // 删除键按钮
            Button removeButton = new Button(() =>
            {
                if (dict.ContainsKey(selectedKey))
                {
                    dict.Remove(selectedKey);
                    RefreshPanelKeys(keyDropdown); // 更新下拉菜单选项
                }
            })
            {
                text = "移除界面"
            };
            
            Button StopButton = new Button(() =>
            {
                AkSoundEngine.StopAll();
            })
            {
                text = "Stop All"
            };
    
            Add(StopButton);
            Add(addButton);
            Add(removeButton);
        }
        
        private void RefreshPanelKeys(PopupField<string> keyDropdown)
        {
            // 更新下拉菜单中的选项
            keyDropdown.choices = new List<string>(dict.Keys);

            if (keyDropdown.choices.Count == 0)
            {
                keyDropdown.choices.Add("No Keys"); // 如果字典为空则显示默认选项
                selectedKey = "No Keys";
            }
            else
            {
                selectedKey = keyDropdown.choices[0]; // 默认选择第一个键
            }

            keyDropdown.value = selectedKey; // 更新下拉菜单中的当前选中项
        }
        #endregion


        #region GameObject

        public void AddGameObject()
        {
            var objectField = new ObjectField
            {
                objectType = typeof(GameObject),
                value = Library.Singer
            };
            objectField.RegisterValueChangedCallback(evt =>
            {
                var choiceGameObject = evt.newValue as GameObject;
                
                if ( choiceGameObject == null)
                {
                    Library.ChoiceSinger = false;
                    objectField.value = Library.AKLister;
                }
                else
                {
                    Library.ChoiceSinger = true;
                    Library.CustomSinger = choiceGameObject;
                }
                Debug.Log(Library.Singer.name);
            });
            
            Add(objectField);
        }

        #endregion
        
        
        #region Button

        public void CreateButton()
        {
            // fileNameTextField = WSElementUtility.CreateTextField(defaultFileName, "File Name:",
            //     callback =>
            //     {
            //         fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            //     });

            saveButton = WSElementUtility.CreateButton("Save", () => Save());

            Button loadButton = WSElementUtility.CreateButton("Load", () => Load());
            Button clearButton = WSElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = WSElementUtility.CreateButton("Reset", () => ResetGraph());
            miniMapButton = WSElementUtility.CreateButton("Minimap", () => ToggleMiniMap());



            // Add to Toolbar
            // Add(fileNameTextField);
            // Add(saveButton);
            // Add(loadButton);
            // Add(clearButton);
            // Add(resetButton);
            // Add(miniMapButton);
        }

        #endregion

        
        
        private void AddStyles()
        {
            this.AddStyleSheets("WwiseSystem/WSToolbarStyles");
        }



        #region Button Event

    
        
        private void Save()
        {
            // Save logic
        }

        private void Load()
        {
            // Load logic
        }

        private void Clear()
        {
            // Clear logic
        }

        private void ResetGraph()
        {
            // Reset graph logic
        }

        private void ToggleMiniMap()
        {
            // Toggle minimap logic
        }
            

        #endregion

    }
}