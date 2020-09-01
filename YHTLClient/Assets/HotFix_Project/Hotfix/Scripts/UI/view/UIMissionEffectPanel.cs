using System.Collections.Generic;

public class CSMissionEffectManager : CSInfo<CSMissionEffectManager>
{
    Queue<int> mNeedPlayEffects = new Queue<int>(8);

    public void Init()
    {
        mClientEvent.AddEvent(CEvent.Task_FinshEffect, TaskFinshEffect);
        mClientEvent.AddEvent(CEvent.Task_AcceptEffect, TaskAcceptEffect);
    }

    private void TaskAcceptEffect(uint id, object data)
    {
        DoEffect(17071);
    }

    void DoEffect(int id)
    {
        mNeedPlayEffects.Enqueue(id);
        FNDebug.LogFormat("<color=#00ff00>[MissionEffect]:Push {0}</color>", id);
        PlayEffect();
    }

    void PlayEffect()
    {
        if (mNeedPlayEffects.Count <= 0)
            return;

        UIMissionEffectPanel panel = UIManager.Instance.GetPanel<UIMissionEffectPanel>();
        if (null != panel)
        {
            return;
        }

        UIManager.Instance.CreatePanel<UIMissionEffectPanel>(f=>
        {
            (f as UIMissionEffectPanel).PlayEffect(mNeedPlayEffects.Peek());
        });
    }

    public int Next()
    {
        if (mNeedPlayEffects.Count > 0)
            mNeedPlayEffects.Dequeue();
        
        if (mNeedPlayEffects.Count > 0)
            return mNeedPlayEffects.Peek();

        return -1;
    }

    private void TaskFinshEffect(uint id, object data)
    {
        DoEffect(17072);
    }

    public override void Dispose()
    {
        mClientEvent.RemoveEvent(CEvent.Task_FinshEffect, TaskFinshEffect);
        mClientEvent.RemoveEvent(CEvent.Task_AcceptEffect, TaskAcceptEffect);
        mNeedPlayEffects?.Clear();
        mNeedPlayEffects = null;
    }
}

public partial class UIMissionEffectPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
	{
		base.Init();
    }
	
	public override void Show()
	{
		base.Show();
	}

    protected void OnEffectPlayFinished()
    {
        int id = CSMissionEffectManager.Instance.Next();
        if (id != -1)
        {
            mmissionEffect.PlayEffect(id);
            FNDebug.LogFormat("<color=#00ff00>[MissionEffect]:播放完成播放下一个特效 {0}</color>",id);
        }
        else
        {
            FNDebug.LogFormat("<color=#00ff00>[MissionEffect]:关闭特效界面</color>");
            this.Close();
        }
    }

    public void PlayEffect(int effectId)
    {
        FNDebug.LogFormat("<color=#00ff00>[MissionEffect]:PlayEffect {0}</color>", effectId);
        //mmissionEffect.StopEffect(OnEffectPlayFinished);
        mmissionEffect.PlayEffect(effectId, 10, true, OnEffectPlayFinished);
    }

    protected override void OnDestroy()
	{
        mmissionEffect?.StopEffect(OnEffectPlayFinished);
        mmissionEffect = null;
        CSEffectPlayMgr.Instance.Recycle(mmissionEffect);

        base.OnDestroy();
    }
}
