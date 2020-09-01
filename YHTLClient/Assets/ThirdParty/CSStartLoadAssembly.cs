using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine.Networking;

public class CSStartLoadAssembly : MonoBehaviour
{
    public bool IsDebugLog;
    public bool IsLoadLocalRes;
    public bool IsRelease;
    public bool IsMiniApp;
    public bool IsMonsterHeadShowConfigID;

    public Assembly assembly;

    private static CSStartLoadAssembly mSington;

    public static CSStartLoadAssembly Sington
    {
        get { return mSington; }
    }

    public void Awake()
    {
        mSington = this;
        StartCoroutine(LoadHotFixAssembly());
    }

    IEnumerator LoadHotFixAssembly()
    {
#if ILRuntime
        Type CSGame = Type.GetType("CSGame");
        //Type CSGame = Activator.CreateInstance("CSGame");// this.assembly.GetType("CSGame");
        Type FirstState = Type.GetType("CSFirstState");//this.assembly.GetType("CSFirstState");
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach(Assembly assembly in assemblies)
        {
            if(assembly != null && assembly.GetName().Name == "UIMainResPanel")
            {
                CSGame = assembly.GetType("CSGame");
                FirstState = assembly.GetType("CSFirstState");
                break;
            }
        }
        GameObject FirstStateObj = GameObject.Find("FirstState");
        GameObject GameStateObj = GameObject.Find("GameState");
        if (FirstStateObj != null && GameStateObj != null)
        {
            FirstStateObj.AddComponent(FirstState);
            GameStateObj.AddComponent(CSGame);
        }
        else
        {
            Debug.LogError(
                $"FirstStateObj  == null ?  {FirstStateObj == null}    GameStateObj  == null ?  {GameStateObj == null}");
        }

        yield break;



#else
        byte[] res;
        UnityWebRequest www;
#if UNITY_EDITOR
        string dllURL = ResourceLoadManager.instance.GetURL("UIMainResPanel.dll");
        res = File.ReadAllBytes(dllURL);
#else
        res = ResourceLoadManager.instance.GetResource("UIMainResPanel");
        if (res == null)
        {
            UnityEngine.Debug.LogError($"TextAsset  error  CSStartLoadAssenbly:  "); 
            yield break;
        }
#endif


        if (!IsRelease)
        {
            byte[] pdb;
            string pdbURL = ResourceLoadManager.instance.GetURL("UIMainResPanel.pdb");
#if UNITY_EDITOR
            pdb = File.ReadAllBytes(pdbURL);
#else
            www = UnityWebRequest.Get(pdbURL);
            yield return www.SendWebRequest();
            if (!string.IsNullOrEmpty(www.error))
                UnityEngine.Debug.LogError(www.error);
            pdb = www.downloadHandler.data;
            www.Dispose();
#endif
            assembly = Assembly.Load(res, pdb);
        }
        else
        {
            assembly = Assembly.Load(res);
        }

        Type CSGame = this.assembly.GetType("CSGame");
        Type FirstState = this.assembly.GetType("CSFirstState");

        GameObject FirstStateObj = GameObject.Find("FirstState");
        GameObject GameStateObj = GameObject.Find("GameState");
        if (FirstStateObj != null && GameStateObj != null)
        {
            FirstStateObj.AddComponent(FirstState);
            GameStateObj.AddComponent(CSGame);
        }
        else
        {
            Debug.LogError(
                $"FirstStateObj  == null ?  {FirstStateObj == null}    GameStateObj  == null ?  {GameStateObj == null}");
        }

        yield break;
#endif
    }
}