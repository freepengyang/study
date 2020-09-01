using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class UICoroutineManager : MonoSingleton<UICoroutineManager>
{
    #region Show/Hide
    public void DelayHide(GameObject go, float delay)
    {
        StartCoroutine(CoDelayHide(go, delay));
    }
    IEnumerator CoDelayHide(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        NGUITools.SetActive(go, false);
    }

    public void DelayShow(GameObject go, float delay)
    {
        StartCoroutine(CoDelayShow(go, delay));
    }
    IEnumerator CoDelayShow(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go!=null)
        {
            NGUITools.SetActive(go, true);
        }
    }
    #endregion

    public void RunCoroutine(IEnumerator routine)
    {
        if (gameObject != null && gameObject.activeSelf)
        {
            StartCoroutine(routine);
        }
    }

    public void DelayDestroy(GameObject go, float delay)
    {
        StartCoroutine(CoDelayDestroy(go, delay));

    }
    IEnumerator CoDelayDestroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(go);
    }

    public void DelayRun(Action a, float delay)
    {
        StartCoroutine(CoDelayRun(a, delay));
    }
    IEnumerator CoDelayRun(Action a, float delay)
    {
        yield return new WaitForSeconds(delay);
        a();
    }


}

