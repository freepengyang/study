using Google.Protobuf.Collections;
using System.ComponentModel;
using TABLE;
using UnityEngine;

public class GuildFightPlayerInfo : IndexedItem
{
	public int Index { get; set; }
    public long guid;//玩家GUID
    public string name;//名字
    public string guildName;//公会名称
    public int rank;//排行
    public int RankAwardId//宝箱ID
    {
        get
        {
            return (30000 + rank) *1000;
        }
    }
    public sabac.PlayerModelInfo model;
    public int weaponModelId;//武器模型ID
    public int clothModelId;//时装模型ID
    public long fetchTime;//奖励领取时间

    public string getAwardValue()
    {
        return getAwardValue(rank);
    }

    public static string getAwardValue(int rankId)
    {
        int id = (30000 + rankId) * 1000;
        TABLE.RANKAWARDS rankAward = null;
        if (!RankAwardsTableManager.Instance.TryGetValue(id, out rankAward))
            return string.Empty;
        return rankAward.awards;
    }
}

public class GuildFightListItemBinder : UIBinder
{
    protected GuildFightPlayerInfo mData;
    UILabel lb_rank;
    UILabel lb_guild_name;
    UISprite sp_box;
    Transform go_redpoint;
    UILabel lb_name;
    UIEventListener btnBox;
    public override void Init(UIEventListener handle)
    {
        lb_rank = Get<UILabel>("lb_rank");
        lb_guild_name = Get<UILabel>("lb_guild_name");
        sp_box = Get<UISprite>("sp_box");
        btnBox = sp_box.GetComponent<UIEventListener>();
        lb_name = Get<UILabel>("lb_name");
        go_redpoint = handle.transform.Find("sp_box/go_redpoint");
        btnBox.onClick = PreviewAward;
    }

    public override void Bind(object data)
    {
        mData = data as GuildFightPlayerInfo;
        if (null != lb_rank)
            lb_rank.text = $"{mData.rank}";
        if (null != lb_name)
        {
            if(!string.IsNullOrEmpty(mData.name))
            {
                lb_name.text = $"{mData.name}".BBCode(ColorType.MainText);
            }
            else
            {
                lb_name.text = CSString.Format(1041);
            }
        }
        if (null != lb_guild_name)
        {
            if (!string.IsNullOrEmpty(mData.guildName))
                lb_guild_name.text = $"{mData.guildName}".BBCode(ColorType.MainText);
            else
                lb_guild_name.text = CSString.Format(1714);
        }
        //下面是宝箱领取红点，高飞说暂时去掉
        go_redpoint.CustomActive(false);
        //bool fetched = mData.fetchTime != 0;
        //bool canFetch = !fetched && mData.guid == CSMainPlayerInfo.Instance.ID;
        //go_redpoint.CustomActive(canFetch);
        //PlayEffect(canFetch);
    }

    protected void PlayEffect(bool play)
    {

    }

    protected void PreviewAward(GameObject go)
    {
        if (null == mData)
            return;

        UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
        {
            (f as UIUnsealRewardPanel).Show(mData.getAwardValue());
        });
    }

    public override void OnDestroy()
    {
        mData = null;
        lb_rank = null;
        lb_guild_name = null;
        sp_box = null;
        btnBox = null;
        lb_name = null;
        go_redpoint = null;
        if(null != btnBox)
        {
            btnBox.onClick = null;
            btnBox = null;
        }
    }
}