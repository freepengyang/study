using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIForceIncrease : UIBase
{
    private GameObject _TargetPoint;  //小红点目标单位
    private GameObject mTargetPoint { get { return _TargetPoint ? _TargetPoint : (_TargetPoint = Get<GameObject>("iconContainer")); } }

    private GameObject _effct;
    private GameObject effct { get { return _effct ? _effct : (_effct = Get<GameObject>("iconContainer/effect")); } }

    private UILabel _pointCount;
    private UILabel pointCount { get { return _pointCount ? _pointCount : (_pointCount = Get<UILabel>("iconContainer/redpoint/redLab")); } }

    protected EventHanlderManager mRedEvent = new EventHanlderManager(EventHanlderManager.DispatchType.RedPoint);

    private CSBetterList<int> CollectedPoints = new CSBetterList<int>();
    
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mTargetPoint).onClick = OnOpenDialog;
        CSForceIncreaseInfo.Instance.Init();
        InitRed();
        
    }

    private void InitRed()
    {
        if(mTargetPoint != null) mTargetPoint.SetActive(false);

        var redDic = CSForceIncreaseInfo.Instance.ForceIncreaseDic;
        if (redDic != null)
        {
            bool state;
            for (redDic.Begin();redDic.Next();)
            {
                mRedEvent.AddEvent((RedPointType)redDic.Key, OnReceiveRedPointMessage);
                state = UIRedPointManager.Instance.GetRedPointState((RedPointType)redDic.Key);
                if(state && !CollectedPoints.Contains(redDic.Key))
                    CollectedPoints.Add(redDic.Key);
            }
        }

        RefreshState();
    }

    public override void Show()
    {
        base.Show();
    }

    private void OnReceiveRedPointMessage(uint id, object data)
    {
        int key = (int) id;
        bool state = UIRedPointManager.Instance.GetRedPointState((RedPointType)id);
        if (CollectedPoints.Contains(key))
        {
            if (!state)
                CollectedPoints.Remove(key);
        }
        else
        {
            if(state && !CollectedPoints.Contains(key))
                CollectedPoints.Add(key);
        }

        RefreshState();
    }

    private void RefreshState()
    {
		if (CollectedPoints.Count > 0)
		{
			if (!mTargetPoint.activeSelf)
			{
				mTargetPoint.SetActive(true);
				CSEffectPlayMgr.Instance.ShowUIEffect(effct, 17006);
			}

            pointCount.text = CollectedPoints.Count.ToString();
        }
        else
        {
			if (mTargetPoint.activeSelf)
			{
				mTargetPoint.SetActive(false);
				CSEffectPlayMgr.Instance.Recycle(effct);
			}
        }

        CSForceIncreaseInfo.Instance.OpenFuncList = CollectedPoints;
    }
    
    private void OnOpenDialog(GameObject btn)
    {  
        UIManager.Instance.CreatePanel<UIForceIncreasePanel>();
    }

    protected override void OnDestroy()
    {
        CollectedPoints?.Clear();
        CollectedPoints = null;
        mRedEvent?.UnRegAll();
        mRedEvent = null;
		CSEffectPlayMgr.Instance.Recycle(effct);
		base.OnDestroy();
  }
}