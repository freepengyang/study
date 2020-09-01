using UnityEngine;
using System.Collections;

public class ToolPhoneStateGet
{
    //static Ping ping = null;      //Remove Unused
    //static System.Action pingEndCall = null;      //Remove Unused

    #region BatteryPower
    public static int GetBatteryLevel()
    {
        int level = -1;
#if UNITY_EDITOR
        level = -1;
#elif UNITY_ANDROID
        level = GetAndriodBatteryLevel();
#elif UNITY_IOS

#endif

        return level;
    }

    private static int GetAndriodBatteryLevel()
    {
        try
        {
            string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            return int.Parse(CapacityString);
        }
        catch (System.Exception e)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.Log("Failed to read battery power; " + e.Message);
        }
        return 0;
    }
    #endregion

    #region Signal
    public static NetworkReachability GetSignal()
    {
        return Application.internetReachability;
    }
    #endregion

}
