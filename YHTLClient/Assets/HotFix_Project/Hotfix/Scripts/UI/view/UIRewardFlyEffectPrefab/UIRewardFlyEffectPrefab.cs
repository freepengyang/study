using UnityEngine;
using System.Collections;

public class UIRewardFlyEffectPrefab : MonoBehaviour
{

	private TweenPositionSingleDirection _tweenHorizontal;
	public TweenPositionSingleDirection tweenHorizontal { get { return _tweenHorizontal ?? (_tweenHorizontal = transform.GetComponent<TweenPositionSingleDirection>()); } }
	private TweenPositionSingleDirection _tweenVertical;
	private TweenPositionSingleDirection tweenVertical { get { return _tweenVertical ?? (_tweenVertical = Utility.Get<TweenPositionSingleDirection>(this.transform, "verticalTween")); } }
	private TweenScale _tweenScal;
	private TweenScale tweenScal { get { return _tweenScal ?? (_tweenScal = transform.GetComponent<TweenScale>()); } }
	private Transform _center;
	public Transform center { get { return transform; } }
	private Transform _itemParent;
	private Transform itemParent { get { return _itemParent ?? (_itemParent = Utility.Get<Transform>(this.transform, "verticalTween/item")); } }
	private Transform _iconTrs;
	private Transform iconTrs { get { return _iconTrs ?? (_iconTrs = Utility.Get<Transform>(this.transform, "verticalTween/item/icon")); } }

	public Transform target;
	private EventDelegate eventDel;
	private UIItemBase itemBase;

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
	public float playTime = 0.2f;
	
	public void SetDuation(float duration)
	{
	    toCenterDuration = duration;
	    toTargetDuration = duration;
	}

	public void InvokeTween(int rewardId)
	{
		itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, itemParent, itemSize.Size66);
		itemBase.Refresh(rewardId);
		itemBase.SetCount(1, Color.white);
		if (itemParent != null && !itemParent.gameObject.activeSelf) itemParent.gameObject.SetActive(true);

		eventDel = new EventDelegate(this, "TweenPositionToTarget");
		Invoke("TweenPositionToCenter", playTime);
		Invoke("DestorySkillIcon", 2f);
	}
	public void InvokeTweenPlayer(int rewardId)
	{
		int quality;
		UISprite sp_icon = iconTrs.GetChild(0).GetComponent<UISprite>();
		UILabel lb_icon = iconTrs.GetChild(1).GetComponent<UILabel>();
		quality = ItemTableManager.Instance.GetItemQuality(rewardId);

		sp_icon.spriteName = ItemTableManager.Instance.GetItemIcon(rewardId);
		lb_icon.text = ItemTableManager.Instance.GetItemName(rewardId);
		lb_icon.color = UtilityCsColor.Instance.GetColor(quality);
		if (iconTrs != null && !iconTrs.gameObject.activeSelf) iconTrs.gameObject.SetActive(true);

		eventDel = new EventDelegate(this, "TweenPositionToTarget");
		Invoke("TweenPositionToCenter", playTime);
		Invoke("DestorySkillIcon", 2f);
	}
	public void InvokeTween(string effectName)
	{

		//if (item.transform.GetComponent<UIEffectPlay>() == null)
		//{
		//item.gameObject.AddComponent<UIEffectPlay>();
		//}

		//item.transform.GetComponent<UIEffectPlay>().ShowUIEffect(effectName);
		//if (item != null && !item.gameObject.activeSelf) item.gameObject.SetActive(true);
		eventDel = new EventDelegate(this, "TweenPositionToTarget");
		Invoke("TweenPositionToCenter", playTime);
		Invoke("DestorySkillIcon", 2f);
	}

	public void TweenPositionToCenter()
	{
		if (this == null)
		{
			return;
		}
		tweenHorizontal.worldSpace = true;
		tweenHorizontal.from = tweenHorizontal.transform.position;
		tweenHorizontal.to = target.position;
		tweenHorizontal.duration = toCenterDuration;
		tweenHorizontal.ResetToBeginning();
		tweenHorizontal.PlayForward();
		tweenHorizontal.AddOnFinished(eventDel);

		tweenVertical.worldSpace = true;
		tweenVertical.from = tweenHorizontal.transform.position;
		tweenVertical.to = target.position;
		tweenVertical.duration = toCenterDuration;
		tweenVertical.ResetToBeginning();
		tweenVertical.PlayForward();
	}

	void TweenPositionToTarget()
	{
		AnimationCurve tempCurve;
		tempCurve = tweenHorizontal.animationCurve;
		tweenHorizontal.animationCurve = tweenVertical.animationCurve;
		tweenVertical.animationCurve = tempCurve;
		tweenHorizontal.duration = toTargetDuration;
		tweenVertical.duration = toTargetDuration;
		tweenHorizontal.RemoveOnFinished(eventDel);
		tweenHorizontal.AddOnFinished(delegate
		{
			//gameObject.SetActive(false);
		});


		tweenHorizontal.worldSpace = true;
		if (tweenHorizontal != null) tweenHorizontal.from = tweenHorizontal.transform.position;
		if (target != null) tweenHorizontal.to = target.position;
		tweenHorizontal.ResetToBeginning();
		tweenHorizontal.PlayForward();

		tweenVertical.worldSpace = true;
		if (tweenHorizontal != null) tweenVertical.from = tweenVertical.transform.position;
		if (target != null) tweenVertical.to = target.position;
		tweenVertical.ResetToBeginning();
		tweenVertical.PlayForward();

		if (tweenScal != null && isOpenTweenScal)
		{
			tweenScal.duration = toTargetDuration;
			tweenScal.ResetToBeginning();
			tweenScal.PlayForward();
		}
	}

	//void DestorySkillIcon()
	//{
	//gameObject.SetActive(false);
	//}

	//private void OnDestroy()
	//{
	//	CancelInvoke();
	//}

	private void OnDestroy()
	{
		if (itemBase != null) { UIItemManager.Instance.RecycleSingleItem(itemBase); }
	}

}
