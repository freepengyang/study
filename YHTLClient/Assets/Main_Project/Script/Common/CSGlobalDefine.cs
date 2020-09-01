using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGlobalDefine
{

}

public enum ESkillEffectType
{
    None,
    Start,
    Move,
    End,
    EndFoot,
}

public enum ESkillEffectLayer
{
    Normal = 0,
    Brighten = 1,//变亮
    Colourfilter = 2,//滤色
    Overly = 3,//重叠
    StrongLight = 4,//强光
    Add = 5,//增加
}
