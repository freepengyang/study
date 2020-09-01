public partial class UILongLiPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_noEquip;
	protected UnityEngine.GameObject mobj_SelfEqiup;
	protected UnityEngine.GameObject mbtn_waerEquipsHL;
	protected UnityEngine.GameObject mobj_BagEquip;
	protected UnityEngine.GameObject mbtn_bagEquipsHL;
	protected UIGridContainer mgrid_BagEquip;
	protected UIGridContainer mgrid_SelfEquip;
	protected UnityEngine.GameObject mtex_bg;
	protected UnityEngine.GameObject mbtn_waerEquips;
	protected UnityEngine.GameObject mbtn_bagEquips;
	protected UnityEngine.Transform mtrans_itemshowPar;
	protected UILabel mlb_showitemName;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mobj_cost;
	protected UnityEngine.GameObject mobj_getway;
	protected UnityEngine.GameObject mbtn_refine;
	protected UILabel mlb_moneyNum;
	protected UISprite msp_moneyIcon;
	protected UnityEngine.GameObject mbtn_moneyBuy;
	protected UILabel mlb_goodsNum;
	protected UISprite msp_goodsIcon;
	protected UnityEngine.GameObject mbtn_goodsBuy;
	protected UIGridContainer mgrid_intenAffix;
	protected UIGridContainer mgrid_baseAffix;
	protected UnityEngine.GameObject mobj_bgs;
	protected UnityEngine.GameObject mobj_selfRed;
	protected UILabel mlb_noEquipDes;
	protected UnityEngine.GameObject mbubble;
	protected override void _InitScriptBinder()
	{
		mobj_noEquip = ScriptBinder.GetObject("obj_noEquip") as UnityEngine.GameObject;
		mobj_SelfEqiup = ScriptBinder.GetObject("obj_SelfEqiup") as UnityEngine.GameObject;
		mbtn_waerEquipsHL = ScriptBinder.GetObject("btn_waerEquipsHL") as UnityEngine.GameObject;
		mobj_BagEquip = ScriptBinder.GetObject("obj_BagEquip") as UnityEngine.GameObject;
		mbtn_bagEquipsHL = ScriptBinder.GetObject("btn_bagEquipsHL") as UnityEngine.GameObject;
		mgrid_BagEquip = ScriptBinder.GetObject("grid_BagEquip") as UIGridContainer;
		mgrid_SelfEquip = ScriptBinder.GetObject("grid_SelfEquip") as UIGridContainer;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mbtn_waerEquips = ScriptBinder.GetObject("btn_waerEquips") as UnityEngine.GameObject;
		mbtn_bagEquips = ScriptBinder.GetObject("btn_bagEquips") as UnityEngine.GameObject;
		mtrans_itemshowPar = ScriptBinder.GetObject("trans_itemshowPar") as UnityEngine.Transform;
		mlb_showitemName = ScriptBinder.GetObject("lb_showitemName") as UILabel;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mobj_cost = ScriptBinder.GetObject("obj_cost") as UnityEngine.GameObject;
		mobj_getway = ScriptBinder.GetObject("obj_getway") as UnityEngine.GameObject;
		mbtn_refine = ScriptBinder.GetObject("btn_refine") as UnityEngine.GameObject;
		mlb_moneyNum = ScriptBinder.GetObject("lb_moneyNum") as UILabel;
		msp_moneyIcon = ScriptBinder.GetObject("sp_moneyIcon") as UISprite;
		mbtn_moneyBuy = ScriptBinder.GetObject("btn_moneyBuy") as UnityEngine.GameObject;
		mlb_goodsNum = ScriptBinder.GetObject("lb_goodsNum") as UILabel;
		msp_goodsIcon = ScriptBinder.GetObject("sp_goodsIcon") as UISprite;
		mbtn_goodsBuy = ScriptBinder.GetObject("btn_goodsBuy") as UnityEngine.GameObject;
		mgrid_intenAffix = ScriptBinder.GetObject("grid_intenAffix") as UIGridContainer;
		mgrid_baseAffix = ScriptBinder.GetObject("grid_baseAffix") as UIGridContainer;
		mobj_bgs = ScriptBinder.GetObject("obj_bgs") as UnityEngine.GameObject;
		mobj_selfRed = ScriptBinder.GetObject("obj_selfRed") as UnityEngine.GameObject;
		mlb_noEquipDes = ScriptBinder.GetObject("lb_noEquipDes") as UILabel;
		mbubble = ScriptBinder.GetObject("bubble") as UnityEngine.GameObject;
	}
}