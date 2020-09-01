using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CSLabel : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    public int mfontSize = 20;
    public int fontSize
    {
        get
        { return mfontSize; }
        set
        {
            if (mfontSize == value) return;
            mfontSize = value;
        }
    }
    [SerializeField]
    [HideInInspector]
    private string mText = string.Empty;
    [SerializeField]
    [HideInInspector]
    public bool mIsoutLine = true;
    public bool IsoutLine
    {
        get { return mIsoutLine; }
        set
        {
            if (mIsoutLine == value) return;
            mIsoutLine = value;
        }
    }

    [SerializeField]
    [HideInInspector]
    public Color mOutColor = Color.black;
    public UnityEngine.Color OutColor
    {
        get { return mOutColor; }
        set
        {
            if (mOutColor.Equals(value)) return;
            mOutColor = value;
        }
    }

    [SerializeField]
    [HideInInspector]
    public Color mColor = Color.white;
    public UnityEngine.Color color
    {
        get { return mColor; }
        set
        {
            if (mColor.Equals(value)) return;
            mColor = value;
        }
    }

    [SerializeField]
    [HideInInspector]
    public NGUIText.Alignment mAlignment = NGUIText.Alignment.Center;
    public NGUIText.Alignment Alignment
    {
        get { return mAlignment; }
        set
        {
            if (mAlignment == value) return;
            mAlignment = value;
        }
    }

    [HideInInspector]
    [SerializeField]
    Vector2 mEffectDistance = new Vector2(1, 1);

    public string text
    {
        get { return mText; }
        set
        {
            mText = value;
            OnFill();
        }
    }

    private int mWidth = 400;
    public int Width
    {
        get { return mWidth; }
        set
        {
            mWidth = value;
            OnFill();
        }
    }

    private MeshFilter mMeshFilter;
    private MeshRenderer mMeshRenderer;
    private UIGeometry geometry;
    private Mesh mesh;
    private Font mfont;
    private float mTextLength = 0;

    void Awake()
    {
        InitData();
    }

    void Start()
    {
        Font.textureRebuilt -= OnFontRebuildCallBack;
        Font.textureRebuilt += OnFontRebuildCallBack;
    }

    void OnDestroy()
    {
        if (geometry != null)
        {
            geometry.Clear();
            geometry = null;
        }
        if (mesh != null)
        {

            mesh.Clear();
            mesh = null;
        }

        Font.textureRebuilt -= OnFontRebuildCallBack;

    }

    private void InitData()
    {
        if (mMeshFilter == null)
        {
            if (gameObject != null)
            {
                mMeshFilter = gameObject.AddComponent<MeshFilter>();
            }
        }
        if (mMeshRenderer == null)
        {
            if (gameObject != null)
            {
                mMeshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }

        if (mfont == null)
        {
            mfont = CSFontManager.msyhFont;
            if (mfont != null)
            {
                mMeshRenderer.material = mfont.material;
            }
        }
    }

    private void OnFontRebuildCallBack(Font font)
    {
        OnFill();
    }

    public void OnFill()
    {
        if (mMeshFilter == null) return;

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }
        if (geometry == null) geometry = new UIGeometry();
        mesh.Clear();
        geometry.Clear();
        UpdateNGUIText();
        NGUIText.PrintCSLable(text, geometry.verts, geometry.uvs, geometry.cols, ref mTextLength);

        if (IsoutLine)
        {
            Vector2 pos = Vector2.zero;
            int end = geometry.verts.size;
            int start = 0;
            pos.x = mEffectDistance.x;
            pos.y = mEffectDistance.y;
            ApplyShadow(geometry.verts, geometry.uvs, geometry.cols, start, end, pos.x, -pos.y);

            start = end;
            end = geometry.verts.size;
            ApplyShadow(geometry.verts, geometry.uvs, geometry.cols, start, end, -pos.x, pos.y);

            start = end;
            end = geometry.verts.size;
            ApplyShadow(geometry.verts, geometry.uvs, geometry.cols, start, end, pos.x, pos.y);

            start = end;
            end = geometry.verts.size;
            ApplyShadow(geometry.verts, geometry.uvs, geometry.cols, start, end, -pos.x, -pos.y);
        }

        mesh.vertices = geometry.verts.buffer;
        mesh.colors32 = geometry.cols.buffer;
        int size = geometry.verts.size;
        int triangleCount = ((size / 4) << 1) * 3;
        mesh.triangles = GenerateCachedIndexBuffer(triangleCount, size);
        mesh.uv = geometry.uvs.buffer;
        mMeshFilter.mesh = mesh;
    }

    public void UpdateNGUIText()
    {
        NGUIText.dynamicFont = mfont;
        NGUIText.fontSize = mfontSize == 0 ? 20 : mfontSize;
        NGUIText.tint = color;
        NGUIText.encoding = true;
        NGUIText.fontScale = 1;
        NGUIText.regionWidth = mWidth;
        NGUIText.spacingX = 0;
        NGUIText.spacingY = 5;
        NGUIText.fontStyle = FontStyle.Normal;
        NGUIText.alignment = mAlignment;
        NGUIText.symbolStyle = NGUIText.SymbolStyle.Normal;
        NGUIText.Update(true);
    }

    int[] GenerateCachedIndexBuffer(int triangleCount, int vertexCount)
    {
        int[] rv = new int[triangleCount];
        int index = 0;
        for (int i = 0; i < vertexCount; i += 4)
        {
            rv[index++] = i;
            rv[index++] = i + 1;
            rv[index++] = i + 2;

            rv[index++] = i + 2;
            rv[index++] = i + 3;
            rv[index++] = i;
        }
        return rv;
    }

    public float TextLength
    {
        get
        {
            return mTextLength;
        }
    }

    public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
    {
        Color c = OutColor;

        for (int i = start; i < end; ++i)
        {
            verts.Add(verts.buffer[i]);
            uvs.Add(uvs.buffer[i]);
            cols.Add(cols.buffer[i]);

            Vector3 v = verts.buffer[i];
            v.x += x;
            v.y += y;
            verts.buffer[i] = v;

            Color32 uc = cols.buffer[i];

            if (uc.a == 255)
            {
                cols.buffer[i] = c;
            }
            else
            {
                Color fc = c;
                fc.a = (uc.a / 255f * c.a);
                cols.buffer[i] = fc;
            }
        }
    }
}
