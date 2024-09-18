using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WS.Utilities
{
    public static class WSIOUtility 
    {
        #region Asset
        
        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }
        

        #endregion
        
        #region Path
        
        public static string GetDataPath(this EditorWindow editorWindow)
        {
            string scriptPath = GetCurrentScriptPath(editorWindow);
            string trimmedPath = TrimPathAfterEditor(scriptPath);

            string dataPath = $"{trimmedPath}Resources/Data";

            return dataPath;
        }
        private static string GetCurrentScriptPath(EditorWindow editorWindow) 
        {
          
            string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(editorWindow));
            return path;
        }

        private static string TrimPathAfterEditor(string path)
        {
            int editorIndex = path.IndexOf("Editor");
            if (editorIndex >= 0)
            {
                path = path.Substring(0, editorIndex);
            }
            return path;
        }

        #endregion
    }
    
    
   
}
