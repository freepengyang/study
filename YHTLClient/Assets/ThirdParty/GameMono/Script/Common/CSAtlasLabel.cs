
/*************************************************************************
** File: CSAtlasLabel.cs
** Author: jiabao
** Time: 2018.8.6
*************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 限制：Atlas中图片的命名以单字符命名
/// </summary>
public class CSAtlasLabel : MonoBehaviour
{
    private int[] triangles = null;
    private Mesh mesh;
    private int length;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private UIGeometry geometry;
    private long damage = 0;
    private List<string> list = new List<string>();
    public int TextType { get; set; }

    private int mGetType
    {
        get
        {
            switch (TextType)
            {
                case ThrowTextType.DivineStrike:
                case ThrowTextType.ShenShengWeiShe:
                case ThrowTextType.ShenShengWeiSheZhongJi:
                case ThrowTextType.ShenShengZhongJi:
                    return 1;
                case ThrowTextType.Exp:
                    return 2;
                case ThrowTextType.WeiSheZhongJi:
                case ThrowTextType.Critical:
                    return 3;
                case ThrowTextType.Cure:
                    return 4;
                case ThrowTextType.JaDe:
                    return 5;
            }
            return 0;
        }
    }

    private string mTypeName
    {
        get
        {
            switch (TextType)
            {
                case ThrowTextType.DivineStrike:
                    return "s";
                case ThrowTextType.Exp:
                    return "d";
                case ThrowTextType.Critical:
                    return "bj";
                case ThrowTextType.Dodge:
                    return "sw";
                case ThrowTextType.JaDe:
                    return "w";
                case ThrowTextType.Immune:
                    return "m";
                case ThrowTextType.GeDang:
                    return "g";
                case ThrowTextType.ShenShengWeiShe:
                    return "ssws";
                case ThrowTextType.ShenShengWeiSheZhongJi:
                    return "sswszj";
                case ThrowTextType.ShenShengZhongJi:
                    return "sszj";
                case ThrowTextType.WeiSheZhongJi:
                    return "wszj";
            }
            return "";
        }

    }

    [HideInInspector]
    [SerializeField]
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
    [HideInInspector]
    [SerializeField]
    private UIAtlas mAtlas;
    public UIAtlas Atlas
    {
        get { return mAtlas; }
        set
        {
            if (mAtlas == value) return;

            mAtlas = value;
            InitComponent();
        }
    }
    public long mDamage
    {
        set
        {
            damage = (long)Mathf.Abs(value);
            list.Clear();
            if (damage > 0)
            {
                SplitData(mGetType, damage);
            }
            if (!string.IsNullOrEmpty(mTypeName))
            {
                list.Add(mTypeName);
            }

            GenerateFilter();
        }
    }

    /// <summary>
    /// 拆分数据
    /// </summary>
    /// <param name="k"></param>
    /// <param name="x"></param>
    private void SplitData(long k, long x)
    {
        long t = x;
        while (true)
        {
            long m = t % 10;
            t /= 10;
            list.Add((k * 10 + m).ToString());

            if (t <= 0)
            {
                break;
            }
        }
    }
    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitComponent()
    {
        if (meshFilter == null) meshFilter = this.GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = this.GetComponent<MeshRenderer>();

        if (meshRenderer.material != null)
        {
            if (meshRenderer.material != mAtlas.spriteMaterial)
            {
                meshRenderer.material = mAtlas.spriteMaterial;
            }
        }
        else
        {
            meshRenderer.material = mAtlas.spriteMaterial;
        }
    }

    public void GenerateFilter()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }
        if (geometry == null) geometry = new UIGeometry();
        mesh.Clear();
        geometry.Clear();
        int length = length = list.Count;
        list.Reverse();

        int vertsLength = length << 2;
        triangles = new int[(length << 1) * 3];
        Texture tex = mAtlas.texture;
        int tmp = 0;
        float x0 = 0;
        float yo = 0;
        for (int i = 0; i < vertsLength; i += 4)
        {
            string s = list[i / 4];

            UISpriteData mSprite = mAtlas.GetSprite(s);

            if (mSprite == null) continue;

            yo = mSprite.height;

            //setting vertices
            geometry.verts.Add(new Vector3(x0, yo));
            geometry.verts.Add(new Vector3(x0, 0));
            x0 += mSprite.width;
            geometry.verts.Add(new Vector3(x0, yo));
            geometry.verts.Add(new Vector3(x0, 0));

            //setting color
            geometry.cols.Add(color);
            geometry.cols.Add(color);
            geometry.cols.Add(color);
            geometry.cols.Add(color);

            //setting uvs
            Rect inner = new Rect(mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
                mSprite.width - mSprite.borderLeft - mSprite.borderRight,
                mSprite.height - mSprite.borderBottom - mSprite.borderTop);
            inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

            geometry.uvs.Add(new Vector2(inner.xMin, inner.yMax));
            geometry.uvs.Add(new Vector2(inner.xMin, inner.yMin));
            geometry.uvs.Add(new Vector2(inner.xMax, inner.yMax));
            geometry.uvs.Add(new Vector2(inner.xMax, inner.yMin));
        
        }

        x0 = x0 / 2;

        for (int i = 0; i < geometry.verts.Count; i++)
        {
            Vector3 v = geometry.verts[i];
            geometry.verts[i].Set(v.x - x0, v.y, v.z);
        }

        for (int i = 0; i < triangles.Length; i += 6)
        {
            tmp = (i / 3) << 1;
            triangles[i] = triangles[i + 3] = tmp;
            triangles[i + 1] = triangles[i + 5] = tmp + 3;
            triangles[i + 2] = tmp + 1;
            triangles[i + 4] = tmp + 2;
        }

        mesh.vertices = geometry.verts.buffer;
        mesh.colors32 = geometry.cols.buffer;
        mesh.triangles = triangles;
        mesh.uv = geometry.uvs.buffer;
        meshFilter.mesh = mesh;
    }

    public void SetColor(Color c)
    {
        if (geometry == null  || meshFilter.mesh == null) return;

        if (geometry.cols.Count != geometry.verts.Count) return;

        color = c;
        for (int i = 0; i < geometry.cols.Count; i += 4)
        {
            geometry.cols[i] = color;
            geometry.cols[i + 1] = color;
            geometry.cols[i + 2] = color;
            geometry.cols[i + 3] = color;
        }
        meshFilter.mesh.colors32 = geometry.cols.buffer;
    }
}
