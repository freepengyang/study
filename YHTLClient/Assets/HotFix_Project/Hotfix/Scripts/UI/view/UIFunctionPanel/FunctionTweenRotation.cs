using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTweenRotation : IDispose
{
    private const float RADIANTOANGER = 57.3f;
    private const float RATETANX = 450;
    private const int maxCount = 6;

    PoolHandleManager _poolHandleManager = new PoolHandleManager();

    private List<FunctionItem> _functionItems = new List<FunctionItem>();

    private Transform mPrefab;

    private FunctionRotateDir funtionRotateDir;

    private TweenRotation _rotation;

    private Schedule _schedule;

    private float angle;
    private int rate;
    private Vector3 mdelVec;
    private int maxleftAngeLimit = 0;
    private int minleftAngeLimit = 0;
    private int maxrightAngeLimit = 0;
    private int minrightAngeLimit = 0;
    private int delAngle = 0;
    EventDelegate _eventDelegate;


    public void Init(Transform parnet, FunctionRotateDir dir, int delAngle)
    {
        if (_functionItems == null) _functionItems = new List<FunctionItem>();
        mPrefab = parnet;
        funtionRotateDir = dir;
        mdelVec = mPrefab.localEulerAngles;
        this.delAngle = delAngle;
        _rotation = parnet.GetComponent<TweenRotation>();

        rate = funtionRotateDir == FunctionRotateDir.left ? -1 : 1;

        _eventDelegate = new EventDelegate(StopRotation);
    }

    public void AddChild(GameObject obj)
    {
        FunctionItem item = _poolHandleManager.GetCustomClass<FunctionItem>();
        item.Init(obj, this);
        _functionItems.Add(item);

        if (_functionItems.Count > maxCount)
            if (funtionRotateDir == FunctionRotateDir.left)
            {
                minleftAngeLimit = (_functionItems.Count - 6) * delAngle * -1;
            }
            else
            {
                maxrightAngeLimit = (_functionItems.Count - 6) * delAngle;
            }

        PlayItemPos();
    }

    public void OnDragStart()
    {
        if (_functionItems.Count <= maxCount) return;
        if (_rotation != null && _rotation.enabled)
        {
            _rotation.enabled = false;
            _rotation.from = _rotation.to = Vector3.zero;
        }

        if (_schedule != null && Timer.Instance.IsInvoking(_schedule))
            Timer.Instance.CancelInvoke(_schedule);
    }

    public void OnDrag(Vector2 delta)
    {
        if (_functionItems.Count <= maxCount) return;
        angle = mPrefab.localEulerAngles.z + RADIANTOANGER * Mathf.Atan2(delta.y, RATETANX) * rate;
        mdelVec.Set(0, 0, angle);
        mPrefab.localEulerAngles = mdelVec;
        PlayItemPos();
    }

    public void OnDragEnd()
    {
        if (_functionItems.Count <= maxCount) return;
        int delY = 0, curAngle;
        if (GetMaxMinPosY(out delY, out curAngle))
        {
            OnRotate(delY, curAngle);
        }
    }

    private void OnRotate(int angle, int cur)
    {
        if (_rotation == null) return;
        _rotation.from.Set(0, 0, cur);
        _rotation.to.Set(0, 0, angle);

        _rotation.ResetToBeginning();
        _rotation.PlayForward();
        _rotation.onFinished.Add(_eventDelegate);

        _schedule = Timer.Instance.InvokeRepeating(0, 0.01f, PlayItemPos);
    }

    private void StopRotation()
    {
        if (_schedule != null && Timer.Instance.IsInvoking(_schedule))
            Timer.Instance.CancelInvoke(_schedule);
    }

    private void PlayItemPos(Schedule _schedule = null)
    {
        for (int i = 0; i < _functionItems.Count; i++)
        {
            _functionItems[i].OnRotate(mPrefab.localEulerAngles.z);
        }
    }

    private bool GetMaxMinPosY(out int angle, out int cur)
    {
        int max, min;
        if (funtionRotateDir == FunctionRotateDir.left)
        {
            max = maxleftAngeLimit;
            min = minleftAngeLimit;
        }
        else
        {
            max = maxrightAngeLimit;
            min = minrightAngeLimit;
        }

        if (mPrefab.localEulerAngles.z > 180)
            cur = (int) mPrefab.localEulerAngles.z - 360;
        else
            cur = (int) mPrefab.localEulerAngles.z;

        if (cur > max)
        {
            angle = max;
            return true;
        }

        if (cur < min)
        {
            angle = min;
            return true;
        }

        angle = 0;
        return false;
    }

    public void Dispose()
    {
        if (_poolHandleManager != null) _poolHandleManager.RecycleAll();
        for (int i = 0; i < _functionItems.Count; i++)
        {
            _functionItems[i].Dispose();
        }

        Timer.Instance.CancelInvoke(_schedule);
        _functionItems.Clear();
        mPrefab = null;
        _rotation = null;
        _schedule = null;
        _eventDelegate = null;
    }
}


public enum FunctionRotateDir
{
    left,
    right
}