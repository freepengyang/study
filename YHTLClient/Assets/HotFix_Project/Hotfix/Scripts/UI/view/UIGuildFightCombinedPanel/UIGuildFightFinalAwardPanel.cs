using Google.Protobuf.Collections;
using TABLE;

public partial class UIGuildFightFinalAwardPanel : UIBasePanel
{
	FastArrayElementFromPool<UIItemBase> mUIItems;

	public override void Init()
	{
		base.Init();

		mbtn_close.onClick = this.Close;
		mUIItems = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_reward.transform);
	}

    public void Show(string guildName,string value,bool win)
	{
		Show(guildName, mPoolHandleManager.Split(value), win);
	}

    public void Show(string guildName, RepeatedField<KEYVALUE> kvs, bool win)
    {
        ScriptBinder._SetAction(win ? "win" : "lose");
		kvs = mPoolHandleManager.GetEffectItems(kvs);
        mUIItems.Clear();
        for (int i = 0; i < kvs.Count; ++i)
        {
            TABLE.ITEM itemCfg = ItemTableManager.Instance.GetItemCfg(kvs[i].key);
            var item = mUIItems.Append();
            item.Refresh(itemCfg, ItemClick);
            item.SetCount(kvs[i].value);
        }
    }

    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
