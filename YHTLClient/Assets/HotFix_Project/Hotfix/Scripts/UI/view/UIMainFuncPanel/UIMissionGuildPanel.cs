using System;
using TABLE;
using UnityEngine;

public partial class UIMissionGuildPanel : UIBasePanel
{
	public override UILayerType PanelLayerType
	{
		get { return UILayerType.Resident; }
	}

	public override bool ShowGaussianBlur
	{
		get { return false; }
	}

	private const int AUTOTRANSFERTIME = 5;
	private int leftTime;
	public override void Init()
	{
		base.Init();
		leftTime = AUTOTRANSFERTIME + 1;
		mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MovePanel);
	}

	WeakReference<UISprite> linkedTarget;
	int taskId = -1;
	protected Vector2 mFixedOffset = new Vector2(1.0f, 0.5f);
	protected Vector3 mPixelOffset = Vector3.zero;

	public void Show(UISprite transform)
	{
		linkedTarget = new WeakReference<UISprite>(transform);
		CheckPosition();
		var curMission = CSMissionManager.Instance.CurSelectMission;
        if (null != curMission)
        {
            taskId = curMission.TaskId;
        }
        ScriptBinder.InvokeRepeating(0, 1, ShowTimeDown);
		ScriptBinder.InvokeRepeating2(0, 0.01f,CheckPosition);
		mEffect.PlayEffect(17909);
		mEffectArrow.PlayEffect(17932);
	}

	void CheckPosition()
	{
        UISprite sprite = null;
        if (linkedTarget.TryGetTarget(out sprite))
        {
            Vector2 pivotOffset = -sprite.pivotOffset + mFixedOffset;
            Vector3 scale = sprite.root.transform.localScale;
            mPixelOffset = pivotOffset * sprite.localSize * scale;
            Vector3 pos = sprite.transform.position + mPixelOffset;
			pos.x += 134 * scale.x;
			mright.position = pos;
            this.Panel.alpha = sprite.finalAlpha;
            if (null == sprite.gameObject || !sprite.gameObject.activeInHierarchy)
				this.Close();
			return;
        }
		this.Close();
    }

	private void ShowTimeDown()
	{
		var missionBase = CSMissionManager.Instance.GetMissionByTaskId(taskId);
		if (null == missionBase)
		{
			ScriptBinder.StopInvokeRepeating();
			this.Close();
			return;
		}

        leftTime--;
		if (leftTime > 0)
		{
			mlb_content.text = CSString.Format(1255, leftTime);
		}
		else
		{
			taskId = -1;
			this.Close();
			CSMissionManager.Instance.CurSelectMission = missionBase;
			missionBase.OnClick(true);
		}
	}

	private void MovePanel(uint id, object data)
	{
		if (data == null) return;
		if ((bool) data)
		{
			UIPrefab.SetActive(false);
		}
		else
		{
			UIPrefab.SetActive(true);
		}
	}
	
	protected override void OnDestroy()
	{
		if(null != mEffect)
		{
            CSEffectPlayMgr.Instance.Recycle(mEffect);
            mEffect = null;
		}
        if (null != mEffectArrow)
        {
            CSEffectPlayMgr.Instance.Recycle(mEffectArrow);
			mEffectArrow = null;
        }
		linkedTarget?.SetTarget(null);
		linkedTarget = null;
		taskId = -1;
		base.OnDestroy();
	}
}
