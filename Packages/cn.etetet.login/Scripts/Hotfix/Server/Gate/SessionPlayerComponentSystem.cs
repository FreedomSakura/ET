namespace ET.Server
{
    [EntitySystemOf(typeof(SessionPlayerComponent))]
    public static partial class SessionPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionPlayerComponent self)
        {
            Scene root = self.Root();
            if (root.IsDisposed)
            {
                return;
            }
            // 发送断线消息 因为使用统一下线流程，故注释
            //root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(self.Player.Id, G2M_SessionDisconnect.Create());
            
            Player player = self.Player;
            if (player != null && self.GetParent<Session>().IsDisposed)
            {
                //如果是服务器主动跟客户端断开，要先移除SessionPlayerComponent，再销毁Session，否则就认为是突然断开了
                //Session突然断开，一段时间后没重连就下线
                Log.Console("Session断开,进入OfflineOutTime流程");
                player.AddComponent<PlayerOfflineOutTimeComponent>();
            }
            
            
            self.Player = null;
        }
        
        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self)
        {

        }
    }
}