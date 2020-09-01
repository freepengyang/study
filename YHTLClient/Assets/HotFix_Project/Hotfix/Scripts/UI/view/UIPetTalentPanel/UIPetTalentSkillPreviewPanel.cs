using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class UIPetTalentSkillPreviewPanel : UIBasePanel
{

    ILBetterList<UITalentSkillPreviewBinder> rightBinderList = new ILBetterList<UITalentSkillPreviewBinder>();
    ILBetterList<TABLE.SKILL> showSkillList = new ILBetterList<TABLE.SKILL>();


    public override void Init()
	{
		base.Init();
        AddCollider();

        if (rightBinderList == null) rightBinderList = new ILBetterList<UITalentSkillPreviewBinder>();
        for (int i = 0; i < 4; i++)//最多同时有四个技能预览
        {
            UITalentSkillPreviewBinder binder = mPoolHandleManager.GetSystemClass<UITalentSkillPreviewBinder>();
            rightBinderList.Add(binder);
        }
    }
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
        if (rightBinderList != null)
        {
            for (int i = 0; i < rightBinderList.Count; i++)
            {
                rightBinderList[i].Destroy();
            }
            rightBinderList.Clear();
            rightBinderList = null;
        }

        showSkillList?.Clear();
        showSkillList = null;

        base.OnDestroy();
	}

    
    public void Refresh(TABLE.CHONGWUHEXIN cfg)
    {
        if (cfg == null) return;
        if (showSkillList == null) showSkillList = new ILBetterList<TABLE.SKILL>();
        showSkillList.Clear();

        TABLE.SKILL temp = GetSkill(cfg.para1);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para2);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para3);
        if (temp != null) showSkillList.Add(temp);
        temp = GetSkill(cfg.para4);
        if (temp != null) showSkillList.Add(temp);

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

        int height = 65 + (int)NGUIMath.CalculateRelativeWidgetBounds(mTable_right.transform, false).size.y;
        msp_rightBg.height = height > 348 ? 348 : height;
    }


    
    TABLE.SKILL GetSkill(int group)
    {
        TABLE.SKILL temp = null;
        if (group == 0) return null;
        SkillTableManager.Instance.TryGetValue(group * 1000 + 1, out temp);
        return temp;
    }
}
