using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIPearlCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public enum ChildPanelType
    {
        CPT_Upgrade = 1,//升级
        CPT_Evolution = 2,//进化
        CPT_Skillslot = 3,//技能槽
    }

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.GradeUpBaoZhu, RefreshData);
        AddCollider();
        mbtn_close.onClick = Close;
        RegChildPanel<UIPearlUpgradePanel>((int)ChildPanelType.CPT_Upgrade, mUIPearlUpgradePanel, mtog_upgrade);
        RegChildPanel<UIPearlEvolutionPanel>((int)ChildPanelType.CPT_Evolution, mUIPearlEvolutionPanel, mtog_evolution);
        RegChildPanel<UIPearlSkillslotPanel>((int)ChildPanelType.CPT_Skillslot, mUIPearlSkillslotPanel, mtog_skillslot);
        
        RegisterRed(mred_evolution, RedPointType.PearlEvolution);
        RegisterRed(mred_skillslot, RedPointType.PearlSkillslot);
    }

    public override void Show()
    {
        base.Show();
        bool isActive = false;
        ILBetterList<bag.BagItemInfo> list = CSPearlInfo.Instance.MyPearlData.ListPearl;
        if (list.Count>0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].gemGrade>1)
                {
                    isActive = true;
                    break;
                }
            }
        }
        mbtn_skillslot.SetActive(isActive);
    }
    
    void RefreshData(uint id, object data)
    {
        if (!mbtn_skillslot.activeSelf)
        {
            PearlData pearlData = CSPearlInfo.Instance.MyPearlData;
            for (int i = 0; i < pearlData.ListSkillSlotDatas.Count ; i++)
            {
                if ( pearlData.ListSkillSlotDatas[i].IsUnlock)
                {
                    mbtn_skillslot.SetActive(true);
                    break;
                }
            }
        }
    }
}