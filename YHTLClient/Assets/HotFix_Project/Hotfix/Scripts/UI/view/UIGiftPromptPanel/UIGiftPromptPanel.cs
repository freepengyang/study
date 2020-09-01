using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGiftPromptPanel : UIBasePanel
{
    List<UIItemBase> uiItemBaselist = new List<UIItemBase> ();
    
	public override void Init()
	{
		base.Init();
        AddCollider();
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect,"effect_instance_success_add");
    }

    public void OpenPanel(Dictionary<int,int> BoxReward) {
        Utility.GetItemByBoxid(BoxReward, mGrid,ref uiItemBaselist,itemSize.Size70);
        
        for (int i = 0; i < uiItemBaselist.Count; i++)
        {
	        int id = uiItemBaselist[i].itemCfg.id;
	        int num = 0;
	        if (BoxReward.ContainsKey(id))
	        {
		        num = BoxReward[id];
	        }
	        uiItemBaselist[i].SetCount(num.ToString(),Color.white); 
        }
        
        //自适应格子宽高
        if ( BoxReward.Count > 5 )
	        mGrid.MaxPerLine = BoxReward.Count / 2 + 1;    
        else
	        mGrid.MaxPerLine = BoxReward.Count;
        int lineNum = Mathf.CeilToInt((float) BoxReward.Count / mGrid.MaxPerLine); 
        
        mTexbg.height = 115 * lineNum;

        
    }



	protected override void OnDestroy()
	{
		base.OnDestroy();
		
		if (uiItemBaselist != null)
		{
			UIItemManager.Instance.RecycleItemsFormMediator(uiItemBaselist);
			uiItemBaselist = null;
		}
		CSEffectPlayMgr.Instance.Recycle(mobj_effect);
	}
	
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
}
