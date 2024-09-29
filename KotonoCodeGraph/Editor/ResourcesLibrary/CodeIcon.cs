using UnityEngine;

namespace Kotono.Code.Editor
{
    public class CodeIcon
    {
        private UnityEngine.Texture2D Port;

        public CodeIcon()
        {
            LoadIcons();
        }
        
        protected UnityEngine.Texture2D GetTexture(string texturePath)
        {
            try
            {
                // return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(texturePath);
                return Resources.Load<Texture2D>(texturePath);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(string.Format("WwiseUnity: Failed to find local texture: {0}", ex));
                return null;
            }
        }
        
        public void LoadIcons()
        {
            var tempPath = "Icons/";
            Port = GetTexture(tempPath + "Port");
        }

        public UnityEngine.Texture2D GetIcon()
        {
            return Port;
        }
    }
}
