using FlyBirds.Model;
using UnityEngine;

public partial class UINewFunctionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

	TimerEventHandle mTimerDelayHandle;
	TimerEventHandle mTimerAlphaFinished;
	TimerEventHandle mTimerColse;
	public override void Init()
	{
		base.Init();
		//mbtn_close.onClick = this.Close;
		if(null != mTimerDelayHandle)
		{
			CSTimer.Instance.remove_timer(mTimerDelayHandle);
			mTimerDelayHandle = null;
		}
        int fadeTime = ScriptBinder.GetIntArgv(0);
        fadeTime = Mathf.Clamp(fadeTime, 0, 3);
        mTimerDelayHandle = CSTimer.Instance.Invoke(fadeTime, OnDelayFade);
	}

	protected void OnDelayFade()
	{
        var tweens = ScriptBinder.GetComponentsInChildren<TweenAlpha>();
        if(null == tweens || tweens.Length <= 0)
        {
            this.Close();
            return;
        }

        for (int i = 0; i < tweens.Length; ++i)
        {
            tweens[i].enabled = true;
            tweens[i].ResetToBeginning();
            tweens[i].PlayForward();
            if (i == 0)
            {
                if (null != mTimerAlphaFinished)
                {
                    CSTimer.Instance.remove_timer(mTimerAlphaFinished);
					mTimerAlphaFinished = null;
                }

                float fadeTime = Mathf.Clamp(tweens[i].delay + tweens[i].duration, 0, 3);
                mTimerAlphaFinished = CSTimer.Instance.Invoke(fadeTime, OnPlayFinish);
            }
        }
    }

	protected void OnPlayFinish()
	{
		if(null != mFuncItem)
		{
			mtweenPosition.enabled = true;
			mtweenPosition.ResetToBeginning();
			mtweenPosition.PlayForward();
			float v = mFuncItem.scale * 0.01f;
			mtweenScale.to = new Vector3(v,v,v);
            Vector3 current = mtweenPosition.to;
			mtweenScale.enabled = true;
			mtweenScale.ResetToBeginning();
			mtweenScale.PlayForward();

            var panel = UIManager.Instance.GetPanel<UIMainSceneManager>();
            if (null != panel)
            {
                var child = panel.UIPrefab.transform.Find(mFuncItem.path);
                if (null != child)
                {
                    current = mtweenPosition.transform.parent.InverseTransformPoint(child.transform.position);
                    mtweenPosition.to = current;
                }
            }

            if (null != mTimerColse)
            {
                CSTimer.Instance.remove_timer(mTimerColse);
                mTimerColse = null;
            }
            float fadeTime = Mathf.Clamp(mtweenPosition.delay + mtweenPosition.duration, 0, 3);
            mTimerColse = CSTimer.Instance.Invoke(fadeTime, this.OnFadeOver);
        }
        else
        {
            this.Close();
        }
	}

    void OnFadeOver()
    {
        var funcItem = mFuncItem;
        this.Close();

        if (null != funcItem)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnNewFunctionPlayOver, funcItem);
        }
    }
	
	public override void Show()
	{
		base.Show();
	}

	TABLE.FUNCOPEN mFuncItem;
	public void Show(TABLE.FUNCOPEN funcItem)
	{
		mFuncItem = funcItem;
		if (null != funcItem)
		{
            if (null != mFuncIcon)
            {
				mFuncIcon.spriteName = funcItem.iconunlock;
            }
            if (null != mFuncName)
            {
                mFuncName.text = funcItem.functionName;
            }
        }
        else
        {
            FNDebug.LogError($"[新功能开启]:表项为空");
        }
	}
	
	protected override void OnDestroy()
	{
        if (null != mTimerColse)
        {
            CSTimer.Instance.remove_timer(mTimerColse);
            mTimerColse = null;
        }

        if (null != mTimerDelayHandle)
        {
            CSTimer.Instance.remove_timer(mTimerDelayHandle);
            mTimerDelayHandle = null;
        }

        if(null != mTimerAlphaFinished)
        {
            CSTimer.Instance.remove_timer(mTimerAlphaFinished);
            mTimerAlphaFinished = null;
        }

        mFuncItem = null;
		base.OnDestroy();

        CSNewFunctionUnlockManager.Instance.Poped = false;
        CSNewFunctionUnlockManager.Instance.TriggerNextAction();
    }
}
