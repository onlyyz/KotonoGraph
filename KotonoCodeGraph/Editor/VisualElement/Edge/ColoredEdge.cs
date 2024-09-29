using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    public class ColoredEdge : Edge
    {
        private Label label; // 中间的文字标签

        public ColoredEdge()
        {
            // 初始化时检查并创建标签
            CheckAndCreateLabel();
            // 注册几何形状变化事件，相当于 Unity 的 OnEnable
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        // 检查并创建 Label
        private void CheckAndCreateLabel()
        {
            if (label == null)
            {
                label = new Label("连接");
                label.style.position = Position.Absolute;
                Add(label); // 将标签添加到 Edge 中
            }
        }

        // 当几何形状变化时触发，类似 OnEnable
        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            // 检查并创建 Label
            CheckAndCreateLabel();
            UpdateLabelPosition();
        }
        
        // 重写 UpdateEdgeControl 方法，确保标签放在中间
        public override bool UpdateEdgeControl()
        {
            Debug.Log("测试");
            if (!base.UpdateEdgeControl())
            {
                return false;
            }

            // 每次更新时检查并重新创建 Label
            CheckAndCreateLabel();

            UpdateLabelPosition(); // 更新标签的位置
            return true;
        }

        // 计算并更新标签的位置（放在 Edge 中间）
        private void UpdateLabelPosition()
        {
            // 确保 controlPoints 存在，并且至少有两个点
            if (edgeControl.controlPoints.Length >= 2)
            {
                Vector2 startPoint = edgeControl.controlPoints[0];
                Vector2 endPoint = edgeControl.controlPoints[edgeControl.controlPoints.Length - 1];

                // 计算中点
                Vector2 middlePoint = (startPoint + endPoint) / 2;

                // 更新 Label 位置，使其位于中点
                label.style.left = middlePoint.x - label.resolvedStyle.width / 2;
                label.style.top = middlePoint.y - label.resolvedStyle.height / 2;

                // 确保 Label 显示在前面
                label.BringToFront();
            }
        }
    }
}