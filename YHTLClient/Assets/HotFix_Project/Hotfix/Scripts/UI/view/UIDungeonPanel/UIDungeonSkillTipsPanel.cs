using System.Collections;
using System.Collections.Generic;
using bag;
using TABLE;
using UnityEngine;

public partial class UIDungeonSkillTipsPanel : UIBase
{
    TABLE.SKILL skillData;
    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.AddEvent(CEvent.LeaveInstance, GetLeaveInstance);
    }

    public void OpenPanel(int skillId)
    {
        SkillTableManager.Instance.TryGetValue(skillId, out skillData);
        if (null != mlb_skill_lv)
            mlb_skill_lv.text = string.Format(mlb_skill_lv.FormatStr, skillData.level);
        if (null != msp_skill_icon)
            msp_skill_icon.spriteName = skillData.icon;
        if (null != mlb_skill_name)
            mlb_skill_name.text = $"{skillData.name.BBCode(ColorType.SecondaryText)}";
        if (null != mlb_skill_desc)
            mlb_skill_desc.text = skillData.description;
        if (null != mlb_skill_cd_time)
            mlb_skill_cd_time.text = string.Format(mlb_skill_cd_time.FormatStr, (skillData.cdTime / 1000).ToString()); ;
        
    }

    private void GetLeaveInstance(uint uiEvtID, object data)
    {
        Close();
    }

    
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Tips; }
    } 
    
    
}