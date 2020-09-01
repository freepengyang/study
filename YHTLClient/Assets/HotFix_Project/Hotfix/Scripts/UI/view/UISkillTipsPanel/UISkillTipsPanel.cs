using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class UISkillTipsPanel : UIBasePanel
{
    public static void CreateHelpTipsPanel(int _id)
    {
        UIManager.Instance.CreatePanel<UISkillTipsPanel>(panel =>
        {
            if (panel is UISkillTipsPanel helpTipPanel)
            {
                helpTipPanel.ShowContent(_id);
            }
        });
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Show()
    {
        Panel.depth = 102;
        base.Show();
        Panel.alpha = 0;
    }

    void ShowContent(int _id)
    {
        //mlb_name.text = SkillTableManager.Instance.GetNameByGroupId(_id);
        mlb_des.text = $"{SkillTableManager.Instance.GetNameByGroupId(_id)}[-]：{SkillTableManager.Instance.GetDesByGroupId(_id)}";
        msp_bg.height = mlb_des.height + 24;
        UIEventListener.Get(mobj_mask).onClick = (p) => { UIManager.Instance.ClosePanel<UISkillTipsPanel>(); };
        if (Panel) Panel.alpha = 1;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        mlb_name = null;
        msp_bg = null;
        mlb_des = null;
        mobj_mask = null;
    }
}
