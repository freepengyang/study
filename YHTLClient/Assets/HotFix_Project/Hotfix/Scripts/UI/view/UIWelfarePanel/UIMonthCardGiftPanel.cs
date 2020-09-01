using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIMonthCardGiftPanel : UIBasePanel
{

    List<UIItemBase> topList;
    List<UIItemBase> botList;
    Dictionary<int, int> awards;

    public override void Init()
	{
		base.Init();
        AddCollider();
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, 17750);
    }
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(topList);
        UIItemManager.Instance.RecycleItemsFormMediator(botList);
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        awards?.Clear();
        awards = null;
        base.OnDestroy();
	}


    public void OpenPanel(monthcard.MonthCardInfo card)
    {
        if (card == null) return;
        int boxId = MonthCardTableManager.Instance.GetMonthCardRewardDay(card.id);
        if (awards == null) awards = new Dictionary<int, int>();
        else awards.Clear();
        BoxTableManager.Instance.GetBoxAwardById(boxId, awards);

        mscroll_bot.gameObject.SetActive(card.keepRewardDay > 0);
        mlb_keeps.gameObject.SetActive(card.keepRewardDay > 0);
        msp_texbg.height = card.keepRewardDay > 0 ? 292 : 172;
        mscroll_top.transform.localPosition = card.keepRewardDay > 0 ? new Vector2(0, 95) : new Vector2(0, 78);

        topList = UIItemManager.Instance.GetUIItems(awards.Count, PropItemType.Normal, mGrid_top.transform, itemSize.Size70);
        if (topList != null)
        {
            int i = 0;
            for (var it = awards.GetEnumerator(); it.MoveNext();)
            {
                topList[i].Refresh(it.Current.Key);
                topList[i].SetCount(it.Current.Value, CSColor.white);
                i++;
            }
            mGrid_top.Reposition();
            mscroll_top.ResetPosition();
        }

        if (card.keepRewardDay > 0)
        {
            mlb_keeps.text = CSString.Format(1127, card.keepRewardDay);

            botList = UIItemManager.Instance.GetUIItems(awards.Count, PropItemType.Normal, mGrid_bot.transform, itemSize.Size70);
            if (botList != null)
            {
                int i = 0;
                for (var it = awards.GetEnumerator(); it.MoveNext();)
                {
                    botList[i].Refresh(it.Current.Key);
                    botList[i].SetCount(it.Current.Value * card.keepRewardDay, CSColor.white);
                    i++;
                }
                mGrid_bot.Reposition();
                mscroll_bot.ResetPosition();
            }
        }
    }
}
