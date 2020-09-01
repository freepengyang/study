using UnityEngine;
using System.Collections;
public enum HelpType
{
    None,
    EQUIP_RECYCLE = 1,//装备回收
    EQUIP_RECAST = 2,//装备重铸
    HELP_TIME_EXP = 3,//泡点神符
    PERSONAL_BOSS = 4,//个人boss
    EQUIP_REFINE = 5,//装备洗练
    VIGOR = 6,        //精力值
    Warehouse = 7,//仓库
    Seal = 8,//封印
    WoLongLevel = 9,//卧龙等级
    Friend = 10,//好友
    HeartValue = 11,//亲密度
    DebtValue = 12,//仇恨度
    WORLD_BOSS = 13,//世界boss
    HandBookSetup = 14,//图鉴装配面板
    HandBookUpgrade = 15,//图鉴升级
    HandBookMerge = 16,//图鉴合并
	HELP_LIANTI = 17,//炼体
    Fashion = 18,//时装
    Auction = 19,//交易行
	WarSoul = 20,//战魂
    Strength = 21,//强化
    GuildInfo = 22,//行会信息
    ZhuFuYou = 23,//祝福油
    DreamLand = 24,//幻境
    AccuseChiefPanel = 25,//弹劾会长
    FamilyListHelp = 26, //家族列表帮助
    GuildBagPanel = 27, //家族仓库tips说明
    UpLevelBaoZhu = 28,//升级宝珠
    UpGradeBaoZhu = 29,//进化宝珠
    SkillslotBaoZhu = 30,//宝珠技能槽
    GemHelp = 31, //宝石
    GuildFightBoxHelp = 32,//行会争霸宝箱TIPS
    GuildFightHelp = 33,//行会争霸 沙城主奖励
    EquipCollection = 34,//装备收集
    EquipCompound = 35,//装备合成
	Help_BossKuangHuan = 36, //boss狂欢
    HonorChanllenge = 37,//荣耀挑战
    ArmRace1 = 38,//军备竞赛1
    ArmRace2 = 39,//军备竞赛2
    ArmRace3 = 40,//军备竞赛3
    ArmRace4 = 41,//军备竞赛4
    LifeTimeFund = 42, //终身基金
    Ultimate = 43,//极限挑战
    DailyActive = 44, //日常活跃度
    HunLian = 45,//魂练
	PetAwake = 46,//战魂觉醒
    SkillHelper = 47,//技能说明
    PetSkillHelper = 48,//战宠技能说明
    SignIn = 50,//签到
    WarPetRefine = 51,//宠物洗炼
    PetTalent = 52,//宠物天赋
    LongLiXiLian = 53,//龙力洗练
	MaFa = 54,//玛法通行证
    ZhangHunTianFu = 55, //宠物升级界面战魂天赋介绍
    WingSpirit = 56,//羽灵
    DailyAeana = 57,//竞技
    PetLevelUpSet = 58,//宠物升级设置
    VigorOpen = 59,//精力值解锁
}

public class UIHelpTipsPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get { return PrefabTweenType.None;}
        protected set => base.PanelTweenType = value;
    }

    public static void CreateHelpTipsPanel(HelpType eHelpType)
    {
        UIManager.Instance.CreatePanel<UIHelpTipsPanel>(panel =>
        {
            if (panel is UIHelpTipsPanel helpTipPanel)
            {
                helpTipPanel.Show((int)eHelpType);
            }
        });
    }

    #region Component
    private UILabel _lb_text = null;
    private UISprite _sp_outframe = null;
    private UIGridContainer _sp_grid = null;
    private UILabel _lb_title = null;
    private UIGridContainer _desc_grid = null;
    private UILabel _lab_DownTitle = null;
    private GameObject _obj_drag = null;
    private Transform _trans_window = null;
    private Transform _trans_view = null;
    private UILabel lb_title { get { return _lb_title ?? (_lb_title = Get<UILabel>("window/title")); } }
    private UILabel lab_DownTitle { get { return _lab_DownTitle ?? (_lab_DownTitle = Get<UILabel>("window/outframe/labDownTitle")); } }
    public UILabel lb_text { get { return _lb_text ?? (_lb_text = Get<UILabel>("view/Scroll View/lb_text")); } }
    private UISprite sp_outframe { get { return _sp_outframe ?? (_sp_outframe = Get<UISprite>("window/outframe")); } }
    private UIGridContainer sp_grid { get { return _sp_grid ?? (_sp_grid = Get<UIGridContainer>("view/Scroll View/SpriteGrid")); } }
    private UIGridContainer Desc_grid { get { return _desc_grid ?? (_desc_grid = Get<UIGridContainer>("view/Scroll View/labGrid")); } }
    private GameObject obj_drag { get { return _obj_drag ?? (_obj_drag = Get<GameObject>("view/GameObject")); } }
    private Transform trans_window { get { return _trans_window ?? (_trans_window = Get<Transform>("window")); } }
    private Transform trans_view { get { return _trans_view ?? (_trans_view = Get<Transform>("view")); } }
    #endregion

    #region member
    private int t_length = 60;  //调整上下高度，请调节数值。
    TABLE.DESCRIPTION tbl_description = null;

    //public override bool Cached { get { return true; } }

    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Tips;
        }
    }
    #endregion

    #region Funtion

    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.AddEvent(CEvent.ClosePanel, (p, q) => { UIManager.Instance.ClosePanel<UIHelpTipsPanel>(); });
    }

    public override void Show()
    {
        base.Show();
        Panel.alpha = 0;
    }

    public void Show(int id)
    {
        if (!DescriptionTableManager.Instance.TryGetValue(id, out tbl_description))
        {
            return;
        }

        if (Desc_grid)
        {
            Desc_grid.MaxCount = 0;
        }

        if (null != tbl_description)
        {
            if (null != lb_text)
            {
                lb_text.text = tbl_description.Description;
            }

            if (null != lb_title)
            {
                lb_title.text = tbl_description.Title;
            }
        }

        if (null != sp_outframe && null != lb_text)
        {
            sp_outframe.height = lb_text.height + t_length;
        }

        if (tbl_description.Section != null)
        {
            char[] IsShowIcon = tbl_description.Section.ToString().ToCharArray();
            sp_grid.MaxCount = IsShowIcon.Length;
            for (int i = 0; i < sp_grid.controlList.Count; i++)
            {
                sp_grid.controlList[i].SetActive(IsShowIcon[i] == '1');
            }
        }
        else
        {
            sp_grid.MaxCount = 0;
        }
        
        if(sp_grid.MaxCount <= 12)
            obj_drag.SetActive(false);
        sp_outframe.height = Mathf.Min(sp_outframe.height, 510);

        var viewY = 85 - (510 - sp_outframe.height)/2;
        trans_view.localPosition = new Vector3(0, viewY);
        trans_window.localPosition = new Vector3(0, viewY);

        Panel.alpha = 1;
    }

    ///暂时不适配，。，有需求 在打开适配
    /// <summary>
    /// tips的扩展
    /// </summary>
    /// <param name="sDesc">内容</param>
    /// <param name="sTitle">标题</param>
    /// <param name="sSection">每行内容标头，1为显示，0不显示，有四行就是：1111</param>
    /*public void Show(Map<string, string> desc, string sTitle = "", string sSection = null, string sDownTitle = "")
    {
        if (lb_text) lb_text.text = "";
        if (lb_title) lb_title.text = sTitle;
        if (Desc_grid)
        {
            Desc_grid.MaxCount = desc.Count;
            int index = 0;
            for (desc.Begin(); desc.Next(); index++)
            {
                Desc_grid.controlList[index].transform.Find("key").GetComponent<UILabel>().text = desc.Key;
                Desc_grid.controlList[index].transform.Find("value").GetComponent<UILabel>().text = desc.Value;
            }
        }
        lab_DownTitle.text = sDownTitle;
        lab_DownTitle.transform.localPosition = new Vector3(-5, -(Desc_grid.CellHeight * Desc_grid.MaxCount + t_length), 0);
        int labLength = sDownTitle == "" ? 0 : lab_DownTitle.height;
        if (sp_outframe) sp_outframe.height = (int)Desc_grid.CellHeight * Desc_grid.MaxCount + labLength + t_length;
        if (sp_outframe) sp_background.height = (int)Desc_grid.CellHeight * Desc_grid.MaxCount + labLength + t_length;
        if (sSection != null)
        {
            char[] IsShowIcon = sSection.ToCharArray();
            sp_grid.MaxCount = IsShowIcon.Length;
            for (int i = 0; i < sp_grid.controlList.Count; i++)
            {
                sp_grid.controlList[i].SetActive(IsShowIcon[i] == '1');
            }
        }
        else
        {
            sp_grid.MaxCount = 0;
        }

        if (Panel) Panel.alpha = 1;
    }*/

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _lb_text = null;
        _sp_outframe = null;
        _sp_grid = null;
    }
    #endregion

}
