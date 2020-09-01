using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CSSkillMoveEffect : CSSkillEffect
{
    private bool isMove = false;
    public CSSkillEndEffectMulti endEffect;
    public CSSkillEndFootEffectMulti endFootEffect;

    protected override ESkillEffectType mType { get { return ESkillEffectType.Move; } }

    public override void Play(float time)
    {
        if(Info == null)
        {
            return;
        }
        curDirection = (int)mAvatar.GetDirection();
        if(Info.targetCoordX == 0 && Info.targetCoordY == 0)
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
        base.OnLoadCallBack();

        if (animation != null)
        {
            animation.setLoop(true);
            BaseFrame b = animation.FrameAni as BaseFrame;
            b.onFinish -= OnFinish;
            b.onFinish += OnFinish;
        }
        else
        {
            OnStart();
        }
    }


    protected override void SetPos()
    {
        if (entity == null)
        {
            return;
        }
        Transform mCahceWorldTrans = CSPreLoadingBase.CahceWorldTrans;
        Vector3 LocalPostion = Vector3.zero;
        mSkillDir = null;

        uint anchor = (mSkillDir != null) ? mSkillDir.maodian : 0;
        Vector3 offsetVec = GetOffsetVec();
        LocalPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(mAvatar), 0);
        Vector3 halfSelf = mCahceWorldTrans.TransformPoint(LocalPostion);
        //LocalPostion = new Vector3(offsetVec.x, offsetVec.y + GetAnchorHeight(anchor, AttackTarget), 0);
        LocalPostion = new Vector3(0,GetAnchorHeight(AttackTarget), 0);
        mHalfTargetHeight = mCahceWorldTrans.TransformPoint(LocalPostion);
        Vector3 vec = (startPosition != null) ? (startPosition.WorldPosition3 + halfSelf) : (mAvatar.OldCell.WorldPosition3 + halfSelf);
        vec.z = cacheTrans.position.z;
        cacheTrans.position = vec;
    }

    public override void Update()
    {
        base.Update();
        MoveTO();
    }

    public override float GetEffectTime()
    {
        Vector3 pos = animation.CahcheTrans.position;
        pos.z = 0;
        Vector3 p = (AttackTarget != null) ? (AttackTarget.GetRealPosition2() + 
            mHalfTargetHeight) : (attackPosition.WorldPosition3 + mHalfTargetHeight);
        float distanse = Vector3.Distance(pos, p);
        float time = distanse / speed;
        return time;
    }

    public override void OnStart()
    {
        if (animation != null)
        {
            //BaseFrame b = animation.FrameAni as BaseFrame;
            //b.onStart -= OnStart;
            isMove = true;
            if (animation.Go != null)
            {
                animation.Go.SetActive(true);
            }
        }
        PlayNextEffect();
        base.OnStart();
    }

    private void PlayNextEffect()
    {
        if(!isMove)
        {
            float delayTime = 0f;

            if (animation != null)
            {
                if (endEffect != null)
                {
                    CSSkillEffect skillResult = endEffect.GetSelfSkillEffect();
                    if (skillResult != null && skillResult.Info != null)
                    {
                        delayTime = GetEffectTime() * skillResult.Info.DelayTime;
                    }
                }
            }

            if (mIsSelfSkillEffect)
            {
                if (endEffect != null)
                {
                    CSSkillEffect skillResult = endEffect.GetSelfSkillEffect();
                    if (skillResult != null && skillResult.Info != null)
                    {
                        delayTime = GetEffectTime() * skillResult.Info.DelayTime;
                    }

                    endEffect.Play(delayTime);
                }
                if (endFootEffect != null)
                {
                    endFootEffect.Play(delayTime);
                }
            }
        }
    }

    private void MoveTO()
    {
        if (isMove) 
        {
            if (animation != null)
            {
                Vector3 postWhithNoZ = animation.CahcheTrans.position;
                float z = animation.CahcheTrans.position.z;
                postWhithNoZ.z = 0;
                Vector3 targetPos = TargetPosition;
                Vector3 vt3 = Vector3.MoveTowards(postWhithNoZ, targetPos, Time.deltaTime * speed); // 每秒5米
                vt3.z = z;
                animation.CahcheTrans.position = vt3;
                float distanse = Vector3.Distance(postWhithNoZ, targetPos);
                SetRotation(targetPos);
                // 只有动作或加效果
                if (distanse <= 0.1f)
                {
                    animation.Stop();
                    Stop();
                    isMove = false;
                    PlayNextEffect();
                }
            }
        }
    }

}
