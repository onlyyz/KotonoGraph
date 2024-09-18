using UnityEngine;
using WS.Utilities;


namespace WS.Library
{
    using Window;
    public class WSLibrary
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

     
        // Element
        public WSWindow editorWindow;
        public WSGraphView graphView;
        public WwiseIcons Icons;

        //单例模式
        private static WSLibrary _instance;
        public static WSLibrary Core
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WSLibrary();
                }
                return _instance;
            }
        }

        public WSLibrary() { }

        public void InstanceWSLibrary(WSWindow Window,WSGraphView graphView)
        {
            this.editorWindow = Window;
            this.graphView = graphView;

            LoadData();
            GetLister();
        }

        
        
        void GetLister()
        {
            AkAudioListener[] listeners = Object.FindObjectsOfType<AkAudioListener>();

            foreach (AkAudioListener listener in listeners)
            {
                if (listener.gameObject.CompareTag("MainCamera"))
                {
                    Lister = listener.gameObject;
                    AKLister = Lister;
                    break;
                }
            }
        }
        
        #region Load Data
        
        public void LoadData()
        {
            string path = editorWindow.GetDataPath();
            // Debug.Log("加载数据 ： " + path);
            Icons = new WwiseIcons();
            // soundcasterData = WSIOUtility.CreateAsset<SoundcasterData>(path, SoundcasterName);
        }

        #endregion
        
    }
}