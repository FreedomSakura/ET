using System;

namespace ET.Server
{
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

        }

        public static void Notice(this NumericNoticeComponent self, int numericType, long newValue)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_NoticeUnitNumeric SingleNumericMessage = M2C_NoticeUnitNumeric.Create(true);
            SingleNumericMessage.UnitId = unit.Id;
            SingleNumericMessage.NumericType = numericType;
            SingleNumericMessage.NewValue = newValue;
            self.LastSendTime = TimeInfo.Instance.ServerNow();

            MapMessageHelper.SendToClient(unit, SingleNumericMessage);
        }
    }
}