using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UISeekTreasureCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    public enum ChildPanelType
    {
        CPT_SeekTreasure = 1,//寻宝
        CPT_Warehouse = 2,//仓库
        CPT_Exchange = 3,//兑换
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
        mbtn_close.onClick = Close;
        mbtn_bg.onClick = Close;
        RegChildPanel<UISeekTreasurePanel>((int)ChildPanelType.CPT_SeekTreasure, mUISeekTreasurePanel, mbtn_seektreasure);
        RegChildPanel<UISeekTreasureWarehousePanel>((int)ChildPanelType.CPT_Warehouse, mUISeekTreasureWarehousePanel, mbtn_stroehouse);
        RegChildPanel<UISeekTreasureExchangePanel>((int)ChildPanelType.CPT_Exchange, mUISeekTreasureExchangePanel, mbtn_exchange);
        
        RegisterRed(mredpoint_stroehouse, RedPointType.SeekTreasureWarehouse);
    }
}