using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailySignInPromptPanel : UIDialog
{
	protected UILabel mlb_days;
	protected UILabel mlb_reached;
	protected UILabel mlb_exchanged;
	protected UIGrid mGrid_reward;
    protected GameObject mobj_effect;

    private List<UIItemBase> rewardItemList;

    protected override void _InitScriptBinder()
	{
		mlb_days = ScriptBinder.GetObject("lb_days") as UILabel;
		mlb_reached = ScriptBinder.GetObject("lb_reached") as UILabel;
		mlb_exchanged = ScriptBinder.GetObject("lb_exchanged") as UILabel;
		mGrid_reward = ScriptBinder.GetObject("Grid_reward") as UIGrid;
        mobj_effect = ScriptBinder.GetObject("obj_effect") as GameObject;
    }


    public override void Init()
    {
        base.Init();
        AddCollider();

        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, 17750);
    }


    public override void Show()
    {
        base.Show();
        RefreshUI();
    }


    void RefreshUI()
    {
        UltimateAchievementData data = CSSignCardInfo.Instance.GetUltimateData();
        if (data == null) return;

        mlb_days.text = data.spendDays.ToString();
        mlb_reached.text = data.reachedCount.ToString();
        mlb_exchanged.text = data.exchangePiecesCount.ToString();

        rewardItemList = UIItemManager.Instance.GetUIItems(data.honorRewardDic.Count, PropItemType.Normal, mGrid_reward.transform);
        if (rewardItemList != null)
        {
            int i = 0;
            for (data.honorRewardDic.Begin();data.honorRewardDic.Next();)
            {
                rewardItemList[i].Refresh(data.honorRewardDic.Key, ItemClick);
                rewardItemList[i].SetCount(data.honorRewardDic.Value, CSColor.white);
                i++;
            }
            mGrid_reward.Reposition();
        }
    }


    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        if (rewardItemList != null && rewardItemList.Count > 0)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(rewardItemList);
        }
        base.OnDestroy();
    }

}
