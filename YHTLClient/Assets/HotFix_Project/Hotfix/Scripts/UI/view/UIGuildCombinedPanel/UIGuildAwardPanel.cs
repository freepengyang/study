using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public partial class UIGuildAwardPanel : UIBasePanel
{
	FastArrayElementFromPool<UIItemBase> mMainItems;
	FastArrayElementFromPool<UIItemBase> mSecondItems;
    RepeatedField<KEYVALUE> mItemDatas;
    RepeatedField<KEYVALUE> mItemDatasOther;
    public override void Init()
	{
		base.Init();
        mMainItems = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mMainGrid.transform);
        mItemDatas = mPoolHandleManager.GetMailItems(82);
        mSecondItems = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mSecondGrid.transform);
        mItemDatasOther = mPoolHandleManager.GetMailItems(83);
        if(null != mbtn_help)
            mbtn_help.onClick = OnClickHelp;

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "guildfight_mask");
	}

    protected void OnClickHelp(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GuildFightHelp);
    }

    void ShowWeapon(int sex = 0)
    {
		int configId = sex == 1 ? 577 : 578;
		TABLE.SUNDRY sundryItem;
		if (!SundryTableManager.Instance.TryGetValue(configId, out sundryItem))
			return;
		var tokens = sundryItem.effect.Split('#');
		if (tokens.Length != 2)
			return;

		int clothId;
        if (!int.TryParse(tokens[0], out clothId))
            return;

        int weaponId;
        if (!int.TryParse(tokens[1], out weaponId))
            return;

		//      TABLE.FASHION fashionItem;
		//if(FashionTableManager.Instance.TryGetValue(weaponId, out fashionItem))
		//{
		//	CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon, fashionItem.weaponryModel.ToString(),ResourceType.UIWeapon);
		//}
		CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon, weaponId.ToString(), ResourceType.UIWeapon);
	}

    void ShowCloth(int sex = 0)
	{
        int configId = sex == 1 ? 577 : 578;
        TABLE.SUNDRY sundryItem;
        if (!SundryTableManager.Instance.TryGetValue(configId, out sundryItem))
            return;
        var tokens = sundryItem.effect.Split('#');
        if (tokens.Length != 2)
            return;

        int clothId;
        if (!int.TryParse(tokens[0], out clothId))
            return;

        int weaponId;
        if (!int.TryParse(tokens[1], out weaponId))
            return;

        mRole.CustomActive(false);
        mCloth.CustomActive(true);
        CSEffectPlayMgr.Instance.ShowUIEffect(mCloth, clothId.ToString(), ResourceType.UIPlayer);

		//TABLE.FASHION fashionItem;
		//if (FashionTableManager.Instance.TryGetValue(clothId, out fashionItem))
		//{
  //          CSEffectPlayMgr.Instance.ShowUIEffect(mCloth, fashionItem.clothesModel.ToString(),ResourceType.UIPlayer);
  //          mRole.CustomActive(false);
  //          mCloth.CustomActive(true);
  //      }
		//else
		//{
		//	mRole.CustomActive(true);
		//	mCloth.CustomActive(false);
		//	string str = sex == 1 ? "615000" : "625000";
		//	CSEffectPlayMgr.Instance.ShowUIEffect(mRole, str, ResourceType.UIPlayer);
		//}
    }

    public override void Show()
	{
		base.Show();

        if(mMainItems.Count == 0)
        {
            mMainItems.Count = mItemDatas.Count;
            for (int i = 0; i < mItemDatas.Count; ++i)
            {
                TABLE.ITEM item;
                if (!ItemTableManager.Instance.TryGetValue(mItemDatas[i].key, out item))
                    continue;

                mMainItems[i].Refresh(item);
                mMainItems[i].SetCount(mItemDatas[i].value);
            }
            mMainGrid.Reposition();
        }

        if(mSecondItems.Count == 0)
        {
            mSecondItems.Count = mItemDatasOther.Count;
            for (int i = 0; i < mItemDatasOther.Count; ++i)
            {
                TABLE.ITEM item;
                if (!ItemTableManager.Instance.TryGetValue(mItemDatasOther[i].key, out item))
                    continue;

                mSecondItems[i].Refresh(item);
                mSecondItems[i].SetCount(mItemDatasOther[i].value);
            }
            mSecondGrid.Reposition();
        }
		ShowWeapon(CSMainPlayerInfo.Instance.Sex);
		ShowCloth(CSMainPlayerInfo.Instance.Sex);
	}
	
	protected override void OnDestroy()
	{
        mItemDatas?.OnRecycle(mPoolHandleManager);
        mItemDatas = null;
        mItemDatasOther?.OnRecycle(mPoolHandleManager);
        mItemDatasOther = null;
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
		CSEffectPlayMgr.Instance.Recycle(mRole);
		CSEffectPlayMgr.Instance.Recycle(mWeapon);
		CSEffectPlayMgr.Instance.Recycle(mCloth);
		mMainItems?.Clear();
		mMainItems = null;
		mSecondItems?.Clear();
		mSecondItems = null;
		base.OnDestroy();
	}
}
