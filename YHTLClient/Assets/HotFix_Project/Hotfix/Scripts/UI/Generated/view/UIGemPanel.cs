public partial class UIGemPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mbtn_quickbuy;
	protected UnityEngine.GameObject mbtn_suit;
	protected UnityEngine.GameObject mbtn_wear;
	protected UnityEngine.GameObject mLingshiList;
	protected UISprite mshowIcon;
	protected UIGrid mPropGrid;
	protected UIGridContainer mgird_GemPart;
	protected UnityEngine.GameObject mtrans_choose;
	protected UISprite mspr_Icon;
	protected UILabel mlb_totalsuit;
	protected UITexture mtex_bg;
	protected UnityEngine.GameObject mobj_wearRed;
	protected UnityEngine.GameObject mobj_idleEffect;
	protected UnityEngine.GameObject mobj_NoAttr;
	protected UnityEngine.GameObject mobj_Attr;
	protected UILabel mlb_name;
	protected override void _InitScriptBinder()
	{
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mbtn_quickbuy = ScriptBinder.GetObject("btn_quickbuy") as UnityEngine.GameObject;
		mbtn_suit = ScriptBinder.GetObject("btn_suit") as UnityEngine.GameObject;
		mbtn_wear = ScriptBinder.GetObject("btn_wear") as UnityEngine.GameObject;
		mLingshiList = ScriptBinder.GetObject("LingshiList") as UnityEngine.GameObject;
		mshowIcon = ScriptBinder.GetObject("showIcon") as UISprite;
		mPropGrid = ScriptBinder.GetObject("PropGrid") as UIGrid;
		mgird_GemPart = ScriptBinder.GetObject("gird_GemPart") as UIGridContainer;
		mtrans_choose = ScriptBinder.GetObject("trans_choose") as UnityEngine.GameObject;
		mspr_Icon = ScriptBinder.GetObject("spr_Icon") as UISprite;
		mlb_totalsuit = ScriptBinder.GetObject("lb_totalsuit") as UILabel;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UITexture;
		mobj_wearRed = ScriptBinder.GetObject("obj_wearRed") as UnityEngine.GameObject;
		mobj_idleEffect = ScriptBinder.GetObject("obj_idleEffect") as UnityEngine.GameObject;
		mobj_NoAttr = ScriptBinder.GetObject("obj_NoAttr") as UnityEngine.GameObject;
		mobj_Attr = ScriptBinder.GetObject("obj_Attr") as UnityEngine.GameObject;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
	}
}
