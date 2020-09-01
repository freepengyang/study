using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ESkillHurtEffectType
{
    public const int None = 0;
    public const int Single = 1;                //1单体
    public const int Cell = 2;                  //2单格
    public const int CircleSelf = 3;            //3自身中心圆形
    public const int Circle = 4;                //4圆形
    public const int FrontLine = 5;             //5前方一直线
    public const int FrontRow = 6;              //6前方一横排
    public const int FrontFan = 7;              //7前方扇形
    public const int DistSingle = 8;            //8判断距离
    public const int Cross = 9;                 //9十字型
    public const int OneSide = 10;              //10单边
}
static public class CSSkillTools
{
    //public static bool GetBestLaunchCoord(int effectArea, int range, int effectRange, CSMisc.Dot2 curPos, CSMisc.Dot2 targetPos)
    //{
    //    bool isGetBestLaunchCoord = false;
    //    isGetBestLaunchCoord = GetBestLaunchCoord(effectArea,range,effectRange, curPos, targetPos);
    //    return isGetBestLaunchCoord;
    //}

    public static bool GetBestLaunchCoord(int effectArea, int range, int effectRange, CSMisc.Dot2 curPos, CSMisc.Dot2 targetPos)
    {
        bool isGetBestLaunchCoord = false;

        switch (effectArea)
        {
            case ESkillHurtEffectType.None:
            case ESkillHurtEffectType.Single:
            case ESkillHurtEffectType.Cell:
            case ESkillHurtEffectType.Circle:
            case ESkillHurtEffectType.DistSingle:   //TODO:ddn
            case ESkillHurtEffectType.Cross:
            case ESkillHurtEffectType.OneSide:
                {
                    if (IsAttackTowerRangeLimit())
                    {
                        isGetBestLaunchCoord = IsStraightLineDistance(curPos, targetPos, range);
                    }
                    else
                    {
                        isGetBestLaunchCoord = IsCellDistance(curPos, targetPos, range);
                    }
                }
                break;
            case ESkillHurtEffectType.CircleSelf:
            case ESkillHurtEffectType.FrontFan:
                {
                    if (IsAttackTowerRangeLimit())
                    {
                        isGetBestLaunchCoord = IsStraightLineDistance(curPos, targetPos, effectRange);
                    }
                    else
                    {
                        isGetBestLaunchCoord = IsCellDistance(curPos, targetPos, effectRange);
                    }
                }
                break;
            case ESkillHurtEffectType.FrontLine:
                {
                    CSMisc.Dot2 d = targetPos - curPos;
                    CSMisc.Dot2 d2 = d.Abs();

                    if (d2.x == d2.y || d2.x == 0 || d2.y == 0)
                    {
                        int dist = d2.x == 0 ? d2.y : d2.x;
                        if (IsAttackTowerRangeLimit())
                        {
                            isGetBestLaunchCoord = IsStraightLineDistance(curPos, targetPos, effectRange);
                        }
                        else if (dist <= effectRange)
                        {
                            //Debug.LogError("======> FrontLine isGetBestLaunchCoord is true");
                            return true;
                        }
                    }
                }
                break;
            case ESkillHurtEffectType.FrontRow:
                {
                    if (effectRange % 2 == 0)
                    {
                        if (FNDebug.developerConsoleVisible) FNDebug.LogError("请检查该技能的effectRange参数,横排打击的只能填奇数");
                        return false;
                    }
                    for (int i = 0; i < CSMisc.dirList.Count; i++)
                    {
                        CSMisc.Dot2 dot = curPos;
                        dot.x += CSMisc.dirList[i].x;
                        dot.y += CSMisc.dirList[i].y;
                        if (dot.Equal(curPos))
                        {
                            return true;
                        }
                        CSMisc.Dot2 d = targetPos - dot;
                        CSMisc.Dot2 d2 = d.Abs();
                        if (d2.x == d2.y || d2.x == 0 || d2.y == 0)
                        {
                            int dist = d2.x == 0 ? d2.y : d2.x;
                            if (IsAttackTowerRangeLimit())
                            {
                                isGetBestLaunchCoord = IsStraightLineDistance(curPos, targetPos, effectRange);
                            }
                            else if (dist <= effectRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
        }

        return isGetBestLaunchCoord;
    }

    private static bool IsAttackTowerRangeLimit()
    {
        //TODO:ddn 
        return false;
        /*if (!CSScene.IsLanuchMainPlayer) return false;
        //if (!CSScene.IsInGrassScene) return false;
        //if (skill == null || skill.SkillInfo == null) return false;
        //if (!CSScene.IsAttackTowerOrDiaoXiang && !CSScene.IsAttackSkillRange3) return false;
        if (CSScene.Sington == null) return false;
        return true;*/
    }

    static bool IsStraightLineDistance(CSMisc.Dot2 curPos, CSMisc.Dot2 targetPos, int range)
    {
        CSMisc.Dot2 dot = curPos - targetPos;
        dot = dot.Abs();
        return dot.Pow2() <= range * range;
    }

    public static bool IsCellDistance(CSMisc.Dot2 curPos, CSMisc.Dot2 targetPos, int range)
    {
        CSMisc.Dot2 d = targetPos - curPos;
        CSMisc.Dot2 d2 = d.Abs();
        return (d2.x <= range && d2.y <= range);
    }

   
}
