using UnityEngine;
using System.Collections;

public class CSBottom : CSOrgan
{
    public CSBottom(GameObject go, CSOrganData _organDatar) : base(go, _organDatar)
    {
        Structure = ModelStructure.Bottom;
        Type = ModelBearing.Bottom;
        IsHasShoadow = false;
    }

    public override void Initialization()
    {
        base.Initialization();
    }
}
