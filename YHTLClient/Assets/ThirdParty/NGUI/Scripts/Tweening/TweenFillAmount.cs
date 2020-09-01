using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Fill Amount")]
public class TweenFillAmount : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;

    private bool mCached = false;
    private UIBasicSprite mBasic;

    private void Cache()
    {
        mCached = true;
        mBasic = GetComponent<UIBasicSprite>();
    }

    public float value
    {
        get
        {
            if (!mCached) Cache();
            return mBasic != null ? mBasic.fillAmount : 1f;
        }
        set
        {
            if (!mCached) Cache();
            if (mBasic != null) mBasic.fillAmount = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    static public TweenFillAmount Begin(GameObject go,float duration, float to)
    {
        TweenFillAmount comp = UITweener.Begin<TweenFillAmount>(go, duration);
        comp.from = comp.value;
        comp.to = to;

        if (duration <= 0)
        {
            comp.Sample(1, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }

    public override void SetEndToCurrentValue() { to = value; }
}