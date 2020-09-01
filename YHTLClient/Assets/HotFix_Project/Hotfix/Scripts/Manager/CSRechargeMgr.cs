using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Main_Project.SDKScript.SDK;
using UnityEngine.Networking;

public class CSRechargeMgr : CSSingOn<CSRechargeMgr>
{
    //string queryPaySignCpIndentID;

    //public Dictionary<int, IPlatformPaySign> mPaySignDic = new Dictionary<int, IPlatformPaySign>();

    // public CSRechargeMgr()
    // {
    //     mPaySignDic.Add(1108, new UCQuDao());
    //     mPaySignDic.Add(1103, new ViVoQuDao());
    //     mPaySignDic.Add(1104, new MeiZuQuDao());
    //     mPaySignDic.Add(1101, new HuaWeiQuDao());
    //     mPaySignDic.Add(1105, new SanXingQuDao());
    //     mPaySignDic.Add(1024, new XiaoAnQuDao());
    //     mPaySignDic.Add(1110, new G4399QuDao());
    //     mPaySignDic.Add(1017, new YiXinAnQuDao());
    // }

    public IEnumerator queryPaySign(TABLE.RECHARGE tb)
    {
        if (tb == null) yield break;
        WWWForm from = null;
        //IPlatformPaySign quDao = null;
        long createTime = 0;

        //queryPaySignCpIndentID = GetIndentId(tb.id);
        createTime = CSServerTime.Instance.TotalMillisecond;
        /*if (mPaySignDic.ContainsKey(Constant.platformid))
        {
            quDao = mPaySignDic[Constant.platformid];
            quDao.queryPaySignCpOrderID = queryPaySignCpIndentID;
            quDao.createTime = createTime;
            from = quDao.GetForm(tb);
        }
        else
        {
            
        }*/
        from = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(AppUrl.GetPaySignUrl, from);

        yield return www.SendWebRequest();

        if (www.error == null && !string.IsNullOrEmpty(www.downloadHandler.text))
        {
            QuDaoInterface.Instance.FuKuan(payParams(tb.money, tb.id, www.downloadHandler.text, createTime));
        }
    }

    public QuDaoPayParams payParams(int count, int indentid, string sign = "", long createTime = 0)
    {
        QuDaoPayParams data = new QuDaoPayParams();
        string[] qudaoUids = CSConstant.loginName.Split(':');
        string qudaoUid = qudaoUids.Length > 1 ? qudaoUids[1] : CSConstant.loginName;
        data.app_Ext1 = CSConstant.mOnlyServerId.ToString();
        data.amount = count.ToString();
        data.product_name = count * 100 + CSString.Format(1628);
        data.product_desc = CSString.Format(1628);
        data.notify_Uri = AppUrlMain.rechargeUrl;
        data.app_user_Name = CSMainPlayerInfo.Instance.Name;
        data.product_Id = indentid.ToString();

        int plantFormId = QuDaoConstant.GetPlatformData().platformID;
        data.sid = CSConstant.mSeverId.ToString();
        data.serverName = Constant.mServerName.ToString();
        data.vipLevel = CSMainPlayerInfo.Instance.VipLevel.ToString();
        data.roleLevel = CSMainPlayerInfo.Instance.Level.ToString();
        data.UnionName = CSMainPlayerInfo.Instance.GuildId == 0 ? "" : CSMainPlayerInfo.Instance.GuildName;
        data.CreateTime = createTime; //
        /*if (mPaySignDic.ContainsKey(Constant.platformid))
        {
            data.app_order_Id = queryPaySignCpIndentID;
        }
        else
        {*/
        data.app_order_Id = GetIndentId(indentid);
        //}

        data.app_User_Id = qudaoUid;
        data.balance = CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao).ToString();
        data.game_Role_Id = CSMainPlayerInfo.Instance.ID.ToString();
        data.sign = sign;
        return data;
    }

    public QuDaoPayParams IOSPayParams(int count, int indentid, string sign = "", long createTime = 0)
    {
        QuDaoPayParams data = new QuDaoPayParams();

        data.amount = count.ToString();
        data.app_Ext1 = CSConstant.mOnlyServerId.ToString();
        string[] qudaoUids = CSConstant.loginName.Split(':');
        string qudaoUid = qudaoUids.Length > 1 ? qudaoUids[1] : CSConstant.loginName;
        data.app_User_Id = qudaoUid;
        data.app_order_Id = GetIndentId(indentid);
        data.game_Role_Id = CSMainPlayerInfo.Instance.ID.ToString();
        data.product_name = CSString.Format(1628);
        data.notify_Uri = AppUrlMain.rechargeUrl;
        data.app_user_Name = CSMainPlayerInfo.Instance.Name;
        data.product_Id = indentid.ToString();
        data.sid = CSConstant.mSeverId.ToString();
        data.serverName = Constant.mServerName.ToString();
        data.vipLevel = CSMainPlayerInfo.Instance.VipLevel.ToString();
        data.roleLevel = CSMainPlayerInfo.Instance.Level.ToString();
        data.UnionName = CSMainPlayerInfo.Instance.GuildId == 0 ? "" : CSMainPlayerInfo.Instance.GuildName;
        data.CreateTime = createTime;
        data.sign = sign;
        data.remainder = CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao).ToString();
        return data;
    }

    public string GetIndentId(int indentid)
    {
        return CSMainPlayerInfo.Instance.ID.ToString() + ":" + indentid.ToString() + ":" +
               CSServerTime.Instance.TotalSeconds.ToString();
    }
}