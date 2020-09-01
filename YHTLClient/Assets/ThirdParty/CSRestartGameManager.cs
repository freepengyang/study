using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSRestartGameManager : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(DestroyResource());
    }

    IEnumerator DestroyResource()
    {
        GameObject go = GameObject.Find("GameManager");
        GameObject go2 = GameObject.Find("(AndroidSDKCallback)");
        GameObject go3 = GameObject.Find("GameState");
        GameObject go4 = GameObject.Find("OutLog");
        GameObject go5 = GameObject.Find("CoroutineManager");
        GameObject go6 = GameObject.Find("UI Root");

        if (go != null) GameObject.Destroy(go);
        if (go2 != null) GameObject.Destroy(go2);
        if (go4 != null) GameObject.Destroy(go4);
        if (go5 != null) GameObject.Destroy(go5);
        if (go3 != null) GameObject.Destroy(go3);
        if (go6 != null) GameObject.Destroy(go6);
        yield return null;
        yield return null;

        Resources.UnloadUnusedAssets();

        yield return new WaitForSeconds(0.1f);

        GC.Collect();

        yield return new WaitForSeconds(0.1f);

#if !UNITY_EDITOR && UNITY_ANDROID
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("runOnUiThread", new AndroidJavaRunnable(delegate() { jo.Call("restartGame"); }));
        }
#endif
    }

    IEnumerator EnterToFirstScene()
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.Debug.Log("======> EnterToFirstScene");
        //TODO:ddn
        AsyncOperation mAsync = SceneManager.LoadSceneAsync("FirstScene");
        mAsync.allowSceneActivation = false;
        yield return null;
        mAsync.allowSceneActivation = true;
        yield return mAsync;
    }
}