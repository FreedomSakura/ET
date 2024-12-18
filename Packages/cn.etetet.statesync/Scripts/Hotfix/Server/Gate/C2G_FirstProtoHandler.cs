
namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_FirstProtoHandler : MessageSessionHandler<C2G_FirstProto, G2C_FirstProto>
    {
        protected override async ETTask Run(Session session, C2G_FirstProto request, G2C_FirstProto response)
        {
            Log.Console("响应了！G2C_FirstProto！@@@@");
            await ETTask.CompletedTask;
        }
    }
}

