public partial class UIGuildListGroupPanel : UIBasePanel
{
	protected UIGrid mTogGroup;
	protected UIToggle mtg_memberList;
	protected UIToggle mtg_applyList;
	protected UIToggle mtg_familyList;
	protected UnityEngine.GameObject mGuilApplyListPanel;
	protected UnityEngine.GameObject mGuildMemberListPanel;
	protected UnityEngine.GameObject mGuildListPanel;
	protected UnityEngine.GameObject mApplyListRedPoint;
	protected UnityEngine.GameObject mFamilyListRedPoint;
	protected override void _InitScriptBinder()
	{
		mTogGroup = ScriptBinder.GetObject("TogGroup") as UIGrid;
		mtg_memberList = ScriptBinder.GetObject("tg_memberList") as UIToggle;
		mtg_applyList = ScriptBinder.GetObject("tg_applyList") as UIToggle;
		mtg_familyList = ScriptBinder.GetObject("tg_familyList") as UIToggle;
		mGuilApplyListPanel = ScriptBinder.GetObject("GuilApplyListPanel") as UnityEngine.GameObject;
		mGuildMemberListPanel = ScriptBinder.GetObject("GuildMemberListPanel") as UnityEngine.GameObject;
		mGuildListPanel = ScriptBinder.GetObject("GuildListPanel") as UnityEngine.GameObject;
		mApplyListRedPoint = ScriptBinder.GetObject("ApplyListRedPoint") as UnityEngine.GameObject;
		mFamilyListRedPoint = ScriptBinder.GetObject("FamilyListRedPoint") as UnityEngine.GameObject;
	}
}
