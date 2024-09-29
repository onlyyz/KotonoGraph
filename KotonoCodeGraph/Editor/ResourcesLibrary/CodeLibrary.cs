using UnityEngine;

namespace Kotono.Code.Editor
{
    public class CodeLibrary : MonoBehaviour
    {
        private const string
            eventDataName = "EventData", 
            SoundcasterName = "SoundcasterData";

        
        public bool ChoiceSinger;
        public GameObject CustomSinger;
        
        public GameObject Lister;
        public GameObject AKLister;
        public GameObject Singer
        {
            get
            {
                return ChoiceSinger ? 
                    CustomSinger? CustomSinger: AKLister : 
                    AKLister;
            }
            set { }
        }

        public CodeIcon Icons;
        
        //单例模式
        private static CodeLibrary _instance;
        public static CodeLibrary Core
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CodeLibrary();
                }
                return _instance;
            }
        }

        public CodeLibrary()
        {
            LoadData();
            GetLister();
        }
        public void InstanceCodeLibrary()
        {
            LoadData();
            GetLister();
        }
        
        void GetLister()
        {
            // AkAudioListener[] listeners = Object.FindObjectsOfType<AkAudioListener>();
            //
            // foreach (AkAudioListener listener in listeners)
            // {
            //     if (listener.gameObject.CompareTag("MainCamera"))
            //     {
            //         Lister = listener.gameObject;
            //         AKLister = Lister;
            //         break;
            //     }
            // }
        }
        
        #region Load Data
        
        public void LoadData()
        {
            Icons = new CodeIcon();
            // soundcasterData = WSIOUtility.CreateAsset<SoundcasterData>(path, SoundcasterName);
        }
        
        #endregion
    }
}
