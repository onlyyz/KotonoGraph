using System;
using System.Collections.Generic;
using DS.ScriptableObjects;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace  DS.Utilities
{
    using Data;
    using Winndos;
    using Elements;
    using Data.Save;
    
    //TODO:Load and Save the Graph
    public static class DSIOUtility
    {
        private static DSGraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;
        private static List<DSGroup> groups;
        private static List<DSNode> nodes;

        private static Dictionary<string, DSDialogueGroupSO> createDialogueGroups;
        private static Dictionary<string, DSDialogueSO> createDialogue;
        public static void Initialize(DSGraphView dsGraphView, string graphName)
        {
            graphView = dsGraphView;
            graphFileName = graphName;

            groups = new List<DSGroup>();
            nodes = new List<DSNode>();
            createDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
            createDialogue = new Dictionary<string, DSDialogueSO>();
            
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";
        }
        
        
        #region Save Methods

        public static void Save()
        {
            CreateStaticFolders();

            GetElementFromGraphView();
            
            DSGraphSaveDataSO graphData =
                CreateAssets<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);
            
            DSDialogueContainerSO dialogueContainer =
                CreateAssets<DSDialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGraph(graphData, dialogueContainer);
            SaveNodes(graphData,dialogueContainer);
            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        #endregion


        #region Creation Methods

        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor/DialogueSystem", "Graphs");
            
            CreateFolder("Assets", "DialogueSystem");
            CreateFolder("Assets/Editor/DialogueSystem", "Dialogues");
            CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

       
        #endregion

        #region Groups

        public static void SaveGraph(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
        {
            foreach (DSGroup group in groups)
            {
                SaveGroupToGraph(group,graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);
            }
        }
        
        public static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataSO graphData)
        {
            DSGroupSaveData groupData = new DSGroupSaveData()
            {
                ID = group.ID,
                Name = group.name,
                Postion = group.GetPosition().position
            };
            graphData.Groups.Add(groupData);
        }
        
        public static void SaveGroupToScriptableObject(DSGroup group, DSDialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;
            
            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");
            
            
            DSDialogueGroupSO dialogueGroup =
                CreateAssets<DSDialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            
            dialogueGroup.Initialize(groupName);
            
            //临时字典，保存group里面的Node
            createDialogueGroups.Add(group.ID,dialogueGroup);
            //Dictionary
            dialogueContainer.DialogueGroups.Add(dialogueGroup,new List<DSDialogueSO>());
            SaveAsset(dialogueGroup);
        }
    
        #endregion

        #region Node
        public static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
        {
            foreach (DSNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);
            }

            UpdateDialogueChoiceConnections();

        }
        
        public static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
        {
            //Clone 防止数据引用同步修改
            List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();
            foreach (var choice in node.Choices)
            {
                DSChoiceSaveData choiceData = new DSChoiceSaveData()
                {
                    NodeID = choice.NodeID,
                    Text = choice.Text
                };
                choices.Add(choiceData);
            }
            
            
            DSNodeSaveData nodeData = new DSNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                GroupID = node.Group?.ID,
                DialogueType = node.DialogueType,
                Postion = node.GetPosition().position
            };
            graphData.Nodes.Add(nodeData);
        }

        public static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
        {
            DSDialogueSO dialogue;

            if (node.Group != null)
            {
                dialogue =
                    CreateAssets<DSDialogueSO>
                        ($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                dialogueContainer.DialogueGroups.AddItem(createDialogueGroups[node.Group.ID],dialogue);
            }
            else
            {
                dialogue =
                    CreateAssets<DSDialogueSO>
                        ($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            dialogue.Initialize
            (
                node.DialogueName,
                node.Text,
                ConvertNodeChoicesTODialogueChoices(node.Choices),
                node.DialogueType,
                node.IsStartingNode()
            );
            
            createDialogue.Add(node.ID,dialogue);
            
            SaveAsset(dialogue);
        }

        public static  List<DSDialogueChoiceData> ConvertNodeChoicesTODialogueChoices(List<DSChoiceSaveData> nodeChoices)
        {
            List<DSDialogueChoiceData> dialogueChoice = new List<DSDialogueChoiceData>();
            foreach (var nodeChoice in nodeChoices)
            {
                DSDialogueChoiceData choiceData = new DSDialogueChoiceData()
                {
                    Text = nodeChoice.Text
                };
                dialogueChoice.Add(choiceData);
            }
            
            
            return dialogueChoice;
        }

        public static void UpdateDialogueChoiceConnections()
        {
            foreach (DSNode node in nodes)
            {
                
            }   
        }
        #endregion
        
        
        #region Get GraphView element

        public static void GetElementFromGraphView()
        {
            Type groupTYpe = typeof(DSGroup);
          
            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DSNode node)
                {
                    nodes.Add(node);
                    return;
                }

                if (graphElement.GetType() == groupTYpe)
                {
                    DSGroup group = (DSGroup)graphElement;
                    groups.Add(group);
                }
            });
        }

        #endregion
        
        
        #region Utility

        
        private static void CreateFolder(string path,string folderName)
        {
            //Assets/Editor/DialogueSystem/Graphs
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }
            //Assets/Editor/DialogueSystem  Graphs
            AssetDatabase.CreateFolder(path, folderName);
        }

        public static T CreateAssets<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            
            // T asset = (T)AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset,fullPath);
            }
            return asset;
        }
        
        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
       
        
        
        #endregion
    }

}