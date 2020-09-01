using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class CSSuperLink : CSInfo<CSSuperLink>
{
    protected Dictionary<int, SuperLinkItem> mFunctionCBDic = new Dictionary<int,SuperLinkItem>((int)CSLinkFunction.CSLF_COUNT);
    protected Regex mRegReLink = new Regex("func:(\\d+):(\\S+)");

    public CSSuperLink()
    {
        Initialize();
        RegFrameLinkFilter();
    }

    public void Initialize()
    {
        RegisterFunction(CSLinkFunction.CSLF_MAIN_ROLE_LINK, "role:(\\d+):(\\w+):(\\d+):(\\d+):(\\d+):(\\d+):(\\d+):(\\d+):(\\d+):(\\d+)", OnClickPlayerName);
        RegisterFunction(CSLinkFunction.CSLF_LOCATION_LINK, "find:(\\d+):(\\d+):(\\d+):(\\w+)", OnClickFindPlayer);
        RegisterFunction(CSLinkFunction.CSLF_ITEM_LINK, "item:(\\d+):(\\S+)", OnClickItem);
        RegisterFunction(CSLinkFunction.CSLF_JOIN_VOICE_LINK, "voice:(\\d+)", OnClickJoinVoice);
        RegisterFunction(CSLinkFunction.CSLF_JOIN_TEAM, "team:(\\d+)", OnClickTeam);
        RegisterFunction(CSLinkFunction.CSLF_WORLDBOSS, "worldboss:(\\d+)", OnClickWorldBoss);
        RegisterFunction(CSLinkFunction.CSLF_MONTHCARD_MAP, "gamemodel:(\\d+)", OnClickMonthCardMap);
        RegisterFunction(CSLinkFunction.CSLF_PANEL_LINK, "panel:(\\d+)", OnClickPanel);
        RegisterFunction(CSLinkFunction.CSLF_DELIVER_LINK, "deliver:(\\d+)", OnClickDeliver);
        RegisterFunction(CSLinkFunction.CSLF_SKILL_DESCRIPTION_LINK, "skilldesc:(\\d+)", OnClickSkillDescrption);
        RegisterFunction(CSLinkFunction.CSLF_HANDBOOK, "handbook:(\\d+):(\\S+)", OnClickHandBook);
        RegisterFunction(CSLinkFunction.CSLF_ACTIVITY_TIP_LINK, "tip:(\\d+)", OnClickTipLink);
        RegisterFunction(CSLinkFunction.CSLF_STATIC_FUNCTION_LINK, "function:(\\w+):(\\w+)", OnLinkFunction);
    }

    Dictionary<int, Func<bool>> mFrameLinkFilters = new Dictionary<int, Func<bool>>(32);
    public void RegFrameLinkFilter()
    {
        mFrameLinkFilters.Add(21000,CSVipInfo.Instance.IsFinishRechargeFirst);
    }

    protected void RegisterFunction(CSLinkFunction id,string pattern,System.Func<GroupCollection,bool> function)
    {
        if(mFunctionCBDic.ContainsKey((int)id))
        {
            FNDebug.LogErrorFormat("Function Already Registerd ... for {0}", id);
            return;
        }

        mFunctionCBDic.Add((int)id,new SuperLinkItem 
        {
            id = id,
            function = function,
            reg = new System.Text.RegularExpressions.Regex(pattern),
        });
    }

    /// <summary>
    /// 超链接 不用直接调用此接口，，，  UILabel.SetupLink();  即可
    /// </summary>
    /// <param name="content"></param>
    public bool Link(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        int id = 0;
        if(!mRegReLink.IsMatch(content))
        {
            return false;
        }
        FNDebug.LogFormat("link=>{0}", content);

        var match = mRegReLink.Match(content);
        if(!match.Success || !int.TryParse(match.Groups[1].Value,out id))
        {
            return false;
        }

        FNDebug.LogFormat("link=> id = {0}", (CSLinkFunction)id);
        SuperLinkItem function = null;
        if (mFunctionCBDic.ContainsKey(id))
            function = mFunctionCBDic[id];
        if (null == function)
            return false;

        if(!function.reg.IsMatch(match.Groups[2].Value))
        {
            return false;
        }

        if (null == function)
            return false;
        if (null == function.function)
            return false;

        return function.function.Invoke(function.reg.Match(match.Groups[2].Value).Groups);
    }

    protected bool OnClickPlayerName(GroupCollection collection)
    {
        if (collection.Count != 11)
            return false;

        long roleId = 0;
        if(!long.TryParse(collection[1].Value,out roleId))
        {
            return false;
        }

        if (CSMainPlayerInfo.Instance.ID == roleId)
        {
            UtilityTips.ShowRedTips(634);
            return false;
        }

        MenuInfo data = new MenuInfo();
        data.sundryId = 374;
        data.SetChatTips(roleId,
            collection[2].Value, 
            int.Parse(collection[3].Value), 
            int.Parse(collection[4].Value), 
            int.Parse(collection[5].Value), 
            int.Parse(collection[6].Value), 
            int.Parse(collection[7].Value), 
            int.Parse(collection[8].Value),
            int.Parse(collection[9].Value),
            int.Parse(collection[10].Value));
        UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
        {
            (f as UIRoleSelectionMenuPanel).ShowSelectData(data);
        });

        return true;
    }

    protected bool OnClickFindPlayer(GroupCollection collection)
    {
        int mapId = 0;
        if(!int.TryParse(collection[1].Value,out mapId))
        {
            return false;
        }
        int x = 0;
        if(!int.TryParse(collection[2].Value,out x))
        {
            return false;
        }
        int y = 0;
        if (!int.TryParse(collection[3].Value, out y))
        {
            return false;
        }
        string name = collection[4].Value;
        FNDebug.LogFormat("<color=#00ff00>[位置查询]=>[mapid:{0}]|[{1}({2},{3})]</color>", mapId, name,x,y);
        UtilityPath.FindPath(mapId, x, y);
        return true;
    }

    protected bool OnClickSkillDescrption(GroupCollection collection)
    {
        int clientTipId = 0;
        if (!int.TryParse(collection[1].Value, out clientTipId))
        {
            return false;
        }

        TABLE.CLIENTTIPS clientTipItem;
        if(!ClientTipsTableManager.Instance.TryGetValue(clientTipId,out clientTipItem))
        {
            return false;
        }

        UIManager.Instance.CreatePanel<UISkillDescriptionPanel>(f =>
        {
            (f as UISkillDescriptionPanel).Show(clientTipItem.context);
        });
        return true;
    }

    protected bool OnClickItem(GroupCollection collection)
    {
        long guid = 0;
        if(!long.TryParse(collection[1].Value,out guid))
        {
            return false;
        }
        string base64EncodeString = collection[2].Value;
        var bytes = System.Convert.FromBase64String(base64EncodeString);
        var bagItemInfo = CSProtoManager.Get<bag.BagItemInfo>();
        bagItemInfo.configId = 0;
        bagItemInfo.randAttrValues.Clear();
        bagItemInfo.longJis.Clear();
        bagItemInfo.baseAffixs.Clear();
        bagItemInfo.intensifyAffixs.Clear();
        using (Google.Protobuf.CodedInputStream stream = new Google.Protobuf.CodedInputStream(bytes))
        {
            bagItemInfo.MergeFrom(stream);
        }
        TABLE.ITEM item = null;
        if(!ItemTableManager.Instance.TryGetValue(bagItemInfo.configId, out item))
        {
            CSProtoManager.Recycle(bagItemInfo);
            return false;
        }
        FNDebug.LogFormat("<color=#00ff00>[物品链接]=>[guid:{0}]|[{1}]</color>", guid, item.name);
        CSProtoManager.Recycle(bagItemInfo);
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item, bagItemInfo);
        return true;
    }

    protected bool OnClickHandBook(GroupCollection collection)
    {
        long guid = 0;
        if (!long.TryParse(collection[1].Value, out guid))
        {
            return false;
        }
        string base64EncodeString = collection[2].Value;
        var bytes = System.Convert.FromBase64String(base64EncodeString);
        var bagItemInfo = CSProtoManager.Get<tujian.TujianInfo>();
        bagItemInfo.id = 0;
        bagItemInfo.handBookId = 0;
        bagItemInfo.slotId = 0;
        using (Google.Protobuf.CodedInputStream stream = new Google.Protobuf.CodedInputStream(bytes))
        {
            bagItemInfo.MergeFrom(stream);
        }
        TABLE.HANDBOOK handbookItem = null;
        if (!HandBookTableManager.Instance.TryGetValue(bagItemInfo.handBookId, out handbookItem))
        {
            CSProtoManager.Recycle(bagItemInfo);
            return false;
        }
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(handbookItem.itemID, out item))
        {
            CSProtoManager.Recycle(bagItemInfo);
            return false;
        }
        FNDebug.LogFormat("<color=#00ff00>[图鉴链接]=>[guid:{0}]|[{1}]</color>",guid,item.name);
        CSProtoManager.Recycle(bagItemInfo);
        UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
        {
            (f as UIHandBookTipsPanel).Show(bagItemInfo.handBookId, bagItemInfo.id, 1 << (int)UIHandBookTipsPanel.MenuType.MT_NO_MENU);
        });
        return true;
    }

    protected bool OnLinkFunction(GroupCollection collection)
    {
        if (string.IsNullOrEmpty(collection[1].Value))
            return false;
        var assembly = GetType().Assembly;
        if (null == assembly)
            return false;
        var type = assembly.GetType(collection[1].Value);
        if (null == type)
            return false;
        var method = type.GetMethod("Link", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        if (null == method)
            return false;

        try
        {
            if (!(method.Invoke(null, new object[] { collection[2].Value }) is bool succeed))
            {
                return false;
            }
        }
        catch(Exception e)
        {
            FNDebug.LogError($"[异常]:OnLinkFunction:[{collection[1].Value}:{collection[2].Value}]:{e.Message}");
        }

        return true;
    }

    protected bool OnClickTipLink(GroupCollection collection)
    {
        int tipId = 0;
        if (!int.TryParse(collection[1].Value, out tipId))
        {
            return false;
        }

        if(!SpecialActivityTipTableManager.Instance.TryGetValue(tipId,out TABLE.SPECIALACTIVITYTIP tipItem))
        {
            FNDebug.LogError($"id = {tipId} can not be found in SpecialActivityTipTableManager");
            return false;
        }

        if (!UICheckManager.Instance.DoCheckFunction(tipItem.funcopen))
        {
            UtilityTips.ShowRedTips(1949);
            return false;
        }

        return Link(tipItem.link);
    }

    protected bool OnClickJoinVoice(GroupCollection collection)
    {
        long loginType = 0;
        if (!long.TryParse(collection[1].Value, out loginType))
        {
            return false;
        }

        VoiceLoginType voiceLoginType = (VoiceLoginType)loginType;
        if (voiceLoginType != VoiceLoginType.team && voiceLoginType != VoiceLoginType.union)
        {
            FNDebug.LogErrorFormat("[语音链接]:登录类型错误:{0}", loginType);
            return false;
        }

        FNDebug.LogFormat("<color=#00ff00>[语音链接]=>[语音类型:{0}]</color>", voiceLoginType);
        if (!VoiceChatManager.Instance.isAllowYvVoice(true))
            return false;

        if (YvVoiceMgr.Instance.mLoginType == (int)voiceLoginType)
        {
            UtilityTips.ShowGreenTips(1857);
            return false;
        }

        VoiceChatManager.Instance.Login(voiceLoginType,()=>
        {
            if (YvVoiceMgr.Instance.mLoginType == (int)voiceLoginType)
                UtilityTips.ShowGreenTips(1847);
            else
                UtilityTips.ShowRedTips(334);
        });
        return true;
    }

    protected bool OnClickTeam(GroupCollection collection)
    {
        FNDebug.LogFormat("<color=#00ff00>[队伍链接]=>[NONE]</color>");
        long teamId = 0;
        if (!long.TryParse(collection[1].Value,out teamId))
            return false;
        Net.ReqApplyTeamMessage(teamId);
        return true;
    }
    protected bool OnClickWorldBoss(GroupCollection collection)
    {
        //Debug.Log(collection[1].Value);
        Net.ReqTransferByDeliverConfigMessage(6, false, 0, false, 0);
        //UtilityPath.FindNpc(int.Parse(collection[1].Value));
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
        return true;
    }

    protected bool OnClickMonthCardMap(GroupCollection collection)
    {
        int gameModelId = 0;
        if (!int.TryParse(collection[1].Value, out gameModelId))
            return false;
        UtilityPanel.JumpToPanel(gameModelId);
        return true;
    }

    protected bool OnClickPanel(GroupCollection collection)
    {
        FNDebug.LogFormat("<color=#00ff00>[界面链接]=>[frameId:{0}]</color>",collection[1].Value);
        int frameId = 0;
        if (!int.TryParse(collection[1].Value, out frameId))
        {
            FNDebug.LogError("<color=#00ff00>[界面链接]=>[解析frameId错误]</color>");
            return false;
        }

        if (!GameModelsTableManager.Instance.TryGetValue(frameId, out TABLE.GAMEMODELS gameModelItem))
            return false;

        if (gameModelItem.functionId != 0 && !UICheckManager.Instance.DoCheckFunction(gameModelItem.functionId))
            return false;

        if(OnFilter(frameId))
        {
            FNDebug.LogFormat("[界面链接过滤]:{0}", frameId);
            return false;
        }

        return UtilityPanel.JumpToPanel(frameId);
    }

    protected bool OnFilter(int frameId)
    {
        if (!mFrameLinkFilters.ContainsKey(frameId))
            return false;
        var filter = mFrameLinkFilters[frameId];
        if (null != filter && filter.Invoke())
            return true;
        return false;
    }

    protected bool OnClickDeliver(GroupCollection collection)
    {
        FNDebug.LogFormat("<color=#00ff00>[传送链接]=>[deliverId:{0}]</color>",collection[1].Value);
        int deliverId = 0;
        if (!int.TryParse(collection[1].Value, out deliverId))
        {
            FNDebug.LogError("<color=#00ff00>[传送链接]=>[解析deliverId错误]</color>");
            return false;
        }
        UtilityPath.FindWithDeliverId(deliverId);
        return true;
    }

    public override void Dispose()
    {
        mFunctionCBDic.Clear();
        mFunctionCBDic = null;
    }
}

public enum CSLinkFunction
{
    CSLF_NONE = 0,
    CSLF_MAIN_ROLE_LINK,//玩家链接 1
    CSLF_LOCATION_LINK,//位置链接 2
    CSLF_ITEM_LINK,//物品链接 3
    CSLF_JOIN_VOICE_LINK,//加入语音链接 4
    CSLF_JOIN_TEAM,//加入队伍 5
    CSLF_WORLDBOSS,//世界BOSS 6
    CSLF_MONTHCARD_MAP,//月卡 7
    CSLF_PANEL_LINK,//界面链接 8
    CSLF_DELIVER_LINK,//传送链接 9
    CSLF_SKILL_DESCRIPTION_LINK,//技能描述链接 10
    CSLF_HANDBOOK = 11,
    CSLF_ACTIVITY_TIP_LINK = 12,//活动链接
    CSLF_STATIC_FUNCTION_LINK = 13,//静态函数链接
    CSLF_COUNT,//枚举个数，永远在最后一个
}

public class SuperLinkItem
{
    public Regex reg;
    public CSLinkFunction id;
    public System.Func<GroupCollection,bool> function;
}