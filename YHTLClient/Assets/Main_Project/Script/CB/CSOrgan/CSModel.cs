
/*************************************************************************
** File: CSModel.cs
** Author: jiabao
** Time: 2015.1.15
** Describe: 模型管理中心
*************************************************************************/
using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSModel
{
    public CSBody Body;       // 身体
    public CSWeapon Weapon;     // 武器
    public CSWing Wing;         // 翅膀
    public CSEffect Effect;     // 特效
    public CSBottom Bottom;     // 选中
    public CSBottomNPC BottomNPC;  // 选中NPC
    private CSAction mAction;
    public int curActionFps = 0;
    private CSOrgan[] parts = null;
    public float lastMotionBeginTime = 0;
    protected int mLastMotion = -1;
    protected int mLastDirection = -1;
    private CSOrganData organData;
    private Transform rootTransform;
    public System.Action<bool> replaceEquip = null;

    public CSAction Action
    {
        get { return mAction; }
    }

    public void Release()
    {
        DestroyBottom();
        Bottom = null;
        BottomNPC = null;
        parts = null;
        replaceEquip = null;
    }

    public CSModel(Transform tr,BoxCollider _box, int avatarType, bool isDataSplit, EShareMatType _matType, System.Action<bool> _replaceEquip)
    {
        this.rootTransform = tr;
        this.replaceEquip = _replaceEquip;
        if (mAction == null) mAction = new CSAction();
        if (parts == null) parts = new CSOrgan[6];
        if (organData == null) organData = new CSOrganData(mAction, _box, avatarType, isDataSplit, _matType);
    }

    public void ShowSelectAndHideOtherBottom(int type)
    {
        switch (type)
        {
            case ModelStructure.Bottom:
                Bottom.Go.SetActive(true);
                BottomNPC.Go.SetActive(false);
                break;
            case ModelStructure.BottomNPC:
                Bottom.Go.SetActive(false);
                BottomNPC.Go.SetActive(true);
                break;
        }
    }

    public void InitPart(CSModelModule module)
    {
        if (module.Body != null && Body == null)
        {
            Body = new CSBody(module.Body, organData);
            parts[0] = Body;
        }
        if (module.Weapon != null && Weapon == null)
        {
            Weapon = new CSWeapon(module.Weapon, organData);
            parts[1] = Weapon;
        }
        if (module.Wing != null && Wing == null)
        {
            Wing = new CSWing(module.Wing, organData);
            parts[2] = Wing;
        }
        if (module.Bottom != null && Bottom == null)
        {
            module.Bottom.SetActive(false);
            Bottom = new CSBottom(module.Bottom, organData);
            parts[3] = Bottom;
        }

        if (module.BottomNPC != null && BottomNPC == null)
        {
            module.BottomNPC.SetActive(false);
            BottomNPC = new CSBottomNPC(module.BottomNPC, organData);
            parts[4] = BottomNPC;
        }

        if (module.Effect != null && Effect == null)
        {
            Effect = new CSEffect(module.Effect, organData);
            parts[5] = Effect;
        }
        InitiOrgan();
        InitPartDepath();
        SetMotionFPS(Action.Motion);
    }

    public int GetModelHeight()
    {
        if (Body != null)
        {
            return Body.GetModelHeight();
        }
        return 0;
    }

    public void InitiOrgan()
    {
        if (parts == null) return;

        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.InitOrgan();
            }
        }
    }

    public void InitPartDepath()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SetPartDepth(GetDepth(part.Structure));
            }
        }
    }

    public void InitAnimationFPS(int modelId)
    {
        if (modelId == 0) return;

        CSModelTools.InitAnimationFPS(modelId, organData.avatarType);
    }

    public void SwitchAction(bool isReset = false)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SwitchAction(mAction.Direction, isReset, GetDepth(part.Structure));
            }
        }
        ReplaceEquip();
    }

    public void SwitchActionDirection(bool isReset = false)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SwitchActionDirection((int)GetDirection(), isReset, GetDepth(part.Structure));
            }
        }
        ReplaceEquip();
        DispatchDirectionChange();
    }

    void ReplaceEquip()
    {
        bool isReplace = false;

        if (mLastMotion != (int)mAction.Motion)
        {
            if (mLastMotion != -1) isReplace = true;
            mLastMotion = (int)mAction.Motion;
        }

        if (mLastDirection != (int)mAction.Direction)
        {
            if (mLastDirection != -1) isReplace = true;
            mLastDirection = (int)mAction.Direction;
        }
        if (isReplace)
        {
            replaceEquip?.Invoke(true);
        }
    }

    public void Play(bool isReset)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.Play(isReset);
            }
        }
    }

 

    public void SetMotionFPS(int motion)
    {
        int avatarType = organData.avatarType;

        if (avatarType == EAvatarType.ZhanHun)
        {
            avatarType = EAvatarType.Player;
        }

        if (!CSMisc.partsFPS[avatarType].ContainsKey(motion))
        {
            if (CSMisc.partsFPS[avatarType].ContainsKey(CSMotion.Static))
            {
                curActionFps = CSMisc.partsFPS[avatarType][CSMotion.Static];
            }
        }
        else
        {
            curActionFps = CSMisc.partsFPS[avatarType][motion];
        }
        SetFPS(curActionFps);
    }

    public void SetFPS(int fps)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SetFPS(fps);
            }
        }
    }

    public void SetLoop(bool bl)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SetLoop(bl);
            }
        }
    }

    public void SetCurrentFrame(int motion)
    {
        if (organData.isDataSplit)
        {
            for (int i = 0; i < parts.Length; ++i)
            {
                CSOrgan part = parts[i];

                if (part != null)
                {
                    part.SetCurrentFrame(motion);
                }
            }
        }
    }

    public void SetStopFrameType(int motion, int stopType)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.SetStopFrameType(motion, stopType);
            }
        }
    }

    public void SetAction(int motion,int stopType = EActionStopFrameType.None)
    {
        // 没有控件的时候，只是初始化了数据。控件上的数据是旧。
        SetMotionFPS(motion);

        int ftame = 6;

        if (CSMisc.motionNamsCount.ContainsKey(motion))
        {
            ftame = CSMisc.motionNamsCount[motion];
        }

        SetCurrentFrame(ftame);
        SetStopFrameType(motion, stopType);
        lastMotionBeginTime = Time.time;

        //TODO:ddn
        if (Action.setAction(motion))
        {
            bool loop = motion == CSMotion.Stand || motion == CSMotion.Run || motion == CSMotion.Walk;
            SetLoop(loop);
            if (organData.isDataSplit)
            {
                Play(true);
            }
            SwitchAction(true);
        }
        else
        {
            if (Action.Motion != CSMotion.Dead)
            {
                if (organData.isDataSplit)
                {
                    Play(!GetLoop(ModelStructure.Body));
                }
                else if (!GetLoop(ModelStructure.Body))  // 同一个动作如果不是循环的，在播发一次,死亡只播放一次
                {
                    Play(true);
                }
            }

        }
        ReplaceEquip();
    }

    public void AttachBottom(CSBottom bottom)
    {
        Bottom = bottom;
        if ((Bottom != null) && (Bottom.GoTrans != null))
        {
            Bottom.GoTrans.parent = rootTransform;
            Bottom.GoTrans.localPosition = new Vector3(0, 0, 1);//比翅膀身一点
        }
    }

    public void AttachBottomNPC(CSBottomNPC bottom)
    {
        BottomNPC = bottom;
        if ((BottomNPC != null) && (BottomNPC.GoTrans != null))
        {
            BottomNPC.GoTrans.parent = rootTransform;
            BottomNPC.GoTrans.localPosition = new Vector3(0, 0, 1);//比翅膀身一点
        }
    }

    public void SetDirection(int direction, bool isForceSet = false)
    {
        if (Action.Direction == direction) return;
        Action.setDirection(direction);
        SwitchActionDirection();
    }

    public void SetDirection(Vector3 position, CSCell oldCell)
    {
        SetDirection(mAction.getDirection(oldCell, position));
    }

    public bool EndOfAction(string motion, int m)
    {
        if (Body!= null)
        {
            return Body.EndOfAction(motion, m);
        }
        return false;
    }

    public void SetModelAtlas(ModelLoadBase p)
    {
        if (parts == null) return;

        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                if (part.Structure == ModelStructure.Bottom || part.Structure == ModelStructure.BottomNPC) continue;

                UIAtlas atlas = p.GetAtlas(part.Structure);

                if (atlas != null)
                {
                    part.SetAtlas(atlas, mAction.Direction);
                }
                else
                {
                    part.SetAtlas(null, 0);
                }
            }
        }

        SetBottomAtlas();
        
    }

    private void SetBottomAtlas()
    {
        if (Body != null && Bottom != null)
        {
            UIAtlas atlas = CSGameManager.Instance.GetStaticAtlas("Scelect");
            if (atlas != null)
            {
                Bottom.SetAtlas(atlas, GetDirection());
                if (Bottom.GoTrans != null)
                {
                    Bottom.GoTrans.localPosition = new Vector3(0, 0, 1);
                }
            }
        }

        if (Body != null && BottomNPC != null)
        {
            UIAtlas atlas = CSGameManager.Instance.GetStaticAtlas("effetc_npc_select");
            if (atlas != null)
            {
                BottomNPC.SetAtlas(atlas, GetDirection());
                if (BottomNPC.GoTrans != null)
                {
                    BottomNPC.GoTrans.localPosition = new Vector3(0, 0, 1);
                }
            }
        }
    }

    public void Destroy()
    {
        mLastMotion = -1;
        mLastDirection = -1;
        ClearOrgan();
        DestroyBottom();
    }

    public void DestroyBottom()
    {
        if (Bottom != null && Bottom.Go != null)
        {

            if (organData.avatarType != EAvatarType.MainPlayer)
            {
                CSGame.MainEventHandler.SendEvent(MainEvent.Avatar_AttachBottom, Bottom);
                Bottom = null;
                CSGame.MainEventHandler.SendEvent(MainEvent.CloseSelectionPanel);
            }
        }
        if (BottomNPC != null && BottomNPC.Go != null)
        {
            if (organData.avatarType != EAvatarType.MainPlayer)
            {
                CSGame.MainEventHandler.SendEvent(MainEvent.Avatar_AttachBottomNPC, BottomNPC);
                BottomNPC = null;
            }
        }
    }

    private void ClearOrgan()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            CSOrgan part = parts[i];

            if (part != null)
            {
                part.ClearAtlas();
            }
        }
    }

    public int GetDepth(int structure)
    {
        if (mAction == null) return 0;

        int direction = mAction.Direction;

        if (direction == CSDirection.None)
        {
            return 0;
        }
        int part = GetBearing(structure);

        switch (part)
        {
            case ModelBearing.Head:
            case ModelBearing.Body:
                return 0;
            case ModelBearing.FootLeft:
            case ModelBearing.HandLeft:
                return CSMisc.DephtLeft[direction];
            case ModelBearing.FootRight:
            case ModelBearing.HandRight:
                return CSMisc.DephtRight[direction];
            case ModelBearing.UnderFoot:
                return 1;
            case ModelBearing.Bottom:
                return 1;
            case ModelBearing.BottomNPC:
                return 1;
            case ModelBearing.Front:
                return -1;
            case ModelBearing.Back:
                if (Action.Motion == CSMotion.Dead)
                    return 2;
                if (Action.Direction == CSDirection.Left || Action.Direction == CSDirection.Right)
                {
                    if (Action.Motion == CSMotion.Attack || Action.Motion == CSMotion.Attack2)
                    {
                        return 2;
                    }
                }
                return CSMisc.BackRight[direction];
        }
        return 0;
    }

    public bool GetLoop(int p)
    {
        CSOrgan part = GetOrgan(p);

        if (part != null)
        {
            return part.getLoop();
        }
        return false;
    }

    public CSOrgan GetOrgan(int s)
    {
        if (parts == null) return null;

        for (int i = 0; i < parts.Length; i++)
        {
            CSOrgan part = parts[i];

            if (part != null && part.Structure == s)
            {
                return part;
            }
        }
        return null;
    }

    public int GetDirection()
    {
        return mAction.Direction;
    }

    public int GetBearing(int structure)
    {
        switch (structure)
        {
            case ModelStructure.Body:
                return ModelBearing.Body;
            case ModelStructure.Bottom:
                return ModelBearing.Bottom;
            case ModelStructure.BottomNPC:
                return ModelBearing.BottomNPC;
            case ModelStructure.Effect:
                return ModelBearing.Front;
            case ModelStructure.Shadow:
                return ModelBearing.Foot;
            case ModelStructure.Structure:
                return ModelBearing.Body;
            case ModelStructure.Weapon:
                return ModelBearing.HandRight;
            case ModelStructure.Wing:
                return ModelBearing.Back;
            default:
                return ModelBearing.Body;
        }
    }

    public bool EndOfAction()
    {
        if (Body != null)
        {
            return Body.getMediaStop();
        }
        return false;
    }

    public float AttackActionTime()
    {
        if (Body != null)
        {
            return Body.getMediaTime();
        }
        return 0f;
    }

    private void DispatchDirectionChange()
    {
        if (organData != null && organData.avatarType == EAvatarType.MainPlayer)
        {
            CSGame.MainEventHandler.SendEvent(MainEvent.MainPlayer_DirectionChange);
        }
    }

    public void Show(bool value)
    {
        GameObject sprite = Body != null ? Body.Go : null;
        GameObject sprite2 = Weapon != null ? Weapon.Go : null;
        GameObject sprite3 = Wing != null ? Wing.Go : null;
        GameObject sprite4 = Effect != null ? Effect.Go : null;
        if (sprite != null && sprite.activeSelf != value) sprite.SetActive(value);
        if (sprite2 != null && sprite2.activeSelf != value) sprite2.SetActive(value);
        if (sprite3 != null && sprite3.activeSelf != value) sprite3.SetActive(value);
        if (sprite4 != null && sprite4.activeSelf != value) sprite4.SetActive(value);
    }

    /// <summary>
    /// 是否在攻击中
    /// </summary>
    /// <returns></returns>
    public bool IsAttackPlaying()
    {
        if (mAction == null) return false;

        if (mAction.Motion == CSMotion.Attack || mAction.Motion == CSMotion.Attack2)
        {
            if (Time.time - lastMotionBeginTime > 1)
            {
                return false;
            }
            return EndOfAction();
        }
        return false;
    }

    public bool IsAttackMotion()
    {
        if (mAction == null) return false;

        return (mAction.Motion == CSMotion.Attack || mAction.Motion == CSMotion.Attack2);
    }
}

