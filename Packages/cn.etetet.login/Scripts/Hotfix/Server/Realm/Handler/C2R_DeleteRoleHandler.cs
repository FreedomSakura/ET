namespace ET.Server
{
    [FriendOfAttribute(typeof(ET.RoleInfo))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_DeleteRoleHandler : MessageSessionHandler<C2R_DeleteRole, R2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2R_DeleteRole request, R2C_DeleteRole response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session.Disconnect().NoContext();
                return;
            }

            string token = session.Root().GetComponent<TokenComponent>().Get(request.Account);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;

                session?.Disconnect().NoContext();
                return;
            }


            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.CreateRole, request.Account.GetLongHashCode()))
                {

                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());

                    var roleInfos = await dbComponent.Query<RoleInfo>(d => d.Id == request.RoleInfoId && d.ServerId == request.ServerId);
                    
                    if (roleInfos == null || roleInfos.Count <= 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExist;
                        return;
                    }

                    var roleInfo = roleInfos[0];
                    // 实体必须挂载实体树上，防止内存泄漏
                    session.AddChild(roleInfo);
                    // 设置状态而不是删除，方便找回角色
                    roleInfo.State = (int)RoleInfoState.Freeze;

                    await dbComponent.Save(roleInfo);
                    response.DeletedRoleInfoId = roleInfo.Id;
                    roleInfo?.Dispose();
                }
            }

        }
    }
}