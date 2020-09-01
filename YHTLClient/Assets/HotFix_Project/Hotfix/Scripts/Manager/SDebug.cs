using UnityEngine;
using System.Collections;

public class SDebug
{
    public static bool developerConsoleVisible = true;
    public static void Log(object message)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.Log(message);
    }

    public static void Log(object message, Object context)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogWarning(message);
    }

    public static void LogWarning(object message, Object context)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, Object context)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogError(message, context);
    }


    public static void Break()
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.Break();
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.0f, bool depthTest = true)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }
}
