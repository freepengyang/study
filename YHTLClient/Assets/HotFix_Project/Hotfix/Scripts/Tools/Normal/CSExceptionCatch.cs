using UnityEngine;
using System.Collections;

public class CSExceptionCatch : MonoBehaviour {
    //int mFrameCount = 10; 
    public string message = "";
	void OnEnable()
    {
        //CSSendMailUtil.StartSend2();
        //Application.RegisterLogCallback(HandleLog);
    }

    void Update()
    {
        //if (Time.frameCount % mFrameCount == 0)
        //{
        //    Debug.LogError("xxxxxx");
        //}
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string str = "logString = " + logString + " stackTrace=" + stackTrace + " logType = " + type;
        if (str != message)
        {
            message = str;
            //CSSendMailUtil.StartSend();
        }
    }

    void OnDisable()
    {
        Application.RegisterLogCallback(null); 
        //Application.logMessageReceived += HandleLog;
    }
}
