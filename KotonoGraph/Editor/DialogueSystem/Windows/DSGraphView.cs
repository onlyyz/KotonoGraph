using System;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Data.Error;
    using Elements;
    using Enumerations;
    using Utilities;
    using Data.Save;

    public class DSGraphView : GraphView
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;

        private SerializableDictionary<String, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<String, DSGroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<String, DSNodeErrorData>> groupNodes;

        private int repeatedNamesAmount;

        public int RepeatedNamesAmount
        {
            get
            {
                return repeatedNamesAmount;
            }
            set
            {
                repeatedNamesAmount = value;

                if (repeatedNamesAmount == 0)
                {
                    //Enable Save Button
                    editorWindow.EnableSaving();
                }

                if (repeatedNamesAmount == 1)
                {
                    //Disable Save Button
                    editorWindow.DisableSaving();
                }
            }
        }
        
        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            editorWindow = dsEditorWindow;
            ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
            groups = new SerializableDictionary<string, DSGroupErrorData>();
            groupNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();
            
            
            
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            //Call back
            OnElementsDeleted(); 
            OnGroupElementAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();
            
            AddStyles();
            
        }

        #region Overrided Methods
   
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort ==port)
                {
                    return; 
                }
                if (startPort.node ==port.node)
                {
                    return; 
                }
                if (startPort.direction ==port.direction)
                {
                    return; 
                }
                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        #endregion

        #region Styles
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0,gridBackground );
        }
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles",
                "DialogueSystem/DSNodeStyles"
                );
        }

        #endregion

        #region AddManipulators
        
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
  
           
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContentDragger()); 
            this.AddManipulator(new RectangleSelector());
            
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
            
            this.AddManipulator(CreateGroupContextualMenu());


        }

        private IManipulator CreateNodeContextualMenu(string actionTitle,DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator
            (
                menuEvet => menuEvet.menu.AppendAction(actionTitle, 
                    actionEvent => AddElement(CreateNode(dialogueType,GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );
            return contextlMenuManipulartor;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator(
            menuEvet => menuEvet.menu.AppendAction("Add Group", 
                    actionEvent => CreateGroup("DialogueGroup",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))));
            return contextlMenuManipulartor;
        }
        
        
        #endregion

        #region Element Addition
        
        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }
            //change value name is context
            nodeCreationRequest = context => SearchWindow.Open(new 
                SearchWindowContext(context.screenMousePosition),searchWindow);
        }
        
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);
            
            node.Initialize(this,position);
            node.Draw();

            //Save Data
            AddUngroupedNode(node);
            
            return node;
        }

        public DSGroup CreateGroup(String title, Vector2 position)
        {
            DSGroup group = new DSGroup(title, position);

            AddGroup(group);
            AddElement(group);
            
            foreach (GraphElement element in selection)
            {
                if (!(element is DSNode))
                {
                    continue;
                }

                DSNode node = (DSNode)element;
                group.AddElement(node);
            }
            
            
            return group;
        }
        
        #endregion

        #region Callbacks
        
        //node delete call back function will remove the node at the data list
        private void OnElementsDeleted()
        {
            Type groupType = typeof(DSGroup);
            Type edgeType = typeof(Edge);
            
            deleteSelection = (operationName, askUser) =>
            {
                List<DSGroup> groupsToDelete = new List<DSGroup>();
                List<DSNode> nodesToDelete = new List<DSNode>();
                List<Edge> edgesToDelete = new ListStack<Edge>();
                foreach (GraphElement element in selection)
                {
                    //mode 
                    if (element is DSNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (element.GetType() == edgeType)
                    {
                       edgesToDelete.Add((Edge) element);
                       continue;
                    }

                    if (element.GetType() != groupType)
                    {
                        continue;
                    }

                    DSGroup group = (DSGroup)element;
                    groupsToDelete.Add(group);
                }

                //Remove the Elements
                
                DeleteElements(edgesToDelete);
                
                
                
                
                foreach (var group in groupsToDelete)
                {
                    //delete the node in the group
                    List<DSNode> groupNodes = new List<DSNode>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is DSNode))
                        {
                            continue;
                        }
                        
                        //get delete node 
                        groupNodes.Add((DSNode)groupElement);
                    }
                    //remove the node at the group
                    group.RemoveElements(groupNodes);
                    
                    RemoveGroup(group);
                    RemoveElement(group);
                }
                
                foreach (var node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }
                    RemoveUngroundedNode(node);
                    
                    node.DisconnectAllPortS();
                    RemoveElement(node);
                }
            };
        }
        
        private void OnGroupElementAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode))
                    {
                        continue;
                    }

                    DSGroup nodeGroup = (DSGroup)group;
                    DSNode node = (DSNode) element;
                    
                    RemoveUngroundedNode(node);
                    AddGroupedNode(node,nodeGroup);
                }
            }; 
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode))
                    {
                        continue;
                    }

                    DSNode node = (DSNode) element;
                    
                    //Remove node form group and add to graph view
                    RemoveGroupedNode(node,group);
                    AddUngroupedNode(node);
                    
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DSGroup dsGroup = (DSGroup) group;
                dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                
                RemoveGroup(dsGroup);
                dsGroup.OldTitle =  dsGroup.title;
                AddGroup(dsGroup);
            };
        }

        // the ID for Port
        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DSNode nextNode = (DSNode) edge.input.node;
                        DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge)element;
                        DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                        choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }

        #endregion
        
        #region Repeated Elements

        /// <summary>
        /// Node 
        /// </summary>
       
        public void AddUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            //check same Node   not'
            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                //Colors and the Nodes
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                
                nodeErrorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName,nodeErrorData);
                return;
            }

            
            //set the error color for the same name node 
            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            
            ungroupedNodesList.Add(node);
            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        //if the name same but change the name for node, need remove the node error color in the Graph view
        public void RemoveUngroundedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();
            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            
            ungroupedNodesList.Remove(node);
            node.ResetStyle();
            if (ungroupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;
                ungroupedNodesList[0].ResetStyle();
                return;
            }
            
            //delete the element for the Dictionary
            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }
        
        
        /// <summary>
        /// Group
        /// </summary>
        private void AddGroup(DSGroup group)
        {
            string groupName = group.title.ToLower();
            if (!groups.ContainsKey(groupName))
            {
                DSGroupErrorData groupErrorData = new DSGroupErrorData();
                
                groupErrorData.Groups.Add(group);
                groups.Add(groupName,groupErrorData);
                return;
            }
            
            //error color
            List<DSGroup> groupsList = groups[groupName].Groups;
            
            groupsList.Add(group);
            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if (groupsList.Count ==2)
            {
                ++RepeatedNamesAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        public void AddGroupedNode(DSNode node, DSGroup group) 
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = group;
            //Same name group
            if (!groupNodes.ContainsKey(group))
            {
                groupNodes.Add(group,new SerializableDictionary<string, DSNodeErrorData>());
            }

            if (!groupNodes[group].ContainsKey(nodeName))
            {
                //Colors and the Nodes
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                
                nodeErrorData.Nodes.Add(node);
                groupNodes[group].Add(nodeName,nodeErrorData);
                return;
            }

            
            //set the error color for the same name node 
            List<DSNode> groupedNodeList = groupNodes[group][nodeName].Nodes;
            groupedNodeList.Add(node);
            Color errorColor = groupNodes[group][nodeName].ErrorData.Color;
            
            node.SetErrorStyle(errorColor);

            if (groupedNodeList.Count == 2)
            {
                ++RepeatedNamesAmount;
                groupedNodeList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(DSNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = null;
            List<DSNode> groupedNodesList = groupNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);
            node.ResetStyle();

            if (groupedNodesList.Count ==1)
            {
                --RepeatedNamesAmount;
                groupedNodesList[0].ResetStyle();   
                return;
            }

            if (groupedNodesList.Count ==0)
            {
                groupNodes[group].Remove(nodeName);
                if (groupNodes[group].Count == 0)
                {
                    groupNodes.Remove(group);
                }
            }
            
        }

        public void RemoveGroup(DSGroup group)
        {
            string oldGroupName = group.OldTitle.ToLower();
            List<DSGroup> groupsList = groups[oldGroupName].Groups;
            
            groupsList.Remove(group);
            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --RepeatedNamesAmount;
                groupsList[0].ResetStyle();
                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }
        
        #endregion
        

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition,bool isSearchWindow = false)
        {
            
            //位置换算
            Vector2 worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(
                editorWindow.rootVisualElement.parent,
                mousePosition - editorWindow.position.position);

            if (!isSearchWindow)
            {
                worldMousePosition = mousePosition;
            }
         
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            
            return localMousePosition;
            
        }

        #endregion
    }
}