using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gem;
using bag;
using TABLE;

public partial class UIGemPanel : UIBasePanel
{
    List<GemData> listGemData; //客户端宝石数据
    Vector3 oldPosition; //选中位置
    int curSubType; //当前选中SubType
    Map<int, PosGemSuit> suitList; //达成套装等级
    List<Map<int, int>> oldidsList;
    private Map<int, int> tempgemids;
    private int oldSubType = 0;
    private int curKillMaxNum;
    int freetimes;
    List<BagItemInfo> listBagItemInfo;
    private Dictionary<int,UIGemPos> uigemposDic;
    private WaitForSeconds mWaitMisson = new WaitForSeconds(0.02f);
    
    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.CSUnlockGemPositionMessage, ReFreshPanel);
        mClientEvent.AddEvent(CEvent.CSGemRefresh, ReFreshPanel);
        mClientEvent.AddEvent(CEvent.GemSuitInfoChange, ChangeGemSuit);
        mClientEvent.AddEvent(CEvent.GemBossKillCount, ReFreshPanel);
        mClientEvent.AddEvent(CEvent.GemTabRedChange,RefreshTabRed);
        UIEventListener.Get(mbtn_help).onClick = OnHelpClick;
        UIEventListener.Get(mbtn_quickbuy).onClick = OnQuickBuyClick;
        UIEventListener.Get(mbtn_suit).onClick = OnsuitClick;
        UIEventListener.Get(mbtn_wear).onClick = OnwearClick;
        
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg.gameObject, "gem_bg");
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_idleEffect, "effect_gem_idle_add");
        mobj_idleEffect.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        listGemData = CSGemInfo.Instance.GetgemDatas();
        if (listGemData.Count > 0)
        {
            curSubType = listGemData[0].subType;
        }

        suitList = CSGemInfo.Instance.suitList;
        
        //重设选中框的值
        if (mgird_GemPart != null && mgird_GemPart.controlList.Count>0)
        {
            mtrans_choose.transform.localPosition = mgird_GemPart.controlList[0].transform.localPosition;    
        }
        
        RefreshUI();
        RefreshTabRed();
    }
    
    /// <summary>
    /// 刷新红点
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    private void RefreshTabRed(uint uievtid = 0, object data = null)
    {
        var dic = CSGemInfo.Instance.GemTabRedDic;
        if (dic.Count <= 0)
            return;
        for (int i = 1; i <= mgird_GemPart.MaxCount; i++)
        {
            Transform redpoint = UtilityObj.Get<Transform>(mgird_GemPart.controlList[i - 1].transform, "redpoint");

            if (dic.ContainsKey(i))
            {
                bool isRed = (dic[i] & 3) > 0;
                redpoint.gameObject.SetActive(isRed);
            }
        }
        if (dic.ContainsKey(curSubType))
            mobj_wearRed.SetActive((dic[curSubType] & 1) == 1);
        
    }

    private void RefreshBossKill(uint uievtid = 0, object data = null)
    {
        
    }


    // private void ReGemFreshPanel(uint uiEvtID, object data)
    // {
    //     var gemInfo = (GemInfo) data;
    //     CSGemInfo.Instance.SetMaxBagGem(gemInfo);
    //     listGemData = CSGemInfo.Instance.GetgemDatas();
    //     RefreshUI();
    // }

    /// <summary>
    /// 刷新整个面板
    /// </summary>
    /// <param name="uiEvtID"></param>
    /// <param name="data"></param>
    private void ReFreshPanel(uint uiEvtID, object data)
    {
        listGemData = CSGemInfo.Instance.GetgemDatas();
        RefreshUI();    
    }

    void RefreshUI()
    {
        mgird_GemPart.MaxCount = listGemData.Count;

        mgird_GemPart.Bind<GemData, UIGemItem>(listGemData, mPoolHandleManager);

        for (int i = 0; i < mgird_GemPart.MaxCount; i++)
        {
            UIEventListener.Get(mgird_GemPart.controlList[i], listGemData[i]).onClick = ReFreshRightUI;
        }

        ReFreshRightUI();

        //显示总属性值

        var attrItems = CSGemInfo.Instance.GetallAttr();
        //mPropGrid.repositionNow = true;


        mobj_Attr.SetActive(attrItems != null);
        mobj_NoAttr.SetActive(attrItems == null);
        if (attrItems != null)
        {
            for (int i = 0; i < attrItems.Count; i++)
            {
                CSStringBuilder.Clear();
                mPropGrid.GetChild(i).GetComponent<UILabel>().text
                    = CSStringBuilder.Append(attrItems[i].Key.BBCode(ColorType.SecondaryText)
                        , CSString.Format(999), attrItems[i].Value.BBCode(ColorType.MainText)).ToString();
            }
        }
    }

    void ReFreshRightUI(GameObject go = null)
    {
        //获取到当前列表的数据，如果parameter 不为空，则使用parameter 数据，如果为空，则使用当前数据
        if (go != null)
        {
            curSubType = ((GemData) UIEventListener.Get(go).parameter).subType;
            oldPosition = mtrans_choose.transform.localPosition;
            mtrans_choose.transform.localPosition = go.transform.localPosition;
        }

        if (suitList.Count>0)
        {
            //全身套装文字显示显示
            GEMSUIT nextsuiData = CSGemInfo.Instance.GetNextSuit(suitList[0].configId, suitList[0].pos);
            if (nextsuiData == null)
                FNDebug.LogError("nextsuiData is null");
            int lv = GemSuitTableManager.Instance.GetGemSuitGamLevel(nextsuiData.id);
            int nextNum = CSGemInfo.Instance.GetGemNum(nextsuiData.gamPosition, lv);
        
            string numstr = CSString.Format(1042, nextNum, nextsuiData.gamNum);
            string outnumstr = nextNum >= nextsuiData.gamNum
                ? numstr.BBCode(ColorType.Green)
                : numstr.BBCode(ColorType.Red);
            mlb_totalsuit.text = $"{nextsuiData.name.BBCode(ColorType.Yellow)}{outnumstr}";
        }
        
        //刷新右侧界面,如果点击的为未解锁 则返回上一个点击
        GemData data = listGemData[curSubType - 1];
        if (data == null)
            return;
        if (data.subType < data.unlockingPosition || data.unlockingPosition == -1)
        {
            int id = suitList[curSubType].configId;
            GEMSUIT gemSuit;
            if (GemSuitTableManager.Instance.TryGetValue(id, out gemSuit))
            {
                //string pic = id != 0 ? gemSuit.pic: CSGemInfo.Instance.GetGemSuitLow(data.subType).pic;
            }
            else
            {
                gemSuit = CSGemInfo.Instance.GetGemSuitLow(data.subType);
            }

            mspr_Icon.spriteName = gemSuit.pic;
            mlb_name.text = gemSuit.name;

            UIEventListener.Get(mspr_Icon.gameObject, data).onClick = ShowPartStarSuit;
            ShowIconColor();
            for (int i = 0; i < mLingshiList.transform.childCount; i++)
            {
                UIGemPos uiGemPos;
                Transform stoneSlot = mLingshiList.transform.GetChild(i);
                if (uigemposDic == null)
                {
                    uigemposDic = mPoolHandleManager.GetSystemClass<Dictionary<int,UIGemPos>>();
                    uigemposDic.Clear();
                }
                
                if (uigemposDic.ContainsKey(i))
                {
                    uiGemPos = uigemposDic[i];
                }
                else
                {
                    uiGemPos = mPoolHandleManager.GetSystemClass<UIGemPos>();
                    
                    uiGemPos.Init(mLingshiList.transform.GetChild(i));
                }
                int position = int.Parse(stoneSlot.gameObject.name);
                bool isInlay = CSGemInfo.Instance.judgeBagGemByBody(data.subType, position);
                bool islevelUp = CSGemInfo.Instance.IsGemLevelUp(data.subType, position);
                uiGemPos.inlayArrow.SetActive(isInlay||islevelUp);
                uiGemPos.lb_hint.gameObject.SetActive(islevelUp);
                uiGemPos.btn_jian.SetActive(false);
                uiGemPos.btn_inlay.SetActive(false);
                uiGemPos.quality.gameObject.SetActive(false);
                uiGemPos.addeffect.gameObject.SetActive(false);
                GemInfo thisGem = data.GemInfos[position];
                if (oldidsList != null)
                {
                    int oldId = oldidsList[curSubType - 1][position];
                    int curId = data.GemInfos[position].gemId;
                    if (oldId != curId && curId != 0 && oldSubType == curSubType)
                    {
                        CSEffectPlayMgr.Instance.ShowUIEffect(uiGemPos.obj_inlayeffect.gameObject, 
                            "effect_gem_inlay_add", 10, false);
                    }
                }

                if (thisGem.gemId == 0)
                {
                    uiGemPos.icon.gameObject.SetActive(false);
                    uiGemPos.btn_inlay.gameObject.SetActive(true);
                    //CSEffectPlayMgr.Instance.ShowUIEffect(quality, "publicup");
                    //添加箭头特效
                    if (uiGemPos.addeffect.atlas == null)
                    {
                        CSEffectPlayMgr.Instance.ShowUIEffect(uiGemPos.addeffect.gameObject, 17086);
                    }

                    uiGemPos.addeffect.gameObject.SetActive(CSBagInfo.Instance.GetGemInfoByPos(position).Count > 0);

                    UIEventListener.Get(uiGemPos.icon.gameObject).onClick = null;
                    UIEventListener.Get(uiGemPos.btn_inlay).onClick = OnInlayClick;
                    UIEventListener.Get(uiGemPos.btn_jian).onClick = null;
                }
                else
                {
                    uiGemPos.btn_jian.SetActive(true);

                    ITEM item;
                    if (ItemTableManager.Instance.TryGetValue(thisGem.gemId, out item))
                    {
                        uiGemPos.icon.spriteName = item.icon;
                        uiGemPos.quality.gameObject.SetActive(true);
                        int qualityValue = item.quality;
                        uiGemPos.quality.spriteName = ItemTableManager.Instance.GetItemQualityBG(qualityValue);
                        uiGemPos.icon.gameObject.SetActive(true);
                        if (isInlay)
                            UIEventListener.Get(uiGemPos.icon.gameObject, thisGem).onClick = OnInlayClick;
                        else
                            UIEventListener.Get(uiGemPos.icon.gameObject, thisGem).onClick = OnStoneOnEquipClick;
                        UIEventListener.Get(uiGemPos.btn_inlay).onClick = null;
                        UIEventListener.Get(uiGemPos.btn_jian).onClick = OnRemoveClick;
                    }
                }
            }

            //CurgemData = data;
            curSubType = data.subType;
            //mobj_wearRed.SetActive(CSGemInfo.Instance.judgeBagGemByBody(curSubType));
            var dic = CSGemInfo.Instance.GemTabRedDic;
            if (dic.ContainsKey(data.subType))
                mobj_wearRed.SetActive((dic[data.subType] & 1)== 1);
            
        }
        else if (data.subType == data.unlockingPosition)
        {
            mtrans_choose.transform.localPosition = oldPosition;
            curKillMaxNum = data.gemslot.num;
            //Debug.Log(data.subType);
            //弹框解锁
            object[] para = new object[] {data.gemslot.costnum};
            UtilityTips.ShowPromptWordTips(54, () => { mtrans_choose.transform.localPosition = oldPosition; }, () =>
            {
                if (((int) MoneyType.yuanbao).GetItemCount() < data.gemslot.costnum)
                {
                    Utility.ShowGetWay((int) MoneyType.yuanbao);
                    return;
                }

                Net.CSUnlockGemPositionMessage(); //解锁槽位
            }, () => { mtrans_choose.transform.localPosition = oldPosition; }, para);
        }
        else
        {
            UtilityTips.ShowRedTips(ClientTipsTableManager.Instance.GetClientTipsContext(943));
            mtrans_choose.transform.localPosition = oldPosition;
        }

        
        // 保存上次 的数据 ,用于镶嵌特效显示
        if (oldidsList == null)
        {
            oldidsList = mPoolHandleManager.GetSystemClass<List<Map<int, int>>>();
            oldidsList.Clear();
            for (int i = 0; i < listGemData.Count; i++)
            {
                var geminfos = listGemData[i].GemInfos;
                Map<int, int> gemids = mPoolHandleManager.GetSystemClass<Map<int, int>>();
                gemids.Clear();
                for (geminfos.Begin(); geminfos.Next();)
                {
                    gemids.Add(geminfos.Value.pos, geminfos.Value.gemId);
                }

                oldidsList.Add(gemids);
                
                mPoolHandleManager.Recycle(gemids);
            }
        }
        else
        {
            if (oldidsList.Count > curSubType - 1)
            {
                var geminfos = data.GemInfos;
                if (tempgemids == null)
                {
                    tempgemids = mPoolHandleManager.GetSystemClass<Map<int, int>>();
                    tempgemids.Clear();
                }
                else
                {
                    tempgemids.Clear();
                }

                for (geminfos.Begin(); geminfos.Next();)
                {
                    tempgemids.Add(geminfos.Value.pos, geminfos.Value.gemId);
                }
                oldidsList[curSubType - 1] = tempgemids;
            }
        }
        //保存当前点击
        oldSubType = curSubType;
    }

    //宝石套装效果变化回调
    private void ChangeGemSuit(uint uiEvtID, object data)
    {
        //Debug.Log("ChangeGemSuit");
        suitList = CSGemInfo.Instance.suitList;
        ShowIconColor();
    }
    
    
    /// <summary>
    /// 显示宝石部件套装特效
    /// </summary>
    private void ShowIconColor()
    {
        bool isActiveEffect = suitList[curSubType].configId != 0;
        mspr_Icon.color = isActiveEffect ? Color.white : Color.black;
        mobj_idleEffect.SetActive(isActiveEffect);
    }

    /// <summary>
    /// 与策划确认过,宝石的二级子页签是宝石升级
    /// </summary>
    /// <param name="type"></param>
    /// <param name="subType"></param>
    public override void SelectChildPanel(int type, int subType)
    {
        
    }
    
    
    #region 右侧按钮点击事件

    //一键穿戴
    private void OnwearClick(GameObject go)
    {
        if (go == null) return;
        listBagItemInfo = CSGemInfo.Instance.GetGeminlayList(curSubType);
        //获取当前部件的空位 
        // 获取背包里空位的宝石
        if (listBagItemInfo.Count > 0)
        {
            freetimes = 0;
            BindCoroutine(1,WearAllStone());
            //CSGame.Sington.StartCoroutine(WearAllStone());
        }
        else
        {
            bool isAllMax = true;
            //判断是否当前所有都是最大值 
            Map<int, GemInfo> GemInfos = null;
               
            for (int i = 0; i < listGemData.Count; i++)
            {
                if (listGemData[i].subType == curSubType)
                {
                    GemInfos = listGemData[i].GemInfos;
                    break;
                }
            }
                
            if (GemInfos != null)
            {
                    
                for (var it = GemInfos.GetEnumerator(); it.MoveNext();)
                {
                    if (!CSGemInfo.Instance.IsMaxLV(it.Current.Value.gemId))
                    {
                        isAllMax = false;
                        break;
                    }
                }
                    
            } 

            if (isAllMax)
            {
                UtilityTips.ShowTips(2006);
                
            }
            else
            {
                string str = SundryTableManager.Instance.GetSundryEffect(1020);
                Utility.ShowGetWay(str);
            }

            
        }
    }

    IEnumerator WearAllStone()
    {
        while (freetimes < listBagItemInfo.Count)
        {
            Net.CSEquipGemMessage(curSubType,
                GemTableManager.Instance.GetGemPosition(listBagItemInfo[freetimes].configId),
                listBagItemInfo[freetimes].bagIndex);

            freetimes++;
            yield return mWaitMisson;
        }

        //CSGame.Sington.StopCoroutine(WearAllStone());
    }

    //点击打开套装效果
    private void ShowPartStarSuit(GameObject obj)
    {
        GemData para = (GemData) UIEventListener.Get(obj).parameter;
        UIManager.Instance.CreatePanel<UIGemStarSuitTipPanel>((f) =>
        {
            (f as UIGemStarSuitTipPanel).OpenPanel(suitList[para.subType]);
        });
    }
    
    /// <summary>
    /// 点击打开宝石合成界面
    /// </summary>
    /// <param name="obj"></param>
    private void OnStoneOnEquipClick(GameObject obj)
    {
        GemInfo thisGem = (GemInfo) UIEventListener.Get(obj).parameter;

        if (CSGemInfo.Instance.IsMaxLV(thisGem.gemId))
        {
            UtilityTips.ShowRedTips(CSString.Format(1168));
            return;
        }

        UIManager.Instance.CreatePanel<UIGemLevelUpPanel>((f) => { (f as UIGemLevelUpPanel).OpenPanel(thisGem); });
    }

    //点击脱下宝石
    private void OnRemoveClick(GameObject obj)
    {
        int pos = int.Parse(obj.transform.parent.name);
        Net.CSEquipGemMessage(curSubType, pos, 0);
    }
    
    /// <summary>
    /// 点击显示添加宝石面板
    /// </summary>
    /// <param name="obj"></param>
    private void OnInlayClick(GameObject obj)
    {
        if (obj == null) return;
        int pos = int.Parse(obj.transform.parent.name);
        //Debug.Log("pos" + pos);
        var itemInfos = CSBagInfo.Instance.GetGemInfoByPos(pos);

        itemInfos.Sort((x, y) =>
        {
            TABLE.GEM gem1 = null;
            GemTableManager.Instance.TryGetValue(x.configId, out gem1);
            TABLE.GEM gem2 = null;
            GemTableManager.Instance.TryGetValue(y.configId, out gem2);
            if (gem1 != null && gem2 != null)
            {
                return (int) (gem2.lv - gem1.lv);
            }

            return 0;
        });

        //Debug.Log("itemInfos.Count" + itemInfos.Count);

        if (itemInfos != null && itemInfos.Count > 0)
        {
            UIManager.Instance.CreatePanel<UIGemItemListPanel>((f) =>
            {
                (f as UIGemItemListPanel).OpenPanel(itemInfos, curSubType, pos);
            });
        }
        else
        {
            int id = CSGemInfo.Instance.GetGemTableLow(pos);
            Utility.ShowGetWay(id);
        }
    }

    //显示装备整体套装效果
    private void OnsuitClick(GameObject obj)
    {
        UIManager.Instance.CreatePanel<UIGemStarSuitTipPanel>((f) =>
        {
            (f as UIGemStarSuitTipPanel).OpenPanel(suitList[0]);
        });
    }

    //显示快捷购买
    private void OnQuickBuyClick(GameObject obj)
    {
        UtilityPanel.JumpToPanel(12301);
    }

    //显示帮助按钮
    private void OnHelpClick(GameObject obj)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GemHelp);
    }
    
    #endregion


    protected override void OnDestroy()
    {
        mgird_GemPart.UnBind<UIGemItem>();
        mgird_GemPart = null;
        for (int i = 0; i < 5; i++)
        {
            UISprite addeffect = UtilityObj.Get<UISprite>(mLingshiList.transform.GetChild(i), "addeffect");
            UISprite obj_inlayeffect = UtilityObj.Get<UISprite>(mLingshiList.transform.GetChild(i), "obj_inlayeffect");
            CSEffectPlayMgr.Instance.Recycle(addeffect.gameObject);
            CSEffectPlayMgr.Instance.Recycle(obj_inlayeffect.gameObject);
        }

        
        CSEffectPlayMgr.Instance.Recycle(mobj_idleEffect);
        listGemData = null;
        suitList = null;
        if (tempgemids != null)
        {
            mPoolHandleManager.Recycle(tempgemids);
        }
       
        mPoolHandleManager.Recycle(oldidsList);
        for (var it = uigemposDic.GetEnumerator();it.MoveNext();)
        {
            mPoolHandleManager.Recycle(it.Current.Value);
        }
        
        mPoolHandleManager.Recycle(uigemposDic);
        // gemids = null;
        oldidsList = null;
        listBagItemInfo = null;
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
}


public class UIGemItem : UIBinder
{
    GameObject bg;
    UISprite frame;
    UILabel name;
    //GameObject choose;
    GameObject redpoint;
    UIGridContainer itemList;
    UILabel itemStr;
    GemData _data;

    public override void Init(UIEventListener handle)
    {
        frame = Get<UISprite>("frame");
        name = Get<UILabel>("name");
        //choose = Get<GameObject>("choose");
        redpoint = Get<GameObject>("redpoint");
        itemList = Get<UIGridContainer>("itemlist");
        itemStr = Get<UILabel>("itemstr");
        //bg = Get<GameObject>("bg");//这个boxCollider没加
    }

    public override void Bind(object data)
    {
        _data = data as GemData;
        //Debug.Log(_data.unlockingPosition);
        frame.spriteName = _data.gemslot.pic.ToString();
        
        // 根据解锁槽位 判断左侧tab 
        if (_data.subType < _data.unlockingPosition || _data.unlockingPosition == -1)
        {
            name.gameObject.SetActive(true);
            itemList.gameObject.SetActive(true);
            itemStr.gameObject.SetActive(false);
            name.text = _data.gemslot.name;
            itemList.MaxCount = _data.GemInfos.Count; //这个需要

            for (int i = 0; i < _data.GemInfos.Count; i++)
            {
                int pos = int.Parse(itemList.controlList[i].name);
                UISprite icon = UtilityObj.Get<UISprite>(itemList.controlList[i].transform, "icon");
                if (_data.GemInfos[pos].gemId == 0)
                {
                    icon.spriteName = null;
                }
                else
                {
                    //UISprite icon = UtilityObj.Get<UISprite>(itemList.controlList[i].transform, "icon");
                    icon.spriteName = ItemTableManager.Instance.GetItemIcon(_data.GemInfos[pos].gemId);
                }
            }

            //redpoint.SetActive(CSGemInfo.Instance.judgeBagGemByBody(_data.subType));
        }
        else if (_data.subType == _data.unlockingPosition)
        {
            name.gameObject.SetActive(false);
            itemList.gameObject.SetActive(false);
            itemStr.gameObject.SetActive(true);
            CSStringBuilder.Clear();
            string tempkillNum = CSStringBuilder.Append(_data.bossCounter, "/", _data.gemslot.num).ToString();
            //string tempkillNum = CSString.Format(1024)
            string killNum = _data.bossCounter >= _data.gemslot.num
                ? tempkillNum.BBCode(ColorType.Green)
                : tempkillNum.BBCode(ColorType.Red);
            int tempCost = _data.gemslot.costnum;
            string cost = ((int) MoneyType.yuanbao).GetItemCount() > tempCost
                ? tempCost.ToString().BBCode(ColorType.Green)
                : tempCost.ToString().BBCode(ColorType.Red);

            itemStr.text = CSString.Format(942, killNum, _data.gemslot.killBoss, cost);
        }
        else
        {
            name.gameObject.SetActive(false);
            itemList.gameObject.SetActive(false);
            itemStr.gameObject.SetActive(true);
            itemStr.text = ClientTipsTableManager.Instance.GetClientTipsContext(943).BBCode(ColorType.WeakText);
        }
    }

    public override void OnDestroy()
    {
        frame = null;
        name = null;
        //choose = null;
        redpoint = null;
        itemList = null;
        _data = null;
    }
    
}

/// <summary>
/// 界面上放置宝石部位的信息
/// </summary>
public class UIGemPos
{
    public GameObject btn_inlay;
    public GameObject btn_jian;
    public GameObject inlayArrow;
    public UISprite icon;
    public UISprite quality;
    public UISprite addeffect;
    public UISprite obj_inlayeffect;
    public Transform lb_hint;
    
    public void Init(Transform stoneSlot)
    {
        //Transform stoneSlot = mLingshiList.transform.GetChild(i);
        int position = int.Parse(stoneSlot.gameObject.name);
        btn_inlay = stoneSlot.Find("btn_inlay").gameObject;
        btn_jian = stoneSlot.Find("btn_jian").gameObject;
        inlayArrow = stoneSlot.Find("arrow").gameObject;
        icon = UtilityObj.Get<UISprite>(stoneSlot, "icon");
        quality = UtilityObj.Get<UISprite>(stoneSlot, "quality");
        addeffect = UtilityObj.Get<UISprite>(stoneSlot, "addeffect");
        obj_inlayeffect = UtilityObj.Get<UISprite>(stoneSlot, "obj_inlayeffect");
        lb_hint = UtilityObj.Get<Transform>(stoneSlot,"lb_hint");
    }
}
