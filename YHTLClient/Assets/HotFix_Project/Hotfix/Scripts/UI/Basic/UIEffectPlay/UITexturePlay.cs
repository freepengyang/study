using UnityEngine;
using System.Collections;

public class  UITexturePlay : UIEffectPlayBase
{
    private bool isMakePixelPerfect = true;
    private UITexture texture;

    public void ShowTexture(GameObject go, string str, int type = ResourceType.UITexture, bool isPerfect = true, System.Action action = null, bool isCanBeDelete = true)
    {
        gameObject = go;
        InitComponent();
        if (string.IsNullOrEmpty(str)) return;
        this.renderCallBack = action;
        if (resName == str)
        {
            if (renderCallBack != null) renderCallBack();
            return;
        }

        if (!string.IsNullOrEmpty(resName))
        {
            if (texture != null)
                texture.mainTexture = null;
            DestroyEffect();
        }
        resName = str;
        isMakePixelPerfect = isPerfect;
        CSResource res = CSResourceManager.Singleton.AddQueue(resName, type, OnLoadTexture, ResourceAssistType.UI);
        mPath = res.Path;
        res.IsCanBeDelete = isCanBeDelete;
        res.mCount++;
    }
    
    private void InitComponent()
    {
        texture = gameObject.GetComponent<UITexture>();
        if (texture == null) texture = gameObject.AddComponent<UITexture>();
    }

    private void OnLoadTexture(CSResource res)
    {
        if(null == texture)
        {
            FNDebug.LogError("GetComponent Failed UITexture");
            return;
        }

        if (res == null) return;

        if (res.MirrorObj == null)
        {
            texture.mainTexture = null;
            return;
        }

        Texture2D tex = res.MirrorObj as Texture2D;

        if (tex == null) return;

        if (tex != null)
        {
            texture.mainTexture = tex;
        }
        if (isMakePixelPerfect)
        {
            texture.MakePixelPerfect();
        }
        if (renderCallBack!=null) renderCallBack();
    }

    protected override void DestroyEffect()
    {
        resName = "";
        if (texture != null)
            texture.mainTexture = null;
        if (CSResourceManager.Instance != null)
        {
            CSResourceManager.Instance.DestroyResource(mPath, false, true, true);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        texture = null;
    }
}
