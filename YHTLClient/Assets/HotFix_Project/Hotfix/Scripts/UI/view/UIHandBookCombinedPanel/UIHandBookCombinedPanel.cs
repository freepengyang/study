using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHandBookCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    
    public enum ChildPanelType
    {
        CPT_SETUP = 1,
        //CPT_PACKAGE = 2,
        CPT_UPGRADE = 3,
        CPT_MERGE = 4,
        CPT_BOOKMARK = 5,
    }

    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = this.Close;
        RegChildPanel<UIHandBookSetupPanel>((int)ChildPanelType.CPT_SETUP,mHandBookSetupHandle.gameObject, mTogSetup);
        //RegChildPanel<UIHandBookPackagePanel>((int)ChildPanelType.CPT_PACKAGE, mHandBookPackagePanel.gameObject, null);
        RegChildPanel<UIHandBookUpgradePanel>((int)ChildPanelType.CPT_UPGRADE, mHandBookUpgradePanel.gameObject, mTogUpgrade);
        RegChildPanel<UIHandBookMergePanel>((int)ChildPanelType.CPT_MERGE, mHandBookMergePanel.gameObject, mTogMerge);
        RegChildPanel<UIHandBookMarkPanel>((int)ChildPanelType.CPT_BOOKMARK, mUIHandBookMarkPanel.gameObject, mTogHandBook);

        RegisterRedList(mSetupRedPoint,RedPointType.HandBookSetuped,RedPointType.HandBookSlotUnlock);
        RegisterRed(mUpgradeLevelRedPoint, RedPointType.HandBookUpgradeLevel);
        RegisterRed(mUpgradeQualityRedPoint, RedPointType.HandBookUpgradeQuality);

        mClientEvent.AddEvent(CEvent.OnHandBookTabChanged, OnHandBookTabChanged);
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnHandBookTabChanged, OnHandBookTabChanged);
        base.OnDestroy();
    }

    protected void OnHandBookTabChanged(uint id,object argv)
    {
        if(argv is object[] argvs && argvs.Length == 2)
        {
            if (argvs[0] is ChildPanelType childPanelType && argvs[1] is HandBookSlotData slotData)
            {
                if(childPanelType == ChildPanelType.CPT_UPGRADE)
                {
                    UIHandBookUpgradePanel p = OpenChildPanel((int)childPanelType) as UIHandBookUpgradePanel;
                    if(null != p)
                    {
                        p.Bind(slotData);
                    }
                    return;
                }
                if (childPanelType == ChildPanelType.CPT_MERGE)
                {
                    UIHandBookMergePanel p = OpenChildPanel((int)childPanelType) as UIHandBookMergePanel;
                    if (null != p)
                    {
                        p.Bind(slotData);
                    }
                    return;
                }
                OpenChildPanel((int)childPanelType);
            }
        }
    }
}