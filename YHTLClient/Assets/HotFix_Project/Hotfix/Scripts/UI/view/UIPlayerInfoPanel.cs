using FlyBirds.Model;
using UnityEngine;

public class PlayerInfoData
{
    public CSPlayerInfo playerInfo;
    public bool isNative;
    public bool hasAttacked;
}

public class UIPlayerInfoBinder : UIBinder
{
    Transform player_transform;
    UISprite sp_tubiao;
    UILabel lb_name;
    UILabel lb_level;
    UISlider slider_hp;
    UISlider slider_magic;
    UISprite fg;
    GameObject go_effect;
    GameObject sp_select;
    UIEventListener sp_click;
    static string[] careerIconName = new string[3]
    {
        "mission_zhan","mission_fa","mission_dao",
    };

    PlayerInfoData mData;
    CSPlayerInfo mPlayerInfo;
    public override void Init(UIEventListener handle)
    {
        player_transform = Get<Transform>("obj_player");
        sp_tubiao = Get<UISprite>("sp_tubiao", player_transform);
        lb_name = Get<UILabel>("lb_name", player_transform);
        lb_level = Get<UILabel>("lb_level", player_transform);
        slider_hp = Get<UISlider>("slider_hp", player_transform);
        slider_magic = Get<UISlider>("slider_magic", player_transform);
        fg = Get<UISprite>("slider_hp/fg");
        go_effect = Get<GameObject>("effect");
        sp_select = Get<GameObject>("sp_select", player_transform);
        sp_click = Get<UIEventListener>("sp_click", player_transform);
        sp_click.onClick = OnClick;

        HotManager.Instance.EventHandler.AddEvent(CEvent.NoSelectLastMyTeamPlayer, OnPlayerCancelChoiced);
        HotManager.Instance.EventHandler.AddEvent(CEvent.SelectLastMyTeamPlayer, OnPlayerChoiced);
    }

    protected void OnPlayerCancelChoiced(uint id,object argv)
    {
        if(argv is long playerId)
        {
            if (null != mData && mData.playerInfo.ID == playerId)
            {
                sp_select.CustomActive(false);
            }
        }
    }

    protected void OnPlayerChoiced(uint id, object argv)
    {
        if (argv is long playerId)
        {
            if (null != mData && mData.playerInfo.ID == playerId)
            {
                sp_select.CustomActive(true);
            }
        }
    }

    void OnClick(GameObject go)
    {
        if (null != mData)
        {
            long id = mData.playerInfo.ID;
            var avatar = CSAvatarManager.Instance.GetAvatar(id);
            if(null == avatar)
            {
                UtilityTips.ShowRedTips(2040);
                return;
            }
            CSSelectTargetManager.Instance.SelectAvatarByID(id);
        }
    }

    public static bool IsNative(CSPlayerInfo playerInfo)
    {
        if (null == playerInfo)
            return false;

        int mode = (CSMainPlayerInfo.Instance.PkModeMap == 0) ? CSMainPlayerInfo.Instance.PkMode : CSMainPlayerInfo.Instance.PkModeMap;
        if(mode == EPkMode.Peace)
        {
            return true;
        }

        if (mode == EPkMode.All)
        {
            return false;
        }

        if (mode == EPkMode.Team)
        {
            return playerInfo.TeamId > 0 && CSMainPlayerInfo.Instance.TeamId == playerInfo.TeamId;
        }

        if (mode == EPkMode.Union)
        {
            return playerInfo.GuildId > 0 && playerInfo.GuildId == CSMainPlayerInfo.Instance.GuildId;
        }

        if (CSAvatarManager.Instance.GetAvatarInfo(CSMainPlayerInfo.Instance.ID) is CSMainPlayerInfo mainPlayerInfo)
        {
            var esrc = CSHeadPlayer.GetPlayerNameColor(mainPlayerInfo);
            var etarget = CSHeadPlayer.GetPlayerNameColor(playerInfo);
            bool equal = (esrc == ColorType.White || esrc == ColorType.Yellow) == (etarget == ColorType.White || etarget == ColorType.Yellow);
            return equal;
        }

        return false;
    }

    public override void Bind(object data)
    {
        if(data is PlayerInfoData playerInfo && null != playerInfo.playerInfo)
        {
            mData = playerInfo;
            mPlayerInfo = playerInfo.playerInfo;

            player_transform.CustomActive(true);

            //occu icon
            if (null != sp_tubiao)
            {
                if(mPlayerInfo.Career >= 1 && mPlayerInfo.Career <= careerIconName.Length)
                    sp_tubiao.spriteName = careerIconName[mPlayerInfo.Career - 1];
                sp_tubiao.color = mPlayerInfo.HP > 0 ? Color.white : Color.gray;
            }

            //name
            if (null != lb_name)
            {
                if(mPlayerInfo.HP > 0)
                {
                    if (mData.isNative)
                        lb_name.text = mPlayerInfo.Name.BBCode(ColorType.Green);
                    else
                        lb_name.text = mPlayerInfo.Name.BBCode(ColorType.MainText);
                }
                else
                {
                    lb_name.text = mPlayerInfo.Name.BBCode(ColorType.WeakText);
                }
                //lb_name.color = UtilityColor.GetColor(mData.HP > 0 ? ColorType.White : ColorType.WeakText);
            }

            //level
            if (null != lb_level)
            {
                lb_level.text = $"Lv{mPlayerInfo.Level}";
            }

            float hpamount = mPlayerInfo.HP * 1.0f / mPlayerInfo.MaxHP;
            //hpamount
            if (null != slider_hp)
                slider_hp.value = hpamount;

            //hp front bg
            if(null != fg)
                fg.spriteName = hpamount >= 0.3f ? "team_hp2" : "team_hp";

            //mpamount
            if (null != slider_magic)
                slider_magic.value = mPlayerInfo.MP * 1f / mPlayerInfo.MaxMP;
        }
        else
        {
            mData = null;
            player_transform.CustomActive(false);
        }
    }

    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.NoSelectLastMyTeamPlayer, OnPlayerCancelChoiced);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.SelectLastMyTeamPlayer, OnPlayerChoiced);
        sp_click.onClick = null;
        sp_click = null;
        sp_tubiao = null;
        lb_name = null;
        lb_level = null;
        slider_hp = null;
        slider_magic = null;
        fg = null;
        sp_select = null;
        go_effect = null;
    }
}
public partial class UIPlayerInfoPanel : UIBasePanel
{
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    FastArrayElementFromPool<PlayerInfoData> mPlayerInfos;
    protected TimerEventHandle mTimer;
    public override void Init()
	{
		base.Init();
        mPlayerInfos = mPoolHandleManager.CreateGeneratePool<PlayerInfoData>(16);
        mTimer = CSTimer.Instance.InvokeRepeating(2.0f,2.0f,BindPlayers);
	}
	
	public override void Show()
	{
		base.Show();

        BindPlayers();
        mClientEvent.SendEvent(CEvent.ReqChoicedTeamPlayer);
    }

    protected void BindPlayers()
    {
        mPlayerInfos.Clear();
        var avatarMgr = CSAvatarManager.Instance;
        if(null != avatarMgr)
        {
            var avatarList = avatarMgr.GetAvatarList(EAvatarType.Player);
            if(null != avatarList)
            {
                for (int i = 0, max = avatarList.Count; i < max; ++i)
                {
                    if (avatarList[i].BaseInfo is CSPlayerInfo playerInfo)
                    {
                        var playerInfoData = mPlayerInfos.Append();
                        playerInfoData.playerInfo = playerInfo;
                        playerInfoData.isNative = UIPlayerInfoBinder.IsNative(playerInfo);
                        playerInfoData.hasAttacked = CSDamageInfo.Instance.GetDamage(playerInfo.ID) > 0;
                    }
                }
            }
        }

        for(int i = mPlayerInfos.Count; i < 4;++i)
        {
            var playerInfoData = mPlayerInfos.Append();
            playerInfoData.isNative = false;
            playerInfoData.hasAttacked = false;
            playerInfoData.playerInfo = null;
        }

        mPlayerInfos.Sort(PlayerInfoDataMaker);

        mgrid_team_players.Bind<PlayerInfoData, UIPlayerInfoBinder>(mPlayerInfos, mPoolHandleManager);
    }

    void PlayerInfoDataMaker(ref long sortedValue, PlayerInfoData value)
    {
        if (null != value.playerInfo)
            sortedValue = 0;
        else
            sortedValue = 10000;

        if (!value.hasAttacked)
            sortedValue += 1000;

        if (!value.isNative)
            sortedValue += 100;

        sortedValue <<= 32;

        if (null != value.playerInfo)
            sortedValue += (value.playerInfo.ID & 0xFFFFFFFF);
    }

    protected override void OnDestroy()
	{
        if(null != mTimer)
        {
            CSTimer.Instance.remove_timer(mTimer);
            mTimer = null;
        }
        mgrid_team_players?.UnBind<UIPlayerInfoBinder>();
        mgrid_team_players = null;
        mPlayerInfos?.Clear();
        mPlayerInfos = null;

        base.OnDestroy();
	}
}
