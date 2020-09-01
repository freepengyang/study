//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using System;
using UnityEngine;

/// <summary>
/// Event Hook class lets you easily add remote event listener functions to an object.
/// Example usage: UIEventListener.Get(gameObject).onClick += MyClickFunction;
/// </summary>

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
    //此处采用系统提供的委托，降低ILRuntime绑定消耗
    //public delegate void VoidDelegate(GameObject go);
    //public delegate void BoolDelegate(GameObject go, bool state);
    //public delegate void FloatDelegate(GameObject go, float delta);
    //public delegate void VectorDelegate(GameObject go, Vector2 delta);
    //public delegate void ObjectDelegate(GameObject go, GameObject obj);
    //public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
    public object parameter;

    public Action<GameObject> onSubmit;
    public Action<GameObject> onClick;
    public Action<GameObject> onDoubleClick;
    public Action<GameObject,bool> onHover;
    public Action<GameObject,bool> onPress;
    public Action<GameObject,bool> onSelect;
    public Action<GameObject,float> onScroll;
    public Action<GameObject> onDragStart;
    public Action<GameObject,Vector2> onDrag;
    public Action<GameObject> onDragOver;
    public Action<GameObject> onDragOut;
    public Action<GameObject> onDragEnd;
    public Action<GameObject,GameObject> onDrop;
    public Action<GameObject,KeyCode> onKey;
    public Action<GameObject,bool> onTooltip;
    public Action<GameObject> onKeepPress;
    public float ClickIntervalTime = 0.3f;
    private float mLastClickTime = 0;

    bool isColliderEnabled
    {
        get
        {
            Collider c = GetComponent<Collider>();
            if (c != null) return c.enabled;
            Collider2D b = GetComponent<Collider2D>();
            return (b != null && b.enabled);
        }
    }

    void OnSubmit() { if (isColliderEnabled && onSubmit != null) onSubmit(gameObject); }
    void OnClick()
    {
        if (isColliderEnabled && onClick != null)
        {
            if (Time.time - mLastClickTime > ClickIntervalTime)
            {
                mLastClickTime = Time.time;
                onClick(gameObject); 
            }
        }
    }
    void OnDoubleClick() { if (isColliderEnabled && onDoubleClick != null) onDoubleClick(gameObject); }
    void OnHover(bool isOver) { if (isColliderEnabled && onHover != null) onHover(gameObject, isOver); }
    void OnPress(bool isPressed) 
    {
        if (isColliderEnabled && onPress != null)
        {
            onPress(gameObject, isPressed);
            return;
        }

        if(isColliderEnabled && onKeepPress != null)
        {
            if (isPressed)
            {
                //Debug.LogFormat("[Enter OnKeepPress]");
                Invoke("OnKeepPress", 0.80f);
            }
            else
            {
                //Debug.LogFormat("[OutPress OnKeepPress]");
                CancelInvoke("OnKeepPress");
            }
            return;
        }
    }
    void OnSelect(bool selected) { if (isColliderEnabled && onSelect != null) onSelect(gameObject, selected); }
    void OnScroll(float delta) { if (isColliderEnabled && onScroll != null) onScroll(gameObject, delta); }
    void OnDragStart() { if (onDragStart != null) onDragStart(gameObject); }
    void OnDrag(Vector2 delta) { if (onDrag != null) onDrag(gameObject, delta); }
    void OnDragOver() { if (isColliderEnabled && onDragOver != null) onDragOver(gameObject); }
    void OnDragOut() { if (isColliderEnabled && onDragOut != null) onDragOut(gameObject); }
    void OnDragEnd() { if (onDragEnd != null) onDragEnd(gameObject); }
    void OnDrop(GameObject go) { if (isColliderEnabled && onDrop != null) onDrop(gameObject, go); }
    void OnKey(KeyCode key) { if (isColliderEnabled && onKey != null) onKey(gameObject, key); }
    void OnTooltip(bool show) { if (isColliderEnabled && onTooltip != null) onTooltip(gameObject, show); }

    private void OnKeepPress()
    {
        //Debug.LogFormat("[Invoke OnKeepPress]");
        onKeepPress?.Invoke(gameObject);
    }

    /// <summary>
    /// Get or add an event listener to the specified game object.
    /// </summary>

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    static public UIEventListener Get(GameObject go, object obj)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        listener.parameter = obj;
        return listener;
    }
}
