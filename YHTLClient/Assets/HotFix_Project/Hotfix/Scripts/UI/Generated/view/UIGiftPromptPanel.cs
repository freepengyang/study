public partial class UIGiftPromptPanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected UnityEngine.GameObject mobj_effect;
	protected UITexture mTexbg;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
		mTexbg = ScriptBinder.GetObject("Texbg") as UITexture;
	}
}
