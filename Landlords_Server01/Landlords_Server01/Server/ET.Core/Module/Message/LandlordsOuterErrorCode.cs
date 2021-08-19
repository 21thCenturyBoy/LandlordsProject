namespace ETModel
{
    public static partial class ErrorCode
    {

        //---------------------自定义错误
        public const int ERR_AccountAlreadyRegisted = 300001;
        public const int ERR_RepeatedAccountExist = 300002;
        public const int ERR_UserNotOnline = 300003;
        public const int ERR_CreateNewCharacter = 300004;
        //---------------------

        public const int ERR_SignError = 10000;

        public const int ERR_Disconnect = 210000;
        public const int ERR_AccountAlreadyRegister = 210001;
        public const int ERR_JoinRoomError = 210002;
        public const int ERR_UserMoneyLessError = 210003;
        public const int ERR_PlayCardError = 210004;
        public const int ERR_LoginError = 210005;

        public const int ERR_GameContinueError = 210006;//游戏继续错误
    }
}