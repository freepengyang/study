using System;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIVIPMapPanel : UIWoLongActivityBasePanel
{
    public override void GetPlayLV()
    {
        playLv = CSMainPlayerInfo.Instance.VipLevel;
    }

    public override int Mapid 
    {
        get
        {
            return 21;
        }
    }

    public override int RedTipId 
    {
        get
        {
            return 1818;
        }
    }
    
    protected override bool IsEnter(ItemParameter para)
    {
        if (para.Instance.vip <= playLv)
            return true;
        else
            return false;
    }

    protected override string GetBtnStr(string mapName,TABLE.INSTANCE para)
    {
        return CSString.Format(1816, mapName, para.vip);
    }

    protected override string GetlevelStr(INSTANCE para)
    {
        return CSString.Format(1817, para.vip);
    }
    
    
    
}

