namespace Main_Project.SDKScript.SDK
{
    public class LoginInfo
    {
        public string userId { get; set; }
        public string token { get; set; }
        public string sign { get; set; }
        public string ext { get; set; }
        public bool isBindPhone { get; set; }
        public string loginType { get; set; }
        public string time { get; set; }

        public bool haveWhitePaper { get; set; }

        public bool isWhitePaper { get; set; }

        public bool isError { get; set; }

        public string gid { get; set; }
    }

}