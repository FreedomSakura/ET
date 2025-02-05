using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent : Entity,IAwake,IDestroy
    {
        // k: AccountName, v: Session - 用户名只对应为唯一的登录Session
        public Dictionary<string, EntityRef<Session>> AccountSessionDictionary = new Dictionary<string, EntityRef<Session>>();
    }
}