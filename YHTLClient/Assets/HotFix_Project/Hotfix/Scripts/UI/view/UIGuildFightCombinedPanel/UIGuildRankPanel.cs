using UnityEngine;
using UnityEngine.UI;

public partial class UIGuildRankPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		mClientEvent.AddEvent(CEvent.OnGuildFightRankListChanged, OnGuildFightRankListChanged);

		mRoles = new GameObject[3];
		mRoles[0] = mrole2;
        mRoles[1] = mrole1;
        mRoles[2] = mrole3;

		if(null != mbtn_help)
			mbtn_help.onClick = OnClickHelp;

		CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "guildfight_mask");

        if (null != mScrollBar)
            EventDelegate.Add(mScrollBar.onChange, InitArrow);
    }

	protected void RefreshAll()
	{
        RefreshModels();
        RefreshRankList();
		InitArrow();
	}

    protected void InitArrow()
    {
        mDownArrow.CustomActive(mScrollBar.value < 1.0f && mScrollView.shouldMoveVertically);
        mUpArrow.CustomActive(mScrollBar.value > 0 && mScrollView.shouldMoveVertically);
    }

    protected void OnClickHelp(GameObject go)
	{
		UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.GuildFightBoxHelp);
	}
	
	public override void Show()
	{
		base.Show();

		RefreshAll();
		CSGuildFightManager.Instance.RequestSabacLastRankInfo();
	}

	GameObject[] mRoles;
	protected void RefreshModels()
	{
		var playerList = CSGuildFightManager.Instance.FightList;
		for (int i = 0;null != mRoles && i < mRoles.Length; ++i)
		{
			//衣服
			GameObject goCloth = mRoles[i].transform.Find("r/Cloth").gameObject;
			//武器
			GameObject goWeapon = mRoles[i].transform.Find("r/Weapon").gameObject;
            //宝箱
            GameObject goBox = mRoles[i].transform.Find("box").gameObject;
            //玩家名字
            UILabel lb_name = mRoles[i].transform.Find("name").GetComponent<UILabel>();
			//虚伪以待
			GameObject goLabel = mRoles[i].transform.Find("xwyd").gameObject;
			if (i < playerList.Count)
			{
				//玩家模型数据
				sabac.PlayerModelInfo modelData = playerList[i].model;

				if(null != modelData)
				{
                    //设置武器
                    goWeapon.LoadAvatarModel(modelData.roleBrief, AvatarModelType.AMT_Weapon);

                    //设置时装
                    goCloth.LoadAvatarModel(modelData.roleBrief, AvatarModelType.AMT_Cloth);
                }
				else
				{
                    //设置武器
                    goWeapon.LoadAvatarModel(null, AvatarModelType.AMT_Weapon);

                    //设置时装
                    goCloth.LoadAvatarModel(null, AvatarModelType.AMT_Cloth);
                }

                //玩家名字
                lb_name.text = playerList[i].name;

				//设置宝箱点击
				UIEventListener.Get(goBox, i + 1).onClick = OnClickPreView;

				//隐藏虚位以待
				goLabel.CustomActive(false);
			}
			else
			{
				lb_name.text = string.Empty;
                //设置宝箱点击
                UIEventListener.Get(goBox, i + 1).onClick = OnClickPreView;
                //设置武器
                //goWeapon.LoadAvatarModel(CSMainPlayerInfo.Instance.Weapon, CSFashionInfo.Instance.GetMyFashionId(), CSMainPlayerInfo.Instance.Sex, AvatarModelType.AMT_Weapon);

                //设置时装
                //goCloth.LoadAvatarModel(CSMainPlayerInfo.Instance.BodyModel, CSFashionInfo.Instance.GetMyFashionId(), CSMainPlayerInfo.Instance.Sex,AvatarModelType.AMT_Cloth);
                goCloth.CustomActive(false);
				goWeapon.CustomActive(false);
                //显示虚位以待
                goLabel.CustomActive(true);
            }
		}
	}

	void OnClickPreView(GameObject go)
	{
		if(UIEventListener.Get(go).parameter is int id)
		{
            UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
            {
                (f as UIUnsealRewardPanel).Show(GuildFightPlayerInfo.getAwardValue(id));
            });
		}
	}

    protected void RefreshRankList()
    {
		var playerList = CSGuildFightManager.Instance.RankList;
		mContainer.Bind<GuildFightListItemBinder,GuildFightPlayerInfo>(playerList);
    }

    void OnGuildFightRankListChanged(uint id,object argv)
	{
		RefreshAll();
    }


	protected override void OnDestroy()
    {
        if (null != mScrollBar)
            EventDelegate.Remove(mScrollBar.onChange, InitArrow);

        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
		if (null != mRoles)
		{
			for(int i = 0; i < mRoles.Length; ++i)
			{
                //衣服
                GameObject goCloth = mRoles[i].transform.Find("r/Cloth").gameObject;
                //武器
                GameObject goWeapon = mRoles[i].transform.Find("r/Weapon").gameObject;

                CSEffectPlayMgr.Instance.Recycle(goCloth);
                CSEffectPlayMgr.Instance.Recycle(goWeapon);
                mRoles[i] = null;
			}
			mRoles = null;
		}
		mClientEvent.RemoveEvent(CEvent.OnGuildFightRankListChanged, OnGuildFightRankListChanged);
		base.OnDestroy();
	}
}
