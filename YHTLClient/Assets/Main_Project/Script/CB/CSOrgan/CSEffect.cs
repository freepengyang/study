
//-------------------------------------------------------------------------
//CSEffect
//Author jiabao
//Time 2015.1.18
//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSEffect : CSOrgan
{
    public CSEffect(GameObject go, CSOrganData _organData) : base(go,_organData)
    {
        Structure = ModelStructure.Effect;
        Type = ModelBearing.Front;
        IsHasShoadow = false;
    }

    public override void Initialization()
    {
        base.Initialization();
    }
}
