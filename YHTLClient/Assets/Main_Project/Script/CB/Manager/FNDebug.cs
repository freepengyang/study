using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class FNDebug
{
    public static bool developerConsoleVisible = true;
    public static bool isDebugMode = true;

    [Conditional("EnableLog")]
    public static void Log(object message)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.Log(message);
    }

    [Conditional("EnableLog")]
    public static void Log(object message, Object context)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.Log(message, context);
    }

    [Conditional("EnableLog")]
    public static void LogWarning(object message)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("EnableLog")]
    public static void LogFormat(UnityEngine.Object message, string format, params object[] args)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogFormat(message, format, args);
    }

    [Conditional("EnableLog")]
    public static void LogFormat(string format, params object[] args)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogFormat(format, args);
    }

    [Conditional("EnableLog")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    [Conditional("EnableLog")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    [Conditional("EnableLog")]
    public static void LogWarning(object message, Object context)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogWarning(message, context);
    }

    [Conditional("EnableLog")]
    public static void LogError(object message)
    {
        if (!developerConsoleVisible) return;
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("EnableLog")]
    public static void LogError(object message, Object context)
    {
        if (!developerConsoleVisible) return;

        UnityEngine.Debug.LogError(message, context);
    }
}
