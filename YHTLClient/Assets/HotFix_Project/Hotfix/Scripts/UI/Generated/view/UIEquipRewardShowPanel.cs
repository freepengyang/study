public partial class UIEquipRewardShowPanel : UIBasePanel
{
	protected UILabel mLbEquipName;
	protected UIGridContainer mGrid;
	protected UIEventListener mBtnClose;
	protected UIEventListener mBtnGo;
	protected UIToggle mTog;
	protected override void _InitScriptBinder()
	{
		mLbEquipName = ScriptBinder.GetObject("LbEquipName") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBtnGo = ScriptBinder.GetObject("BtnGo") as UIEventListener;
		mTog = ScriptBinder.GetObject("Tog") as UIToggle;
	}
}
