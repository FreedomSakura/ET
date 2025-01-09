using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class G2M_RequestExitGameHandler : MessageLocationHandler<Unit,G2M_RequestExitGame,M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response)
        {
            Log.Console("玩家从Map游戏逻辑服下线");
            
            // 这不对吧，应该换成缓存服
            
            unit.GetComponent<UnitDBSaveComponent>()?.SaveChangeNoWait();
            UnitRemove(unit).NoContext();
            await ETTask.CompletedTask;
        }


        private async ETTask UnitRemove(Unit unit)
        {
            await unit.Fiber().WaitFrameFinish();
            await unit.RemoveLocation(LocationType.Unit);
            unit.Scene().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(unit.Id);
            UnitComponent unitComponent =  unit.Scene().GetComponent<UnitComponent>();
            unitComponent.Remove(unit.Id);
        }
    }
}