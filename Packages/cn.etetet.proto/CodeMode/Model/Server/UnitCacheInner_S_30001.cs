using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 玩家缓存相关
    /// </summary>
    // 增加或更新Unit缓存
    [MemoryPackable]
    [Message(UnitCacheInner.Other2UnitCache_AddOrUpdateUnit)]
    [ResponseType(nameof(UnitCache2Other_AddOrUpdateUnit))]
    public partial class Other2UnitCache_AddOrUpdateUnit : MessageObject, IRequest
    {
        public static Other2UnitCache_AddOrUpdateUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2UnitCache_AddOrUpdateUnit>(isFromPool);
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        /// <summary>
        /// 需要缓存的UnitId
        /// </summary>
        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        [MemoryPackOrder(1)]
        public List<string> EntityTypes { get; set; } = new();

        /// <summary>
        /// 实体序列化后的bytes
        /// </summary>
        [MemoryPackOrder(2)]
        public List<byte[]> EntityBytes { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;
            this.EntityTypes.Clear();
            this.EntityBytes.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(UnitCacheInner.UnitCache2Other_AddOrUpdateUnit)]
    public partial class UnitCache2Other_AddOrUpdateUnit : MessageObject, IResponse
    {
        public static UnitCache2Other_AddOrUpdateUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<UnitCache2Other_AddOrUpdateUnit>(isFromPool);
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Recycle(this);
        }
    }

    // 获取Unit缓存
    [MemoryPackable]
    [Message(UnitCacheInner.Other2UnitCache_GetUnit)]
    [ResponseType(nameof(UnitCache2Other_GetUnit))]
    public partial class Other2UnitCache_GetUnit : MessageObject, IRequest
    {
        public static Other2UnitCache_GetUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<Other2UnitCache_GetUnit>(isFromPool);
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        /// <summary>
        /// 需要获取的组件名
        /// </summary>
        [MemoryPackOrder(1)]
        public List<string> ComponentNameList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;
            this.ComponentNameList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(UnitCacheInner.UnitCache2Other_GetUnit)]
    public partial class UnitCache2Other_GetUnit : MessageObject, IResponse
    {
        public static UnitCache2Other_GetUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<UnitCache2Other_GetUnit>(isFromPool);
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<byte[]> EntityList { get; set; } = new();

        [MemoryPackOrder(4)]
        public List<string> ComponentNameList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.EntityList.Clear();
            this.ComponentNameList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    public static class UnitCacheInner
    {
        public const ushort Other2UnitCache_AddOrUpdateUnit = 30002;
        public const ushort UnitCache2Other_AddOrUpdateUnit = 30003;
        public const ushort Other2UnitCache_GetUnit = 30004;
        public const ushort UnitCache2Other_GetUnit = 30005;
    }
}