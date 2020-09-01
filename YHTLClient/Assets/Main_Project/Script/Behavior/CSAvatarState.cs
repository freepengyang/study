using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSAvatarState
{
    public bool IsBeControl { get; set; }
    //buff
    public bool IsHiding { get; set; }

    public bool IsVertigo { get; set; }

    public bool IsHold { get; set; }

    public int ColorType { get; set; }      //1红 2绿 3灰  

    public void ResetAll()
    {
        IsBeControl = false;
        IsHiding = false;
        IsVertigo = false;
        IsHold = false;
        ColorType = 0;
    }

    public void Reset()
    {
        IsHiding = false;
        IsVertigo = false;
        IsHold = false;
        ColorType = 0;
    }

    public CSAvatarState Copy(CSAvatarState state)
    {
        this.IsBeControl = state.IsBeControl;
        this.IsHiding = state.IsHiding;
        this.IsVertigo = state.IsVertigo;
        this.IsHold = state.IsHold;
        this.ColorType = state.ColorType;
        return this;
    }

};

