public partial class UIServerActivityRechargePanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected UILabel mLbMoney;
	protected UnityEngine.GameObject mBtnGet;
	protected UnityEngine.GameObject mBGBanner10;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mLbMoney = ScriptBinder.GetObject("LbMoney") as UILabel;
		mBtnGet = ScriptBinder.GetObject("BtnGet") as UnityEngine.GameObject;
		mBGBanner10 = ScriptBinder.GetObject("BGBanner10") as UnityEngine.GameObject;
	}
}
