public partial class UIGuilApplyListPanel : UIBasePanel
{
	protected UIGridContainer mGrildList;
	protected UILabel mapply_settings_text;
	protected UIEventListener mapply_settings_options;
	protected UISprite mapply_settings_arrow;
	protected UIGridContainer mapply_settings_grid;
	protected UIEventListener mbtn_allagree;
	protected UIEventListener mbtn_alldisagree;
	protected override void _InitScriptBinder()
	{
		mGrildList = ScriptBinder.GetObject("GrildList") as UIGridContainer;
		mapply_settings_text = ScriptBinder.GetObject("apply_settings_text") as UILabel;
		mapply_settings_options = ScriptBinder.GetObject("apply_settings_options") as UIEventListener;
		mapply_settings_arrow = ScriptBinder.GetObject("apply_settings_arrow") as UISprite;
		mapply_settings_grid = ScriptBinder.GetObject("apply_settings_grid") as UIGridContainer;
		mbtn_allagree = ScriptBinder.GetObject("btn_allagree") as UIEventListener;
		mbtn_alldisagree = ScriptBinder.GetObject("btn_alldisagree") as UIEventListener;
	}
}
