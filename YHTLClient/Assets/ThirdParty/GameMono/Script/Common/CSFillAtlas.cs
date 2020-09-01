using UnityEngine;
using System.Collections;

public class CSFillAtlas : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private UIAtlas mAtlas;
    public UIAtlas Atlas
    {
        get { return mAtlas; }
        set
        {
            if (mAtlas == value) return;
            mAtlas = value;

            if (mAtlas != null && meshRenderer != null)
            {
                meshRenderer.material = mAtlas.spriteMaterial;
            }

        }
    }

    private string mSpriteName;
    public string SpriteName
    {
        get { return mSpriteName; }
        set
        {
            //if (mSpriteName == value) return;
            mSpriteName = value;
            Fill();
        }
    }

    [SerializeField]
    [HideInInspector]
    private float mFillCount = 1;
    public float fillAmount
    {
        get { return mFillCount; }
        set
        {
            value = Mathf.Clamp01(value);
            if (mFillCount == value) return;
            mFillCount = value;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Color mColor = Color.white;
    public UnityEngine.Color Color
    {
        get { return mColor; }
        set
        {
            if (mColor.Equals(value)) return;
            mColor = value;
            Fill();
        }
    }

    private Vector3[] verts = null;
    private Vector2[] uvs = null;
    private Color[] colors = null;
    private int[] triangles = null;
    private Mesh mesh;
    private int length = 1;
    private MeshFilter mMeshFilter;
    private MeshRenderer meshRenderer;
    private float[] cacheArray = new float[4];

    private void GenerateFilter()
    {
        if (mMeshFilter == null)
        {
            mMeshFilter = gameObject.GetComponent<MeshFilter>();
            if (mMeshFilter == null)
                mMeshFilter = gameObject.AddComponent<MeshFilter>();
        }
        if (meshRenderer == null)
        { 
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
                meshRenderer = gameObject.AddComponent<MeshRenderer>();

            if (mAtlas != null)
                meshRenderer.material = mAtlas.spriteMaterial;
        }
    }

    void Fill()
    {
        if (Atlas == null) return;

        GenerateFilter();

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }
        else
        {
            mesh.Clear();
        }

        if(!string.IsNullOrEmpty(mSpriteName))
        {
            if (cacheArray == null) cacheArray = new float[4];

            verts = new Vector3[length << 2];
            uvs = new Vector2[verts.Length];
            triangles = new int[(length << 1) * 3];
            Texture tex = mAtlas.texture;
            colors = new Color[verts.Length];
            int tmp = 0;
            float x0 = 0;
            float yo = 0;

            UISpriteData mSprite = mAtlas.GetSprite(mSpriteName);

            if (mSprite == null || tex == null) return;

            yo = mSprite.height;

            //setting vertices
            verts[0] = new Vector3(x0, yo);
            verts[1] = new Vector3(x0, 0);
            x0 += mSprite.width;
            verts[2] = new Vector3(x0, yo);
            verts[3] = new Vector3(x0, 0);

            //setting color
            colors[0] = mColor;
            colors[1] = mColor;
            colors[2] = mColor;
            colors[3] = mColor;

            //setting uvs
            Rect inner = new Rect(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
            inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

            uvs[0] = new Vector2(inner.xMin, inner.yMax);
            uvs[1] = new Vector2(inner.xMin, inner.yMin);
            uvs[2] = new Vector2(inner.xMax, inner.yMax);
            uvs[3] = new Vector2(inner.xMax, inner.yMin);


            for (int i = 0; i < triangles.Length; i += 6)
            {
                tmp = (i / 3) << 1;
                triangles[i] = triangles[i + 3] = tmp;
                triangles[i + 1] = triangles[i + 5] = tmp + 3;
                triangles[i + 2] = tmp + 1;
                triangles[i + 4] = tmp + 2;
            }

            //偏移值处理
            uvs[2].x = (uvs[2].x - uvs[0].x) * mFillCount + uvs[0].x;
            uvs[3].x = (uvs[3].x - uvs[1].x) * mFillCount + uvs[1].x;

            verts[2].x = (verts[2].x - verts[0].x) * mFillCount + verts[0].x;
            verts[3].x = (verts[3].x - verts[1].x) * mFillCount + verts[1].x;

            //临时储存
            cacheArray[0] = uvs[2].x;
            cacheArray[1] = uvs[3].x;

            cacheArray[2] = verts[2].x;
            cacheArray[3] = verts[3].x;

            mesh.vertices = verts;
            mesh.colors = colors;
            mesh.triangles = triangles;
            mesh.uv = uvs;
        }
        mMeshFilter.mesh = mesh;
    }

   public void SetAmount(float _fillAmount)
    {
        if (mesh == null || uvs == null || verts == null || mMeshFilter == null|| cacheArray==null) return;

        fillAmount = _fillAmount;

        //偏移值处理
        uvs[2].x = (cacheArray[0] - uvs[0].x) * mFillCount + uvs[0].x;
        uvs[3].x = (cacheArray[1] - uvs[1].x) * mFillCount + uvs[1].x;

        verts[2].x = (cacheArray[2] - verts[0].x) * mFillCount + verts[0].x;
        verts[3].x = (cacheArray[3] - verts[1].x) * mFillCount + verts[1].x;

        mesh.vertices = verts;
        mesh.uv = uvs;
        mMeshFilter.mesh = mesh;
    }
}
