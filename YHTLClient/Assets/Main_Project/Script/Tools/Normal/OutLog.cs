using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OutLog : MonoBehaviour
{
    private static object _lock = new object();
    private static OutLog instance;

    public static OutLog Instance
    {
        get { return instance; }
    }

    public static OutLog InitCallback()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject outlog = GameObject.Find("OutLog");
                if (outlog == null)
                {
                    outlog = new GameObject("OutLog");
                    UnityEngine.Object.DontDestroyOnLoad(outlog);
                    instance = outlog.AddComponent<OutLog>();
                }
                else
                    instance = outlog.GetComponent<OutLog>();
            }
        }

        return instance;
    }

    void Start()
    {
#if !UNITY_EDITOR
        Application.logMessageReceived -= HandleLog;
        Application.logMessageReceived += HandleLog;
#endif
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string statckTrace, LogType type)
    {
        if (type == LogType.Assert || type == LogType.Exception || type == LogType.Error)
        {
#if !UNITY_EDITOR
            StartCoroutine(StartPostLog(logString, statckTrace, type));
#endif

            if (CSDebug.IsOpenDebug)
            {
                UIDebugInfo.BeginGroup(ELogToggleType.Exception, type.ToString() + " " + logString, ELogColorType.Red);
                string[] strs = statckTrace.Split('\n');
                for (int i = 0; i < strs.Length; i++)
                {
                    UIDebugInfo.AddLog(ELogToggleType.Exception, strs[i], ELogColorType.Red);
                }

                UIDebugInfo.EndGroup();
            }
        }
    }

    IEnumerator StartPostLog(string logString, string statckTrace, LogType type)
    {
        WWWForm sum = new WWWForm();
        //TODO:ddn
        sum.AddField("name----- ", CSConstant.MainPlayerName);
        if (statckTrace.Length > 1000) statckTrace = statckTrace.Substring(0, 1000);
        sum.AddField("platformid == ", CSConstant.platformid);
        sum.AddField("graphicsDeviceName == ", SystemInfo.graphicsDeviceName);
        sum.AddField("deviceName == ", SystemInfo.deviceName);
        sum.AddField("systemMemorySize == ", SystemInfo.systemMemorySize);
        sum.AddField("mSeverId == ", CSConstant.mSeverId);
        sum.AddField("logString == ", logString);
        sum.AddField("stackTrace == ", statckTrace);
        UnityWebRequest ww2 = UnityWebRequest.Post(AppUrlMain.errorUrl, sum);
        yield return ww2.SendWebRequest();
        ww2.Dispose();
    }
}