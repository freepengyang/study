using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using AssetBundles;
using Object = UnityEngine.Object;

//UI管理
public class UIManager
{
    private static UIManager mInstance;

    public static UIManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new UIManager();
            }

            return mInstance;
        }
        set { mInstance = value; }
    }

	#region 字段

    public Transform root;
    public Stack<UIBase> showPanes;
    public List<UIBase> showPaneList; //面板集合

    public List<UIBase> cachedPanes;

	#endregion

	#region 列表

    //添加
    void Push(UIBase pane)
    {
        showPanes.Push(pane);
        showPaneList.Add(pane);
    }

    //移除
    void Pop(UIBase pane)
    {
        showPaneList.Remove(pane);
        showPanes.Pop();
    }

    //取出
    public UIBase Peek()
    {
        int peek = showPaneList.Count - 1;
        return showPaneList[peek];
    }

    /// <summary>
    /// 不采用异步加载的预制，用于小包
    /// </summary>
    public List<string> ignoreABList;

    public UIManager()
    {
        showPanes = new Stack<UIBase>();
        cachedPanes = new List<UIBase>();

        showPaneList = new List<UIBase>();

        ignoreABList = new List<string>()
        {
            "UIDownloading",
            "UIWaiting",
            "UILoading",
            "UIWaitingAB",
            "UITips",
            "ItemBase",
        };
    }

	#endregion

    public Transform GetRoot()
    {
        if (root == null)
        {
            GameObject uiroot = GameObject.Find("UI Root/Camera");
            if (uiroot != null)
            {
                root = uiroot.transform;
            }
        }

        if (root == null)
        {
            UICamera uiroot = GameObject.FindObjectOfType<UICamera>();
            if (uiroot != null)
            {
                root = uiroot.transform;
            }
        }

        return root;
    }


    /// <summary>
    /// 此处仅用于判断，，不要直接获取面板调用方法，，采用事件处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        for (int i = 0; i < showPaneList.Count; i++)
        {
            UIBase ins = showPaneList[i];
            if (ins != null)
            {
                if (ins.GetType() == type)
                {
                    return ins as T;
                }
            }
        }

        return null;
    }

    public bool IsPanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        for (int i = 0; i < showPaneList.Count; i++)
        {
            UIBase ins = showPaneList[i];
            if (ins.GetType() == type)
            {
                return true;
            }
        }

        return false;
    }


	#region 创建面板

    public void CreatePanel(string assetPath)
    {
        Type type = Type.GetType(assetPath);
        CreatePanel(type, null);
    }

    public void CreatePanel(string assetPath, Transform mtr, System.Action<UIBase> action)
    {
        Type type = Type.GetType(assetPath);
        CreatePanel(type, action);
    }

    public void CreatePanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        CreatePanel(type, null);
    }

    public void CreatePanel<T>(System.Action<UIBase> action) where T : UIBase
    {
        Type type = typeof(T);
        CreatePanel(type, action);
    }

    public void CreatePanel(Type type, System.Action<UIBase> action = null,
        System.Action<UIBase> actionPanelOpen = null)
    {
        //此处只能检测大面板是否开启
        if (!UICheckManager.Instance.DoCheckPanel(type))
        {
            return;
        }

        for (int i = 0; i < showPaneList.Count; i++)
        {
            UIBase ins = showPaneList[i];
            if (ins == null)
            {
                showPaneList.RemoveAt(i);
                continue;
            }

            if (ins.name == type.Name)
            {
                ins.Show();
                if (ins.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                if (action != null) action(ins);
                if (actionPanelOpen != null) actionPanelOpen(ins);
                return;
            }
        }

        if (cachedPanes.Count > 0)
        {
            UIBase pane = FindUIBase(cachedPanes, type); // cachedPanes.Find(p => p.GetType() == type);
            if (pane != null)
            {
                pane.Show();
                if (pane.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                cachedPanes.Remove(pane);
                Push(pane);
                if (action != null) action(pane);
                if (actionPanelOpen != null) actionPanelOpen(pane);
                return;
            }
        }

        if (CSGame.Sington.IsMiniApp && !ignoreABList.Contains(type.Name))
            CSGame.Sington.StartCoroutine(StartCreatePanel(type, action, true, actionPanelOpen));
        else
            CSGame.Sington.StartCoroutine(StartCreatePanel(type, action, false, actionPanelOpen));
    }

	#endregion

	#region 关闭面板

    /// <summary>
    /// 关闭指定面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="justPop"></param>
    public void ClosePanel<T>() where T : UIBase
    {
        ClosePanel(typeof(T), true);
    }

    public void ClosePanelImmediately<T>() where T : UIBase
    {
        ClosePanel(typeof(T), true,true);
    }

    public void ClosePanel<T>(bool isHasAudio) where T : UIBase
    {
        ClosePanel(typeof(T), isHasAudio);
    }

    public void ClosePanel(Type type)
    {
        ClosePanel(type, true);
    }

    public void ClosePanel(Type type, bool isHasAudio,bool immediately = false)
    {
        for (int i = 0; i < showPaneList.Count; i++)
        {
            UIBase ins = showPaneList[i];
            if (ins != null)
            {
                if (ins.GetType() == type)
                {
                    if (FNDebug.developerConsoleVisible) FNDebug.Log($"UIManager ClosePanel name :  {type.Name}");

                    if (ins.ShowGaussianBlur)
                        CSGaussianBlurMgr.Instance.RemovePanel(type.Name);
                    Pop(ins);

                    if (isHasAudio) CSAudioManager.Instance.Play(true, 7);
                    if (ins.PanelTweenType != PrefabTweenType.None)
                        CSScenePanelPosManager.Instance.RemovePanel(ins.name);
                    if (immediately || ins.PanelTweenType == PrefabTweenType.None || ins.PanelTweenType == PrefabTweenType.Special)
                    {
                        DestroyPanel(ins, type.Name);
                    }
                    else
                    {
                        UIPrefabTweenManager.Instance.PlayTweenClose(ins.PanelTweenType, ins.UIPrefab,
                            () => { DestroyPanel(ins, type.Name); });
                    }

                    break;
                }
            }
        }
    }

    private void DestroyPanel(UIBase ins, string name, bool isForceClose = false)
    {
        if (ins == null) return;

        HotManager.Instance.EventHandler.SendEvent(CEvent.ClosePanel, ins);

        if (!ins.Cached || isForceClose)
        {
            ins.Destroy();
            UIGameObjects.Entry en = FindEntry(name);
            if (en != null) AssetBundleManager.UnloadAssetBundle(en.path);
        }
        else
        {
            ins.OnRecycle();
            cachedPanes.Add(ins);
        }
    }

    /// <summary>
    /// 关闭所有面板   强制关闭所有页面 不加动画
    /// </summary>
    public void CloseAllPanel(List<Type> exceptList = null, bool isForceClose = false)
    {
        CSGaussianBlurMgr.Instance.RemoveAll();
        for (int i = showPaneList.Count - 1; i >= 0; i--)
        {
            UIBase ins = showPaneList[i];
            if (ins.GetType() == typeof(UIDebugPanel)) continue;
            if (exceptList != null && exceptList.Contains(ins.GetType())) continue;

            Pop(ins);

            if (isForceClose || ins.PanelTweenType == PrefabTweenType.None ||
                ins.PanelTweenType == PrefabTweenType.Special)
            {
                DestroyPanel(ins, ins.GetType().Name, isForceClose);
            }
            else
            {
                UIPrefabTweenManager.Instance.PlayTweenClose(ins.PanelTweenType, ins.UIPrefab,
                    () => { DestroyPanel(ins, ins.GetType().Name, isForceClose); });
            }
        }

        if (isForceClose)
        {
            if (cachedPanes != null)
            {
                for (int i = cachedPanes.Count - 1; i >= 0; i--)
                {
                    UIBase ins = cachedPanes[i];
                    if (ins.GetType() == typeof(UIDebugPanel)) continue;
                    if (exceptList != null && exceptList.Contains(ins.GetType())) continue;
                    cachedPanes.RemoveAt(i);
                    var instanceType = ins.GetType();
                    ins.Destroy();
                    UIGameObjects.Entry en = UIGameObjects.Instance.gos.Find(p => p.type == instanceType);
                    if (en != null) AssetBundleManager.UnloadAssetBundle(en.path);
                }
            }
        }
    }

    /// <summary>
    /// 打开功能区关闭非常驻面板， 现在默认关闭所有非 Resident层面板  exceptList 是其它层级特殊不关闭面板
    /// </summary>
    /// <param name="exceptList"></param>
    public void CloseAllPanelByFunc()
    {
        for (int i = showPaneList.Count - 1; i >= 0; i--)
        {
            UIBase ins = showPaneList[i];
            if (ins.PanelLayerType == UILayerType.Resident ||
                (UtilityPanel.OpenFuncExceptList != null &&
                 UtilityPanel.OpenFuncExceptList.Contains(ins.GetType()))) continue;

            Pop(ins);
            if (ins.PanelTweenType == PrefabTweenType.None || ins.PanelTweenType == PrefabTweenType.Special)
            {
                DestroyPanel(ins, ins.GetType().Name);
            }
            else
            {
                UIPrefabTweenManager.Instance.PlayTweenClose(ins.PanelTweenType, ins.UIPrefab,
                    () => { DestroyPanel(ins, ins.GetType().Name); });
            }
        }
    }

	#endregion

	#region 特殊需求创建面板，其他误用

    //[System.Obsolete("Use 'CreatePanel' instead")]
    public T OpenPanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        UIBase b = OpenPanelByType(type);
        if (!UICheckManager.Instance.DoCheckPanel(type))
        {
            ClosePanel<T>();
        }

        return b as T;
    }

	#endregion

	#region 加载预制

    /// <summary>
    /// 加载UI, 直接调用此方法的，在预制全部销毁时，要手动调用ab卸载
    /// AssetBundleManager.UnloadAssetBundle(_name);
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject loadUIPanel(string _name, Transform parent)
    {
        bool isMiniLoad = CSGame.Sington.IsMiniApp && !UIManager.Instance.ignoreABList.Contains(_name);
        GameObject obj = AssetBundleLoad.LoadUIAsset(_name, isMiniLoad);

        if (obj != null)
        {
            GameObject inst = Object.Instantiate(obj);
            if (inst != null)
            {
                inst.SetActive(true);
                NGUITools.SetParent(parent, inst);
                return inst;
            }
        }

        Resources.UnloadAsset(obj);

        Object.Destroy(obj);
        obj = null;

        return null;
    }

	#endregion


	#region Private

    UIBase FindUIBase(List<UIBase> list, Type type)
    {
        if (list == null) return null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType() == type)
                return list[i];
        }

        return null;
    }

    UIGameObjects.Entry FindEntry(string typeName)
    {
        for (int i = 0; i < UIGameObjects.Instance.gos.Count; i++)
        {
            if (UIGameObjects.Instance.gos[i].typeName == typeName)
            {
                return UIGameObjects.Instance.gos[i];
            }
        }

        return null;
    }

    private IEnumerator StartCreatePanel(Type type, System.Action<UIBase> action, bool isAsync = false,
        System.Action<UIBase> actionPanelOpen = null)
    {
        UIGameObjects.Entry en = FindEntry(type.Name);

        if (en == null)
        {
            FNDebug.LogError($"找不到界面  {type.Name}");
            yield break;
        }

        if (!string.IsNullOrEmpty(en.path))
        {
#if UNITY_EDITOR
            FNDebug.Log($"UIManager CreatePanel name :  {type.Name}");
#endif
            GameObject go = null;
            go = CSGameManager.Instance.GetStaticObj(type.Name) as GameObject;

            if (isAsync && go == null)
            {
                yield return AssetBundleManager.LoadUIAssetToAsync(en.path);
            }

            if (go == null) go = loadUIPanel(en.path, GetRoot());

            if (go != null)
            {
                go.name = type.Name;
                UIBase ret = Activator.CreateInstance(type) as UIBase;
                if (ret == null)
                {
                    FNDebug.Log($"UIManager StartCreatePanel: ret null {go.name}  --  {en.typeName}");
                    yield break;
                }

                Push(ret);
                ret.UIPrefab = go;
                UILayerMgr.Instance.SetLayer(go, ret.PanelLayerType);
                ret.Init();
                if (ret.PanelTweenType != PrefabTweenType.None) CSScenePanelPosManager.Instance.AddPanel(ret.name);

                yield return null;

                if (ret.Disposed)
                    yield break;

                if (ret.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                ret.Show();
                if (action != null) action(ret);

                if (ret.Disposed)
                    yield break;
                //没有动画，直接发送打开页面时间，特殊动画自己发送，其他在动画播放完成后发送
                if (ret.PanelTweenType == PrefabTweenType.None || ret.PanelTweenType == PrefabTweenType.Special)
                {
                    if (actionPanelOpen != null) actionPanelOpen(ret);
                    HotManager.Instance.EventHandler.SendEvent(CEvent.OpenPanel, ret);
                }
                else
                {
                    ret.Conceal();
                    yield return null;
                    UIPrefabTweenManager.Instance.PlayTweenShow(ret.PanelTweenType, go, () =>
                    {
                        if (actionPanelOpen != null) actionPanelOpen(ret);
                        HotManager.Instance.EventHandler.SendEvent(CEvent.OpenPanel, ret);
                    });
                }
            }
            else if (FNDebug.developerConsoleVisible) FNDebug.LogError("can't find the prefab in " + en.path);
        }
        else if (FNDebug.developerConsoleVisible) FNDebug.LogError("UIGameObjects asset is not correct");
    }

    UIBase OpenPanelByType(Type type)
    {
        for (int i = 0; i < showPaneList.Count; i++)
        {
            UIBase ins = showPaneList[i];
            if (ins == null)
            {
                showPaneList.RemoveAt(i);
                continue;
            }

            if (ins.name == type.Name)
            {
                ins.Show();
                if (ins.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                return ins;
            }
        }

        if (cachedPanes.Count > 0)
        {
            UIBase pane = FindUIBase(type);
            if (pane != null)
            {
                pane.Show();
                if (pane.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                cachedPanes.Remove(pane);
                Push(pane);
                return pane;
            }
        }

        //create new instance
        UIGameObjects.Entry en = FindEntry(type.Name);
        if (!string.IsNullOrEmpty(en.path))
        {
            GameObject go = null;

            go = CSGameManager.Instance.GetStaticObj(type.Name) as GameObject;

            if (go == null) go = loadUIPanel(en.path, GetRoot());
            if (go != null)
            {
                go.name = type.Name;
                UIBase ret = Activator.CreateInstance(type) as UIBase;
                //添加
                Push(ret);
                ret.UIPrefab = go;
                if (ret.ShowGaussianBlur)
                    CSGaussianBlurMgr.Instance.AddPanel(type.Name);
                ret.Init();
                ret.Show();
                UILayerMgr.Instance.SetLayer(go, ret.PanelLayerType);
                return ret;
            }
            else if (FNDebug.developerConsoleVisible) FNDebug.LogError("can't find the prefab in " + en.path);
        }
        else if (FNDebug.developerConsoleVisible) FNDebug.LogError("UIGameObjects asset is not correct");

        return null;
    }

    private UIBase FindUIBase(Type type)
    {
        for (int i = 0; i < cachedPanes.Count; i++)
        {
            if (cachedPanes[i].GetType() == type) return cachedPanes[i];
        }

        return null;
    }

	#endregion
}