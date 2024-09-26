using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Kotono.Code.Editor
{
    [CustomEditor(typeof(CodeGraphAsset))]
    public class CodeGraphAssetEditor : UnityEditor.Editor
    {
        //双击资源打开
        [OnOpenAsset]
        public static bool OpenAsset(int instanceID, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset.GetType() == typeof(CodeGraphAsset))
            {
                CodeEditorWindow.Open((CodeGraphAsset)asset);
                return true;
            }

            return false;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open CodeGraph Window"))
            {
                CodeEditorWindow.Open((CodeGraphAsset)target);
            }
        }
    }
}
