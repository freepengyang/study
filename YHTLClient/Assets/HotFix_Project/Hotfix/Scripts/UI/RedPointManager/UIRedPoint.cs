using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIRedPoint : IDispose
{
    private GameObject gameObject;

    private bool _mEnabled = false; //是否激活
    public List<RedPointType> RedPointList = new List<RedPointType>();

    public void Start()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
        Show();
    }

    public void Show()
    {
        OnReceiveRedPointMessage(0, null);
    }

    public void Register(GameObject go, RedPointType type)
    {
        gameObject = go;
        if (!RedPointList.Contains(type))
        {
            UIRedPointManager.Instance.mRedEvent.AddEvent(type, OnReceiveRedPointMessage);
            RedPointList.Add(type);
        }

        Start();
    }

    public void RegisterList(GameObject go, params RedPointType[] typeList)
    {
        gameObject = go;
        if (typeList != null && typeList.Length > 0)
        {
            for (var i = 0; i < typeList.Length; i++)
            {
                if (!RedPointList.Contains(typeList[i]))
                {
                    RedPointList.Add(typeList[i]);
                    UIRedPointManager.Instance.mRedEvent.AddEvent(typeList[i], OnReceiveRedPointMessage);
                }
            }
        }

        Start();
    }

    //接收小红点返回数据
    private void OnReceiveRedPointMessage(uint id, object data)
    {
        _mEnabled = false;
        if (RedPointList == null || gameObject == null) return;
        for (int i = 0; i < RedPointList.Count; i++)
        {
            _mEnabled = UIRedPointManager.Instance.GetRedPointState(RedPointList[i]);
            if (_mEnabled)
                break;
        }

        if (gameObject != null)
        {
            gameObject.SetActive(_mEnabled);
        }
    }

    public void OnHideRedPoint()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    public void Dispose()
    {
        gameObject = null;
        _mEnabled = false;
        if (RedPointList != null)
        {
            for (int i = 0; i < RedPointList.Count; i++)
            {
                UIRedPointManager.Instance.mRedEvent.RemoveEvent(RedPointList[i], OnReceiveRedPointMessage);
            }

            RedPointList.Clear();
        }
    }
}