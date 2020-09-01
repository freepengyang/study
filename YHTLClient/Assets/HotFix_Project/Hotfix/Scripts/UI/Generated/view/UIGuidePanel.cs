public partial class UIGuidePanel : UIBasePanel
{
	protected UISprite mArrow;
	protected UnityEngine.GameObject mEffect;
	protected UnityEngine.GameObject mTips;
	protected override void _InitScriptBinder()
	{
		mArrow = ScriptBinder.GetObject("Arrow") as UISprite;
		mEffect = ScriptBinder.GetObject("Effect") as UnityEngine.GameObject;
		mTips = ScriptBinder.GetObject("Tips") as UnityEngine.GameObject;
	}
}
