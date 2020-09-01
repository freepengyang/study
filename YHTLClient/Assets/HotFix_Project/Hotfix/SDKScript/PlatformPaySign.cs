using UnityEngine;
using System.Collections;
using System;
using TABLE;
using System.Security.Cryptography;
using System.Text;

public interface IPlatformPaySign
{
    int mChanelId { get; }

    WWWForm GetForm(TABLE.RECHARGE tb);

    string getUrl { get; }

    string queryPaySignCpOrderID { get; set; }

    long createTime { get; set; }
}

public class UCQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1108; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //string loginName = CSConstant.loginName;
        //from.AddField("accountId", loginName.Replace("1108:", ""));
        //from.AddField("amount", tb.qian.ToString() + ".00");
        //from.AddField("notifyUrl", getUrl);
        //string ext = string.Format("{0}:{1}:{2}",Constant.mOnlyServerId,CSAvatarManager.MainPlayerInfo.ID,tb.id);
        //from.AddField("callbackInfo", ext);
        //from.AddField("cpOrderId", queryPaySignCpOrderID);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class ViVoQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1103; } }

    public string queryPaySignCpOrderID { get; set; }
    public long createTime { get; set; }



    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();

        //TODO:ddn
        //if (tb == null) return from;
        //from.AddField("appId", 101967396);
        //from.AddField("cpOrderNumber", queryPaySignCpOrderID);
        //string p_name = Utility.StringConvertCurrencyExchange((tb.qian * 100).ToString()) + CSString.Format(100969);
        //string p_desc = CSString.Format(100969);
        //from.AddField("productDesc", p_desc);
        //from.AddField("productName", p_name);
        //from.AddField("orderAmount", (tb.qian * 100).ToString());
        //from.AddField("notifyUrl", getUrl);
        //string info = string.Format("{0}:{1}:{2}:{3}",Constant.mOnlyServerId,CSAvatarManager.MainPlayerInfo.ID, tb.id,Constant.platformid);
        //from.AddField("extInfo", info);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class MeiZuQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1104; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //string loginName = CSConstant.loginName;
        //from.AddField("cp_order_id", queryPaySignCpOrderID);
        //from.AddField("uid", loginName.Replace("1104:", ""));
        //from.AddField("product_id", tb.id.ToString());//CP 游戏道具ID
        //from.AddField("product_subject", Utility.StringConvertCurrencyExchange((tb.qian * 100).ToString()) + "金子");//dingdan标题
        //from.AddField("product_body", Utility.StringConvertCurrencyExchange((tb.qian * 100).ToString()) + "金子");//dingdan标题
        //from.AddField("product_unit", "个");//游戏道具的单位
        //from.AddField("buy_amount", 1);//道具购买的数量
        //from.AddField("product_per_price", tb.qian.ToString());//游戏道具单价
        //from.AddField("total_price", tb.qian.ToString());//总金额
        //from.AddField("create_time", createTime.ToString());//创建时间戳
        //from.AddField("pay_type", 0);//zhifu方式
        //long roleId = CSScene.Sington.MainPlayer.Info.ID;
        //string str = String.Format("{0}:{1}:{2}", Constant.mOnlyServerId, roleId, tb.id);
        //from.AddField("user_info", str);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class HuaWeiQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1101; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        if (tb == null) return from;
        from.AddField("amount", tb.Money.ToString() + ".00");
        from.AddField("productName", tb.Money.ToString() + "金子");
        from.AddField("productDesc", tb.Money.ToString() + "金子");
        from.AddField("sdkChannel", "3");
        from.AddField("currency", "CNY");
        from.AddField("country", "CN");
        from.AddField("urlver", "2");
        from.AddField("merchantId", "890086200102368479");
        from.AddField("applicationID", "101068201");
        from.AddField("requestId", queryPaySignCpOrderID);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class SanXingQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1105; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //string loginName = CSConstant.loginName;
        //from.AddField("appuserid", loginName.Replace("1105:", ""));
        //from.AddField("waresid", tb.id.ToString());
        //from.AddField("cpOrderId", queryPaySignCpOrderID);
        //from.AddField("price", tb.qian.ToString());
        //from.AddField("currency",  "RMB");
        //string info = string.Format("{0}:{1}:{2}", Constant.mOnlyServerId,CSAvatarManager.MainPlayerInfo.ID, tb.id);
        //from.AddField("cpprivateinfo", info);
        //from.AddField("notifyUrl", getUrl);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class XiaoAnQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1024; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //string loginName = CSConstant.loginName;
        //from.AddField("uid", loginName.Replace("1024:", ""));
        //from.AddField("cpOrderId", queryPaySignCpOrderID);
        //from.AddField("roleId",CSAvatarManager.MainPlayerInfo.ID.ToString());
        //from.AddField("zoneId", "1");
        //from.AddField("amount", (tb.qian * 100).ToString());
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}

public class YiXinAnQuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1017; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //from.AddField("v", "2.9.0");
        //string info = string.Format("{0}:{1}:{2}:{3}", queryPaySignCpOrderID,Constant.mOnlyServerId,CSAvatarManager.MainPlayerInfo.ID, tb.id);
        //from.AddField("thirdpart_orderid", info);
        //from.AddField("thirdpart_ordertime", createTime.ToString());
        //from.AddField("tradeName", "金子");
        //from.AddField("price", tb.qian.ToString()+".00");
        //from.AddField("access_token", Constant.mToken);
        //from.AddField("goodscount", 1);
        return from;
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}
public class G4399QuDao : IPlatformPaySign
{
    public int mChanelId { get { return 1110; } }

    public string queryPaySignCpOrderID { get; set; }

    public long createTime { get; set; }

    public WWWForm GetForm(TABLE.RECHARGE tb)
    {
        WWWForm from = new WWWForm();
        //TODO:ddn
        //if (tb == null) return from;
        //from.AddField("roleId",CSAvatarManager.MainPlayerInfo.ID.ToString());
        //from.AddField("goodId", tb.id.ToString());
        //string a = string.Format("{0}{1}VUK589ficFCsOdXN",CSAvatarManager.MainPlayerInfo.ID, tb.id);
        //string b = SHA1_Encrypt(a);
        //from.AddField("sign", b);
        return from;
    }

    /// 对字符串进行SHA1加密
    /// </summary>
    /// <param name="strIN">需要加密的字符串</param>
    /// <returns>密文</returns>
    public static string SHA1_Encrypt(string Source_String)
    {
        byte[] StrRes = Encoding.Default.GetBytes(Source_String);
        HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
        StrRes = iSHA.ComputeHash(StrRes);
        StringBuilder EnText = new StringBuilder();
        foreach (byte iByte in StrRes)
        {
            EnText.AppendFormat("{0:x2}", iByte);
        }
        return EnText.ToString().ToUpper();
    }

    public string getUrl
    {
        get { return string.Format("http://cz.zt.17tanwan.com:3000/recharge/{0}", Constant.platformid); }
    }
}



