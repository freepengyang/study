using UnityEngine;
using System.Collections;

public class RenderQueueModifier : MonoBehaviour
{

    public enum RenderType
    {
        FRONT,
        BACK
    }

    public UIWidget m_target = null;

    public RenderType m_type = RenderType.BACK;

    Renderer[] _renderers;

    int _lastQueue = 0;


    void Start()
    {
        _renderers = transform.GetComponentsInChildren<Renderer>();
    }

    private void EffectLoaderSuccess(uint uiEvtId, object data)
    {
        _renderers = transform.GetComponentsInChildren<Renderer>();
        _lastQueue = 0;
    }

    void FixedUpdate()
    {

        if (m_target == null || m_target.drawCall == null)
            return;

        int queue = m_target.drawCall.renderQueue;

        queue += m_type == RenderType.FRONT ? 1 : -1;

        if (_lastQueue != queue && _renderers != null && _renderers.Length > 0)
        {
            _lastQueue = queue;
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                    _renderers[i].material.renderQueue = _lastQueue;
            } 
        }
    }

    private void OnDestroy()
    {
    }
}