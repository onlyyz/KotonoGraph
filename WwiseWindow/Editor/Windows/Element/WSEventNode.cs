using System;
using UnityEngine;


namespace WS
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;
    using WS.Library;
    using WS.StyleUtilities;
    using WS.Window;
    public class WSEventNode : Node
    {
        public  GameObject Singer =>  WSLibrary.Core.Singer;
        
        private string Name;
        private Vector2 Position;
        private float MaxDecayDistance;
        private uint playingID = UInt32.MinValue;
        
        public  WSEventNode( Vector2 position,WwiseObjectReference reference)
        {
            Name = reference.ObjectName;
            Position = position;
            title = Name;
            
            Draw(reference.Id);
        }


        public virtual void Draw(uint id)
        {
            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

           
            
            
            /* BUTTON CONTAINER */
            uint sfxID = 1;
            bool state = false;
            
            Button playButton = new Button(() =>
            {
                if(state)
                    return;
                sfxID =  AkSoundEngine.PostEvent(id, Singer);
            })
            {
                text = "Play"
            };
            Button stopButton = new Button(() =>
            {
                state = false;
                AkSoundEngine.StopPlayingID(sfxID);
                AkSoundEngine.StopPlayingID(id);
            })
            {
                text = "Stop"
            };
            
            
            this.AddStyleSheets("WwiseSystem/WSEventNode");
            playButton.AddToClassList("ds-node__custom-data-button");
            stopButton.AddToClassList("ds-node__custom-data-button");
           
            
            inputContainer.AddToClassList("centerContainer");
            outputContainer.AddToClassList("centerContainer");
            inputContainer.Add(playButton);
            outputContainer.Add(stopButton);
        }
        
        
    }

}
