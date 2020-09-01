public partial class UIDealPkValuePanel : UIBasePanel
{
	protected UILabel mlb_title;
	protected UILabel mlb_say;
	protected UnityEngine.GameObject mbtnClose;
	protected UnityEngine.GameObject mbtn_Exchange;
	protected UnityEngine.GameObject mbtn_Leave;
	protected UnityEngine.GameObject mobj_itemPar;
	protected UILabel mlb_count;
	protected override void _InitScriptBinder()
	{
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mlb_say = ScriptBinder.GetObject("lb_say") as UILabel;
		mbtnClose = ScriptBinder.GetObject("btnClose") as UnityEngine.GameObject;
		mbtn_Exchange = ScriptBinder.GetObject("btn_Exchange") as UnityEngine.GameObject;
		mbtn_Leave = ScriptBinder.GetObject("btn_Leave") as UnityEngine.GameObject;
		mobj_itemPar = ScriptBinder.GetObject("obj_itemPar") as UnityEngine.GameObject;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
	}
}
