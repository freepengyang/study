public partial class UITeamPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_tab_team;
	protected UIEventListener mbtn_refuseteam;
	protected UIEventListener mbtn_manualteam;
	protected UIEventListener mbtn_automaticteam;
	protected UIEventListener mbtn_myteam;
	protected UIEventListener mbtn_nearbyteam;
	protected UIEventListener mbtn_reqteam;
	protected UnityEngine.GameObject mobj_btn_existteam;
	protected UIEventListener mbtn_releaseteam;
	protected UIEventListener mbtn_invitationteam;
	protected UIEventListener mbtn_quitteam;
	protected UILabel mlb_numberteam;
	protected UnityEngine.GameObject mobj_releaseteam;
	protected UIEventListener mbtn_close_releaseteam;
	protected UIEventListener mbtn_nearby_releaseteam;
	protected UIEventListener mbtn_guild_releaseteam;
	protected UnityEngine.GameObject mobj_btn_nonexistentteam;
	protected UIEventListener mbtn_createteam;
	protected UnityEngine.GameObject mobj_btn_refreshteam;
	protected UIEventListener mbtn_refreshteam;
	protected UIGridContainer mgrid_myteam;
	protected UIGridContainer mgrid_nearbyteam;
	protected UIGridContainer mgrid_reqteam;
	protected UnityEngine.GameObject mobj_nonteam;
	protected UnityEngine.GameObject mScrollViewMyTeam;
	protected UnityEngine.GameObject mScrollViewNearbyTeam;
	protected UnityEngine.GameObject mScrollViewReqTeam;
	protected UnityEngine.GameObject mobj_nonteamNearby;
	protected UnityEngine.GameObject mobj_nonteamReq;
	protected UnityEngine.GameObject mbtn_friends;
	protected UILabel mlb_nostalgiatime;
	protected override void _InitScriptBinder()
	{
		mobj_tab_team = ScriptBinder.GetObject("obj_tab_team") as UnityEngine.GameObject;
		mbtn_refuseteam = ScriptBinder.GetObject("btn_refuseteam") as UIEventListener;
		mbtn_manualteam = ScriptBinder.GetObject("btn_manualteam") as UIEventListener;
		mbtn_automaticteam = ScriptBinder.GetObject("btn_automaticteam") as UIEventListener;
		mbtn_myteam = ScriptBinder.GetObject("btn_myteam") as UIEventListener;
		mbtn_nearbyteam = ScriptBinder.GetObject("btn_nearbyteam") as UIEventListener;
		mbtn_reqteam = ScriptBinder.GetObject("btn_reqteam") as UIEventListener;
		mobj_btn_existteam = ScriptBinder.GetObject("obj_btn_existteam") as UnityEngine.GameObject;
		mbtn_releaseteam = ScriptBinder.GetObject("btn_releaseteam") as UIEventListener;
		mbtn_invitationteam = ScriptBinder.GetObject("btn_invitationteam") as UIEventListener;
		mbtn_quitteam = ScriptBinder.GetObject("btn_quitteam") as UIEventListener;
		mlb_numberteam = ScriptBinder.GetObject("lb_numberteam") as UILabel;
		mobj_releaseteam = ScriptBinder.GetObject("obj_releaseteam") as UnityEngine.GameObject;
		mbtn_close_releaseteam = ScriptBinder.GetObject("btn_close_releaseteam") as UIEventListener;
		mbtn_nearby_releaseteam = ScriptBinder.GetObject("btn_nearby_releaseteam") as UIEventListener;
		mbtn_guild_releaseteam = ScriptBinder.GetObject("btn_guild_releaseteam") as UIEventListener;
		mobj_btn_nonexistentteam = ScriptBinder.GetObject("obj_btn_nonexistentteam") as UnityEngine.GameObject;
		mbtn_createteam = ScriptBinder.GetObject("btn_createteam") as UIEventListener;
		mobj_btn_refreshteam = ScriptBinder.GetObject("obj_btn_refreshteam") as UnityEngine.GameObject;
		mbtn_refreshteam = ScriptBinder.GetObject("btn_refreshteam") as UIEventListener;
		mgrid_myteam = ScriptBinder.GetObject("grid_myteam") as UIGridContainer;
		mgrid_nearbyteam = ScriptBinder.GetObject("grid_nearbyteam") as UIGridContainer;
		mgrid_reqteam = ScriptBinder.GetObject("grid_reqteam") as UIGridContainer;
		mobj_nonteam = ScriptBinder.GetObject("obj_nonteam") as UnityEngine.GameObject;
		mScrollViewMyTeam = ScriptBinder.GetObject("ScrollViewMyTeam") as UnityEngine.GameObject;
		mScrollViewNearbyTeam = ScriptBinder.GetObject("ScrollViewNearbyTeam") as UnityEngine.GameObject;
		mScrollViewReqTeam = ScriptBinder.GetObject("ScrollViewReqTeam") as UnityEngine.GameObject;
		mobj_nonteamNearby = ScriptBinder.GetObject("obj_nonteamNearby") as UnityEngine.GameObject;
		mobj_nonteamReq = ScriptBinder.GetObject("obj_nonteamReq") as UnityEngine.GameObject;
		mbtn_friends = ScriptBinder.GetObject("btn_friends") as UnityEngine.GameObject;
		mlb_nostalgiatime = ScriptBinder.GetObject("lb_nostalgiatime") as UILabel;
	}
}
