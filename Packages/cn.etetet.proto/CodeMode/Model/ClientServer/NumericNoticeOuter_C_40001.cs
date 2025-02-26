using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(NumericNoticeOuter.M2C_NoticeUnitNumeric)]
    public partial class M2C_NoticeUnitNumeric : MessageObject, IMessage
    {
        public static M2C_NoticeUnitNumeric Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_NoticeUnitNumeric>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int NumericType { get; set; }

        [MemoryPackOrder(2)]
        public long NewValue { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.NumericType = default;
            this.NewValue = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(NumericNoticeOuter.M2C_NoticeNumericMsg)]
    public partial class M2C_NoticeNumericMsg : MessageObject, IMessage
    {
        public static M2C_NoticeNumericMsg Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_NoticeNumericMsg>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int NumericType { get; set; }

        [MemoryPackOrder(1)]
        public long NewValue { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.NumericType = default;
            this.NewValue = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(NumericNoticeOuter.M2C_NoticeUnitNumericList)]
    public partial class M2C_NoticeUnitNumericList : MessageObject, IMessage
    {
        public static M2C_NoticeUnitNumericList Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_NoticeUnitNumericList>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public List<int> NumericTypeList { get; set; } = new();

        [MemoryPackOrder(2)]
        public List<long> NewValueList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.NumericTypeList.Clear();
            this.NewValueList.Clear();

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(NumericNoticeOuter.C2M_TestNumericValue)]
    [ResponseType(nameof(M2C_TestNumericValue))]
    public partial class C2M_TestNumericValue : MessageObject, ILocationRequest
    {
        public static C2M_TestNumericValue Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<C2M_TestNumericValue>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(NumericNoticeOuter.M2C_TestNumericValue)]
    public partial class M2C_TestNumericValue : MessageObject, ILocationResponse
    {
        public static M2C_TestNumericValue Create(bool isFromPool = false)
        {
            return ObjectPool.Fetch<M2C_TestNumericValue>(isFromPool);
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
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

    public static class NumericNoticeOuter
    {
        public const ushort M2C_NoticeUnitNumeric = 40002;
        public const ushort M2C_NoticeNumericMsg = 40003;
        public const ushort M2C_NoticeUnitNumericList = 40004;
        public const ushort C2M_TestNumericValue = 40005;
        public const ushort M2C_TestNumericValue = 40006;
    }
}