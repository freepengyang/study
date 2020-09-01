using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIPetTalentPreviewPanel : UIBasePanel
{

    FastArrayElementKeepHandle<CSTalentData> coreList;

    CSBetterLisHot<TABLE.SKILL> showSkillList = new CSBetterLisHot<TABLE.SKILL>();
    CSTalentCore curSslectCore;

    EndLessKeepHandleList<UITalentPreviewBinder, CSTalentData> endLessList;

    ILBetterList<UITalentSkillPreviewBinder> rightBinderList = new ILBetterList<UITalentSkillPreviewBinder>();

    public override void Init()
	{
		base.Init();
        AddCollider();

        mClientEvent.AddEvent(CEvent.PetTalentCorePreviewSelect, PreviewCoreSelecte);
	}
	
	public override void Show()
	{
		base.Show();

        coreList = CSPetTalentInfo.Instance.CoreTalentData;
        curSslectCore = null;

        RefreshLeftUI();
        mobj_panelRight.CustomActive(false);
    }
	
	protected override void OnDestroy()
	{
        //mGrid_left.UnBind<UITalentPreviewBinder>();
        //mGrid_right.UnBind<UITalentSkillPreviewBinder>();
        if (rightBinderList != null)
        {
            for (int i = 0; i < rightBinderList.Count; i++)
            {
                rightBinderList[i].Destroy();
            }
            rightBinderList.Clear();
            rightBinderList = null;
        }
        coreList = null;
        showSkillList?.Clear();
        showSkillList = null;
        curSslectCore = null;

        endLessList?.Destroy();
        endLessList = null;

        base.OnDestroy();
	}


    void InitRightBinder()
    {
        if (rightBinderList == null) rightBinderList = new ILBetterList<UITalentSkillPreviewBinder>();
        for (int i = 0; i < 4; i++)//最多同时有四个技能预览
        {
            UITalentSkillPreviewBinder binder = mPoolHandleManager.GetSystemClass<UITalentSkillPreviewBinder>();
            rightBinderList.Add(binder);
        }
    }


    void RefreshLeftUI()
    {
        //mGrid_left.Bind<CSTalentData, UITalentPreviewBinder>(coreList, mPoolHandleManager);
        if (endLessList == null)
        {
            endLessList = new EndLessKeepHandleList<UITalentPreviewBinder, CSTalentData>(SortType.Vertical, mwrap_left, mPoolHandleManager, 12, ScriptBinder);
        }
        endLessList.Clear();

        if (coreList == null) return;
        for (int i = 0; i < coreList.Count; i++)
        {
            endLessList.Append(coreList[i]);
        }

        endLessList.Bind();

        int length = mwrap_left.itemSize * coreList.Count;
        mscroll_left.ScrollImmidate(0);
        mscroll_left.SetDynamicArrowVerticalWithWrap(length, msp_scroll);
    }


    void RefreshRightUI()
    {
        if (curSslectCore == null || curSslectCore.config == null) return;

        if (showSkillList == null) showSkillList = new CSBetterLisHot<TABLE.SKILL>();
        else showSkillList.Clear();
        TABLE.CHONGWUHEXIN cfg = curSslectCore.config;
        TABLE.SKILL temp = GetSkill(cfg.para1);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para2);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para3);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para4);
        if (temp != null) showSkillList.Add(temp);

        mobj_panelRight.CustomActive(showSkillList.Count > 0);

        if (showSkillList.Count <= 0) return;

        int needTableCount = showSkillList.Count - mTable_right.GetChildList().Count;
        if (needTableCount > 0)
        {
            for (int i = 0; i < needTableCount; i++)
            {
                GameObject.Instantiate(mtableChild, mTable_right.transform);
            }
        }

        var tableList = mTable_right.GetChildList();
        for (int i = 0; i < tableList.Count; i++)
        {
            var obj = tableList[i].gameObject;
            obj.CustomActive(i < showSkillList.Count);
            if (i >= showSkillList.Count) continue;
            UITalentSkillPreviewBinder binder = null;
            if (i >= rightBinderList.Count)
            {
                binder = mPoolHandleManager.GetSystemClass<UITalentSkillPreviewBinder>();
                rightBinderList.Add(binder);
            }
            else binder = rightBinderList[i];

            if (binder != null)
            {
                var handle = UIEventListener.Get(obj);
                binder.Setup(handle);
                binder.Bind(showSkillList[i]);
            }
        }
        mTable_right.Reposition();
        //mGrid_right.Bind<TABLE.SKILL, UITalentSkillPreviewBinder>(showSkillList, mPoolHandleManager);

        int height = 65 + (int)NGUIMath.CalculateRelativeWidgetBounds(mTable_right.transform, false).size.y;
        msp_rightBg.height = height > 348 ? 348 : height;
    }


    void PreviewCoreSelecte(uint id, object param)
    {
        CSTalentCore data = param as CSTalentCore;
        if (data == null || data.config == null) return;
        curSslectCore = data;
        RefreshRightUI();
    }


    TABLE.SKILL GetSkill(int group)
    {
        TABLE.SKILL temp = null;
        if (group == 0) return null;
        SkillTableManager.Instance.TryGetValue(group * 1000 + 1, out temp);
        return temp;
    }

}



public class UITalentPreviewBinder : UIBinder
{
    protected UILabel lb_name;
    protected UILabel lb_value;

    protected CSTalentData mData;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("name");
        lb_value = Get<UILabel>("value");

    }

    public override void Bind(object data)
    {
        if (lb_name == null || lb_value == null) return;
        lb_name.text = "空数据";
        lb_value.text = "检查表格";
        mData = data as CSTalentData;
        if (mData == null || mData.config == null || mData.config.type != 2) return;

        if (string.IsNullOrEmpty(mData.desc))
        {
            mData.SetDesc();
        }

        if (string.IsNullOrEmpty(mData.desc)) return;

        bool isActive = CSPetTalentInfo.Instance.CurActivatedPoint >= mData.Id;

        string colorA = isActive ? UtilityColor.MainText : UtilityColor.WeakText;
        string colorB = isActive ? UtilityColor.Green : UtilityColor.WeakText;
        string newStr = mData.desc.Replace("[u=skill]", $"[u]{colorB}");

        string talentLvStr = CSPetTalentInfo.Instance.talentLvStr;
        lb_name.text = $"{colorA}{CSString.Format(talentLvStr, mData.Id)}";

        lb_value.text = $"{colorA}{newStr}";

        if (newStr.Contains("[u]"))
        {
            UIEventListener.Get(lb_value.gameObject).onClick = OnClick;
        }
    }

    public override void OnDestroy()
    {
        mData = null;
        lb_name = null;
        lb_value = null;
    }


    void OnClick(GameObject go)
    {
        if (mData == null || mData.linkedCore == null || mData.linkedCore.config == null) return;
        HotManager.Instance.EventHandler.SendEvent(CEvent.PetTalentCorePreviewSelect, mData.linkedCore);
    }

}


public class UITalentSkillPreviewBinder : UITalentPreviewBinder
{
    TABLE.SKILL config;

    Transform trans_line;

    public override void Init(UIEventListener handle)
    {
        base.Init(handle);
        trans_line = Get<Transform>("line");
    }


    public override void Bind(object data)
    {
        config = data as TABLE.SKILL;
        if (config == null) return;

        if (lb_name != null)
        {
            lb_name.text = $"{UtilityColor.MainText}{config.name}";
        }

        if (lb_value != null)
        {
            lb_value.text = $"{UtilityColor.SubTitle}{config.description}";
            if (trans_line != null)
            {
                float offset = lb_value.height + 6.5f;
                trans_line.localPosition = new Vector3(47, 0 - offset, 0);
            }
        }
               
            
    }


    public override void OnDestroy()
    {
        config = null;
        trans_line = null;
        base.OnDestroy();
    }

}

