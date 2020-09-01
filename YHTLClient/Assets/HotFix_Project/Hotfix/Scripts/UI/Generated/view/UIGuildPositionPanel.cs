public partial class UIGuildPositionPanel : UIBasePanel
{
	protected UIGridContainer mGrildList;
	protected UnityEngine.GameObject mPostWin;
	protected UILabel mplayername;
	protected UnityEngine.GameObject mSprite;
	protected UIEventListener mBtnClose;
	protected override void _InitScriptBinder()
	{
		mGrildList = ScriptBinder.GetObject("GrildList") as UIGridContainer;
		mPostWin = ScriptBinder.GetObject("PostWin") as UnityEngine.GameObject;
		mplayername = ScriptBinder.GetObject("playername") as UILabel;
		mSprite = ScriptBinder.GetObject("Sprite") as UnityEngine.GameObject;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
	}
}
