using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBottomNPC : CSOrgan
{
    public CSBottomNPC(GameObject go, CSOrganData _organDatar) : base(go, _organDatar)
    {
        Structure = ModelStructure.BottomNPC;
        Type = ModelBearing.BottomNPC;
        IsHasShoadow = false;
    }

    public override void Initialization()
    {
        base.Initialization();
    }
}
