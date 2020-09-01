using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum ChatChannelType
{
    None,
    ChatPanel,
    Friend,
}

public enum LinkInsertMode
{
    LM_INSERT_BEGIN = 0,
    LM_INSERT_CURRENT = 1,
    LM_INSERT_END = 2,
}
public delegate void OnLinkSelected(string emtion, LinkInsertMode insertMode);

public class UIChatAddPanelData
{
    public ChatType channel;
    public ChatChannelType channelType;
    public OnLinkSelected onLinkSelected;
}

public partial class UIChatAddPanel : UIBasePanel
{
    private Vector3 ItemPanelClose = new Vector3(0, -468);
    private Vector3 ItemPanelOpen = new Vector3(0, -267);

    protected ChatType channel = ChatType.CT_PRIVATE;
    protected ChatChannelType channelType = ChatChannelType.None;
    protected OnLinkSelected onLinkSelected;
    protected const int BagMaxGridCount = 20;
    protected FastArrayElementFromPool<UIItemBase> mBagItemsExtendL;
    protected FastArrayElementFromPool<UIItemBase> mBagItems;
    protected FastArrayElementFromPool<UIItemBase> mBagItemsExtendR;
    protected FastArrayElementKeepHandle<bag.BagItemInfo> mBagDatasCommon = new FastArrayElementKeepHandle<bag.BagItemInfo>(20);
    protected FastArrayElementKeepHandle<bag.BagItemInfo> mBagDatas = new FastArrayElementKeepHandle<bag.BagItemInfo>(BagMaxGridCount);

    protected enum OpMode
    {
        OM_EMOTION = 0,
        OM_BAG = 1,
        OM_LOCATION = 2,
        OM_EQUIP = 3,
        OM_NEIGONG = 4,
    }
    protected OpMode mOpMode = OpMode.OM_EMOTION;
    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.CloseChatAddPanel, OnClosePanel);
        EventDelegate.Add(mEmotionToggle.onChange,()=> { OnEmotionClicked(mEmotionToggle.value); });
        EventDelegate.Add(mBagToggle.onChange, () => { OnBagClicked(mBagToggle.value); });
        EventDelegate.Add(mWearedNormalEquipToggle.onChange, () => { OnWearedEquipClicked(mWearedNormalEquipToggle.value); });
        EventDelegate.Add(mWearedNeigongEquipToggle.onChange, () => { OnWearedNeiGongEquipClicked(mWearedNeigongEquipToggle.value); });
        EventDelegate.Add(mLocationToggle.onChange, () => { OnLocationClicked(mLocationToggle.value); });
        EventDelegate.Add(mWindowPositoinTween.onFinished, ItemWindowTweenFinish);

        mBagItems = new FastArrayElementFromPool<UIItemBase>(BagMaxGridCount,
        ()=>
        {
            return UIItemManager.Instance.GetItem(PropItemType.Normal, mGridItems.transform);
        },
        (f)=>
        {
            UIItemManager.Instance.RecycleSingleItem(f);
        });
        mBagItemsExtendL = new FastArrayElementFromPool<UIItemBase>(BagMaxGridCount,
        () =>
        {
            return UIItemManager.Instance.GetItem(PropItemType.Normal, mGridItemsL.transform);
        },
        (f) =>
        {
            UIItemManager.Instance.RecycleSingleItem(f);
        });
        mBagItemsExtendR = new FastArrayElementFromPool<UIItemBase>(BagMaxGridCount,
        () =>
        {
            return UIItemManager.Instance.GetItem(PropItemType.Normal, mGridItemsR.transform);
        },
        (f) =>
        {
            UIItemManager.Instance.RecycleSingleItem(f);
        });
    }

    public void EnableDrag(bool enable)
    {
        if(enable)
        {
            UIEventListener.Get(mBagDragScrollView.gameObject).onDrag = OnDragItem;
            UIEventListener.Get(mBagDragScrollView.gameObject).onDragEnd = OnDragEnd;
        }
        else
        {
            UIEventListener.Get(mBagDragScrollView.gameObject).onDrag = null;
            UIEventListener.Get(mBagDragScrollView.gameObject).onDragEnd = null;
        }
        mBagScrollview.disableDragIfFits = !enable;
    }

    private float mAccuracy = 100f;
    private float temp_delate_x = 0;
    private void OnDragItem(GameObject go, Vector2 delta)
    {
        temp_delate_x = temp_delate_x + delta.x;
    }

    protected void SetDefaultToggle(int idx)
    {
        if (null != mGridPoints && idx >= 0 && idx < mGridPoints.controlList.Count)
            mGridPoints.controlList[idx].GetComponent<UIToggle>().value = true;
    }

    private int mCurPageIndex = 1;
    private void OnDragEnd(GameObject go)
    {
        if (temp_delate_x < -mAccuracy)
        {
            if (mCurPageIndex < mGridPoints.controlList.Count)
            {
                SetDefaultToggle(mCurPageIndex);
                mCurPageIndex++;
            }
            else
            {
                SetDefaultToggle(mCurPageIndex - 1);
                mCurPageIndex = Mathf.Max(1, mGridPoints.controlList.Count);
            }

            if (mOpMode == OpMode.OM_BAG)
            {
                OnBagPageNumChanged(mCurPageIndex - 1);
            }
            else if(mOpMode == OpMode.OM_EQUIP)
            {
                OnWearedEquipPageNumChanged(mCurPageIndex - 1);
            }
            else if (mOpMode == OpMode.OM_NEIGONG)
            {
                OnWearedNeiGongEquipPageNumChanged(mCurPageIndex - 1);
            }
        }
        else if (temp_delate_x > mAccuracy)
        {
            if (mCurPageIndex > 1)
            {
                mCurPageIndex--;
                SetDefaultToggle(mCurPageIndex - 1);
            }
            else
            {
                mCurPageIndex = 1;
                SetDefaultToggle(mCurPageIndex - 1);
            }

            if (mOpMode == OpMode.OM_BAG)
            {
                OnBagPageNumChanged(mCurPageIndex - 1);
            }
            else if (mOpMode == OpMode.OM_EQUIP)
            {
                OnWearedEquipPageNumChanged(mCurPageIndex - 1);
            }
            else if (mOpMode == OpMode.OM_NEIGONG)
            {
                OnWearedNeiGongEquipPageNumChanged(mCurPageIndex - 1);
            }
        }
        temp_delate_x = 0;
        mBagScrollview.ResetPosition();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        mBagItems.Clear();
        mBagItemsExtendL.Clear();
        mBagItemsExtendR.Clear();
        mBagDatas.Clear();
        mBagDatasCommon.Clear();
    }

    protected UIItemBase OnCreate()
    {
        return UIItemManager.Instance.GetItem(PropItemType.Normal, mBagScrollview.transform);
    }

    protected void OnRecycle(UIItemBase item)
    {
        if(null != item)
        {
            UIItemManager.Instance.RecycleSingleItem(item);
        }
    }

    protected void ResetTogglePoints(int n)
    {
        if (n <= BagMaxGridCount)
            n = 0;
        else
            n = (n - 1) / BagMaxGridCount + 1;

        mGridPoints.MaxCount = n;
    }

    void OnBagPageNumChanged(int page)
    {
        int pagIdx = page;
        int start = BagMaxGridCount * pagIdx;
        int lstart = Mathf.Max(0, start - BagMaxGridCount);
        int end = start + BagMaxGridCount;
        int rend = end + BagMaxGridCount;
        var bagItems = CSBagInfo.Instance.GetBagItemData();
        int idx = 0;
        mBagDatasCommon.Clear();
        var it = bagItems.GetEnumerator();
        int cnt = 0;
        while (it.MoveNext())
        {
            mBagDatasCommon.Append(it.Current.Value);
            ++idx;
            ++cnt;
        }

        //设置左边背包
        mBagDatas.Clear();
        for (int i = lstart; i < start && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendL, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsL.CustomActive(cnt > BagMaxGridCount);
        mGridItemsL.Reposition();
        mLPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;
        //设置中间段
        mBagDatas.Clear();
        for (int i = start; i < end && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItems, mBagDatas, cnt > BagMaxGridCount);
        mGridItems.Reposition();
        //设置尾端
        mBagDatas.Clear();
        for (int i = end; i < rend && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendR, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsR.CustomActive(cnt > BagMaxGridCount);
        mGridItemsR.Reposition();
        mRPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;

        EnableDrag(cnt > BagMaxGridCount);
        mBagScrollview.ResetPosition();
    }

    void OnBagClicked(bool value)
    {
        EnableBagItems(value);
        if(!value)
        {
            return;
        }
        mOpMode = OpMode.OM_BAG;

        //创建toggles
        int n = 0;
        var datas = CSBagInfo.Instance.GetBagItemData();
        if (null != datas)
        {
            n = datas.Count;
        }
        ResetTogglePoints(n);
        SetDefaultToggle(0);
        OnBagPageNumChanged(0);
    }

    void OnWearedEquipClicked(bool value)
    {
        EnableBagItems(value);
        if (!value)
        {
            return;
        }
        mOpMode = OpMode.OM_EQUIP;

        var datas = CSBagInfo.Instance.GetEquipItemData();
        mBagDatas.Clear();
        for (var it = datas.GetEnumerator();it.MoveNext();)
        {
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(it.Current.Value.configId, out item) || !CSBagInfo.Instance.IsNormalEquip(item))
            {
                continue;
            }

            mBagDatas.Append(it.Current.Value);
        }

        ResetTogglePoints(mBagDatas.Count);
        SetDefaultToggle(0);
        OnWearedEquipPageNumChanged(0);
    }

    void OnWearedEquipPageNumChanged(int page)
    {
        int pagIdx = page;
        int start = BagMaxGridCount * pagIdx;
        int lstart = Mathf.Max(0, start - BagMaxGridCount);
        int end = start + BagMaxGridCount;
        int rend = end + BagMaxGridCount;
        var bagItems = CSBagInfo.Instance.GetEquipItemData();
        int idx = 0;
        mBagDatasCommon.Clear();
        var it = bagItems.GetEnumerator();
        int cnt = 0;
        while (it.MoveNext())
        {
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(it.Current.Value.configId, out item) || !CSBagInfo.Instance.IsNormalEquip(item))
            {
                continue;
            }

            mBagDatasCommon.Append(it.Current.Value);
            ++idx;
            ++cnt;
        }

        //设置左边背包
        mBagDatas.Clear();
        for (int i = lstart; i < start && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendL, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsL.CustomActive(cnt > BagMaxGridCount);
        mGridItemsL.Reposition();
        mLPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;
        //设置中间段
        mBagDatas.Clear();
        for (int i = start; i < end && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItems, mBagDatas, cnt > BagMaxGridCount);
        mGridItems.Reposition();
        //设置尾端
        mBagDatas.Clear();
        for (int i = end; i < rend && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendR, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsR.CustomActive(cnt > BagMaxGridCount);
        mGridItemsR.Reposition();
        mRPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;

        EnableDrag(cnt > BagMaxGridCount);
        mBagScrollview.ResetPosition();
    }

    void OnWearedNeiGongEquipClicked(bool value)
    {
        EnableBagItems(value);
        if (!value)
        {
            return;
        }
        mOpMode = OpMode.OM_NEIGONG;

        var datas = CSBagInfo.Instance.GetEquipItemData();
        mBagDatas.Clear();
        for (var it = datas.GetEnumerator();it.MoveNext();)
        {
            TABLE.ITEM item = null;
            if(ItemTableManager.Instance.TryGetValue(it.Current.Value.configId,out item) && CSBagInfo.Instance.IsWoLongEquip(item))
            {
                mBagDatas.Append(it.Current.Value);
            }
        }

        ResetTogglePoints(mBagDatas.Count);
        SetDefaultToggle(0);
        OnWearedNeiGongEquipPageNumChanged(0);
    }

    void OnWearedNeiGongEquipPageNumChanged(int page)
    {
        int pagIdx = page;
        int start = BagMaxGridCount * pagIdx;
        int lstart = Mathf.Max(0, start - BagMaxGridCount);
        int end = start + BagMaxGridCount;
        int rend = end + BagMaxGridCount;
        var bagItems = CSBagInfo.Instance.GetEquipItemData();
        int idx = 0;
        mBagDatasCommon.Clear();
        var it = bagItems.GetEnumerator();
        int cnt = 0;
        while (it.MoveNext())
        {
            TABLE.ITEM item = null;
            if (!ItemTableManager.Instance.TryGetValue(it.Current.Value.configId, out item) || !CSBagInfo.Instance.IsWoLongEquip(item))
            {
                continue;
            }

            mBagDatasCommon.Append(it.Current.Value);
            ++idx;
            ++cnt;
        }

        //设置左边背包
        mBagDatas.Clear();
        for (int i = lstart; i < start && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendL, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsL.CustomActive(cnt > BagMaxGridCount);
        mGridItemsL.Reposition();
        mLPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;
        //设置中间段
        mBagDatas.Clear();
        for (int i = start; i < end && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItems, mBagDatas, cnt > BagMaxGridCount);
        mGridItems.Reposition();
        //设置尾端
        mBagDatas.Clear();
        for (int i = end; i < rend && i < mBagDatasCommon.Count; ++i)
        {
            mBagDatas.Append(mBagDatasCommon[i]);
        }
        SetBagItems(mBagItemsExtendR, mBagDatas, cnt > BagMaxGridCount);
        mGridItemsR.CustomActive(cnt > BagMaxGridCount);
        mGridItemsR.Reposition();
        mRPanel.alpha = mBagDatas.Count > 0 ? 1.0f : 0.0f;

        EnableDrag(cnt > BagMaxGridCount);
        mBagScrollview.ResetPosition();
    }

    protected void SetBagItems(FastArrayElementFromPool<UIItemBase> bagItems,FastArrayElementKeepHandle<bag.BagItemInfo> bagDatas,bool enableDrag)
    {
        bagItems.Count = BagMaxGridCount;
        for (int i = 0; i < bagItems.Count; ++i)
        {
            UIEventListener handle = UIEventListener.Get(bagItems[i].obj);
            if(enableDrag)
            {
                handle.onDrag = OnDragItem;
                handle.onDragEnd = OnDragEnd;
            }
            else
            {
                handle.onDrag = null;
                handle.onDragEnd = null;
            }
            if (i < bagDatas.Count)
            {
                var data = bagDatas[i];
                bagItems[i].Refresh(data);
                handle.onClick = go => { OnSendItemData(data); };
            }
            else
            {
                handle.onClick = null;
                bagItems[i].UnInit();
            }
        }
    }

    private void OnSendItemData(bag.BagItemInfo itemInfo)
    {
        if (null != itemInfo)
        {
            var tbl_item = ItemTableManager.Instance.GetItemCfg(itemInfo.configId);
            if(null != tbl_item)
            {
                var link = CSChatManager.Instance.AddItem(channel, tbl_item, itemInfo);
                if(CSChatManager.Instance.GetLastUrl(channel))
                {
                    onLinkSelected?.Invoke(CSString.Format(394), LinkInsertMode.LM_INSERT_CURRENT);
                }
                onLinkSelected?.Invoke(link.Express, LinkInsertMode.LM_INSERT_CURRENT);
                CSChatManager.Instance.SetLastUrl(channel, true);
            }
        }
    }

    void OnLocationClicked(bool value)
    {
        mLocalPosition.gameObject.SetActive(value);
        if(!value)
        {
            return;
        }
        mOpMode = OpMode.OM_LOCATION;

        if (CSMainPlayerInfo.Instance != null)
        {
            TABLE.MAPINFO tbl_mapInfo;
            if (MapInfoTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out tbl_mapInfo))
            {
                var link = CSChatManager.Instance.AddLocation(channel, tbl_mapInfo,CSAvatarManager.MainPlayer.NewCell.Coord.x,CSAvatarManager.MainPlayer.NewCell.Coord.y);
                if(CSChatManager.Instance.GetLastUrl(channel))
                {
                    onLinkSelected?.Invoke(CSString.Format(394),LinkInsertMode.LM_INSERT_END);
                }
                onLinkSelected?.Invoke(link.Express, LinkInsertMode.LM_INSERT_END);

                CSChatManager.Instance.SetLastUrl(channel, true);

                CSStringBuilder.Clear();
                mLocalPosition.text = CSString.Format(311, tbl_mapInfo.name,CSAvatarManager.MainPlayer.NewCell.Coord.x,CSAvatarManager.MainPlayer.NewCell.Coord.y);
            }
        }
    }

    protected void EnableBagItems(bool enable)
    {
        mBagScrollview.gameObject.SetActive(enable);
        mToggleGroup.gameObject.SetActive(enable);
    }

    protected void OnEmotionClicked(bool value)
    {
        mEmotionPanel?.SetActive(value);
        if(!value)
        {
            return;
        }

        mOpMode = OpMode.OM_EMOTION;
        for (int i = 0; i < mEmotionGrids.transform.childCount; i++)
        {
            UIEventListener.Get(mEmotionGrids.transform.GetChild(i).gameObject).onClick = OnEmoticonClick;
        }
    }

    /// <summary>
    /// 点击表情
    /// </summary>
    /// <param name="gp"></param>
    public void OnEmoticonClick(GameObject gp)
    {
        string linkValue = gp.GetComponent<UISprite>().spriteName;
        var link = CSChatManager.Instance.AddEmotion(channel,linkValue);
        onLinkSelected?.Invoke(link.Express, LinkInsertMode.LM_INSERT_CURRENT);
    }

    public void Show(UIChatAddPanelData data)
    {
        if (!UIPrefab.activeSelf)
        {
            UIPrefab.SetActive(true);
            mWindowPositoinTween.from = mWindowPositoinTween.value;
            mWindowPositoinTween.to = ItemPanelOpen;
            mWindowPositoinTween.ResetToBeginning();
            mWindowPositoinTween.PlayForward();
        }

        channel = data.channel;
        channelType = data.channelType;
        onLinkSelected = data.onLinkSelected;
        mEmotionToggle.value = true;
    }

    protected void OnClosePanel(uint id,object argv)
    {
        if (UIPrefab.activeSelf)
        {
            mWindowPositoinTween.from = mWindowPositoinTween.value;
            mWindowPositoinTween.to = ItemPanelClose;
            mWindowPositoinTween.ResetToBeginning();
            mWindowPositoinTween.PlayForward();
        }
        channelType = ChatChannelType.None;
    }

    private void ItemWindowTweenFinish()
    {
        if (mWindowPositoinTween.value == ItemPanelClose)
        {
            UIManager.Instance.ClosePanel<UIChatAddPanel>();
        }
    }
}