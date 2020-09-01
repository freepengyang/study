using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWoLongActivityPanel : UIWoLongActivityBasePanel
{
    public override void GetPlayLV()
    {
        playLv = CSWoLongInfo.Instance.ReturnWoLongInfo().wolongLevel;
    }

    public override int Mapid 
    {
        get
        {
            return 6;
        }
    }
}

