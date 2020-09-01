public partial class UIFunctionPanel : UIBasePanel
{
	protected TweenRotation mleftRotation;
	protected TweenRotation mrightRotation;
	protected UnityEngine.GameObject mbtn_texleftbg;
	protected UnityEngine.GameObject mbtn_texrightbg;
	protected UnityEngine.Transform mbtn_leftIcons;
	protected UnityEngine.Transform mbtn_rightIcons;
	protected UnityEngine.GameObject mshowEffect;
	protected CSInvoke mbgFour;
	protected override void _InitScriptBinder()
	{
		mleftRotation = ScriptBinder.GetObject("leftRotation") as TweenRotation;
		mrightRotation = ScriptBinder.GetObject("rightRotation") as TweenRotation;
		mbtn_texleftbg = ScriptBinder.GetObject("btn_texleftbg") as UnityEngine.GameObject;
		mbtn_texrightbg = ScriptBinder.GetObject("btn_texrightbg") as UnityEngine.GameObject;
		mbtn_leftIcons = ScriptBinder.GetObject("btn_leftIcons") as UnityEngine.Transform;
		mbtn_rightIcons = ScriptBinder.GetObject("btn_rightIcons") as UnityEngine.Transform;
		mshowEffect = ScriptBinder.GetObject("showEffect") as UnityEngine.GameObject;
		mbgFour = ScriptBinder.GetObject("bgFour") as CSInvoke;
	}
}
