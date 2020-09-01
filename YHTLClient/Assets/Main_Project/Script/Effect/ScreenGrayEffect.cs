
using UnityEngine;
using System.Collections;

public class ScreenGrayEffect : MonoBehaviour {

    private Shader grayShader;
    [Range(0f, 1f)]public float grayScaleAmount = 0.0f;

    private Material _grayMaterial;
    public Material grayMaterial
    {
        get {
            if (_grayMaterial == null)
            {
                _grayMaterial = new Material(grayShader);
                _grayMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return _grayMaterial;
        }
    }
    
    void Start () {
        grayShader = Shader.Find("Custom/ScreenGray");
        //if (!SystemInfo.supportsImageEffects)
        //{
        //    enabled = false;
        //    return;
        //}
        if (grayShader != null && !grayShader.isSupported)
        {
            enabled = false;
        }
	}

    void OnRenderImage(RenderTexture srcTex, RenderTexture destTex)
    {
        if (grayShader != null)
        {
            grayMaterial.SetFloat("_LuminosityAmount", grayScaleAmount);
            Graphics.Blit(srcTex, destTex, grayMaterial);
        }
        else
        {
            Graphics.Blit(srcTex, destTex);
        }
    }

    private void OnDisable()
    {
        if (grayMaterial != null)
        {
            DestroyImmediate(grayMaterial);
        }
    }

    private void OnDestroy()
    {
        _grayMaterial = null;
    }
}
