using UnityEngine;
using System.Collections;

public class UIEffectPlay : UIEffectPlayBase
{
    private int mSpeed;
    private bool mIsLoop;
    private UISprite _sprite;
    private UISpriteAnimation tempAnimation;
    private System.Action action;

    public void ShowUIEffect(GameObject go, string str, int _speed = 10, bool _loop = true,
        int type = ResourceType.UIEffect, bool delete = true, System.Action _action = null, int _loadType = ResourceAssistType.UI)
    {
        gameObject = go;
        InitComponent();
        if (string.IsNullOrEmpty(str)) return;

        if (resName == str)
        {
            if (tempAnimation != null)
            {
                tempAnimation.RebuildSpriteList();
                tempAnimation.ResetToBeginning();
                if (mSpeed > 0)
                    tempAnimation.framesPerSecond = mSpeed;
                tempAnimation.loop = mIsLoop;
            }
            return;
        }

        if (_speed > 0) mSpeed = _speed;
        mIsLoop = _loop;
        /*if (!string.IsNullOrEmpty(resName))
        {
            if (gameObject.GetComponent<UISprite>() != null)
                gameObject.GetComponent<UISprite>().atlas = null;
            DestroyEffect();
        }*/
        action = _action;
        resName = str;
        CSResource res = CSResourceManager.Singleton.AddQueue(str, type, OnLoadUIEffect, _loadType);
        mLoaddingPath = res.Path;
        res.IsCanBeDelete = delete;
        res.mCount++;
        mIsLoad = true;
    }

    private void InitComponent()
    {
        _sprite = gameObject.GetComponent<UISprite>();
        if (_sprite == null) _sprite = gameObject.AddComponent<UISprite>();
        tempAnimation= gameObject.GetComponent<UISpriteAnimation>();

        if (tempAnimation == null) tempAnimation = gameObject.AddComponent<UISpriteAnimation>();
    }

    private void OnLoadUIEffect(CSResource res)
    {
        if (gameObject == null)
        {
            CSResourceManager.Instance.DestroyResource(res.Path, false, true, true);
            return;
        }
        
        if (_sprite == null || res == null) return;
        if (res.MirrorObj == null)
        {
            _sprite.atlas = null;
            return;
        }

        GameObject tempGo = res.MirrorObj as GameObject;

        if (tempGo == null) return;

        UIAtlas atlas = tempGo.GetComponent<UIAtlas>();

        if (atlas == null) return;

        _sprite.atlas = atlas;

        if (!string.IsNullOrEmpty(mPath))
        {
            if (CSResourceManager.Instance != null)
            {
                CSResourceManager.Instance.DestroyResource(mPath, false, true, true);
            }
        }

        mLoaddingPath = string.Empty;
        mPath = res.Path;

        if (tempAnimation == null) tempAnimation = this.gameObject.AddComponent<UISpriteAnimation>();

        if (tempAnimation != null)
        {
            tempAnimation.RebuildSpriteList();
            tempAnimation.ResetToBeginning();
            if (mSpeed > 0)
                tempAnimation.framesPerSecond = mSpeed;
            tempAnimation.loop = mIsLoop;
        }
        if (action !=null)
        {
            action();
        }
    }

    protected override void DestroyEffect()
    {
        resName = "";
        
        if (_sprite != null)
            _sprite.atlas = null;
        if (CSResourceManager.Instance != null)
        {
            CSResourceManager.Instance.DestroyResource(mPath, false, true, true);
        }
    }

    public override void Dispose()
    {
        mSpeed = 0;
        mIsLoop = false;
        base.Dispose();

        _sprite = null;
        tempAnimation = null;
    }
}