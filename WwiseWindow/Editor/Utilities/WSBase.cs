using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WS.BaseElement
{
    public static class WSBase
    {
        public static VisualElement CreateHorizontalContainer(int heightvalue = 200)
        {
            // 创建水平容器
            return new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row, 
                    justifyContent = Justify.Center,
                    height = heightvalue
                }
            };
        }

        public static VisualElement CreateVerticalContainer(int heightvalue = 200)
        {
            return new VisualElement
            {
                style = { flexDirection = FlexDirection.Column,
                    flexGrow = 1, 
                    paddingLeft = 10, 
                    paddingRight = 10 ,
                    height = heightvalue,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
        }
    }

}