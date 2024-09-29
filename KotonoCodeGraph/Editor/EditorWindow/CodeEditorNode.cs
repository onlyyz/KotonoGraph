using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace Kotono.Code.Editor
{
    public class CodeEditorNode : Node
    {
        private CodeGraphNode m_node;
        public CodeGraphNode Node => m_node;
       
        private Port m_outputPort;
        private Port m_inputPort;
        private List<Port> m_Ports;
       
        private SerializedObject m_serializedObject;
        private SerializedProperty m_serializedProperty;
        
        public List<Port> Ports => m_Ports;
        public Port InputPort => m_inputPort;
        
        //
        public static CodeIcon codeIcons => CodeLibrary.Core.Icons;
        
        private Pill inputPortPill;
        private Pill outputPortPill;
        
        public CodeEditorNode(CodeGraphNode node,SerializedObject codeGraphObject)
        {
            this.AddToClassList("code-graph-code");
            
            m_node = node;
            m_serializedObject = codeGraphObject;
            
            Type typeInfo = node.GetType();
            UCLASSAttribute info = typeInfo.GetCustomAttribute<UCLASSAttribute>();
            
            title = info.title;
            m_Ports = new List<Port>();
            
            
            string[] depths = info.menuItem.Split('/');
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(' ','-'));
            }
            
            name = typeInfo.Name;

            if (info.hasFlowInput)
            {
                CreateFlowInputPort();
            }

            if (info.hasFlowOutput)
            {
                CreateFlowOutputPort();
            }

            DrawProperty(typeInfo);
            
           
            
            RefreshExpandedState();
            RefreshPorts();
            
            this.AddStyleSheets("Styles/CodeEditorNode");
        }


        
        public void SvaPosition()
        {
            m_node.SetPosition(GetPosition());
        }

        #region Port

        private void CreateFlowInputPort()
        {
            m_inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(PortTypes.FlowPort));
            m_inputPort.portColor = Color.blue; // 设置输入端口颜色
            m_inputPort.portName = "Input";
            m_inputPort.tooltip = "Flow input Port";
            
            // m_Ports.Add(m_inputPort);
            inputContainer.Add(m_inputPort);
           
            // 直接设置自定义图标
            // inputContainer.style.backgroundImage = new StyleBackground(codeIcons.GetIcon()); // 替换为你的图标路径
            // inputContainer.style.width = 20;  // 设置图标的宽度
            // inputContainer.style.height = 20; // 设置图标的高度
            // inputContainer.style.backgroundColor = Color.clear;


            // SetPortPill(true);
        }
        
        private void CreateFlowOutputPort()
        {
            m_outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(PortTypes.FlowPort));
            m_outputPort.portColor = Color.red; // 设置输出端口颜色
            m_outputPort.portName = "Out";
            m_outputPort.tooltip = "Flow out Port";
            
            m_Ports.Add(m_outputPort);
            outputContainer.Add(m_outputPort);
         
            // m_outputPort.style.backgroundImage = new StyleBackground(codeIcons.GetIcon()); // 替换为你的图标路径
            // m_outputPort.style.width = 20;  // 设置图标的宽度
            // m_outputPort.style.height = 20; // 设置图标的高度
            // m_outputPort.style.backgroundColor = Color.clear;
            
            
            // SetPortPill(false);
        }

        #endregion

        #region Properties


        private void DrawProperty(Type typeInfo)
        {
            foreach (FieldInfo property in typeInfo.GetFields())
            {
                // if (property.GetCustomAttribute<UPROPERTYAttribute>() is UPROPERTYAttribute UPROPERTYProperty)
                // {
                //     //抓取参数
                //     // Debug.Log(property.Name);
                //     
                //     DrawProperty(property.Name);
                //     
                //     // PropertyField field =  DrawProperty(property.Name);
                //     // field.RegisterValueChangeCallback(OnFieldValueChangeCallback);
                // }else if (property.GetCustomAttribute<WWISEAttribute>() is WWISEAttribute WWISEProperty)
                // {
                //     DrawProperty(property.Name);
                // }else if (property.GetCustomAttribute<UTITLEAttribute>() is UTITLEAttribute UTITLEProperty)
                // {
                //     // DrawProperty(property.Name);
                // }
                
                // 获取属性类型
                var attributes = property.GetCustomAttributes<Attribute>();

                foreach (var attribute in attributes)
                {
                    switch (attribute)
                    {
                        case UPROPERTYAttribute _:
                            // 处理 UPROPERTYAttribute
                            DrawProperty(property.Name);
                            break;

                        case WWISEAttribute _:
                            // 处理 WWISEAttribute
                            DrawProperty(property.Name);
                            break;

                        case UTITLEAttribute _:
                            // 处理 UTITLEAttribute
                            DrawTitle(property.Name);
                            break;

                        default:
                            // 处理其他情况（如果需要）
                            break;
                    }
                }
            }
        }
        
   
        // 如果还没有序列化的属性对象，先获取它
        private void FetchSerializedProperty()
        {
            // m_serializedProperty.target 是 当前绑定的Graph View
            SerializedProperty nodes = m_serializedObject.FindProperty("m_nodes");
          
            if (nodes.isArray)
            {
                int size = nodes.arraySize;
                for (int i = 0; i < size; i++)
                {
                    var element = nodes.GetArrayElementAtIndex(i);
                    var elementId = element.FindPropertyRelative("m_guid");
                    if (elementId.stringValue == m_node.id)
                    {
                        m_serializedProperty = element;
                    }
                }
            }
        }
        
        private PropertyField DrawProperty(string propertyName)
        {
            if (m_serializedProperty == null)
            {
                // 如果还没有序列化的属性对象，先获取它
                FetchSerializedProperty(); 
            }
            // m_serializedProperty.target 是 当前绑定的Node
            SerializedProperty prop = m_serializedProperty.FindPropertyRelative(propertyName);
          
            PropertyField field = new PropertyField(prop);
           
            // 设置绑定路径 同步修改
            field.bindingPath = prop.propertyPath; 
            extensionContainer.Add(field); // 添加字段到UI容器
            
            
            //设置USS
            field.name = "bindingPath";
            
            return field;
        }

        private void DrawWwiseProperty(string propertyName)
        {
        }

        private void DrawTitle(string propertyName)
        {
            if (m_serializedProperty == null)
            {
                // 如果还没有序列化的属性对象，先获取它
                FetchSerializedProperty(); 
            }
            // m_serializedProperty.target 是 当前绑定的Node
            SerializedProperty prop = m_serializedProperty.FindPropertyRelative(propertyName);
            PropertyField field = new PropertyField(prop);
            field.bindingPath = prop.propertyPath; 
            this.title = prop.stringValue;
            
        }
        #endregion

        #region CallBack
        
        //Edge
        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            return Port.Create<ColoredEdge>(orientation, direction, capacity, type);
        }
        
        #endregion
        
        #region Style
        

        public void SetPortPill( bool input = true)
        {
            if (input)
            {
                inputPortPill = new Pill();
                inputPortPill.icon = codeIcons.GetIcon();
                m_inputPort.Add(inputPortPill);
            }
            else
            {
                outputPortPill = new Pill();
                outputPortPill.icon = codeIcons.GetIcon();
                m_outputPort.Add(outputPortPill);
            }
        }
        
        #endregion
    }
}