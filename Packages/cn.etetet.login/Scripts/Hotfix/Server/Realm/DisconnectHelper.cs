namespace ET.Server
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            TimerComponent timerComponent = self.Root().GetComponent<TimerComponent>();
            await timerComponent.WaitAsync(1000);

            if (self.InstanceId != instanceId)
            {
                return;
            }
            self.Dispose();
        }

        public static async ETTask KickPlayerNoLock(Player player)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            
            switch (player.PlayerState)
            {
                case PlayerState.Disconnect:
                    break;
                case PlayerState.Gate:
                    break;
                case PlayerState.Game:
                    
                    //通知Map游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                    var m2GRequestExitGame = (M2G_RequestExitGame)await player.Root().GetComponent<MessageLocationSenderComponent>()
                                                                            .Get(LocationType.Unit).Call(player.UnitId, G2M_RequestExitGame.Create());
                    if (m2GRequestExitGame.Error != ErrorCode.ERR_Success)
                    {
                        Log.Error($"离开Map游戏逻辑服时发生错误 : {m2GRequestExitGame.Error}");
                    }
                    
                    //通知邮件服下线MailUnit
                    Mail2G_ExistMailServer mail2GExistMailServer = (Mail2G_ExistMailServer)await player.Root().GetComponent<MessageLocationSenderComponent>()
                                                                            .Get(LocationType.Mail).Call(player.UnitId,  G2Mail_ExistMailServer.Create());
                    if (mail2GExistMailServer.Error != ErrorCode.ERR_Success)
                    {
                        Log.Error($"离开邮件中心服时发生错误 : {mail2GExistMailServer.Error}");
                    }
                    player.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Mail)?.Remove(player.UnitId);

                    
                    //通知登录中心服移除账号角色登录信息
                    G2L_RemoveLoginRecord g2LRemoveLoginRecord = G2L_RemoveLoginRecord.Create();
                    g2LRemoveLoginRecord.AccountName = player.Account;
                    g2LRemoveLoginRecord.ServerId    = player.Zone();
                    
                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.GetBySceneName(1000,"LoginCenter");
                    var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord) await player.Root().GetComponent<MessageSender>().Call(loginCenterConfig.ActorId, g2LRemoveLoginRecord);
                    if (L2G_RemoveLoginRecord.Error != ErrorCode.ERR_Success)
                    {
                        Log.Error($"通知登陆中心服时发生错误 : {L2G_RemoveLoginRecord.Error}");
                    }
                    
                    break;
            }
            
            TimerComponent timerComponent = player.Root().GetComponent<TimerComponent>();
            player.PlayerState = PlayerState.Disconnect;

            await player.GetComponent<PlayerSessionComponent>().RemoveLocation(LocationType.GateSession);
            await player.RemoveLocation(LocationType.Player);
            
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.GateSession)?.Remove(player.UnitId);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Unit)?.Remove(player.UnitId);
            
            player.Root().GetComponent<PlayerComponent>()?.Remove(player);
            player?.Dispose();
            
            await timerComponent.WaitAsync(300);
        }
        
        
        public static async ETTask KickPlayer(Player player)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            long instanceId = player.InstanceId;

            CoroutineLockComponent coroutineLockComponent = player.Root().GetComponent<CoroutineLockComponent>();
            
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, player.Account.GetLongHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                {
                    return;
                }
                await KickPlayerNoLock(player);
            }
        }
    }
}