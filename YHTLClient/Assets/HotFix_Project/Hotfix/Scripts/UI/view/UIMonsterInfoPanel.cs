using FlyBirds.Model;
using UnityEngine;

public class MonsterInfoData
{
    public CSMonsterInfo monsterInfo;
    public long sortId;
    public TABLE.MONSTERINFO config;
}
public class UIMonsterInfoBinder : UIBinder
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

    MonsterInfoData mData;
    CSMonsterInfo mMonsterInfo;
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
        HotManager.Instance.EventHandler.AddEvent(CEvent.NoSelectMonster, OnMonsterCancelChoiced);
        HotManager.Instance.EventHandler.AddEvent(CEvent.SelectMonster, OnMonsterChoiced);
    }

    protected void OnMonsterCancelChoiced(uint id, object argv)
    {
        if (argv is long monsterId)
        {
            if (null != mData && null != mData.monsterInfo && mData.monsterInfo.ID == monsterId)
            {
                sp_select.CustomActive(false);
            }
        }
    }

    protected void OnMonsterChoiced(uint id, object argv)
    {
        if (argv is long monsterId)
        {
            if (null != mData && null != mData.monsterInfo && mData.monsterInfo.ID == monsterId)
            {
                sp_select.CustomActive(true);
            }
        }
    }

    void OnClick(GameObject go)
    {
        if (null != mData && null != mData.monsterInfo)
        {
            long id = mData.monsterInfo.ID;
            var avatar = CSAvatarManager.Instance.GetAvatar(id);
            if (null == avatar)
            {
                UtilityTips.ShowRedTips(2040);
                return;
            }
            CSSelectTargetManager.Instance.SelectAvatarByID(id);
        }
    }

    public override void Bind(object data)
    {
        if (data is MonsterInfoData playerInfo && null != playerInfo.monsterInfo)
        {
            mData = playerInfo;
            mMonsterInfo = playerInfo.monsterInfo;

            player_transform.CustomActive(true);

            if(!MonsterInfoTableManager.Instance.TryGetValue((int)mMonsterInfo.ConfigId,out TABLE.MONSTERINFO monsterInfo))
            {
                return;
            }

            bool isBoss = monsterInfo.type == 2 || monsterInfo.type == 5;
            sp_tubiao.CustomActive(true);
            if(null != sp_tubiao)
                sp_tubiao.spriteName = isBoss ? "monster_level_1" : "monster_level_0";

            //name
            if (null != lb_name)
            {
                lb_name.text = CSString.Format(2019, monsterInfo.name,monsterInfo.level).BBCode(monsterInfo.quality);
            }

            //owner
            if (null != lb_level)
            {
                if (string.IsNullOrEmpty(mMonsterInfo.MonsterOwner))
                    lb_level.text = CSString.Format(2026).BBCode(ColorType.MainText);
                else
                    lb_level.text = CSString.Format(2018, mMonsterInfo.MonsterOwner).BBCode(ColorType.MainText);
            }

            float hpamount = mMonsterInfo.HP * 1.0f / mMonsterInfo.MaxHP;
            //hpamount
            if (null != slider_hp)
                slider_hp.value = hpamount;

            //hp front bg
            if (null != fg)
                fg.spriteName = hpamount >= 0.3f ? "team_hp2" : "team_hp";
        }
        else
        {
            player_transform.CustomActive(false);
        }
    }

    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.NoSelectMonster, OnMonsterCancelChoiced);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.SelectMonster, OnMonsterChoiced);
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
public partial class UIMonsterInfoPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }

    FastArrayElementFromPool<MonsterInfoData> mMonsterInfos;
    protected TimerEventHandle mTimer;
    public override void Init()
    {
        base.Init();

        mMonsterInfos = mPoolHandleManager.CreateGeneratePool<MonsterInfoData>(16);
        mTimer = CSTimer.Instance.InvokeRepeating(2.0f, 2.0f, BindMonsters);
    }

    public override void Show()
	{
		base.Show();

        BindMonsters();
        mClientEvent.SendEvent(CEvent.ReqChoicedMonster);
    }

    protected void BindMonsters()
    {
        mMonsterInfos.Clear();
        var avatarList = CSAvatarManager.Instance.GetAvatarList(EAvatarType.Monster);
        if (null != avatarList)
        {
            for (int i = 0, max = avatarList.Count; i < max; ++i)
            {
                if (avatarList[i].BaseInfo is CSMonsterInfo monsterInfo)
                {
                    TABLE.MONSTERINFO monsterConfig;
                    if(!MonsterInfoTableManager.Instance.TryGetValue((int)monsterInfo.ConfigId,out monsterConfig))
                    {
                        continue;
                    }
                    var monsterInfoData = mMonsterInfos.Append();
                    monsterInfoData.monsterInfo = monsterInfo;
                    monsterInfoData.config = monsterConfig;
                    monsterInfoData.sortId = 0;
                    if(!(monsterConfig.type == 2 || monsterConfig.type == 5))
                        monsterInfoData.sortId += 10000;
                    if (string.IsNullOrEmpty(monsterInfo.MonsterOwner))
                        monsterInfoData.sortId += 1000;
                    monsterInfoData.sortId += (10 - monsterInfo.Quality);
                    monsterInfoData.sortId <<= 32;
                    monsterInfoData.sortId += (monsterInfo.ID & 0xFFFFFFFF);
                }
            }
        }

        for (int i = mMonsterInfos.Count; i < 4; ++i)
        {
            var monsterInfoData = mMonsterInfos.Append();
            monsterInfoData.monsterInfo = null;
            monsterInfoData.sortId = 2100000000L << 32;
            monsterInfoData.config = null;
        }

        mMonsterInfos.Sort(MonsterInfoDataMaker);
        mgrid_team_players.Bind<MonsterInfoData, UIMonsterInfoBinder>(mMonsterInfos, mPoolHandleManager);
    }

    void MonsterInfoDataMaker(ref long sortedValue, MonsterInfoData value)
    {
        sortedValue = value.sortId;
    }

    protected override void OnDestroy()
	{
        if (null != mTimer)
        {
            CSTimer.Instance.remove_timer(mTimer);
            mTimer = null;
        }
        mgrid_team_players?.UnBind<UIMonsterInfoBinder>();
        mgrid_team_players = null;
        mMonsterInfos?.Clear();
        mMonsterInfos = null;

        base.OnDestroy();
	}
}
