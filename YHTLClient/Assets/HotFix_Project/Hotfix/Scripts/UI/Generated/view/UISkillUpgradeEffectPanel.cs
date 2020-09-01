public partial class UISkillUpgradeEffectPanel : UIBasePanel
{
	protected UITable mGridEffects;
	protected UIEventListener mBtnClose;
	protected UISprite mBG;
	protected UIWidget mview;
	protected UISprite mTitle;
	protected UISprite mTail;
	protected override void _InitScriptBinder()
	{
		mGridEffects = ScriptBinder.GetObject("GridEffects") as UITable;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBG = ScriptBinder.GetObject("BG") as UISprite;
		mview = ScriptBinder.GetObject("view") as UIWidget;
		mTitle = ScriptBinder.GetObject("Title") as UISprite;
		mTail = ScriptBinder.GetObject("Tail") as UISprite;
	}
}
