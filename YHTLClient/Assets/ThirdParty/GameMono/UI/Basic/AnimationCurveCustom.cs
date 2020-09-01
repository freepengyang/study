using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationCurveCustom : MonoBehaviour {

    public List<AnimationCurve> CurveList = new List<AnimationCurve>();

    public AnimationCurve Normal = new AnimationCurve();
    public AnimationCurve SpeedUp = new AnimationCurve();
    public AnimationCurve SpeedDown = new AnimationCurve();
}
