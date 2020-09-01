using System;
using System.Collections.Generic;
using UnityEngine;
class DynamicFontProblemFix:MonoBehaviour
{
    /*
    public static DynamicFontProblemFix instance;
    public Font font;
    public TextAsset text;
    void Awake()
    {
        if (instance!=null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            font.RequestCharactersInTexture(text.ToString());
        }
    }*/
}
