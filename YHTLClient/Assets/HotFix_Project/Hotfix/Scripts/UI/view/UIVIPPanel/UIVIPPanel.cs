using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIVIPPanel : UIBasePanel
{
    int viplv; // 玩家vip等级
    Map<int, vipItem> vipItemList = new Map<int, vipItem>();

    Dictionary<int, TABLE.VIP> vipTableList; //显示vip列表的数据
    int curClicklv; //当前点击的vip等级
    vip.VipInfo vipInfo;
    private long TasteTime;

    public Dictionary<int, bool> vipRewardInfos;
    private int maxlv;
    private FastArrayElementFromPool<UIItemBase> items;
    
    
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_get).onClick = OnGetClick;
        UIEventListener.Get(mbtn_upgrade).onClick = OnUpGradeClick;
        mClientEvent.AddEvent(CEvent.VipInfoChange, VipInfoChange);
        mscrollview_vip.SetDynamicArrowVertical(msp_down,msp_up);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(901),out maxlv);
        
        if (items == null)
        {
            items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_items.transform,8,itemSize.Size64);
            items.Clear();
        }
        CSEffectPlayMgr.Instance.ShowUITexture(mvip_bg.gameObject, "vip_bg");
        CSEffectPlayMgr.Instance.ShowUIEffect(mvip_effect, "effect_vip_add");
        CSEffectPlayMgr.Instance.ShowUIEffect(mbtngeteffect.gameObject, "effect_button_blue_add");
        RefreshUI();
        if (!CSVipInfo.Instance.IsVipTasteExpired())
        {
            TasteTime = (CSVipInfo.Instance.ExperienceCardInfo.endTime - CSServerTime.Instance.TotalMillisecond) / 1000;
            ScriptBinder.InvokeRepeating(0, 1, TasteExpireTime);
        }
    }

    private void RefreshUI()
    {
        vipInfo = CSVipInfo.Instance.VipInfo;
        vipRewardInfos = CSVipInfo.Instance.vipRwardInfos;
        viplv = CSMainPlayerInfo.Instance.VipLevel;
        
        int lv = CSVipInfo.Instance.GetFirstRwardLv();
        int firstRewardlv = lv == 0 ? 1 : lv;;
        curClicklv = firstRewardlv;
        TABLE.VIP tablevip;
        VIPTableManager.Instance.TryGetValue(viplv, out tablevip);
        
        
        //刷新左侧信息
        bool ismax = viplv == maxlv;

        mobj_topLevel.SetActive(ismax);
        mobj_upLevel.SetActive(!ismax);
        if (!ismax)
        {
            mlb_hint.text = CSString.Format(1058, tablevip.upgrade - vipInfo.exp); //下级经验还差
            CSStringBuilder.Clear();
            mlb_exp.text = CSStringBuilder.Append(vipInfo.exp, "/", tablevip.upgrade).ToString(); // 当前经验/这级全部经验
            mslider_exp.value = (float) (vipInfo.exp) / (float) tablevip.upgrade;
        }

        mlb_levelLeft.text = viplv.ToString();
        
        //刷新vip列表
        vipTableList = CSVipInfo.Instance.GetvipListInfo();
        mgrid_vips.MaxCount = vipTableList.Count;

        vipItemList.Clear();
        for (int i = 1; i <= mgrid_vips.MaxCount; i++)
        {
            // 
            UILabel[] labels = mgrid_vips.controlList[i - 1].GetComponentsInChildren<UILabel>();
            for (int j = 0; j < labels.Length; j++)
            {
                labels[j].text = i.ToString();
            }

            UIEventListener.Get(mgrid_vips.controlList[i - 1], i).onClick = RefreshvipItem;
            if (!vipItemList.ContainsKey(i))
            {
                vipItem vipItem = new vipItem();
                vipItem.Init(mgrid_vips.controlList[i - 1]);
                vipItemList.Add(i, vipItem);
            }
        }
        
        if (mgrid_vips.MaxCount > 4)
        {
            mscrollview_vip.ResetPosition();
            //计算滚轮的位置
            int vipwviewNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(605)); // vip条目数量
            
            mscrollview_vip.verticalScrollBar.value 
                = (float) (firstRewardlv - 1) / (float) (mgrid_vips.MaxCount - vipwviewNum);
        }

        RefreshvipItem();
    }

    private void TasteExpireTime()
    {
        mlb_time.text = CSString.Format(1619, CSServerTime.Instance.FormatLongToTimeStr(TasteTime, 3));
        TasteTime--;
        if (TasteTime < 0)
        {
            mlb_time.gameObject.SetActive(false);
            ScriptBinder.StopInvokeRepeating();
        }
    }

    private void VipInfoChange(uint uiEvtID, object data)
    {
        RefreshUI();
    }

    private void OnUpGradeClick(GameObject obj)
    {
        Utility.ShowGetWay(19);
    }

    private void OnGetClick(GameObject obj)
    {
        Net.ReqDrawVipPackMessage(curClicklv);
    }

    private void RefreshvipItem(GameObject obj = null)
    {
        if (obj != null)
        {
            curClicklv = (int) UIEventListener.Get(obj).parameter;
        }

        //如果点击为当前lv 的后两级 那么 弹出tips
        // if (curClicklv >= viplv + 2)
        // {
        //     UtilityTips.ShowRedTips(CSString.Format(1072));
        //     return;
        // }
        //设置右侧vip按钮
        for (vipItemList.Begin(); vipItemList.Next();)
        {
            GameObject normal = vipItemList.Value.normal;
            GameObject select = vipItemList.Value.select;
            GameObject gray = vipItemList.Value.gray;
            GameObject redPoint = vipItemList.Value.redPoint;
            gray.SetActive(false);

            bool isSelect = curClicklv == vipItemList.Key;
            select.SetActive(isSelect);
            normal.SetActive(!isSelect);

            if (vipRewardInfos != null)
            {
                redPoint.SetActive(vipRewardInfos[vipItemList.Key]);
            }
        }

        RefreshRightUI(curClicklv);
    }

    private void RefreshRightUI(int lv)
    {
        TABLE.VIP tablevip;
        VIPTableManager.Instance.TryGetValue(lv, out tablevip);
        mlb_level.text = lv.ToString();
        mlb_levelbox.text = lv.ToString();
        CSStringBuilder.Clear();
        mlb_tips.text = CSStringBuilder.Append(UtilityColor.NPCMainText, tablevip.tips).ToString();

        Dictionary<int, int> dicboxgift = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
        dicboxgift.Clear();
        BoxTableManager.Instance.GetBoxAwardById(tablevip.lvGift,dicboxgift);
        int index = 0;
        for (var it =  dicboxgift.GetEnumerator(); it.MoveNext();)
        {
            UIItemBase item;
            if (index > items.Count -1)
            {
                item = items.Append();
            }
            else
            {
                item = items[index];
            }
            item.Refresh(it.Current.Key);
            item.SetCount(it.Current.Value);
            
            index++;
        }
        
        mPoolHandleManager.Recycle(dicboxgift);
        
        //设置领取按钮
        int viplevel = vipInfo.vipLevel; //实际的vip等级

        mlb_Received.SetActive(false);
        mbtn_get.SetActive(false);
        msp_unreach.SetActive(false);
        mbtngeteffect.SetActive(false);

        if (!CSVipInfo.Instance.IsVipTasteExpired() && viplevel == 0)
        {
            msp_unreach.SetActive(true);
        }
        else
        {
            if (curClicklv > viplevel)
            {
                msp_unreach.SetActive(true);
            }
            else
            {
                if (vipRewardInfos != null && vipRewardInfos.ContainsKey(lv))
                {
                    if (vipRewardInfos[lv] == false)
                    {
                        mlb_Received.SetActive(true);
                    }
                    else
                    {
                        mbtn_get.SetActive(true);
                        mbtngeteffect.SetActive(true);
                        mbtn_get.GetComponent<UISprite>().color = Color.white;
                    }
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CSEffectPlayMgr.Instance.Recycle(mvip_bg);
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
}

public class vipItem
{
    public  GameObject normal;
    public  GameObject select;
    public  GameObject gray;
    public  GameObject redPoint;

    public void Init(GameObject vipItemList)
    {
        normal = vipItemList.transform.GetChild(0).gameObject;
        select = vipItemList.transform.GetChild(1).gameObject;
        gray = vipItemList.transform.GetChild(2).gameObject;
        redPoint = vipItemList.transform.Find("redpoint").gameObject;
    }



    
    
    
    
}

