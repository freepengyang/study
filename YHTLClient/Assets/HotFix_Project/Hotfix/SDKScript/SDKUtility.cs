using Main_Project.SDKScript.SDK;

public class SDKUtility
{
    /// <summary>
    /// 玩家数据收集
    /// </summary>
    /// <param name="extraType">1;//选择服务器,2;//创建角色,3;//进入游戏,4;//等级提升,5;//退出游戏,6:元宝到账</param>
    public static void SubmitGameData(int extraType)
    {
        //TODO:ddn 
        try
        {
            string[] sdkUids = CSConstant.loginName.Split(':');
            string sdkUid = "";
            if (sdkUids.Length > 1)
                sdkUid = sdkUids[1];
            else
                sdkUid = CSConstant.loginName;

            ExtraGameData gameData = new ExtraGameData();

            gameData.dataType = extraType;
            gameData.userID = sdkUid;
            gameData.guildName = CSMainPlayerInfo.Instance.GuildName;
            gameData.moneyNum = (int)CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao);
            gameData.roleID = CSMainPlayerInfo.Instance.ID.ToString();
            gameData.roleName = CSMainPlayerInfo.Instance.Name;
            gameData.serverID = CSConstant.mSeverId;
            gameData.roleLevel = CSMainPlayerInfo.Instance.Level.ToString();
            gameData.serverName = Constant.mServerName;
            gameData.vipLevel = CSMainPlayerInfo.Instance.VipLevel;
            gameData.vipExp = CSMainPlayerInfo.Instance.RoleExtraValues.vipExp;
            gameData.updateLevelTime = CSMainPlayerInfo.Instance.RoleExtraValues.updateLevelTime;
            gameData.guildLevel = "0";
            gameData.guildID = CSMainPlayerInfo.Instance.GuildId == 0
                ? "0"
                : CSMainPlayerInfo.Instance.GuildId.ToString();
            gameData.guildLeaderName = "";
            gameData.rolePower = CSMainPlayerInfo.Instance.fightPower;
            gameData.professionID = CSMainPlayerInfo.Instance.Career;
            gameData.profession = Utility.GetCareerName(CSMainPlayerInfo.Instance.Career);
            gameData.professionRoleName = "无";
            gameData.sex = CSMainPlayerInfo.Instance.Sex == 0 ? "女" : "男";
            if (CSMainPlayerInfo.Instance.RoleExtraValues != null)
                gameData.createRoleTime = CSMainPlayerInfo.Instance.RoleExtraValues.createTime;
            gameData.guildName = CSMainPlayerInfo.Instance.GuildId == 0 ? "无" : CSMainPlayerInfo.Instance.GuildName;
            QuDaoInterface.Instance.SubmitGameData(extraType, gameData);
        }
        catch
        {
            FNDebug.LogError("SubmitGameData error");
        }
    }
    
    public static void Pay(QuDaoPayParams data)
    {
        //TODO:ddn
        if (!QuDaoConstant.OpenRecharge)
        {
            UtilityTips.ShowRedTips(1298);
            return;
        }
        QuDaoInterface.Instance.FuKuan(data);
    }
}