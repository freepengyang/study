
/*************************************************************************
** File: CSInvoke.cs
** Author: jiabao
** Time: 2020.5.19
** Des: 封装了一下MonoBehaviour-->Invoke,为了适配ILRuntime
*************************************************************************/
using System;
using UnityEngine;
using System.Collections;

public class CSInvoke : MonoBehaviour {

    public System.Action action_1 = null;

    public System.Action action_2 = null;

    public void StopInvoke()
    {
        action_1 = null;
        if (IsInvoking("InvokeDelay"))
            CancelInvoke("InvokeDelay");
    }

    public void StopInvokeRepeating()
    {
        action_2 = null;
        if (IsInvoking("InvokingDelay"))
            CancelInvoke("InvokingDelay");
    }

    public void Invoke(float time, System.Action _acticon)
    {
        this.action_1 = _acticon;

        if (!this.IsInvoking("InvokeDelay"))
            this.Invoke("InvokeDelay", time);
    }

    public void InvokeRepeating(float time, float repeatRate, System.Action _acticon)
    {
        this.action_2 = _acticon;
        if (!this.IsInvoking("InvokingDelay"))
            this.InvokeRepeating("InvokingDelay", time, repeatRate);
    }

    void InvokeDelay()
    {
        if (action_1 != null) action_1();
    }

    void InvokingDelay()
    {
        if (action_2 != null) action_2();
    }

    void OnDestroy()
    {
        if (IsInvoking("InvokeDelay"))
            CancelInvoke("InvokeDelay");

        if (IsInvoking("InvokingDelay"))
            CancelInvoke("InvokingDelay");
        action_1 = null;
        action_2 = null;
    }
}
