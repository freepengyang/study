namespace Main_Project.SDKScript.SDK
{
    /// <summary>
    ///     SDK 登录结果
    /// </summary>
    public class LoginResult
    {
        public int code { get; set; }

        public string message { get; set; }

        public string openUid { get; set; }

        public bool isBindPhone { get; set; }
    }
}