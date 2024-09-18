using UnityEngine;
using UnityEngine.UIElements;
using WS.DataElement;

namespace WS.Selection
{
    using BaseElement;
    using StyleUtilities;
    using Window;


    public class WSSelectionSection : VisualElement
    {
        private EnumField enumField;
        private VisualElement rootVisualElement;
        private WSGraphView graphView;

        private VisualElement States;
        private VisualElement Switchs;
        private VisualElement RTPCs;
        private VisualElement Triggers;

        public ScrollView statesScrollView;
        public ScrollView SwitchsScrollView;
        public ScrollView RTPCsScrollView;
        public ScrollView TriggersScrollView;

        public WSSelectionSection(WSGraphView graphView)
        {
            this.graphView = graphView;

            // 添加水平容器
            AddHorizontalContainers();
            AddStyles();
            graphView.Add(this);
        }



        #region Add Panel

        private void AddHorizontalContainers()
        {
            // 创建水平容器
            var horizontalContainer = WSBase.CreateHorizontalContainer();


            States = CreateVerticalContainer("States");
            Switchs = CreateVerticalContainer("Switchs");
            RTPCs = CreateVerticalContainer("RTPCs");
            Triggers = CreateVerticalContainer("Triggers");
           
            States.Add( statesScrollView =  CreateScrollView());
            Switchs.Add( SwitchsScrollView =  CreateScrollView());
            RTPCs.Add( RTPCsScrollView =  CreateScrollView());
            Triggers.Add( TriggersScrollView =  CreateScrollView());
            
            
            horizontalContainer.Add(States);
            horizontalContainer.Add(Switchs);
            horizontalContainer.Add(RTPCs);
            horizontalContainer.Add(Triggers);


            Add(horizontalContainer); // 将水平容器添加到根元素
        }


        #endregion


        private VisualElement CreateVerticalContainer(string name)
        {
            // 创建垂直容器
            var verticalContainer = WSBase.CreateVerticalContainer(160);

            // 添加容器标题
            AddContainerLabel(verticalContainer, name);

            // 添加容器操作按钮
            AddContainerButton(verticalContainer);
            
            return verticalContainer;
        }

        public ScrollView CreateScrollView()
        {
            return new ScrollView()
            {
                verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible,
                style =
                {
                    height = 120
                }
            };
        }
        
        private void AddContainerLabel(VisualElement verticalContainer, string name)
        {
            // 在垂直容器中添加标签
            var label = new Label(name);
            verticalContainer.Add(label);
        }

        private void AddContainerButton(VisualElement verticalContainer)
        {
            // 为每个垂直容器添加按钮，点击后调用相应的函数
            var button = new Button(() => FunctionForContainer1())
            {
                text = $"Show All"
            };
            verticalContainer.Add(button);
        }

      

        // 为每个容器预留函数接口
        private void FunctionForContainer1()
        {
            Debug.Log("Function for Container 1 executed");
            // 在这里添加容器1的逻辑
        }

        private void FunctionForContainer2()
        {
            Debug.Log("Function for Container 2 executed");
            // 在这里添加容器2的逻辑
        }

        private void FunctionForContainer3()
        {
            Debug.Log("Function for Container 3 executed");
            // 在这里添加容器3的逻辑
        }

        private void FunctionForContainer4()
        {
            Debug.Log("Function for Container 4 executed");
            // 在这里添加容器4的逻辑
        }




        #region Styles

        private void AddStyles()
        {
              
            
            States.AddStyleSheets("WwiseSystem/WSProperty");
            Switchs.AddStyleSheets("WwiseSystem/WSProperty");
            RTPCs.AddStyleSheets("WwiseSystem/WSProperty");
            Triggers.AddStyleSheets("WwiseSystem/WSProperty");
            this.AddStyleSheets("WwiseSystem/WSSelectionSection");
        }

        #endregion

    }
}