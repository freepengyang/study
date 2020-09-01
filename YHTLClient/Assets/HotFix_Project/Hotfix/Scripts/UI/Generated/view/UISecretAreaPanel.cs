public partial class UISecretAreaPanel : UIBasePanel
{
	protected UIGridContainer mGridTab;
	protected UILabel mLbDesc;
	protected UnityEngine.GameObject mTextBg;
	protected UILabel mLbLevel;
	protected UIGridContainer mGrid;
	protected UILabel mLbTime;
	protected UILabel mLbFree;
	protected UIEventListener mBtnGo;
	protected UILabel mLbBtn;
	protected UISprite mSpIcon;
	protected UILabel mLbValue;
	protected UIEventListener mBtnAdd;
	protected UnityEngine.GameObject mUIItemBar;
	protected UnityEngine.GameObject mLimit;
	protected UIEventListener mSpIconBtn;
	protected override void _InitScriptBinder()
	{
		mGridTab = ScriptBinder.GetObject("GridTab") as UIGridContainer;
		mLbDesc = ScriptBinder.GetObject("LbDesc") as UILabel;
		mTextBg = ScriptBinder.GetObject("TextBg") as UnityEngine.GameObject;
		mLbLevel = ScriptBinder.GetObject("LbLevel") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mLbTime = ScriptBinder.GetObject("LbTime") as UILabel;
		mLbFree = ScriptBinder.GetObject("LbFree") as UILabel;
		mBtnGo = ScriptBinder.GetObject("BtnGo") as UIEventListener;
		mLbBtn = ScriptBinder.GetObject("LbBtn") as UILabel;
		mSpIcon = ScriptBinder.GetObject("SpIcon") as UISprite;
		mLbValue = ScriptBinder.GetObject("LbValue") as UILabel;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UIEventListener;
		mUIItemBar = ScriptBinder.GetObject("UIItemBar") as UnityEngine.GameObject;
		mLimit = ScriptBinder.GetObject("Limit") as UnityEngine.GameObject;
		mSpIconBtn = ScriptBinder.GetObject("SpIconBtn") as UIEventListener;
	}
}
