using System.Collections.Generic;
using UnityEngine;

public partial class UIGuildEndActivityPanel : UIBasePanel
{

    List<UIItemBase> itembase = new List<UIItemBase>();

    public override void Init()
	{
		base.Init();
        AddCollider();
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect, "effect_instance_success_add");
    }
	
	public override void Show()
	{
		base.Show();
	}


    public void OpenPanel(Dictionary<int, int> rewards, int rank, int score)
    {
        if (rewards != null)
        {
            Utility.GetItemByBoxid(rewards, mGrid, ref itembase);
        }

        mlb_rank.text = rank.ToString();
        mlb_score.text = score.ToString();
    }

	
	protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(meffect);
        UIItemManager.Instance.RecycleItemsFormMediator(itembase);
        base.OnDestroy();
	}


}
