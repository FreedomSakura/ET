using System;

namespace ET.Server
{
    [Invoke(TimerInvokeType.NumericSync)]
    public class NumericSyncTimerHandler : ATimer<NumericNoticeComponent>
    {
        protected override void Run(NumericNoticeComponent self)
        {
            self?.NoticeQueueMsgImmediately();
        }
    }
            
    [EntitySystemOf(typeof(NumericNoticeComponent))]
    public static partial class NumericNoticeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.NumericNoticeComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Server.NumericNoticeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.SyncTimerId);
            self.LastSendTime = 0;
            self.SyncTime = 0;

            for (int i = 0; i < self.QueueMessage.Count; ++i)
            {
                M2C_NoticeNumericMsg queueMsg = (M2C_NoticeNumericMsg)self.QueueMessage.Dequeue();
                queueMsg?.Dispose();
            }

            foreach (var queueMsg in self.OutPutMessageDict.Values)
            {
                queueMsg?.Dispose();
            }
            
            self.OutPutMessageDict.Clear();
            self.QueueMessage.Clear();
            self.OutPutMessageDict = default;
            self.QueueMessage = default;
        }

        public static void Notice(this NumericNoticeComponent self, int numericType, long newValue)
        {
            if (self.LastSendTime > 0 && TimeInfo.Instance.ServerNow() - self.LastSendTime < 100)
            {
                self.AddQueueMessage(numericType, newValue);
                self.CheckSyncTimer();
            }
            else
            {
                self.NoticeImmediately(numericType, newValue);
            }
        }

        public static void NoticeImmediately(this NumericNoticeComponent self, int numericType, long newValue)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_NoticeUnitNumeric SingleNumericMessage = M2C_NoticeUnitNumeric.Create(true);
            SingleNumericMessage.UnitId = unit.Id;
            SingleNumericMessage.NumericType = numericType;
            SingleNumericMessage.NewValue = newValue;
            self.LastSendTime = TimeInfo.Instance.ServerNow();

            MapMessageHelper.SendToClient(unit, SingleNumericMessage);
        }
        
        public static void AddQueueMessage(this NumericNoticeComponent self, int numericType, long newValue)
        {
            if (self.OutPutMessageDict.TryGetValue(numericType, out M2C_NoticeNumericMsg message))
            {
                message.NewValue = newValue;
            }
            else
            {
                message = M2C_NoticeNumericMsg.Create(true);
                message.NumericType = numericType;
                message.NewValue = newValue;
                self.OutPutMessageDict.Add(numericType, message);
                self.QueueMessage.Enqueue(message);
            }
        }

        public static void CheckSyncTimer(this NumericNoticeComponent self)
        {
            if (self.SyncTime < TimeInfo.Instance.ServerNow())
            {
                if (self.SyncTimerId > 0)
                {
                    self.Root().GetComponent<TimerComponent>().Remove(ref self.SyncTimerId);
                }

                self.SyncTime = TimeInfo.Instance.ServerNow() + 100;
                self.SyncTimerId = self.Root().GetComponent<TimerComponent>().NewOnceTimer(self.SyncTime, TimerInvokeType.NumericSync, self);
            }
        }

        public static void NoticeQueueMsgImmediately(this NumericNoticeComponent self)
        {
            int queueMsgNum = self.QueueMessage.Count;
            if (queueMsgNum <= 0)
            {
                return;
            }
            
            Unit unit = self.GetParent<Unit>();
            self.OutPutMessageDict.Clear();
            
            M2C_NoticeUnitNumericList m2C_NoticeUnitNumericList = M2C_NoticeUnitNumericList.Create(true);
            m2C_NoticeUnitNumericList.UnitId = unit.Id;
            
            for (int i = 0; i < queueMsgNum; i++)
            {
                M2C_NoticeNumericMsg queueMsg = (M2C_NoticeNumericMsg)self.QueueMessage.Dequeue();
                m2C_NoticeUnitNumericList.NumericTypeList.Add(queueMsg.NumericType);
                m2C_NoticeUnitNumericList.NewValueList.Add(queueMsg.NewValue);
                queueMsg?.Dispose();
            }
            
            self.LastSendTime = TimeInfo.Instance.ServerNow();
            MapMessageHelper.SendToClient(unit, m2C_NoticeUnitNumericList);
        }
    }
}