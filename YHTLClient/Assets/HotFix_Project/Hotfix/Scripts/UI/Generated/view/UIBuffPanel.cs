public partial class UIBuffPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mobj_nonbuff;
	protected UIGridContainer mgrid_buff;
	protected UISprite msp_bg;
	protected UIScrollView mScrollView_buff;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mobj_nonbuff = ScriptBinder.GetObject("obj_nonbuff") as UnityEngine.GameObject;
		mgrid_buff = ScriptBinder.GetObject("grid_buff") as UIGridContainer;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mScrollView_buff = ScriptBinder.GetObject("ScrollView_buff") as UIScrollView;
	}
}
