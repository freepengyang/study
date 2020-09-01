using UnityEngine;
using System.Collections;

public class TweenPositionSingleDirection : UITweener
{
    public Vector3 from;
    public Vector3 to;
    public bool vertical;

    [HideInInspector]
    public bool worldSpace = false;

    Transform mTrans;
    UIRect mRect;

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    [System.Obsolete("Use 'value' instead")]
    public Vector3 position { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
        }
        set
        {
            if (mRect == null || !mRect.isAnchored || worldSpace)
            {
                if (vertical)
                {
                    if (worldSpace) cachedTransform.position = new Vector3(cachedTransform.position.x, value.y, value.z);
                    else cachedTransform.localPosition = new Vector3(cachedTransform.position.x, value.y, value.z);
                }
                else
                {
                    if (worldSpace) cachedTransform.position = new Vector3(value.x, cachedTransform.position.y, value.z);
                    else cachedTransform.localPosition = new Vector3(value.x, cachedTransform.position.y, value.z);
                }
            }
            else
            {
                value -= cachedTransform.localPosition;
                if (vertical)
                {
                    NGUIMath.MoveRect(mRect, 0, value.y);
                }
                else
                {
                    NGUIMath.MoveRect(mRect, value.x, 0);
                }
            }
        }
    }

    void Awake() { mRect = GetComponent<UIRect>(); }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.worldSpace = worldSpace;
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}
