using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
部位         格子           装备部位(type==2)           部位(卧龙)   格子           装备部位(type==5)

武器          1               1                         武器          101               1           
衣服          2               2                         衣服          102               2
头盔          3               3                         头盔          103               3
项链          4               4                         项链          104               4
护腕(左)      5               5                         护腕(左)      105               5
护腕(右)      6               5                         护腕(右)      106               5
戒指(左)      7               6                         戒指(左)      107               6
戒指(右)      8               6                         戒指(右)      108               6
靴子          9               7                         靴子          109               7
腰带          10              8                         腰带          110               8
勋章          11              9                         勋章          111               9
宝石          12              10                        宝石          112               10
 **/

public class UIRoleEquipPanel : UIBasePanel
{
    public enum equipType
    {
        Normal,
        WoLong,
    }

    public static int[] normalEquipIndex = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; //格子对应的服务器pos
    public static int[] normalPosIndex = { 1, 2, 3, 4, 5, 5, 6, 6, 7, 8, 9, 10 }; //格子对应的装备Pos

    public static int[] wolongEquipIndex = { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112 }; //格子对应的服务器pos
    public static int[] wolongPosIndex = { 101, 102, 103, 104, 105, 105, 106, 106, 107, 108, 109, 110 }; //格子对应的装备Pos
    //{ "武器", "衣服", "头盔", "项链", "手镯(左)", "手镯(右)", "戒指(左)", "戒指(右)", "鞋子", "腰带", "勋章", "宝石" };

    #region forms

    private GameObject _obj_equippar;

    private GameObject obj_equippar
    {
        get { return _obj_equippar ?? (_obj_equippar = Get("view/equips/equips").gameObject); }
    }

    private UILabel _lb_rolename;

    private UILabel lb_rolename
    {
        get { return _lb_rolename ?? (_lb_rolename = Get<UILabel>("view/rolename")); }
    }

    private UILabel _lb_roleLv;

    private UILabel lb_roleLv
    {
        get { return _lb_roleLv ?? (_lb_roleLv = Get<UILabel>("view/roleoffice")); }
    }

    GameObject _obj_roleModel;

    GameObject obj_roleModel
    {
        get { return _obj_roleModel ?? (_obj_roleModel = Get("roleModel/Role").gameObject); }
    }

    GameObject _obj_roleWeapon;

    GameObject obj_roleWeapon
    {
        get { return _obj_roleWeapon ?? (_obj_roleWeapon = Get("roleModel/Weapon").gameObject); }
    }

    GameObject _obj_roleCloth;

    GameObject obj_roleCloth
    {
        get { return _obj_roleCloth ?? (_obj_roleCloth = Get("roleModel/Cloth").gameObject); }
    }

    GameObject _obj_wlEquippar;

    GameObject obj_wlEquippar
    {
        get { return _obj_wlEquippar ?? (_obj_wlEquippar = Get("view/equips/wolongEquip").gameObject); }
    }

    GameObject _btn_ToWoLong;

    GameObject btn_ToWoLong
    {
        get { return _btn_ToWoLong ?? (_btn_ToWoLong = Get("event/btn_ToWoLong").gameObject); }
    }

    GameObject _btn_huaijiu;

    GameObject btn_huaijiu
    {
        get { return _btn_huaijiu ?? (_btn_huaijiu = Get("event/btn_huaijiu").gameObject); }
    }


    GameObject _btn_ToNormal;

    GameObject btn_ToNormal
    {
        get { return _btn_ToNormal ?? (_btn_ToNormal = Get("event/btn_ToNormal").gameObject); }
    }
    GameObject _btn_LongJi;
    GameObject btn_LongJi
    {
        get { return _btn_LongJi ?? (_btn_LongJi = Get("event/btn_LongJi").gameObject); }
    }

    GameObject _btn_LongLi;

    GameObject btn_LongLi
    {
        get { return _btn_LongLi ?? (_btn_LongLi = Get("event/btn_LongLi").gameObject); }
    }


    #endregion

    #region variable

    Dictionary<int, bag.BagItemInfo> mEquipData;
    Dictionary<int, bag.BagItemInfo> mWLEquipData;
    List<EquipItem> equipItems;
    List<EquipItem> WLequipItems;
    equipType EquipType = equipType.Normal;
    bool openZhuFu = false;
    #endregion

    public override void Init()
    {
        base.Init();
        mEquipData = new Dictionary<int, bag.BagItemInfo>();
        mEquipData.Clear();
        mWLEquipData = new Dictionary<int, bag.BagItemInfo>();
        mWLEquipData.Clear();
        mClientEvent.Reg((uint)CEvent.WearEquip, EquipGetOn);
        mClientEvent.Reg((uint)CEvent.UnWeatEquip, EquipGetOff);
        mClientEvent.Reg((uint)CEvent.EquipRebuildNtfMessage, EquipChange);
        mClientEvent.AddEvent(CEvent.ItemListChange, ItemChange);
        mClientEvent.AddEvent(CEvent.WoLongLevelUpgrade, ItemChange);
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, ItemChange);
        mClientEvent.AddEvent(CEvent.Rename, GetRenameBack);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.Player_ReplaceEquip, GetModelChange);

        UIEventListener.Get(btn_ToWoLong, equipType.Normal).onClick = ChangeEquipBtnClick;
        UIEventListener.Get(btn_ToNormal, equipType.WoLong).onClick = ChangeEquipBtnClick;
        UIEventListener.Get(btn_LongJi).onClick = ShowLongJi;
        UIEventListener.Get(btn_LongLi).onClick = ShowLongLi;
        UIEventListener.Get(btn_huaijiu).onClick = OnShowHuaiJiu;

        if (GetHuaiJiubtn())
        {
            var redpoint = UtilityObj.Get<Transform>(btn_huaijiu.transform, "redpoint");
            RegisterRed(redpoint.gameObject, RedPointType.Nostalgia);
        }

        ShowName();
        EquipItemsInit();
        RefreshEquipItems();
        RefreshWLEquipItems();
        ShowWeapon();
        ShowCloth();
    }

    private void OnShowHuaiJiu(GameObject obj)
    {
        //UIManager.Instance.ClosePanel<UIBasePanel>();
        UIManager.Instance.CreatePanel<UINostalgiaEquipPanel>();
    }

    public void SetEquipType(equipType type = equipType.Normal)
    {
        //Debug.Log(" 切换装备类型  " + type);
        EquipType = type;
        EquipChange();
    }
    public void SetEquipType(int type)
    {
        EquipType = (equipType)type;
        //Debug.Log(" 切换装备类型  " + EquipType);
        EquipChange();
    }
    public void SelectWeapon()
    {
        openZhuFu = true;
        if (openZhuFu && equipItems != null)
        {
            openZhuFu = false;
            equipItems[0].SelectWeapon();
        }
    }

    protected override void OnDestroy()
    {
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.Player_ReplaceEquip, GetModelChange);
        CSEffectPlayMgr.Instance.Recycle(obj_roleWeapon);
        CSEffectPlayMgr.Instance.Recycle(obj_roleCloth);
        CSEffectPlayMgr.Instance.Recycle(obj_roleModel);
        mEquipData = null;
        mWLEquipData = null;
        equipItems = null;
        WLequipItems = null;
        base.OnDestroy();
    }

    void ChangeEquipBtnClick(GameObject _go)
    {
        EquipType = (equipType)UIEventListener.Get(_go).parameter;
        //Debug.Log(EquipType);
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcp_wolong))
        {
            EquipType = (EquipType == equipType.Normal) ? equipType.WoLong : equipType.Normal;
            EquipChange();
        }
        else
        {
            mClientEvent.SendEvent(CEvent.ChangeEquipShow, EquipType);
        }
    }
    void ShowLongJi(GameObject _go)
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcp_wolong))
        {
            return;
        }
        if (!CSBagInfo.Instance.IsHasLongJi())
        {
            int wayId = 0;
            int.TryParse(SundryTableManager.Instance.GetSundryEffect(1022), out wayId);
            Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1022));
            //UtilityTips.ShowTips(1877);
            return;
        }
        UIManager.Instance.CreatePanel<UILongJiTipsPanel>();
    }
    void ShowLongLi(GameObject _go)
    {
        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcp_wolong))
        {
            return;
        }
        if (!CSBagInfo.Instance.IsHasBaseAffix())
        {
            int wayId = 0;
            int.TryParse(SundryTableManager.Instance.GetSundryEffect(1021), out wayId);
            Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1021));
            //UtilityTips.ShowTips(1876);
            return;
        }
        UIManager.Instance.CreatePanel<UILongLiTipsPanel>();
    }
    void EquipChange()
    {
        if (EquipType == equipType.Normal)
        {
            btn_ToWoLong.SetActive(true);
            obj_wlEquippar.SetActive(false);
            btn_ToNormal.SetActive(false);
            obj_equippar.SetActive(true);

            btn_LongJi.SetActive(false);
            btn_LongLi.SetActive(false);
        }
        else
        {
            btn_ToWoLong.SetActive(false);
            obj_wlEquippar.SetActive(true);
            btn_ToNormal.SetActive(true);
            obj_equippar.SetActive(false);

            bool wolongIsOpen = UICheckManager.Instance.DoCheckFunction(FunctionType.funcp_wolong);
            btn_LongJi.SetActive(wolongIsOpen);
            btn_LongLi.SetActive(wolongIsOpen);
        }

        GetHuaiJiubtn();
    }

    /// <summary>
    /// 判断怀旧装备是否开启
    /// </summary>
    bool GetHuaiJiubtn()
    {
        bool huaijiu = UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_Nostalgia) && EquipType == equipType.Normal;
        btn_huaijiu.SetActive(huaijiu);
        return huaijiu;
    }

    void ShowName()
    {
        lb_rolename.text = CSMainPlayerInfo.Instance.Name;
        lb_roleLv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), CSMainPlayerInfo.Instance.Level);
    }

    void ShowModel()
    {
    }

    void ShowWeapon()
    {
        CSBagInfo.Instance.GetNormalEquip(mEquipData);
        bag.BagItemInfo info;
        if (mEquipData != null && mEquipData.ContainsKey(1))
        {
            info = mEquipData[1];
            CSEffectPlayMgr.Instance.ShowUIEffect(obj_roleWeapon,
                ItemTableManager.Instance.GetItemModel(info.configId).ToString(), ResourceType.UIWeapon);
        }
        else
        {
            CSEffectPlayMgr.Instance.Recycle(obj_roleWeapon);
        }
    }

    void ShowCloth()
    {
        CSBagInfo.Instance.GetNormalEquip(mEquipData);
        bag.BagItemInfo tempinfo;
        if (mEquipData != null && mEquipData.ContainsKey(2))
        {
            tempinfo = mEquipData[2];
            CSEffectPlayMgr.Instance.ShowUIEffect(obj_roleCloth,
                ItemTableManager.Instance.GetItemModel(tempinfo.configId).ToString(), ResourceType.UIPlayer);
            obj_roleModel.SetActive(false);
            obj_roleCloth.SetActive(true);
        }
        else
        {
            obj_roleModel.SetActive(true);
            obj_roleCloth.SetActive(false);
            string str = CSMainPlayerInfo.Instance.Sex == 1 ? "615000" : "625000";
            CSEffectPlayMgr.Instance.ShowUIEffect(obj_roleModel, str, ResourceType.UIPlayer);
        }
    }

    void EquipItemsInit()
    {
        equipItems = new List<EquipItem>();
        equipItems.Clear();
        for (int i = 0; i < obj_equippar.transform.childCount; i++)
        {
            equipItems.Add(new EquipItem(obj_equippar.transform.GetChild(i).gameObject, i, 1));
        }

        WLequipItems = new List<EquipItem>();
        WLequipItems.Clear();
        for (int i = 0; i < obj_wlEquippar.transform.childCount; i++)
        {
            WLequipItems.Add(new EquipItem(obj_wlEquippar.transform.GetChild(i).gameObject, i, 2));
        }
    }

    void RefreshEquipItems()
    {
        CSBagInfo.Instance.GetNormalEquip(mEquipData);
        if (mEquipData != null)
        {
            for (int i = 0; i < equipItems.Count; i++)
            {
                EquipItem item = equipItems[i];
                bag.BagItemInfo info;
                if (mEquipData.ContainsKey(normalEquipIndex[i]))
                {
                    info = mEquipData[normalEquipIndex[i]];
                    equipItems[i].RefreshItem(mEquipData[normalEquipIndex[i]]);
                }
                else
                {
                    equipItems[i].UnInit();
                }
            }
        }
        if (openZhuFu && equipItems != null)
        {
            openZhuFu = false;
            equipItems[0].SelectWeapon();
        }
    }

    void RefreshWLEquipItems()
    {
        CSBagInfo.Instance.GetWoLongEquip(mWLEquipData);
        if (mWLEquipData != null)
        {
            for (int i = 0; i < WLequipItems.Count; i++)
            {
                EquipItem item = WLequipItems[i];
                bag.BagItemInfo info;
                if (mWLEquipData.ContainsKey(wolongEquipIndex[i]))
                {
                    info = mWLEquipData[wolongEquipIndex[i]];
                    WLequipItems[i].RefreshItem(mWLEquipData[wolongEquipIndex[i]]);
                }
                else
                {
                    WLequipItems[i].UnInit();
                }
            }
        }
    }
    void GetRenameBack(uint id, object data)
    {
        lb_rolename.text = CSMainPlayerInfo.Instance.Name;
    }
    void GetModelChange(uint id, object data)
    {
        //obj_roleWeapon.LoadAvatarModel(modelData.roleBrief, AvatarModelType.AMT_Weapon);

        ////设置时装
        //goCloth.LoadAvatarModel(modelData.roleBrief, AvatarModelType.AMT_Cloth);
    }
    void EquipGetOn(uint id, object data)
    {
        bag.EquipItemMsg msg = (bag.EquipItemMsg)data;
        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(msg.equip.configId);
        if (cfg != null)
        {
            if (cfg.type == 2 && cfg.subType == 1) //武器
            {
                ShowWeapon();
            }
            else if (cfg.type == 2 && cfg.subType == 2) //衣服
            {
                ShowCloth();
            }
        }

        if (CSBagInfo.Instance.IsNormalEquip(cfg))
        {
            EquipType = equipType.Normal;
            EquipChange();
            equipItems[msg.position - 1].RefreshItem(msg.equip);
            equipItems[msg.position - 1].ShowTakeOnEffect();
        }

        if (CSBagInfo.Instance.IsWoLongEquip(cfg))
        {
            EquipType = equipType.WoLong;
            EquipChange();
            WLequipItems[msg.position - 101].RefreshItem(msg.equip);
            WLequipItems[msg.position - 101].ShowTakeOnEffect();
        }

        //刷新格子加号
        RefreshAdd();
    }

    void EquipGetOff(uint id, object data)
    {
        bag.UnEquipItemResponse res = (bag.UnEquipItemResponse)data;

        TABLE.ITEM cfg = null;

        if (1 <= res.position && res.position <= 12)
        {
            cfg = equipItems[res.position - 1].itemCfg;
        }
        else if (101 <= res.position && res.position <= 112)
        {
            cfg = WLequipItems[res.position - 101].itemCfg;
        }

        if (cfg != null)
        {
            if (cfg.type == 2 && cfg.subType == 1) //武器
            {
                ShowWeapon();
            }
            else if (cfg.type == 2 && cfg.subType == 2) //衣服
            {
                ShowCloth();
            }
        }

        if (CSBagInfo.Instance.IsNormalEquip(cfg))
        {
            EquipType = equipType.Normal;
            EquipChange();
            equipItems[res.position - 1].UnInit();
        }

        if (CSBagInfo.Instance.IsWoLongEquip(cfg))
        {
            EquipType = equipType.WoLong;
            EquipChange();
            WLequipItems[res.position - 101].UnInit();
        }

        RefreshAdd();
    }

    void EquipChange(uint id, object data)
    {
        bag.EquipInfo res = ((bag.EquipRebuildNtf)data).equip;

        TABLE.ITEM cfg = null;
        if (res.position > 0) return;
        res.position *= -1;

        if (1 <= res.position && res.position <= 12)
        {
            cfg = equipItems[res.position - 1].itemCfg;
        }
        else if (101 <= res.position && res.position <= 112)
        {
            cfg = WLequipItems[res.position - 101].itemCfg;
        }

        if (CSBagInfo.Instance.IsNormalEquip(cfg))
        {
            equipItems[res.position - 1].ChangeParameter(res.equip);
        }

        if (CSBagInfo.Instance.IsWoLongEquip(cfg))
        {
            WLequipItems[res.position - 101].ChangeParameter(res.equip);
        }
    }

    void ItemChange(uint id, object data)
    {
        RefreshAdd();
    }

    void RefreshAdd()
    {
        for (int i = 0; i < equipItems.Count; i++)
        {
            equipItems[i].RefreshAdd();
        }

        for (int i = 0; i < WLequipItems.Count; i++)
        {
            WLequipItems[i].RefreshAdd();
        }
    }
}

public class EquipItem
{
    public bag.BagItemInfo info;
    public TABLE.ITEM itemCfg;
    bool isNull = true;
    public GameObject obj;
    public GameObject bg;
    public UISprite quality;
    public UISprite icon;
    public GameObject add;
    public UILabel forgeLv;
    public GameObject bgEff;
    public GameObject frontEff;
    public UISprite isWoLongSeal;
    public UILabel NormalSuit;
    public int equipPos; //装备位置（item表位置）
    public int gridPos; //格子位置(服务器位置)
    public int childPos; //子物体位置
    private bool isShow; //是否拿来展示用
    TweenScale t_scale;
    static Schedule schedule;
    static long curUid = 0;
    private int type = 0;

    public EquipItem(GameObject _go, int _gridIndex, int _type, bool isView = false)
    {
        isShow = isView;
        obj = _go;
        icon = obj.transform.Find("icon")?.GetComponent<UISprite>();
        quality = obj.transform.Find("icon/sp_qualitybg")?.GetComponent<UISprite>();
        forgeLv = obj.transform.Find("icon/lb_forgeLv")?.GetComponent<UILabel>();
        bgEff = obj.transform.Find("icon/bgEffect")?.gameObject;
        frontEff = obj.transform.Find("icon/frontEffect")?.gameObject;
        add = obj.transform.Find("background/add")?.gameObject;
        isWoLongSeal = obj.transform.Find("icon/sp_dragon")?.GetComponent<UISprite>();
        NormalSuit = obj.transform.Find("icon/lb_suit")?.GetComponent<UILabel>();
        forgeLv.text = "";
        childPos = _gridIndex;

        t_scale = obj.GetComponent<TweenScale>();

        UIEventListener.Get(obj).onClick = ItemClick;

        if (_type == 1)
        {
            equipPos = UIRoleEquipPanel.normalPosIndex[childPos];
            gridPos = UIRoleEquipPanel.normalEquipIndex[childPos];
        }
        else if (_type == 2)
        {
            equipPos = UIRoleEquipPanel.wolongPosIndex[childPos];
            gridPos = UIRoleEquipPanel.wolongEquipIndex[childPos];
        }

        type = _type;
    }

    public void SetPosInfo(int _equipPos, int _gridPos)
    {
    }

    public void RefreshItem(bag.BagItemInfo _info)
    {
        info = _info;
        isNull = false;
        itemCfg = ItemTableManager.Instance.GetItemCfg(info.configId);
        icon.gameObject.SetActive(true);
        icon.spriteName = itemCfg.icon;
        TipClickParams clickParamsre = new TipClickParams(TipsOpenType.RoleEquip, info, itemCfg);
        UIEventListener.Get(obj).parameter = clickParamsre;

        if (CSBagInfo.Instance.IsNormalEquip(itemCfg))
        {
            NormalSuit.text = "";
            isWoLongSeal?.gameObject.SetActive(false);
            if (itemCfg.level >= 30)
            {
                NormalSuit.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(itemCfg.level));
            }
            equipPos = UIRoleEquipPanel.normalPosIndex[childPos];
            gridPos = UIRoleEquipPanel.normalEquipIndex[childPos];
            quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(info.quality);
            int lv = CSEnhanceInfo.Instance.GetEnhanceLv(gridPos);
            forgeLv.text = lv > 0 ? $"+{lv}".BBCode(ColorType.Green) : "";
        }

        if (CSBagInfo.Instance.IsWoLongEquip(itemCfg))
        {
            isWoLongSeal?.gameObject.SetActive(true);
            isWoLongSeal.spriteName = CSBagInfo.Instance.GetItemBaseWoLongIconName(itemCfg.levClass);
            NormalSuit.text = "";
            equipPos = UIRoleEquipPanel.wolongPosIndex[childPos];
            gridPos = UIRoleEquipPanel.wolongEquipIndex[childPos];
            quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(itemCfg.quality);
        }

        ChangeAddState();
    }

    public void RefreshAdd()
    {
        ChangeAddState();
    }

    public void ChangeParameter(bag.BagItemInfo _info)
    {
        TipClickParams clickParamsre = new TipClickParams(TipsOpenType.RoleEquip, _info, itemCfg);
        UIEventListener.Get(obj).parameter = clickParamsre;
    }

    void ChangeAddState()
    {
        if (!isNull) return;
        add.SetActive(CSBagInfo.Instance.IsHasSamePosEquipInBag(equipPos));
    }

    public void UnInit()
    {
        Timer.Instance.CancelInvoke(schedule);
        CSEffectPlayMgr.Instance.Recycle(bgEff);
        bgEff.SetActive(false);
        isNull = true;
        forgeLv.text = "";
        icon.gameObject.SetActive(false);
        if (isShow) return;
        ChangeAddState();
    }

    void ItemClick(GameObject _go)
    {
        if (isShow)
        {
            if (isNull) return;
            TipClickParams data = (TipClickParams)UIEventListener.Get(obj).parameter;
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, data.cfg, data.info, gridPos);
            return;
        }

        if (isNull)
        {
            bag.BagItemInfo t_info = CSBagInfo.Instance.GetBestEquipByPos(equipPos);
            if (t_info != null)
            {
                Net.ReqEquipItemMessage(t_info.bagIndex, gridPos, 0, t_info);
            }
            else
            {
                switch (type)
                {
                    case 1://普通装备
                        if (CSBagInfo.Instance.IsHasNormalEquipObtain())
                        {
                            UIManager.Instance.CreatePanel<UIRoleEquipObtainPanel>(p =>
                            {
                                (p as UIRoleEquipObtainPanel).ShowRoleEquipObtain(EquipType.Normal, gridPos);
                            });
                        }
                        else
                        {
                            UtilityTips.ShowRedTips(1316);
                        }
                        break;
                    case 2://卧龙装备
                        if (CSBagInfo.Instance.IsHasWolongEquipObtain())
                        {
                            UIManager.Instance.CreatePanel<UIRoleEquipObtainPanel>(p =>
                            {
                                (p as UIRoleEquipObtainPanel).ShowRoleEquipObtain(EquipType.Wolong, gridPos - 100);
                            });
                        }
                        else
                        {
                            UtilityTips.ShowRedTips(1316);
                        }
                        break;
                }

            }
        }
        else
        {
            //Debug.Log($"单击   {itemCfg.name} {info.id} {Timer.Instance.IsInvoking(schedule)}   {curUid}");
            if (!Timer.Instance.IsInvoking(schedule))
            {
                curUid = info.id;
                schedule = Timer.Instance.Invoke(0.2f, DelayClick);
            }
            else
            {
                if (info != null)
                {
                    //Debug.Log($"二次点击 取消延时  {itemCfg.name}  {info.id}   {curUid}");
                    Timer.Instance.CancelInvoke(schedule);
                    if (curUid != info.id)
                    {
                        curUid = info.id;
                        schedule = Timer.Instance.Invoke(0.2f, DelayClick);
                    }
                    else
                    {
                        if (isNull)
                        {
                            return;
                        }
                        Net.ReqUnEquipItemMessage(gridPos);
                    }
                }
            }
        }
    }

    void DelayClick(Schedule _schedule)
    {
        //Debug.Log("延时方法执行    " + itemCfg.name);
        Timer.Instance.CancelInvoke(schedule);
        TipClickParams data = (TipClickParams)UIEventListener.Get(obj).parameter;
        UITipsManager.Instance.CreateTips(TipsOpenType.RoleEquip, data.cfg, data.info, gridPos);
    }

    void ItemDoubleClick(GameObject _go)
    {
        if (isNull)
        {
            return;
        }
        Net.ReqUnEquipItemMessage(gridPos);
    }

    public void ShowTakeOnEffect()
    {
        bgEff.SetActive(true);
        CSEffectPlayMgr.Instance.ShowUIEffect(bgEff, "addlingshi", 10, false);
        t_scale.from = new Vector3(1.05f, 1.05f, 1.05f);
        t_scale.to = Vector3.one;
        t_scale.duration = 0.2f;
        t_scale.PlayTween();
    }
    public void SelectWeapon()
    {
        TipClickParams data = (TipClickParams)UIEventListener.Get(obj).parameter;
        UITipsManager.Instance.CreateTips(TipsOpenType.RoleEquip, data.cfg, data.info, gridPos, () => { HotManager.Instance.EventHandler.SendEvent(CEvent.OpenZhuFuYou); });
    }
    public void ShowAdd(bool _state)
    {
        add.SetActive(_state);
    }
}