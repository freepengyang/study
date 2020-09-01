using activity;
using System;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIServerActivityGiftPanel : UIBasePanel
{
    List<UIItemBase> uiItemBaseList = new List<UIItemBase>();
    int curacid;
    SPECIALACTIVEREWARD curspecialTable;
    Dictionary<int, SpecialActivityData> mapSpecialData = new Dictionary<int, SpecialActivityData>(16); //服务端返回数据
    private CSBetterLisHot<SPECIALACTIVITY> typeList = new CSBetterLisHot<SPECIALACTIVITY>();
    Dictionary<int, bool> GiftType = new Dictionary<int, bool>(16); // 判断当是否进行购买操作
    Dictionary<int,GameObject> dicObjs = new Dictionary<int, GameObject>(16);
    Dictionary<int, int> rewardsDic = new Dictionary<int, int>(16);

    int diff = 10105;
    public override void Init()
    {
        base.Init();
        // rewardsDic = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
        // dicObjs = mPoolHandleManager.GetSystemClass<Dictionary<int, GameObject>>();
        // GiftType = mPoolHandleManager.GetSystemClass<Dictionary<int, bool>>();
        // mapSpecialData = mPoolHandleManager.GetSystemClass<Dictionary<int, SpecialActivityData>>();
        // uiItemBaseList = mPoolHandleManager.GetSystemClass<List<UIItemBase>>();
        rewardsDic.Clear();
        dicObjs.Clear();
        GiftType.Clear();
        mapSpecialData.Clear();
        uiItemBaseList.Clear();
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, ResSpecialActivityChange);

        UIEventListener.Get(mbtn_get).onClick = OnGetClick;
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_banner3.gameObject, "banner3");

        typeList = SpecialActivityTableManager.Instance.GetTableDataByEventId(6);
        mgrid_tab.MaxCount = typeList.Count;
        for (int i = 0; i < typeList.Count; i++)
        {
            ScriptBinder binder = mgrid_tab.controlList[i];
            dicObjs.Add(i+1,binder.gameObject);
            //GiftType.Add(typeList[i].id, false);
            UILabel mlb_name = binder.GetObject("lb_name") as UILabel;
            mlb_name.text = typeList[i].smallName;
            UISprite msp_icon = binder.GetObject("sp_icon") as UISprite;

            int boxid = SpecialActiveRewardTableManager.Instance.GetBoxId(typeList[i].id, 0);
            int showid = BoxTableManager.Instance.GetBoxShow(boxid);
            msp_icon.spriteName = showid.ToString();
            UIEventListener.Get(binder.gameObject, typeList[i].id).onClick = OnTabClick;
        }
        curacid = typeList[0].id;
    }
    
    public override void Show()
    {
        base.Show();
        for (int i = 0; i < typeList.Count; i++)
        {
            GiftType[typeList[i].id] = false;
        }
        // for (var it = GiftType.GetEnumerator();it.MoveNext();)
        // {
        //     GiftType[it.Current.Key] = true;
        // }
        
        SelectChildPanel();
    }

    public override void OnHide()
    {
        
        base.OnHide();
    }

    private void ResSpecialActivityChange(uint uiEvtID, object data)
    {
        SpecialActivityData mes = (SpecialActivityData) data;
        //Debug.Log("ResSpecialActivityChange" + mes.activityId);
        //判断数据是否是该面板使用
        var temp = typeList.FirstOrNull(x => x.id == mes.activityId);
        if (temp == null)
            return;
        if (mapSpecialData.ContainsKey(mes.activityId))
            mapSpecialData[mes.activityId] = mes;
        else
            mapSpecialData.Add(mes.activityId, mes);

        //SelectChildPanel(curacid - diff);
        
        var rewardGoals = mapSpecialData[mes.activityId].rewardGoals;
        if (!GiftType.ContainsKey(mes.activityId))
            return;
        if (rewardGoals.Contains(CSOpenServerACInfo.Instance.GetRewardid(curacid)) && GiftType[mes.activityId])
        {
            int boxid = SpecialActiveRewardTableManager.Instance.GetBoxId(curacid, 0);
            rewardsDic?.Clear();
            BoxTableManager.Instance.GetBoxAwardById(boxid, rewardsDic);
            Utility.OpenGiftPrompt(rewardsDic);
        }
        
        OnTabClick();
        
    }
    
    public override void SelectChildPanel(int type = 1)
    {
		if (type > 5) return;
        curacid = type + diff;
        if (dicObjs.ContainsKey(type))
        {
            mobj_select.transform.localPosition = dicObjs[type].transform.localPosition;
        }
        
        if (!mlb_money)
            return;
        curspecialTable = SpecialActiveRewardTableManager.Instance.GetDataByacId(curacid);
        mlb_money.text = curspecialTable.termNum.ToString();

        Utility.GetItemByBoxid(curspecialTable.reward, mgrid_item, ref uiItemBaseList, itemSize.Size60);
        //自适应物品格子    
        int count = mgrid_item.controlList.Count;
        if (mgrid_item.controlList.Count > 5)
            mgrid_item.MaxPerLine = count / 2 + 1;
        else
            mgrid_item.MaxPerLine = count;

        bool isReceive = false;
        if (mapSpecialData.ContainsKey(curacid))
            isReceive = mapSpecialData[curacid].rewardGoals.Contains(CSOpenServerACInfo.Instance.GetRewardid(curacid));
        //string boxEffectid = BoxTableManager.Instance.GetBoxEffect(curspecialTable.reward);
        mlb_Receive.SetActive(isReceive);
        mbtn_get.SetActive(!isReceive);
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect.gameObject, Utility.GetBoxEffect(curspecialTable.reward));
    }
    
    
    private void OnTabClick(GameObject obj = null)
    {
        if (obj != null)
        {
            curacid = (int) UIEventListener.Get(obj).parameter;
            //mobj_select.transform.localPosition = obj.transform.localPosition;
        }

        SelectChildPanel(curacid - diff);
    }

    private void OnGetClick(GameObject obj)
    {
        int cost = int.Parse(curspecialTable.termNum);

        if (((int)MoneyType.yuanbao).GetItemCount() < cost)
        {
            Utility.ShowGetWay(3);
            return;
        }
        //如果满足提示购买
        UtilityTips.ShowPromptWordTips(68, () =>
        {
            if (GiftType.ContainsKey(curacid))
                GiftType[curacid] = true;
            Net.CSBuyYuanbaoGiftMessage(curacid);
        },curspecialTable.termNum);

    }

    protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(meffect.gameObject);
        if (uiItemBaseList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(uiItemBaseList);
            uiItemBaseList.Clear();
        }
        
        if (mtex_banner3.gameObject != null)
        {
            CSEffectPlayMgr.Instance.Recycle(mtex_banner3.gameObject);
        }
        
        
        uiItemBaseList = null;
        curspecialTable = null;
        mapSpecialData = null;
        typeList = null;
        GiftType = null;
        dicObjs = null;
        
        base.OnDestroy();
    }

    
}
