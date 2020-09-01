public partial class UISkillCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public enum SkillPanelType
    {
        SPT_SKILL = 1,
        SPT_CONFIG = 2,
    }

    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = this.Close;
        RegChildPanel<UISkillPanel>((int)SkillPanelType.SPT_SKILL, mSkillPanel.gameObject, mTogSkill, CreateSkillPanel);
        RegChildPanel<UISkillPanel>((int)SkillPanelType.SPT_CONFIG, mSkillPanel.gameObject, mTogSkillConfig, CreateSkillPanel);

        RegisterRed(mSkillUpgradeRedPoint,RedPointType.SkillUpgrade);
    }

    UISkillPanel mUISkillPanel = null;
    protected UISkillPanel CreateSkillPanel(int typeId)
    {
        if(null == mUISkillPanel)
        {
            mUISkillPanel = new UISkillPanel();
            mUISkillPanel.UIPrefab = mSkillPanel.gameObject;
            mUISkillPanel.Init();
        }
        mUISkillPanel.Show();
        mUISkillPanel.OnShow(typeId);
        return mUISkillPanel;
    }
}