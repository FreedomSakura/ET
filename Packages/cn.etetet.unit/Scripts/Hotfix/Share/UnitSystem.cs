using System;
using ET.Server;

namespace ET
{
    [EntitySystemOf(typeof(Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }

        public static UnitConfig Config(this Unit self)
        {
            return UnitConfigCategory.Instance.Get(self.ConfigId);
        }
        
        public static int Type(this Unit self)
        {
            return self.Config().Type;
        }

        [EntitySystem]
        private static void GetComponentSys(this Unit unit, Type type)
        {
            if (!typeof(IUnitCache).IsAssignableFrom(type))
            {
                return;
            }

            EventSystem.Instance.Publish(unit.Scene(), new UnitGetComponent()
            {
                Unit = unit,
                Type = type
            });
        }
    }
}