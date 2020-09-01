public partial class UIGuildBagDonatePanel : UIBasePanel
{
	protected UIInput mInputCount;
	protected UIEventListener mBtnreduce;
	protected UIEventListener mBtnadd;
	protected UIEventListener mBtndispose;
	protected UILabel mLabdispose;
	protected UIEventListener mBtncancel;
	protected UILabel mLabitemValue;
	protected UILabel mLabtitle;
	protected UnityEngine.GameObject mItemParent;
	protected UIEventListener mbtnBG;
	protected override void _InitScriptBinder()
	{
		mInputCount = ScriptBinder.GetObject("InputCount") as UIInput;
		mBtnreduce = ScriptBinder.GetObject("Btnreduce") as UIEventListener;
		mBtnadd = ScriptBinder.GetObject("Btnadd") as UIEventListener;
		mBtndispose = ScriptBinder.GetObject("Btndispose") as UIEventListener;
		mLabdispose = ScriptBinder.GetObject("Labdispose") as UILabel;
		mBtncancel = ScriptBinder.GetObject("Btncancel") as UIEventListener;
		mLabitemValue = ScriptBinder.GetObject("LabitemValue") as UILabel;
		mLabtitle = ScriptBinder.GetObject("Labtitle") as UILabel;
		mItemParent = ScriptBinder.GetObject("ItemParent") as UnityEngine.GameObject;
		mbtnBG = ScriptBinder.GetObject("btnBG") as UIEventListener;
	}
}
