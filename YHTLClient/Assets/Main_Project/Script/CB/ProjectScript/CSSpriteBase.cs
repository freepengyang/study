using UnityEngine;
using System.Collections;
using System;

public interface ISFSprite
{
    Vector2 getUV { get; set; }
    Vector2 getCentre { get; set; }
    void SetShader(Material mat, Vector4 color, Vector4 greyColor);
    SFAtlas getAtlas { get; set; }
}

public class CSSpriteBase : MonoBehaviour, ISFSprite
{
    public virtual UnityEngine.Color LastShaderGrey
    {
        get { return Color.white; }
        set {  }
    }
    public virtual CSSpriteBase getShadowSprite { get { return null; } set { } }

    public Vector2 UV = Vector2.zero;

    public virtual Vector2 getUV
    {
        get { return UV; }
        set { UV = value; }
    }

    public Vector2 Centre = Vector2.zero;
    public virtual Vector2 getCentre
    {
        get { return Centre; }
        set { Centre = value; }
    }

    public virtual void SetShader(Material mat, Vector4 color, Vector4 greyColor){}
    public virtual SFAtlas getAtlas { get { return null; } set { } }

    public string mSpriteName = string.Empty;

    protected bool mIsInit = false;
    public bool IsInit
    {
        get { return mIsInit; }
        set { mIsInit = value; }
    }

    public Texture mPicture;

    public Material mMaterial;

    public Vector2 mSize = Vector2.zero;
    
    protected bool misUVChange = false;
    public bool IsUVChange
    {
        get { return misUVChange; }
        set { misUVChange = value; }
    }

    protected static float angle = 48.78f;

    public bool isSlant;
    /// <summary>
    /// X 加 或 减
    /// </summary>
    public bool isNegative; // 仅仅使用与影子，模型只要五个方向时使用

    public float mTestOffsetX = 348;
    public float mTestOffsetY = 220;

    public float mTestNegativeOffsetX = 241;
    public float mTestNegativeOffsetY = 215;

    public float originalHalfWidthOffsetX = 100;

    public float mCenterOffsetX = 0;
    public float mCenterOffsetY = 0;

    public float mUVXOffstX = 0;
    public float mUVYOffsetY = 0;
    protected MeshFilter mMeshFilter;

    protected MeshRenderer mMeshRenderer;

    protected Transform mScaleTrans;

    protected Vector3 mScaleTransVec;

    protected bool mIsScalTrans = false;

    protected Texture mTexture = null;

    protected string mInitSetSpriteName;

    protected Transform mCacheTrans;
    public UnityEngine.Transform CacheTrans
    {
        get { return mCacheTrans; }
        set { mCacheTrans = value; }
    }
    protected virtual void Awake()
    {
        mCacheTrans = transform;
    }

    protected virtual void Start()
    {
    }

    public virtual string SpriteName { get; set; }
    public virtual Material MainMaterial { get; set; }

    public string InitSetSpriteName
    {
        get { return mInitSetSpriteName; }
        set
        {
            if (mInitSetSpriteName == value)
                return;
            mInitSetSpriteName = value;
            if (mIsInit)
            {
                SpriteName = value;
            }
        }
    }

    public virtual Texture Texture
    {
        get
        {
            Material mat = MainMaterial;
            if (mat == null) return null;
            return MainMaterial.mainTexture;
        }
        set
        {
            Material mat = MainMaterial;
            if (mat != null)
            {
                mat.mainTexture = value;
            }

            mTexture = value;
            if (mat.mainTexture != null)
            {
                this.mSize.y = (float)mat.mainTexture.height;
                this.mSize.x = (float)mat.mainTexture.width;
            }
        }
    }

    public void ResetMesh()
    {
        mMaterial = null;
        if (mMeshRenderer != null) mMeshRenderer.sharedMaterial = null;
        if (mMeshFilter != null) mMeshFilter.mesh.Clear();
    }

    public void ApplyScaleTrans(Transform trans, Vector3 vec)
    {
        mScaleTrans = trans;
        mIsScalTrans = true;
        mScaleTransVec = vec;
    }

    protected void setMeshVertices(Vector2 Uv, Color[] meshColorList)
    {
        Vector3 vec3 = Vector3.zero;
        vec3.x = -UV.x / 2f;
        vec3.y = -Uv.y / 2f;
        CSMisc.MeshVertices[0] = vec3;
        vec3.x = -UV.x / 2f;
        vec3.y = Uv.y / 2f;
        CSMisc.MeshVertices[1] = vec3;
        vec3.x = UV.x / 2f;
        vec3.y = -Uv.y / 2f;
        CSMisc.MeshVertices[2] = vec3;
        vec3.x = UV.x / 2f;
        vec3.y = Uv.y / 2f;
        CSMisc.MeshVertices[3] = vec3;
        mMeshFilter.mesh.vertices = CSMisc.MeshVertices;
        mMeshFilter.mesh.normals = CSMisc.MeshVertices;
        mMeshFilter.mesh.colors = meshColorList;
    }

    protected void setMeshVertices(int paddingLeft, int paddingRight, int paddingTop, int paddingBottom, int width, int height, Color[] meshColorList)
    {
        float originalHalfWidth = (paddingLeft + paddingRight + width) * 0.5f;
        float originalHalfHeight = (paddingTop + paddingBottom + height) * 0.5f;

        //左下
        float px0 = -(originalHalfWidth - paddingLeft);
        float py0 = -(originalHalfHeight - paddingBottom);

        //左上
        float px1 = -(originalHalfWidth - paddingLeft);
        float py1 = (originalHalfHeight - paddingTop);
        //右下
        float px2 = (originalHalfWidth - paddingRight);
        float py2 = -(originalHalfHeight - paddingBottom);
        //右上
        float px3 = (originalHalfWidth - paddingRight);
        float py3 = (originalHalfHeight - paddingTop);


        Vector3 vec3 = Vector3.zero;
        if (isSlant)
        {
            float bevelEdge = (float)Math.Sin(angle * Math.PI / 180) * (paddingBottom + height);
            //float OffsetX = ((float)Math.Sin(angle * Math.PI / 180) * bevelEdge) - 218;
            //float OffsetY = ((float)Math.Cos(angle * Math.PI / 180) * bevelEdge) - 190;

            float OffsetX = ((float)Math.Sin(angle * Math.PI / 180) * bevelEdge) - mTestOffsetX;
            float OffsetY = ((float)Math.Cos(angle * Math.PI / 180) * bevelEdge) - mTestOffsetY;

            if (isNegative)
            {
                px1 = px1 + OffsetX;
            }
            else
            {
                px1 = px1 - OffsetX;
            }
            vec3.x = px1;
            vec3.y = py1 - OffsetY;
            CSMisc.MeshVertices[1] = vec3;
        
            if (isNegative)
            {
                px3 = px3 + OffsetX;
            }
            else
            {
                px3 = px3 - OffsetX;
            }
            vec3.x = px3 ;
            vec3.y = py3 - OffsetY;
            CSMisc.MeshVertices[3] = vec3;

            bevelEdge = (float)Math.Sin(angle * Math.PI / 180) * paddingBottom;

            //OffsetX = (float)Math.Sin(angle * Math.PI / 180) * bevelEdge - 218;
            //OffsetY = (float)Math.Cos(angle * Math.PI / 180) * bevelEdge - 193;
            OffsetX = (float)Math.Sin(angle * Math.PI / 180) * bevelEdge - mTestNegativeOffsetX;
            OffsetY = (float)Math.Cos(angle * Math.PI / 180) * bevelEdge - mTestNegativeOffsetY;

            if (isNegative)
            {
                px0 = px0 + OffsetX;
            }
            else
            {
                px0 = px0 - OffsetX;
            }
            vec3.x = px0 ;
            vec3.y = py0 - OffsetY;
            CSMisc.MeshVertices[0] = vec3;
          
            if (isNegative)
            {
                px2 = px2 + OffsetX;
            }
            else
            {
                px2 = px2 - OffsetX;
            }

            vec3.x = px2 ;
            vec3.y = py2 - OffsetY;
            CSMisc.MeshVertices[2] = vec3;
        }
        else
        {
            vec3.x = px0;
            vec3.y = py0;
            CSMisc.MeshVertices[0] = vec3;

            vec3.x = px1;
            vec3.y = py1;
            CSMisc.MeshVertices[1] = vec3;

            vec3.x = px2;
            vec3.y = py2;
            CSMisc.MeshVertices[2] = vec3;

            vec3.x = px3;
            vec3.y = py3;
            CSMisc.MeshVertices[3] = vec3;
        }

        mMeshFilter.mesh.vertices = CSMisc.MeshVertices;
        mMeshFilter.mesh.normals = CSMisc.MeshVertices;
        mMeshFilter.mesh.colors = meshColorList;
    }

    public Vector3[] GetMeshVertices()
    {
        if (mMeshFilter == null) return null;
        return mMeshFilter.mesh.vertices;
    }

    protected void setMeshTriangles()
    {
        CSMisc.array2[0] = 0;
        CSMisc.array2[1] = 1;
        CSMisc.array2[2] = 2;
        CSMisc.array2[3] = 1;
        CSMisc.array2[4] = 3;
        CSMisc.array2[5] = 2;
        mMeshFilter.mesh.triangles = CSMisc.array2;
    }
    protected void setMeshUV()
    {
        Vector2 vec2 = Vector2.zero;
        vec2.x = 0;
        vec2.y = 0;
        CSMisc.uvs[0] = vec2;

        vec2.x = 0;
        vec2.y = 1;
        CSMisc.uvs[1] = vec2;

        vec2.x = 1;
        vec2.y = 0;
        CSMisc.uvs[2] = vec2;

        vec2.x = 1;
        vec2.y = 1;
        CSMisc.uvs[3] = vec2;


        mMeshFilter.mesh.uv = CSMisc.uvs;
    }

    protected void setMeshUV(int x, int width, int y, int height)
    {
        Vector2 size;
        size.x = this.mSize.x;
        size.y = this.mSize.y;
        Vector2 mUV_One;
        mUV_One.x = x / size.x;
        mUV_One.y = (size.y - (float)(y + height)) / size.y;

        Vector2 mUV_Two;
        mUV_Two.x = x / size.x; ;
        mUV_Two.y = (size.y - (float)y) / size.y;

        Vector2 mUV_Three;
        mUV_Three.x = (float)(x + width) / size.x;
        mUV_Three.y = (size.y - (float)(y + height)) / size.y;

        Vector2 mUV_Four;
        mUV_Four.x = (float)(x + width) / size.x;
        mUV_Four.y = (size.y - (float)y) / size.y;

        if (mMeshFilter != null)
        {
            CSMisc.uvs[0] = mUV_One;
            CSMisc.uvs[1] = mUV_Two;
            CSMisc.uvs[2] = mUV_Three;
            CSMisc.uvs[3] = mUV_Four;
            mMeshFilter.mesh.uv = CSMisc.uvs;
        }
    }

    protected void getComponent()
    {
        if (Texture == null) return;

        if (mMeshFilter == null)
        {
            mMeshFilter = gameObject.GetComponent<MeshFilter>();

            if (mMeshFilter == null)
            {
                mMeshFilter = gameObject.AddComponent<MeshFilter>();
            }
            mMeshFilter.mesh.MarkDynamic();
        }

        if (mMeshRenderer == null)
        {
            mMeshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (mMeshRenderer == null)
            {
                mMeshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }
        mMeshRenderer.sharedMaterial = this.MainMaterial;
    }

    public static void DrawRect(float sx, float sy, float width, float height, float PixelRatio)
    {
        Vector2[] point = new Vector2[4];

        point[0] = new Vector2(sx, sy);
        point[1] = new Vector2(sx, height);
        point[2] = new Vector2(width, height);
        point[3] = new Vector2(width, sy);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point[0] * PixelRatio, point[1] * PixelRatio);
        Gizmos.DrawLine(point[1] * PixelRatio, point[2] * PixelRatio);
        Gizmos.DrawLine(point[2] * PixelRatio, point[3] * PixelRatio);
        Gizmos.DrawLine(point[3] * PixelRatio, point[0] * PixelRatio);
    }

    public virtual void Destroy()
    {

    }
}

