public partial class UITeamInvitationPanel : UIBasePanel
{
	protected UIEventListener mbtn_nearby;
	protected UIEventListener mbtn_friend;
	protected UIEventListener mbtn_guild;
	protected UIGridContainer mgrid_reqteam;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_close1;
	protected UnityEngine.GameObject mlb_tips1;
	protected UnityEngine.GameObject mlb_tips2;
	protected UnityEngine.GameObject mlb_tips3;
	protected override void _InitScriptBinder()
	{
		mbtn_nearby = ScriptBinder.GetObject("btn_nearby") as UIEventListener;
		mbtn_friend = ScriptBinder.GetObject("btn_friend") as UIEventListener;
		mbtn_guild = ScriptBinder.GetObject("btn_guild") as UIEventListener;
		mgrid_reqteam = ScriptBinder.GetObject("grid_reqteam") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_close1 = ScriptBinder.GetObject("btn_close1") as UIEventListener;
		mlb_tips1 = ScriptBinder.GetObject("lb_tips1") as UnityEngine.GameObject;
		mlb_tips2 = ScriptBinder.GetObject("lb_tips2") as UnityEngine.GameObject;
		mlb_tips3 = ScriptBinder.GetObject("lb_tips3") as UnityEngine.GameObject;
	}
}
