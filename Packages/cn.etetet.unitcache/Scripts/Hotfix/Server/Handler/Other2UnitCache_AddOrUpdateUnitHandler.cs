namespace ET.Server
{
    [MessageHandler(SceneType.UnitCache)]
    public class Other2UnitCache_AddOrUpdateUnitHandler : MessageHandler<Scene, Other2UnitCache_AddOrUpdateUnit, UnitCache2Other_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UpdateUnitCacheAsync(scene, request, response).NoContext();
            
            await ETTask.CompletedTask;
        }

        private async ETTask UpdateUnitCacheAsync(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            var unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            
            long unitId = request.UnitId;
            using (ListComponent<Entity> entityList = new ListComponent<Entity>())
            {
                for (int i = 0; i < request.EntityTypes.Count; ++i)
                {
                    Entity entity = MongoHelper.Deserialize<Entity>(request.EntityBytes[i]);
                    entityList.Add(entity);
                }
                
                await unitCacheComponent.AddorUpdate(unitId, entityList);
            }
        }
    }
}