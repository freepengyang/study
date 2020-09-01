using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public  class TweenProgressBar : UITweener
{
    public float from = 0;      //初始值
    public float to = 1;        //目标值
    UIProgressBar m_ProgressBar; //目标组件
    //获取目标组件
    public UIProgressBar cachedProgress
    {
        get
        {
            if (m_ProgressBar == null)
            {
                m_ProgressBar = GetComponent<UIProgressBar>();
            }
            return m_ProgressBar;
        }
    }
    //用属性的方式去设置和获取目标ProcessBar的Value值
    public float value
    {
        get { return cachedProgress.value; }
        set { cachedProgress.value = value; }
    }
    /// <summary>
    /// 重写UITweener的OnUpdate
    /// </summary>
    /// <param name="factor">插值</param>
    /// <param name="isFinished"></param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        //设置ProcessBar的Value的值
        value = Mathf.Lerp(from, to, factor);
    }
 
    /// <summary>
    /// 静态方法（方便在代码中动态添加动画）
    /// </summary>
    /// <param name="processBar">目标</param>
    /// <param name="duration">动画时长</param>
    /// <param name="from">起始值</param>
    /// <param name="to">目标值</param>
    /// <returns></returns>
    public static TweenProgressBar Begin(UIProgressBar processBar, float duration, float from, float to)
    {
        //通过调用UITweener的Begin方法，给目标组件绑定一个TweenProgressBar
        TweenProgressBar comp = UITweener.Begin<TweenProgressBar>(processBar.gameObject, duration);
        comp.from = from;
        comp.to = to;
        comp.m_ProgressBar = processBar;
 
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
