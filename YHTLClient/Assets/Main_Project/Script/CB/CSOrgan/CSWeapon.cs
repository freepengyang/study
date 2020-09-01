
//-------------------------------------------------------------------------
//CSWeapon
//Author jiabao
//Time 2015.1.13
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CSWeapon : CSOrgan
{
    public CSWeapon(GameObject go, CSOrganData _organDatar) : base(go, _organDatar)
    {
        Structure = ModelStructure.Weapon;
        Type = ModelBearing.HandRight;
        IsHasShoadow = false;
    }

    public override void Initialization()
    {
        base.Initialization();
        Type = ModelBearing.HandRight;
    }

    public override void SetPartDepth(int depth)
    {
        switch (organData.action.Direction)
        {
            case CSDirection.Up:
            case CSDirection.Down:
                {
                    depth = -depth;
                }
                break;
        }
        base.SetPartDepth(depth);
    }
}



