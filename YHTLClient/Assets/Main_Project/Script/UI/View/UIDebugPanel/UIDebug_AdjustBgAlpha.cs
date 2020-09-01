﻿using UnityEngine;
using System.Collections;

public class UIDebug_AdjustBgAlpha : MonoBehaviour {
    //public UIDebugPanel debugPanel;
    public UISlider slider;
    public UISprite bg;
    public void Show(/*UIDebugPanel panel*/)
    {
        //debugPanel = panel;
        EventDelegate.Add(slider.onChange, () => { OnSliderChange(slider.gameObject, slider.value); });
        slider.value = UIDebugInfo.bgAlpha;
        OnSliderChange(slider.gameObject, UIDebugInfo.bgAlpha);
    }

    void OnSliderChange(GameObject go,float value)
    {
        UIDebugInfo.bgAlpha = value;
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1 - slider.value);
    }
}
