namespace ET.Server
{
    [Invoke(SceneType.UnitCache)]
    public class AddToBytesInvoke_Cache : AInvokeHandler<AddToBytes>
    {
        public override void Handle(AddToBytes args)
        {
            Unit unit = args.Unit;
            unit?.GetComponent<UnitDBSaveComponent>().AddToBytes(args.Type, args.Bytes);
        }
    }
}