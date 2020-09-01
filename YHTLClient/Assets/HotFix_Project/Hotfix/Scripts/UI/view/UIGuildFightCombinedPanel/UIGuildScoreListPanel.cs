public class UIScoreRankItemBinder : UIBinder
{
	UISprite sp_bg;
	UISprite sp_rank;
	UILabel lb_rank;
	UILabel lb_guildName;
	UILabel lb_name;
	UILabel lb_score;

	public override void Init(UIEventListener handle)
	{
		sp_bg = Get<UISprite>("sp_bg");
		sp_rank = Get<UISprite>("sp_rank");
		lb_rank = Get<UILabel>("lb_rank");
		lb_guildName = Get<UILabel>("lb_guildName");
		lb_name = Get<UILabel>("lb_name");
		lb_score = Get<UILabel>("lb_score");
	}

	GuildFightRankInfo mData;
	public override void Bind(object data)
	{
		mData = data as GuildFightRankInfo;
		if (null == mData)
			return;
		if (null != sp_bg)
			sp_bg.CustomActive(true);

		sp_rank.CustomActive(mData.rank <= 3);
		if (null != sp_rank && mData.rank <= 3)
			sp_rank.spriteName = $"rank{mData.rank}";

		lb_rank.CustomActive(mData.rank > 3);
		if (null != lb_rank && mData.rank > 3)
			lb_rank.text = $"{mData.rank}";

		if (null != lb_guildName)
			lb_guildName.text = mData.guildName;
		if (null != lb_name)
			lb_name.text = mData.playerName;
		if (null != lb_score)
			lb_score.text = $"{mData.score}";
	}

	public override void OnDestroy()
	{
		lb_score = null;
		lb_name = null;
		lb_guildName = null;
		lb_rank = null;
		sp_bg = null;
	}
}

public partial class UIGuildScoreListPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
	{
		base.Init();

		mbtn_close.onClick = this.Close;
		mbtnBG.onClick = this.Close;
		mClientEvent.AddEvent(CEvent.OnGuildFightScoreListChanged, OnGuildFightScoreListChanged);
		EventDelegate.Add(mScrollBar.onChange, InitArrow);
		mScrollView.onStoppedMoving += InitArrow;
	}

    protected void InitArrow()
    {
		marrow.CustomActive(mScrollBar.value < 1.0f && mScrollView.shouldMoveVertically);
    }

    protected void Refresh()
	{
		if (null != mmyrank)
		{
			if(CSGuildFightManager.Instance.MyRank > 10 || CSGuildFightManager.Instance.MyRank < 1)
			{
				//Î´ÉÏ°ñ
				mmyrank.text = CSString.Format(1045);
			}
			else
				mmyrank.text = string.Format(mmyrank.FormatStr, CSGuildFightManager.Instance.MyRank);
		}
			
        if (null != mmyscore)
			mmyscore.text = string.Format(mmyscore.FormatStr, CSGuildFightManager.Instance.MyScore);
		var datas = CSGuildFightManager.Instance.ScoreRankList;
		mGrildList.Bind<UIScoreRankItemBinder,GuildFightRankInfo>(datas);
    }
	
	public override void Show()
	{
		base.Show();

		CSGuildFightManager.Instance.RequestSabacCurrentRankInfo();
		Refresh();
	}

	protected void OnGuildFightScoreListChanged(uint id,object argv)
	{
		Refresh();
	}

	protected override void OnDestroy()
	{
        EventDelegate.Remove(mScrollBar.onChange, InitArrow);
        mScrollView.onStoppedMoving -= InitArrow;
        mGrildList?.UnBind<UIScoreRankItemBinder>();
		mGrildList = null;
		mClientEvent.RemoveEvent(CEvent.OnGuildFightScoreListChanged, OnGuildFightScoreListChanged);
		base.OnDestroy();
	}
}
