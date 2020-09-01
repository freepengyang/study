using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Unity.Collections;
using UnityEngine;

public partial class UISeekTreasureWarehousePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private const int MaxCount = 45;
    private int mCurIndex = 1;
    private const int maxPage = 12;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.SeekTreasureStorehouse, SeekTreasureStorehouse);
        mClientEvent.Reg((uint) CEvent.SeekTreasureItemChanged, SeekTreasureStorehouse);
        mbtn_useExp.onClick = OnClickUseExp;
        mbtn_recEquip.onClick = OnClickRecEquip;
        mbtn_get.onClick = OnClickGet;
        mScrollView.onDragFinished = OnDragEnd;
        mDragScrollView.onDrag = OnDragItem;
    }

    void SeekTreasureStorehouse(uint id, object data)
    {
        RefreshAllData();
    }

    public override void Show()
    {
        base.Show();
        Net.ReqTreasureStorehouseMessage();
        mbtn_recEquip.gameObject.SetActive(CSMainPlayerInfo.Instance.VipLevel >=
                                           int.Parse(SundryTableManager.Instance.GetSundryEffect(582)));
    }

    void RefreshAllData()
    {
        // SetBagGridIndex();
        RefreshStoreHouse();
    }

    private Dictionary<UIItemBase, int> dicItemBaseIndex;
    List<UIItemBase> listItemBases = new List<UIItemBase>();

    private void RefreshStoreHouse()
    {
        if (mCurIndex < 1 || mCurIndex > maxPage) return;
        SetBagGridIndex();
        mgrid_item.MaxCount = MaxCount;
        int count = (mCurIndex - 1) * MaxCount;
        GameObject gp;
        // GameObject sp_icon;
        // GameObject bg_quality;
        if(dicItemBaseIndex == null)
            dicItemBaseIndex = new Dictionary<UIItemBase, int>();
        dicItemBaseIndex.Clear();
        for (int i = 0; i < mgrid_item.MaxCount; i++)
        {
            gp = mgrid_item.controlList[i];
            // sp_icon = gp.transform.Find("sp_itemicon").gameObject;
            // bg_quality = gp.transform.Find("sp_quality").gameObject;

            if (listItemBases.Count <= i)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size68));

            UIItemBase itemBase = listItemBases[i];
            // sp_icon = itemBase.obj.transform.Find("sp_itemicon").gameObject;
            // bg_quality = itemBase.obj.transform.Find("sp_quality").gameObject;

            if ((i + count) < CSSeekTreasureInfo.Instance.WarehouseBagItemInfos.Count &&
                CSSeekTreasureInfo.Instance.WarehouseBagItemInfos[i + count].count > 0)
            {
                TABLE.ITEM cfg;
                if (ItemTableManager.Instance.TryGetValue(
                    CSSeekTreasureInfo.Instance.WarehouseBagItemInfos[i + count].configId, out cfg))
                {
                    itemBase.SetMaskJudgeState(true);
                    itemBase.Refresh(cfg);
                    dicItemBaseIndex.Add(itemBase,
                        CSSeekTreasureInfo.Instance.WarehouseBagItemInfos[i + count].bagIndex);
                    itemBase.ShowOrHideIconAndQuality(true);
                    itemBase.DoubleClick = OnDoubleClickItem;
                }
                UIEventListener.Get(itemBase.obj).onDrag = OnDragItem;
                UIEventListener.Get(itemBase.obj).ClickIntervalTime = 0f;
            }
            else
            {
                // sp_icon.SetActive(false);
                // bg_quality.SetActive(false);
                // itemBase.ShowOrHideIconAndQuality(false);
                itemBase.UnInit();
                UIEventListener.Get(itemBase.obj).parameter = null;
                UIEventListener.Get(itemBase.obj).onDoubleClick = null;
                UIEventListener.Get(itemBase.obj).onClick = null;
                UIEventListener.Get(itemBase.obj).onDrag = OnDragItem;
                itemBase.DoubleClick = null;
            }
        }
    }

    private void SetBagGridIndex()
    {
        mgrid_page.MaxCount = maxPage;
        GameObject sp_icon;
        for (int i = 0; i < maxPage; i++)
        {
            sp_icon = mgrid_page.controlList[i].transform.GetChild(0).gameObject;
            sp_icon.SetActive(i == mCurIndex - 1);
        }
    }

    void OnDoubleClickItem(UIItemBase itemBase)
    {
        if (itemBase == null) return;
        if (dicItemBaseIndex != null)
        {
            if (dicItemBaseIndex.ContainsKey(itemBase))
            {
                int bagIndex = dicItemBaseIndex[itemBase];
                Net.ReqGetItemMessage(bagIndex);
            }
        }
    }

    float temp = 0;

    private void OnDragItem(GameObject go, Vector2 delat)
    {
        temp += delat.x;
    }

    private void OnDragEnd()
    {
        if (temp > 100)
        {
            OnLeftClick();
        }
        else if (temp < -200)
        {
            OnRightClick();
        }

        temp = 0;
        mScrollView.ScrollImmidate(0);
        RefreshStoreHouse();
    }

    private void OnLeftClick(GameObject go = null)
    {
        if (mCurIndex == 1) return;
        mCurIndex--;
    }

    private void OnRightClick(GameObject go = null)
    {
        if (mCurIndex == maxPage) return;
        mCurIndex++;
    }

    /// <summary>
    /// 地元丹使用
    /// </summary>
    /// <param name="go"></param>
    void OnClickUseExp(GameObject go)
    {
        if (go == null) return;
        RepeatedField<int> woLongTans = CSSeekTreasureInfo.Instance.GetWoLongTans();
        if (woLongTans.Count == 0)
        {
            UtilityTips.ShowTips(CSString.Format(1073), 1.5f, ColorType.Red);
        }
        else
        {
            Net.ReqUseTreasureExpMessage(woLongTans);
        }
    }

    /// <summary>
    /// 地元装备回收
    /// </summary>
    /// <param name="go"></param>
    void OnClickRecEquip(GameObject go)
    {
        if (go == null) return;
        RepeatedField<int> woLongEquips = CSSeekTreasureInfo.Instance.GetRecyclableWoLongItems();
        if (woLongEquips.Count>0)
            Net.ReqHuntCallBackMessage(woLongEquips);
        else
            UtilityTips.ShowRedTips(735);
    }

    /// <summary>
    /// 一键提取
    /// </summary>
    /// <param name="go"></param>
    void OnClickGet(GameObject go)
    {
        if (go == null) return;
        Net.ReqGetWholeItemMessage();
    }

    protected override void OnDestroy()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
        base.OnDestroy();
    }
}