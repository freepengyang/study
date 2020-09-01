using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public class GuildFightItemData : IndexedItem
{
    public int Index { get; set; }
    public GuildFightActivity fightActivity;
    public SHACHENGYUANBAO item;
    public KEYVALUE normal = new KEYVALUE
    {
        key = (int)MoneyType.yuanbao,
        value = 0,
    };
    public SHACHENGYUANBAO extraItem;
    public KEYVALUE extra = new KEYVALUE
    {
        key = (int)MoneyType.yuanbao,
        value = 0,
    };
    public bool IsMain
    {
        get
        {
            var c = CSGuildFightManager.Instance.CurrentFight;
            if (null != c && null != fightActivity && c.times == fightActivity.times)
                return true;
            return false;
        }
    }
}

public class GuildFightAwardItemBinder : UIBinder
{
    protected GuildFightItemData mData;
    UILabel lb_title;
    UILabel lb_desc;
    GameObject go_ap;
    GameObject go_bp;
    UISprite sp_asp;
    UISprite sp_bsp;
    UIItemBase itemBaseA;
    UIItemBase itemBaseB;
    UITexture sp_bg;
    public override void Init(UIEventListener handle)
    {
        sp_bg = handle.transform.GetComponent<UITexture>();
        lb_title = Get<UILabel>("lb_title");
        lb_desc = Get<UILabel>("lb_desc");
        go_ap = handle.transform.Find("items/A/item_parent").gameObject;
        go_bp = handle.transform.Find("items/B/item_parent").gameObject;
        sp_asp = Get<UISprite>("items/A/spr_acquired");
        sp_bsp = Get<UISprite>("items/B/spr_acquired");
        itemBaseA = UIItemManager.Instance.GetItem(PropItemType.Normal, go_ap.transform,itemSize.Size60);
        itemBaseB = UIItemManager.Instance.GetItem(PropItemType.Normal, go_bp.transform,itemSize.Size60);
    }

    protected void Adjust()
    {
        if (null != mData && mData.IsMain)
        {
            sp_bg.width = 140;
            sp_bg.height = 320;
        }
        else
        {
            sp_bg.width = 126;
            sp_bg.height = 288;
        }
    }

    public bool TryRefreshTime()
    {
        bool needExpressAward = false;
        if (null != lb_desc && null != mData)
        {
            var fightActivity = mData.fightActivity;
            bool main = mData.IsMain;
            if (null != fightActivity && !string.IsNullOrEmpty(fightActivity.winGuildName))
            {
                //xxx 行会获胜
                lb_desc.text = CSString.Format(1036, fightActivity.winGuildName).BBCode(ColorType.NPCMainText);
                needExpressAward = true;
            }
            else if (null != fightActivity && fightActivity.IsRunning(main))
            {
                //进行中
                lb_desc.text = CSString.Format(1035);
                needExpressAward = true;
            }
            else if (null != fightActivity && fightActivity.IsEnd(main))
            {
                //已经结束
                lb_desc.text = CSString.Format(1143);
                needExpressAward = true;
            }
            else
            {
                //剩余几天
                lb_desc.text = fightActivity.GetStartDay(main).BBCode(ColorType.NPCMainText);
                needExpressAward = main;
            }
        }
        return needExpressAward;
    }

    public override void Bind(object data)
    {
        mData = data as GuildFightItemData;

        itemSize size = null != mData && mData.IsMain ? itemSize.Size60 : itemSize.Size54;

        if (null != itemBaseA)
            itemBaseA.SetSize(size);
        if (null != itemBaseB)
            itemBaseB.SetSize(size);

        if (null != sp_bg)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(sp_bg.gameObject, "guildfight_label_bg",true,Adjust);
        }

        if(null != lb_title)
        {
            if (CSGuildFightManager.Instance.ShowMode == 0)
            {
                lb_title.text = CSString.Format(1030, mData.fightActivity.times);
            }
            else
            {
                var week = CSServerTime.StampToDateTime(mData.fightActivity.startTime).DayOfWeek;
                lb_title.text = CSString.Format(1034, CSGuildFightManager.Instance.GetWeekName(week));
            }
        }

        bool needExpressAward = TryRefreshTime();

        bool isNativeGuild = mData.fightActivity.winGuildId == CSMainPlayerInfo.Instance.GuildId;
        bool needEffect = isNativeGuild && mData.fightActivity.acquiredTime == 0 && mData.fightActivity.winGuildId != 0;

        itemBaseA.Refresh(mData.normal.key, OnClickAward,true, needEffect);
        itemBaseA.SetCount(mData.normal.value, Color.green,true);
        itemBaseB.Refresh(mData.extra.key, OnClickAward,true, needEffect);

        if (needExpressAward)
            itemBaseB.SetCount(mData.extra.value, Color.green, true);
        else
            itemBaseB.SetCount(0, Color.green, true);

        sp_asp.CustomActive(isNativeGuild && mData.fightActivity.acquiredTime > 0);
        sp_bsp.CustomActive(isNativeGuild && mData.fightActivity.acquiredTime > 0);
    }

    public void OnClickAward(UIItemBase itemBase)
    {
        if(null != mData && mData.fightActivity.acquiredTime == 0 && mData.fightActivity.winGuildId > 0 && mData.fightActivity.winGuildId == CSMainPlayerInfo.Instance.GuildId)
        {
            //这里奖励通过邮件发送，无需点击领取
        }
    }

    public override void OnDestroy()
    {
        if (null != sp_bg)
        {
            CSEffectPlayMgr.Instance.Recycle(sp_bg.gameObject);
            sp_bg = null;
        }
        if (null != itemBaseA)
        {
            UIItemManager.Instance.RecycleSingleItem(itemBaseA);
            itemBaseA = null;
        }
        if (null != itemBaseB)
        {
            UIItemManager.Instance.RecycleSingleItem(itemBaseB);
            itemBaseB = null;
        }

        mData = null;
    }
}