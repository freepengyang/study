using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIRechargeFirstPanel : UIBasePanel
{
    //List<Transform> tagList = new List<Transform>(); //tag列表
    Map<int, TagPara> TagParas = new Map<int, TagPara>(); //数据列表
    int day; //当前领取天数
    List<UIItemBase> uiItemlist = null;
    private List<List<string>> icons; //图标的脚标
    public struct TagPara
    {
        public int day;  //第几天
        public string boxid; //展示奖励boxid
        public int itemid; //展示物品模型id
        public Transform tagtrans; //tag的transform

    }
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        
        int career = CSMainPlayerInfo.Instance.Career;
        int sex = CSMainPlayerInfo.Instance.Sex;
        int sundryid = sex == 1 ? 584 : 718;
        
        string [] tempboxid = GetItemListbyCareer(career).Split('#');
        string[] tempModeIds = SundryTableManager.Instance.GetSundryEffect(sundryid).Split('&');
        string[] modeids = tempModeIds[career - 1].Split('#');


        for (int i = 0; i < 3; i++)
        {
            Transform trans = UtilityObj.Get<Transform>(mobj_ToggleGroup.transform.GetChild(i), "Background");

            TagPara tagpara = new TagPara();
            tagpara.day = i+1;
            tagpara.boxid = tempboxid[i];
            tagpara.itemid = int.Parse(modeids[i]);
            tagpara.tagtrans = trans;

            //boxids.Add(i, temp[i - 1]);
            TagParas.Add(i+1,tagpara);

            //tagList.Add(trans);
            UIEventListener.Get(trans.gameObject,i+1).onClick = ontagClick;    
        }
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_recharge).onClick = OnRechargeClick;
        UIEventListener.Get(mbtn_draw).onClick = OnDrawClick;

        mClientEvent.Reg((uint)CEvent.FirstRechargeInfoChange, OnFirstRechargeInfoChange);
       
        
       
        CSEffectPlayMgr.Instance.ShowUITexture(mrecharge_label_bg, "recharge_label_bg");
        mClientEvent.SendEvent(CEvent.FirstRechargeRedChange);
        
        var tempicon = SundryTableManager.Instance.GetSundryEffect(679).Split('&');

        if (icons == null)
        {
            icons = mPoolHandleManager.GetSystemClass<List<List<string>>>();
            icons.Clear();
        }
        for (int i = 0; i < tempicon.Length; i++)
        {
            var tempList = mPoolHandleManager.GetSystemClass<List<string>>();
            tempList.Clear();
            var tempids = tempicon[i].Split('#');
            for (int j = 0; j < tempids.Length; j++)
            {
                tempList.Add(tempids[j]);
            }
            icons.Add(tempList);
            mPoolHandleManager.Recycle(tempList);
        }
        
    }

    public override void Show()
    {
        base.Show();

        day = 1;
        for (int i = 0; i < 3; i++)
        {
            if (!CSVipInfo.Instance.IsRecharge(i + 1))
            {
                day = i + 1;
                break;
            }
        }
        RefreshUI(day);
        
    }

    private void OnFirstRechargeInfoChange(uint uiEvtID, object data)
    {
        RefreshUI(day);
        
    }
    //领取
    private void OnDrawClick(GameObject obj)
    {
        //Net.CSDrawFirstRechargeRewardMessage(day);
        int rechargeDay = CSVipInfo.Instance.GetRechargeDay();
        //Debug.Log("rechargeDay" + rechargeDay);
        if (rechargeDay >= day)
        {
            Net.CSDrawFirstRechargeRewardMessage(day);
        }
        else 
        {
            UtilityTips.ShowRedTips(CSString.Format(1051));
        }
        
    }

    //充钱
    private void OnRechargeClick(GameObject obj)
    {
        UtilityPanel.JumpToPanel(12305);
        //Debug.Log("OnRechargeClick");
    }

    private void ontagClick(GameObject obj = null)
    {
        int index = (int)UIEventListener.Get(obj).parameter; //传入的index从1 开始
        if (index == day)
            return;
        
        day = index;
        RefreshUI(day);
    }

    private void RefreshUI(int dayInfo)
    {
        //Debug.Log("day" + day);
        //if (uiItemlist != null)
        //{
        //    UIItemManager.Instance.RecycleItemsFormMediator(uiItemlist);
        //    uiItemlist.Clear();
        //}
        
        Utility.GetItemByBoxid(int.Parse(TagParas[dayInfo].boxid), mgrid_items,ref uiItemlist,itemSize.Size64);

        if (icons!= null)
        {
            for (int i = 0; i < mgrid_items.MaxCount; i++)
            {
                UISprite icon = UtilityObj.Get<UISprite>(mgrid_items.controlList[i].transform, "flag");
                int index = dayInfo - 1;
                
                if (i >= icons[index].Count)
                {
                    icon.gameObject.SetActive(false);
                }
                else
                {
                    icon.gameObject.SetActive(true);
                    icon.spriteName = icons[index][i];
                }
            }
        }
        
       
        
        for (int i = 0; i < TagParas.Count; i++)
        {
            //Debug.Log(TagParas[day].tagtrans.parent.gameObject.name + "||" +(i + 1 == day));
            UtilityObj.Get<Transform>(TagParas[i + 1].tagtrans, "Checkmark").gameObject.SetActive(i+1 == dayInfo);
        }
        
        CSEffectPlayMgr.Instance.ShowUIEffect(msp_icon.gameObject, TagParas[dayInfo].itemid.ToString(), ResourceType.UIEffect);
        //int itemid = TagParas[day].itemid;
        //msp_icon.gameObject.LoadAvatarModel(itemid,0,0,AvatarModelType.AMT_Weapon);
       // msp_icon.gameObject.LoadAvatarModel(itemid,0,0,AvatarModelType.);
        ReFreshBtn();
    }

    
    private void ReFreshBtn()
    {
        //Debug.Log(CSVipInfo.Instance.isRecharge(day));
        if (CSVipInfo.Instance.IsFirstRecharge())
        {
            bool isRecharge = CSVipInfo.Instance.IsRecharge(day);
            mobj_Received.SetActive(isRecharge);
            mbtn_recharge.SetActive(false);//充点小钱
            int openDay = CSVipInfo.Instance.GetRechargeDay();
            mbtn_draw.SetActive(!isRecharge && day <= openDay); //领取按钮
            mlb_Day.gameObject.SetActive(!isRecharge && day >openDay);
            mlb_Day.text = day == 2 ? CSString.Format(1279) : CSString.Format(1280);

            if (!isRecharge && day <= openDay)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(meffect.gameObject, 17712);
                CSEffectPlayMgr.Instance.ShowUIEffect(meffectbtn.gameObject, 17710);
            }
            else
            {
                CSEffectPlayMgr.Instance.Recycle(meffect.gameObject);
                CSEffectPlayMgr.Instance.Recycle(meffectbtn.gameObject);
            }

            //SundryTableManager.Instance.GetSundryEffect(1279);
            //string[] strs = SundryTableManager.Instance.GetSundryEffect(1279).Split('#');
            // if (strs.Length>=2)
            // {
            //     mlb_Day.text = day == 2 ? strs[0] : strs[1];
            // }

        }
        else
        {
            mobj_Received.SetActive(false);
            mbtn_recharge.SetActive(true); //充点小钱
            mbtn_draw.SetActive(false);
            mlb_Day.gameObject.SetActive(false);
            CSEffectPlayMgr.Instance.ShowUIEffect(meffect.gameObject, 17711);
            CSEffectPlayMgr.Instance.ShowUIEffect(meffectbtn.gameObject, 17710);
        }
    }

    private string GetItemListbyCareer(int career) {

        switch (career)
        {
            case 1:
                return SundryTableManager.Instance.GetSundryEffect(466);
            case 2:
                return SundryTableManager.Instance.GetSundryEffect(467);
            case 3:
                return SundryTableManager.Instance.GetSundryEffect(468);
            default:
                return SundryTableManager.Instance.GetSundryEffect(466);
        }
    }

    protected override void OnDestroy()
    {
        
        if (uiItemlist != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(uiItemlist);
            uiItemlist.Clear();
        }
        mPoolHandleManager.Recycle(icons);
        CSEffectPlayMgr.Instance.Recycle(meffectbtn.gameObject);
        CSEffectPlayMgr.Instance.Recycle(meffect.gameObject);
        CSEffectPlayMgr.Instance.Recycle(mrecharge_label_bg.gameObject);
        CSEffectPlayMgr.Instance.Recycle(msp_icon.gameObject);
        
        base.OnDestroy();
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    // public override bool ShowGaussianBlur
    // {
    //     get { return false; }
    // }

}
