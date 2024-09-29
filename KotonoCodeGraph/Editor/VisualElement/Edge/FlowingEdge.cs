using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{
    // FlowingEdge类继承自Edge类，用于实现自定义连接线（Edge）的显示、样式和流动效果
    public class FlowingEdge : Edge
    {
        // 是否启用流动效果的开关
        public bool enableFlow
        {
            get => _isEnableFlow;
            set
            {
                if (_isEnableFlow == value)
                {
                    return; // 如果状态没变化，不做任何操作
                }

                _isEnableFlow = value;
                if (_isEnableFlow)
                {
                    Add(_flowImg); // 启用时，将流动的图像添加到Edge上
                }
                else
                {
                    Remove(_flowImg); // 禁用时，移除流动的图像
                }
            }
        }

        private bool _isEnableFlow; // 内部字段，保存是否启用流动

        // 流动图像的大小，影响图像的宽度和高度
        public float flowSize
        {
            get => _flowSize;
            set
            {
                _flowSize = value;
                _flowImg.style.width = new Length(_flowSize, LengthUnit.Pixel); // 设置宽度
                _flowImg.style.height = new Length(_flowSize, LengthUnit.Pixel); // 设置高度
            }
        }

        private float _flowSize = 6f; // 默认的流动图像大小

        // 控制流动速度
        public float flowSpeed { get; set; } = 300f;

        // 用于显示流动效果的图像控件
        private readonly Image _flowImg;

        // 构造函数，初始化Edge并设置流动图像的样式
        public FlowingEdge()
        {
            // 初始化流动图像
            _flowImg = new Image
            {
                name = "flow-image",
                style =
                {
                    width = new Length(flowSize, LengthUnit.Pixel),
                    height = new Length(flowSize, LengthUnit.Pixel),
                    borderTopLeftRadius = new Length(flowSize / 2, LengthUnit.Pixel), // 圆形边角
                    borderTopRightRadius = new Length(flowSize / 2, LengthUnit.Pixel),
                    borderBottomLeftRadius = new Length(flowSize / 2, LengthUnit.Pixel),
                    borderBottomRightRadius = new Length(flowSize / 2, LengthUnit.Pixel),
                }
            };

            // 注册Edge控件的几何形状变化事件，当Edge形状变化时重置流动
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeControlGeometryChanged);

            enableFlow = true; // 默认启用流动效果
        }

        // 重写UpdateEdgeControl，用于更新Edge和流动图像
        public override bool UpdateEdgeControl()
        {
            if (!base.UpdateEdgeControl())
            {
                return false; // 如果基类的更新失败，直接返回
            }

            UpdateFlow(); // 更新流动状态
            return true;
        }

        #region 流动效果逻辑

        // 记录Edge的总长度、当前流动的长度和流动阶段等参数
        private float _totalEdgeLength;  // 整条边的总长度
        private float _passedEdgeLength; // 已经过的边的长度
        private int _flowPhaseIndex;     // 当前的流动阶段索引
        private double _flowPhaseStartTime; // 当前流动阶段开始的时间
        private double _flowPhaseDuration;  // 当前流动阶段的持续时间
        private float _currentPhaseLength;  // 当前阶段的长度

        // 更新流动图像的位置和颜色
        public void UpdateFlow()
        {
            if (!enableFlow)
            {
                return; // 如果流动效果禁用，直接返回
            }

            // 计算当前位置的进度（百分比）
            var posProgress = (EditorApplication.timeSinceStartup - _flowPhaseStartTime) / _flowPhaseDuration;
            var flowStartPoint = edgeControl.controlPoints[_flowPhaseIndex];       // 当前阶段的起点
            var flowEndPoint = edgeControl.controlPoints[_flowPhaseIndex + 1];     // 当前阶段的终点
            var flowPos = Vector2.Lerp(flowStartPoint, flowEndPoint, (float)posProgress); // 根据进度计算当前位置
            _flowImg.transform.position = flowPos - Vector2.one * flowSize / 2;    // 更新图像位置

            // 根据位置进度计算颜色变化（从输出端到输入端的渐变）
            var colorProgress = (_passedEdgeLength + _currentPhaseLength * posProgress) / _totalEdgeLength;
            var startColor = edgeControl.outputColor;
            var endColor = edgeControl.inputColor;
            var flowColor = Color.Lerp(startColor, endColor, (float)colorProgress); // 根据进度插值颜色
            _flowImg.style.backgroundColor = flowColor;

            // 如果当前阶段完成，进入下一个阶段
            if (posProgress >= 0.99999f)
            {
                _passedEdgeLength += _currentPhaseLength; // 更新已过长度

                _flowPhaseIndex++; // 进入下一个阶段
                if (_flowPhaseIndex >= edgeControl.controlPoints.Length - 1)
                {
                    // 如果所有阶段完成，重置到起点
                    _flowPhaseIndex = 0;
                    _passedEdgeLength = 0;
                }

                // 更新流动阶段的起始时间和阶段长度
                _flowPhaseStartTime = EditorApplication.timeSinceStartup;
                _currentPhaseLength = Vector2.Distance(edgeControl.controlPoints[_flowPhaseIndex],
                    edgeControl.controlPoints[_flowPhaseIndex + 1]);
                _flowPhaseDuration = _currentPhaseLength / flowSpeed;
            }
        }

        // 当Edge的形状发生变化时，重置流动状态
        public void OnEdgeControlGeometryChanged(GeometryChangedEvent evt)
        {
            // 重置流动参数
            _flowPhaseIndex = 0;
            _passedEdgeLength = 0;
            _flowPhaseStartTime = EditorApplication.timeSinceStartup;
            _currentPhaseLength = Vector2.Distance(edgeControl.controlPoints[_flowPhaseIndex],
                edgeControl.controlPoints[_flowPhaseIndex + 1]);
            _flowPhaseDuration = _currentPhaseLength / flowSpeed;

            // 计算整个Edge的长度
            _totalEdgeLength = 0;
            for (int i = 0; i < edgeControl.controlPoints.Length - 1; i++)
            {
                var p = edgeControl.controlPoints[i];
                var pNext = edgeControl.controlPoints[i + 1];
                var phaseLen = Vector2.Distance(p, pNext); // 每段的长度
                _totalEdgeLength += phaseLen; // 累加到总长度
            }
        }

        #endregion
    }
    
}
