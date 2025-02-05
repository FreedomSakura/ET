namespace ET.Server
{
    /// <summary>
    /// 定时断开无用Session
    /// </summary>
    [ComponentOf(typeof(Session))]
    public class AccountCheckOutTimeComponent: Entity,IAwake<string>,IDestroy
    {
        public long Timer = 0;

        public string Account;
    }
}