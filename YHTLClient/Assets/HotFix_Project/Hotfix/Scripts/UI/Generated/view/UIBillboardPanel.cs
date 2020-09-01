public partial class UIBillboardPanel : UIBasePanel
{
	protected UILabel mlb_content;
	protected UIEventListener mbtn_know;
	protected UILabel mlb_btnKnow;
	protected TweenAlpha mtweenAlpha;
	protected UnityEngine.GameObject mobj_center;
	protected override void _InitScriptBinder()
	{
		mlb_content = ScriptBinder.GetObject("lb_content") as UILabel;
		mbtn_know = ScriptBinder.GetObject("btn_know") as UIEventListener;
		mlb_btnKnow = ScriptBinder.GetObject("lb_btnKnow") as UILabel;
		mtweenAlpha = ScriptBinder.GetObject("tweenAlpha") as TweenAlpha;
		mobj_center = ScriptBinder.GetObject("obj_center") as UnityEngine.GameObject;
	}
}
