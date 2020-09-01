using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[System.Serializable]
public class ScriptBinderItem
{
    public int iHashCode;
    public UnityEngine.Object component;
    public string varName;
    public bool locked;
}

[System.Serializable]
public class ScriptStateItem
{
    public int iHashCode;
    public string statusName;
    public UnityEvent action;
    public bool locked;
}

public class ScriptBinder : MonoBehaviour
{
    [HideInInspector]
    public string labelSpace = string.Empty;
    [HideInInspector]
    public int labelTypeId = -1;
    public string[] argumentsString = new string[0];
    public int[] argumentsInt = new int[0];
    public float[] argumentsFloat = new float[0];
    [HideInInspector]
    public ScriptBinderItem[] scriptItems = new ScriptBinderItem[0];
    [HideInInspector]
    public ScriptStateItem[] scriptStatus = new ScriptStateItem[0];
    [SerializeField]
    public int[] moneyIds = new int[0];
    //[HideInInspector]
    //public ClientFrame mFrameHandle = null;
    public System.Action onFrameClosed = null;

    public UnityAction onStart;

    protected bool destroyed = false;

    protected void Start()
    {
        if (null != onStart)
        {
            onStart.Invoke();
            onStart = null;
        }
    }

    protected int findItemLabel(string key)
    {
        for (int i = 0; i < scriptItems.Length; ++i)
        {
            if (key == scriptItems[i].varName)
            {
                return i;
            }
        }
        return -1;
    }

    protected int findStatusLabel(string key)
    {
        for (int i = 0; i < scriptStatus.Length; ++i)
        {
            if (key == scriptStatus[i].statusName)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetText(string key, string value)
    {
        var find = findItemLabel(key);
        if (find >= 0 && find < scriptItems.Length)
        {
            var text = scriptItems[find].component as Text;
            if (null != text)
            {
                text.text = value;
                return;
            }
        }

        Debug.LogErrorFormat("ComScriptBinder SetText label = {0} error !!!", key);
    }

    public void _SetAction(string key)
    {
        var find = findStatusLabel(key);
        if (find >= 0 && find < scriptStatus.Length)
        {
            if (null != scriptStatus[find].action)
            {
                scriptStatus[find].action.Invoke();
                return;
            }
        }

        Debug.LogErrorFormat("ComScriptBinder SetAction label = {0} is failed!!!", key);
    }

    public void SetText(string key, int argumentsIndex)
    {
        if (argumentsIndex >= 0 && argumentsIndex < argumentsString.Length)
        {
            SetText(key, argumentsString[argumentsIndex]);
            return;
        }
        Debug.LogErrorFormat("ComScriptBinder SetText argumentsIndex = {0} is out of range !!!", argumentsIndex);
    }

    public T GetScript<T>(string key) where T : Component
    {
        var find = findItemLabel(key);
        if (find >= 0 && find < scriptItems.Length)
        {
            return scriptItems[find].component as T;
        }

        Debug.LogErrorFormat("ComScriptBinder GetScript label = {0} error !!!", key);
        return null;
    }

    public UnityEngine.Object GetObject(string key)
    {
        var find = findItemLabel(key);
        if (find >= 0 && find < scriptItems.Length)
        {
            return scriptItems[find].component;
        }
        return null;
    }
    public int GetIntArgv(int index)
    {
        if (index >= 0 && index < argumentsInt.Length)
        {
            return argumentsInt[index];
        }

        Debug.LogErrorFormat("GetIntArgv index ={0} is out of range len = {1}!", index, argumentsInt.Length);
        return 0;
    }

    public string GetStringArgv(int index)
    {
        if (index >= 0 && index < argumentsString.Length)
        {
            return argumentsString[index];
        }

        Debug.LogErrorFormat("GetStringArgv index = {0} is out of range len = {1}!", index, argumentsString.Length);
        return string.Empty;
    }

    public float GetFloatArgv(int index)
    {
        if (index >= 0 && index < argumentsFloat.Length)
        {
            return argumentsFloat[index];
        }

        Debug.LogErrorFormat("GetFloatArgv index = {0} is out of range len = {1}!", index, argumentsFloat.Length);
        return 0.0f;
    }

    public void RegisterButtonEvent(string key, UnityAction callback)
    {
        Button button = GetObject(key) as Button;
        if (null != button && null != callback)
        {
            button.onClick.AddListener(callback);
        }
    }

    public void UnRegisterButtonEvent(string key, UnityAction callback)
    {
        Button button = GetObject(key) as Button;
        if (null != button && null != callback)
        {
            button.onClick.RemoveListener(callback);
        }
    }

    public void RegisterToggleEvent(string key, UnityAction<bool> callback)
    {
        Toggle toggle = GetObject(key) as Toggle;
        if (null != toggle)
        {
            toggle.onValueChanged.AddListener(callback);
        }
    }

    public void UnRegisterToggleEvent(string key, UnityAction<bool> callback)
    {
        Toggle toggle = GetObject(key) as Toggle;
        if (null != toggle)
        {
            toggle.onValueChanged.RemoveListener(callback);
        }
    }

    public void OnClickLinkInfo(string argv)
    {
        if (!string.IsNullOrEmpty(argv))
        {
            //ActiveManager.GetInstance().OnClickLinkInfo(argv);
        }
    }

    #region Invoke   InvokeRepeating
    public System.Action action_base = null;
    public System.Action onceAction_1 = null;
    public System.Action onceAction_2 = null;
    public System.Action onceAction_3 = null;
    public System.Action onceAction_4 = null;
    public System.Action repeatAction_1 = null;
    public System.Action repeatAction_2 = null;
    public void StopInvoke()
    {
        onceAction_1 = null;
        if (IsInvoking("InvokeDelay"))
            CancelInvoke("InvokeDelay");
    }
    public void StopInvoke2()
    {
        onceAction_2 = null;
        if (IsInvoking("InvokeDelay2"))
            CancelInvoke("InvokeDelay2");
    }
    public void StopInvoke3()
    {
        onceAction_3 = null;
        if (IsInvoking("InvokeDelay3"))
            CancelInvoke("InvokeDelay3");
    }
    public void StopInvoke4()
    {
        onceAction_4 = null;
        if (IsInvoking("InvokeDelay4"))
            CancelInvoke("InvokeDelay4");
    }

    public void StopInvokeRepeating()
    {
        repeatAction_1 = null;
        if (IsInvoking("InvokingDelay"))
            CancelInvoke("InvokingDelay");
    }

    public void StopInvokeRepeating2()
    {
        repeatAction_2 = null;
        if (IsInvoking("InvokingDelay2"))
            CancelInvoke("InvokingDelay2");
    }

    /// <summary>
    /// 基类专用
    /// </summary>
    public void StopInvokeBase()
    {
        this.action_base = null;
        if (this.IsInvoking("InvokeBaseDelay"))
            this.CancelInvoke("InvokeBaseDelay");
    }

    /// <summary>
    /// 基类专用
    /// </summary>
    /// <param name="time"></param>
    /// <param name="_acticon"></param>
    public void InvokeBase(float time, System.Action _acticon)
    {
        this.action_base = _acticon;
        if (!this.IsInvoking("InvokeBaseDelay"))
            this.Invoke("InvokeBaseDelay", time);
    }

    void InvokeBaseDelay()
    {
        this.action_base?.Invoke();
        this.action_base = null;
    }

    public void Invoke(float time, System.Action _acticon)
    {
        this.onceAction_1 = _acticon;
        if (!this.IsInvoking("InvokeDelay"))
            this.Invoke("InvokeDelay", time);
    }

    public void Invoke2(float time, System.Action _acticon)
    {
        this.onceAction_2 = _acticon;
        if (!this.IsInvoking("InvokeDelay2"))
            this.Invoke("InvokeDelay2", time);
    }

    public void Invoke3(float time, System.Action _acticon)
    {
        this.onceAction_3 = _acticon;
        if (!this.IsInvoking("InvokeDelay3"))
            this.Invoke("InvokeDelay3", time);
    }

    public void Invoke4(float time, System.Action _acticon)
    {
        this.onceAction_4 = _acticon;
        if (!this.IsInvoking("InvokeDelay4"))
            this.Invoke("InvokeDelay4", time);
    }

    public void InvokeRepeating(float time, float repeatRate, System.Action _acticon)
    {
        this.repeatAction_1 = _acticon;
        if (!this.IsInvoking("InvokingDelay"))
            this.InvokeRepeating("InvokingDelay", time, repeatRate);
    }

    public void InvokeRepeating2(float time, float repeatRate, System.Action _acticon)
    {
        this.repeatAction_2 = _acticon;
        if (!this.IsInvoking("InvokingDelay2"))
            this.InvokeRepeating("InvokingDelay2", time, repeatRate);
    }

    void InvokeDelay()
    {
        if (onceAction_1 != null)
        {
            onceAction_1();
            onceAction_1 = null;
        }
    }

    void InvokeDelay2()
    {
        if (onceAction_2 != null)
        {
            onceAction_2();
            onceAction_2 = null;
        }
    }

    void InvokeDelay3()
    {
        if (onceAction_3 != null)
        {
            onceAction_3();
            onceAction_3 = null;
        }
    }

    void InvokeDelay4()
    {
        if (onceAction_4 != null)
        {
            onceAction_4();
            onceAction_4 = null;
        }
    }

    void InvokingDelay()
    {
        if (repeatAction_1 != null) repeatAction_1();
    }

    void InvokingDelay2()
    {
        if (repeatAction_2 != null) repeatAction_2();
    }
    #endregion


    public void DestroyWithFrame()
    {
        if (destroyed)
        {
            return;
        }
        destroyed = true;
        StopAllCoroutines();
        StopAllTimers();
        for (int i = 0; i < scriptItems.Length; ++i)
        {
            var scriptItem = scriptItems[i];
            if (null != scriptItem)
            {
                if (scriptItem.component is Button button)
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.Invoke();
                }
                else if (scriptItem.component is Toggle toggle)
                {
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.Invoke(false);
                }
                else if (scriptItem.component is UIButton nguiButton)
                {
                    nguiButton.onClick.Clear();
                }
                else if (scriptItem.component is UIEventListener nguiEventListener)
                {
                    nguiEventListener.onClick = null;
                    nguiEventListener.parameter = null;
                }
                else if (scriptItem.component is Image img)
                {
                    img.sprite = null;
                }
                if (scriptItem.component is ScriptBinder scriptBinder)
                {
                    scriptBinder.DestroyWithFrame();
                }
            }
            scriptItems[i] = null;
        }
    }

    void StopAllTimers()
    {
        action_base = null;
        onceAction_1 = null;
        repeatAction_1 = null;
        repeatAction_2 = null;
        onceAction_2 = null;
        onceAction_3 = null;
        onceAction_4 = null;
        this.CancelInvoke();
    }

    void OnDestroy()
    {
        DestroyWithFrame();
        if(null != onFrameClosed)
        {
            onFrameClosed.Invoke();
            onFrameClosed = null;
        }
    }
}