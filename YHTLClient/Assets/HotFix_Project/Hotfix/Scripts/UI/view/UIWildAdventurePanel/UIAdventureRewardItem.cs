using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdventureRewardItem : IDispose
{
    public GameObject go;
    UISprite sp_icon;

    UIAdventureRewardManager manager;

    Schedule removeSch;

    public void Init(UIAdventureRewardManager _manager, GameObject _go, string icon, float timer = -1f)
    {
        if (_manager == null ||_go == null) return;
        manager = _manager;
        go = _go;
        sp_icon = go.GetComponent<UISprite>();
        if (!string.IsNullOrEmpty(icon)) sp_icon.spriteName = icon;

        Timer.Instance.CancelInvoke(removeSch);
        if (timer > 0)
        {
            removeSch = Timer.Instance.Invoke(timer, RemoveTimer);
        }
    }


    void RemoveTimer(Schedule sch)
    {
        if (manager != null && go != null)
        {
            manager.RecycleItem(this);
        }
    }


    public void Dispose()
    {
        Timer.Instance.CancelInvoke(removeSch);
        manager = null;
        go = null;
        sp_icon = null;
    }


}


public class UIAdventureRewardManager : IDispose
{
    GameObject template;
    float autoRecycleItemTime;

    int baseDepth;
    int DepthOffset;

    Vector3 basePos;
    Vector2 showPosOffset = new Vector2(100, 60);


    PoolHandleManager mPoolHandle = new PoolHandleManager();

    List<GameObject> objPool = new List<GameObject>();

    public void InitManager(GameObject _go)
    {
        if (_go == null) return;
        template = _go;
        template.SetActive(false);
        basePos = template.transform.localPosition;

        if (mPoolHandle == null) mPoolHandle = new PoolHandleManager();
        if (objPool == null) objPool = new List<GameObject>();
    }


    public void GetItem(string icon, float removeTime = -1f)
    {
        GameObject go = GetUseableObj();
        if (go == null) return;
        UIAdventureRewardItem item = mPoolHandle.GetCustomClass<UIAdventureRewardItem>();
        item.Init(this, go, icon, removeTime);
    }


    public void RecycleItem(UIAdventureRewardItem item)
    {
        if (objPool == null) objPool = new List<GameObject>();
        item.go.SetActive(false);
        objPool.Add(item.go);
        mPoolHandle.Recycle(item);
    }


    GameObject GetUseableObj()
    {
        if (template == null) return null;
        if (objPool == null) objPool = new List<GameObject>();

        GameObject useable = null;
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].activeSelf == false)
            {
                useable = objPool[i];
                objPool.RemoveAt(i);
                break;
            }
        }
        if (useable == null)
        {
            useable = GameObject.Instantiate(template) as GameObject;
        }
        
        useable.transform.SetParent(template.transform.parent);
        useable.transform.localEulerAngles = template.transform.localEulerAngles;
        useable.transform.localScale = template.transform.localScale;

        float ranX = Random.Range(-showPosOffset.x, showPosOffset.x);
        float ranY = Random.Range(-showPosOffset.y, showPosOffset.y);
        useable.transform.localPosition = basePos + new Vector3(ranX, ranY, 0);

        useable.SetActive(true);

        return useable;
    }


    public void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        objPool?.Clear();
        objPool = null;
    }
}