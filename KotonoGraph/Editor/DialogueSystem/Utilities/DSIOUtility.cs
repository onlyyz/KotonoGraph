using System.Collections;
using System.Collections.Generic;
using DS.Winndos;
using UnityEditor;
using UnityEngine;

namespace  DS.Utilities
{
    //TODO:Load and Save the Graph
    public static class DSIOUtility
    {
        private static DSGraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;
        
        public static void Initialize(string graphName)
        {
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";
        }
        
        
        #region Save Methods

        public static void Save()
        {
            CreateStaticFolders();

            GetElementFromGraphView();
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

        #region Get GraphView element

        public static void GetElementFromGraphView()
        {
            
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


        #endregion
    }

}