using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await TransferHelper.Transfer(unit, sceneInstanceId, sceneName);
        }
        

        public static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            Scene root = unit.Root();
            
            // location加锁
            long unitId = unit.Id;
            
            // 一定要将变动的数据存入库中 
            unit.GetComponent<UnitDBSaveComponent>()?.SaveChangeNoWait();
            
            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            
            // 引入了缓存服，原本这套序列化+反序列化Unit上的组件的方案就过时了
            // foreach (Entity entity in unit.Components.Values)
            // {
            //     if (entity is ITransfer)
            //     {
            //         request.Entitys.Add(entity.ToBson());
            //     }
            // }

            foreach (var kv in unit.GetComponent<UnitDBSaveComponent>().Bytes)
            {
                request.Types.Add(kv.Key.FullName);
                request.Entitys.Add(kv.Value);
            }
            
            unit.Dispose();
            
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}