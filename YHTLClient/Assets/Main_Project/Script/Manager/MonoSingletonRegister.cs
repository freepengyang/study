using UnityEngine;
using System.Collections;

public class MonoSingletonRegister : MonoBehaviour
{
    protected System.Action onStart;
    protected System.Action onDestroy;
    public Transform CachedTransform
    {
        get;private set;
    }
    public static MonoSingletonRegister Register(string name, System.Action awake, System.Action start, System.Action onDestroy)
    {
        GameObject gameObject = new GameObject(name);
        MonoSingletonRegister handle = gameObject.AddComponent<MonoSingletonRegister>();
        handle.CachedTransform = handle.transform;
        awake?.Invoke();
        handle.onStart = start;
        handle.onDestroy = onDestroy;
        return handle;
    }

    private void Start()
    {
        onStart?.Invoke();
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}