using UnityEngine;
using System.Collections;

public class CSMoveToTargetPos
{
    private Vector3 mTarget;
    private AvatarUnit mAvatar;
    private System.Action onMoveUpdate;
    private System.Action onMoveFinish;

    private bool mIsBegin = false;
    private bool mIsFinish = false;

    public void BeginMove(AvatarUnit avatar, Vector3 target, System.Action moveFinishAction, System.Action moveUpdateAction = null)
    {
        mAvatar = avatar;
        mTarget = target;
        onMoveUpdate = moveUpdateAction;
        onMoveFinish = moveFinishAction;
        mIsBegin = true;
        mIsFinish = false;
    }

    // Update is called once per frame
    public void Update () 
    {
        if (!mIsBegin || mAvatar == null || mAvatar.CacheTransform == null)
        {
            return;
        }
        Vector3 vec = Vector3.Lerp(mAvatar.GetRealPosition2(), mTarget, Time.deltaTime * 13);
        mAvatar.CacheTransform.position = vec;
        if (Vector3.Distance(vec, mTarget) < 0.01f)
        {
            mIsBegin = false;
            mIsFinish = true;
            if (onMoveFinish != null)
            {
                onMoveFinish();
                onMoveFinish = null;
            }
        }
        else
        {
            onMoveUpdate?.Invoke();
        }
    }

    public void Stop()
    {
        mIsBegin = false;
    }

    public void Destroy()
    {
        mIsBegin = false;
        mIsFinish = false;
        mAvatar = null;
        onMoveUpdate = null;
        onMoveFinish = null;
    }
}
