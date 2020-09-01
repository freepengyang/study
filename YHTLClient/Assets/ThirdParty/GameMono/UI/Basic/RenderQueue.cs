using System;
using System.Collections.Generic;
using UnityEngine;
public class RenderQueue : MonoBehaviour
{
    public int renderQueue = 2703;
    public int sortingStage = 300;
    public Renderer[] renderers;
    public bool relyOnRenderQ;
    void OnEnable()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        //Debug.Log("RenderQueue " + renderers.Length);
    }

    void Start()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            if (renderer != null && renderer.sharedMaterial != null)
            {
                if (relyOnRenderQ)
                    renderer.sharedMaterial.renderQueue = renderQueue + i;
                else
                    renderer.sharedMaterial.renderQueue = renderQueue;
                renderer.sortingOrder = sortingStage;
            }
        }
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            if (renderer != null && renderer.sharedMaterial != null)
            {
                if (relyOnRenderQ)
                    renderer.sharedMaterial.renderQueue = renderQueue + i;
                else
                    renderer.sharedMaterial.renderQueue = renderQueue;
                renderer.sortingOrder = sortingStage;
            }
        }
    }
}