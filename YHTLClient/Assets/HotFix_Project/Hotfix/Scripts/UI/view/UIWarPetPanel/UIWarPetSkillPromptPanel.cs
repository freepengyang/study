using UnityEngine;

public partial class UIWarPetSkillPromptPanel : UIBasePanel
{
    private ILBetterList<TABLE.SKILL> listWarPetSkills;

    public override void Init()
    {
        base.Init();
        AddCollider();
        mbtn_close.onClick = Close;
        mbar.onChange.Add(new EventDelegate(OnChange));
        listWarPetSkills = CSWarPetRefineInfo.Instance.AllPreviewSkills;
    }

    void OnChange()
    {
        int maxCount = mgrid_skill.MaxPerLine * 3;
        msp_scroll.SetActive(mbar.value < 0.95 && mgrid_skill.MaxCount > maxCount);
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        mgrid_skill.MaxCount = listWarPetSkills.Count;
        GameObject gp;
        ScriptBinder scriptBinder;
        UILabel lb_name;
        UISprite sp_name;
        for (int i = 0; i < mgrid_skill.MaxCount; i++)
        {
            gp = mgrid_skill.controlList[i];
            scriptBinder = gp.GetComponent<ScriptBinder>();
            lb_name = scriptBinder.GetObject("lb_name") as UILabel;
            sp_name = scriptBinder.GetObject("sp_name") as UISprite;

            TABLE.SKILL skill = listWarPetSkills[i];
            lb_name.text = skill.name;
            sp_name.spriteName = skill.icon;
            UIEventListener.Get(gp, i).onClick = OnClickSkill;
        }

        int maxCount = mgrid_skill.MaxPerLine * 3;
        msp_scroll.SetActive(mgrid_skill.MaxCount > maxCount);
        msp_bg.height = mgrid_skill.MaxCount > maxCount
            ? 436
            : 436 - Mathf.FloorToInt((maxCount - mgrid_skill.MaxCount) * 1f / mgrid_skill.MaxPerLine) *
            (int) mgrid_skill.CellHeight;
    }

    void OnClickSkill(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        int skillId = listWarPetSkills[index].id;
        Utility.OpenUIWarPetSkillTipsPanel(skillId, SkillTipsAdaptiveType.BottomRight/*, go.transform.GetChild(1).gameObject*/);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}