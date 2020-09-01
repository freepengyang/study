using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillEffectSystem : MonoBehaviour
{
    public Transform Anchor { get; set; }  //技能锚点（技能特效在该锚点下）
    public CSSkillStartEffectMulti StartEffect { get; set; }
    public CSSkillMoveEffectMulti MoveEffect { get; set; }
    public CSSkillEndEffectMulti EndEffect { get; set; }
    public CSSkillEndFootEffectMulti EndFootEffect { get; set; }

    public CSSkillEffectData startEffectData;
    public CSSkillEffectData moveEffectData;
    public CSSkillEffectData endEffectData;
    public CSSkillEffectData endFootEffectData;

    private AvatarUnit mAvatar;
    private TABLE.SKILLEFFECT effect;

    public static bool IsVaild(int skillEffectId)
    {
        TABLE.SKILLEFFECT tbl;
        if(SkillEffectTableManager.Instance.TryGetValue(skillEffectId,out tbl))
        {
            return true;
        }
        return false;
    }

    public void Create(AvatarUnit avatar, AvatarUnit target, Transform parent, int skillEffectId, int targetCoordX, int targetCoordY)
    {
        if (!SkillEffectTableManager.Instance.TryGetValue(skillEffectId, out effect))
        {
            return;
        }
        //Anchor = new GameObject(avatar.UnitID.ToString()).transform;
        Anchor = new GameObject(avatar.Go.name).transform;
        mAvatar = avatar;
        Transform trans = (avatar.AvatarType == EAvatarType.MainPlayer) ?
            parent : CSPreLoadingBase.Instance.SkillAnchor;
        NGUITools.SetParent(trans, Anchor.gameObject);

        Initialized(targetCoordX, targetCoordY);
        StartEffect = new CSSkillStartEffectMulti();
        StartEffect.Init(mAvatar,target, startEffectData,parent);

        MoveEffect = new CSSkillMoveEffectMulti();
        MoveEffect.Init(mAvatar, target, moveEffectData, parent);

        EndEffect = new CSSkillEndEffectMulti();
        EndEffect.Init(mAvatar, target, endEffectData, parent);

        EndFootEffect = new CSSkillEndFootEffectMulti();
        EndFootEffect.Init(mAvatar, target, endFootEffectData, parent);

        StartEffect.UpdateNextEffect(MoveEffect,EndEffect,EndFootEffect);
        MoveEffect.UpdateNextEffect(EndEffect,EndFootEffect);

    }

    private void Initialized(int targetCoordX, int targetCoordY)
    {
        startEffectData = GetSkillEffectData(targetCoordX, targetCoordY, effect.id, effect.model, effect.delaytime, effect.scale,effect.cengji, effect.zhenlv,effect.cengji, effect.xuanzhuan, 
            effect.modelframelist, effect.frame, effect.num, effect.point, effect.beAttackDelayTime, effect.footEffectType,effect.cameraShakeDelay,effect.CameraShakeTime,effect.CameraShakeAmplitude);

        moveEffectData = GetSkillEffectData(targetCoordX, targetCoordY, effect.id, effect.model3, effect.delaytime3, effect.scale3,effect.cengji3, effect.zhenlv3,
            effect.cengji3, effect.xuanzhuan3, effect.modelframelist3, effect.frame3, effect.num, effect.point, effect.beAttackDelayTime, effect.footEffectType, effect.cameraShakeDelay, effect.CameraShakeTime, effect.CameraShakeAmplitude);

        endEffectData = GetSkillEffectData(targetCoordX, targetCoordY, effect.id, effect.model2, effect.delaytime2, effect.scale2,effect.cengji2, effect.zhenlv2, 
            effect.cengji2, effect.xuanzhuan2, effect.modelframelist2, effect.frame2, effect.num, effect.point, effect.beAttackDelayTime, effect.footEffectType, effect.cameraShakeDelay, effect.CameraShakeTime, effect.CameraShakeAmplitude);

        endFootEffectData = GetSkillEffectData(targetCoordX, targetCoordY, effect.id, effect.model4, effect.delaytime4, effect.scale4,effect.cengji4,effect.zhenlv4,
            effect.cengji4, effect.xuanzhuan4, effect.modelframelist4, effect.frame4,effect.num, effect.point, effect.beAttackDelayTime, effect.footEffectType, effect.cameraShakeDelay, effect.CameraShakeTime, effect.CameraShakeAmplitude);
    }

    private CSSkillEffectData GetSkillEffectData(int targetCoordX, int targetCoordY,int skillEffectId,int modelID, float delayTime, float scale, int layer, int fps, int depthType, 
        int rotationDirection, int modelFrameDir, int frameCount, int num,string strPoint,int beAttackDelayTime, int footEffectType,int shakeDelay, int shakeTime, int shakeAmplitude)
    {
        CSSkillEffectData effectData = new CSSkillEffectData();
        effectData.targetCoordX = targetCoordX;
        effectData.targetCoordY = targetCoordY;
        effectData.ID = skillEffectId;
        effectData.ModelID = modelID;
        effectData.DelayTime = delayTime * 0.001f;
        effectData.Scale = scale * 0.01f;
        effectData.Layer = layer;
        effectData.Fps = (fps == 0) ? 20 : fps;
        effectData.DepthType = (depthType == 0) ? 200 : depthType;
        effectData.RotationDirction = rotationDirection;
        effectData.IsMultiDirection = (modelFrameDir != 0);
        effectData.FrameCount = frameCount;
        effectData.Num = num;
        effectData.Point = strPoint;
        effectData.BeAttackDelayTime = beAttackDelayTime;
        effectData.footEffectType = footEffectType;
        effectData.cameraShakeDelay = shakeDelay * 0.001f;
        effectData.cameraShakeTime = shakeTime*0.001f;
        effectData.cameraShakeAmplitude = shakeAmplitude * 0.001f;
        return effectData;
    }


    public void Attach(AvatarUnit avatar, AvatarUnit target,int skillEffectID, int targetCoordX, int targetCoordY)
    {
        if (StartEffect == null || MoveEffect == null || EndEffect == null || EndFootEffect == null)
        {
            return;
        }
        int lastSkillId = (effect == null) ? 0 : effect.id;
        if(skillEffectID == lastSkillId)
        {
            StartEffect.UpdateIAvater(avatar, target);
            MoveEffect.UpdateIAvater(avatar, target);
            EndEffect.UpdateIAvater(avatar, target);
            EndFootEffect.UpdateIAvater(avatar, target);
            return;
        }
        if (!SkillEffectTableManager.Instance.TryGetValue(skillEffectID, out effect))
        {
            return;
        }
        Initialized(targetCoordX, targetCoordY);
        StartEffect.UpdateSkill(avatar, target, startEffectData, Anchor);
        MoveEffect.UpdateSkill(avatar, target, moveEffectData, Anchor);
        EndEffect.UpdateSkill(avatar, target, endEffectData, Anchor);
        EndFootEffect.UpdateSkill(avatar, target, endFootEffectData, Anchor);
    }

    public void RemoveAttach()
    {
        if (StartEffect != null)
        {
            StartEffect.RemoveAttach();
        }
        if (MoveEffect != null)
        {
            MoveEffect.RemoveAttach();
        }
        if (EndEffect != null)
        {
            EndEffect.RemoveAttach();
        }
        if (EndFootEffect != null)
        {
            EndFootEffect.RemoveAttach();
        }
    }

    public void Update()
    {
        if (StartEffect != null)
        {
            StartEffect.Update();
        }
        if (MoveEffect != null)
        {
            MoveEffect.Update();
        }
        if (EndEffect != null)
        {
            EndEffect.Update();
        }
        if (EndFootEffect != null)
        {
            EndFootEffect.Update();
        }
    }

    /// <summary>
    /// 播放技能特效
    /// </summary>
    /// <param name="delayTime">技能特效延迟播放时间</param>
    public void Play(AvatarUnit target, AvatarUnit attack, int targetCoordX, int targetCoordY)
    {
        if(mAvatar == null || attack == null)
        {
            return;
        }
        mAvatar = attack;
        if (MoveEffect != null)
        {
            MoveEffect.UpdateData(target, attack, targetCoordX, targetCoordY);
        }
        if (EndEffect != null)
        {
            EndEffect.UpdateData(target, attack, targetCoordX, targetCoordY);
        }
        if (EndFootEffect != null)
        {
            EndFootEffect.UpdateData(target, attack, targetCoordX, targetCoordY);
        }
        if (StartEffect != null)
        {
            StartEffect.UpdateData(target, attack, targetCoordX, targetCoordY);
            float delayTime = 0;
            if (mAvatar.Model != null)
            {
                delayTime = mAvatar.Model.AttackActionTime() * ((float)effect.delaytime) / 1000;
            }
            StartEffect.Play(delayTime);
        }
    }

    public bool IsPlaying()
    {
        if(StartEffect != null && (StartEffect.isPlaying || StartEffect.IsBeginDelay))
        {
            return true;
        }
        if (MoveEffect != null && MoveEffect.isPlaying)
        {
            return true;
        }
        if (EndEffect != null && EndEffect.isPlaying)
        {
            return true;
        }
        if (EndFootEffect != null && EndFootEffect.isPlaying)
        {
            return true;
        }
        return false;
    }

    public float StartPlayHurt()
    {
        if(effect == null)
        {
            return 0;
        }
        return effect.startPlayHurt * 0.001f;
    }

    public void Release()
    {
       
    }

    public void Stop()
    {
        if(StartEffect != null)
        {
            StartEffect.Stop();
        }
        if(MoveEffect != null)
        {
            MoveEffect.Stop();
        }
        if(EndEffect != null)
        {
            EndEffect.Stop();
        }
        if (EndFootEffect != null)
        {
            EndFootEffect.Stop();
        }
    }
}
