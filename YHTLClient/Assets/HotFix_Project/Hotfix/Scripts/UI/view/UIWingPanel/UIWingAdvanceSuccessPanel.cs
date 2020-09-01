using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWingAdvanceSuccessPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
    }
    
    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mtitle, "wing_label_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(mculet, "wing_levelup");
        
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect_wing_title_add, 17803,20);
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect_wing_levelup_add,17802);
        
        // CSEffectPlayMgr.Instance.ShowUIEffect(mwing.gameObject, "901009", ResourceType.UIWing);
        
        TABLE.WING wing;
        if (WingTableManager.Instance.TryGetValue(CSWingInfo.Instance.MyWingData.id, out wing))
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mwing.gameObject, wing.model.ToString(), ResourceType.UIWing, 6);
            int num = (int)wing.rank;//现在的阶数
            mlb_level.text = num.ToString();
            mtable_title.repositionNow = true;
            mtable_title.Reposition();
            mlb_name.text = CSString.Format(712, num);
            mlb_name.gameObject.SetActive(wing.rankType==3);
            mitem.SetActive(wing.rankType==3);
            if (wing.rankType==3)
            {
                TABLE.SKILL skill;
                if (SkillTableManager.Instance.TryGetValue((int) wing.rankNum, out skill))
                {
                    msp_itemicon.spriteName = skill.icon;
                    mlb_effect.text = $"{skill.name}{CSString.Format(999)}{skill.description}";
                }
            }
        }
        
    }
    
    /// <summary>
    /// 打开升阶完成界面
    /// </summary>
    /// <param name="data">升阶后的翅膀信息</param>
    // public void OpenWingAdvanceSuccess(wing.WingInfo data)
    // {
    //     if (mapWing == null||data==null) return;
    //     TABLE.WING wing;
    //     if (mapWing.TryGetValue(data.wingId, out wing))
    //     {
    //         int num = (int)wing.rank;//现在的阶数
    //         mlb_level.text = num.ToString();
    //         mtable_title.repositionNow = true;
    //         mtable_title.Reposition();
    //         mlb_name.text = CSString.Format(712, num);
    //         mlb_name.gameObject.SetActive(wing.rankType==3);
    //         mitem.SetActive(wing.rankType==3);
    //         CSEffectPlayMgr.Instance.ShowUIEffect(mwing.gameObject, wing.model.ToString(), ResourceType.UIWing);
    //         // CSEffectPlayMgr.Instance.ShowUIEffect(mwing.gameObject, "901009", ResourceType.UIWing);
    //         if (wing.rankType==3)
    //         {
    //             TABLE.SKILL skill;
    //             if (SkillTableManager.Instance.dic.TryGetValue((int) wing.rankNum, out skill))
    //             {
    //                 msp_itemicon.spriteName = skill.icon;
    //                 mlb_effect.text = $"{skill.name}{CSString.Format(999)}{skill.description}";
    //             }
    //         }
    //     }
    // }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CSEffectPlayMgr.Instance.Recycle(meffect_wing_title_add);
        CSEffectPlayMgr.Instance.Recycle(meffect_wing_levelup_add);
        CSEffectPlayMgr.Instance.Recycle(mwing.gameObject);
    }
}