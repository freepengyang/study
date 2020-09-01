public partial class UIDungeonSiegeInstancePanel : UIBasePanel
{
	protected UILabel mlb_count;
	protected UILabel mlb_integral;
	protected UnityEngine.GameObject mbtn_Inspire;
	protected UILabel mlb_hint;
	protected UILabel mlb_finish;
	protected UIGrid mgrid_reward;
	protected UnityEngine.GameObject mobj_inspireEffect;
	protected UnityEngine.GameObject mItemBase;
	protected UISprite msp_quality;
	protected UISprite msp_itemicon;
	protected UILabel mlb_itemcount;
	protected override void _InitScriptBinder()
	{
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mlb_integral = ScriptBinder.GetObject("lb_integral") as UILabel;
		mbtn_Inspire = ScriptBinder.GetObject("btn_Inspire") as UnityEngine.GameObject;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mlb_finish = ScriptBinder.GetObject("lb_finish") as UILabel;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGrid;
		mobj_inspireEffect = ScriptBinder.GetObject("obj_inspireEffect") as UnityEngine.GameObject;
		mItemBase = ScriptBinder.GetObject("ItemBase") as UnityEngine.GameObject;
		msp_quality = ScriptBinder.GetObject("sp_quality") as UISprite;
		msp_itemicon = ScriptBinder.GetObject("sp_itemicon") as UISprite;
		mlb_itemcount = ScriptBinder.GetObject("lb_itemcount") as UILabel;
	}
}
