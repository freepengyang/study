using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITipsViewPool : CSInfo<UITipsViewPool>
{
	public enum TipsViewType
	{
		Center,
		LeftDown,
		Down,
		MoveUp,
        LeftDownCoverPanel,
        CenterRight,
    }

    private const int MAX_POOL_COUNT = 6;
	private Queue<UITipsViewBase>[] mPoolTipsList = new Queue<UITipsViewBase>[Enum.GetNames(typeof(TipsViewType)).Length];

	private CSBetterList<UITipsViewBase>[] mShowTipsList = new CSBetterList<UITipsViewBase>[Enum.GetNames(typeof(TipsViewType)).Length];


	public UITipsViewBase PopUITipPanel<T>(TipsViewType type)
	{
		CSBetterList<UITipsViewBase> tipsList = GetTipsList(type);
		if(tipsList == null)
		{
			tipsList = new CSBetterList<UITipsViewBase>();
			mShowTipsList[(int)type] =tipsList;
		}

		switch (type)
		{
			case TipsViewType.Center:
			case TipsViewType.LeftDown:
				return GetCycleTips<T>(tipsList, type);
			case TipsViewType.Down:
			case TipsViewType.MoveUp:
				return GetAstrictTips<T>(tipsList, type);
            case TipsViewType.LeftDownCoverPanel:
                return GetCycleTips<T>(tipsList, type);
            case TipsViewType.CenterRight:
                return GetAstrictTips<T>(tipsList, type);
        }
		return null;
	}

	public CSBetterList<UITipsViewBase> GetTipsList(TipsViewType type)
	{
		return mShowTipsList[(int) type];
	}

	private UITipsViewBase GetAstrictTips<T>(CSBetterList<UITipsViewBase> tipsList, TipsViewType type)
	{
		if (tipsList.Count >= MAX_POOL_COUNT) return null;

		UITipsViewBase tip = LoadTips<T>(type);

		tipsList.Add(tip);
		return tip;
	}

	private UITipsViewBase GetCycleTips<T>(CSBetterList<UITipsViewBase> tipsList, TipsViewType type)
	{
		UITipsViewBase tip;
		if (tipsList.Count >= MAX_POOL_COUNT)
		{
			tip = tipsList[0];
			tipsList.RemoveAt(0);
			tip.OnRecycle();
		}
		else
		{
			tip = LoadTips<T>(type);
		}

		if (tip == null) return tip;
		tipsList.Add(tip);
		return tip;
	}
	private UITipsViewBase LoadTips<T>(TipsViewType type)
	{
		UITipsViewBase tip = null;

		Queue<UITipsViewBase> tipsList = mPoolTipsList[(int) type];
		if(tipsList == null)
		{
			tipsList = new Queue<UITipsViewBase>();
			mPoolTipsList[(int)type] =tipsList;
		}
		if (tipsList.Count > 0)
		{
			tip = tipsList.Dequeue();
		}
		else
		{
            GameObject panel;
            if (type== TipsViewType.CenterRight)
            {
                panel = UIManager.Instance.loadUIPanel("UICenterRightTips", UIManager.Instance.GetRoot());
            }else if (type == TipsViewType.LeftDown)
            {
	            panel = UIManager.Instance.loadUIPanel("UILeftDownTips", UIManager.Instance.GetRoot());
            }
            else
            {
                panel = UIManager.Instance.loadUIPanel("UITips", UIManager.Instance.GetRoot());
            }
            if (panel == null) return tip;
			tip = Activator.CreateInstance<T>() as UITipsViewBase;
			tip.UIPrefab = panel;
			tip.UIPrefab.SetActive(false);
			UILayerMgr.Instance.SetLayer(panel, tip.PanelLayerType);
			tip.Init();
			tip.Show();
		}

		return tip;
	}

	public void PushUITipPanel(UITipsViewBase tip)
	{
		if (tip == null) return;
		CSBetterList<UITipsViewBase> tipsList = mShowTipsList[(int)tip.TipPos];
		if(tipsList == null) return;
		tipsList.Remove(tip);
		tip.OnRecycle();
		Queue<UITipsViewBase> tipsListPool = mPoolTipsList[(int) tip.TipPos];
		if(tipsListPool == null) return;
		tipsListPool.Enqueue(tip);
	}

	public override void Dispose()
	{

	}
}