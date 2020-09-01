public partial class UIServerActivityMapPanel : UIBasePanel
{
	protected UnityEngine.GameObject mBanner13;
	protected UIEventListener mBtnMoney;
	protected UIEventListener mBtnEnter;
	protected UISprite mSpIcon;
	protected UILabel mLbValue;
	protected UnityEngine.GameObject mBtnAdd;
	protected UIEventListener mIconTips;
	protected UIGridContainer mGrid;
	protected override void _InitScriptBinder()
	{
		mBanner13 = ScriptBinder.GetObject("Banner13") as UnityEngine.GameObject;
		mBtnMoney = ScriptBinder.GetObject("BtnMoney") as UIEventListener;
		mBtnEnter = ScriptBinder.GetObject("BtnEnter") as UIEventListener;
		mSpIcon = ScriptBinder.GetObject("SpIcon") as UISprite;
		mLbValue = ScriptBinder.GetObject("LbValue") as UILabel;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UnityEngine.GameObject;
		mIconTips = ScriptBinder.GetObject("IconTips") as UIEventListener;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
	}
}
