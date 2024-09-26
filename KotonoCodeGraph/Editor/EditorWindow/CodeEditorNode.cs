using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;


namespace Kotono.Code.Editor
{
    public class CodeEditorNode : Node
    {
        private CodeGraphNode m_node;
        public CodeGraphNode Node => m_node;
       
        private Port m_outputPort;
        private List<Port> m_Ports;
       
        private SerializedObject m_serializedObject;
        private SerializedProperty m_serializedProperty;
        public List<Port> Ports => m_Ports;
       
        public CodeEditorNode(CodeGraphNode node,SerializedObject codeGraphObject)
        {
            this.AddToClassList("code-graph-code");
            
            m_node = node;
            m_serializedObject = codeGraphObject;
            
            Type typeInfo = node.GetType();
            NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();
           
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

            foreach (FieldInfo property in typeInfo.GetFields())
            {
                if (property.GetCustomAttribute<ExposedPropertyAttribute>() is ExposedPropertyAttribute exposedProperty)
                {
                    //抓取参数
                    // Debug.Log(property.Name);
                    PropertyField field =  DrawProperty(property.Name);
                    
                    // field.RegisterValueChangeCallback(OnFieldValueChangeCallback);
                }
            }
            
            RefreshExpandedState();
            RefreshPorts();
            
            this.AddToClassList("code-graph-code");
        }

        // 如果还没有序列化的属性对象，先获取它
        private void FetchSerializedProperty()
        {
            // m_serializedProperty.target 是 当前绑定的Graph View
            SerializedProperty nodes = m_serializedObject.FindProperty("m_nodes");
            if (nodes == null)
            {
                Debug.Log("No Nodes found");
            }
            
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
            return field;
        }

        
        
   

        
        private void OnFieldValueChangeCallback(SerializedPropertyChangeEvent evt)
        {
            throw new NotImplementedException();
        }

        #region Port

        private void CreateFlowInputPort()
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(PortTypes.FlowPort));
            inputPort.portName = "Input";
            inputPort.tooltip = "Flow input Port";
            m_Ports.Add(inputPort);
            inputContainer.Add(inputPort);
        }
        
        private void CreateFlowOutputPort()
        {
            m_outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(PortTypes.FlowPort));
            m_outputPort.portName = "Out";
            m_outputPort.tooltip = "Flow out Port";
            m_Ports.Add(m_outputPort);
            outputContainer.Add(m_outputPort);
        }

        #endregion

        public void SvaPosition()
        {
            m_node.SetPosition(GetPosition());
        }
    }
}