namespace ET
{
    public static partial class TimerInvokeType
    {
        public const int SessionIdleChecker = PackageType.Login * 1000 + 1;
        public const int SessionAcceptTimeout = PackageType.Login * 1000 + 2;
        public const int AccountSessionCheckOutTime = PackageType.Login * 1000 + 3;

        public const int PlayerOfflineOutTime = PackageType.Login * 1000 + 4;
    }
}