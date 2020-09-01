public partial class UIConfigBasePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_selectRole;
	protected UnityEngine.GameObject mbtn_login;
	protected UnityEngine.GameObject mbtn_resetAll;
	protected UIToggle mtg_fixJoystick;
	protected UIToggle mtg_pushAct;
	protected UIToggle mtg_forbidFriend;
	protected UIToggle mtg_forbidSociety;
	protected UIToggle mtg_rejectStranger;
	protected UISlider mslider_bgm;
	protected UISlider mslider_effect;
	protected UISlider mslider_voice;
	protected UIToggle mtg_bgm;
	protected UIToggle mtg_effect;
	protected UIToggle mtg_voice;
	protected override void _InitScriptBinder()
	{
		mbtn_selectRole = ScriptBinder.GetObject("btn_selectRole") as UnityEngine.GameObject;
		mbtn_login = ScriptBinder.GetObject("btn_login") as UnityEngine.GameObject;
		mbtn_resetAll = ScriptBinder.GetObject("btn_resetAll") as UnityEngine.GameObject;
		mtg_fixJoystick = ScriptBinder.GetObject("tg_fixJoystick") as UIToggle;
		mtg_pushAct = ScriptBinder.GetObject("tg_pushAct") as UIToggle;
		mtg_forbidFriend = ScriptBinder.GetObject("tg_forbidFriend") as UIToggle;
		mtg_forbidSociety = ScriptBinder.GetObject("tg_forbidSociety") as UIToggle;
		mtg_rejectStranger = ScriptBinder.GetObject("tg_rejectStranger") as UIToggle;
		mslider_bgm = ScriptBinder.GetObject("slider_bgm") as UISlider;
		mslider_effect = ScriptBinder.GetObject("slider_effect") as UISlider;
		mslider_voice = ScriptBinder.GetObject("slider_voice") as UISlider;
		mtg_bgm = ScriptBinder.GetObject("tg_bgm") as UIToggle;
		mtg_effect = ScriptBinder.GetObject("tg_effect") as UIToggle;
		mtg_voice = ScriptBinder.GetObject("tg_voice") as UIToggle;
	}
}
