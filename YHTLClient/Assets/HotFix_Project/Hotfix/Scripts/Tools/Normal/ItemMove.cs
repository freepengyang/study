using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemMove : MonoBehaviour
{
    /// <summary>
    /// 当前坐标点
    /// </summary>
    public Vector3 CurPos
    {
        get;
        set;
    }

    /// <summary>
    /// 目标坐标点
    /// </summary>
    public Vector3 GoalPos
    {
        get;
        set;
    }

    public delegate void OnReachToTarget();
    public OnReachToTarget onReachToTarget;


    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CurPos = Vector3.SmoothDamp(CurPos, GoalPos, ref velocity, smoothTime);

        transform.position = CurPos;

        float dis = Vector3.Distance(CurPos, GoalPos);

        if (dis <= 0.2f)
        {
            if (onReachToTarget != null)
            {
                onReachToTarget();
            }
            DestroyImmediate(this);
        }
    }
}
