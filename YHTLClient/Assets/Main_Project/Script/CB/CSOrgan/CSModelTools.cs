using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSModelTools
{
    /// <summary>
    /// CSMotion 数量
    /// </summary>
    public const int ENUM_MOTION_COUNT = 16;
    public static void InitAnimationFPS(int modelId, int avatarType)
    {
        //有GC问题 
        //Array array = Enum.GetValues(typeof(CSMotion));
        
        for (int i = 0; i < ENUM_MOTION_COUNT; ++i)
        {
            int key = modelId * 10 + i;

            int fps = 0;

            if (fps == 0)
            {
                if ((avatarType == EAvatarType.MainPlayer) || (avatarType == EAvatarType.Player))
                {
                    fps = GetPlayerFps(i);
                }
                else if (avatarType == EAvatarType.Monster)
                {
                    if (i == CSMotion.Attack)
                    {
                        fps = 10;
                    }
                    else if (i == CSMotion.Attack2)
                    {
                        fps = 10;
                    }
                    else if (i == CSMotion.Attack3)
                    {
                        fps = 10;
                    }
                    else if (i == CSMotion.BeAttack)
                    {
                        fps = 7;
                    }
                    else if (i == CSMotion.Dead)
                    {
                        fps = 12;
                    }
                    else if (i == CSMotion.Mining)
                    {
                        fps = 10;
                    }
                    else if (i == CSMotion.Run)
                    {
                        fps = 13;
                    }
                    else if (i == CSMotion.Stand)
                    {
                        fps = 5;
                    }
                    else if (i == CSMotion.Walk)
                    {
                        fps = 12;
                    }
                    else if (i == CSMotion.RunToStand)
                    {
                        fps = 13;
                    }
                    else if (i == CSMotion.StandToRun)
                    {
                        fps = 13;
                    }
                }
                else if (avatarType == EAvatarType.Pet)
                {
                    fps = GetPetFps(i);

                }
                else if (avatarType == EAvatarType.ZhanHun)
                {
                    fps = GetPlayerFps(i);
                }
                else if (avatarType == EAvatarType.NPC)
                {
                    fps = 6;
                }
            }

            int avaterType = (int)avatarType;

            if (avatarType == EAvatarType.ZhanHun)
            {
                avaterType = (int)EAvatarType.Player;
            }
            if (CSMisc.partsFPS.ContainsKey(avaterType))
            {
                if (CSMisc.partsFPS[avaterType].ContainsKey(i))
                {
                    CSMisc.partsFPS[avaterType].Remove(i);
                }
                CSMisc.partsFPS[avaterType].Add(i, fps);
            }
        }
    }

    public static int GetPlayerFps(int i)
    {
        int fps = 0;
        if (i == CSMotion.Attack)
        {
            fps = 20;
        }
        else if (i == CSMotion.Attack2)
        {
            fps = 9;
        }
        else if (i == CSMotion.Attack3)
        {
            fps = 20;
        }
        else if (i == CSMotion.BeAttack)
        {
            fps = 7;
        }
        else if (i == CSMotion.Dead)
        {
            fps = 10;
        }
        else if (i == CSMotion.Mining)
        {
            fps = 10;
        }
        else if (i == CSMotion.Run)
        {
            fps = 15;
        }
        else if (i == CSMotion.RunToStand)
        {
            fps = 15;
        }
        else if (i == CSMotion.StandToRun)
        {
            fps = 15;
        }
        else if (i == CSMotion.Stand)
        {
            fps = 6;
        }
        else if (i == CSMotion.Walk)
        {
            fps = 6;
        }
        return fps;
    }

    public static int GetPetFps(int i)
    {
        int fps = 0;
        if (i == CSMotion.Attack)
        {
            fps = 10;
        }
        else if (i == CSMotion.Attack2)
        {
            fps = 10;
        }
        else if (i == CSMotion.BeAttack)
        {
            fps = 7;
        }
        else if (i == CSMotion.Dead)
        {
            fps = 10;
        }
        else if (i == CSMotion.Mining)
        {
            fps = 10;
        }
        else if (i == CSMotion.Run)
        {
            fps = 13;
        }
        else if (i == CSMotion.RunToStand)
        {
            fps = 13;
        }
        else if (i == CSMotion.StandToRun)
        {
            fps = 13;
        }
        else if (i == CSMotion.Stand)
        {
            fps = 5;
        }
        else if (i == CSMotion.Walk)
        {
            fps = 26;
        }
        return fps;
    }
}
