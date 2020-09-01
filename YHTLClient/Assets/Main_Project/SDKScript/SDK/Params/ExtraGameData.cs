namespace Main_Project.SDKScript.SDK
{
    public class ExtraGameData
    {
        public const int TYPE_SELECT_SERVER = 1; //选择服务器
        public const int TYPE_CREATE_ROLE = 2; //创建角色
        public const int TYPE_ENTER_GAME = 3; //进入游戏
        public const int TYPE_LEVEL_UP = 4; //等级提升
        public const int TYPE_EXIT_GAME = 5; //退出游戏

        //调用时机，设置为上面定义的类型，在各个对应的地方调用submitGameData方法
        public int dataType { get; set; }

        public string userID { get; set; }

        //角色ID
        public string roleID { get; set; }

        //角色名称
        public string roleName { get; set; }

        //角色等级
        public string roleLevel { get; set; }

        //服务器ID
        public int serverID { get; set; }

        //服务器名称
        public string serverName { get; set; }

        //当前角色生成拥有的虚拟币数量
        public int moneyNum { get; set; }

        public int vipLevel { get; set; } //VIP等级

        public int vipExp { get; set; } //VIP经验

        public string guildName { get; set; } //家族名称

        public string guildID { get; set; } //家族ID

        public string guildLevel { get; set; } //家族等级

        public string guildLeaderName { get; set; } //会长名称

        public int rolePower { get; set; } //角色战斗力

        public int professionID { get; set; } //职业ID

        public string profession { get; set; } //职业

        public string professionRoleName { get; set; } //职业称号

        public string sex { get; set; } //性别

        public string friendList { set; get; } //好友列表

        public long createRoleTime { get; set; } //创建角色时间

        public long updateLevelTime { get; set; } //玩家升级时间

        public string remainder { get; set; } //玩家剩余金子

        public string mainTaskId { get; set; } //玩家主线任务进度
    }
}