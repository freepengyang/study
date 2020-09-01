public partial class UIChooseGuildPanel : UIBasePanel
{
	protected UnityEngine.GameObject mItemPrefab;
	protected UIGrid mContent;
	protected UIEventListener mbtn_allapply;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mItemPrefab = ScriptBinder.GetObject("ItemPrefab") as UnityEngine.GameObject;
		mContent = ScriptBinder.GetObject("Content") as UIGrid;
		mbtn_allapply = ScriptBinder.GetObject("btn_allapply") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
