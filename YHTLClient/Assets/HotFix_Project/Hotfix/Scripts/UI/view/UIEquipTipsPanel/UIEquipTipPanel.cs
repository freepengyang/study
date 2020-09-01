using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIEquipTipPanel : UITipsBase
{
    private GameObject _sp_Bg;
    private GameObject sp_Bg { get { return _sp_Bg ?? (_sp_Bg = Get("di").gameObject); } }
    #region right
    #region title
    //par
    GameObject _obj_rightpar;
    GameObject obj_rightpar { get { return _obj_rightpar ?? (_obj_rightpar = Get("Right").gameObject); } }
    GameObject _obj_rightparCon;
    GameObject obj_rightparCon { get { return _obj_rightparCon ?? (_obj_rightparCon = Get("Right/content").gameObject); } }
    UITable _table_content_r;
    UITable table_content_r { get { return _table_content_r ?? (_table_content_r = Get<UITable>("Right/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_content_r;
    UIWidget widge_content_r { get { return _widge_content_r ?? (_widge_content_r = Get<UIWidget>("Right/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_contentPar_r;
    UIWidget widge_contentPar_r { get { return _widge_contentPar_r ?? (_widge_contentPar_r = Get<UIWidget>("Right/content/scrollPar")); } }
    //箭头
    GameObject _r_arrow_Par;
    GameObject r_arrow_Par { get { return _r_arrow_Par ?? (_r_arrow_Par = Get("Right/content/ArrowPanel").gameObject); } }
    GameObject _r_arrow_up;
    GameObject r_arrow_up { get { return _r_arrow_up ?? (_r_arrow_up = Get("Right/content/ArrowPanel/UpArrow").gameObject); } }
    GameObject _r_arrow_down;
    GameObject r_arrow_down { get { return _r_arrow_down ?? (_r_arrow_down = Get("Right/content/ArrowPanel/DownArrow").gameObject); } }
    UIScrollBar _r_scrollBar;
    UIScrollBar r_scrollBar { get { return _r_scrollBar ?? (_r_scrollBar = Get<UIScrollBar>("Right/content/ScrollBar")); } }

    private UISprite _sp_Bg_r;
    private UISprite sp_Bg_r { get { return _sp_Bg_r ?? (_sp_Bg_r = Get<UISprite>("Right/bg")); } }
    private GameObject _obj_title_r;
    private GameObject obj_title_r { get { return _obj_title_r ?? (_obj_title_r = Get("Right/content/titlePart").gameObject); } }
    private UILabel _lb_title_r;
    private UILabel lb_title_r { get { return _lb_title_r ?? (_lb_title_r = Get<UILabel>("lb_title", obj_title_r.transform)); } }
    private UILabel _lb_Isbind_r;
    private UILabel lb_Isbind_r { get { return _lb_Isbind_r ?? (_lb_Isbind_r = Get<UILabel>("lb_Isbind", obj_title_r.transform)); } }
    private UILabel _lb_fightPower_r;
    private UILabel lb_fightPower_r { get { return _lb_fightPower_r ?? (_lb_fightPower_r = Get<UILabel>("lb_fightPower", obj_title_r.transform)); } }
    private UILabel _lb_job_r;
    private UILabel lb_job_r { get { return _lb_job_r ?? (_lb_job_r = Get<UILabel>("lb_job", obj_title_r.transform)); } }
    private UILabel _lb_lv_r;
    private UILabel lb_lv_r { get { return _lb_lv_r ?? (_lb_lv_r = Get<UILabel>("lb_lv", obj_title_r.transform)); } }
    private UILabel _lb_pos_r;
    private UILabel lb_pos_r { get { return _lb_pos_r ?? (_lb_pos_r = Get<UILabel>("lb_pos", obj_title_r.transform)); } }
    private UISprite _sp_line_r;
    private UISprite sp_line_r { get { return _sp_line_r ?? (_sp_line_r = Get<UISprite>("line", obj_title_r.transform)); } }
    private GameObject _obj_equipped_r;
    private GameObject obj_equipped_r { get { return _obj_equipped_r ?? (_obj_equipped_r = Get("sp_use", obj_title_r.transform).gameObject); } }
    private GameObject _tex_texbg_r;
    private GameObject tex_texbg_r { get { return _tex_texbg_r ?? (_tex_texbg_r = Get("texbg", obj_title_r.transform).gameObject); } }
    private GameObject _obj_arrow_r;
    private GameObject obj_arrow_r { get { return _obj_arrow_r ?? (_obj_arrow_r = Get("arrow", obj_title_r.transform).gameObject); } }
    //item
    private UISprite _sp_itemBg_r;
    private UISprite sp_itemBg_r { get { return _sp_itemBg_r ?? (_sp_itemBg_r = Get<UISprite>("sp_itemBg", obj_title_r.transform)); } }
    private UISprite _sp_icon_r;
    private UISprite sp_icon_r { get { return _sp_icon_r ?? (_sp_icon_r = Get<UISprite>("sp_itemBg/sp_icon", obj_title_r.transform)); } }
    private UILabel _lb_enhance_r;
    private UILabel lb_enhance_r { get { return _lb_enhance_r ?? (_lb_enhance_r = Get<UILabel>("sp_itemBg/lb_lv", obj_title_r.transform)); } }
    private UIScrollView _sc_scroll_r;
    private UIScrollView sc_scroll_r { get { return _sc_scroll_r ?? (_sc_scroll_r = Get<UIScrollView>("content/scrollPar/Scroll", obj_rightpar.transform)); } }
    UISprite _obj_wolongSeal_r;
    UISprite obj_wolongSeal_r { get { return _obj_wolongSeal_r ?? (_obj_wolongSeal_r = Get<UISprite>("sp_itemBg/sp_dragon", obj_title_r.transform)); } }
    UILabel _lb_normalSuit_r;
    UILabel lb_normalSuit_r { get { return _lb_normalSuit_r ?? (_lb_normalSuit_r = Get<UILabel>("sp_itemBg/lb_suit", obj_title_r.transform)); } }
    #endregion
    //基础属性
    private GameObject _obj_basic_r;
    private GameObject obj_basic_r { get { return _obj_basic_r ?? (_obj_basic_r = Get("content/scrollPar/Scroll/Table/basicInfo", obj_rightpar.transform).gameObject); } }
    private UIGridContainer _grid_basicGrid_r;
    private UIGridContainer grid_basicGrid_r { get { return _grid_basicGrid_r ?? (_grid_basicGrid_r = Get<UIGridContainer>("despGrid", obj_basic_r.transform)); } }
    //普通装备元魂
    GameObject _r_yuanhun;
    GameObject r_yuanhun { get { return _r_yuanhun ?? (_r_yuanhun = Get("content/scrollPar/Scroll/Table/equipYuanHun", obj_rightpar.transform).gameObject); } }
    UIGridContainer _r_grid_yuanhun;
    UIGridContainer r_grid_yuanhun { get { return _r_grid_yuanhun ?? (_r_grid_yuanhun = Get<UIGridContainer>("despGrid", r_yuanhun.transform)); } }
    //随机属性
    private GameObject _obj_random_r;
    private GameObject obj_random_r { get { return _obj_random_r ?? (_obj_random_r = Get("content/scrollPar/Scroll/Table/randomInfo", obj_rightpar.transform).gameObject); } }

    private UIGridContainer _grid_randomGrid_r;
    private UIGridContainer grid_randomGrid_r { get { return _grid_randomGrid_r ?? (_grid_randomGrid_r = Get<UIGridContainer>("despGrid", obj_random_r.transform)); } }
    //装备技能
    private GameObject _obj_skill_r;
    private GameObject obj_skill_r { get { return _obj_skill_r ?? (_obj_skill_r = Get("content/scrollPar/Scroll/Table/equipSkill", obj_rightpar.transform).gameObject); } }
    private UIGridContainer _grid_skillGrid_r;
    private UIGridContainer grid_skillGrid_r { get { return _grid_skillGrid_r ?? (_grid_skillGrid_r = Get<UIGridContainer>("despGrid", obj_skill_r.transform)); } }
    //期望随机属性
    GameObject _obj_exceptedRandomAttr;
    GameObject obj_exceptedRandomAttr { get { return _obj_exceptedRandomAttr ?? (_obj_exceptedRandomAttr = Get("content/scrollPar/Scroll/Table/exceptedrandomInfo", obj_rightpar.transform).gameObject); } }
    UILabel _lb_exceptedRandomAttr;
    UILabel lb_exceptedRandomAttr { get { return _lb_exceptedRandomAttr ?? (_lb_exceptedRandomAttr = Get<UILabel>("key", obj_exceptedRandomAttr.transform)); } }
    //卧龙套装属性
    GameObject _obj_wolongPart_r;
    GameObject obj_wolongPart_r { get { return _obj_wolongPart_r ?? (_obj_wolongPart_r = Get("content/scrollPar/Scroll/Table/wolongSuit", obj_rightpar.transform).gameObject); } }
    UILabel _lb_wolongName_r;
    UILabel lb_wolongName_r { get { return _lb_wolongName_r ?? (_lb_wolongName_r = Get<UILabel>("des/key", obj_wolongPart_r.transform)); } }
    UILabel _lb_wolongPro_r;
    UILabel lb_wolongPro_r { get { return _lb_wolongPro_r ?? (_lb_wolongPro_r = Get<UILabel>("des/value", obj_wolongPart_r.transform)); } }
    UILabel _lb_wolongdes1_r;
    UILabel lb_wolongdes1_r { get { return _lb_wolongdes1_r ?? (_lb_wolongdes1_r = Get<UILabel>("des1/value", obj_wolongPart_r.transform)); } }
    UILabel _lb_wolongdes2_r;
    UILabel lb_wolongdes2_r { get { return _lb_wolongdes2_r ?? (_lb_wolongdes2_r = Get<UILabel>("des2/value", obj_wolongPart_r.transform)); } }
    //卧龙龙魂属性
    GameObject _r_wolonglonghun;
    GameObject r_wolonglonghun { get { return _r_wolonglonghun ?? (_r_wolonglonghun = Get("content/scrollPar/Scroll/Table/WLlonghun", obj_rightpar.transform).gameObject); } }
    UIGridContainer _r_wolonglonghunlGrid;
    UIGridContainer r_wolonglonghunlGrid { get { return _r_wolonglonghunlGrid ?? (_r_wolonglonghunlGrid = Get<UIGridContainer>("despGrid", r_wolonglonghun.transform)); } }
    UILabel _r_wolongLonghunDes;
    UILabel r_wolongLonghunDes { get { return _r_wolongLonghunDes ?? (_r_wolongLonghunDes = Get<UILabel>("des", r_wolonglonghun.transform)); } }
    //卧龙随机技能
    GameObject _r_wolongRandomSkill;
    GameObject r_wolongRandomSkill { get { return _r_wolongRandomSkill ?? (_r_wolongRandomSkill = Get("content/scrollPar/Scroll/Table/WlequipSkill", obj_rightpar.transform).gameObject); } }
    UIGridContainer _r_wolongRandomSkillGrid;
    UIGridContainer r_wolongRandomSkillGrid { get { return _r_wolongRandomSkillGrid ?? (_r_wolongRandomSkillGrid = Get<UIGridContainer>("despGrid", r_wolongRandomSkill.transform)); } }
    //卧龙装备龙力
    GameObject _r_longLi;
    GameObject r_longLi { get { return _r_longLi ?? (_r_longLi = Get("content/scrollPar/Scroll/Table/LongLiAttr", obj_rightpar.transform).gameObject); } }
    UILabel _r_noWolongHun;
    UILabel r_noWolongHun { get { return _r_noWolongHun ?? (_r_noWolongHun = Get<UILabel>("des", r_longLi.transform)); } }
    UIGridContainer _r_grid_longliBase;
    UIGridContainer r_grid_longliBase { get { return _r_grid_longliBase ?? (_r_grid_longliBase = Get<UIGridContainer>("baseAffixGrid", r_longLi.transform)); } }
    UIGridContainer _r_grid_longliInten;
    UIGridContainer r_grid_longliInten { get { return _r_grid_longliInten ?? (_r_grid_longliInten = Get<UIGridContainer>("despGrid", r_longLi.transform)); } }
    //卧龙装备龙力（强化词缀）
    GameObject _r_longLiinten;
    GameObject r_longLiinten { get { return _r_longLiinten ?? (_r_longLiinten = Get("content/scrollPar/Scroll/Table/LongLiInten", obj_rightpar.transform).gameObject); } }
    GameObject _r_longLiintenItem;
    GameObject r_longLiintenItem { get { return _r_longLiintenItem ?? (_r_longLiintenItem = Get("item", r_longLiinten.transform).gameObject); } }
    UITable _r_table_longjiInten;
    UITable r_table_longjiInten { get { return _r_table_longjiInten ?? (_r_table_longjiInten = Get<UITable>("grid", r_longLiinten.transform)); } }
    //系统回收奖励
    GameObject _r_recycle;
    GameObject r_recycle { get { return _r_recycle ?? (_r_recycle = Get("content/scrollPar/recycle", obj_rightpar.transform).gameObject); } }
    GameObject _r_recyclePar;
    GameObject r_recyclePar { get { return _r_recyclePar ?? (_r_recyclePar = Get("content/scrollPar/recycle/par", obj_rightpar.transform).gameObject); } }
    UILabel _r_recycledes;
    UILabel r_recycledes { get { return _r_recycledes ?? (_r_recycledes = Get<UILabel>("recycleDes/recycleNum", r_recyclePar.transform)); } }
    UILabel _r_recycleDonate;
    UILabel r_recycleDonate { get { return _r_recycleDonate ?? (_r_recycleDonate = Get<UILabel>("donateDes/recycleNum", r_recyclePar.transform)); } }

    UIGridContainer _r_grid_linePar;
    UIGridContainer r_grid_linePar { get { return _r_grid_linePar ?? (_r_grid_linePar = Get<UIGridContainer>("Right/content/scrollPar/Scroll/lineParent")); } }
    //侧边按钮
    UIGridContainer _grid_btnsGrid_r;
    UIGridContainer grid_btnsGrid_r { get { return _grid_btnsGrid_r ?? (_grid_btnsGrid_r = Get<UIGridContainer>("content/Btns/SubBtns", obj_rightpar.transform)); } }
    TweenPosition _moveTween;
    TweenPosition moveTween { get { return _moveTween ?? (_moveTween = Get<TweenPosition>("Right")); } }
    #endregion

    #region  Middle
    #region title
    private GameObject _obj_middlepar;
    private GameObject obj_middlepar { get { return _obj_middlepar ?? (_obj_middlepar = Get("Middle").gameObject); } }
    GameObject _obj_middleparCon;
    GameObject obj_middleparCon { get { return _obj_middleparCon ?? (_obj_middleparCon = Get("Middle/content").gameObject); } }
    UITable _table_content_m;
    UITable table_content_m { get { return _table_content_m ?? (_table_content_m = Get<UITable>("Middle/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_content_m;
    UIWidget widge_content_m { get { return _widge_content_m ?? (_widge_content_m = Get<UIWidget>("Middle/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_contentPar_m;
    UIWidget widge_contentPar_m { get { return _widge_contentPar_m ?? (_widge_contentPar_m = Get<UIWidget>("Middle/content/scrollPar")); } }
    //箭头
    GameObject _m_arrow_Par;
    GameObject m_arrow_Par { get { return _m_arrow_Par ?? (_m_arrow_Par = Get("Middle/content/ArrowPanel").gameObject); } }
    GameObject _m_arrow_up;
    GameObject m_arrow_up { get { return _m_arrow_up ?? (_m_arrow_up = Get("Middle/content/ArrowPanel/UpArrow").gameObject); } }
    GameObject _m_arrow_down;
    GameObject m_arrow_down { get { return _m_arrow_down ?? (_m_arrow_down = Get("Middle/content/ArrowPanel/DownArrow").gameObject); } }
    UIScrollBar _m_scrollBar;
    UIScrollBar m_scrollBar { get { return _m_scrollBar ?? (_m_scrollBar = Get<UIScrollBar>("Middle/content/ScrollBar")); } }
    private UISprite _sp_Bg_m;
    private UISprite sp_Bg_m { get { return _sp_Bg_m ?? (_sp_Bg_m = Get<UISprite>("Middle/bg")); } }
    private GameObject _obj_title_m;
    private GameObject obj_title_m { get { return _obj_title_m ?? (_obj_title_m = Get("Middle/content/titlePart").gameObject); } }
    private UILabel _lb_title_m;
    private UILabel lb_title_m { get { return _lb_title_m ?? (_lb_title_m = Get<UILabel>("lb_title", obj_title_m.transform)); } }
    private UILabel _lb_Isbind_m;
    private UILabel lb_Isbind_m { get { return _lb_Isbind_m ?? (_lb_Isbind_m = Get<UILabel>("lb_Isbind", obj_title_m.transform)); } }
    private UILabel _lb_fightPower_m;
    private UILabel lb_fightPower_m { get { return _lb_fightPower_m ?? (_lb_fightPower_m = Get<UILabel>("lb_fightPower", obj_title_m.transform)); } }
    private UILabel _lb_job_m;
    private UILabel lb_job_m { get { return _lb_job_m ?? (_lb_job_m = Get<UILabel>("lb_job", obj_title_m.transform)); } }
    private UILabel _lb_lv_m;
    private UILabel lb_lv_m { get { return _lb_lv_m ?? (_lb_lv_m = Get<UILabel>("lb_lv", obj_title_m.transform)); } }
    private UILabel _lb_pos_m;
    private UILabel lb_pos_m { get { return _lb_pos_m ?? (_lb_pos_m = Get<UILabel>("lb_pos", obj_title_m.transform)); } }
    private UISprite _sp_line_m;
    private UISprite sp_line_m { get { return _sp_line_m ?? (_sp_line_m = Get<UISprite>("line", obj_title_m.transform)); } }
    private GameObject _obj_equipped_m;
    private GameObject obj_equipped_m { get { return _obj_equipped_m ?? (_obj_equipped_m = Get("sp_use", obj_title_m.transform).gameObject); } }
    private GameObject _tex_texbg_m;
    private GameObject tex_texbg_m { get { return _tex_texbg_m ?? (_tex_texbg_m = Get("texbg", obj_title_m.transform).gameObject); } }
    private GameObject _obj_arrow_m;
    private GameObject obj_arrow_m { get { return _obj_arrow_m ?? (_obj_arrow_m = Get("arrow", obj_title_m.transform).gameObject); } }
    //item
    private UISprite _sp_itemBg_m;
    private UISprite sp_itemBg_m { get { return _sp_itemBg_m ?? (_sp_itemBg_m = Get<UISprite>("sp_itemBg", obj_title_m.transform)); } }
    private UISprite _sp_icon_m;
    private UISprite sp_icon_m { get { return _sp_icon_m ?? (_sp_icon_m = Get<UISprite>("sp_itemBg/sp_icon", obj_title_m.transform)); } }
    private UILabel _lb_enhance_m;
    private UILabel lb_enhance_m { get { return _lb_enhance_m ?? (_lb_enhance_m = Get<UILabel>("sp_itemBg/lb_lv", obj_title_m.transform)); } }
    private UIScrollView _sc_scroll_m;
    private UIScrollView sc_scroll_m { get { return _sc_scroll_m ?? (_sc_scroll_m = Get<UIScrollView>("content/scrollPar/Scroll", obj_middlepar.transform)); } }
    UISprite _obj_wolongSeal_m;
    UISprite obj_wolongSeal_m { get { return _obj_wolongSeal_m ?? (_obj_wolongSeal_m = Get<UISprite>("sp_itemBg/sp_dragon", obj_title_m.transform)); } }
    UILabel _lb_normalSuit_m;
    UILabel lb_normalSuit_m { get { return _lb_normalSuit_m ?? (_lb_normalSuit_m = Get<UILabel>("sp_itemBg/lb_suit", obj_title_m.transform)); } }
    #endregion
    //基础属性
    private GameObject _obj_basic_m;
    private GameObject obj_basic_m { get { return _obj_basic_m ?? (_obj_basic_m = Get("content/scrollPar/Scroll/Table/basicInfo", obj_middlepar.transform).gameObject); } }
    private UIGridContainer _grid_basicGrid_m;
    private UIGridContainer grid_basicGrid_m { get { return _grid_basicGrid_m ?? (_grid_basicGrid_m = Get<UIGridContainer>("despGrid", obj_basic_m.transform)); } }
    //普通装备元魂
    GameObject _m_yuanhun;
    GameObject m_yuanhun { get { return _m_yuanhun ?? (_m_yuanhun = Get("content/scrollPar/Scroll/Table/equipYuanHun", obj_middlepar.transform).gameObject); } }
    UIGridContainer _m_grid_yuanhun;
    UIGridContainer m_grid_yuanhun { get { return _m_grid_yuanhun ?? (_m_grid_yuanhun = Get<UIGridContainer>("despGrid", m_yuanhun.transform)); } }
    //随机属性
    private GameObject _obj_mandom_m;
    private GameObject obj_mandom_m { get { return _obj_mandom_m ?? (_obj_mandom_m = Get("content/scrollPar/Scroll/Table/randomInfo", obj_middlepar.transform).gameObject); } }

    private UIGridContainer _grid_mandomGrid_m;
    private UIGridContainer grid_mandomGrid_m { get { return _grid_mandomGrid_m ?? (_grid_mandomGrid_m = Get<UIGridContainer>("despGrid", obj_mandom_m.transform)); } }
    //装备技能
    private GameObject _obj_skill_m;
    private GameObject obj_skill_m { get { return _obj_skill_m ?? (_obj_skill_m = Get("content/scrollPar/Scroll/Table/equipSkill", obj_middlepar.transform).gameObject); } }
    private UIGridContainer _grid_skillGrid_m;
    private UIGridContainer grid_skillGrid_m { get { return _grid_skillGrid_m ?? (_grid_skillGrid_m = Get<UIGridContainer>("despGrid", obj_skill_m.transform)); } }
    //卧龙套装属性
    GameObject _obj_wolongPart_m;
    GameObject obj_wolongPart_m { get { return _obj_wolongPart_m ?? (_obj_wolongPart_m = Get("content/scrollPar/Scroll/Table/wolongSuit", obj_middlepar.transform).gameObject); } }
    UILabel _lb_wolongName_m;
    UILabel lb_wolongName_m { get { return _lb_wolongName_m ?? (_lb_wolongName_m = Get<UILabel>("des/key", obj_wolongPart_m.transform)); } }
    UILabel _lb_wolongPro_m;
    UILabel lb_wolongPro_m { get { return _lb_wolongPro_m ?? (_lb_wolongPro_m = Get<UILabel>("des/value", obj_wolongPart_m.transform)); } }
    UILabel _lb_wolongdes1_m;
    UILabel lb_wolongdes1_m { get { return _lb_wolongdes1_m ?? (_lb_wolongdes1_m = Get<UILabel>("des1/value", obj_wolongPart_m.transform)); } }
    UILabel _lb_wolongdes2_m;
    UILabel lb_wolongdes2_m { get { return _lb_wolongdes2_m ?? (_lb_wolongdes2_m = Get<UILabel>("des2/value", obj_wolongPart_m.transform)); } }
    //卧龙龙魂属性
    GameObject _m_wolonglonghun;
    GameObject m_wolonglonghun { get { return _m_wolonglonghun ?? (_m_wolonglonghun = Get("content/scrollPar/Scroll/Table/WLlonghun", obj_middlepar.transform).gameObject); } }
    UIGridContainer _m_wolonglonghunlGrid;
    UIGridContainer m_wolonglonghunlGrid { get { return _m_wolonglonghunlGrid ?? (_m_wolonglonghunlGrid = Get<UIGridContainer>("despGrid", m_wolonglonghun.transform)); } }
    UILabel _m_wolongLonghunDes;
    UILabel m_wolongLonghunDes { get { return _m_wolongLonghunDes ?? (_m_wolongLonghunDes = Get<UILabel>("des", m_wolonglonghun.transform)); } }
    //卧龙随机技能
    GameObject _m_wolongRandomSkill;
    GameObject m_wolongRandomSkill { get { return _m_wolongRandomSkill ?? (_m_wolongRandomSkill = Get("content/scrollPar/Scroll/Table/WlequipSkill", obj_middlepar.transform).gameObject); } }
    UIGridContainer _m_wolongRandomSkillGrid;
    UIGridContainer m_wolongRandomSkillGrid { get { return _m_wolongRandomSkillGrid ?? (_m_wolongRandomSkillGrid = Get<UIGridContainer>("despGrid", m_wolongRandomSkill.transform)); } }
    //卧龙装备龙力
    GameObject _m_longLi;
    GameObject m_longLi { get { return _m_longLi ?? (_m_longLi = Get("content/scrollPar/Scroll/Table/LongLiAttr", obj_middlepar.transform).gameObject); } }
    UIGridContainer _m_grid_longliBase;
    UIGridContainer m_grid_longliBase { get { return _m_grid_longliBase ?? (_m_grid_longliBase = Get<UIGridContainer>("baseAffixGrid", m_longLi.transform)); } }
    UIGridContainer _m_grid_longliInten;
    UIGridContainer m_grid_longliInten { get { return _m_grid_longliInten ?? (_m_grid_longliInten = Get<UIGridContainer>("despGrid", m_longLi.transform)); } }
    //卧龙装备龙力（强化词缀）
    GameObject _m_longLiinten;
    GameObject m_longLiinten { get { return _m_longLiinten ?? (_m_longLiinten = Get("content/scrollPar/Scroll/Table/LongLiInten", obj_middlepar.transform).gameObject); } }
    GameObject _m_longLiintenItem;
    GameObject m_longLiintenItem { get { return _m_longLiintenItem ?? (_m_longLiintenItem = Get("item", m_longLiinten.transform).gameObject); } }
    UITable _m_table_longjiInten;
    UITable m_table_longjiInten { get { return _m_table_longjiInten ?? (_m_table_longjiInten = Get<UITable>("grid", m_longLiinten.transform)); } }
    //系统回收奖励
    GameObject _m_recycle;
    GameObject m_recycle { get { return _m_recycle ?? (_m_recycle = Get("content/scrollPar/recycle", obj_middlepar.transform).gameObject); } }
    GameObject _m_recyclePar;
    GameObject m_recyclePar { get { return _m_recyclePar ?? (_m_recyclePar = Get("content/scrollPar/recycle/par", obj_middlepar.transform).gameObject); } }
    UILabel _m_recycledes;
    UILabel m_recycledes { get { return _m_recycledes ?? (_m_recycledes = Get<UILabel>("recycleDes/recycleNum", m_recyclePar.transform)); } }
    UILabel _m_recycleDonate;
    UILabel m_recycleDonate { get { return _m_recycleDonate ?? (_m_recycleDonate = Get<UILabel>("donateDes/recycleNum", m_recyclePar.transform)); } }

    UIGridContainer _m_grid_linePar;
    UIGridContainer m_grid_linePar { get { return _m_grid_linePar ?? (_m_grid_linePar = Get<UIGridContainer>("Middle/content/scrollPar/Scroll/lineParent")); } }
    #endregion

    #region Left
    #region title
    private GameObject _obj_leftpar;
    private GameObject obj_leftpar { get { return _obj_leftpar ?? (_obj_leftpar = Get("Left").gameObject); } }
    GameObject _obj_leftparCon;
    GameObject obj_leftparCon { get { return _obj_leftparCon ?? (_obj_leftparCon = Get("Left/content").gameObject); } }
    UITable _table_content_l;
    UITable table_content_l { get { return _table_content_l ?? (_table_content_l = Get<UITable>("Left/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_content_l;
    UIWidget widge_content_l { get { return _widge_content_l ?? (_widge_content_l = Get<UIWidget>("Left/content/scrollPar/Scroll/Table")); } }
    UIWidget _widge_contentPar_l;
    UIWidget widge_contentPar_l { get { return _widge_contentPar_l ?? (_widge_contentPar_l = Get<UIWidget>("Left/content/scrollPar")); } }
    //箭头
    GameObject _l_arrow_Par;
    GameObject l_arrow_Par { get { return _l_arrow_Par ?? (_l_arrow_Par = Get("Left/content/ArrowPanel").gameObject); } }
    GameObject _l_arrow_up;
    GameObject l_arrow_up { get { return _l_arrow_up ?? (_l_arrow_up = Get("Left/content/ArrowPanel/UpArrow").gameObject); } }
    GameObject _l_arrow_down;
    GameObject l_arrow_down { get { return _l_arrow_down ?? (_l_arrow_down = Get("Left/content/ArrowPanel/DownArrow").gameObject); } }
    UIScrollBar _l_scrollBar;
    UIScrollBar l_scrollBar { get { return _l_scrollBar ?? (_l_scrollBar = Get<UIScrollBar>("Left/content/ScrollBar")); } }
    private UISprite _sp_Bg_l;
    private UISprite sp_Bg_l { get { return _sp_Bg_l ?? (_sp_Bg_l = Get<UISprite>("Left/bg")); } }
    private GameObject _obj_title_l;
    private GameObject obj_title_l { get { return _obj_title_l ?? (_obj_title_l = Get("Left/content/titlePart").gameObject); } }
    private UILabel _lb_title_l;
    private UILabel lb_title_l { get { return _lb_title_l ?? (_lb_title_l = Get<UILabel>("lb_title", obj_title_l.transform)); } }
    private UILabel _lb_Isbind_l;
    private UILabel lb_Isbind_l { get { return _lb_Isbind_l ?? (_lb_Isbind_l = Get<UILabel>("lb_Isbind", obj_title_l.transform)); } }
    private UILabel _lb_fightPower_l;
    private UILabel lb_fightPower_l { get { return _lb_fightPower_l ?? (_lb_fightPower_l = Get<UILabel>("lb_fightPower", obj_title_l.transform)); } }
    private UILabel _lb_job_l;
    private UILabel lb_job_l { get { return _lb_job_l ?? (_lb_job_l = Get<UILabel>("lb_job", obj_title_l.transform)); } }
    private UILabel _lb_lv_l;
    private UILabel lb_lv_l { get { return _lb_lv_l ?? (_lb_lv_l = Get<UILabel>("lb_lv", obj_title_l.transform)); } }
    private UILabel _lb_pos_l;
    private UILabel lb_pos_l { get { return _lb_pos_l ?? (_lb_pos_l = Get<UILabel>("lb_pos", obj_title_l.transform)); } }
    private UISprite _sp_line_l;
    private UISprite sp_line_l { get { return _sp_line_l ?? (_sp_line_l = Get<UISprite>("line", obj_title_l.transform)); } }
    private GameObject _obj_equipped_l;
    private GameObject obj_equipped_l { get { return _obj_equipped_l ?? (_obj_equipped_l = Get("sp_use", obj_title_l.transform).gameObject); } }
    private GameObject _tex_texbg_l;
    private GameObject tex_texbg_l { get { return _tex_texbg_l ?? (_tex_texbg_l = Get("texbg", obj_title_l.transform).gameObject); } }
    private GameObject _obj_arrow_l;
    private GameObject obj_arrow_l { get { return _obj_arrow_l ?? (_obj_arrow_l = Get("arrow", obj_title_l.transform).gameObject); } }
    //item
    private UISprite _sp_itemBg_l;
    private UISprite sp_itemBg_l { get { return _sp_itemBg_l ?? (_sp_itemBg_l = Get<UISprite>("sp_itemBg", obj_title_l.transform)); } }
    private UISprite _sp_icon_l;
    private UISprite sp_icon_l { get { return _sp_icon_l ?? (_sp_icon_l = Get<UISprite>("sp_itemBg/sp_icon", obj_title_l.transform)); } }
    private UILabel _lb_enhance_l;
    private UILabel lb_enhance_l { get { return _lb_enhance_l ?? (_lb_enhance_l = Get<UILabel>("sp_itemBg/lb_lv", obj_title_l.transform)); } }
    private UIScrollView _sc_scroll_l;
    private UIScrollView sc_scroll_l { get { return _sc_scroll_l ?? (_sc_scroll_l = Get<UIScrollView>("content/scrollPar/Scroll", obj_leftpar.transform)); } }
    UISprite _obj_wolongSeal_l;
    UISprite obj_wolongSeal_l { get { return _obj_wolongSeal_l ?? (_obj_wolongSeal_l = Get<UISprite>("sp_itemBg/sp_dragon", obj_title_l.transform)); } }
    UILabel _lb_normalSuit_l;
    UILabel lb_normalSuit_l { get { return _lb_normalSuit_l ?? (_lb_normalSuit_l = Get<UILabel>("sp_itemBg/lb_suit", obj_title_l.transform)); } }
    #endregion
    //基础属性
    private GameObject _obj_basic_l;
    private GameObject obj_basic_l { get { return _obj_basic_l ?? (_obj_basic_l = Get("content/scrollPar/Scroll/Table/basicInfo", obj_leftpar.transform).gameObject); } }
    private UIGridContainer _grid_basicGrid_l;
    private UIGridContainer grid_basicGrid_l { get { return _grid_basicGrid_l ?? (_grid_basicGrid_l = Get<UIGridContainer>("despGrid", obj_basic_l.transform)); } }
    //普通装备元魂
    GameObject _l_yuanhun;
    GameObject l_yuanhun { get { return _l_yuanhun ?? (_l_yuanhun = Get("content/scrollPar/Scroll/Table/equipYuanHun", obj_leftpar.transform).gameObject); } }
    UIGridContainer _l_grid_yuanhun;
    UIGridContainer l_grid_yuanhun { get { return _l_grid_yuanhun ?? (_l_grid_yuanhun = Get<UIGridContainer>("despGrid", l_yuanhun.transform)); } }
    //随机属性
    private GameObject _obj_landom_l;
    private GameObject obj_landom_l { get { return _obj_landom_l ?? (_obj_landom_l = Get("content/scrollPar/Scroll/Table/randomInfo", obj_leftpar.transform).gameObject); } }

    private UIGridContainer _grid_landomGrid_l;
    private UIGridContainer grid_landomGrid_l { get { return _grid_landomGrid_l ?? (_grid_landomGrid_l = Get<UIGridContainer>("despGrid", obj_landom_l.transform)); } }
    //装备技能
    private GameObject _obj_skill_l;
    private GameObject obj_skill_l { get { return _obj_skill_l ?? (_obj_skill_l = Get("content/scrollPar/Scroll/Table/equipSkill", obj_leftpar.transform).gameObject); } }
    private UIGridContainer _grid_skillGrid_l;
    private UIGridContainer grid_skillGrid_l { get { return _grid_skillGrid_l ?? (_grid_skillGrid_l = Get<UIGridContainer>("despGrid", obj_skill_l.transform)); } }
    //卧龙套装属性
    GameObject _obj_wolongPart_l;
    GameObject obj_wolongPart_l { get { return _obj_wolongPart_l ?? (_obj_wolongPart_l = Get("content/scrollPar/Scroll/Table/wolongSuit", obj_leftpar.transform).gameObject); } }
    UILabel _lb_wolongName_l;
    UILabel lb_wolongName_l { get { return _lb_wolongName_l ?? (_lb_wolongName_l = Get<UILabel>("des/key", obj_wolongPart_l.transform)); } }
    UILabel _lb_wolongPro_l;
    UILabel lb_wolongPro_l { get { return _lb_wolongPro_l ?? (_lb_wolongPro_l = Get<UILabel>("des/value", obj_wolongPart_l.transform)); } }
    UILabel _lb_wolongdes1_l;
    UILabel lb_wolongdes1_l { get { return _lb_wolongdes1_l ?? (_lb_wolongdes1_l = Get<UILabel>("des1/value", obj_wolongPart_l.transform)); } }
    UILabel _lb_wolongdes2_l;
    UILabel lb_wolongdes2_l { get { return _lb_wolongdes2_l ?? (_lb_wolongdes2_l = Get<UILabel>("des2/value", obj_wolongPart_l.transform)); } }
    //卧龙龙魂属性
    GameObject _l_wolonglonghun;
    GameObject l_wolonglonghun { get { return _l_wolonglonghun ?? (_l_wolonglonghun = Get("content/scrollPar/Scroll/Table/WLlonghun", obj_leftpar.transform).gameObject); } }
    UIGridContainer _l_wolonglonghunlGrid;
    UIGridContainer l_wolonglonghunlGrid { get { return _l_wolonglonghunlGrid ?? (_l_wolonglonghunlGrid = Get<UIGridContainer>("despGrid", l_wolonglonghun.transform)); } }
    UILabel _l_wolongLonghunDes;
    UILabel l_wolongLonghunDes { get { return _l_wolongLonghunDes ?? (_l_wolongLonghunDes = Get<UILabel>("des", l_wolonglonghun.transform)); } }
    //卧龙随机技能
    GameObject _l_wolongRandomSkill;
    GameObject l_wolongRandomSkill { get { return _l_wolongRandomSkill ?? (_l_wolongRandomSkill = Get("content/scrollPar/Scroll/Table/WlequipSkill", obj_leftpar.transform).gameObject); } }
    UIGridContainer _l_wolongRandomSkillGrid;
    UIGridContainer l_wolongRandomSkillGrid { get { return _l_wolongRandomSkillGrid ?? (_l_wolongRandomSkillGrid = Get<UIGridContainer>("despGrid", l_wolongRandomSkill.transform)); } }
    //卧龙装备龙力
    GameObject _l_longLi;
    GameObject l_longLi { get { return _l_longLi ?? (_l_longLi = Get("content/scrollPar/Scroll/Table/LongLiAttr", obj_leftpar.transform).gameObject); } }
    UIGridContainer _l_grid_longliBase;
    UIGridContainer l_grid_longliBase { get { return _l_grid_longliBase ?? (_l_grid_longliBase = Get<UIGridContainer>("baseAffixGrid", l_longLi.transform)); } }
    UIGridContainer _l_grid_longliInten;
    UIGridContainer l_grid_longliInten { get { return _l_grid_longliInten ?? (_l_grid_longliInten = Get<UIGridContainer>("despGrid", l_longLi.transform)); } }
    //卧龙装备龙力（强化词缀）
    GameObject _l_longLiinten;
    GameObject l_longLiinten { get { return _l_longLiinten ?? (_l_longLiinten = Get("content/scrollPar/Scroll/Table/LongLiInten", obj_leftpar.transform).gameObject); } }
    GameObject _l_longLiintenItem;
    GameObject l_longLiintenItem { get { return _l_longLiintenItem ?? (_l_longLiintenItem = Get("item", l_longLiinten.transform).gameObject); } }
    UITable _l_table_longjiInten;
    UITable l_table_longjiInten { get { return _l_table_longjiInten ?? (_l_table_longjiInten = Get<UITable>("grid", l_longLiinten.transform)); } }
    //系统回收奖励
    GameObject _l_recycle;
    GameObject l_recycle { get { return _l_recycle ?? (_l_recycle = Get("content/scrollPar/recycle", obj_leftpar.transform).gameObject); } }
    GameObject _l_recyclePar;
    GameObject l_recyclePar { get { return _l_recyclePar ?? (_l_recyclePar = Get("content/scrollPar/recycle/par", obj_leftpar.transform).gameObject); } }
    UILabel _l_recycledes;
    UILabel l_recycledes { get { return _l_recycledes ?? (_l_recycledes = Get<UILabel>("recycleDes/recycleNum", l_recyclePar.transform)); } }
    UILabel _l_recycleDonate;
    UILabel l_recycleDonate { get { return _l_recycleDonate ?? (_l_recycleDonate = Get<UILabel>("donateDes/recycleNum", l_recyclePar.transform)); } }

    UIGridContainer _l_grid_linePar;
    UIGridContainer l_grid_linePar { get { return _l_grid_linePar ?? (_l_grid_linePar = Get<UIGridContainer>("Left/content/scrollPar/Scroll/lineParent")); } }
    #endregion 

    EqiupTipData data;
    UIPanel scroll_R;
    UIPanel scroll_M;
    UIPanel scroll_L;
    private TipsOpenType openType;
    List<TipsBtnData> btnsName = new List<TipsBtnData>();
    public object ex_data;
    bool positionReset = false;
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    int weight = 332;
    int totalHeight = 620;
    int scrollHeight = 420;
    List<bag.EquipInfo> samePosEquiplist = new List<bag.EquipInfo>();
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.CloseTips, GetCloseTipsEvent);
        mClientEvent.Reg((uint)CEvent.OpenZhuFuYou, GetOpenZhuFuYou);
        EventDelegate.Add(moveTween.onFinished, OpenEquipZhuFuPanel);
        mClientEvent.Reg((uint)CEvent.EquipRebuildNtfMessage, GetRecastBack);
        EventDelegate.Add(r_scrollBar.onChange, RightArrowChange);
        EventDelegate.Add(m_scrollBar.onChange, MiddleArrowChange);
        EventDelegate.Add(l_scrollBar.onChange, LeftArrowChange);
    }
    void GetOpenZhuFuYou(uint id, object data)
    {
        MoveAndShowZhuFu();
    }
    public void MoveAndShowZhuFu()
    {
        obj_middlepar.SetActive(false);
        obj_leftpar.SetActive(false);
        grid_btnsGrid_r.gameObject.SetActive(false);
        moveTween.enabled = true;
        positionReset = true;
    }
    private void OpenEquipZhuFuPanel()
    {
        UIManager.Instance.CreatePanel<UIEquipZhuFuPanel>(p =>
        {
            (p as UIEquipZhuFuPanel).RefreshInfo(data.r_itemInfo);

        });
    }
    private void GetRecastBack(uint id, object _info)
    {
        if (_info == null) return;
        bag.EquipRebuildNtf msg = (bag.EquipRebuildNtf)_info;
        StruectureData(openType, data.r_itemCfg, msg.equip.equip);
        RefreshBasic(grid_basicGrid_r, data.r_basicInfo);
        StructBtnData(openType, data.r_itemCfg, msg.equip.equip);
        RefreshRightPart();
        ScriptBinder.StartCoroutine(ChangePos());
    }
    CSBetterLisHot<float> maxY = new CSBetterLisHot<float>();

    public override void ShowTip(TipsOpenType _type, TABLE.ITEM _cfg, bag.BagItemInfo _info = null, object data = null, System.Action _action = null)
    {
        ex_data = data;
        openType = _type;
        UIEventListener.Get(sp_Bg).onClick = p => { UIManager.Instance.ClosePanel(this.GetType()); };
        base.ShowTip(_type, _cfg, _info);
        StruectureData(_type, _cfg, _info);
        StructBtnData(_type, _cfg, _info);
        RefreshRightPart();
        RefreshMiddlePart();
        RefreshLeftPart();
        if (_action != null)
        {
            _action?.Invoke();
        }
        ScriptBinder.StartCoroutine(ChangePos());
    }

    IEnumerator ChangePos()
    {
        yield return null;
        maxY.Clear();
        maxY.Add(obj_rightparCon.transform.localPosition.y);
        maxY.Add(obj_middleparCon.transform.localPosition.y);
        maxY.Add(obj_leftparCon.transform.localPosition.y);
        maxY.Sort((a, b) => { return (int)(b - a); });
        sp_Bg_r.transform.localPosition = new Vector3(sp_Bg_r.transform.localPosition.x, Math.Abs(maxY[0] - obj_rightparCon.transform.localPosition.y), 0);
        sp_Bg_m.transform.localPosition = new Vector3(sp_Bg_m.transform.localPosition.x, Math.Abs(maxY[0] - obj_middleparCon.transform.localPosition.y), 0);
        sp_Bg_l.transform.localPosition = new Vector3(sp_Bg_l.transform.localPosition.x, Math.Abs(maxY[0] - obj_leftparCon.transform.localPosition.y), 0);
        yield return null;
        sc_scroll_r.ResetPosition();
        sc_scroll_m.ResetPosition();
        sc_scroll_l.ResetPosition();
        yield return null;
        scroll_R.alpha = 1f;
        sp_Bg_r.alpha = 1;
        obj_title_r.SetActive(true);
        r_recycle.SetActive(true);
        if (!positionReset)
        {

            scroll_M.alpha = 1f;
            sp_Bg_m.alpha = 1;
            obj_title_m.SetActive(true);
            m_recycle.SetActive(true);

            scroll_L.alpha = 1f;
            sp_Bg_l.alpha = 1;
            obj_title_l.SetActive(true);
            l_recycle.SetActive(true);
            grid_btnsGrid_r.gameObject.SetActive(true);
        }
    }
    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        samePosEquiplist = null;
        positionReset = false;
        CSEffectPlayMgr.Instance.Recycle(tex_texbg_m);
        if (data.r_basicInfo != null && data.r_basicInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.r_basicInfo); };
        if (data.r_randomInfo != null && data.r_randomInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.r_randomInfo); };
        if (data.r_equipSkillInfo != null && data.r_equipSkillInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.r_equipSkillInfo); };
        if (data.m_basicInfo != null && data.m_basicInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.m_basicInfo); };
        if (data.m_randomInfo != null && data.m_randomInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.m_randomInfo); };
        if (data.m_equipSkillInfo != null && data.m_equipSkillInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.m_equipSkillInfo); };
        if (data.l_basicInfo != null && data.l_basicInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.l_basicInfo); };
        if (data.l_randomInfo != null && data.l_randomInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.l_randomInfo); };
        if (data.l_equipSkillInfo != null && data.l_equipSkillInfo.Properties.Count != 0) { StructTipData.Instance.RecycleSingle(data.l_equipSkillInfo); };
        data = null;
        scroll_R = null;
        btnsName = null;
        ex_data = null;
        base.OnDestroy();
        UIManager.Instance.ClosePanel<UIEquipZhuFuPanel>();
    }
    void GetCloseTipsEvent(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIEquipTipPanel>();
    }
    public void StruectureData(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
        if (_itemCfg == null)
        {
            return;
        }
        if (data == null) { data = new EqiupTipData(); }
        data.r_itemCfg = _itemCfg;
        data.r_itemInfo = _info;

        float addValue = 0;
        if (ex_data != null && _type == TipsOpenType.RoleEquip)
        {
            int posId = Convert.ToInt32(ex_data);
            //装备位强化额外属性值
            addValue += CSEnhanceInfo.Instance.GetEnhanceAddAttribute(posId);
        }
        data.r_basicInfo = StructTipData.Instance.GetEquipBasicData(_itemCfg, _info, addValue);
        if (CSBagInfo.Instance.IsWoLongEquip(_itemCfg))
        {
            //data.r_wolongAttr = StructTipData.Instance.GetWolongRandomAttr(_itemCfg, _info);
            data.r_wolongSkill = StructTipData.Instance.GetWolongRandomSkill(_itemCfg, _info);
        }
        else
        {
            data.r_wolongSuit = StructTipData.Instance.GetWoLongSuitData(_itemCfg);
            data.r_randomInfo = StructTipData.Instance.GetEquipRandomData(_itemCfg, _info);
            //data.r_equipSkillInfo = StructTipData.Instance.GetEquipSkillData(_info);
        }
        //只有背包内点击，会显示多个装备的对比
        if (_type == TipsOpenType.Bag)
        {
            obj_equipped_r.SetActive(false);

            CSBagInfo.Instance.GetEquipInfoByPos(data.r_itemCfg.subType, samePosEquiplist);
            if (samePosEquiplist.Count == 0)
            {
                if (!positionReset) obj_rightpar.transform.localPosition = Vector3.zero;
            }
            else if (samePosEquiplist.Count == 1)
            {
                obj_equipped_m.SetActive(true);
                data.m_itemInfo = samePosEquiplist[0].equip;
                data.m_itemCfg = ItemTableManager.Instance.GetItemCfg(data.m_itemInfo.configId);
                if (CSBagInfo.Instance.IsWoLongEquip(data.m_itemCfg))
                {
                    //data.m_wolongAttr = StructTipData.Instance.GetWolongRandomAttr(data.m_itemCfg, data.m_itemInfo);
                    data.m_wolongSkill = StructTipData.Instance.GetWolongRandomSkill(data.m_itemCfg, data.m_itemInfo); ;
                }
                else
                {
                    data.m_randomInfo = StructTipData.Instance.GetEquipRandomData(data.m_itemCfg, data.m_itemInfo);
                    data.m_wolongSuit = StructTipData.Instance.GetWoLongSuitData(data.m_itemCfg);
                    //data.m_equipSkillInfo = StructTipData.Instance.GetEquipSkillData(data.m_itemInfo);
                }
                addValue += CSEnhanceInfo.Instance.GetEnhanceAddAttribute(Convert.ToInt32(samePosEquiplist[0].position));
                data.m_basicInfo = StructTipData.Instance.GetEquipBasicData(data.m_itemCfg, data.m_itemInfo, addValue);

                if (!positionReset)
                {
                    obj_rightpar.transform.localPosition = new Vector3(weight - 60 - weight / 2, 0, 0);
                    obj_middlepar.transform.localPosition = new Vector3(-60 - weight / 2, 0, 0);
                }
                else
                {
                    obj_middlepar.SetActive(false);
                }
            }
            else
            {
                obj_equipped_m.SetActive(true);
                //middle
                int middlePos = (samePosEquiplist[0].position < samePosEquiplist[1].position) ? 1 : 0;
                data.m_itemInfo = samePosEquiplist[middlePos].equip;
                data.m_itemCfg = ItemTableManager.Instance.GetItemCfg(data.m_itemInfo.configId);
                if (CSBagInfo.Instance.IsWoLongEquip(data.m_itemCfg))
                {
                    //data.m_wolongAttr = StructTipData.Instance.GetWolongRandomAttr(data.m_itemCfg, data.m_itemInfo);
                    data.m_wolongSkill = StructTipData.Instance.GetWolongRandomSkill(data.m_itemCfg, data.m_itemInfo); ;
                }
                else
                {
                    data.m_randomInfo = StructTipData.Instance.GetEquipRandomData(data.m_itemCfg, data.m_itemInfo);
                    data.m_wolongSuit = StructTipData.Instance.GetWoLongSuitData(data.m_itemCfg);
                    //data.m_equipSkillInfo = StructTipData.Instance.GetEquipSkillData(data.m_itemInfo);
                }
                addValue += CSEnhanceInfo.Instance.GetEnhanceAddAttribute(Convert.ToInt32(samePosEquiplist[middlePos].position));
                data.m_basicInfo = StructTipData.Instance.GetEquipBasicData(data.m_itemCfg, data.m_itemInfo, addValue);

                obj_equipped_l.SetActive(true);
                //left
                int leftPos = (samePosEquiplist[0].position < samePosEquiplist[1].position) ? 0 : 1;
                data.l_itemInfo = samePosEquiplist[leftPos].equip;
                data.l_itemCfg = ItemTableManager.Instance.GetItemCfg(data.l_itemInfo.configId);
                if (CSBagInfo.Instance.IsWoLongEquip(data.l_itemCfg))
                {
                    //data.l_wolongAttr = StructTipData.Instance.GetWolongRandomAttr(data.l_itemCfg, data.l_itemInfo);
                    data.l_wolongSkill = StructTipData.Instance.GetWolongRandomSkill(data.l_itemCfg, data.l_itemInfo); ;
                }
                else
                {
                    data.l_randomInfo = StructTipData.Instance.GetEquipRandomData(data.l_itemCfg, data.l_itemInfo);
                    data.l_wolongSuit = StructTipData.Instance.GetWoLongSuitData(data.l_itemCfg);
                    //data.l_equipSkillInfo = StructTipData.Instance.GetEquipSkillData(data.l_itemInfo);
                }
                addValue += CSEnhanceInfo.Instance.GetEnhanceAddAttribute(Convert.ToInt32(samePosEquiplist[leftPos].position));
                data.l_basicInfo = StructTipData.Instance.GetEquipBasicData(data.l_itemCfg, data.l_itemInfo, addValue);


                if (!positionReset)
                {
                    //向左偏移40  否则右边按钮会被遮挡
                    obj_rightpar.transform.localPosition = new Vector3(weight - 60, 0, 0);
                    obj_middlepar.transform.localPosition = new Vector3(-60, 0, 0);
                    obj_leftpar.transform.localPosition = new Vector3(-weight - 60, 0, 0);
                }
                else
                {
                    obj_middlepar.SetActive(false);
                    obj_leftpar.SetActive(false);
                }
            }
        }
        else
        {
            if (_type == TipsOpenType.RoleEquip) { obj_equipped_r.SetActive(true); }
            if (!positionReset) obj_rightpar.transform.localPosition = Vector3.zero;
        }
    }
    #region 侧边按钮
    void StructBtnData(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
        btnsName.Clear();
        if (_type == TipsOpenType.Bag)
        {
            btnsName = StructTipData.Instance.StructBtnData(_type, _itemCfg, _info);
            for (int i = btnsName.Count - 1; i >= 0; i--)
            {
                if (btnsName[i].type == TipBtnType.ChongZhu)
                {
                    if (CSBagInfo.Instance.IsNormalEquip(_itemCfg) && _itemCfg.subType != 10)
                    {
                        if (_info.quality == 5 || !UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu) || (_itemCfg.levClass == 0))
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                }
                else if (btnsName[i].type == TipBtnType.XiLian)
                {
                    if (CSBagInfo.Instance.IsNormalEquip(_itemCfg))
                    {
                        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                }
                else if (btnsName[i].type == TipBtnType.Intensify && (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance)))
                {
                    btnsName.RemoveAt(i);
                }
                else if (btnsName[i].type == TipBtnType.Deal)
                {
                    if (_info.bind == 1 || _itemCfg.saleType == 3)
                    {
                        btnsName.RemoveAt(i);
                    }
                }
                else if (btnsName[i].type == TipBtnType.Forge)
                {
                    if (CSBagInfo.Instance.IsWoLongEquip(_itemCfg))
                    {
                        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_LongJi) || _itemCfg.levClass == 0)
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (CSBagInfo.Instance.IsNormalEquip(_itemCfg))
                        {
                            if (UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu) && _itemCfg.levClass != 0)
                            {
                                if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
                                {
                                    if (_info.quality == 5)
                                    {
                                        btnsName.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                btnsName.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            //当前部位身上没有，补一个穿戴 3   有： 替换5  替换左8   替换右9
            CSBagInfo.Instance.GetEquipInfoByPos(_itemCfg.subType, samePosEquiplist);
            if (samePosEquiplist.Count == 0)
            {
                btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(2), TipBtnType.Wear, _itemCfg, _info, _type, CSBagInfo.Instance.GetEquipWearPos(_itemCfg)));
            }
            else if (samePosEquiplist.Count == 1)
            {
                //双位置装备显示穿戴，默认穿在空位置上   ，单件装备直接替换掉身上的
                if (_itemCfg.subType == 5 || _itemCfg.subType == 6 || _itemCfg.subType == 105 || _itemCfg.subType == 106)
                {
                    btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(2), TipBtnType.Wear, _itemCfg, _info, _type, CSBagInfo.Instance.GetEquipWearPos(_itemCfg)));
                }
                else
                {
                    btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(4), TipBtnType.Replace, _itemCfg, _info, _type));
                }
            }
            else if (samePosEquiplist.Count == 2)
            {
                btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(7), TipBtnType.ReplaceLeft, _itemCfg, _info, _type));
                btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(8), TipBtnType.ReplaceRight, _itemCfg, _info, _type));
            }
        }
        else if (_type == TipsOpenType.RoleEquip) //补一个   卸下4
        {
            btnsName = StructTipData.Instance.StructBtnData(_type, _itemCfg, _info);

            for (int i = btnsName.Count - 1; i >= 0; i--)
            {
                if (btnsName[i].type == TipBtnType.ChongZhu)
                {
                    if (CSBagInfo.Instance.IsNormalEquip(_itemCfg) && _itemCfg.subType != 10)
                    {
                        if (_info.quality == 5 || !UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu))
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                }
                else if (btnsName[i].type == TipBtnType.XiLian)
                {
                    if (CSBagInfo.Instance.IsNormalEquip(_itemCfg))
                    {
                        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                }
                else if (btnsName[i].type == TipBtnType.Discard)//策划需求去掉丢弃
                {
                    btnsName.RemoveAt(i);
                }
                else if (btnsName[i].type == TipBtnType.Intensify && (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_enhance)))
                {
                    btnsName.RemoveAt(i);
                }
                else if (btnsName[i].type == TipBtnType.Deal)
                {
                    btnsName.RemoveAt(i);
                }
                else if (btnsName[i].type == TipBtnType.Forge)
                {
                    if (CSBagInfo.Instance.IsWoLongEquip(_itemCfg))
                    {
                        if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_LongJi) || _itemCfg.levClass == 0)
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (UICheckManager.Instance.DoCheckFunction(FunctionType.funP_chongzhu) && _itemCfg.levClass != 0)
                        {
                            if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
                            {
                                if (_info.quality == 5)
                                {
                                    btnsName.RemoveAt(i);
                                }
                            }
                        }
                        else
                        {
                            btnsName.RemoveAt(i);
                        }
                    }
                }
            }
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(3), TipBtnType.TakeOff, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.BagWarehouse)//只有一个放入 (仓库打开时背包格子)
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(10), TipBtnType.PutIn, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.WarehouseBag)//只有一个取出  （仓库打开时仓库格子点击）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(11), TipBtnType.TakeOut, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.Bag2Recycle)//只有一个回收  （背包到回收时背包格子点击）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(14), TipBtnType.Recycle, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.Recycle2Bag)//只有一个撤销  （回收到背包时回收格子点击）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(15), TipBtnType.CancelRecycle, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.GuildWareHouseDonate)//公会仓库捐赠
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(12), TipBtnType.Donate, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.GuildWareHouseExchange)//公会仓库兑换
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(13), TipBtnType.Exchange, _itemCfg, _info, _type, ex_data));
        }
        else if (_type == TipsOpenType.WildAdventureRewardReceive)//只有一个取出  （野外探险用）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(11), TipBtnType.TakeOut, _itemCfg, _info, _type, ex_data));
        }
    }
    void RefreshBtns()
    {
        if (btnsName == null) return;
        btnsName.Reverse();
        grid_btnsGrid_r.MaxCount = btnsName.Count;
        for (int i = 0; i < grid_btnsGrid_r.controlList.Count; i++)
        {
            UILabel name = grid_btnsGrid_r.controlList[i].transform.Find("Label").GetComponent<UILabel>();
            name.text = btnsName[i].name;
            if (btnsName[i].type == TipBtnType.Wear || btnsName[i].type == TipBtnType.Replace || btnsName[i].type == TipBtnType.ReplaceLeft || btnsName[i].type == TipBtnType.ReplaceRight)
            {
                UISprite sprite = grid_btnsGrid_r.controlList[i].transform.Find("Background").GetComponent<UISprite>();
                sprite.spriteName = "btn_samll1";
                name.color = UtilityColor.HexToColor("#b0bbcf");
            }
            Transform RedPoint = grid_btnsGrid_r.controlList[i].transform.Find("sp_icon");
            if (RedPoint != null) RedPoint.gameObject.SetActive(btnsName[i].type == TipBtnType.ZhuFu && 50000028.GetItemCount() > 0);
            UIEventListener.Get(grid_btnsGrid_r.controlList[i], btnsName[i]).onClick = TipBtnClick;
        }
    }
    #endregion
    RepeatedField<bag.WolongRandomEffect> baseAffix = new RepeatedField<bag.WolongRandomEffect>();
    RepeatedField<bag.WolongRandomEffect> intenAffix = new RepeatedField<bag.WolongRandomEffect>();
    void RefreshRightPart()
    {
        scroll_R = sc_scroll_r.GetComponent<UIPanel>();
        float start_y = scroll_R.GetViewSize().y / 2;
        scroll_R.alpha = 0.05f;
        sp_Bg_r.alpha = 0.05f;
        obj_title_r.SetActive(false);
        //title
        RefreshRightTitle();
        //基础属性展示
        if (data.r_basicInfo != null && data.r_basicInfo.Properties.Count != 0)
        {
            obj_basic_r.SetActive(true);
            RefreshBasic(grid_basicGrid_r, data.r_basicInfo);
        }
        else
        {
            obj_basic_r.SetActive(false);
        }
        if (CSBagInfo.Instance.IsNormalEquip(data.r_itemCfg))
        {
            //普通装备元魂属性
            RightNormalEquipYuanHun();
            //普通装备随机属性
            if (data.r_randomInfo != null && data.r_randomInfo.Properties.Count != 0)
            {
                obj_random_r.SetActive(true);
                obj_exceptedRandomAttr.SetActive(false);
                RefreshRandom(grid_randomGrid_r, data.r_randomInfo);
            }
            else
            {
                if (SpecialEquipTableManager.Instance.IsSpecialEquip(data.r_itemCfg.id))
                {
                    obj_random_r.SetActive(true);
                    obj_exceptedRandomAttr.SetActive(false);
                    RefreshRandom(grid_randomGrid_r, StructTipData.Instance.FirstChargeEquipShowAttr(data.r_itemCfg, SpecialEquipTableManager.Instance.GetSpecialEquipAttr(data.r_itemCfg.id), mPoolHandleManager));
                }
                else
                {

                    obj_random_r.SetActive(false);
                    obj_exceptedRandomAttr.SetActive(true);
                    RefreshExceptedRandomInfo();
                }
            }
            //普通装备随机技能(目前版本去掉随机技能)
            #region
            //if (data.r_equipSkillInfo != null && data.r_equipSkillInfo.Properties.Count != 0)
            //{
            //    obj_skill_r.SetActive(true);
            //    RefreshSkill(grid_skillGrid_r, data.r_equipSkillInfo);
            //}
            //else
            //{
            //    if (SpecialEquipTableManager.Instance.IsSpecialEquip(data.r_itemCfg.id))
            //    {
            //        TipDataItem item = mPoolHandleManager.GetCustomClass<TipDataItem>();
            //        item = StructTipData.Instance.FirstChargeEquipShowSkill(data.r_itemCfg, SpecialEquipTableManager.Instance.GetSpecialEquipAttr(data.r_itemCfg.id));
            //        if (item != null && item.Properties.Count != 0)
            //        {
            //            obj_skill_r.SetActive(true);
            //            RefreshSkill(grid_skillGrid_r, StructTipData.Instance.FirstChargeEquipShowSkill(data.r_itemCfg, SpecialEquipTableManager.Instance.GetSpecialEquipAttr(data.r_itemCfg.id)));
            //        }
            //        else
            //        {
            //            obj_skill_r.SetActive(false);
            //        }
            //    }
            //    else
            //    {
            //        obj_skill_r.SetActive(false);
            //    }
            //}
            #endregion
            //战魂套装信息
            if (data.r_wolongSuit != null && data.r_wolongSuit.Name != null)
            {
                obj_wolongPart_r.SetActive(true);
                RefreshRightWoLongSuit(data.r_wolongSuit);
            }
            else
            {
                obj_wolongPart_r.SetActive(false);
            }
        }
        else
        {
            //卧龙龙魂属性
            RefreshRightWoLongLongHun();
            //卧龙龙技属性
            if (data.r_wolongSkill != null && data.r_wolongSkill.Properties.Count != 0)
            {
                r_wolongRandomSkill.SetActive(true);
                RefreshRightWoLongRandomSkill(r_wolongRandomSkillGrid, data.r_wolongSkill);
            }
            else
            {
                if (SpecialEquipTableManager.Instance.IsSpecialEquip(data.r_itemCfg.id))
                {
                    r_wolongRandomSkill.SetActive(true);
                    string str = SpecialEquipTableManager.Instance.GetSpecialEquipLongji(data.r_itemCfg.id);
                    data.r_wolongSkill = StructTipData.Instance.GetWolongRandomSkill(data.r_itemCfg, str);
                    RefreshRightWoLongRandomSkill(r_wolongRandomSkillGrid, data.r_wolongSkill);
                }
                else
                {
                    if (data.r_itemInfo != null && data.r_itemCfg.levClass == 0)
                    {
                        r_wolongRandomSkill.SetActive(false);
                    }
                    else
                    {
                        r_wolongRandomSkillGrid.MaxCount = 1;
                        UILabel des = r_wolongRandomSkillGrid.controlList[0].transform.Find("key").GetComponent<UILabel>();
                        des.text = ClientTipsTableManager.Instance.GetClientTipsContext(1746);
                        r_wolongRandomSkill.SetActive(true);
                    }
                }
            }
            if (data.r_itemInfo == null)
            {
                //卧龙龙力
                if (SpecialEquipTableManager.Instance.IsSpecialEquip(data.r_itemCfg.id))
                {
                    baseAffix.Clear();
                    intenAffix.Clear();
                    string str1 = SpecialEquipTableManager.Instance.GetSpecialEquipLongli1(data.r_itemCfg.id);
                    string str2 = SpecialEquipTableManager.Instance.GetSpecialEquipLongli2(data.r_itemCfg.id);
                    //FNDebug.Log($"{str1}      {str2}");
                    StructTipData.Instance.GetWoLongLongliBaseAffix(baseAffix, str1);
                    StructTipData.Instance.GetWoLongLongliBaseAffix(intenAffix, str2);
                    ShowLongLi(baseAffix, intenAffix, r_longLi, r_longLiinten, r_grid_longliBase, r_grid_longliInten, r_table_longjiInten);
                }
                else
                {
                    RefreshWolongLi(data.r_itemInfo, r_longLi, r_longLiinten, r_grid_longliBase, r_grid_longliInten, r_table_longjiInten);
                }
            }
            else
            {
                RefreshWolongLi(data.r_itemInfo, r_longLi, r_longLiinten, r_grid_longliBase, r_grid_longliInten, r_table_longjiInten);
            }
        }
        //回收
        RefreshRightRecycle();
        #region 位置计算
        r_grid_linePar.MaxCount = table_content_r.transform.childCount;
        int gap = 10;
        float temp_toHeigh = 0;
        int lineGap = 10;
        int lineIndex = -1;
        for (int i = 0; i < table_content_r.transform.childCount; i++)
        {
            Transform trans = table_content_r.transform.GetChild(i);
            float temp_heigh = NGUIMath.CalculateRelativeWidgetBounds(trans, false).size.y;
            r_grid_linePar.controlList[i].SetActive(trans.gameObject.activeSelf);
            if (trans.gameObject.activeSelf)
            {
                lineIndex = i;
            }
            if (temp_heigh > 0 && i >= (table_content_r.transform.childCount - 1))
            {
                r_grid_linePar.controlList[i].SetActive(false);
            }
            temp_toHeigh = temp_toHeigh + temp_heigh;
            r_grid_linePar.controlList[i].transform.localPosition = new Vector3(0, -temp_toHeigh - gap - lineGap, 0);
            gap = (temp_heigh == 0) ? gap : gap + 20;
        }
        int recycleHeigh = (int)NGUIMath.CalculateRelativeWidgetBounds(r_recyclePar.transform, false).size.y;
        int scrTotalH = (totalHeight - 140 - recycleHeigh - 10) >= scrollHeight ? scrollHeight : (totalHeight - 140 - recycleHeigh - 10);

        if (lineIndex != -1)
        {
            r_grid_linePar.controlList[lineIndex].SetActive(false);
            gap = gap - 10;
        }
        int scrHei = (int)(gap - 10 + temp_toHeigh) > scrTotalH ? scrTotalH : (int)(gap - 10 + temp_toHeigh);
        int total_high = 140 + (scrHei > scrollHeight ? scrollHeight : scrHei) + recycleHeigh + ((lineIndex != -1) ? 10 : 20);
        widge_content_r.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        table_content_r.Reposition();
        widge_contentPar_r.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        UIWidget widge_drag_r = Get<UIWidget>("Right/content/scrollPar/drag");
        widge_drag_r.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        RefreshBtns();
        grid_btnsGrid_r.gameObject.SetActive(false);
        r_recycle.SetActive(false);
        sp_Bg_r.height = (total_high >= totalHeight) ? totalHeight : total_high;
        #endregion
    }
    void RefreshMiddlePart()
    {
        scroll_M = sc_scroll_m.GetComponent<UIPanel>();
        if (data.m_itemInfo == null)
        {
            return;
        }
        float start_y = scroll_M.GetViewSize().y / 2;
        obj_middlepar.SetActive(data.m_itemInfo != null);
        scroll_M.alpha = 0.05f;
        sp_Bg_m.alpha = 0.05f;
        obj_title_m.SetActive(false);
        //title
        RefreshMiddleTitle();
        //基础属性展示
        if (data.m_basicInfo != null && data.m_basicInfo.Properties.Count != 0)
        {
            obj_basic_m.SetActive(true);
            RefreshBasic(grid_basicGrid_m, data.m_basicInfo);
        }
        else
        {
            obj_basic_m.SetActive(false);
        }

        if (CSBagInfo.Instance.IsNormalEquip(data.m_itemCfg))
        {
            //普通装备元魂属性
            MiddleNormalEquipYuanHun();
            //普通装备随机属性
            if (data.m_randomInfo != null && data.m_randomInfo.Properties.Count != 0)
            {
                obj_mandom_m.SetActive(true);
                RefreshRandom(grid_mandomGrid_m, data.m_randomInfo);
            }
            else
            {
                obj_mandom_m.SetActive(false);
            }
            #region 普通装备随机技能(目前版本去掉随机技能)
            //if (data.m_equipSkillInfo != null && data.m_equipSkillInfo.Properties.Count != 0)
            //{
            //    obj_skill_m.SetActive(true);
            //    RefreshSkill(grid_skillGrid_m, data.m_equipSkillInfo);
            //}
            //else
            //{
            //    obj_skill_m.SetActive(false);
            //}
            #endregion
            //战魂套装信息
            if (data.m_wolongSuit != null && data.m_wolongSuit.Name != null)
            {
                obj_wolongPart_m.SetActive(true);
                RefreshMiddleWoLongSuit(data.m_wolongSuit);
            }
            else
            {
                obj_wolongPart_m.SetActive(false);
            }
        }
        else
        {
            //卧龙龙魂属性
            RefreshMiddleWoLongLongHun();
            //卧龙龙技属性
            if (data.m_wolongSkill != null && data.m_wolongSkill.Properties.Count != 0)
            {
                m_wolongRandomSkill.SetActive(true);
                RefreshRightWoLongRandomSkill(m_wolongRandomSkillGrid, data.m_wolongSkill);
            }
            else
            {
                m_wolongRandomSkill.SetActive(false);
            }
            //卧龙龙力
            RefreshWolongLi(data.m_itemInfo, m_longLi, m_longLiinten, m_grid_longliBase, m_grid_longliInten, m_table_longjiInten);
        }
        //回收
        RefreshMiddleRecycle();
        #region 位置计算
        m_grid_linePar.MaxCount = table_content_m.transform.childCount;
        int gap = 10;
        float temp_toHeigh = 0;
        int lineGap = 10;
        int lineIndex = -1;
        for (int i = 0; i < table_content_m.transform.childCount; i++)
        {
            Transform trans = table_content_m.transform.GetChild(i);
            float temp_heigh = NGUIMath.CalculateRelativeWidgetBounds(trans, false).size.y;
            m_grid_linePar.controlList[i].SetActive(trans.gameObject.activeSelf);
            if (trans.gameObject.activeSelf)
            {
                lineIndex = i;
            }
            if (i == (table_content_m.transform.childCount - 1))
            {
                m_grid_linePar.controlList[i].SetActive(false);
            }
            temp_toHeigh = temp_toHeigh + temp_heigh;
            m_grid_linePar.controlList[i].transform.localPosition = new Vector3(0, -temp_toHeigh - gap - lineGap, 0);
            gap = (temp_heigh == 0) ? gap : gap + 20;
        }
        int recycleHeigh = (int)NGUIMath.CalculateRelativeWidgetBounds(m_recyclePar.transform, false).size.y;
        if (lineIndex != -1)
        {
            m_grid_linePar.controlList[lineIndex].SetActive(false);
            gap = gap - 10;
        }
        int scrHei = (int)(gap - 10 + temp_toHeigh);
        int total_high = 140 + (scrHei > scrollHeight ? scrollHeight : scrHei) + recycleHeigh + ((lineIndex != -1) ? 10 : 20);

        widge_content_m.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        table_content_m.Reposition();
        widge_contentPar_m.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        UIWidget widge_drag_m = Get<UIWidget>("Middle/content/scrollPar/drag");
        widge_drag_m.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        sc_scroll_m.ScrollImmidate(0.01f);
        m_recycle.SetActive(false);
        sp_Bg_m.height = (total_high >= totalHeight) ? totalHeight : total_high;
        #endregion
    }
    void RefreshLeftPart()
    {
        scroll_L = sc_scroll_l.GetComponent<UIPanel>();
        if (data.l_itemInfo == null)
        {
            return;
        }
        float start_y = scroll_L.GetViewSize().y / 2;
        obj_leftpar.SetActive(data.l_itemInfo != null);
        scroll_L.alpha = 0.05f;
        sp_Bg_l.alpha = 0.05f;
        obj_title_l.SetActive(false);
        //title
        RefreshLeftTitle();
        //基础属性展示
        if (data.l_basicInfo != null && data.l_basicInfo.Properties.Count != 0)
        {
            obj_basic_l.SetActive(true);
            RefreshBasic(grid_basicGrid_l, data.l_basicInfo);
        }
        else
        {
            obj_basic_l.SetActive(false);
        }
        if (CSBagInfo.Instance.IsNormalEquip(data.l_itemCfg))
        {
            //普通装备元魂属性
            LeftNormalEquipYuanHun();
            //普通装备随机属性
            if (data.l_randomInfo != null && data.l_randomInfo.Properties.Count != 0)
            {
                obj_landom_l.SetActive(true);
                RefreshRandom(grid_landomGrid_l, data.l_randomInfo);
            }
            else
            {
                obj_landom_l.SetActive(false);
            }
            #region  普通装备随机技能(目前版本去掉随机技能)
            //if (data.l_equipSkillInfo != null && data.l_equipSkillInfo.Properties.Count != 0)
            //{
            //    obj_skill_l.SetActive(true);
            //    RefreshSkill(grid_skillGrid_l, data.l_equipSkillInfo);
            //}
            //else
            //{
            //    obj_skill_l.SetActive(false);
            //}
            #endregion
            //战魂套装信息
            if (data.l_wolongSuit != null && data.l_wolongSuit.Name != null)
            {
                obj_wolongPart_l.SetActive(true);
                RefreshLeftWoLongSuit(data.l_wolongSuit);
            }
            else
            {
                obj_wolongPart_l.SetActive(false);
            }
        }
        else
        {
            //卧龙龙魂属性
            RefreshLeftWoLongLongHun();
            //卧龙龙技属性
            if (data.l_wolongSkill != null && data.l_wolongSkill.Properties.Count != 0)
            {
                l_wolongRandomSkill.SetActive(true);
                RefreshRightWoLongRandomSkill(l_wolongRandomSkillGrid, data.l_wolongSkill);
            }
            else
            {
                l_wolongRandomSkill.SetActive(false);
            }
            //卧龙龙力
            RefreshWolongLi(data.l_itemInfo, l_longLi, l_longLiinten, l_grid_longliBase, l_grid_longliInten, l_table_longjiInten);
        }
        //回收
        RefreshLeftRecycle();
        #region 位置计算
        l_grid_linePar.MaxCount = table_content_l.transform.childCount;
        int gap = 10;
        float temp_toHeigh = 0;
        int lineGap = 10;
        int lineIndex = -1;
        for (int i = 0; i < table_content_l.transform.childCount; i++)
        {
            Transform trans = table_content_l.transform.GetChild(i);
            float temp_heigh = NGUIMath.CalculateRelativeWidgetBounds(trans, false).size.y;
            l_grid_linePar.controlList[i].SetActive(trans.gameObject.activeSelf);
            if (trans.gameObject.activeSelf)
            {
                lineIndex = i;
            }
            if (i == (table_content_l.transform.childCount - 1))
            {
                l_grid_linePar.controlList[i].SetActive(false);
            }
            temp_toHeigh = temp_toHeigh + temp_heigh;
            l_grid_linePar.controlList[i].transform.localPosition = new Vector3(0, -temp_toHeigh - gap - lineGap, 0);
            gap = (temp_heigh == 0) ? gap : gap + 20;
        }
        int recycleHeigh = (int)NGUIMath.CalculateRelativeWidgetBounds(l_recyclePar.transform, false).size.y;
        if (lineIndex != -1)
        {
            l_grid_linePar.controlList[lineIndex].SetActive(false);
            gap = gap - 10;
        }
        int scrHei = (int)(gap - 10 + temp_toHeigh);
        int total_high = 140 + (scrHei > scrollHeight ? scrollHeight : scrHei) + recycleHeigh + ((lineIndex != -1) ? 10 : 20);

        widge_content_l.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        table_content_l.Reposition();
        widge_contentPar_l.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        UIWidget widge_drag_l = Get<UIWidget>("Left/content/scrollPar/drag");
        widge_drag_l.height = (scrHei > scrollHeight ? scrollHeight : scrHei);
        sc_scroll_l.ScrollImmidate(0.01f);
        l_recycle.SetActive(false);
        sp_Bg_l.height = (total_high >= totalHeight) ? totalHeight : total_high;
        #endregion
    }
    List<string> jobs = new List<string>();
    #region title
    void RefreshRightTitle()
    {
        if (data.r_itemInfo == null)
        {
            lb_title_r.text = data.r_itemCfg.name;
            lb_title_r.color = UtilityCsColor.Instance.GetColor(data.r_itemCfg.quality);
            lb_Isbind_r.gameObject.SetActive(false);
            sp_itemBg_r.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.r_itemCfg.quality);
            sp_line_r.spriteName = $"line_eqtips{data.r_itemCfg.quality}";
            CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_r, $"qualitybg{data.r_itemCfg.quality}");
            lb_fightPower_r.text = "";
        }
        else
        {
            lb_Isbind_r.gameObject.SetActive(data.r_itemInfo.bind == 1);
            int fightPower = UtilityFightPower.GetFightPower(data.r_itemInfo, data.r_itemCfg);
            lb_fightPower_r.text = $"{ClientTipsTableManager.Instance.GetClientTipsContext(1267)}{fightPower}";
            if (openType == TipsOpenType.RoleEquip)
            {
                obj_arrow_r.SetActive(false);
            }
            else
            {
                int minPower = CSItemCountManager.Instance.GetEquipedMinFightScore(data.r_itemCfg.id);
                obj_arrow_r.SetActive(fightPower > minPower ? true : false);
            }


            if (CSBagInfo.Instance.IsWoLongEquip(data.r_itemCfg))
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_r, $"qualitybg{data.r_itemCfg.quality}");
                sp_line_r.spriteName = $"line_eqtips{data.r_itemCfg.quality}";
                sp_itemBg_r.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.r_itemCfg.quality);
                string nameStr = "";
                for (int i = 0; i < data.r_itemInfo.baseAffixs.Count; i++)
                {
                    WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
                    nameStr = $"{nameStr}{SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(data.r_itemInfo.baseAffixs[i].id))}";
                }
                lb_title_r.text = $"{UtilityColor.GetItemNameValue(data.r_itemCfg.quality)}{data.r_itemCfg.name}[-]{nameStr}";
            }
            else
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_r, $"qualitybg{data.r_itemInfo.quality}");
                sp_line_r.spriteName = $"line_eqtips{data.r_itemInfo.quality}";
                sp_itemBg_r.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.r_itemInfo.quality);
                lb_title_r.text = $"{UtilityColor.GetItemNameValue(data.r_itemInfo.quality)}{data.r_itemCfg.name}";
            }
        }

        if (CSBagInfo.Instance.IsWoLongEquip(data.r_itemCfg))
        {
            lb_lv_r.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(434), data.r_itemCfg.wolongLv);
            lb_lv_r.color = (data.r_itemCfg.wolongLv <= CSWoLongInfo.Instance.GetWoLongLevel() ? CSColor.beige : CSColor.red);
            obj_wolongSeal_r.gameObject.SetActive(true);
            obj_wolongSeal_r.spriteName = CSBagInfo.Instance.GetItemBaseWoLongIconName(data.r_itemCfg.levClass);
            lb_normalSuit_r.text = "";
        }
        else
        {
            if (data.r_itemCfg.level >= 30)
            {
                lb_normalSuit_r.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(data.r_itemCfg.level));
            }
            lb_lv_r.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), data.r_itemCfg.level);
            lb_lv_r.color = (data.r_itemCfg.level <= CSMainPlayerInfo.Instance.Level ? CSColor.beige : CSColor.red);
            obj_wolongSeal_r.gameObject.SetActive(false);
        }
        lb_job_r.text = Utility.GetJob(data.r_itemCfg.career);
        if ((data.r_itemCfg.career == CSMainPlayerInfo.Instance.Career) || data.r_itemCfg.career == 0)
        {
            lb_job_r.color = CSColor.beige;
        }
        else
        {
            lb_job_r.color = CSColor.red;
        }
        lb_pos_r.text = StructTipData.Instance.GetEquipPosName(data.r_itemCfg.subType % 100);
        sp_icon_r.spriteName = data.r_itemCfg.icon;
        lb_enhance_r.text = "";
        //强化等级
        if (openType == TipsOpenType.RoleEquip && ex_data != null)
        {
            int posId = Convert.ToInt32(ex_data);
            int lv = CSEnhanceInfo.Instance.GetEnhanceLv(posId);
            lb_enhance_r.text = lv > 0 ? $"+{lv}" : "";
        }
    }
    void RefreshMiddleTitle()
    {
        if (data.m_itemInfo == null)
        {
            lb_title_m.text = data.m_itemCfg.name;
            lb_title_m.color = UtilityCsColor.Instance.GetColor(data.m_itemCfg.quality);
            lb_Isbind_m.gameObject.SetActive(false);
            sp_itemBg_m.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.m_itemCfg.quality);
            sp_line_m.spriteName = $"line_eqtips{data.m_itemCfg.quality}";
            CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_m, $"qualitybg{data.m_itemCfg.quality}");
            lb_fightPower_m.text = "";
        }
        else
        {
            lb_Isbind_m.gameObject.SetActive(data.m_itemInfo.bind == 1);
            int fightPower = UtilityFightPower.GetFightPower(data.m_itemInfo, data.m_itemCfg);
            lb_fightPower_m.text = $"{ClientTipsTableManager.Instance.GetClientTipsContext(1267)}{fightPower}";
            //int minPower = CSItemCountManager.Instance.GetFightScore(data.r_itemInfo.id);
            //obj_arrow_m.SetActive(fightPower > minPower ? true : false);

            if (CSBagInfo.Instance.IsWoLongEquip(data.m_itemCfg))
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_m, $"qualitybg{data.m_itemCfg.quality}");
                sp_line_m.spriteName = $"line_eqtips{data.m_itemCfg.quality}";
                sp_itemBg_m.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.m_itemCfg.quality);
                string nameStr = "";
                for (int i = 0; i < data.m_itemInfo.baseAffixs.Count; i++)
                {
                    WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
                    nameStr = $"{nameStr}{SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(data.m_itemInfo.baseAffixs[i].id))}";
                }
                lb_title_m.text = $"{UtilityColor.GetItemNameValue(data.m_itemCfg.quality)}{data.m_itemCfg.name}[-]{nameStr}";
            }
            else
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_m, $"qualitybg{data.m_itemInfo.quality}");
                sp_line_m.spriteName = $"line_eqtips{data.m_itemInfo.quality}";
                sp_itemBg_m.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.m_itemInfo.quality);
                lb_title_m.text = $"{UtilityColor.GetItemNameValue(data.m_itemInfo.quality)}{data.m_itemCfg.name}";
            }
        }

        if (CSBagInfo.Instance.IsWoLongEquip(data.m_itemCfg))
        {
            lb_lv_m.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(434), data.m_itemCfg.wolongLv);
            lb_lv_m.color = (data.m_itemCfg.level <= CSWoLongInfo.Instance.GetWoLongLevel() ? CSColor.beige : CSColor.red);
            obj_wolongSeal_m.gameObject.SetActive(true);
            obj_wolongSeal_m.spriteName = CSBagInfo.Instance.GetItemBaseWoLongIconName(data.m_itemCfg.levClass);
            lb_normalSuit_m.text = "";
        }
        else
        {
            if (data.m_itemCfg.level >= 30)
            {
                lb_normalSuit_m.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(data.m_itemCfg.level));
            }
            lb_lv_m.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), data.m_itemCfg.level);
            lb_lv_m.color = (data.m_itemCfg.level <= CSMainPlayerInfo.Instance.Level ? CSColor.beige : CSColor.red);
            obj_wolongSeal_m.gameObject.SetActive(false);
        }
        lb_job_m.text = Utility.GetJob(data.m_itemCfg.career);
        if ((data.m_itemCfg.career == CSMainPlayerInfo.Instance.Career) || data.m_itemCfg.career == 0)
        {
            lb_job_m.color = CSColor.beige;
        }
        else
        {
            lb_job_m.color = CSColor.red;
        }
        lb_pos_m.text = StructTipData.Instance.GetEquipPosName(data.m_itemCfg.subType % 100);
        sp_icon_m.spriteName = data.m_itemCfg.icon;
        lb_enhance_m.text = "";
        //强化等级
        if (openType == TipsOpenType.RoleEquip && ex_data != null)
        {
            int posId = Convert.ToInt32(ex_data);
            int lv = CSEnhanceInfo.Instance.GetEnhanceLv(posId);
            lb_enhance_m.text = lv > 0 ? $"+{lv}" : "";
        }
    }
    void RefreshLeftTitle()
    {
        if (data.l_itemInfo == null)
        {
            lb_title_l.text = data.l_itemCfg.name;
            lb_title_l.color = UtilityCsColor.Instance.GetColor(data.l_itemCfg.quality);
            lb_Isbind_l.gameObject.SetActive(false);
            sp_itemBg_l.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.l_itemCfg.quality);
            sp_line_l.spriteName = $"line_eqtips{data.l_itemCfg.quality}";
            CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_l, $"qualitybg{data.l_itemCfg.quality}");
            lb_fightPower_l.text = "";
        }
        else
        {
            lb_Isbind_l.gameObject.SetActive(data.l_itemInfo.bind == 1);
            int fightPower = UtilityFightPower.GetFightPower(data.l_itemInfo, data.l_itemCfg);
            lb_fightPower_l.text = $"{ClientTipsTableManager.Instance.GetClientTipsContext(1267)}{fightPower}";
            //int minPower = CSItemCountManager.Instance.GetFightScore(data.r_itemInfo.id);
            //obj_arrow_l.SetActive(fightPower > minPower ? true : false);
            if (CSBagInfo.Instance.IsWoLongEquip(data.l_itemCfg))
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_l, $"qualitybg{data.l_itemCfg.quality}");
                sp_line_l.spriteName = $"line_eqtips{data.l_itemCfg.quality}";
                sp_itemBg_l.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.l_itemCfg.quality);
                string nameStr = "";
                for (int i = 0; i < data.l_itemInfo.baseAffixs.Count; i++)
                {
                    WoLongRandomAttrTableManager ins = WoLongRandomAttrTableManager.Instance;
                    nameStr = $"{nameStr}{SkillTableManager.Instance.GetNameByGroupId(ins.GetWoLongRandomAttrParameter(data.l_itemInfo.baseAffixs[i].id))}";
                }
                lb_title_l.text = $"{UtilityColor.GetItemNameValue(data.l_itemCfg.quality)}{data.l_itemCfg.name}[-]{nameStr}";
            }
            else
            {
                CSEffectPlayMgr.Instance.ShowUITexture(tex_texbg_l, $"qualitybg{data.l_itemInfo.quality}");
                sp_line_l.spriteName = $"line_eqtips{data.l_itemInfo.quality}";
                sp_itemBg_l.spriteName = ItemTableManager.Instance.GetItemQualityBG(data.l_itemInfo.quality);
                lb_title_l.text = $"{UtilityColor.GetItemNameValue(data.l_itemInfo.quality)}{data.l_itemCfg.name}";
            }
        }

        if (CSBagInfo.Instance.IsWoLongEquip(data.l_itemCfg))
        {
            lb_lv_l.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(434), data.l_itemCfg.wolongLv);
            lb_lv_l.color = (data.l_itemCfg.level <= CSWoLongInfo.Instance.GetWoLongLevel() ? CSColor.beige : CSColor.red);
            obj_wolongSeal_l.gameObject.SetActive(true);
            obj_wolongSeal_l.spriteName = CSBagInfo.Instance.GetItemBaseWoLongIconName(data.l_itemCfg.levClass);
            lb_normalSuit_l.text = "";
        }
        else
        {
            if (data.l_itemCfg.level >= 30)
            {
                lb_normalSuit_l.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2027), UtilityMath.GetTenMultiple(data.l_itemCfg.level));
            }
            lb_lv_l.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(570), data.l_itemCfg.level);
            lb_lv_l.color = (data.l_itemCfg.level <= CSMainPlayerInfo.Instance.Level ? CSColor.beige : CSColor.red);
            obj_wolongSeal_l.gameObject.SetActive(false);
        }
        lb_job_l.text = Utility.GetJob(data.l_itemCfg.career);
        if ((data.l_itemCfg.career == CSMainPlayerInfo.Instance.Career) || data.l_itemCfg.career == 0)
        {
            lb_job_l.color = CSColor.beige;
        }
        else
        {
            lb_job_l.color = CSColor.red;
        }
        lb_pos_l.text = StructTipData.Instance.GetEquipPosName(data.l_itemCfg.subType % 100);
        sp_icon_l.spriteName = data.l_itemCfg.icon;
        lb_enhance_l.text = "";
        //强化等级
        if (openType == TipsOpenType.RoleEquip && ex_data != null)
        {
            int posId = Convert.ToInt32(ex_data);
            int lv = CSEnhanceInfo.Instance.GetEnhanceLv(posId);
            lb_enhance_l.text = lv > 0 ? $"+{lv}" : "";
        }
    }
    #endregion

    //基础属性
    void RefreshBasic(UIGridContainer _go, TipDataItem _mes)
    {
        _go.MaxCount = _mes.Properties.Count;
        for (int i = 0; i < _mes.Properties.Count; i++)
        {
            UILabel key = Get<UILabel>("key", _go.controlList[i].transform);
            UILabel value = Get<UILabel>("value", _go.controlList[i].transform);
            key.text = _mes.Properties[i].Name;
            value.text = _mes.Properties[i].MaxValueName;
            key.color = _mes.Properties[i].Color;
        }
    }
    //随机属性
    void RefreshRandom(UIGridContainer _go, TipDataItem _mes)
    {
        _go.MaxCount = _mes.Properties.Count;
        for (int i = 0; i < _mes.Properties.Count; i++)
        {
            UILabel key = Get<UILabel>("key", _go.controlList[i].transform);
            UILabel value = Get<UILabel>("value", _go.controlList[i].transform);
            key.text = _mes.Properties[i].Name;
            value.text = _mes.Properties[i].MaxValueName;
            key.color = UtilityCsColor.Instance.GetColor(_mes.Properties[i].quality);
        }
    }
    //期望属性
    void RefreshExceptedRandomInfo()
    {
        lb_exceptedRandomAttr.text = SundryTableManager.Instance.GetSundryEffect(427);
    }
    #region 普通装备元魂属性
    void RightNormalEquipYuanHun()
    {
        if (data.r_itemCfg.bufferParam != "")
        {
            r_yuanhun.SetActive(true);
            List<int> yuanhun = UtilityMainMath.SplitStringToIntList(data.r_itemCfg.bufferParam);
            if (yuanhun.Count > 0)
            {
                r_grid_yuanhun.MaxCount = 1;
                UILabel label = r_grid_yuanhun.controlList[0].transform.Find("key").GetComponent<UILabel>();
                label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1905), Math.Round(Convert.ToDecimal(yuanhun[0] * 0.01f), 1,
                MidpointRounding.AwayFromZero));
                label.color = CSColor.purple;
            }
        }
        else
        {
            r_yuanhun.SetActive(false);
        }
    }
    void MiddleNormalEquipYuanHun()
    {
        if (data.m_itemCfg.bufferParam != "")
        {
            m_yuanhun.SetActive(true);
            List<int> yuanhun = UtilityMainMath.SplitStringToIntList(data.m_itemCfg.bufferParam);
            if (yuanhun.Count > 0)
            {
                m_grid_yuanhun.MaxCount = 1;
                UILabel label = m_grid_yuanhun.controlList[0].transform.Find("key").GetComponent<UILabel>();
                label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1905), Math.Round(Convert.ToDecimal(yuanhun[0] * 0.01f), 1,
                MidpointRounding.AwayFromZero));
                label.color = CSColor.purple;
            }
        }
        else
        {
            m_yuanhun.SetActive(false);
        }
    }
    void LeftNormalEquipYuanHun()
    {
        if (data.l_itemCfg.bufferParam != "")
        {
            l_yuanhun.SetActive(true);
            List<int> yuanhun = UtilityMainMath.SplitStringToIntList(data.l_itemCfg.bufferParam);
            if (yuanhun.Count > 0)
            {
                l_grid_yuanhun.MaxCount = 1;
                UILabel label = l_grid_yuanhun.controlList[0].transform.Find("key").GetComponent<UILabel>();
                label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1905), Math.Round(Convert.ToDecimal(yuanhun[0] * 0.01f), 1,
                MidpointRounding.AwayFromZero));
                label.color = CSColor.purple;
            }
        }
        else
        {
            l_yuanhun.SetActive(false);
        }
    }
    #endregion

    #region 卧龙套装属性
    void RefreshRightWoLongSuit(TipProperty property)
    {
        lb_wolongName_r.text = property.Name;
        lb_wolongPro_r.text = property.exValue2;
        lb_wolongdes1_r.text = property.MaxValueName;
        lb_wolongdes2_r.text = property.exValue;
    }
    void RefreshMiddleWoLongSuit(TipProperty property)
    {
        lb_wolongName_m.text = property.Name;
        lb_wolongPro_m.text = property.exValue2;
        lb_wolongdes1_m.text = property.MaxValueName;
        lb_wolongdes2_m.text = property.exValue;
    }
    void RefreshLeftWoLongSuit(TipProperty property)
    {
        lb_wolongName_l.text = property.Name;
        lb_wolongPro_l.text = property.exValue2;
        lb_wolongdes1_l.text = property.MaxValueName;
        lb_wolongdes2_l.text = property.exValue;
    }
    #endregion

    #region  卧龙龙魂属性
    List<List<int>> WLLonghunattr;
    void RefreshRightWoLongLongHun()
    {
        r_wolonglonghun.SetActive(true);
        r_wolonglonghunlGrid.MaxCount = 1;
        UILabel label = r_wolonglonghunlGrid.controlList[0].transform.Find("key").GetComponent<UILabel>();
        if (WLLonghunattr == null)
        {
            WLLonghunattr = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(41));
        }
        int mul = 0;
        if ((data.r_itemCfg.subType % 100) == 1 || (data.r_itemCfg.subType % 100) == 2)
        {
            mul = WLLonghunattr[0][data.r_itemCfg.levClass] / 100;
        }
        else
        {
            mul = WLLonghunattr[1][data.r_itemCfg.levClass] / 100;
        }
        label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1722), StructTipData.Instance.GetEquipPosName(data.r_itemCfg.subType % 100), mul);
        if (openType == TipsOpenType.Bag)
        {
            r_wolongLonghunDes.gameObject.SetActive(true);
            label.color = CSColor.gray;
        }

        if (openType == TipsOpenType.RoleEquip)
        {
            bag.BagItemInfo item = CSBagInfo.Instance.GetEquipedData(((int)ex_data % 100));
            if (item != null && item.quality == 5)
            {
                label.color = CSColor.purple;
                r_wolongLonghunDes.gameObject.SetActive(false);
            }
            else
            {
                r_wolongLonghunDes.gameObject.SetActive(true);
                label.color = CSColor.gray;
            }
        }
        else
        {
            r_wolongLonghunDes.gameObject.SetActive(true);
            label.color = CSColor.gray;
        }
    }
    void RefreshMiddleWoLongLongHun()
    {
        m_wolonglonghun.SetActive(true);
        m_wolonglonghunlGrid.MaxCount = 1;
        UILabel label = m_wolonglonghunlGrid.controlList[0].transform.Find("key").GetComponent<UILabel>();
        if (WLLonghunattr == null)
        {
            WLLonghunattr = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(41));
        }
        int mul = 0;
        if ((data.r_itemCfg.subType % 100) == 1 || (data.r_itemCfg.subType % 100) == 2)
        {
            mul = WLLonghunattr[0][data.r_itemCfg.levClass] / 100;
        }
        else
        {
            mul = WLLonghunattr[1][data.r_itemCfg.levClass] / 100;
        }
        label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1722), StructTipData.Instance.GetEquipPosName(data.m_itemCfg.subType % 100), mul);
        label.color = CSColor.purple;
        m_wolongLonghunDes.gameObject.SetActive(false);
    }
    void RefreshLeftWoLongLongHun()
    {
        l_wolonglonghun.SetActive(true);
        l_wolonglonghunlGrid.MaxCount = 1;
        UILabel label = l_wolonglonghunlGrid.controlList[0].transform.Find("key").GetComponent<UILabel>();
        if (WLLonghunattr == null)
        {
            WLLonghunattr = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(41));
        }
        int mul = 0;
        if ((data.r_itemCfg.subType % 100) == 1 || (data.r_itemCfg.subType % 100) == 2)
        {
            mul = WLLonghunattr[0][data.r_itemCfg.levClass] / 100;
        }
        else
        {
            mul = WLLonghunattr[1][data.r_itemCfg.levClass] / 100;
        }
        label.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1722), StructTipData.Instance.GetEquipPosName(data.l_itemCfg.subType % 100), mul);
        label.color = CSColor.purple;
        l_wolongLonghunDes.gameObject.SetActive(false);
    }
    #endregion
    //卧龙龙技
    void RefreshRightWoLongRandomSkill(UIGridContainer _go, TipDataItem _mes)
    {
        _go.MaxCount = _mes.Properties.Count;
        for (int i = 0; i < _mes.Properties.Count; i++)
        {
            UILabel key = Get<UILabel>("key", _go.controlList[i].transform);
            UILabel value = Get<UILabel>("value", _go.controlList[i].transform);
            key.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1731), _mes.Properties[i].Name);
            key.color = UtilityCsColor.Instance.GetColor(_mes.Properties[i].quality);
        }
    }
    #region 卧龙装备龙力
    void RefreshWolongLi(bag.BagItemInfo _info, GameObject _longLi, GameObject _longLiinten, UIGridContainer _base, UIGridContainer _inten, UITable _table)
    {
        if (_info == null)
        {
            _longLi.SetActive(true);
            _longLiinten.SetActive(false);
            _base.gameObject.SetActive(false);
            _inten.gameObject.SetActive(false);
            r_noWolongHun.gameObject.SetActive(true);
            r_noWolongHun.text = ClientTipsTableManager.Instance.GetClientTipsContext(1747);
            return;
        }
        ShowLongLi(_info.baseAffixs, _info.intensifyAffixs, _longLi, _longLiinten, _base, _inten, _table);
    }
    void ShowLongLi(RepeatedField<bag.WolongRandomEffect> _baseAffix, RepeatedField<bag.WolongRandomEffect> _intenAffix, GameObject _longLi, GameObject _longLiinten, UIGridContainer _base, UIGridContainer _inten, UITable _table)
    {
        if (_baseAffix.Count != 0 || _intenAffix.Count != 0)
        {
            _longLi.SetActive(true);
            _longLiinten.SetActive(true);
            r_noWolongHun.gameObject.SetActive(false);
        }
        else
        {
            _longLi.SetActive(false);
            _longLiinten.SetActive(false);
        }
        _base.MaxCount = _baseAffix.Count;
        _inten.MaxCount = _intenAffix.Count;
        //string BaseAttrStr = "";
        //string IntenAttrStr = "";
        for (int i = 0; i < _base.MaxCount; i++)
        {
            //string str = $"{_baseAffix[i].id}#{_baseAffix[i].effectValue}#{_baseAffix[i].quality}";
            //if ((i + 1) < _base.MaxCount)
            //{
            //    str = $"{str}&";
            //}
            //BaseAttrStr = $"{BaseAttrStr}{str}";
            UILabel name = _base.controlList[i].GetComponent<UILabel>();
            int effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_baseAffix[i].id);
            name.text = SkillTableManager.Instance.GetNameByGroupId(effectId);
        }
        //Debug.Log($"基础词缀   {BaseAttrStr}  {_baseAffix.Count}");
        for (int i = 0; i < _inten.MaxCount; i++)
        {
            bag.WolongRandomEffect _mes = _intenAffix[i];
            //string str1 = $"{_mes.id}#{_mes.effectValue}#{_mes.quality}"; ;
            //if ((i + 1) < _inten.MaxCount)
            //{
            //    str1 = $"{str1}&";
            //}
            //IntenAttrStr = $"{IntenAttrStr}{str1}";
            UILabel des = _inten.controlList[i].transform.Find("key").GetComponent<UILabel>();
            int effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_mes.id);
            int value = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPer(effectId);
            int point = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPoint(effectId);//判断取小数点几位
            string str = $"{_mes.effectValue}";
            //10000的除以100,1000的除以1000
            if (value == 10000)
            {
                str = $"{Math.Round(Convert.ToDecimal(_mes.effectValue * 0.01f), point, MidpointRounding.AwayFromZero)}";
            }
            else if (value == 1000)
            {
                str = $"{Math.Round(Convert.ToDecimal((float)_mes.effectValue / value), point, MidpointRounding.AwayFromZero)}";
            }
            //Debug.Log($"{_mes.id}    {_mes.effectValue}   {effectId}  {value}  {str}");
            des.text = string.Format(ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectDesc(effectId), str);
            des.color = UtilityCsColor.Instance.GetColor(_mes.quality);
        }
        //Debug.Log($"强化词缀   {IntenAttrStr}    {_intenAffix.Count}");
        ILBetterList<int> showedAffix = new ILBetterList<int>(5);
        for (int i = 0; i < _baseAffix.Count; i++)
        {
            if (showedAffix.Contains(_baseAffix[i].id))
            {
                continue;
            }
            int effectId = WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrParameter(_baseAffix[i].id);
            GameObject go = GameObject.Instantiate(r_longLiintenItem, _table.transform);
            //go.transform.SetParent(_longLiinten.transform);
            go.SetActive(true);
            UILabel name = go.transform.Find("des/key").GetComponent<UILabel>();
            UILabel des1 = go.transform.Find("des1/value").GetComponent<UILabel>();
            UILabel des2 = go.transform.Find("des2/value").GetComponent<UILabel>();
            go.transform.localScale = Vector3.one;
            name.text = SkillTableManager.Instance.GetNameByGroupId(effectId);
            int affixCount = CSBagInfo.Instance.GetWoLongLongLiAffixCount(effectId);
            string str = (affixCount >= 12) ? $"[00ff00]({affixCount}/{12})[-]" : $"[ff0000]({affixCount}/{12})[-]";
            des1.text = $"{string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1726), 12)}{str}";

            Dictionary<int, WoLongAffixEffect> paraDic = CSBagInfo.Instance.GetWoLongIntenAffixState(effectId);
            RepeatedField<bag.WolongRandomEffect> mesList = _intenAffix;

            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            string para5 = "";
            for (int k = 1; k <= 5; k++)
            {
                if (paraDic != null)
                {
                    if (paraDic.ContainsKey(k))
                    {
                        WoLongAffixEffect eff = paraDic[k];
                        string AddOrReduce = (eff.plus == 1) ? "+" : "-";
                        string value = eff.value.ToString();
                        int point = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPoint(eff.id);
                        if (eff.per == 10000)
                        {
                            value = Math.Round(Convert.ToDecimal(eff.value * 0.01f), point, MidpointRounding.AwayFromZero).ToString();
                        }
                        else if (eff.per == 1000)
                        {
                            value = Math.Round(Convert.ToDecimal(eff.value * 0.001f), point, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (k == 1)
                        {
                            para1 = $"[00ff00]({AddOrReduce}{value})[-]";
                        }
                        else if (k == 2)
                        {
                            para2 = $"[00ff00]({AddOrReduce}{value})[-]";
                        }
                        else if (k == 3)
                        {
                            para3 = $"[00ff00]({AddOrReduce}{value})[-]";
                        }
                        else if (k == 4)
                        {
                            para4 = $"[00ff00]({AddOrReduce}{value})[-]";
                        }
                        else if (k == 5)
                        {
                            para5 = $"[00ff00]({AddOrReduce}{value})[-]";
                        }
                    }
                }
            }
            des2.text = string.Format(SkillTableManager.Instance.GetDesByGroupId(effectId, CSWoLongInfo.Instance.WoLongEnabledSkillLevel(effectId)), para1, para2, para3, para4, para5);
            showedAffix.Add(_baseAffix[i].id);
        }
        _table.Reposition();
    }
    #endregion

    #region  系统捐献
    void RefreshRightRecycle()
    {
        r_recycle.SetActive(true);
        if (data.r_itemCfg.uniondonate != 0)
        {
            r_recycleDonate.transform.parent.gameObject.SetActive(true);
            r_recycleDonate.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1271), data.r_itemCfg.uniondonate);
        }
        else
        {
            r_recycleDonate.transform.parent.gameObject.SetActive(false);
        }
        LongArray longArrays = data.r_itemCfg.callback;
        if (longArrays.Count <= 0)
        {
            r_recycledes.color = CSColor.red;
            r_recycledes.text = ClientTipsTableManager.Instance.GetClientTipsContext(2004);
        }
        else
        {
            r_recycledes.color = CSColor.green;
            string str = "";
            for (int i = 0; i < longArrays.Count; i++)
            {
                str = $"{str}{ItemTableManager.Instance.GetItemName(longArrays[i].key())}*{longArrays[i].value()}";
                if (i + 1 < longArrays.Count)
                {
                    str = $"{str},";
                }
            }
            r_recycledes.text = str;
        }
    }
    void RefreshLeftRecycle()
    {
        l_recycle.SetActive(true);
        if (data.l_itemCfg.uniondonate != 0)
        {
            l_recycleDonate.transform.parent.gameObject.SetActive(true);
            l_recycleDonate.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1271), data.l_itemCfg.uniondonate);
        }
        else
        {
            l_recycleDonate.transform.parent.gameObject.SetActive(false);
        }
        LongArray longArrays = data.l_itemCfg.callback;

        if (longArrays.Count <= 0)
        {
            l_recycledes.color = CSColor.red;
            l_recycledes.text = ClientTipsTableManager.Instance.GetClientTipsContext(2004);
        }
        else
        {
            l_recycledes.color = CSColor.green;
            string str = "";
            for (int i = 0; i < longArrays.Count; i++)
            {
                str = $"{str}{ItemTableManager.Instance.GetItemName(longArrays[i].key())}*{longArrays[i].value()}";
                if (i + 1 < longArrays.Count)
                {
                    str = $"{str},";
                }
            }
            l_recycledes.text = str;
        }
    }
    void RefreshMiddleRecycle()
    {
        m_recycle.SetActive(true);
        if (data.m_itemCfg.uniondonate != 0)
        {
            m_recycleDonate.transform.parent.gameObject.SetActive(true);
            m_recycleDonate.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1271), data.m_itemCfg.uniondonate);
        }
        else
        {
            m_recycleDonate.transform.parent.gameObject.SetActive(false);
        }
        LongArray longArrays = data.m_itemCfg.callback;

        if (longArrays.Count <= 0)
        {
            m_recycledes.color = CSColor.red;
            m_recycledes.text = ClientTipsTableManager.Instance.GetClientTipsContext(2004);
        }
        else
        {
            m_recycledes.color = CSColor.green;
            string str = "";
            for (int i = 0; i < longArrays.Count; i++)
            {
                str = $"{str}{ItemTableManager.Instance.GetItemName(longArrays[i].key())}*{longArrays[i].value()}";
                if (i + 1 < longArrays.Count)
                {
                    str = $"{str},";
                }
            }
            m_recycledes.text = str;
        }
    }
    #endregion

    #region 箭头
    void RightArrowChange()
    {
        r_arrow_down.SetActive(r_scrollBar.value < 0.95f && sc_scroll_r.shouldMoveVertically);
        //r_arrow_up.SetActive(r_scrollBar.value > 0.05f && sc_scroll_r.shouldMoveVertically);
    }
    void MiddleArrowChange()
    {
        m_arrow_down.SetActive(m_scrollBar.value < 0.95f && sc_scroll_m.shouldMoveVertically);
        //m_arrow_up.SetActive(m_scrollBar.value > 0.05f && sc_scroll_m.shouldMoveVertically);
    }
    void LeftArrowChange()
    {
        l_arrow_down.SetActive(l_scrollBar.value < 0.95f && sc_scroll_l.shouldMoveVertically);
        //l_arrow_up.SetActive(l_scrollBar.value > 0.05f && sc_scroll_l.shouldMoveVertically);
    }
    #endregion
}
