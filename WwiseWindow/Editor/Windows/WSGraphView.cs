using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using WS.Library;


namespace WS.Window
{
    using DataClass;
    using Utilities;
    using Utilitiy;
    using Selection;
    using StyleUtilities;
    public class WSGraphView : GraphView
    {
      
        private const int 
            NodeSize = 100,
            GridSpacing = 60;
        
        
        
        Node previousNode = null; // 用于存储上一个操作的节点
        
        WSToolbar toolbar;
        WSSelectionSection SelectionSection;
        WSWindow editorWindow;
      
      
        SoundcasterData soundcasterData;
        
        public WSGraphView(WSWindow window)
        {
            editorWindow = window;
            toolbar = new WSToolbar(this);
            SelectionSection = new WSSelectionSection(this);
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            Insert(0, new GridBackground());


            InitiateWSLibrary();
            AddManipulators();
            
            AddStyles();
        }

        #region Add Panel

        private void InitiateWSLibrary()
        {
            WSLibrary.Core.InstanceWSLibrary(editorWindow,this);
        }
        
        
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
        
        #endregion

        public void DragEvent(WwiseObjectReference reference,Dictionary<System.Type, WwiseObjectType> ScriptTypeMap)
        {
            if(reference.WwiseObjectType == WwiseObjectType.Event)
                CreateNodeNextToPrevious(reference);
            
            
            SelectionSection.AddProperty(reference,ScriptTypeMap);
        }
        
        public void CreateNodeNextToPrevious(WwiseObjectReference reference)
        {
            Vector2 position;
            if (previousNode == null)
            {
                position = Vector2.zero;
            }
            else
            {
                position = previousNode.GetPosition().position;
            }
            position.x += NodeSize + GridSpacing;
            WSEventNode node = new WSEventNode(position, reference);
            
            
            node.SetPosition(new Rect(position, new Vector2(NodeSize, NodeSize)));
            AddElement(node);
            previousNode = node;
        }
        

        #region Styles

        private void AddStyles()
        {
         
            this.AddStyleSheets(
                "WwiseSystem/WSGraphViewStyles",
                "WwiseSystem/WSNodeStyles"
            );
        }

        #endregion
        
    }
    
}