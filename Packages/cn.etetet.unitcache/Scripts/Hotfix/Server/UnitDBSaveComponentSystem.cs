using System;
using Sirenix.OdinInspector;

namespace ET.Server
{
    [Invoke(TimerInvokeType.SaveChangeDBData)]
    public class UnitDBSaveComponentTimer : ATimer<UnitDBSaveComponent>
    {
        protected override void Run(UnitDBSaveComponent self)
        {
            try
            {
                self?.SaveChange().NoContext();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    [EntitySystemOf(typeof(UnitDBSaveComponent))]
    [FriendOfAttribute(typeof(ET.Server.UnitDBSaveComponent))]
    public static partial class UnitDBSaveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitDBSaveComponent self)
        {
            // 正式上线部署 每10 - 15分钟随机存储落地一次
            // 测试环境，4秒一次
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(4 * 1000, TimerInvokeType.SaveChangeDBData, self);
        }
        [EntitySystem]
        private static void Destroy(this ET.Server.UnitDBSaveComponent self)
        {

        }

        public static async ETTask SaveChange(this ET.Server.UnitDBSaveComponent self)
        {
            
        }
        
        public static void SaveChangeNoWait(this ET.Server.UnitDBSaveComponent self)
        {

        }
        
        
    }
}

