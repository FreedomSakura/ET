namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_RequestRepeatedly = ERR_WithException + PackageType.Login * 1000 + 1;

        public const int ERR_LoginInfoIsNull = ERR_WithException + PackageType.Login * 1000 + 2;

        public const int ERR_AccountNameFormError = ERR_WithException + PackageType.Login * 1000 + 3;

        public const int ERR_PasswordFormError = ERR_WithException + PackageType.Login * 1000 + 4;

        public const int ERR_AccountInBlackListError = ERR_WithException + PackageType.Login * 1000 + 5;

        public const int ERR_LoginPasswordError = ERR_WithException + PackageType.Login * 1000 + 6;


        public const int ERR_OtherAccountLogin = ERR_WithException + PackageType.Login * 1000 + 7;
        
        public const int ERR_LoginGameGateError01 = ERR_WithException + PackageType.Login * 1000 + 8;

        public const int ERR_SessionPlayerError = ERR_WithException + PackageType.Login * 1000 + 9;

        public const int ERR_NonePlayerError = ERR_WithException + PackageType.Login * 1000 + 10;

        public const int ERR_ReEnterGameError2 = ERR_WithException + PackageType.Login * 1000 + 11;

        public const int ERR_PlayerSessionError = ERR_WithException + PackageType.Login * 1000 + 12;


        public const int ERR_ReEnterGameError = ERR_WithException + PackageType.Login * 1000 + 13;

        public const int ERR_EnterGameError = ERR_WithException + PackageType.Login * 1000 + 14;


        public const int ERR_TokenError = ERR_WithException + PackageType.Login * 1000 + 15;

        public const int ERR_RoleNameIsNull = ERR_WithException + PackageType.Login * 1000 + 16;

        public const int ERR_RoleNameSame = ERR_WithException + PackageType.Login * 1000 + 17;

        public const int ERR_RoleNotExist = ERR_WithException + PackageType.Login * 1000 + 18;
        
        
        public const int ERR_ConnectGateKeyError = ERR_WithException + PackageType.Login * 1000 + 21;

    }
}