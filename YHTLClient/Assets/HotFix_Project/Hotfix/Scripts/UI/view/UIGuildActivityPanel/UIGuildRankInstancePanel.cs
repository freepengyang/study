using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rank;


public partial class UIGuildRankInstancePanel : UIBasePanel
{
    public override UILayerType PanelLayerType => UILayerType.Resident;

    public override bool ShowGaussianBlur => false;


    RankData myGuildData;
    int myGuildRank;

    ILBetterList<GuildCombatRankData> showRanks = new ILBetterList<GuildCombatRankData>();

    string noneStr;

    public override void Init()
	{
		base.Init();

        noneStr = ClientTipsTableManager.Instance.GetClientTipsContext(1163);

        mClientEvent.AddEvent(CEvent.RankInfoChange, UpdateRanks);
    }
	
	public override void Show()
	{
		base.Show();

        RefreshUI();

        Net.ReqRankInfoMessage((int)RankType.GUILD_BATTLE);
    }
	
	protected override void OnDestroy()
	{
        mgrid_ranks.UnBind<UIGuildCombatRankBinder>();
        mPoolHandleManager.RecycleAll();
        showRanks?.Clear();
        showRanks = null;
        myGuildData = null;

        base.OnDestroy();
	}


    void UpdateRanks(uint id, object data)
    {
        RankInfo msg = data as RankInfo;
        if (msg.type != (int)RankType.GUILD_BATTLE)
            return;

        if (msg.myRank >= 0 && msg.myRank < msg.ranks.Count)
        {
            myGuildData = msg.ranks[msg.myRank];
        }
        myGuildRank = msg.myRank;

        mPoolHandleManager.RecycleAll();
        showRanks.Clear();
        int count = msg.ranks.Count > 10 ? 10 : msg.ranks.Count;
        for (int i = 0; i < count; i++)
        {
            var rData = mPoolHandleManager.GetCustomClass<GuildCombatRankData>();
            rData.SetData(msg.ranks[i], i, i == msg.myRank);
            showRanks.Add(rData);
        }

        RefreshUI();
    }


    void RefreshUI()
    {
        mlb_rank.text = $"{noneStr}".BBCode(ColorType.Green);
        mlb_name.text = $"{CSMainPlayerInfo.Instance.GuildName}".BBCode(ColorType.Green);
        mlb_num.text = $"0".BBCode(ColorType.Green);
        if (myGuildData != null)
        {
            mlb_rank.text = $"{myGuildRank + 1}".BBCode(ColorType.Green);
            mlb_name.text = $"{myGuildData.name}".BBCode(ColorType.Green);
            string score = myGuildData.value > 10000 ? UtilityMath.GetDecimalValue(myGuildData.value, "F2") : myGuildData.value.ToString();
            mlb_num.text = $"{score}".BBCode(ColorType.Green);
        }

        if (showRanks != null)
        {
            mgrid_ranks.Bind<GuildCombatRankData, UIGuildCombatRankBinder>(showRanks, mPoolHandleManager);
        }
    }

}


public class UIGuildCombatRankBinder : UIBinder
{
    UILabel lb_rank;
    UILabel lb_name;
    UILabel lb_score;

    GuildCombatRankData mData;

    public override void Init(UIEventListener handle)
    {
        lb_rank = Get<UILabel>("rank");
        lb_name = Get<UILabel>("name");
        lb_score = Get<UILabel>("num");
    }


    public override void Bind(object data)
    {
        mData = data as GuildCombatRankData;
        if (mData == null || mData.rankData == null) return;
        ColorType color = mData.isMine ? ColorType.Green : ColorType.MainText;
        lb_rank.text = (mData.rank + 1).ToString().BBCode(color);
        lb_name.text = mData.rankData.name.BBCode(color);
        string score = mData.rankData.value > 10000 ? UtilityMath.GetDecimalValue(mData.rankData.value, "F2") : mData.rankData.value.ToString();
        lb_score.text = score.BBCode(color);
    }


    public override void OnDestroy()
    {
        mData = null;
        lb_rank = null;
        lb_name = null;
        lb_score = null;
    }
}



public class GuildCombatRankData : IDispose
{
    public RankData rankData;
    public int rank;
    public bool isMine;

    public void Dispose()
    {
        rankData = null;
    }

    public void SetData(RankData data, int _rank, bool _isMine = false)
    {
        rankData = data;
        rank = _rank;
        isMine = _isMine;
    }
}
