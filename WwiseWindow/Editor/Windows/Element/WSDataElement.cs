using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace WS.DataElement
{
    
    using Library;
    using Window;
    
    public static class WSDataElement
    {
        #region Main
       
        public static GameObject Singer => WSLibrary.Core.Singer;
        public static WwiseIcons wwiseIcons => WSLibrary.Core.Icons;
        public static void CrateStatesElement(this VisualElement root, string group, List<string> states,string selection)
        {
            VisualElement mianContainer = CreateHorizontalContainer();
            VisualElement Enum = CreateEnum(group,states, selection);
            
            CreateIconName(mianContainer, group,WwiseObjectType.StateGroup);
            
            mianContainer.Add(Enum);
            // root.AddRemove(mianContainer);
            root.Add(mianContainer);
        }

        public static void CrateRTPCElement(this VisualElement root,AkWwiseProjectData.AkInformation info)
        {
            VisualElement mianContainer = CreateHorizontalContainer();

            CreateIconName(mianContainer,  info.Name,WwiseObjectType.WorkUnit);
            
            VisualElement slider = CreateSlider(info.Id);
            
            mianContainer.Add(slider);
            mianContainer.name =  info.Name;
            // root.AddRemove(mianContainer);
            root.Add(mianContainer);
        }
        
        public static void CrateSwitchElement(this VisualElement root,string group, List<string> states,string selection)
        {
            VisualElement mianContainer = CreateHorizontalContainer();
            VisualElement Enum = CreateEnum(group,states, selection,false);
            
            CreateIconName(mianContainer, group,WwiseObjectType.SwitchGroup);
            
            mianContainer.Add(Enum);
            // root.AddRemove(mianContainer);
            root.Add(mianContainer);
        }
        

        public static VisualElement CrateTriggerElement(string name)
        {
            return new VisualElement();
        }

         

        #endregion
        
        public static VisualElement CreateSlider(uint RTPCId)
        {
            VisualElement sliderContainer =CreateHorizontalContainer();
            sliderContainer.ToRight();
            
            
            // Create a text field for the number
            TextField numberField = new TextField()
            {
                value = "50",
            };
            sliderContainer.Add(numberField);

           
            Slider slider = new Slider(0, 100);
            slider.value = 50;
            sliderContainer.Add(slider);

            // Register value changed callbacks
            numberField.RegisterValueChangedCallback( evt =>
            {
                float newValue;
                if (float.TryParse(evt.newValue, out newValue))
                {
                    slider.value = (int)newValue;
                    AkSoundEngine.SetRTPCValue(RTPCId, (int) slider.value, Singer);
                }
            });
            slider.RegisterValueChangedCallback(evt =>
            {
                numberField.value = ((int)evt.newValue).ToString();
                AkSoundEngine.SetRTPCValue(RTPCId, (int)evt.newValue, Singer);
            });

            // Add drag-to-change functionality to the number field
            bool isDragging = false;
            float dragStartValue = 0;
            
            numberField.RegisterCallback<MouseDownEvent>(evt =>
            {
                isDragging = true;
                dragStartValue = (int)slider.value;
                evt.StopPropagation();
            });
            numberField.RegisterCallback<MouseUpEvent>(evt =>
            {
                isDragging = false;
                evt.StopPropagation();
            });
            numberField.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (isDragging)
                {
                    float newValue = dragStartValue + evt.mouseDelta.x;
                    newValue = Mathf.Clamp(newValue, slider.lowValue, slider.highValue);
                    slider.value = (int)newValue;
                    numberField.value = ((int)newValue).ToString();
                }
            });
            
            return sliderContainer;
        }
        
        public static VisualElement CreateEnum(string group,List<string> enumString,string selection,bool state = true)
        {
            VisualElement Enum = new VisualElement();
            Enum.ToRight();
            
            PopupField<string> PopupField = new PopupField<string>(enumString,selection);
            
            
            PopupField.RegisterValueChangedCallback(( evt) =>
            {
                
                if (state)
                {
                    Debug.Log( group +  " state：" + evt.newValue);
                    AkSoundEngine.SetState(group, evt.newValue.ToString());
                }
                else
                {
                    Debug.Log( group +  " Switch：" + evt.newValue);
                    AkSoundEngine.SetSwitch(group, evt.newValue.ToString(), Singer);
                }
            });
            
            
            Enum.Add(PopupField);
            
            return Enum;
        }
        
        
        #region Element

        public static void ToLeft(this VisualElement root)
        {
            root.style.position = Position.Absolute;
            root.style.left = 0;
        }
        public static void ToRight(this VisualElement root)
        {
            root.style.position = Position.Absolute;
            root.style.right = 0;
        }
        public static void CreateIconName(this VisualElement root,string name,WwiseObjectType type)
        {
            VisualElement iconName = CreateHorizontalContainer("IconName");
            Image iconImage = new Image();
            iconImage.image = wwiseIcons.GetIcon(type);
            
            // Create a label for the icon and name
            Label label = new Label()
            {
                text =  name, // Replace <icon> with the actual icon
            };
            
            iconName.Add(iconImage);
            iconName.Add(label);
            root.Add(iconName);
        }

        public static void AddRemove(this VisualElement root,VisualElement main)
        {
            // Create a label for the icon and name
            Image iconImage = new Image();
            Button button = new Button(() =>
            {
                root.Remove(main);
            })
            {
                text = "x",
               
            };
         
            button.style.alignSelf = Align.FlexEnd;
            
            
            main.Add(button);
            root.Add(main);
        }
        
        
        #endregion
        
        
        #region Visual Element 
        
        public static VisualElement CreateHorizontalContainer(string name = "")
        {
            // 创建水平容器
            return new VisualElement
            {
                name = name,
                style =
                {
                    height = 30,
                    flexDirection = FlexDirection.Row, 
                    paddingLeft = 2,
                    paddingRight = 2
                }
            };
        }

        public static VisualElement CreateVerticalContainer(string name = "")
        {
            return new VisualElement
            {
                name = name,
                style = {
                    height = 30,
                    flexDirection = FlexDirection.Column,
                    paddingLeft = 2,
                    paddingRight = 2
                }
            };
        }
        

        
        
        
        #endregion
    }
}