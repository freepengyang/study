using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIRewardFlyEffectPanel : UIBasePanel
{
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}

	private TweenPositionSingleDirection _tweenHorizontal;
    public TweenPositionSingleDirection tweenHorizontal { get { return _tweenHorizontal ?? (_tweenHorizontal = Get<TweenPositionSingleDirection>("horizontalTween")); } }
    private TweenPositionSingleDirection _tweenVertical;
    private TweenPositionSingleDirection tweenVertical { get { return _tweenVertical ?? (_tweenVertical = Get<TweenPositionSingleDirection>("horizontalTween/verticalTween")); } }
    private TweenScale _tweenScal;
    private TweenScale tweenScal { get { return _tweenScal ?? (_tweenScal = Get<TweenScale>("horizontalTween")); } }

    private UIGridContainer _rewardGrid;
    private UIGridContainer rewardGrid { get { return _rewardGrid ?? (_rewardGrid = Get<UIGridContainer>("center")); } }
    private Transform _playerTargetTrs;
	private Transform playerTargetTrs { get { return _playerTargetTrs ?? (_playerTargetTrs = Get("playerTarget")); } }

	private CSInvoke _csInvoke;
	private CSInvoke csInvoke { get { return _csInvoke ?? (_csInvoke = Get<CSInvoke>("CSInvoke")); } }
	public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.TopWindow;
        }
    }

    private Transform targetTransform;

	private float _toCenterDuration = 0.4f;
	public float toCenterDuration
	{
		get { return _toCenterDuration; }
		set { _toCenterDuration = value; }
	}
	private float _toTargetDuration = 0.4f;
	public float toTargetDuration
	{
		get { return _toTargetDuration; }
		set { _toTargetDuration = value; }
	}
	private bool _isOpenTweenScal = false;
	public bool isOpenTweenScal
	{
		get { return _isOpenTweenScal; }
		set { _isOpenTweenScal = value; }
	}


	// Use this for initialization
	void Start()
	{

	}

	public void SetAnimationDuration(float duration)
	{
		toCenterDuration = duration;
		toTargetDuration = duration;
	}

	public void ShowEffect(List<List<int>> reward, Vector3 startPosition, bool IsClosePanel)
	{

		Initialization();

		targetTransform = UIManager.Instance.GetPanel<UIMainSceneManager>().btn_bagBtn.transform;
		rewardGrid.transform.position = startPosition + new Vector3(0, 0.05f, 0);

		RefreshReward(reward, IsClosePanel);
	}

	public void ShowFlyItem(List<List<int>> items, Vector3 startPosition, Transform target, bool isClosePanel)
	{
		Initialization();

		targetTransform = target;
		rewardGrid.transform.position = startPosition + new Vector3(0, 0.05f, 0);

		RefreshReward(items, isClosePanel);
	}

	public void ShowFlyItemToPlayer(List<int> itemList, List<Vector3> startList, bool isClosePanel)
	{
		Initialization();
		playerTargetTrs.position = new Vector3(0,-0.1f,0);
		targetTransform = playerTargetTrs;
		RefreshReward(itemList, startList,isClosePanel);
	}

	public void ShowEffect(string effectName, Vector3 startPosition, Transform endPosition, System.Action onFinished)
	{
		Initialization();

		targetTransform = endPosition;
		rewardGrid.transform.position = startPosition;

		rewardGrid.MaxCount = 1;
		UIRewardFlyEffectPrefab prefab = null;

		prefab = rewardGrid.controlList[0].GetComponent<UIRewardFlyEffectPrefab>();
		prefab.SetDuation(toCenterDuration);
		prefab.target = targetTransform;
		prefab.InvokeTween(effectName);

		prefab.tweenHorizontal.AddOnFinished(delegate
		{
			onFinished();
			DestorySkillIconIt();
		});

		csInvoke?.Invoke(1.5f, DestorySkillIcon);
	}

	//初始化Tween数据
	private void Initialization()
	{
		tweenHorizontal.duration = toCenterDuration;
		tweenVertical.duration = toCenterDuration;
		tweenHorizontal.duration = toTargetDuration;
		tweenVertical.duration = toTargetDuration;

		if (tweenScal != null && isOpenTweenScal)
		{
			tweenScal.duration = toTargetDuration;
		}
	}

	private void RefreshReward(List<List<int>> reward, bool IsClosePanel)
	{
		if (reward.Count <= 0)
		{
			if (IsClosePanel) DestorySkillIconIt();
			return;
		}
		rewardGrid.MaxCount = reward.Count;
		UIRewardFlyEffectPrefab prefab = null;
		for (int i = 0; i < rewardGrid.MaxCount; i++)
		{
			prefab = rewardGrid.controlList[i].GetComponent<UIRewardFlyEffectPrefab>();
			prefab.SetDuation(toCenterDuration);
			prefab.target = targetTransform;
			prefab.playTime += 0.05f * i;
			prefab.InvokeTween(reward[i][0]);
			if (i == rewardGrid.MaxCount - 1)
			{
				prefab.tweenHorizontal.AddOnFinished(delegate
				{
					if (IsClosePanel) DestorySkillIconIt();
				});
			}
		}
		if (IsClosePanel)
			csInvoke?.Invoke(1.5f, DestorySkillIcon);
	}
	private void RefreshReward(List<int> reward, List<Vector3> startList, bool IsClosePanel)
	{
		if (reward.Count <= 0)
		{
			if (IsClosePanel) DestorySkillIconIt();
			return;
		}
		rewardGrid.MaxCount = reward.Count;
		UIRewardFlyEffectPrefab prefab = null;
		for (int i = 0; i < rewardGrid.MaxCount; i++)
		{
			prefab = rewardGrid.controlList[i].GetComponent<UIRewardFlyEffectPrefab>();
			prefab.SetDuation(toCenterDuration);
			prefab.transform.position = startList[i];
			prefab.target = targetTransform;
			prefab.playTime += 0.02f * i;
			prefab.InvokeTweenPlayer(reward[i]);
			if (i == rewardGrid.MaxCount - 1)
			{
				prefab.tweenHorizontal.AddOnFinished(delegate
				{
					if (IsClosePanel) DestorySkillIconIt();
				});
			}
		}
		if (IsClosePanel)
			csInvoke?.Invoke(1.5f, DestorySkillIcon);
	}

	void DestorySkillIcon()
	{
		csInvoke?.StopInvokeRepeating();
		DestorySkillIconIt();
	}
	void DestorySkillIconIt()
	{
		UIManager.Instance.ClosePanel<UIRewardFlyEffectPanel>();
	}
	protected override void OnDestroy()
	{
		csInvoke?.StopInvokeRepeating();
	}
}
