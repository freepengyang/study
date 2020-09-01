using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CSDebug : MonoBehaviour
{
    private static object _lock = new object();

    private static CSDebug instance;

    public static CSDebug Instance
    {
        get { return instance; }
    }

    public static CSDebug InitCallback()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject csdebug = GameObject.Find("CSDebug");
                if (csdebug == null)
                {
                    csdebug = new GameObject("CSDebug");
                    UnityEngine.Object.DontDestroyOnLoad(csdebug);
                    instance = csdebug.AddComponent<CSDebug>();
                }
                else
                    instance = csdebug.GetComponent<CSDebug>();
            }
        }

        return instance;
    }

    public static bool IsOpenDebug = false;


    // Update is called once per frame
    void Awake()
    {
    }
    
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {

            IsOpenDebug = true;
            //UIDebugPanel panel = UIManager.Instance.GetPanel<UIDebugPanel>();
            // if (panel != null)
            // {
            //     panel.HideToLeft(false);
            // }
            // else
            // {
            //     //UIManager.Instance.CreatePanel<UIDebugPanel>();
            // }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (Platform.mPlatformType)
            {
                case PlatformType.EDITOR:
                    //IsOpenDebug = false;
                    //UIDebugPanel panel = UIManager.Instance.GetPanel<UIDebugPanel>();
                    //if (panel != null) panel.HideToLeft(true);
                    break;
            }
        }

    }
#endif

    void OnConformExit()
    {
        //Application.Quit();
        QuDaoInterface.Instance.FinishGame();
    }
#if false
    void OnGUI()
    {
        if (IsOpenDebug == false)
        {
            return;
        }

        GUI.backgroundColor = Color.black;

        GUI.Box(new Rect(0, 0, Screen.width, 40), "CSDebug");
        GUI.backgroundColor = Color.white;
        GUI.Box(new Rect(0, 40, Screen.width - 10, Screen.height - 50), "");

        GUI.backgroundColor = Color.green;

        strSearch = GUI.TextField(new Rect(Screen.width - 490, 20, 400, 20), strSearch);

        if (GUI.Button(new Rect(Screen.width - 80, 20, 60, 20), "搜索"))
            Search();
        if (GUI.Button(new Rect(Screen.width - 590, 20, 100, 20), "清除高亮"))
            ClearHighLight();
        GUI.Label(new Rect(Screen.width - 750, 20, 200, 20), "带汉字的搜索请复制粘贴");

        if (GUI.Button(new Rect(0, 20, 60, 20), "清除"))
            Clear();

        if (!isStopLogWrite)
        {
            if (GUI.Button(new Rect(70, 20, 100, 20), "停止日志写入"))
                StopOrContinue();
        }
        else
        {
            if (GUI.Button(new Rect(70, 20, 100, 20), "回复日志写入"))
                StopOrContinue();
        }

        if (highlightPosList.Count > 0)
        {
            if (GUI.Button(new Rect(Screen.width - 750 - 100, 20, 100, 20), "下一个"))
                NextHighLight();
            GUI.Label(new Rect(Screen.width - 950, 20, 200, 20), "当前是第" + (curHighLightIndex + 1).ToString() + "个高亮");
            if (GUI.Button(new Rect(Screen.width - 950 - 100, 20, 100, 20), "上一个"))
                PreHighLight();
        }


        float height = 14 * lineNum;
        height = (height > Screen.height - 40) ? height : Screen.height - 80;
        GUI.SetNextControlName("MyScrollView");
        scrollPosition =
 GUI.BeginScrollView(new Rect(0, 40, Screen.width, Screen.height - 40), scrollPosition, new Rect(0, 40, Screen.width - 40, height + 40), false, false);
        GUI.backgroundColor = Color.blue;
        for (int i = 0; i < highlightPosList.Count; i++)
        {
            GUI.Button(new Rect(0, 40 + highlightPosList[i] + 4, Screen.width - 10, 14), "");//4 是button和lab的偏移像素
            GUI.Label(new Rect(Screen.width - 100, 40 + highlightPosList[i], 100, 20), (i + 1).ToString());//4 是button和lab的偏移像素
        }

        float frontHeghit = 0.0f;
        for (int i = 0; i < logList.Count; i++)
        {
            int singleLogLine = logList[i].Split('\n').Length;
            GUI.color = colorList[i];
            GUI.Label(new Rect(0, 40 + frontHeghit, Screen.width, singleLogLine * 30), logList[i]);
            frontHeghit += singleLogLine * 14;
        }

        //结束滚动视图   
        GUI.EndScrollView();
    }
#endif

    static void ClearHighLight()
    {
        strSearch = "";
        highlightPosList.Clear();
        GUI.FocusControl("MyScrollView");
        curHighLightIndex = -1;
    }

    /// <summary>
    /// 下一个
    /// </summary>
    static void NextHighLight()
    {
        curHighLightIndex++;
        if (curHighLightIndex >= highlightPosList.Count)
        {
            curHighLightIndex = 0;
        }

        float height = highlightPosList[curHighLightIndex];
        height = (height > Screen.height - 40) ? height : Screen.height - 40;
        scrollPosition = new Vector2(0, height);
    }

    /// <summary>
    /// 上一个
    /// </summary>
    static void PreHighLight()
    {
        curHighLightIndex--;
        if (curHighLightIndex < 0)
        {
            curHighLightIndex = highlightPosList.Count - 1;
        }

        float height = highlightPosList[curHighLightIndex];
        height = (height > Screen.height - 40) ? height : Screen.height - 40;
        scrollPosition = new Vector2(0, height);
    }

    static void Search()
    {
        GUI.FocusControl("MyScrollView");

        if (string.IsNullOrEmpty(strSearch))
        {
            return;
        }

        string temp = "";
        for (int i = 0; i < contentList.Count; i++)
        {
            temp += contentList[i];
        }

        temp = temp.ToLower();
        string tempStrSearch = strSearch.ToLower();
        int index = temp.IndexOf(tempStrSearch);

        if (index == -1)
        {
            return;
        }

        string[] lines = temp.Split('\n');
        highlightPosList.Clear();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].ToLower().Contains(tempStrSearch))
            {
                highlightPosList.Add(i * 14);
            }
        }
    }

    /// <summary>
    /// 清楚
    /// </summary>
    static void Clear()
    {
        lineNum = 0;
        contentList.Clear();
        scrollPosition = Vector3.zero;
        colorList.Clear();
        logList.Clear();
        ClearHighLight();
    }

    /// <summary>
    /// 停止日志写入||回复日志写入
    /// </summary>
    static void StopOrContinue()
    {
        isStopLogWrite = !isStopLogWrite;
    }

    public static void Logc(object obj)
    {
        if (isStopLogWrite)
            return;

        curConsoleType = ConsoleType.Normal;
        if (obj == null)
        {
            Output("null");
        }
        else
        {
            Output(obj.ToString());
        }
    }

    public static void Log(object obj)
    {
        if (isStopLogWrite)
            return;

        curConsoleType = ConsoleType.Normal;
        if (obj == null)
        {
            Output("null");
        }
        else
        {
            Output(obj.ToString());
        }
    }

    public static void LogWarning(object obj)
    {
        if (isStopLogWrite)
            return;
        FNDebug.LogWarning(obj);

        curConsoleType = ConsoleType.Warnning;
        if (obj == null)
        {
            Output("Warning: null");
        }
        else
        {
            Output("Warning: " + obj.ToString());
        }
    }

    public static void LogError(object obj)
    {
        if (isStopLogWrite)
            return;
        FNDebug.LogError(obj);

        curConsoleType = ConsoleType.Error;

        if (obj == null)
        {
            Output("Error: null");
        }
        else
        {
            Output("Error: " + obj.ToString());
        }

        PrintCallStack();
    }

    public static bool IsNull(object obj)
    {
        if (obj == null)
        {
            LogError(obj);
            return true;
        }

        return false;
    }

    public static void PrintCallStack() // print the application stack frame
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
        for (int i = 0; i < st.FrameCount; i++)
        {
            System.Diagnostics.StackFrame sf = st.GetFrame(i);

            Output(sf.ToString());
        }
    }

    private static void Output(string text) // output something
    {
        switch (curConsoleType)
        {
            case ConsoleType.Normal:
            {
                colorList.Add(Color.white);
            }
                break;
            case ConsoleType.Warnning:
            {
                colorList.Add(Color.yellow);
            }
                break;
            case ConsoleType.Error:
            {
                colorList.Add(Color.red);
                //前面日志的
                int ln = (text.Length - text.Replace("\n", "").Length) + 1 + lineNum;
                float height = 14 * ln;
                height = (height > Screen.height - 40) ? height : Screen.height - 40;
                scrollPosition = new Vector2(0, height);
            }
                break;
        }

        logList.Add(text);
        contentList.Add(text + "\n");
        lineNum = (text.Length - text.Replace("\n", "").Length) + 1 + lineNum;
    }

    static string strSearch = "";
    static Vector2 scrollPosition = Vector3.zero;
    static List<string> contentList = new List<string>();

    enum ConsoleType
    {
        Normal,
        Warnning,
        Error,
    }

    static ConsoleType curConsoleType;
    static List<Color> colorList = new List<Color>();
    static List<string> logList = new List<string>();
    static int lineNum = 0;
    static List<int> highlightPosList = new List<int>();
    public static bool isStopLogWrite = false;
    private static int curHighLightIndex = -1;
}