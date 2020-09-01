public partial class UIGetItemPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_equip;
	protected UILabel mTitle;
	protected UILabel mName;
	protected UILabel mFightPoint;
	protected UnityEngine.GameObject mItemParent;
	protected UnityEngine.GameObject mBg;
	protected UILabel mUsage;
	protected UILabel mTitleUsage;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_equip = ScriptBinder.GetObject("btn_equip") as UIEventListener;
		mTitle = ScriptBinder.GetObject("Title") as UILabel;
		mName = ScriptBinder.GetObject("Name") as UILabel;
		mFightPoint = ScriptBinder.GetObject("FightPoint") as UILabel;
		mItemParent = ScriptBinder.GetObject("ItemParent") as UnityEngine.GameObject;
		mBg = ScriptBinder.GetObject("Bg") as UnityEngine.GameObject;
		mUsage = ScriptBinder.GetObject("Usage") as UILabel;
		mTitleUsage = ScriptBinder.GetObject("TitleUsage") as UILabel;
	}
}
