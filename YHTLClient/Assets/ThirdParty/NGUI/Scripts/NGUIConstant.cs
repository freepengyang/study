using System;
using UnityEngine;
using System.Collections;

public class NGUIConstant
{
    public static float SpecialAdaptRadio = 1.0f;
    public static float SpecialAdaptRadioLeft = 1.0f;
    public static float SpecialAdaptRadioRight = 1.0f;

    public static void SetScreen(int direction)
    {
        if(Math.Abs(SpecialAdaptRadio - 1) < 0.01f) return;
        if (direction == 0)
        {
            SpecialAdaptRadioLeft = SpecialAdaptRadio;
            SpecialAdaptRadioRight = 1;
        }
        else
        {
            SpecialAdaptRadioLeft = 1;
            SpecialAdaptRadioRight = SpecialAdaptRadio;
        }

        NGUITools.UpdateAnchors();
    }
}
