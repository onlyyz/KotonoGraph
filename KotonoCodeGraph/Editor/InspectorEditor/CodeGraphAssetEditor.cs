using UnityEditor;
using UnityEngine;

namespace Kotono.Code.Editor
{
    [CustomEditor(typeof(CodeGraphAsset))]
    public class CodeGraphAssetEditor : UnityEditor.Editor
    {
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
