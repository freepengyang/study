
/*************************************************************************
** File: CSAction.cs
** Author: jiabao
** Time: 2015.12.15
** Describe: 动作跟方向
*************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSAction
{
    private int xValue, yValue;

    //private CSMotion mMotion = CSMotion.Stand;

    //private CSDirection mDirection = CSDirection.None;

    /// <summary>
    /// 动作
    /// </summary>
    public int Motion = CSMotion.Stand;

    /// <summary>
    /// 方向
    /// </summary>
    public int Direction = CSDirection.None;
    //{
    //    get { return mDirection; }
    //}

    /// <summary>
    /// 设置动作
    /// </summary>
    /// <param name="motion"></param>
    /// <returns></returns>
    public bool setAction(int motion)
    {
        if (Motion != motion)
        {
            Motion = motion;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置朝向
    /// </summary>
    /// <param name="direction"></param>
    public void setDirection(int direction)
    {
        Direction = direction;
    }

    /// <summary>
    /// 根据坐标得到朝向
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public int getDirection(CSCell cell, Vector3 target)
    {
        int direction = CSDirection.None;

        if (cell.InX(target.x))
        {
            xValue = 0;
        }
        else if (target.x > cell.LocalPosition2.x)
        {
            xValue = 1;
        }
        else if (target.x < cell.LocalPosition2.x)
        {
            xValue = -1;
        }

        if (cell.InY(target.y))
        {
            yValue = 0;
        }
        else if (target.y > cell.LocalPosition2.y)
        {
            yValue = 1;
        }
        else if (target.y < cell.LocalPosition2.y)
        {
            yValue = -1;
        }

        if (xValue == 0 && yValue == 0)      //  本身
        {
            direction = Direction;
        }
        else
        {
            byte b = (byte)((xValue + 1) * 10 + (yValue + 1));
            return CSMisc.dirDic[b];
        }
        return direction;
    }
}
