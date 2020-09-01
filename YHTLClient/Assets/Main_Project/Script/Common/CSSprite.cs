
//-------------------------------------------------------------------------
//实现渲染，并支持NGUI图集
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;

public class CSSprite : CSSpriteBase
{
    public UIAtlas mAtlas;
    private CSSprite mShoadowSprite = null;
    public CSSprite ShoadowSprite
    {
        get { return mShoadowSprite; }
        set { mShoadowSprite = value; }
    }
    CSObjectPoolItem poolItem = null;
    public override CSSpriteBase getShadowSprite
    {
        get
        {
            return ShoadowSprite;
        }
        set
        {
            ShoadowSprite = value as CSSprite;
        }
    }

    public override SFAtlas getAtlas
    {
        get
        {
            return Atlas;
        }
        set
        {
            Atlas = value as UIAtlas;
        }
    }

    public UIAtlas Atlas
    {
        get { return mAtlas; }
        set
        {
            if (mAtlas == value) return;
            RemovePoolItem();
            mAtlas = value;
            if (mAtlas != null) mAtlas.InitShaderMiss();
            ResetMesh();
            if (value != null) UpdateSpriteData();
            AddPoolItem();
        }
    }
    public override Texture Texture
    {
        get
        {
            if (isSlant)
            {
                if (mAtlas != null)
                {
                    Material mat = MainMaterial;
                    if (mat != null && mat.mainTexture == null)
                    {
                        if (mAtlas.spriteMaterial != null && mAtlas.spriteMaterial.mainTexture != null)
                        {
                            mat.mainTexture = mAtlas.spriteMaterial.mainTexture;
                            return mat.mainTexture;
                        }
                    }
                }
            }
            Material mat2 = MainMaterial;
            if (mat2 == null) return null;
            return mat2.mainTexture;
        }
        set
        {
            Material mat = MainMaterial;
            if (mat != null) mat.mainTexture = value;
            mTexture = value;
            if (mat != null && mat.mainTexture != null)
            {
                this.mSize.y = (float)mat.mainTexture.height;
                this.mSize.x = (float)mat.mainTexture.width;
            }
        }
    }

    void RemovePoolItem()
    {
        if (CSObjectPoolMgr.Instance != null && poolItem != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(poolItem);
            poolItem = null;
            //     mAtlas = null;
        }
    }

    void AddPoolItem()
    {

        if (mAtlas != null && CSObjectPoolMgr.Instance != null)
        {
            if (mAtlas.IsUsePoolItem)
            {
                poolItem = CSObjectPoolMgr.Instance.GetAndAddPoolItem_Resource(mAtlas.name, mAtlas.ResPath, null);
            }
        }
    }

    public Texture Picture
    {
        get { return mPicture; }
        set
        {
            mPicture = value;
            UpdateSpriteData();
        }
    }

    public override string SpriteName
    {
        get { return mSpriteName; }
        set
        {
            mSpriteName = value;
            if (ShoadowSprite != null)
            {
                if (ShoadowSprite.IsInit)
                {
                    ShoadowSprite.SpriteName = value;
                }
                else
                {
                    ShoadowSprite.InitSetSpriteName = value;
                }
            }
            UpdateSprite(mSpriteName);
        }
    }

    public override Material MainMaterial
    {
        get
        {
            if (mMaterial == null)
            {
                if (mAtlas != null)
                {
                    mMaterial = CSShaderManager.GetShareMaterial(mAtlas, EShareMatType.Normal);
                }
                else if (mPicture != null)
                {
                    mMaterial = CSShaderManager.GetShareMaterial(mPicture, EShareMatType.Normal);
                }
            }
            return mMaterial;
        }
        set
        {
            mMaterial = value;
            if (mMeshRenderer != null && mMaterial != null)
            {
                if (mMeshRenderer.sharedMaterial != mMaterial)
                {
                    mMeshRenderer.sharedMaterial = mMaterial;
                    if (mTexture != null)
                        mMeshRenderer.sharedMaterial.mainTexture = mTexture;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        //CSCharactarGo chara = gameObject.GetComponentInParent<CSCharactarGo>();
        //if (chara != null)
        //{
        //    float y = chara.transform.localPosition.y + UV.y / 2f;

        //    float sx = chara.transform.localPosition.x - UV.x / 2f;
        //    float sy = y - UV.y / 2f;
        //    float ex = chara.transform.localPosition.x + UV.x / 2f;
        //    float ey = y + UV.y / 2f;

        //    DrawRect(sx, sy, ex, ey, Constant.PixelRatio);
        //}
    }

    protected override void Start()
    {
        base.Start();
        if (mAtlas != null && mAtlas.spriteList.Count > 0)
        {
            Texture = mAtlas.spriteMaterial.mainTexture;
            SpriteName = mInitSetSpriteName;
        }

        if (mPicture != null)
        {
            Texture = this.mPicture;
            SpriteName = this.mPicture.name;
        }
        mIsInit = true;
    }

    public void Initialize()
    {
        base.Start();
        if (mAtlas != null && mAtlas.spriteList.Count > 0)
        {
            Texture = mAtlas.spriteMaterial.mainTexture;
            SpriteName = mInitSetSpriteName;
        }

        if (mPicture != null)
        {
            Texture = this.mPicture;
            SpriteName = this.mPicture.name;
        }
    }

    private void UpdateSpriteData()
    {
        if (mAtlas != null && mAtlas.spriteList.Count > 0)
        {
            if (mAtlas.spriteMaterial == null)
            {
                if (FNDebug.developerConsoleVisible) FNDebug.LogError("mAtlas.spriteMaterial = " + mAtlas.name);
            }
            Texture = mAtlas.spriteMaterial.mainTexture;
        }

        if (mPicture != null)
        {
            Texture = this.mPicture;
            SpriteName = this.mPicture.name;
        }
    }
    /// <summary>
    /// 默认第一个时 值=“”
    /// </summary>
    /// <param name="name"></param>
    private void UpdateSprite(string name = "")
    {
        if (Texture == null) return;

        if (mAtlas != null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = mAtlas.spriteList[0].name;
            }

            UISpriteData spriteData = mAtlas.GetSprite(name);

            if (spriteData != null)
            {
                AssignSpriteUV(spriteData);
                if (mIsScalTrans)
                {
                    mIsScalTrans = false;
                    if (mScaleTrans != null)
                        mScaleTrans.localScale = mScaleTransVec;
                }
            }
            else
            {
                if (mMeshRenderer != null && mMeshRenderer.enabled)
                {
                    mMeshRenderer.enabled = true;
                }
            }
        }
        else
        {
            AssignSpriteUV();
        }
    }

    protected virtual void AssignSpriteUV()
    {
        getComponent();

        if (mMeshFilter != null)
        {
            this.UV = this.mSize;
            this.misUVChange = true;
            setMeshVertices(this.UV, CSMisc.meshColorList);
            setMeshTriangles();
            setMeshUV();
        }
        mMeshRenderer.enabled = !string.IsNullOrEmpty(SpriteName);
    }

    private void AssignSpriteUV(UISpriteData spriteData)
    {
        if (spriteData != null)
        {
            getComponent();

            if (mMeshFilter != null)
            {
                Vector2 vec2 = Vector2.zero;
                vec2.x = spriteData.width;
                vec2.y = spriteData.height;
                UV = vec2;

                Vector2 vec3 = Vector2.zero;
                vec3.x = spriteData.width * 0.5f - ((spriteData.paddingLeft + spriteData.width + spriteData.paddingRight) * 0.5f - spriteData.paddingLeft);
                vec3.y = spriteData.height * 0.5f - ((spriteData.paddingTop + spriteData.height + spriteData.paddingBottom) * 0.5f - spriteData.paddingBottom);
                Centre = vec3;


                this.misUVChange = true;
                setMeshVertices(spriteData.paddingLeft, spriteData.paddingRight, spriteData.paddingTop, spriteData.paddingBottom, spriteData.width, spriteData.height,
                    CSMisc.meshColorList);
                setMeshTriangles();
                setMeshUV(spriteData.x, spriteData.width, spriteData.y, spriteData.height);
            }
            mMeshRenderer.enabled = true;
        }
    }
    Color lastShaderColor = Color.white;
    public UnityEngine.Color LastShaderColor
    {
        get { return lastShaderColor; }
        set { lastShaderColor = value; }
    }

    Color lastShaderGrey = Color.white;
    public override UnityEngine.Color LastShaderGrey
    {
        get { return lastShaderGrey; }
        set { lastShaderGrey = value; }
    }
    public override void SetShader(Material mat, Vector4 color, Vector4 greyColor)
    {
        if (mMaterial == mat)
        {
            if (lastShaderColor.Equals(color) && lastShaderGrey.Equals(greyColor)) return;
        }
        lastShaderColor = color;
        lastShaderGrey = greyColor;
        Material atlasMat = Atlas != null ? Atlas.spriteMaterial : null;
        if (mat != null)
        {
            MainMaterial = mat;
            if (mat.HasProperty("_MainTexRGBA"))
            {
                mat.SetVector("_MainTexRGBA", color);
            }
            if (mat.HasProperty("_MainTexGrey"))
            {
                mat.SetVector("_MainTexGrey", greyColor);
            }
            if (atlasMat != null && mat.HasProperty("_AlphaTex") && atlasMat.HasProperty("_AlphaTex"))
            {
                Texture tex = atlasMat.GetTexture("_AlphaTex");
                if (tex != null)
                {
                    mat.SetTexture("_AlphaTex", tex);
                    mat.SetFloat("_IsAlphaSplit", 1);
                }
            }
        }
    }

    void OnDestroy()
    {
        RemovePoolItem();
    }

    public override void Destroy()
    {
        RemovePoolItem();
        base.Destroy();
    }
}