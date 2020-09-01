using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSWing : CSOrgan
{
    public CSWing(GameObject go, CSOrganData _organDatar): base(go, _organDatar)
    {
        Structure = ModelStructure.Wing;
        Type = ModelBearing.Back;
        IsHasShoadow = false;
    }

    public override void Initialization()
    {
        base.Initialization();
        Type = ModelBearing.Back;
    }

    public override void SetPartDepth(int depth)
    {
        switch (organData.action.Direction)
        {
            case CSDirection.Left:
            case CSDirection.Right:
                {
                    if(Animation != null && Animation.getFrameAni != null)
                    {
                        if (Animation.getFrameAni.curFrame < 4)
                        {
                            base.SetPartDepth(2);
                        }
                        else
                        {
                            base.SetPartDepth(-2);
                        }
                    }
                }
                break;
        }
        base.SetPartDepth(depth);
    }
}




