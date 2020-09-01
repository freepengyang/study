namespace Main_Project.SDKScript.SDK
{
    /// <summary>
    /// zhifu接口需要的参数
    /// </summary>
    public class QuDaoPayParams
    {
        public string app_User_Id { get; set; }

        public string game_Role_Id { get; set; }

        public string notify_Uri { get; set; }

        public string amount { get; set; }

        public string app_Ext1 { get; set; }

        public string app_Ext2 { get; set; }

        public string app_name { get; set; }

        public string app_order_Id { get; set; } //dingdan号

        public string app_user_Name { get; set; }

        public string product_Id { get; set; }

        public string sid { get; set; }

        public string serverName { get; set; }

        public string product_name { get; set; }

        public string product_desc { get; set; }

        public string vipLevel { get; set; }

        public string roleLevel { set; get; }

        public string UnionName { set; get; }

        public long CreateTime { set; get; }

        public string balance { set; get; }

        //部分渠道需要从服务器中获得sign,再传入sdk
        public string sign { get; set; }

        public int district { get; set; } //区id

        public string remainder { get; set; }
    }
}