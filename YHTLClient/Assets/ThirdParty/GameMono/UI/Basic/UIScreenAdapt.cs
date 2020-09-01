using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// UI自适应还需基于锚点控制
/// </summary>
class UIScreenAdapt : MonoBehaviour
{  
    private UIRoot mRoot;
    public bool RunOnlyOnce = false;
    void Awake()
    {
        mRoot = NGUITools.FindInParents<UIRoot>(this.gameObject);
        UICamera.onScreenResize += ScreenSizeChanged;
    }

    void OnDestroy() { UICamera.onScreenResize -= ScreenSizeChanged; }

    void ScreenSizeChanged() { Adapt(); }


    void Start()
    {
        Adapt();
    }
    public static float scaleCoeff=1;
    void Adapt()
    {
        int ManualWidth = 1136;  //已你自己默认屏幕来适配
        int ManualHeight = 640;
        if (mRoot != null)
        {
            if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            {
                scaleCoeff = Convert.ToSingle(ManualWidth) / Screen.width;
                mRoot.manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
            }
            else
                mRoot.manualHeight = ManualHeight;
        }

        if (RunOnlyOnce && Application.isPlaying)
        {
            this.enabled = false;
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        Adapt();
    }
#endif
     
}
