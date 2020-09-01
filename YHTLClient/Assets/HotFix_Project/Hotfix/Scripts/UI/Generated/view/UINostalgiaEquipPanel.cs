public partial class UINostalgiaEquipPanel : UIBasePanel
{
	protected UIToggle mbtn_levelup;
	protected UIToggle mbtn_jewelry;
	protected UnityEngine.GameObject mUIJewelryBoxPanel;
	protected UnityEngine.GameObject mUINostalgiaUpLevelPanel;
	protected UnityEngine.GameObject mbtn_close;
	protected override void _InitScriptBinder()
	{
		mbtn_levelup = ScriptBinder.GetObject("btn_levelup") as UIToggle;
		mbtn_jewelry = ScriptBinder.GetObject("btn_jewelry") as UIToggle;
		mUIJewelryBoxPanel = ScriptBinder.GetObject("UIJewelryBoxPanel") as UnityEngine.GameObject;
		mUINostalgiaUpLevelPanel = ScriptBinder.GetObject("UINostalgiaUpLevelPanel") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
	}
}
