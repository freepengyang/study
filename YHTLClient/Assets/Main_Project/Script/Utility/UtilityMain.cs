using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityMain 
{

    public static bool IsInViewRange(CSMisc.Dot2 v, int deltaX = 0, int deltaY = 0)
    {
        CSMisc.Dot2 vec = CSMainParameterManager.mainPlayerOldCell.Coord;

        if (v.x >= vec.x - (CSConstant.viewRange.x + deltaX) &&
            v.x <= vec.x + (CSConstant.viewRange.x + deltaX) &&
            v.y >= vec.y - (CSConstant.viewRange.y + deltaY) &&
            v.y <= vec.y + (CSConstant.viewRange.y + deltaY))
        {
            return true;
        }
        return false;
    }

    public static bool IsNumber(int type)
    {
        switch (type)
        {
            case ThrowTextType.NormalDamage:
            case ThrowTextType.Cure:
            case ThrowTextType.Mp:
            case ThrowTextType.Exp:
            case ThrowTextType.Score:
                return true;
            default:
                return false;
        }
    }
}
