using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

public class CSSceneLoadManager : CSGameMgrBase2<CSSceneLoadManager>
{
    public MainEventHanlderManager EventHandler = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
    public override bool IsDonotDestroy
    {
        get
        {
            return true;
        }
    }

    private AsyncOperation mAsync = null;
    public AsyncOperation Async
    {
        get { return mAsync; }
    }

    private string mLoadingSceneName = "";
    public string LoadingSceneName
    {
        get { return mLoadingSceneName; }
        set { mLoadingSceneName = value; }
    }

    /// <summary>
    /// 从FirstScene到LoginScene,从LoginScene到MainScene
    /// </summary>
    /// <param name="sceneName"></param>
    public void loadScene(string sceneName, bool isToRoleListPanel = false)
    {
        if (CSResourceManager.Singleton.IsChangingScene == true) return;
        StartCoroutine(WaitForRequest(sceneName));
    }

    /// <summary>
    /// 从游戏里面返回
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="isToRoleListPanel"></param>
    public void loadSceneBackFromGame(string sceneName, bool isToRoleListPanel = false)
    {
        StopPassWithoutEmpty();
        StartCoroutine(WaitForRequest(sceneName));
    }

    private void StopPassWithoutEmpty()
    {
        StopAllCoroutines();
    }

    public void LoadScenePassEmptyScene(string sceneName, bool isFirstScene = false, bool isSelectChactorToMainScene = false)
    {
        if (CSResourceManager.Singleton.IsChangingScene == true)
        {
            FNDebug.LogErrorFormat(" {0}  {1} changing scene is true ",sceneName,isSelectChactorToMainScene);
            return;
        }

        if (isSelectChactorToMainScene)
        {
            StartCoroutine(WaitForLoadScenePassEmpty(sceneName, isFirstScene));
        }
        else
        {
            StartCoroutine(ChangeSceneWithoutEmptyScene(sceneName));
        }
    }

    IEnumerator ChangeSceneWithoutEmptyScene(string sceneName)
    {
        CSResourceManager.Instance.IsChangingScene = true;
        CSScaleMapSystem.Instance.Create(CSPreLoadingBase.go.transform);
        CSScaleMapSystem.Instance.PreloadingScaleMap();

        while (!CSScaleMapSystem.Instance.IsLoadedScaleMap)
        {
            yield return null;
        }

        CSResourceManager.Instance.DestroyAllResource(true);
        CSResourceManager.Instance.ResetMapTexCanDelete();
        EventHandler.SendEvent(MainEvent.ClearCSMapManager);
        EventHandler.SendEvent(MainEvent.DestroyCSScene);
        yield return null;//Destroy 延后一帧
        yield return null;//Destroy 延后一帧 少延迟一帧发现Terrain和Mesh的数据冗余了

        EventHandler.SendEvent(MainEvent.CSSceneInit);
        EventHandler.SendEvent(MainEvent.PreLoadingScene, sceneName);
        while (!CSPreLoadingBase.Instance.IsTerrainDataLoaded)
        {
            yield return null;
        }

        CSGameManager.Instance.Init();
        CSResourceManager.Instance.IsChangingScene = false;//不能放在CSScene.Sington.StartEnterScene里面，里面有逻辑判断IsChangingScene
        CSResourceManager.Instance.nextDestroyTime = Time.time + 3;
        EventHandler.SendEvent(MainEvent.StartEnterScene);

        while(!CSMainParameterManager.StartEnterSceneComplete)
        {
            yield return null;
        }
        yield return null;
        CSResourceManager.Instance.ResourcesUnloadUnusedAssets();
        yield return null;
    }

    IEnumerator WaitForLoadScenePassEmpty(string sceneName, bool isFirstScene = false)
    {
        if (mLoadingSceneName != sceneName)
        {
            CSResourceManager.Instance.IsChangingScene = true;
            mLoadingSceneName = sceneName;
            if (!isFirstScene)
            {
                CSResourceManager.Instance.DestroyAllResource();
                EventHandler.SendEvent(MainEvent.ClearCSMapManager);
            }
            mAsync = SceneManager.LoadSceneAsync("EmptyScene");
            yield return mAsync;
            yield return null;
            StartCoroutine(WaitForRequest(sceneName));
        }
        else
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError("you try to load the same Scene = " + mLoadingSceneName);
        }
    }

    IEnumerator WaitForRequest(string sceneName)
    {
        if (sceneName == "MainScene")
        {
            yield return EnterToMainScene(sceneName);
        }
        else
        {
            yield return EnterToOther(sceneName);
        }
    }

    IEnumerator EnterToOther(string sceneName)
    {
        //TODO:ddn
        CSResourceManager.Instance.IsChangingScene = true;
        CSGameManager.Instance.Init();
        EventHandler.SendEvent(MainEvent.PreLoadingScene, sceneName);
        mAsync = SceneManager.LoadSceneAsync(sceneName);
        mAsync.allowSceneActivation = false;
        yield return null;
        mAsync.allowSceneActivation = true;
        yield return mAsync;
		SceneAddStateComponent(sceneName);
        CSResourceManager.Instance.IsChangingScene = false;//在下载完主角后回调
        mLoadingSceneName = "";
    }

    IEnumerator EnterToMainScene(string sceneName)
    {
        if (CSGame.Sington == null) yield break;
        EventHandler.SendEvent(MainEvent.ShowUILoading);
        CSResourceManager.Instance.IsChangingScene = true;
        CSGameManager.Instance.Init();
        EventHandler.SendEvent(MainEvent.PreLoadingScene, sceneName);
        CSScaleMapSystem.Instance.Create(CSPreLoadingBase.go.transform);
        CSScaleMapSystem.Instance.PreloadingScaleMap();

        mAsync = SceneManager.LoadSceneAsync(sceneName);
        yield return mAsync;
        
        SceneAddStateComponent(sceneName);
        while (!CSPreLoadingBase.Instance.IsTerrainDataLoaded)
        {
            yield return null;
        }

        while (!CSScaleMapSystem.Instance.IsLoadedScaleMap)
        {
            yield return null;
        }
        CSResourceManager.Instance.IsChangingScene = false;//在下载完主角后回调
        EventHandler.SendEvent(MainEvent.StartEnterScene);
        
        while(!CSMainParameterManager.StartEnterSceneComplete)
        {
            yield return null;
        }
    }
	
    public void SceneAddStateComponent(string sceneName)
    {
        switch (sceneName)
        {
            case "LoginScene":
                GameObject state1 = GameObject.Find("LoginState");
                if (state1 != null) state1.AddComponent<CSLoginState>();
                break;
            case "MainScene":
                GameObject state2 = GameObject.Find("MainState");
                if (state2 != null) state2.AddComponent<CSMainSceneState>();
                break;
        }
    }
}
