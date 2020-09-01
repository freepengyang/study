public partial class UIGuildCombinedPanel : UIBasePanel
{
	protected UnityEngine.GameObject mGuildInfoPanel;
	protected UIEventListener mbtn_close;
	protected UIGrid mToggleGroup;
	protected UIToggle mTogInfo;
	protected UIToggle mTogHouse;
	protected UIToggle mTogPractice;
	protected UIToggle mTogRedPkg;
	protected UnityEngine.GameObject mGuildListPanel;
	protected UIToggle mTogList;
	protected UnityEngine.GameObject mGuildListGroupPanel;
	protected UnityEngine.GameObject mGuildBagPanel;
	protected UnityEngine.GameObject mGuildPracticePanel;
	protected UnityEngine.GameObject mGuildWealPanel;
	protected UIEventListener mbtn_help;
	protected UITexture mtexBg;
	protected UILabel mListTabName;
	protected UILabel mListTabCheckName;
	protected UnityEngine.GameObject mPractiseRedPoint;
	protected UnityEngine.GameObject mMemberRedPoint;
	protected UnityEngine.GameObject mUIGuildActivityPanel ;
	protected UIToggle mtg_Activity;
	protected override void _InitScriptBinder()
	{
		mGuildInfoPanel = ScriptBinder.GetObject("GuildInfoPanel") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mTogInfo = ScriptBinder.GetObject("TogInfo") as UIToggle;
		mTogHouse = ScriptBinder.GetObject("TogHouse") as UIToggle;
		mTogPractice = ScriptBinder.GetObject("TogPractice") as UIToggle;
		mTogRedPkg = ScriptBinder.GetObject("TogRedPkg") as UIToggle;
		mGuildListPanel = ScriptBinder.GetObject("GuildListPanel") as UnityEngine.GameObject;
		mTogList = ScriptBinder.GetObject("TogList") as UIToggle;
		mGuildListGroupPanel = ScriptBinder.GetObject("GuildListGroupPanel") as UnityEngine.GameObject;
		mGuildBagPanel = ScriptBinder.GetObject("GuildBagPanel") as UnityEngine.GameObject;
		mGuildPracticePanel = ScriptBinder.GetObject("GuildPracticePanel") as UnityEngine.GameObject;
		mGuildWealPanel = ScriptBinder.GetObject("GuildWealPanel") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mtexBg = ScriptBinder.GetObject("texBg") as UITexture;
		mListTabName = ScriptBinder.GetObject("ListTabName") as UILabel;
		mListTabCheckName = ScriptBinder.GetObject("ListTabCheckName") as UILabel;
		mPractiseRedPoint = ScriptBinder.GetObject("PractiseRedPoint") as UnityEngine.GameObject;
		mMemberRedPoint = ScriptBinder.GetObject("MemberRedPoint") as UnityEngine.GameObject;
		mUIGuildActivityPanel  = ScriptBinder.GetObject("UIGuildActivityPanel ") as UnityEngine.GameObject;
		mtg_Activity = ScriptBinder.GetObject("tg_Activity") as UIToggle;
	}
}
