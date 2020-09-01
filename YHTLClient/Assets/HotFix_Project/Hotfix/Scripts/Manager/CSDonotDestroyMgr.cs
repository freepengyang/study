using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSDonotDestroyMgr : CSGameMgrBase<CSDonotDestroyMgr>
{
    public override bool IsDonotDestroy
    {
        get
        {
            return true;
        }
    }
    CSBetterDic<string,GameObject> mDic;

    public override void Awake()
    {

        mDic = new CSBetterDic<string,GameObject>();
        base.Awake();
    }

    public void AddGameObject(string str,GameObject go)
    {
        if (mDic.ContainsKey(str)) return;
        mDic.Add(str, go);
        go.transform.parent = CahcheTrans;
    }

    public void RemoveGameObject(string str)
    {
        mDic.Remove(str);
    }
}
