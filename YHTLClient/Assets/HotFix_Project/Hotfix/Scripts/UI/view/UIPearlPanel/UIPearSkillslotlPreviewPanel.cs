using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIPearSkillslotlPreviewPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get
        {
            return false;
        }
    }
    
    private PearlData myPearlData = null;
    private List<SkillSlotData> listSkillSlots = null;
    private Dictionary<int, List<int>> dicSkills = null;
    /// <summary>
    /// 当前选中技能槽的所有技能列表
    /// </summary>
    private List<int> listSkills = null;

    private int selectIndex = 0;
    private int lastSelectIndex = 0;

    public override void Init()
    {
        base.Init();
        mbtn_bg.onClick = Close;
        mbtn_close.onClick = Close;
    }
    
    public override void Show()
    {
        base.Show();
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null) return;
        listSkillSlots = CSPearlInfo.Instance.MyPearlData.ListSkillSlotDatas;
        SetDicSkills();
        InitData();
    }

    void SetDicSkills()
    {
        dicSkills = new Dictionary<int, List<int>>();
        dicSkills.Clear();
        for (int i = 0; i < listSkillSlots.Count; i++)
        {
            List<int> listSkill = new List<int>();
            listSkill.Clear();
            var arr = BaoZhuSkillLibTableManager.Instance.array.gItem.handles;
            for(int k = 0,max = arr.Length;i < max;++i)
            {
                var item = arr[k].Value as TABLE.BAOZHUSKILLLIB;
                if (item.libID==listSkillSlots[i].LibID)
                {
                    listSkill.Add(item.skill);
                }
            }
            dicSkills.Add(listSkillSlots[i].SkillSlotId, listSkill);
        }
    }

    void InitData()
    {
        if (listSkillSlots!=null&&listSkillSlots.Count>0)
        {
            InitGrid();
            SetRightInfo(selectIndex);
        }
    }

    void InitGrid()
    {
        mgrid_catalog.MaxCount = listSkillSlots.Count;
        GameObject gp = null;
        UILabel lb_content;
        GameObject choose;
        for (int i = 0; i < mgrid_catalog.MaxCount; i++)
        {
            gp = mgrid_catalog.controlList[i];
            lb_content = gp.transform.Find("lb_content").gameObject.GetComponent<UILabel>();
            choose = gp.transform.Find("choose").gameObject;
            lb_content.text = CSString.Format(947, listSkillSlots[i].SkillSlotId);
            choose.SetActive(i==selectIndex);
            UIEventListener.Get(gp, i).onClick = OnClickItem;
        }
    }

    void OnClickItem(GameObject go)
    {
        if (go == null) return;
        int index = (int)UIEventListener.Get(go).parameter;
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        RefreshItem(selectIndex);
        RefreshItem(lastSelectIndex);
        SetRightInfo(index);
    }
    
    void RefreshItem(int index)
    {
        GameObject gp;
        GameObject choose;
        gp = mgrid_catalog.controlList[index];
        choose = gp.transform.Find("choose").gameObject;
        choose.SetActive(index==selectIndex);
    }

    void SetRightInfo(int index)
    {
        listSkills = dicSkills[listSkillSlots[index].SkillSlotId];
        mgrid_skill.MaxCount = listSkills.Count;
        GameObject gp;
        UISprite sp_itemicon;
        UILabel lb_itemName;
        for (int i = 0; i < mgrid_skill.MaxCount; i++)
        {
            gp = mgrid_skill.controlList[i];
            sp_itemicon = gp.transform.Find("sp_itemicon").gameObject.GetComponent<UISprite>();
            lb_itemName = gp.transform.Find("lb_itemName").gameObject.GetComponent<UILabel>();

            sp_itemicon.spriteName = SkillTableManager.Instance.GetSkillIcon(listSkills[i]);
            lb_itemName.text = SkillTableManager.Instance.GetSkillName(listSkills[i]);
            UIEventListener.Get(gp, i).onClick = OnClickSkillItem;
        }
    }

    void OnClickSkillItem(GameObject go)
    {
        if (go == null) return;
        int index = (int)UIEventListener.Get(go).parameter;
        int skillId = listSkills[index];
        Utility.OpenSkillTipPanel(skillId);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
