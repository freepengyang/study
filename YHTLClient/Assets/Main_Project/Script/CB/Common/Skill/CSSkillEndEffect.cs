using UnityEngine;
using System.Collections;

public class CSSkillEndEffect : CSSkillEffect
{
    private bool mIsMove = false;
    private bool mIsWaitToResSkill = false;
    private float mBeginPlayTime = 0;
    protected override ESkillEffectType mType
    {
        get
        {
            return ESkillEffectType.End;
        }
    }

    public override void Play(float time)
    {
        if (entity != null && entity.activeSelf)
        {
            entity.SetActive(false);
        }

        int curDirection = (int)mAvatar.GetDirection();

        if (Info.targetCoordX == 0 && Info.targetCoordY == 0)
        {
            CSMisc.Dot2 d = mAvatar.OldCell.Coord;
            CSCell cell = CSMesh.Instance.getCell(d.x, d.y);
            attackPosition = (AttackTarget != null) ? AttackTarget.OldCell : cell;
        }
        else
        {
            CSCell cell = CSMesh.Instance.getCell(Info.targetCoordX, Info.targetCoordY);
            attackPosition = cell;
            base.AttackTarget = null;
        }

        base.curDirection = curDirection;
        base.attackPosition = attackPosition;
        base.Play(time);
    }


    protected override void OnLoadCallBack()
    {
        if (animation != null)
        {
            BaseFrame b = animation.FrameAni as BaseFrame;

            b.onFinish -= OnFinish;
            b.onFinish += OnFinish;
        } 
    }
   
    protected override void SetPos()
    {
        if (entity == null)
        {
            return;
        }
        if (AttackTarget == null && attackPosition == null)
        {
            return;
        }
        Vector3 offsetVec = GetOffsetVec();
        Vector3 localPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(AttackTarget), 0);
        if (cacheWorldTrans != null)
        {
            mHalfTargetHeight = cacheWorldTrans.TransformPoint(localPostion);
        }
        Vector3 vec = TargetPosition;
        vec.z = cacheTrans.position.z;
        cacheTrans.position = vec;
    }

    public override void OnStart()
    {
        if (animation != null)
        {
            BaseFrame b = animation.FrameAni as BaseFrame;
            if (animation.Go != null)
            {
                animation.Go.SetActive(true);
            }
            mIsMove = true;
        }
        mIsWaitToResSkill = false;

        if(Info != null && Info.BeAttackDelayTime > CSMisc.FloatZero)
        {
            mIsWaitToResSkill = true;
            mBeginPlayTime = Time.time;
        }
        else
        {
            ApplyMoveOverEffect();
        }
        base.OnStart();
    }
	
	public override void Update()
    {
        base.Update();
        if(Info == null || Info.BeAttackDelayTime <= CSMisc.FloatZero)
        {
            return;
        }

        if (mIsWaitToResSkill)
        {
            if (Time.time - mBeginPlayTime >= Info.BeAttackDelayTime)
            {
                mIsWaitToResSkill = false;
                ApplyMoveOverEffect();
            }
        }

        if (mIsMove && entity != null)
        {
            entity.transform.position = TargetPosition;
        }
    }

    public override void Stop()
    {
        base.Stop();
        mIsMove = false;
        mIsWaitToResSkill = false;
    }

    private void ApplyMoveOverEffect()
    {
        //if(mAvatar != null)
        //{
        //    //mAvatar.OnSkillResult();
        //}
        //if (mAvatar != null && mAvatar.SkillEngine.IsMoveOverApplySkillEffect(mSkill))
        //{
        //    mAvatar.ResSkillEffect();
        //}
    }

}
