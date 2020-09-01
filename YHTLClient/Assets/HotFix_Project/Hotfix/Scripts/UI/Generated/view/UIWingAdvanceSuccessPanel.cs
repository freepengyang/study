public partial class UIWingAdvanceSuccessPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UITable mtable_title;
	protected UILabel mlb_level;
	protected UILabel mlb_name;
	protected UnityEngine.GameObject mitem;
	protected UILabel mlb_effect;
	protected UISprite msp_itemicon;
	protected UISprite mwing;
	protected UnityEngine.GameObject mculet;
	protected UnityEngine.GameObject mtitle;
	protected UnityEngine.GameObject meffect_wing_title_add;
	protected UnityEngine.GameObject meffect_wing_levelup_add;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtable_title = ScriptBinder.GetObject("table_title") as UITable;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mitem = ScriptBinder.GetObject("item") as UnityEngine.GameObject;
		mlb_effect = ScriptBinder.GetObject("lb_effect") as UILabel;
		msp_itemicon = ScriptBinder.GetObject("sp_itemicon") as UISprite;
		mwing = ScriptBinder.GetObject("wing") as UISprite;
		mculet = ScriptBinder.GetObject("culet") as UnityEngine.GameObject;
		mtitle = ScriptBinder.GetObject("title") as UnityEngine.GameObject;
		meffect_wing_title_add = ScriptBinder.GetObject("effect_wing_title_add") as UnityEngine.GameObject;
		meffect_wing_levelup_add = ScriptBinder.GetObject("effect_wing_levelup_add") as UnityEngine.GameObject;
	}
}
