using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHandleMainEventManager : Singleton<CSHandleMainEventManager>
{
    public static MainEventHanlderManager MainEventHandler =
        new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);


    public void Init()
    {
        MainEventHandler.AddEvent(MainEvent.ClearCSMapManager, ClearCSMapManager);
        MainEventHandler.AddEvent(MainEvent.CSSceneInit, CSSCeneInit);
        MainEventHandler.AddEvent(MainEvent.StartEnterScene, StartEnterScene);
        MainEventHandler.AddEvent(MainEvent.DestroyCSScene, DestroyCSScene);
        MainEventHandler.AddEvent(MainEvent.ShowUILoading, ShowUILoading);
        MainEventHandler.AddEvent(MainEvent.InitCSGameManager, InitCSGameManager);
        MainEventHandler.AddEvent(MainEvent.PreLoadingScene, PreLoadingScene);
        MainEventHandler.AddEvent(MainEvent.DestroyCSGameManager, DestroyCSGameManager);
    }


    private void ClearCSMapManager(uint id, object data)
    {
        CSGameManager.Instance.Clear();
    }

    private void DestroyCSScene(uint id, object data)
    {
        CSScene.Sington.Destroy();
    }

    private void CSSCeneInit(uint id, object data)
    {
        CSScene.Sington.Init();
        ////切换场景后，需要重新注册
        CSMapManager.Instance.RegisterCEvent();
    }

    private void StartEnterScene(uint id, object data)
    {
        CoroutineManager.DoCoroutine(CSScene.Sington.StartEnterScene());
    }

    private void ShowUILoading(uint id, object data)
    {
        UIBillboardPanel uiloading = UIManager.Instance.GetPanel<UIBillboardPanel>();
        if (uiloading == null) uiloading = UIManager.Instance.OpenPanel<UIBillboardPanel>();
        //uiloading.ShowLoginScene();

        UIManager.Instance.ClosePanel<UIDownloading>();
        UIManager.Instance.ClosePanel<UILoginRolePanel>();
    }

    private void InitCSGameManager(uint id, object data)
    {
        //CSGameManager.Instance.Init();
        if (CSGameManager.Instance.Root == null) return;
        Transform Root = CSGameManager.Instance.Root.transform;
        HttpRequest.CreateInstance(Root);

        CSAvatarManager.CreateInstance(Root);
        CSAudioMgr.CreateInstance(Root);
        CSSceneLoadManager.CreateInstance(Root);
        CSResourceManager.CreateInstance(Root);
        CSTouchEvent.CreateInstance(Root);
        UIItemBarManager.CreateInstance(Root);
        UIItemMoneyManager.CreateInstance(Root);
        UISabacItemManager.CreateInstance(Root);
        UIDialogBarManager.CreateInstance(Root);
    }

    private void DestroyCSGameManager(uint id, object data)
    {
        if (CSAvatarManager.Instance != null) CSAvatarManager.Instance.Destroy();
        if (CSTouchEvent.Instance != null) CSTouchEvent.Instance.Destroy();
        CSDamageInfo.Instance.ClearAllDamages();
        CSPoolManager.Dispose();
    }

    private void PreLoadingScene(uint id, object data)
    {
        string sceneName = (string) data;
        if (sceneName == "MainScene")
        {
            if (CSScene.Sington == null)
            {
                CSPreLoadingBase.CreatePreLoading();
                if (CSPreLoadingBase.Instance != null) CSScene.Sington = CSPreLoadingBase.go.AddComponent<CSScene>();
            }

            CSPreLoadingBase.Instance.PreLoadingProc();
        }
    }
}