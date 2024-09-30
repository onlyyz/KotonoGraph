using System.Collections.Generic;
using UnityEngine;

namespace Kotono.Code
{
    public class PropertyDataBase
    {
        // 单字典缓冲：存储所有 Edge 的数据
        protected Dictionary<string, object> edgeData = new Dictionary<string, object>();

        private object m_Value {get; set; }

        public object Value
        {
            get { return m_Value; }
            
        }
        
        
        // 判断是否存在数据
        public bool HasData(string edgeId)
        {
            return edgeData.ContainsKey(edgeId);
        }

        // 执行逻辑时的通用接口，可以被子类重写
        public virtual void Execute(Dictionary<string, object> edgeData)
        {
            // 基类可以提供默认的执行逻辑，具体节点可以重写
        }
        
        
        
        // 存储数据：以 Edge ID 作为键，存储数据
        public void StoreData(string edgeId, object value)
        {
            if (!edgeData.ContainsKey(edgeId))
            {
                edgeData[edgeId] = value;
            }
            else
            {
                edgeData[edgeId] = value;  // 更新已有的数据
            }
        }

        // 获取数据并拆箱：根据 Edge ID 获取数据，并将其拆箱为指定类型
        public T GetData<T>(string edgeId)
        {
            if (edgeData.ContainsKey(edgeId))
            {
                return (T)edgeData[edgeId];  // 拆箱
            }
            return default(T);  // 如果找不到，返回类型的默认值
        }
    }
}
