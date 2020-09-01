using System.Collections.Generic;
using UnityEngine;

public class CSRedPointManager : CSInfo<CSRedPointManager>
{
    private PoolHandleManager _manager = new PoolHandleManager();

    private Dictionary<int, UIRedPoint> _UIResPoints = new Dictionary<int, UIRedPoint>(128);

    /// <summary>
    /// 注册单个小红点协议
    /// </summary>
    /// <param name="go"></param>
    /// <param name="type"></param>
    public void RegisterRedPoint(GameObject go, RedPointType type)
    {
        UIRedPoint redPoint = GetRedPoint(go.GetHashCode());
        redPoint.Register(go, type);
    }

    /// <summary>
    /// 注册多个小红点协议
    /// </summary>
    /// <param name="go"></param>
    /// <param name="typeList"></param>
    public void RegisterRedPoint(GameObject go, params RedPointType[] typeList)
    {
        UIRedPoint redPoint = GetRedPoint(go.GetHashCode());
        redPoint.RegisterList(go, typeList);
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="go"></param>
    public void Recycle(GameObject go)
    {
        UIRedPoint redPoint;
        var hashCode = go.GetHashCode();
        if (_UIResPoints.ContainsKey(hashCode))
        {
            redPoint = _UIResPoints[hashCode];
            _UIResPoints.Remove(go.GetHashCode());
            redPoint.Dispose();
            _manager.Recycle(redPoint);
        }
    }

    /// <summary>
    /// 关闭所有小红点
    /// </summary>
    public void CloseAllRedPoint()
    {
        if (_UIResPoints != null && _UIResPoints.Count > 0)
        {
            for(var it = _UIResPoints.GetEnumerator();it.MoveNext();)
            {
                it.Current.Value?.OnHideRedPoint();
            }
        }
    }

    private UIRedPoint GetRedPoint(int id)
    {
        UIRedPoint redPoint;
        if (!_UIResPoints.ContainsKey(id))
        {
            redPoint = _manager.GetCustomClass<UIRedPoint>();
            _UIResPoints.Add(id, redPoint);
        }
        else
        {
            redPoint = _UIResPoints[id];
        }

        return redPoint;
    }


    public override void Dispose()
    {
        if (_UIResPoints != null)
        {
            for (var it = _UIResPoints.GetEnumerator(); it.MoveNext();)
            {
                it.Current.Value?.Dispose();
            }

            _UIResPoints.Clear();
        }

        _UIResPoints = null;
        _manager.OnDestroy();
        _manager = null;

    }
}