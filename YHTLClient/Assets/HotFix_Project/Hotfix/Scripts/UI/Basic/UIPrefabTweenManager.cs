

using System;
using UnityEngine;
using Object = UnityEngine.Object;

public enum PrefabTweenType
{
    None,
    FirstPanel,  //一级面板
    NpcDialog,//Npc对话类，从右侧移出
    Special,//自己实现动画效果，但是需要隐藏主页面
}


public class UIPrefabTweenManager : Singleton<UIPrefabTweenManager>
{
    public void PlayTweenShow(PrefabTweenType tweenType, GameObject gameObject, System.Action openAction)
    {
        switch (tweenType)
        {
            case PrefabTweenType.FirstPanel:
                ShowFirstPanelEffect(gameObject, openAction);
                break;
            case PrefabTweenType.NpcDialog:
                PlayNpcShowEffect(gameObject, openAction);
                break;
        }
    }

    public void PlayTweenClose(PrefabTweenType tweenType, GameObject gameObject, System.Action closeAction)
    {
        switch (tweenType)
        {
            case PrefabTweenType.None:
                if (closeAction != null)
                    closeAction();
                break;
            case PrefabTweenType.FirstPanel:
                CloseFirstPanelEffect(gameObject, closeAction);
                break;
            case PrefabTweenType.NpcDialog:
                PlayNpcCloseEffect(gameObject, closeAction);
                break;
        }
    }

    #region FirstPanel
    public virtual void ShowFirstPanelEffect(GameObject gameObject, System.Action openAction)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.2f);

        position.from.Set(0, -300, 0);
        position.to = Vector3.zero;
        
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(gameObject, 0.2f);
        alpha.from = 0;
        alpha.to = 1;

        position.onFinished.Clear();
        EventDelegate.Add(position.onFinished,openAction);
        position.PlayForward();
        alpha.PlayForward();
        
    }
    
    public void CloseFirstPanelEffect(GameObject gameObject, System.Action closeAction)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.2f);

        position.from = Vector3.zero;
        position.to.Set(0, -300, 0);

        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(gameObject, 0.2f);
        alpha.from = 1;
        alpha.to = 0;

        position.onFinished.Clear();
        EventDelegate.Add(position.onFinished, closeAction);
        position.ResetToBeginning();
        position.PlayForward();
        alpha.PlayForward();
    }
    

    #endregion

    #region NPCDialog
    protected void PlayNpcShowEffect(GameObject gameObject, System.Action openAction)
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(gameObject, 0.2f);
        alpha.from = 0;
        alpha.to = 1;

        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.2f);
        position.from.Set(420,0,0);
        position.to = Vector3.zero;
        
        position.onFinished.Add(new EventDelegate(openAction));
        alpha.PlayForward();
        position.PlayForward();
    }
    
    protected void PlayNpcCloseEffect(GameObject gameObject, System.Action closeAction)
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(gameObject, 0.2f);
        alpha.from = 1;
        alpha.to = 0;

        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.2f);
        position.from = Vector3.zero;
        position.to.Set(420,0,0);
        
        position.onFinished.Add(new EventDelegate(closeAction));
        alpha.PlayForward();
        position.PlayForward();
    }
    

    #endregion
    
}