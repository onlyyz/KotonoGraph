using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    public class RedSegmentEdge : FlowingEdge
    {
        public RedSegmentEdge()
        {
            // 设置Edge的颜色为红色
            edgeControl.inputColor = Color.red;
            edgeControl.outputColor = Color.red;

            // 重写Edge控件的形状变化逻辑，实现三段的控制点
            edgeControl.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                // 将Edge分为三段
                Vector2 startPoint = edgeControl.from;
                Vector2 endPoint = edgeControl.to;

                // 中间的两个控制点，形成三段
                Vector2 middlePoint1 = (startPoint + endPoint) / 3;
                Vector2 middlePoint2 = (startPoint + endPoint) * 2 / 3;

                // 设置控制点（将Edge分为三段）
                // edgeControl.controlPoints = new Vector2[] { startPoint, middlePoint1, middlePoint2, endPoint };

                // 重新计算流动和形状
                OnEdgeControlGeometryChanged(evt);
            });
        }
    }
}
