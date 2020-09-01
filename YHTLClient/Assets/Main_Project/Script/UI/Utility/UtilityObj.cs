using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityObj
{
    public static Transform FindChildren(Transform parent, string name)
    {
        if (parent == null) return null;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == name)
            {
                return parent.GetChild(i);
            }
            else
            {
                Transform trs = FindChildren(parent.GetChild(i), name);
                if (trs != null)
                {
                    return trs;
                }
            }
        }

        return null;
    }

    public static T Get<T>(Transform parent, string path) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        Transform transform = parent.Find(path);

        if (transform != null)
        {
            GameObject go = transform.gameObject;
            if (go)
            {
                T obj = go.GetComponent<T>();
                if (obj)
                {
                    return obj;
                }
            }
        }

        return null;
    }

    public static T GetOrAdd<T>(Transform parent, string path) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        Transform transform = parent.Find(path);

        if (transform != null)
        {
            GameObject go = transform.gameObject;
            if (go)
            {
                T obj = go.GetComponent<T>();

                if(obj == null)
                {
                    obj = go.AddComponent<T>();
                }
                if (obj)
                {
                    return obj as T;
                }
            }
        }

        return null;
    }

    static public T GetInstantiate<T>(T obj, Transform parent) where T : Component
    {
        if (obj != null && parent != null)
            return GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity, parent) as T;

        return null;
    }

    public static T GetObject<T>(Transform parent, string path) where T : UnityEngine.Object
    {
        if (parent == null) return null;

        Transform transform = parent.Find(path);
        if (transform == null) return null;

        if (typeof(T) == typeof(Transform)) return transform as T;

        if (typeof(T) == typeof(GameObject)) return transform.gameObject as T;
        return transform.GetComponent<T>();
    }

    public static T Get<T>(Transform parent, string name, ref T obj) where T : UnityEngine.Object
    {
        return obj ?? (obj = GetObject<T>(parent, name));
    }

    public static Transform Get(string _path, Transform parent = null)
    {
        if (parent == null)
        {
            return null;
        }
        return parent.Find(_path);
    }
    
    /// <summary>
    /// 获取组件中最大的层级
    /// </summary>
    /// <param name="panelGroup"></param>
    /// <returns></returns>
    public static int GetMaxDepth(GameObject panelGroup)
    {
        UIPanel[] panels = panelGroup.GetComponentsInChildren<UIPanel>(true);
        int maxDepth = -100;
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0) maxDepth = panels[i].depth;
            else
            {
                maxDepth = Mathf.Max(panels[i].depth, maxDepth);
            }
        }

        return maxDepth;
    }

    /// <summary>
    /// 获取组件中最大的层级
    /// </summary>
    /// <param name="panelGroup"></param>
    /// <param name="maxDepth"></param>
    /// <returns></returns>
    public static bool GetMaxDepth(GameObject panelGroup, out int maxDepth)
    {
        UIPanel[] panels = panelGroup.GetComponentsInChildren<UIPanel>(true);
        maxDepth = 0;
        if (panels == null || panels.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0) maxDepth = panels[i].depth;
            else
            {
                maxDepth = Mathf.Max(panels[i].depth, maxDepth);
            }
        }

        return true;
    }

    /// <summary>
    /// 获取组件中最小的层级
    /// </summary>
    /// <param name="panelGroup"></param>
    /// <returns></returns>
    public static int GetMinDepth(GameObject panelGroup)
    {
        UIPanel[] panels = panelGroup.GetComponentsInChildren<UIPanel>(true);
        int minDepth = -100;
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0) minDepth = panels[i].depth;
            else
            {
                minDepth = Mathf.Min(panels[i].depth, minDepth);
            }
        }

        return minDepth;
    }

}
