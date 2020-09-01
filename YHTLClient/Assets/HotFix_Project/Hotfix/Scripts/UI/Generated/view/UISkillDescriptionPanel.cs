public partial class UISkillDescriptionPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UISprite mBG;
	protected UIWidget mview;
	protected UISprite mTitle;
	protected UISprite mTail;
	protected UILabel mskillName;
	protected UILabel mDescription;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBG = ScriptBinder.GetObject("BG") as UISprite;
		mview = ScriptBinder.GetObject("view") as UIWidget;
		mTitle = ScriptBinder.GetObject("Title") as UISprite;
		mTail = ScriptBinder.GetObject("Tail") as UISprite;
		mskillName = ScriptBinder.GetObject("skillName") as UILabel;
		mDescription = ScriptBinder.GetObject("Description") as UILabel;
	}
}
