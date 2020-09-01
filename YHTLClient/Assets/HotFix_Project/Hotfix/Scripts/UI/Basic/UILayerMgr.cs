using UnityEngine;
using System;
using System.Collections.Generic;
public enum UILayerType
{
    Base = -30,

    Resident = 0,//主界面层（常驻类）
    /// <summary>
    /// 界面层
    /// </summary>
    Panel = 30,//第二层
    /// <summary>
    /// 窗口层
    /// </summary>
    Window = 60,//第三层
    /// <summary>
    /// 提示层
    /// </summary>
    Tips = 150,//第四层
    /// <summary>
    /// 新手引导层
    /// </summary>
    Guide = 180,//第五层

    /// <summary>
    /// 界面最顶层
    /// </summary>
    TopWindow = 210,//

    /// <summary>
    /// 短线重连
    /// </summary>
    Connect = 240,

    /// <summary>
    /// 弹框提示
    /// </summary>
    Hint = 270,
}
/// <summary>
/// UI层深度管理
/// </summary>
public class UILayerMgr
{
    private Dictionary<int, GameObject> mLayerDic;
    private GameObject mParent;
    private Dictionary<string, int> parentPanelDepth;

    public UILayerMgr()
    {
        mLayerDic = new Dictionary<int, GameObject>();
        parentPanelDepth = new Dictionary<string, int>();
    }
    private static UILayerMgr mInstance;
    public static UILayerMgr Instance
    {
        get
        {
            if (mInstance == null) mInstance = new UILayerMgr();
            return mInstance;
        }
        set { mInstance = value; }
    }
    public void LayerInit()
    {
        mParent = GameObject.Find("UI Root");
        GameObject.DontDestroyOnLoad(mParent);
        int nums = Enum.GetNames(typeof(UILayerType)).Length;

        for (int i = 0; i < nums; i++)
        {
            object obj = Enum.GetValues(typeof(UILayerType)).GetValue(i);
            int key = (int)((UILayerType)obj);
            if (mLayerDic.ContainsKey(key))
                mLayerDic[key] = CreateLayerGameObject(obj.ToString(), (UILayerType)obj);
            else
                mLayerDic.Add(key, CreateLayerGameObject(obj.ToString(), (UILayerType)obj));
        }
    }

    private GameObject CreateLayerGameObject(string name, UILayerType type)
    {
        GameObject layer = new GameObject(name);
        layer.transform.parent = mParent.transform;
        layer.transform.localPosition = new Vector3(0, 0, ((int)type) * -1);
        layer.transform.localScale = Vector3.one;
        return layer;
    }

    public void SetLayer(GameObject current, UILayerType type)
    {
        if (mLayerDic.Count < Enum.GetNames(typeof(UILayerType)).Length)
        {
            LayerInit();
        }
        //------------对象下所有Panle最大深度值------------------//
        int t = (int)type;
        GameObject layerobj = mLayerDic[t];
        int depth = t;
        if (!UtilityObj.GetMaxDepth(layerobj, out depth))
            depth = t;
        if (depth < t) depth = t;
        //-------------------------------------------------------//

        NGUITools.SetParent(mLayerDic[t].transform, current);
        UIPanel[] panelArr = current.GetComponentsInChildren<UIPanel>(true);
        for (int i = 0; i < panelArr.Length; i++)
        {
            UIPanel panel = panelArr[i];
            depth += 1;
            panel.depth = depth;
        }

        if (parentPanelDepth.ContainsKey(current.name))
        {
            parentPanelDepth.Remove(current.name);
        }

        parentPanelDepth.Add(current.name, depth);
    }

    public void SetLayer(Transform parent, GameObject current)
    {
        //------------对象下原本父物体Panle最大深度值------------------//
        int depth = 0;
        if (parentPanelDepth.ContainsKey(parent.name))
        {
            depth = parentPanelDepth[parent.name];
        }
        else
        {
            UIPanel parentPanel = parent.GetComponent<UIPanel>();
            if (parentPanel != null)
            {
                depth = parentPanel.depth;
                if (!parentPanelDepth.ContainsKey(current.name))
                {
                    parentPanelDepth.Add(current.name, depth);
                }
            }
        }
        //-------------------------------------------------------//
        UIPanel[] panelArr = current.GetComponentsInChildren<UIPanel>(true);
        for (int i = 0; i < panelArr.Length; i++)
        {
            UIPanel panel = panelArr[i];
            depth += 1;
            panel.depth = depth;
        }
    }

    public void SetLayerDown(GameObject current, UILayerType type)
    {
        //------------对象下所有Panle最小深度值------------------//
        int t = (int)type;
        GameObject layerobj = mLayerDic[t];
        int depth = UtilityObj.GetMinDepth(layerobj);
        //-------------------------------------------------------//
        NGUITools.SetParent(mLayerDic[t].transform, current);
        current.GetComponent<UIPanel>().depth = depth - 1;
        current.transform.localPosition = new Vector3(0, 0, depth - 1);
        if (parentPanelDepth.ContainsKey(current.name))
        {
            parentPanelDepth.Remove(current.name);
        }

        parentPanelDepth.Add(current.name, depth);
    }


    /// <summary>
    /// 气泡使用，设置从层级最大的消息向后排序
    /// </summary>
    /// <param name="current"></param>
    /// <param name="type"></param>
    public void SetLayerMaxDown(GameObject current, UILayerType type, int num = 0)
    {
        //------------对象下所有Panle最大深度值------------------//
        int t = (int)type;
        GameObject layerobj = mLayerDic[t];
        int depth = UtilityObj.GetMaxDepth(layerobj);
        //-------------------------------------------------------//
        NGUITools.SetParent(mLayerDic[t].transform, current);

        current.GetComponent<UIPanel>().depth = depth - num;
        current.transform.localPosition = new Vector3(0, 0, depth - num);
        if (parentPanelDepth.ContainsKey(current.name))
        {
            parentPanelDepth.Remove(current.name);
        }

        parentPanelDepth.Add(current.name, depth);
    }
    
}