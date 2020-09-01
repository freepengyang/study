public partial class UIGuildPracticePanel : UIBasePanel
{
	protected UIEventListener mbtn_help;
	protected UIGridContainer mgrid_effects;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mGoPropsRoot;
	protected UIEventListener mbtn_upAttr;
	protected UnityEngine.GameObject mGoMaxLV;
	protected UITexture mTexBg1;
	protected UITexture mTexBg2;
	protected UnityEngine.GameObject mcost_items;
	protected UnityEngine.GameObject mPractiseRedPoint;
	protected override void _InitScriptBinder()
	{
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mGoPropsRoot = ScriptBinder.GetObject("GoPropsRoot") as UnityEngine.GameObject;
		mbtn_upAttr = ScriptBinder.GetObject("btn_upAttr") as UIEventListener;
		mGoMaxLV = ScriptBinder.GetObject("GoMaxLV") as UnityEngine.GameObject;
		mTexBg1 = ScriptBinder.GetObject("TexBg1") as UITexture;
		mTexBg2 = ScriptBinder.GetObject("TexBg2") as UITexture;
		mcost_items = ScriptBinder.GetObject("cost_items") as UnityEngine.GameObject;
		mPractiseRedPoint = ScriptBinder.GetObject("PractiseRedPoint") as UnityEngine.GameObject;
	}
}
