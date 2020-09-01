namespace Main_Project.SDKScript.SDK
{
    /// <summary>
    ///     SDK购买数据收集类
    /// </summary>
    public class BuyItemInfo
    {
        public int coinCount { get; set; } //银子数量

        public int bindCoinCount { get; set; } //绑定银子数量

        public int reminCoinCount { get; set; } //剩余银子数量

        public int reminBindCounCount { get; set; } //剩余绑定数量

        public int buyItemCount { get; set; } //购买道具数量

        public string itemName { get; set; }

        public string itemDesc { get; set; }

        //角色ID
        public long roleID { get; set; }

        //角色名称
        public string roleName { get; set; }

        //服务器ID
        public int serverID { get; set; }
    }
}