using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPathData 
{
    private CSBetterList<Node> mPath;
    private CSBetterList<Node> mPathData;
    private CSBetterList<Node> mArray = new CSBetterList<Node>();

    public CSBetterList<Node> PathArray
    {
        get { return mPath; }
        set
        {
            if (mPath == null) mPath = new CSBetterList<Node>();

            mPath = value;
        }
    }

    public CSBetterList<Node> GetPathNode()
    {
        if (mPathData == null) mPathData = new CSBetterList<Node>();

        mPathData.Clear();

        if (PathArray != null && PathArray.Count > 0)
        {
            // 取值范围
            int StartIndex = 0;
            int EndCount = 0;
            StartIndex = 1;                          // 排除自己
            EndCount = PathArray.Count - StartIndex; // 剩余的总个数
            if (EndCount <= 0) return null;

            // 至少一个格子
            if (EndCount <= 0) EndCount = 1;

            if (EndCount > PathArray.Count)
            {
                EndCount = PathArray.Count;
            }

            // 排除自己的位置0
            mArray = PathArray.GetRange(StartIndex, EndCount, mArray);

            for (int i = 0; i < mArray.Count; ++i)
            {
                Node node = mArray[i] as Node;
                mPathData.Add(node);
            }
            PathArray.Clear();
        }
        return mPathData;
    }

    public static float GetAndAddSpeed(int direction, float stepTime)
    {
        switch (direction)
        {
            case CSDirection.Right:
            case CSDirection.Left:
                return (CSCell.Size.x / stepTime);
            case CSDirection.Up:
            case CSDirection.Down:
                return (CSCell.Size.y / stepTime);
            case CSDirection.Left_Down:
            case CSDirection.Left_Up:
            case CSDirection.Right_Up:
            case CSDirection.Right_Down:
                return (Mathf.Sqrt(CSCell.Size.y * CSCell.Size.y + CSCell.Size.x * CSCell.Size.x) / stepTime);
        }
        return 10.0f;
    }

    public void Clear()
    {
        PathArray?.Clear();
    }

    public void Release()
    {
        mPath?.Release();
    }
}
