using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWingColorBinder : UIBinder
{
    public bool isSelect = false;
    public Action<int> actionItem;
    public Action<int> actionWear;
    public Action<int> actionGet;
    public Action<int> actionActive;
    public Action<int> actionTakeOff;
    public Action<int> actionItemBase;
    private GameObject bg;
    GameObject select;
    UISprite sp_itemicon;
    UISprite sp_quality;
    UILabel lb_name;
    UILabel lb_time;
    GameObject btn_wear;
    GameObject btn_get;
    GameObject btn_active;
    UILabel lb_hint;
    UILabel lb_hint_top;
    GameObject redpoint;
    GameObject btn_takeOff;
    private GameObject ItemBase;
    private WingColorData wingColorData;

    public override void Init(UIEventListener handle)
    {
        bg = Get<GameObject>("bg");
        select = Get<GameObject>("select");
        ItemBase = Get<GameObject>("ItemBase");
        sp_itemicon = Get<UISprite>("ItemBase/sp_itemicon");
        sp_quality = Get<UISprite>("ItemBase/sp_quality");
        lb_name = Get<UILabel>("lb_name");
        lb_time = Get<UILabel>("lb_time");
        btn_wear = Get<GameObject>("btn_wear");
        btn_get = Get<GameObject>("btn_get");
        btn_active = Get<GameObject>("btn_active");
        lb_hint = Get<UILabel>("lb_hint");
        lb_hint_top = Get<UILabel>("lb_hint_top");
        redpoint = Get<GameObject>("redpoint");
        btn_takeOff = Get<GameObject>("btn_takeOff");

        UIEventListener.Get(ItemBase).onClick = OnClickItemBase;
        UIEventListener.Get(bg).onClick = OnClickItem;
        UIEventListener.Get(btn_wear).onClick = OnClickWear;
        UIEventListener.Get(btn_get).onClick = OnClickGet;
        UIEventListener.Get(btn_active).onClick = OnClickActive;
        UIEventListener.Get(btn_takeOff).onClick = OnClickTakeOff;

    }

    public override void Bind(object data)
    {
        if (data == null) return;
        wingColorData = (WingColorData) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        TABLE.ITEM wingColor;
        if (!ItemTableManager.Instance.TryGetValue(wingColorData.configId, out wingColor)) return;
        select.SetActive(isSelect);
        lb_name.text = wingColor.name;
        lb_name.color = UtilityCsColor.Instance.GetColor(wingColor.quality);
        sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(wingColor.quality);
        sp_itemicon.spriteName = wingColor.icon;

        if (wingColorData.count <= 0 && wingColorData.endTime == 0) //未获得
        {
            btn_takeOff.SetActive(false);
            btn_wear.SetActive(false);
            // btn_get.SetActive(index == selectIndex);
            btn_get.SetActive(true);
            btn_active.SetActive(false);
            // lb_hint.text = CSString.Format(718);
            // lb_hint.gameObject.SetActive(index != selectIndex);
            lb_hint.gameObject.SetActive(false);
            lb_hint_top.gameObject.SetActive(false);
            //获取途径
            lb_time.gameObject.SetActive(false);
            List<int> list = UtilityMainMath.SplitStringToIntList(wingColor.getWay);
            if (list.Count > 0)
            {
                lb_time.text = GetWayTableManager.Instance.GetGetWayName(list[0]);
                lb_time.color = UtilityColor.GetColor(ColorType.SecondaryText);
                lb_time.gameObject.SetActive(true);
            }

            sp_itemicon.color = Color.black;
            redpoint.SetActive(false);
        }
        else //已获得
        {
            sp_itemicon.color = Color.white;
            if (wingColorData.count > 1)
            {
                lb_hint_top.text =
                    CSString.Format(730, wingColorData.count);
                lb_hint_top.gameObject.SetActive(true);
            }

            if (wingColorData.endTime == 0) //已获得未激活
            {
                btn_takeOff.SetActive(false);
                btn_wear.SetActive(false);
                btn_get.SetActive(false);
                // btn_active.SetActive(index == selectIndex);
                btn_active.SetActive(true);
                // lb_hint.text = CSString.Format(732); //等待激活
                // lb_hint.gameObject.SetActive(index != selectIndex);
                lb_hint.gameObject.SetActive(false);

                lb_time.gameObject.SetActive(true);
                List<int> listInt = UtilityMainMath.SplitStringToIntList(wingColor.timeLimit);
                if (listInt.Count == 3)
                {
                    lb_time.text = CSString.Format(733, listInt[1]);
                }

                redpoint.SetActive(true);
            }
            else
            {
                if (wingColorData.configId != CSWingInfo.Instance.MyWingData.wingColorId) //已激活未穿戴
                {
                    btn_takeOff.SetActive(false);
                    // btn_wear.SetActive(index == selectIndex);
                    btn_wear.SetActive(true);
                    btn_get.SetActive(false);
                    btn_active.SetActive(false);
                    // lb_hint.text = CSString.Format(717);
                    // lb_hint.gameObject.SetActive(index != selectIndex);
                    lb_hint.gameObject.SetActive(false);
                }
                else //已穿戴(显示脱下按钮)
                {
                    btn_takeOff.SetActive(true);
                    btn_wear.SetActive(false);
                    btn_get.SetActive(false);
                    btn_active.SetActive(false);
                    // lb_hint.text = CSString.Format(714);
                    // lb_hint.gameObject.SetActive(true);
                    lb_hint.gameObject.SetActive(false);
                }

                redpoint.SetActive(false);

                //计时(放主页面计时)或者永久
                if (wingColorData.endTime==-1)
                    lb_time.text = CSString.Format(719);
                lb_time.gameObject.SetActive(true);
            }
        }
    }

    void OnClickItemBase(GameObject go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, wingColorData.configId);
        actionItemBase?.Invoke(index);
    }

    void OnClickItem(GameObject go)
    {
        actionItem?.Invoke(index);
    }
    
    void OnClickWear(GameObject go)
    {
        actionWear?.Invoke(index);
    }
    
    void OnClickGet(GameObject go)
    {
        actionGet?.Invoke(index);
    }
    
    void OnClickActive(GameObject go)
    {
        actionActive?.Invoke(index);
    }
    
    void OnClickTakeOff(GameObject go)
    {
        actionTakeOff?.Invoke(index);
    }

    public override void OnDestroy()
    {
        actionItem = null;
        actionWear = null;
        actionGet = null;
        actionActive = null;
        actionTakeOff = null;
        bg = null;
        select = null;
        sp_itemicon = null;
        sp_quality = null;
        lb_name = null;
        lb_time = null;
        btn_wear = null;
        btn_get = null;
        btn_active = null;
        lb_hint = null;
        lb_hint_top = null;
        redpoint = null;
        btn_takeOff = null;
        wingColorData = null;
        ItemBase = null;
        actionItemBase = null;
    }
}
