using ET.Server;

namespace ET.Client
{
    [MessageHandler(SceneType.StateSync)]
    public class M2C_NoticeUnitNumericListHandler : MessageHandler<Scene, M2C_NoticeUnitNumericList>
    {
        protected override async ETTask Run(Scene root, M2C_NoticeUnitNumericList message)
        {
            Unit unit = root?.CurrentScene()?.GetComponent<UnitComponent>()?.Get(message.UnitId);
            var numericComponent = unit?.GetComponent<NumericComponent>();
            int count = message.NumericTypeList.Count;
            for (int i = 0; i < count; i++)
            {
                int numericType = message.NumericTypeList[i];
                long newValue = message.NewValueList[i];
                numericComponent?.Set(numericType, newValue);
            }
            
            await ETTask.CompletedTask;
        }
    }
}

