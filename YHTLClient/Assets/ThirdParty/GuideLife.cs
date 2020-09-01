using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLife : MonoBehaviour
{
    public int guideId;
    public int step;
    public System.Action<bool> onDeath;
    public System.Action onStart;//等待激活触发，只会出现一次
    bool destroyed = false;
    bool started = false;

    public void Remove()
    {
        if (destroyed)
            return;
        destroyed = true;
        onStart = null;
        onDeath?.Invoke(true);
        onDeath = null;
        Object.Destroy(this);
    }

    private void OnEnable()
    {
        if(started)
        {
            onStart?.Invoke();
            onStart = null;
        }
    }

    private void Start()
    {
        started = true;
        onStart?.Invoke();
        onStart = null;
    }

    private void OnDisable()
    {
        if (destroyed)
            return;
        destroyed = true;
        onStart = null;
        onDeath?.Invoke(false);
        onDeath = null;
        Object.Destroy(this);
    }

    private void OnDestroy()
    {
        if (destroyed)
            return;
        destroyed = true;
        onStart = null;
        onDeath?.Invoke(false);
        onDeath = null;
    }
}
