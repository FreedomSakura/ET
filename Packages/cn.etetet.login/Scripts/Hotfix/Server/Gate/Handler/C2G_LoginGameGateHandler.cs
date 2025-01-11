using System;


namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_LoginGameGateHandler : MessageSessionHandler<C2G_LoginGameGate, G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response)
        {
            Scene root = session.Root();
            
            if ( session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                return;
            }
            
            string account = root.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            if (account == null)
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                session?.Disconnect().NoContext();
                return;
            }
            
            root.GetComponent<GateSessionKeyComponent>().Remove(request.Key);
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            CoroutineLockComponent coroutineLockComponent = root.GetComponent<CoroutineLockComponent>();
            
            long instanceId = session.InstanceId;
            using (session.AddComponent<SessionLockingComponent>())
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, request.AccountName.GetLongHashCode()))
            {
                if (instanceId != session.InstanceId)
                {
                    response.Error = ErrorCode.ERR_LoginGameGateError01;
                    return;
                }

                //通知登录中心服 记录本次登录的服务器Zone
                G2L_AddLoginRecord g2LAddLoginRecord = G2L_AddLoginRecord.Create();

                g2LAddLoginRecord.AccountName = request.AccountName;
                g2LAddLoginRecord.ServerId    = root.Zone();

                StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(1000,"LoginCenter");
                
                L2G_AddLoginRecord l2ARoleLogin = (L2G_AddLoginRecord) await root.GetComponent<MessageSender>().Call(startSceneConfig.ActorId, g2LAddLoginRecord);
				
                if (l2ARoleLogin.Error != ErrorCode.ERR_Success)
                {
                    response.Error = l2ARoleLogin.Error;
                    session?.Disconnect().NoContext();
                    return;
                }
                
                PlayerComponent playerComponent = root.GetComponent<PlayerComponent>();
                Player player = playerComponent.GetByAccount(account);
                if (player == null)
                {
                    player = playerComponent.AddChildWithId<Player, string>(request.RoleId,account);
                    player.UnitId = request.RoleId;
                    
                    playerComponent.Add(player);
                    PlayerSessionComponent playerSessionComponent = player.AddComponent<PlayerSessionComponent>();
                    playerSessionComponent.AddComponent<MailBoxComponent, int>(MailBoxType.GateSession);
                    await playerSessionComponent.AddLocation(LocationType.GateSession);
			
                    player.AddComponent<MailBoxComponent, int>(MailBoxType.UnOrderedMessage);
                    await player.AddLocation(LocationType.Player);
			
                    session.AddComponent<SessionPlayerComponent>().Player = player;
                    playerSessionComponent.Session = session;

                    player.PlayerState = PlayerState.Gate;
                }
                else
                {
                    player.RemoveComponent<PlayerOfflineOutTimeComponent>();
                    
                    session.AddComponent<SessionPlayerComponent>().Player = player;
                    player.GetComponent<PlayerSessionComponent>().Session = session;
                }
                
                response.PlayerId = player.Id;
            }
            
        }
    }
}