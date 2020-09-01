using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;
using UnityEngine;


public class UIItemBase
{
    public UIItemBase(PropItemType _type)
    {
        type = _type;
    }

    public UIItemBase(GameObject _go, PropItemType _type)
    {
        type = _type;
        obj = _go;
        obj_trans = obj.transform;
    }


    #region

    BoxCollider _collder;

    BoxCollider collider
    {
        get { return _collder ?? (_collder = obj.GetComponent<BoxCollider>()); }
    }

    UISprite _bg;

    UISprite bg
    {
        get { return _bg ?? (_bg = obj.GetComponent<UISprite>()); }
    }

    GameObject _obj_lock;

    GameObject obj_lock
    {
        get { return _obj_lock ?? (_obj_lock = obj.transform.Find("lock").gameObject); }
    }

    UISprite _sp_quality;

    UISprite sp_quality
    {
        get { return _sp_quality ?? (_sp_quality = obj.transform.Find("sp_quality").GetComponent<UISprite>()); }
    }

    GameObject _obj_arrow;

    GameObject obj_arrow
    {
        get { return _obj_arrow ?? (_obj_arrow = obj.transform.Find("arrow").gameObject); }
    }

    UISprite _sp_Icon;

    UISprite sp_Icon
    {
        get { return _sp_Icon ?? (_sp_Icon = obj.transform.Find("sp_itemicon").GetComponent<UISprite>()); }
    }

    UILabel _count;

    UILabel lb_count
    {
        get { return _count ?? (_count = obj.transform.Find("lb_count").GetComponent<UILabel>()); }
    }

    UILabel _lb_strengthen;

    UILabel lb_strengthen
    {
        get { return _lb_strengthen ?? (_lb_strengthen = obj.transform.Find("lb_strengthen").GetComponent<UILabel>()); }
    }

    GameObject _obj_choose;

    GameObject obj_choose
    {
        get { return _obj_choose ?? (_obj_choose = obj.transform.Find("select").gameObject); }
    }

    UISprite _sp_choose;

    UISprite sp_choose
    {
        get { return _sp_choose ?? (_sp_choose = obj.transform.Find("select/bg").GetComponent<UISprite>()); }
    }

    GameObject _obj_effect;

    GameObject obj_effect
    {
        get { return _obj_effect ?? (_obj_effect = obj.transform.Find("effect").gameObject); }
    }

    UISprite _sp_effect;

    UISprite sp_effect
    {
        get { return _sp_effect ?? (_sp_effect = obj.transform.Find("effect").GetComponent<UISprite>()); }
    }


    UISprite _obj_mask;

    UISprite obj_mask
    {
        get { return _obj_mask ?? (_obj_mask = obj.transform.Find("mask").GetComponent<UISprite>()); }
    }

    GameObject _obj_CancerEqual;

    GameObject obj_CancerEqual
    {
        get { return _obj_CancerEqual ?? (_obj_CancerEqual = obj.transform.Find("sp_stop")?.gameObject); }
    }

    UISprite cdMask;

    UISprite CdMask
    {
        get { return cdMask ?? (cdMask = obj.transform.Find("cdmask").GetComponent<UISprite>()); }
    }

    UILabel cdTime;

    UILabel CdTime
    {
        get { return cdTime ?? (cdTime = CdMask.transform.Find("time").GetComponent<UILabel>()); }
    }

    GameObject _obj_redPoint;

    GameObject obj_redPoint
    {
        get { return _obj_redPoint ?? (_obj_redPoint = obj.transform.Find("redpoint").gameObject); }
    }

    UIDragScrollView drag;

    UIDragScrollView DragCom
    {
        get { return drag ?? (drag = obj.GetComponent<UIDragScrollView>()); }
    }

    private GameObject _sp_lock_mini;


    GameObject sp_lock_mini
    {
        get { return _sp_lock_mini ?? (_sp_lock_mini = obj.transform.Find("sp_lock_mini").gameObject); }
    }

    UISprite _is_WoLongEquip;

    UISprite is_WoLongEquip
    {
        get { return _is_WoLongEquip ?? (_is_WoLongEquip = obj.transform.Find("sp_dragon").GetComponent<UISprite>()); }
    }
    UILabel _lb_normalSuit;
    UILabel lb_normalSuit
    {
        get { return _lb_normalSuit ?? (_lb_normalSuit = obj.transform.Find("lb_suit").GetComponent<UILabel>()); }
    }

    UISprite _sp_nostalgia_flag;

    UISprite sp_nostalgia_flag
    {
        get
        {
            return _sp_nostalgia_flag ??
                   (_sp_nostalgia_flag = obj.transform.Find("sp_nostalgia_flag").GetComponent<UISprite>());
        }
    }

    UISprite _sp_nostalgia_num;

    UISprite sp_nostalgia_num
    {
        get
        {
            return _sp_nostalgia_num ??
                   (_sp_nostalgia_num = obj.transform.Find("sp_nostalgia_num").GetComponent<UISprite>());
        }
    }

    #endregion

    public itemSize size;
    public PropItemType type;
    public GameObject obj;
    public Transform obj_trans;
    public Action<UIItemBase> OnClick;
    public Action<UIItemBase> DoubleClick;
    public Action<UIItemBase> OnPress;

    TABLE.ITEM _itemCfg = null;

    public TABLE.ITEM itemCfg
    {
        get { return _itemCfg; }
        set
        {
            if (null != _itemCfg)
            {
                StopCD();
                ItemCDManager.Instance.RemoveAction(itemCfg.group, CDUpdate, StopCD);
                _itemCfg = null;
            }

            _itemCfg = value;
        }
    }

    public bag.BagItemInfo infos;
    Vector2 vector;
    Vector3 startPos;
    System.Action Update;
    public EventHanlderManager mSocketEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Socket);
    public EventHanlderManager EventHandler = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    float intervalTime = 0.2f;
    PropItemState itemState = PropItemState.Normal;
    public int lockNum = 0;
    bool NeedTips = false;
    static Schedule schedule;
    static long curUid = 0;
    bool needDrag = true;
    bool hasCD = false;
    public bool IsHuaijiuIcon = true; //是否显示怀旧装备的图标 ,如果不显示需要在refresh前调用
    private bool _chooseState;

    /// <summary>
    /// 当先勾选状态
    /// </summary>
    public bool chooseState
    {
        get => _chooseState;
        set => _chooseState = value;
    }

    public void UnInit()
    {
        HasCD = false;
        if (null != itemCfg)
            ItemCDManager.Instance.RemoveAction(itemCfg.group, CDUpdate, StopCD);
        StopCD();
        Gray(false);
        CSEffectPlayMgr.Instance.Recycle(obj_arrow);
        CSEffectPlayMgr.Instance.Recycle(obj_effect);
        itemState = PropItemState.Normal;
        sp_Icon.spriteName = "";
        sp_quality.spriteName = "";
        //obj_effect.SetActive(false);
        lb_count.text = "";
        lb_count.color = CSColor.white;
        lb_strengthen.text = "";
        lb_normalSuit.text = "";
        obj_lock.SetActive(false);
        obj_arrow.SetActive(false);
        obj_choose.SetActive(false);
        chooseState = false;
        obj_mask.color = CSColor.white;
        obj_mask.gameObject.SetActive(false);
        obj_redPoint.SetActive(false);
        obj_effect.SetActive(false);
        obj_CancerEqual?.SetActive(false);
        SetMiniLock(false);
        is_WoLongEquip?.gameObject.SetActive(false);
        needDrag = true;
        DragCom.enabled = needDrag;
        lockNum = 0;
        DoubleClick = null;
        OnPress = null;
        OnClick = null;
        NeedTips = false;
        itemCfg = null;
        infos = null;
        ExtendData = null;
        isShowWoLongSeal = true;
        hasCD = false;
        maskJudgeState = false;
        IsHuaijiuIcon = true;
        sp_nostalgia_flag.gameObject.SetActive(false);
        sp_nostalgia_num.gameObject.SetActive(false);
        isShowNormalSuit = true;
    }

    #region 外部设置方法

    public void CancelDrag()
    {
        UIEventListener.Get(obj).onDragStart = null;
        UIEventListener.Get(obj).onDrag = null;
        UIEventListener.Get(obj).onDragEnd = null;
    }

    public void ListenDrag()
    {
        UIEventListener.Get(obj).onDragStart = DragStart;
        UIEventListener.Get(obj).onDrag = Drag;
        UIEventListener.Get(obj).onDragEnd = DragEnd;
    }

    Vector3 countVec = new Vector3();

    public void SetSize(itemSize _size)
    {
        if (size == _size)
        {
            return;
        }

        size = _size;
        int sizeNum = (int)size;
        if (size == itemSize.Default)
        {
            collider.size = new Vector3(80, 79, 0);
        }
        else
        {
            collider.size = new Vector3(sizeNum, sizeNum, 0);
        }

        bg.width = sizeNum;
        bg.height = sizeNum;
        sp_quality.width = sizeNum - 4;
        sp_quality.height = sizeNum - 4;
        sp_Icon.width = sizeNum - 4;
        sp_Icon.height = sizeNum - 4;
        obj_mask.width = sizeNum - 4;
        obj_mask.height = sizeNum - 4;
        CdMask.width = sizeNum - 4;
        CdMask.height = sizeNum - 4;
        sp_choose.width = sizeNum - 4;
        sp_choose.height = sizeNum - 4;
        sp_effect.width = sizeNum + 22;
        sp_effect.height = sizeNum + 22;
        countVec.x = (sizeNum / 2) - 2;
        countVec.y = -(sizeNum / 2) + 1;
        lb_count.transform.localPosition = countVec;
    }

    public void SetParent(Transform _parent)
    {
        if (obj_trans != null)
        {
            obj_trans.SetParent(_parent);
            obj_trans.localPosition = Vector3.zero;
            obj_trans.localScale = Vector3.one;
        }
    }

    public void ChangeDragCom(bool _state)
    {
        needDrag = _state;
        DragCom.enabled = needDrag;
    }

    public void SetMiniLock(bool _state)
    {
        sp_lock_mini.SetActive(_state);
    }

    #endregion

    #region 刷新

    public void Refresh(bag.BagItemInfo _msg = null, Action<UIItemBase> _action = null, bool needTips = true,
        bool isEffect = false)
    {
        itemState = PropItemState.Normal;
        if (_msg == null)
        {
            itemState = PropItemState.Unlock;
        }

        infos = (_msg != null) ? _msg : null;
        if (null == _msg)
        {
            UnInit();
            return;
        }

        itemCfg = ItemTableManager.Instance.GetItemCfg(infos.configId);
        if (null == itemCfg)
        {
            UnInit();
            return;
        }

        Refresh(_action, needTips, isEffect);
    }

    public void Refresh(TABLE.ITEM _itemCfg, Action<UIItemBase> _action = null, bool needTips = true,
        bool isEffect = false)
    {
        itemCfg = _itemCfg;
        if (null == itemCfg)
        {
            return;
        }

        Refresh(_action, needTips, isEffect);
    }

    public void Refresh(int _config, Action<UIItemBase> _action = null, bool needTips = true, bool isEffect = false)
    {
        itemCfg = ItemTableManager.Instance.GetItemCfg(_config);
        Refresh(itemCfg, _action, needTips, isEffect);
    }

    private void Refresh(Action<UIItemBase> _action, bool needTips, bool isEffect)
    {
        NeedTips = needTips;
        OnClick = _action;
        ShowIcon();
        ShowCount();
        ShowStrengthenNum();
        ShowLock();
        ShowArrow();
        ShowSelect(false);
        ShowMask();
        CheckCD();
        ShowRedPoint();
        ShowEffect(isEffect);
        ShowWoLongEquipSeal();
        ShowNormalSuit();
        DragCom.enabled = needDrag;
        UIEventListener.Get(obj).onClick = Click;
        UIEventListener.Get(obj).onKeepPress = Press;
        //判断如果是怀旧装备显示阶级信息
        if (itemCfg.type == 10)
        {
            ShowHuaiJiuInfo(itemCfg);
        }
    }


    public void ShowHuaiJiuInfo(ITEM item)
    {
        if (IsHuaijiuIcon)
        {
            HUAIJIUSUIT huaijiusuit;
            if (HuaiJiuSuitTableManager.Instance.TryGetValue(item.huaiJiuSuit, out huaijiusuit))
            {
                sp_nostalgia_num.spriteName = huaijiusuit.pic;
                sp_nostalgia_flag.spriteName = $"1010{huaijiusuit.type}";
            }
        }
        sp_nostalgia_num.gameObject.SetActive(IsHuaijiuIcon);
        sp_nostalgia_flag.gameObject.SetActive(IsHuaijiuIcon);
    }

    public object ExtendData;

    public void SetExtendData(object _data)
    {
        ExtendData = _data;
    }

    public object ExtendData1;

    public void SetExtend1Data(object _data)
    {
        ExtendData1 = _data;
    }

    #endregion

    #region 单机双击

    public void SetClick()
    {
        UIEventListener.Get(obj).onClick = Click;
    }

    public void SetItemDBClickedCB(Action<UIItemBase> _action)
    {
        DoubleClick = _action;
    }

    public void SetOnPress(Action<UIItemBase> _action)
    {
        OnPress = _action;
    }

    void DelayClick(Schedule _schedule)
    {
        //Debug.Log("延时方法执行    " + itemCfg.name);
        Timer.Instance.CancelInvoke(schedule);
        if (itemState == PropItemState.Unlock)
        {
            //不可选中状态不执行点击事件
            //UtilityTips.ShowRedTips("尚未解锁");
            GetCost();
            return;
        }

        else if (itemState == PropItemState.Normal)
        {
            itemState = PropItemState.Selected;
        }
        else if (itemState == PropItemState.Selected)
        {
            itemState = PropItemState.Normal;
        }

        if (itemCfg == null)
        {
            return;
        }

        if (OnClick != null)
        {
            OnClick(this);
        }

        StateChange();
    }

    private void Press(GameObject arg1)
    {
        if (OnPress == null)
            return;

        OnPress(this);
    }

    void Click(GameObject go)
    {
        // Debug.Log($"单击   {itemCfg.name} {infos.id} {Timer.Instance.IsInvoking(schedule)}   {curUid}");
        // Debug.Log($"单击   {itemCfg.name} {Timer.Instance.IsInvoking(schedule)}   {curUid}");
        if (!Timer.Instance.IsInvoking(schedule))
        {
            if (infos != null)
            {
                curUid = infos.id;
            }

            schedule = Timer.Instance.Invoke(intervalTime, DelayClick);
        }
        else
        {
            if (infos != null)
            {
                // Debug.Log($"二次点击 取消延时  {itemCfg.name}  {infos.id}   {curUid}");
                Timer.Instance.CancelInvoke(schedule);

                if (curUid != infos.id)
                {
                    curUid = infos.id;
                    schedule = Timer.Instance.Invoke(intervalTime, DelayClick);
                }
                else
                {
                    if (itemCfg == null)
                    {
                        return;
                    }

                    if (type == PropItemType.Bag)
                    {
                        if (itemCfg.type == (int)ItemType.Box) //如果是宝箱 发送点击宝箱的消息
                        {
                            CSBagInfo.Instance.CancelNewBoxRedPoint(infos.id);
                            EventData data = CSEventObjectManager.Instance.SetValue(infos, ItemChangeType.NumAdd);
                            EventHandler.SendEvent(CEvent.BagBoxItemClick, data);
                            CSEventObjectManager.Instance.Recycle(data);
                        }
                    }

                    if (DoubleClick != null)
                    {
                        DoubleClick(this);
                    }
                }
            }
            else
            {
                // Debug.Log($"二次点击 取消延时  {itemCfg.name} {curUid}");
                Timer.Instance.CancelInvoke(schedule);
                if (DoubleClick != null && _itemCfg != null)
                    DoubleClick(this);
            }
        }
    }

    #endregion

    #region 转态切换

    void StateChange()
    {
        if (type == PropItemType.Recycle)
        {
            //ShowSelect(itemState == PropItemState.Selected ? true : false);
        }
        else if (type == PropItemType.Normal)
        {
            if (NeedTips)
            {
                UITipsManager.Instance.CreateTips(TipsOpenType.Normal, itemCfg, infos);
            }
        }
        else if (type == PropItemType.Bag)
        {
            if (itemCfg.type == (int)ItemType.Box) //如果是宝箱 发送点击宝箱的消息
            {
                CSBagInfo.Instance.CancelNewBoxRedPoint(infos.id);
                EventData data = CSEventObjectManager.Instance.SetValue(infos, ItemChangeType.NumAdd);
                EventHandler.SendEvent(CEvent.BagBoxItemClick, data);
                CSEventObjectManager.Instance.Recycle(data);
            }
        }
    }

    public void ChangeType(PropItemType _type)
    {
        type = _type;
    }

    public PropItemState GetItemState()
    {
        return itemState;
    }

    public void SetItemState(PropItemState eState)
    {
        if (itemState != eState)
        {
            itemState = eState;
            StateChange();
        }
    }

    #endregion

    #region 刷新

    //刷新品质 icon 特效
    public void ShowIconFashion(int id)
    {
        if (FashionTableManager.Instance[id] != null)
        {
            sp_Icon.spriteName = FashionTableManager.Instance[id].icon;
            sp_quality.spriteName =
                ItemTableManager.Instance.GetItemQualityBG(FashionTableManager.Instance[id].quality);
        }
    }

    const float grayValue = 76.0f / 255.0f;
    const float minValue = 1.0f / 255.0f;

    public void Gray(bool gray)
    {
        obj_mask.CustomActive(gray);
        obj_mask.color = gray ? new Color(minValue, 0, 0, grayValue) : Color.white;
    }

    public void IconGray()
    {
        sp_Icon.color = Color.black;
    }

    void ShowIcon()
    {
        sp_Icon.color = Color.white;
        sp_Icon.spriteName = itemCfg.icon;
        sp_Icon.gameObject.SetActive(true);
        sp_quality.gameObject.SetActive(true);
        if (itemCfg.type == 2 && infos != null)
        {
            if (CSBagInfo.Instance.IsNormalEquip(itemCfg))
            {
                sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(infos.quality);
            }
            else
            {
                sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(itemCfg.quality);
            }
        }
        else
        {
            if (itemCfg.type == (int)ItemType.Handbook && ExtendData1 != null)
            {
                AuctionItemData info = (AuctionItemData)ExtendData1;
                if (info.type == 2)
                {
                    int qua = (int)((info.handbookId << 0) >> 24);
                    sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(qua);
                }
                else
                {
                    sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(itemCfg.quality);
                }
            }
            else
            {
                sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(itemCfg.quality);
            }
        }

        obj_effect.SetActive(false);
    }

    public void SetQuality(int _qua)
    {
        sp_quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(_qua);
    }

    void ShowCount()
    {
        lb_count.text = "";
        if (infos != null)
        {
            if (itemCfg.type != 2 && infos.count > 1)
            {
                lb_count.text = UtilityMath.GetDecimalTenThousandValue(infos.count);
            }
        }
    }

    void ShowStrengthenNum()
    {
        if (type == PropItemType.EquipShow)
        {
            lb_strengthen.text = "+0";
        }
        else
        {
            lb_strengthen.text = "";
        }
    }

    private void ShowLock()
    {
        obj_lock.SetActive(itemState == PropItemState.Unlock);
    }

    public void ShowArrow(bool forceShow = false)
    {
        bool showArrow = false;
        forceShow = forceShow && CSBagInfo.Instance.GetEquipCompareResult(itemCfg, infos);
        if (forceShow)
        {
            CSEffectPlayMgr.Instance.Recycle(obj_arrow);
            CSEffectPlayMgr.Instance.ShowUIEffect(obj_arrow, "publicup");
        }

        if (type == PropItemType.Bag)
        {
            if (itemCfg.type == (int)ItemType.Equip)
            {
                if (itemCfg.career == CSMainPlayerInfo.Instance.Career || itemCfg.career == 0)
                {
                    if (itemCfg.sex == 2 || itemCfg.sex == CSMainPlayerInfo.Instance.Sex)
                    {
                        if (itemCfg.level <= CSMainPlayerInfo.Instance.Level)
                        {
                            showArrow = CSBagInfo.Instance.GetEquipCompareResult(itemCfg, infos);
                        }
                        //Debug.Log($"{itemCfg.name} {showArrow}");
                    }
                }

                obj_arrow.SetActive(showArrow);
                if (showArrow)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(obj_arrow, "publicup");
                }
            }
            //判断宝石是否显示arrow
            else if (itemCfg.type == (int)ItemType.Gem)
            {
                if (itemCfg.subType >= 1 && itemCfg.subType <= 5)
                {
                    int needLevel = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(47);
                    if (CSMainPlayerInfo.Instance.Level < needLevel)
                        return;
                    if (CSGemInfo.Instance.judgebodyGemByBag(itemCfg.id))
                    {
                        showArrow = true;
                    }

                }
                else if (itemCfg.subType >= 6 && itemCfg.subType <= 8)
                {
                    int needLevel = FuncOpenTableManager.Instance.GetFuncOpenNeedLevel(61);
                    if (CSMainPlayerInfo.Instance.Level < needLevel)
                        return;
                    if (CSWingInfo.Instance.judgebodyWingSoulByBag(itemCfg.id, itemCfg.subType - 5))
                        showArrow = true;
                }

                obj_arrow.SetActive(showArrow);
                if (showArrow)
                    CSEffectPlayMgr.Instance.ShowUIEffect(obj_arrow, "publicup");
            }

            else if (itemCfg.type == (int)ItemType.NostalgicEquip)
            {
                bool isShow = CSNostalgiaEquipInfo.Instance.IsRepleace(itemCfg.huaiJiuSuit, itemCfg.subType) != 0;
                //FNDebug.Log($"显示箭头{isShow}");
                obj_arrow.SetActive(isShow);
                if (isShow)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(obj_arrow, "publicup");
                }
            }
            else
            {
                obj_arrow.SetActive(false);
            }
        }
        else
        {
            obj_arrow.SetActive(forceShow);
        }
    }

    public void ShowSelect(bool _state)
    {
        if (obj_choose.activeSelf != _state)
        {
            obj_choose.SetActive(_state);
            chooseState = _state;
        }
    }

    bool maskJudgeState = false;
    public void SetMaskJudgeState(bool _maskJudgeState)
    {
        maskJudgeState = _maskJudgeState;
    }
    void ShowMask()
    {
        //type == PropItemType.Bag || type == PropItemType.Warehouse || type == PropItemType.GuildWarehouse ||type == PropItemType.GuildBag
        if (maskJudgeState)
        {
            JudgeMask();
        }
        else
        {
            obj_CancerEqual?.SetActive(false);
            obj_mask.gameObject.SetActive(false);
        }
    }

    void JudgeMask()
    {
        if (itemCfg.type == (int)ItemType.Equip)
        {
            obj_CancerEqual?.SetActive(!CSBagInfo.Instance.IsEquipCareerSexEqual(itemCfg));
            if (CSBagInfo.Instance.IsWoLongEquip(itemCfg))
            {
                if (CSWoLongInfo.Instance.GetWoLongLevel() < itemCfg.wolongLv)
                {
                    obj_mask.gameObject.SetActive(true);
                    Color nowColor;
                    ColorUtility.TryParseHtmlString("#F10F0F", out nowColor);
                    obj_mask.color = nowColor;
                    obj_mask.alpha = 76 / 255f;
                }
                else
                {
                    obj_mask.gameObject.SetActive(false);
                }
            }
            else
            {
                if (CSMainPlayerInfo.Instance.Level < itemCfg.level)
                {
                    obj_mask.gameObject.SetActive(true);
                    Color nowColor;
                    ColorUtility.TryParseHtmlString("#F10F0F", out nowColor);
                    obj_mask.color = nowColor;
                    obj_mask.alpha = 76 / 255f;
                }
                else
                {
                    obj_mask.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (((itemCfg.career != 0) && itemCfg.career != CSMainPlayerInfo.Instance.Career) ||
                (itemCfg.sex != 2 && itemCfg.sex != CSMainPlayerInfo.Instance.Sex))
            {
                obj_CancerEqual?.SetActive(true);
            }
            else
            {
                obj_CancerEqual?.SetActive(false);
            }

            if (CSMainPlayerInfo.Instance.Level < itemCfg.level)
            {
                obj_mask.gameObject.SetActive(true);
                Color nowColor;
                ColorUtility.TryParseHtmlString("#F10F0F", out nowColor);
                obj_mask.color = nowColor;
                obj_mask.alpha = 76 / 255f;
            }
            else
            {
                obj_mask.gameObject.SetActive(false);
            }
        }
    }

    public void JudgeMaskWithoutType()
    {
        JudgeMask();
    }

    public int CDGroup
    {
        get { return 4; }
    }


    public bool HasCD
    {
        get { return hasCD; }
        set { hasCD = true; }
    }

    void CheckCD()
    {
        StopCD();
        ItemCDManager.Instance.RemoveAction(itemCfg.group, CDUpdate, StopCD);
        if (HasCD && null != itemCfg && itemCfg.itemcd > 0)
        {
            if (null != itemCfg)
            {
                ItemCDManager.Instance.AddAction(itemCfg.group, CDUpdate, StopCD);
            }
        }
    }

    void ShowRedPoint()
    {
        if (type == PropItemType.Bag)
        {
            //暂时没有规则，不显示
            if (itemCfg.type == (int)ItemType.Box && infos != null)
            {
                obj_redPoint.SetActive(CSBagInfo.Instance.GetBoxRedStateById(infos.id));
            }
            else
            {
                obj_redPoint.SetActive(false);
            }
        }
        else
        {
            obj_redPoint.SetActive(false);
        }
    }

    public void ShowRedPoint(bool isActive)
    {
        obj_redPoint.SetActive(isActive);
    }

    private void ShowEffect(bool isEffect)
    {
        if (isEffect && !string.IsNullOrEmpty(itemCfg.effect))
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(_obj_effect, int.Parse(itemCfg.effect));
        }

        _obj_effect.SetActive(isEffect);
    }

    bool isShowWoLongSeal = true;

    public bool IsShowWoLongSeal
    {
        get => isShowWoLongSeal;
        set => isShowWoLongSeal = value;
    }

    void ShowWoLongEquipSeal()
    {
        if (isShowWoLongSeal && CSBagInfo.Instance.IsWoLongEquip(itemCfg))
        {
            is_WoLongEquip.gameObject.SetActive(true);
            is_WoLongEquip.spriteName = CSBagInfo.Instance.GetItemBaseWoLongIconName(itemCfg.levClass);
        }
        else
        {
            is_WoLongEquip.gameObject.SetActive(false);
        }
    }
    public bool isShowNormalSuit = true;
    void ShowNormalSuit()
    {
        if (isShowNormalSuit && CSBagInfo.Instance.IsNormalEquip(itemCfg) && itemCfg.level >= 30)
        {
            lb_normalSuit.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(itemCfg.level));
        }
        else
        {
            lb_normalSuit.text = "";
        }
    }

    public void ShowOrHideIconAndQuality(bool isShow)
    {
        sp_Icon.gameObject.SetActive(isShow);
        sp_quality.gameObject.SetActive(isShow);
        if (isShow)
            ShowWoLongEquipSeal();
        else
            is_WoLongEquip.gameObject.SetActive(false);
    }

    #endregion

    #region 设置数量

    public void SetCount(long _count, Color color, bool hideIfCountLessThanZero = false)
    {
        //if (_count <= 1)
        //{
        //    return;
        //}
        if (hideIfCountLessThanZero && _count <= 0)
        {
            lb_count.text = string.Empty;
        }
        else
        {
            lb_count.text = UtilityMath.GetItemBaseCount(_count);
        }

        lb_count.color = color;
    }

    public void SetCount(long _count, bool isShowOne = false, bool isGetDecimalValue = true)
    {
        if (_count == 1 && !isShowOne)
        {
            lb_count.text = "";
            return;
        }

        if (_count <= 0)
        {
            lb_count.text = "";
            return;
        }

        lb_count.text = isGetDecimalValue ? UtilityMath.GetItemBaseCount(_count) : _count.ToString();
    }

    public void SetCount(string _count, Color color)
    {
        lb_count.text = _count;
        lb_count.color = color;
    }

    public void SetCount(int curCount, int maxCount)
    {
        if (curCount >= 0 && maxCount >= 0)
        {
            string cur = UtilityMath.GetDecimalTenThousandValue(curCount);
            string max = UtilityMath.GetDecimalTenThousandValue(maxCount);
            lb_count.text = $"{cur}/{max}";
            Color color = UtilityColor.GetColor(curCount >= maxCount ? ColorType.Green : ColorType.Red);
            lb_count.color = color;
        }
    }

    // /// <summary>
    // /// 货币和物品显示数量形式需要不一样的特殊需求
    // /// </summary>
    // /// <param name="cfg"></param>
    // /// <param name="curCount"></param>
    // /// <param name="maxCount"></param>
    // public void SetCount(TABLE.ITEM cfg, int curCount, int maxCount)
    // {
    //     switch (cfg.type)
    //     {
    //         case 1://货币
    //             lb_count.text = UtilityMath.GetDecimalTenThousandValue(maxCount);
    //             break;
    //         default:
    //             SetCount(curCount, maxCount);
    //             break;
    //     }
    // }

    #endregion

    #region 格子锁定

    public void ShowUnlock(int _num)
    {
        lockNum = _num;
        itemState = PropItemState.Unlock;
        ShowLock();
    }

    public void ShowHuaiJiuLock(int _num, Action<GameObject> Click)
    {
        lockNum = _num;
        obj_lock.SetActive(true);
        UIEventListener.Get(obj, lockNum).onClick = Click;
    }


    List<int> cost;
    bool isCostEnough = false;
    int lockcostId = 0;

    void GetCost()
    {
        //int result = 0;
        int startNum = lockNum - 50;
        string str;
        int costCount = GetSingleCost();
        if (isCostEnough)
        {
            str = $"[00ff00]{costCount}";
        }
        else
        {
            str = $"[ff0000]{costCount}";
        }

        if (type == PropItemType.Bag) //默认解锁75格
        {
            lockcostId = int.Parse(SundryTableManager.Instance.GetSundryEffect(63));
            UtilityTips.ShowPromptWordTips(71, ConfirmUnlockItems, str,
                ItemTableManager.Instance.GetItemName(lockcostId));
        }
        else if (type == PropItemType.Warehouse) //默认解锁50格
        {
            UtilityTips.ShowPromptWordTips(3, ConfirmUnlockItems, str);
        }
    }

    void ConfirmUnlockItems()
    {
        if (type == PropItemType.Bag)
        {
            if (CSBagInfo.Instance.GetItemCount(lockcostId) >= GetSingleCost())
            {
                Net.AddBagCountReqMessage(lockNum - CSBagInfo.Instance.GetMaxCount());
            }
            else
            {
                Utility.ShowGetWay(lockcostId);
                //不要首冲判断，直接弹获取途径
                //if (!CSVipInfo.Instance.IsFirstRecharge())
                //{
                //    UtilityTips.ShowPromptWordTips(5, () =>
                //    {
                //        UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
                //        UIManager.Instance.ClosePanel<UIBagPanel>();
                //    });
                //    return;
                //}
                //else
                //{
                //    UtilityTips.ShowPromptWordTips(6, () =>
                //    {
                //        UtilityPanel.JumpToPanel(12305);
                //        UIManager.Instance.ClosePanel<UIBagPanel>();
                //    });
                //}
            }
        }
        else if (type == PropItemType.Warehouse)
        {
            if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.gold) >= GetSingleCost())
            {
                Net.ReqAddStorehouseCountMessage(lockNum - CSStorehouseInfo.Instance.GetMaxCount());
            }
            else
            {
                Utility.ShowGetWay(3);
                //不要首冲判断，直接弹获取途径
                //if (!CSVipInfo.Instance.IsFirstRecharge())
                //{
                //    UtilityTips.ShowPromptWordTips(5, () =>
                //    {
                //        UIManager.Instance.CreatePanel<UIRechargeFirstPanel>();
                //        UIManager.Instance.ClosePanel<UIBagPanel>();
                //    });
                //    return;
                //}

                //UtilityTips.ShowPromptWordTips(6, () =>
                //{
                //    UtilityPanel.JumpToPanel(12305);
                //    UIManager.Instance.ClosePanel<UIBagPanel>();
                //});
            }
        }
    }

    int GetSingleCost()
    {
        long selfNum = 0;
        if (type == PropItemType.Warehouse)
        {
            cost = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetDes(71), '#');
            selfNum = CSItemCountManager.Instance.GetItemCount((int)MoneyType.gold);
        }
        else
        {
            cost = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetDes(62), '#');
            int costId = int.Parse(SundryTableManager.Instance.GetSundryEffect(63));
            selfNum = CSItemCountManager.Instance.GetItemCount(costId);
        }

        //已经扩大的数量
        int startNum = (type == PropItemType.Warehouse
            ? CSStorehouseInfo.Instance.GetMaxCount()
            : CSBagInfo.Instance.GetMaxCount());
        int gap = (type == PropItemType.Warehouse ? 50 : 75);
        int count = lockNum - startNum;

        int price = 0;
        for (int i = startNum + 1; i <= lockNum; i++)
        {
            double ind = Math.Ceiling((i - gap) / 5f) - 1;
            int addPrice = cost[(int)ind];
            price += addPrice;
            //Debug.Log(i+"  "+ lockNum + "   "+ind+"   "+addPrice);
        }

        if (selfNum >= price)
        {
            isCostEnough = true;
        }
        else
        {
            isCostEnough = false;
        }

        return price;
    }

    #endregion

    #region CD刷新

    public void StopCD()
    {
        if (CdMask != null)
        {
            CdMask.CustomActive(false);
        }
    }

    void CDUpdate(float fillAmount)
    {
        if (CdMask != null)
        {
            CdMask.CustomActive(true);
            //Debug.LogFormat("FM={0}", fillAmount);
            CdMask.fillAmount = 1.0f - fillAmount;
            if (null != itemCfg)
                CdTime.text = ((1.0f - fillAmount) * itemCfg.itemcd * 0.001f).ToString("F1");
        }
    }

    #endregion

    #region 格子拖拽

    //static long curDragUid = 0;

    void DragStart(GameObject _go)
    {
        //Debug.Log($"拖拽开始    ");
        //startPos = obj.transform.parent.parent.parent.parent.transform.localPosition;
        //EventData data = CSEventObjectManager.Instance.SetValue(type, startPos);
        //EventHandler.SendEvent(CEvent.BagItemsStartDrag, data);
        //CSEventObjectManager.Instance.Recycle(data);
    }

    void Drag(GameObject _go, Vector2 vec)
    {
        if (vector == null)
        {
            vector = Vector2.zero;
        }

        vector = vector + vec;
    }

    void DragEnd(GameObject _go)
    {
        //Debug.Log($"拖拽结束      {vector}    {startPos}");
        //EventData data = CSEventObjectManager.Instance.SetValue(vector, startPos);
        ItemBaseDragPara para = new ItemBaseDragPara();
        para.mtype = type;
        para.mvector = vector;
        para.mstartPos = startPos;
        EventHandler.SendEvent(CEvent.BagItemsDrag, para);
        //CSEventObjectManager.Instance.Recycle(data);
        vector = Vector2.zero;
    }

    #endregion
}

public struct ItemBaseDragPara
{
    public PropItemType mtype;
    public Vector2 mvector;
    public Vector3 mstartPos;
}

