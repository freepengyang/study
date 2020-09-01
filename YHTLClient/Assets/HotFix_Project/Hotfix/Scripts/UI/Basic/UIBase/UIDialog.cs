using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///   2级菜单 和 对话框基类
/// </summary>
public class UIDialog : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        if (UIPrefab == null) return;
        AddCollider(UIPrefab);
        PlayCommonOpenEffect();
        CSScenePanelPosManager.Instance.AddPanel(name);

    }

    /// <summary>
	/// 二级面板  打开需要缩放效果的 调用此方法
	/// </summary>
    protected void PlayCommonOpenEffect()
    {
        TweenScale scale = UITweener.Begin<TweenScale>(UIPrefab, 0.1f);

        scale.from.Set(0.8f, 0.8f, 0.8f);
        scale.to.Set(1.05f, 1.05f, 1.05f);
        scale.delay = 0.0f;

        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(UIPrefab, 0.3f);
        alpha.from = 0.50f;
        alpha.to = 1.0f;

        TweenScale scaleDelay = UITweener.Begin<TweenScale>(UIPrefab, 0.1f);
        scaleDelay.from.Set(1.05f, 1.05f, 1.05f);
        scaleDelay.to.Set(1.0f, 1.0f, 1.0f);
        scaleDelay.delay = 0.1f;
        scaleDelay.duration = 0.20f;

        scale.PlayForward();
        alpha.PlayForward();
        scaleDelay.PlayForward();
    }
    
    protected override void OnDestroy()
    {
        CSScenePanelPosManager.Instance.RemovePanel(name);
        base.OnDestroy();
    }
}
