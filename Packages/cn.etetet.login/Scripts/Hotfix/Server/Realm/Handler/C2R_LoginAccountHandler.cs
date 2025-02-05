using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(Account))]
    public class C2R_LoginAccountHandler : MessageSessionHandler<C2R_LoginAccount, R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response)
        {
            // SessionAcceptTimoutComponent - 5s后自动断开连接（防外挂），因为该Handler继承的是MessageSessionHandler，
            // 所以能走到这里就说明Session验证成功了，可以删除该组件了。
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            // 判断当前是否锁住（下面的代码里有加锁逻辑）
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session.Disconnect().NoContext();
                return;
            }
            
            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNull;
                session.Disconnect().NoContext();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                session.Disconnect().NoContext();
                return;
            }

            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_PasswordFormError;
                session.Disconnect().NoContext();
                return;
            }


            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            // 加锁，避免重复的登陆请求（防挂）
            using (session.AddComponent<SessionLockingComponent>())
            {
                // 账号登陆锁，同一时间只能有一个同名账号执行db操作
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount, request.AccountName.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());

                    List<Account> accountInfoList = await dbComponent.Query<Account>(d => d.AccountName.Equals(request.AccountName));
                    Account account = null;
                    
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        // 实体必须有上下文，所以我们必须讲Account实体挂载到Session这个父实体上（加入了实体树中），这样才可以对服务端所有对象进行可控的管理。
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            session.Disconnect().NoContext();
                            account?.Dispose();
                            return;
                        }


                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            session.Disconnect().NoContext();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password    = request.Password;
                        account.CreateTime  = TimeInfo.Instance.ServerNow();
                        account.AccountType = (int)AccountType.General;
                        await dbComponent.Save<Account>(account);
                    }
                    
                    
                    R2L_LoginAccountRequest r2LLoginAccountRequest = R2L_LoginAccountRequest.Create();
                    r2LLoginAccountRequest.AccountName = request.AccountName;

                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.GetBySceneName(1000,"LoginCenter");
                    var loginAccountResponse =  await session.Fiber().Root.GetComponent<MessageSender>()
                                               .Call(loginCenterConfig.ActorId, r2LLoginAccountRequest) as L2R_LoginAccountRequest;
                    
                    if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = loginAccountResponse.Error;
                        session?.Disconnect().NoContext();
                        account?.Dispose();
                        return;
                    }

                    // 顶号登录校验
                    // otherSession == 当前在使用该账号的玩家所对应的Session
                    Session otherSession  = session.Root().GetComponent<AccountSessionsComponent>().Get(request.AccountName);
                    // 若otherSession有内容，说明当前有人在使用这个号，我们要用这个Session发个断线通知给当前在线的用户。
                    otherSession?.Send( A2C_Disconnect.Create());
                    otherSession?.Disconnect().NoContext();
                    // Temp: 这一行在视频中有添加，但是Account业务包里没有，我后来加上了，观察一阵再说
                    session.Root().GetComponent<AccountSessionsComponent>().Add(request.AccountName, session);
                    // 
                    session.AddComponent<AccountCheckOutTimeComponent, string>(request.AccountName);
                    // 更新原本的AccountName-Session映射关系
                    string Token = TimeInfo.Instance.ServerNow().ToString() + RandomGenerator.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.Root().GetComponent<TokenComponent>().Remove(request.AccountName);
                    session.Root().GetComponent<TokenComponent>().Add(request.AccountName, Token);
                    
                    response.Token = Token;
                    
                    account?.Dispose();
                }
            }
        }
    }
}