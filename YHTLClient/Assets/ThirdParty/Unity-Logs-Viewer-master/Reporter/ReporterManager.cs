using UnityEngine;

public class ReporterManager
{
    private static ReporterManager m_Instance = null;

    public static ReporterManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ReporterManager();
            }

            return m_Instance;
        }
    }

    private GameObject logObj;

    public void ShowLog()
    {
        if(logObj != null) return;
        logObj = new GameObject("log");
        GameObject uiRoot = GameObject.Find("UI Root");
        if (uiRoot != null)
        {
            NGUITools.SetParent(uiRoot.transform, logObj);
        }

        logObj.AddComponent<ReporterButtonGUI>();
    }
    
    public void OnDestroy()
    {
        if(logObj != null)
        {
            Object.Destroy(logObj);
        }
    }
}