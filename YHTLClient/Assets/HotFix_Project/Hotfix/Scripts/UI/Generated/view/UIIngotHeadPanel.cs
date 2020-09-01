public partial class UIIngotHeadPanel : UIBasePanel
{
	protected UIEventListener mBtnIcon;
	protected UISprite mSpHp;
	protected UnityEngine.GameObject mUIIngotHeadPanel;
	protected override void _InitScriptBinder()
	{
		mBtnIcon = ScriptBinder.GetObject("BtnIcon") as UIEventListener;
		mSpHp = ScriptBinder.GetObject("SpHp") as UISprite;
		mUIIngotHeadPanel = ScriptBinder.GetObject("UIIngotHeadPanel") as UnityEngine.GameObject;
	}
}
