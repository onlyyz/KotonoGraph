using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    public class KotonoEdge : Edge
    {
        private Label edgeLabel;
        public KotonoEdge(Port inputPort, Port outputPort)
        {
            input = inputPort;
            output = outputPort;

            // 根据端口的类型设置不同的样式
            if (input.portType == typeof(PortTypes.FlowPort))
            {
                SetEdgeStyle(Color.blue, 2f); // FlowPort样式
            }
            // else if (input.portType == typeof())
            // {
            //     SetEdgeStyle(Color.red, 3f);  // DataPort样式
            // }
            // 其他类型的Port也可以继续添加...
            // 创建一个Label并添加到Edge中
            edgeLabel = new Label("Edge Label"); // 你可以自定义文本内容
            this.Add(edgeLabel);

            // 设置Label的初始位置（例如居中显示）
            edgeLabel.style.position = Position.Absolute;
            edgeLabel.style.left = 50; // 你可以动态计算位置
            edgeLabel.style.top = 10;
        }

        private void SetEdgeStyle(Color color, float width)
        {
            // 设置Edge的颜色和粗细
            var edgeColor = new StyleColor(color);
            var edgeWidth = new StyleFloat(width);

            // 访问Edge的UI元素并设置样式
            var edgeElement = this.Q("edgeControl");
            edgeElement.style.borderBottomColor = edgeColor;
            edgeElement.style.borderBottomWidth = edgeWidth;
        }
        
        // 每当Edge的位置发生变化时，更新Label的位置
        // public override void UpdateLayout()
        // {
        //     base.UpdateLayout();
        //
        //     // 根据Edge的起点和终点，动态调整文本位置
        //     var fromPos = edgeControl.from;
        //     var toPos = edgeControl.to;
        //     var midPoint = (fromPos + toPos) / 2;
        //
        //     edgeLabel.style.left = midPoint.x;
        //     edgeLabel.style.top = midPoint.y;
        // }
    }
}
