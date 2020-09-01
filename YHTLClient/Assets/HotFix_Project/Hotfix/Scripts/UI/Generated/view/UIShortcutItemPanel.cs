public partial class UIShortcutItemPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_arrow;
	protected UnityEngine.GameObject mbtn_quickUse;
	protected UISprite msp_icon;
	protected UILabel mlb_curNum;
	protected UISprite mcdmask;
	protected UILabel mcdtime;
	protected UnityEngine.GameObject mobj_bubble;
	protected UILabel mlb_bubble;
	protected UIGridContainer mgrid;
	protected override void _InitScriptBinder()
	{
		mbtn_arrow = ScriptBinder.GetObject("btn_arrow") as UnityEngine.GameObject;
		mbtn_quickUse = ScriptBinder.GetObject("btn_quickUse") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_curNum = ScriptBinder.GetObject("lb_curNum") as UILabel;
		mcdmask = ScriptBinder.GetObject("cdmask") as UISprite;
		mcdtime = ScriptBinder.GetObject("cdtime") as UILabel;
		mobj_bubble = ScriptBinder.GetObject("obj_bubble") as UnityEngine.GameObject;
		mlb_bubble = ScriptBinder.GetObject("lb_bubble") as UILabel;
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
	}
}
