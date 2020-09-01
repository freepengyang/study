using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class UINostalgiaEquipPanel : UIBasePanel
{
    private enum Type {
        UIJewlryBox = 1, 
        NostalgiaUpLevel = 2,
    }
    
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    public override void Init()
    {
        base.Init();
        RegChildPanel<UIJewelryBoxPanel>((int)Type.UIJewlryBox, mUIJewelryBoxPanel, mbtn_jewelry);
        RegChildPanel<UINostalgiaUpLevelPanel>((int)Type.NostalgiaUpLevel, mUINostalgiaUpLevelPanel, mbtn_levelup);
        
        RegisterRed(mbtn_jewelry.transform.Find("redpoint").gameObject, RedPointType.None);
        RegisterRed(mbtn_levelup.transform.Find("redpoint").gameObject, RedPointType.None);
        UIEventListener.Get(mbtn_close).onClick = Close;
    }
    
    public override void Show()
    {
        base.Show();
        SetMoneyIds(1, 4);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    {
        UIBasePanel basePanel;
        if (type == 2)
        {
            basePanel = base.OpenChildPanel(type, fromToggle);
            if (basePanel is UINostalgiaUpLevelPanel panel)
            {
                panel.OpenPanel();
            }
        }
        else
        {
            basePanel =  base.OpenChildPanel(type, fromToggle);
        }

        return basePanel;
    }

}