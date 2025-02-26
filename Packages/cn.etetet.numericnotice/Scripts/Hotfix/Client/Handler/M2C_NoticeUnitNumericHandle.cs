namespace ET.Client
{
    [MessageHandler(SceneType.StateSync)]
    public class M2C_NoticeUnitNumericHandler : MessageHandler<Scene, M2C_NoticeUnitNumeric>
    {
        protected override async ETTask Run(Scene root, M2C_NoticeUnitNumeric message)
        {
            // 找到Unit并为其设置新数值
            root?.CurrentScene()?.GetComponent<UnitComponent>()?.Get(message.UnitId)?.GetComponent<NumericComponent>()?
                    .Set(message.NumericType, message.NewValue);
            
            await ETTask.CompletedTask;
        }
    }
}