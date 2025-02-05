using System;

namespace ET.Server
{
    [Event(SceneType.All)]
    public class UnitGetComponent_GetComponent : AEvent<Scene, UnitGetComponent>
    {
        protected override async ETTask Run(Scene scene, UnitGetComponent args)
        {
            Unit unit = args.Unit;
            Type type = args.Type;
            
            unit.GetComponent<UnitDBSaveComponent>()?.AddChange(type);
            
            //判定Unit身上是否存在需要获取的组件
            if (unit.Components.ContainsKey(type.TypeHandle.Value.ToInt64()))
            {
                return;
            }
            
            UnitDBSaveComponent unitDBSaveComponent = unit.GetComponent<UnitDBSaveComponent>();
            if (unitDBSaveComponent == null)
            {
                return;
            }
            
            //Unit身上不存在需要挂载的组件，这时就从字节数组容器中获取，并进行反序列化挂载到Unit上
            if (!unit.GetComponent<UnitDBSaveComponent>().Bytes.TryGetValue(type, out byte[] bs))
            {
                return;
            }
            
            // 这里的意图时延迟组件的反序列时机，玩家有用到对应组件再对需要的组件进行反序列化操作，抹平CPU消耗尖峰
            Entity t = MongoHelper.Deserialize(type, bs) as Entity;
            unit.AddComponent(t);
            
            await ETTask.CompletedTask;
        }
    }
}

