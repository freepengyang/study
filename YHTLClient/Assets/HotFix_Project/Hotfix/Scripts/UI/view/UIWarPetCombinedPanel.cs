public partial class UIWarPetCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public override void Init()
	{
		base.Init();
        mBtnClose.onClick = Close;
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenStateChange);
        RegChildPanel<UIWarPetSkillPanel>((int)ChildPanelType.CPT_SKILL, mWarPetSkillPanel, mTogSkill);
        RegChildPanel<UIWarSoulPanel>((int)ChildPanelType.CPT_WARSOUL, mUIWarSoulPanel, mTogWarsoul);
        RegChildPanel<UIWarPetRefinePanel>((int)ChildPanelType.CPT_Refine, mUIWarPetRefinePanel, mTogRefine);
        RegChildPanel<UIPetTalentPanel>((int)ChildPanelType.CPT_TALENT, mUIPetTalentPanel, mTogTalent);

        FuncOpenBind();
        RegisterRed(mSkillRedPoint,RedPointType.PetSkillUpgrade);
        RegisterRed(mTalentRedPoint, RedPointType.PetTalent);
        RegisterRed(mRefineRedpoint, RedPointType.PetRefine);

        if (mgrid_Group != null)
        {
	        mgrid_Group.repositionNow = true;
	        mgrid_Group.Reposition();
        }
        SetMoneyIds(1,4);
	}
	
	public override void Show()
	{
		base.Show();
		FunctionOpenStateChange(0, null);
	}
	
	void FuncOpenBind()
	{
		UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_WarPetRefine, mbtn_refine.gameObject);
		UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_WarPetSkill, mbtn_skill.gameObject);
	}


	void FunctionOpenStateChange(uint id, object data)
	{
		if (mgrid_Group != null)
		{
			mgrid_Group.repositionNow = true;
			mgrid_Group.Reposition();
		}
	}


	public override void SelectChildPanel(int type, int subType)
	{
		if (type == (int)ChildPanelType.CPT_WARSOUL)
		{
			UIWarSoulPanel p = OpenChildPanel(type) as UIWarSoulPanel;
			p.SetItemClick(subType);
		}
		else
			OpenChildPanel(type);
	}
	public enum ChildPanelType
	{
		CPT_SKILL = 1,
		CPT_WARSOUL = 2,
		CPT_Refine = 3,
		CPT_TALENT = 4,
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}