using UnityEngine;

public partial class UIRenameGuildPanel : UIBasePanel
{
    FastArrayElementKeepHandle<ItemBarData> mItemDatas;
    public override void Init()
    {
        base.Init();
        AddCollider();
        if (null != mbtn_close)
            mbtn_close.onClick = this.Close;
        if (null != mbtn_confirm)
            mbtn_confirm.onClick = OnBtnRename;
        if (null != mbtn_bg)
            mbtn_bg.onClick = this.Close;
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, ResGetUnionTabMessage);

        mItemDatas = new FastArrayElementKeepHandle<ItemBarData>(2);
        var itemBarData = UIItemBarManager.Instance.Get();
        itemBarData.cfgId = 50000502;//行会改名卡
        itemBarData.needed = 1;
        itemBarData.owned = CSItemCountManager.Instance.GetItemCount(itemBarData.cfgId);
        itemBarData.flag = (int)ItemBarData.ItemBarType.IBT_COMPARE | (int)ItemBarData.ItemBarType.IBT_RED_GREEN | (int)ItemBarData.ItemBarType.IBT_ADD | (int)ItemBarData.ItemBarType.IBT_SMALL_ICON;
        mItemDatas.Append(itemBarData);
    }

    public override void Show()
    {
        base.Show();

        UIItemBarManager.Instance.Bind(mgrid, mItemDatas);
    }

    private void OnBtnRename(GameObject obj)
    {
        if (null == mrename)
            return;

        if (string.IsNullOrEmpty(mrename.value))
        {
            UtilityTips.ShowRedTips(748);
            return;
        }

        if (mrename.value.Length >= 10)
        {
            UtilityTips.ShowRedTips(788);
            return;
        }

        if(!CSGuildInfo.Instance.GuildRenameId.IsItemEnough(1, 50000502))
        {
            return;
        }

        Net.CSUnionRenameMessage(mrename.value);
    }

    protected override void OnDestroy()
    {
        UIItemBarManager.Instance.UnBind(mgrid);
        mgrid = null;
        mItemDatas.Clear();

        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, ResGetUnionTabMessage);
        base.OnDestroy();
    }

    private void ResGetUnionTabMessage(uint id, object argv)
    {
        this.Close();
    }
}