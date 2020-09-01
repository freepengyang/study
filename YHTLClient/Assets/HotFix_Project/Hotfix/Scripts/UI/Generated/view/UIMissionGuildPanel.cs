public partial class UIMissionGuildPanel : UIBasePanel
{
	protected UnityEngine.Transform mright;
	protected UnityEngine.GameObject mEffect;
	protected UILabel mlb_content;
	protected UnityEngine.GameObject mEffectArrow;
	protected override void _InitScriptBinder()
	{
		mright = ScriptBinder.GetObject("right") as UnityEngine.Transform;
		mEffect = ScriptBinder.GetObject("Effect") as UnityEngine.GameObject;
		mlb_content = ScriptBinder.GetObject("lb_content") as UILabel;
		mEffectArrow = ScriptBinder.GetObject("EffectArrow") as UnityEngine.GameObject;
	}
}
