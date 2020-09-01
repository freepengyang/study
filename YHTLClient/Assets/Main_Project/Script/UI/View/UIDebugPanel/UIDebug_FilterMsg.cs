﻿using UnityEngine;
using System.Collections;

public class UIDebug_FilterMsg : MonoBehaviour
{
    //public UIDebugPanel debugPanel;
    public UIInput input;
    public void Show(/*UIDebugPanel panel*/)
    {
        //debugPanel = panel;
        EventDelegate.Add(input.onChange, () => { OnSliderChange(input.gameObject, input.value); });
        input.value = UIDebugInfo.FilterMsg;
        OnSliderChange(input.gameObject, UIDebugInfo.FilterMsg);
    }

    void OnSliderChange(GameObject go,string value)
    {
        UIDebugInfo.FilterMsg = value;
    }
}
