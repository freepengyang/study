using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TABLE;
using UnityEngine;

public partial class UISeekTreasureExchangePanel : UIBasePanel
{
    enum TypeTab
    {
        Weapon = 1, //武器
        Clothes, //衣服
        Jewelry, //首饰
        Other, //其他
    }

    private TypeTab surTypeTab = TypeTab.Weapon;
    private Dictionary<int, List<TABLE.POINTS>> pointDic;

    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.SeekTreasureHistory, SeekTreasureHistory);
        mClientEvent.Reg((uint) CEvent.ItemListChange, SeekTreasureHistory);
        mClientEvent.Reg((uint) CEvent.MoneyChange, SeekTreasureHistory);
        UIEventListener.Get(mtab_weapon.gameObject, TypeTab.Weapon).onClick = OnClickTab;
        UIEventListener.Get(mtab_clothes.gameObject, TypeTab.Clothes).onClick = OnClickTab;
        UIEventListener.Get(mtab_jewelry.gameObject, TypeTab.Jewelry).onClick = OnClickTab;
        UIEventListener.Get(mtab_other.gameObject, TypeTab.Other).onClick = OnClickTab;
    }


    void SeekTreasureHistory(uint id, object data)
    {
        RefreshAllData(surTypeTab);
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mtreasure_line, "treasure_line");
        Net.ReqPointInfoMessage();
    }

    void RefreshAllData(TypeTab type)
    {
        SetIntegralInfo();
        SetGridExchangeItem(type);
        SetRecordInfo();
    }

    /// <summary>
    /// 设置积分信息
    /// </summary>
    void SetIntegralInfo()
    {
        mlb_integral.text = CSItemCountManager.Instance.GetItemCount((int)MoneyType.xunbaojifen).ToString();
        pointDic = CSSeekTreasureInfo.Instance.GetDicFilterByType();
    }

    /// <summary>
    /// 点击分页签
    /// </summary>
    /// <param name="go"></param>
    void OnClickTab(GameObject go)
    {
        if (go == null) return;
        TypeTab type = (TypeTab) UIEventListener.Get(go).parameter;
        surTypeTab = type;
        SetGridExchangeItem(type);
    }

    
    UISeekTreasureExchangeItemBinderData mData = new UISeekTreasureExchangeItemBinderData();
    /// <summary>
    /// 设置兑换列表
    /// </summary>
    /// <param name="type"></param>
    void SetGridExchangeItem(TypeTab type)
    {
        List<TABLE.POINTS> pointList;
        if (pointDic == null) return;
        if (pointDic.TryGetValue((int) type, out pointList))
        {
            mgrid_exchange.MaxCount = pointList.Count;
            GameObject go;
            for (int i = 0; i < mgrid_exchange.MaxCount; i++)
            {
                go = mgrid_exchange.controlList[i];
                var eventHandle = UIEventListener.Get(go);
                UISeekTreasureExchangeItemBinder Binder;
                if (eventHandle.parameter == null)
                {
                    Binder = new UISeekTreasureExchangeItemBinder();
                    Binder.Setup(eventHandle);
                }
                else
                {
                    Binder = eventHandle.parameter as UISeekTreasureExchangeItemBinder;
                }
                
                if (ItemTableManager.Instance.TryGetValue((int) pointList[i].itemId, out mData.itemCfg))
                {
                    //兑换消耗道具和积分（道具可能为无）
                    TABLE.ITEM item;
                    if (ItemTableManager.Instance.TryGetValue((int) pointList[i].preItem, out item))
                    {
                        if ((CSBagInfo.Instance.GetAllItemCount(item.id) > 0 || CSBagInfo.Instance.IsEquip(item.id))
                            && CSItemCountManager.Instance.GetItemCount((int)MoneyType.xunbaojifen) >= pointList[i].points)
                        {
                            mData.isEnough = true;
                            go.transform.SetAsFirstSibling();
                        }
                        else
                        {
                            mData.isEnough = false;
                        }

                        mData.cost = CSString.Format(1062, item.name, pointList[i].points);
                    }
                    else
                    {
                        if (CSItemCountManager.Instance.GetItemCount((int)MoneyType.xunbaojifen) >= pointList[i].points)
                        {
                            mData.isEnough = true;
                            go.transform.SetAsFirstSibling();
                        }
                        else
                        {
                            mData.isEnough = false;
                        }

                        mData.cost = CSString.Format(1063, pointList[i].points);
                    }
                    
                    mData.pointsId = pointList[i].id;
                    mData.action = OnExchangeClick;
                    mData.count = pointList[i].itemNum;
                    Binder.Bind(mData);
                }
            }

            mScrollView_exchange.ResetPosition();
        }
    }

    void OnExchangeClick(int pointsId)
    {
        if (pointsId > 0)
        {
            Net.ReqExchangePointMessage(pointsId);
        }
    }

    /// <summary>
    /// 设置兑换记录信息
    /// </summary>
    void SetRecordInfo()
    {
        List<string> integralHistory = CSSeekTreasureInfo.Instance.IntegralHistory;
        //添加数据
        MatchCollection matchs;
        string pattern = "\\[item:(\\d+)\\]";
        mgrid_record.MaxCount = integralHistory.Count;
        UILabel lb_content;
        for (int i = 0; i < mgrid_record.MaxCount; i++)
        {
            lb_content = mgrid_record.controlList[i].GetComponent<UILabel>();

            matchs = Regex.Matches(integralHistory[i], pattern);
            int itemid = 0;
            TABLE.ITEM item = null;
            if (matchs.Count > 0)
            {
                int star = 6;
                int lenght = matchs[0].Value.Length - 7;
                itemid = int.Parse(matchs[0].Value.Substring(star, lenght));

                if (ItemTableManager.Instance.TryGetValue(itemid, out item))
                {
                    CSStringBuilder.Clear();
                    lb_content.text = CSStringBuilder.Append(UtilityColor.GetColorString(ColorType.MainText),
                        integralHistory[i].Replace(matchs[0].Value, item.name)).ToString();
                }
            }

            UIEventListener.Get(lb_content.gameObject, item).onClick = OnClickLookItemInfo;
        }

        //UI表现
        CSGame.Sington.StartCoroutine(SetScrollValue(1));
    }

    IEnumerator SetScrollValue(float value)
    {
        yield return null;
        mRecordView.ResetPosition();
        mRecordView.verticalScrollBar.value = mRecordView.GetComponent<UIPanel>().height >=
                                              mgrid_record.CellHeight * mgrid_record.controlList.Count
            ? 0
            : value;
    }

    private void OnClickLookItemInfo(GameObject go)
    {
        TABLE.ITEM item = go.GetComponent<UIEventListener>().parameter as TABLE.ITEM;
        if (item == null) return;
        CSSeekTreasureInfo.Instance.ShowItemTip(item.id);
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtreasure_line);
        mgrid_exchange.UnBind<UISeekTreasureExchangeItemBinder>();
        base.OnDestroy();
    }
}