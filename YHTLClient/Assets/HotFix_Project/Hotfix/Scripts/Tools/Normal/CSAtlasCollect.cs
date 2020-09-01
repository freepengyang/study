using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSAtlasCollect : MonoBehaviour
{

    //[System.Serializable]
    //public class ObjData
    //{
    //    public Texture tex;
    //    //public Texture alphaTex;
    //    public int referenceCount;
    //}

    //private bool mStart = true;
    //public static Dictionary<Texture, ObjData> refDataDic = new Dictionary<Texture, ObjData>();
    //public List<ObjData> objList = new List<ObjData>();
    //void Awake()
    //{
    //    if (objList.Count == 0)
    //    {
    //        //Debug.LogError("objList.Count == 0 " + gameObject.name);
    //    }
    //    AddRefData();
    //}

    //void AddRefData()
    //{
    //    for (int i = 0; i < objList.Count; i++)
    //    {
    //        AddRefData(objList[i]);
    //    }
    //}

    //void AddRefData(ObjData data)
    //{
    //    if (data == null) return;
    //    ObjData d = null;
    //    if (data.tex == null) return;
    //    if (!refDataDic.ContainsKey(data.tex))
    //    {
    //        d = new ObjData();
    //        d.referenceCount = 0;
    //        d.tex = data.tex;
    //        //d.alphaTex = data.alphaTex;
    //        refDataDic.Add(data.tex, d);
    //    }
    //    else
    //    {
    //        d = refDataDic[data.tex];
    //    }
    //    d.referenceCount++;
    //}


    //void OnDestroy()
    //{
    //    RemoveRefData();

    //    //objList.Clear();

    //    //System.GC.Collect(0);
    //}

    //void RemoveRefData()
    //{
    //    for (int i = objList.Count - 1; i >= 0; i--)
    //    {
    //        RemoveRefData(objList[i]);
    //    }
    //    objList.Clear();
    //}

    //void RemoveRefData(ObjData d)
    //{
    //    if (d == null || d.tex == null) return;
    //    if (refDataDic.ContainsKey(d.tex))
    //    {
    //        ObjData data = refDataDic[d.tex];
    //        data.referenceCount--;
    //        if (data.referenceCount <= 0)
    //        {
    //            refDataDic.Remove(data.tex);
    //            DestroyTex(data);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("数据不对应，请将所有UI Prefab重新生成数据 =" + gameObject.name);
    //    }
    //}

    //void DestroyTex(ObjData data)
    //{
    //    if (data != null)
    //    {
    //        if (data.tex != null)
    //        {
    //            //Debug.Log("UnLoad Asset = " + data.tex.name);
    //            Resources.UnloadAsset(data.tex);
    //        }
    //        //if (data.alphaTex != null)
    //        //{
    //        //    //Debug.Log("UnLoad Asset = " + data.alphaTex.name);
    //        //    Resources.UnloadAsset(data.alphaTex);
    //        //}
    //    }
    //}

    //public void ClearObjList()
    //{
    //    objList.Clear();
    //}

    //public void AddObjData(Texture tex, Texture alphaTex, GameObject go)
    //{
    //    if (tex == null || go == null) return;
    //    ObjData data = objList.Find((ObjData d) => { return d.tex == tex; });
    //    if (data != null) return;
    //    data = new ObjData();
    //    data.tex = tex;
    //    //data.alphaTex = alphaTex;
    //    data.referenceCount = 0;
    //    objList.Add(data);
    //}
}
