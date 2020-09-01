
//-------------------------------------------------------------------------
//游戏状态
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;

public enum GameState//和Mono有关，无法满足需求
{
    EmptyState,
    FirstScene,
    LoginScene,
    MainScene,
    RestartScene,
}

public class CSGameState : MonoBehaviour
{
//#if UNITY_IPHONE
//    static Camera ipadBlackCamera;
//#endif
    protected GameState mState = GameState.EmptyState;
    public virtual GameState State
    {
        get { return mState; }
    }

    public void Awake()
    {
        GL.Clear(false, true, Color.black);
    }

    void Start()
    {
        Initialize();
    }

    // 初始化
    public virtual void Initialize()
    {
    }

    //销毁
    public virtual void Destroy()
    {
    }

    void OnDestroy()
    {
        Destroy();
    }

    /*public static void AddBlackCamera()
    {
#if UNITY_IPHONE
        if (ipadBlackCamera != null)
            return;
        GameObject go = new GameObject("BlackCamera");
        ipadBlackCamera = go.AddComponent<Camera>();
        ipadBlackCamera.clearFlags = CameraClearFlags.Skybox;
        ipadBlackCamera.backgroundColor = Color.black;
        ipadBlackCamera.depth = 1;
        ipadBlackCamera.cullingMask = 0;
        DontDestroyOnLoad(go);
#endif
    }*/

    /*public static void DestroyBlackCamera()
    {
#if UNITY_IPHONE
        if (ipadBlackCamera != null)
        {
            Destroy(ipadBlackCamera.gameObject);
            ipadBlackCamera = null;
        }
#endif
    }*/
}
