
using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 可以直接将Unit数据保存到DB中
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class UnitDBSaveComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public HashSet<Type> EntityChangeTypeSet { get; } = new HashSet<Type>();
        public Dictionary<Type, byte[]> Bytes { get; } = new Dictionary<Type, byte[]>();
    } 
}