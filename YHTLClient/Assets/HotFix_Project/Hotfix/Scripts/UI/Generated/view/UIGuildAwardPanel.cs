public partial class UIGuildAwardPanel : UIBasePanel
{
	protected UIScrollView mMainScrollView;
	protected UIScrollView mSecondScrollView;
	protected UIGrid mMainGrid;
	protected UIGrid mSecondGrid;
	protected UnityEngine.GameObject mRole;
	protected UnityEngine.GameObject mWeapon;
	protected UnityEngine.GameObject mCloth;
	protected UnityEngine.GameObject mTaizi;
	protected UnityEngine.GameObject mtex_bg;
	protected UIEventListener mbtn_help;
	protected override void _InitScriptBinder()
	{
		mMainScrollView = ScriptBinder.GetObject("MainScrollView") as UIScrollView;
		mSecondScrollView = ScriptBinder.GetObject("SecondScrollView") as UIScrollView;
		mMainGrid = ScriptBinder.GetObject("MainGrid") as UIGrid;
		mSecondGrid = ScriptBinder.GetObject("SecondGrid") as UIGrid;
		mRole = ScriptBinder.GetObject("Role") as UnityEngine.GameObject;
		mWeapon = ScriptBinder.GetObject("Weapon") as UnityEngine.GameObject;
		mCloth = ScriptBinder.GetObject("Cloth") as UnityEngine.GameObject;
		mTaizi = ScriptBinder.GetObject("Taizi") as UnityEngine.GameObject;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
	}
}
