using UnityEngine;
using System.Collections;

public class  UIEffectPlayBase : GridContainerBase
{
    protected string mPath = string.Empty;
    //加载过程的路径，防止对象销毁时，资源还未加载完成
    protected string mLoaddingPath = string.Empty;
    protected string resName;
    protected bool mIsLoad = false;
    protected bool IsDestroy = false;
    protected System.Action renderCallBack;
    

    protected virtual void DestroyEffect()
    {
        
    }

    public override void Dispose()
    {
        DestroyEffect();

        mPath = string.Empty;
        resName = string.Empty;
        mLoaddingPath = string.Empty;
        gameObject = null;
        renderCallBack = null;
        
        if (!string.IsNullOrEmpty(mLoaddingPath) && CSResourceManager.Instance != null)
        {
            CSResourceManager.Instance.DestroyResource(mLoaddingPath, false, true, true);
        }
    }
}
