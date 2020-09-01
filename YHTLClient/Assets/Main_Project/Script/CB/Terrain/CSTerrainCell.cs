
//-------------------------------------------------------------------------
//Resource load
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class CSTerrainCell
{
    public GameObject go;
    private Transform mCacheTrans;
    public CSSprite sprite;
    public Texture tex;
    public CSCell Cell;
    public readonly static Vector2 Size = new Vector2(512f, 256f);
    public readonly static Vector2 HalfSize = new Vector2(256f, 128f);
    private Vector4 Bounds;
    private Transform parent;

    public delegate void OnFinish(CSTerrainCell c);
    public event OnFinish onFinish;
    private bool mIsFinish = false;
    public bool IsFinish
    {
        get { return mIsFinish; }
        set { mIsFinish = value; }
    }

    private CSObjectPoolItem mPoolItem;
    public CSObjectPoolItem PoolItem
    {
        get { return mPoolItem; }
        set { mPoolItem = value; }
    }

    public string resPath = string.Empty;

    public void Init(Transform tran, CSCell cell, bool isWaitLoad)
    {
        parent = tran;
        Cell = cell;
        NewGameObject();
    }

    void NewGameObject()
    {
        //TODO:ddn
        if (go == null) go = new GameObject();
        string name = Cell.Coord.x + "_" + Cell.Coord.y;
        go.name = name;
        mCacheTrans = go.transform;
        mCacheTrans.parent = parent;
        mCacheTrans.localPosition = Cell.LocalPosition;
        mCacheTrans.localScale = Vector3.one;

        if (go != null)
        {
            if (sprite == null) sprite = go.GetComponent<CSSprite>();
            if (sprite == null) sprite = go.AddComponent<CSSprite>();
            mIsFinish = false;
            CSStringBuilder.Clear();
            string texName = CSStringBuilder.Append(CSConstant.mapImg.ToString(), "/", CSConstant.mapImg,"_" ,name).ToString();
            sprite.SpriteName = null;
            resPath = CSResource.GetPath(texName, ResourceType.Map, false/*CSConstant.mapImg*/);
            CSResourceManager.Singleton.AddQueue(resPath, ResourceType.Map, LoadMap, ResourceAssistType.ForceLoad, true);
            if (!go.activeSelf) go.SetActive(true);
        }
    }

    private void LoadMap(CSResource res)
    {
        OnWaitDeal(res, null);
    }

    bool OnWaitDeal(object obj, object param)
    {
        CSResource res = obj as CSResource;
        if (res != null)
        {
            if (resPath != res.Path) return false;
            tex = res.GetObjInst() as Texture;

            if (sprite != null && tex != null)
            {
                Shader shader = null;
                Material material = null;
                if (sprite.MainMaterial == null)
                {
                    shader = Shader.Find("Mobile/LZF/Alpha Blended");

                    if (shader != null)
                    {
                        material = new Material(shader);
                    }
                }
                else
                {
                    shader = sprite.MainMaterial.shader;
                    material = sprite.MainMaterial;
                }
                sprite?.SetShader(material, Vector4.one, Vector4.one);
                sprite.Picture = tex;
            }
        }

        if (onFinish != null) onFinish(this);
        mIsFinish = true;
        onFinish = null;

        if(Cell != null)
        {
            if (!CSTerrain.Instance.isInDisplayMapDic(Cell.getKey))
            {
                return false;
            }
        }
        return true;
    }

    public void Destroy()
    {
        if (mPoolItem != null && CSObjectPoolMgr.Instance != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(mPoolItem);
            if (go != null && go.activeSelf)
            {
                go.SetActive(false);
            }
        }
        else
        {
            if (sprite != null)
                GameObject.Destroy(sprite.gameObject);
        }
        mPoolItem = null;
    }
}
