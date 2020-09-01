public partial class UIGuildFightRulePanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();

		mbtn_close.onClick = this.Close;
		UIEventListener.Get(mobj_bg).onClick = this.Close;
        CSEffectPlayMgr.Instance.ShowUITexture(msand_bg, "guildfight_sand_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(msand, "guildfight_sand");
        CSEffectPlayMgr.Instance.ShowUITexture(msand_line, "guildfight_sand_line");
		CSEffectPlayMgr.Instance.ShowUITexture(msandfight_road, "guildfight_road");
	}
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(msand_bg);
        CSEffectPlayMgr.Instance.Recycle(msand);
        CSEffectPlayMgr.Instance.Recycle(msand_line);
		CSEffectPlayMgr.Instance.Recycle(msandfight_road);
		base.OnDestroy();
	}
}
