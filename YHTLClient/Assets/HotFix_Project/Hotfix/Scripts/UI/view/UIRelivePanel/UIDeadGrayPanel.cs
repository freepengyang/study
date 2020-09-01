using UnityEngine;

public class UIDeadGrayPanel : UIBase
{
    public override bool ShowGaussianBlur { get => false; }
    
    private ScreenGrayEffect mScreenEffect = null;
    private Schedule mChangeGraySchedule;
    public override void Init()
    {
        base.Init();

        if(CSMainPlayerInfo.Instance.HP > 0)
        {
            UIManager.Instance.ClosePanel<UIDeadGrayPanel>();
            return;
        }

#if !UNITY_IPHONE
        GameObject camera = Camera.main.gameObject;
        if (camera != null)
        {
            mScreenEffect = camera.GetComponent<ScreenGrayEffect>();
            if (mScreenEffect == null)
            {
                mScreenEffect = camera.AddComponent<ScreenGrayEffect>();
            }
            mChangeGraySchedule = Timer.Instance.InvokeRepeating(0.0f, 0.02f, OnChangeToGray);
        }
#endif
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RemoveSchedule();
        if(mScreenEffect != null)
        {
            GameObject.Destroy(mScreenEffect);
        }
    }

    private void OnChangeToGray(Schedule schedule)
    {
        if (mScreenEffect != null && mScreenEffect.grayScaleAmount < 1)
        {
            mScreenEffect.grayScaleAmount += 0.015f;
        }
        else
        {
            RemoveSchedule();
        }
    }

    private void RemoveSchedule()
    {
        if(mChangeGraySchedule != null)
        {
            Timer.Instance.CancelInvoke(mChangeGraySchedule);
        }
        mChangeGraySchedule = null;
    }
}
