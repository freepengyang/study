public partial class UITimeExpHighStageEffectPanel : UIBasePanel
{
	protected UIGridContainer mGridEffects;
	protected UIEventListener mBtnClose;
	protected UISprite mSprite;
	protected UISprite mBG;
	protected override void _InitScriptBinder()
	{
		mGridEffects = ScriptBinder.GetObject("GridEffects") as UIGridContainer;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mSprite = ScriptBinder.GetObject("Sprite") as UISprite;
		mBG = ScriptBinder.GetObject("BG") as UISprite;
	}
}
