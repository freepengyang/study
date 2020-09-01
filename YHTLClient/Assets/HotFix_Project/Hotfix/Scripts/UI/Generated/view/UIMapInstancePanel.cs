public partial class UIMapInstancePanel : UIBasePanel
{
	protected UILabel mLbTitle;
	protected UILabel mLbBossNum;
	protected UILabel mLbMonsterNum;
	protected UILabel mLbTip;
	protected override void _InitScriptBinder()
	{
		mLbTitle = ScriptBinder.GetObject("LbTitle") as UILabel;
		mLbBossNum = ScriptBinder.GetObject("LbBossNum") as UILabel;
		mLbMonsterNum = ScriptBinder.GetObject("LbMonsterNum") as UILabel;
		mLbTip = ScriptBinder.GetObject("LbTip") as UILabel;
	}
}
