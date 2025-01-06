
namespace ET
{
    public static partial class CoroutineLockType
    {
        public const int LoginAccount = PackageType.Login * 1000 + 1;

        public const int LoginCenterLock = PackageType.Login * 1000 + 2;

        public const int LoginGate = PackageType.Login * 1000 + 3;

        public const int CreateRole = PackageType.Login * 1000 + 4;
    }
}