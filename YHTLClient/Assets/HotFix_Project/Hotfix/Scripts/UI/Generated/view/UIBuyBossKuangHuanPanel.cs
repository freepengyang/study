public partial class UIBuyBossKuangHuanPanel : UIBasePanel
{
	protected UILabel mLbItemName;
	protected UIEventListener mBtnBuy;
	protected UIEventListener mBtnClose;
	protected UnityEngine.GameObject mUIItemBarPrefab;
	protected UnityEngine.GameObject mUIItemBarTotal;
	protected UnityEngine.Transform mItemBase;
	protected override void _InitScriptBinder()
	{
		mLbItemName = ScriptBinder.GetObject("LbItemName") as UILabel;
		mBtnBuy = ScriptBinder.GetObject("BtnBuy") as UIEventListener;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mUIItemBarPrefab = ScriptBinder.GetObject("UIItemBarPrefab") as UnityEngine.GameObject;
		mUIItemBarTotal = ScriptBinder.GetObject("UIItemBarTotal") as UnityEngine.GameObject;
		mItemBase = ScriptBinder.GetObject("ItemBase") as UnityEngine.Transform;
	}
}
