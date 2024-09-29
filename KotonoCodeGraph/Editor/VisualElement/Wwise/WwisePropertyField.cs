using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    public class WwisePropertyField : VisualElement, IBindable
    {
        private PropertyField propertyField;

        public WwisePropertyField(SerializedProperty property)
        {
            propertyField = new PropertyField(property);
            Add(propertyField);

            // 在这里添加额外的 UI 元素或者修改 propertyField 的样式
            // 修改 propertyField 的背景颜色
            propertyField.style.backgroundColor = new Color(1, 0, 0, 1); // 红色
            propertyField.style.width = 200;
            
            this.bindingPath = property.propertyPath;
        }

        public IBinding binding { get; set; }
        public string bindingPath { get; set; }
    }
  
}
