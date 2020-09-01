using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// how to use:
/// public class CSTest
/// {
///     public IEnumerator DoAction()
///        {
///            yield return new WaitForSeconds(5);
///            Debug.Log("AAAAA");
///            yield return new WaitForSeconds(5);
///            Debug.Log("BBBBB");
///        }
///        
///     public void TestMe()
///     {
///         CoroutineManager.DoCoroutine(DoAction());
///     }
/// }
/// 
/// 
/// </summary>
public class CoroutineManager
{
    internal class CoroutineManagerMonoBehaviour : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        private void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }

    private static CoroutineManagerMonoBehaviour _CoroutineManagerMonoBehaviour;
    static CoroutineManager()
    {
        Init();
    }

    public static Coroutine DoCoroutine(IEnumerator routine)
    {
        return _CoroutineManagerMonoBehaviour.StartCoroutine(routine);
    }

    public static Coroutine DoCoroutine(string enumerator, object value)
    {
        return _CoroutineManagerMonoBehaviour.StartCoroutine(enumerator, value);
    }

    public static void StopCoroutine(IEnumerator routine)
    {
        if (applicationIsQuitting)
        {
            return;
        }
        _CoroutineManagerMonoBehaviour.StopCoroutine(routine);
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        if (applicationIsQuitting)
        {
            return;
        }
        _CoroutineManagerMonoBehaviour.StopCoroutine(coroutine);
    }

    public static void StopCoroutine(string methodName)
    {
        _CoroutineManagerMonoBehaviour.StopCoroutine(methodName);
    }
    
    public static void StopAllCoroutine()
    {
        _CoroutineManagerMonoBehaviour.StopAllCoroutines();
    }


    public static void Init()
    {
        if (applicationIsQuitting)
        {
            return;
        }
        if (CoroutineManagerIsCreate)
        {
            return;
        }
        CoroutineManagerIsCreate = true;
        var go = new GameObject();
        go.name = "CoroutineManager";
        _CoroutineManagerMonoBehaviour = go.AddComponent<CoroutineManagerMonoBehaviour>();
        GameObject.DontDestroyOnLoad(go);
    }

    private static bool applicationIsQuitting = false;
    private static bool CoroutineManagerIsCreate = false;
    internal static object instance;
}
