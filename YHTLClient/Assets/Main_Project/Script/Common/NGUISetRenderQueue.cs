using UnityEngine;
using System.Collections;

public class NGUISetRenderQueue : MonoBehaviour {

    public int renderQueue = 3300;

    Renderer[] _renderers;


    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        UpdateRenderQueue();
    }

    void Update()
    {
        UpdateRenderQueue();
    }

    private void UpdateRenderQueue()
    {
        if (_renderers == null)
            return;

        int count = _renderers.Length;
        for (int i = 0; i < count; i++)
        {
            Renderer r = _renderers[i];
            if (r != null && r.material.renderQueue != renderQueue)
                r.material.renderQueue = renderQueue;
        }
    }

}
