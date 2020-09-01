using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CSSkillEffectMultiBase<T> where T : CSSkillEffect, new()
{
    protected CSBetterList<T> mList = new CSBetterList<T>();
    protected CSBetterList<mPointData> mListPoint = new CSBetterList<mPointData>();
    protected AvatarUnit mAvatar { get; set; }
    protected int mType = 0;
    protected int mNum = 0;
    public struct mPointData
    {
        public int x;
        public int y;
        public int time;
        public int mlength;
    }

    public bool isPlaying
    {
        get
        {
            if(mList.Count <= 0)
            {
                return false;
            }
            return mList[0].IsPlaying;
        }
    }

    public bool isLoadingRes
    {
        get
        {
            return mList[0].mIsLoadingRes;
        }
        set
        {
            mList[0].IsLoadingRes = value;
        }
    }

    public bool IsTakeEffect
    {
        get
        {
            return mList[0].IsTakeEffect;
        }
    }

    public void Release()
    {
        mList.Reverse();
    }

    public void UpdateData(AvatarUnit target,AvatarUnit attack, int targetCoordX, int targetCoordY)
    {
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.UpdateData(target, attack,targetCoordX, targetCoordY);
        }
    }

    public virtual void Init(AvatarUnit avatar, AvatarUnit target,CSSkillEffectData effectData, Transform parent)
    {
        mList.Clear();
        mAvatar = avatar;
        T t = new T();
        t.Init(avatar, target, effectData, true, parent);
        t.speed = 4;
        mList.Add(t);
    }

    public void UpdateSkill(AvatarUnit avatar,AvatarUnit target, CSSkillEffectData effectData, Transform parent)
    {
        this.mAvatar = avatar;
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.Init(avatar, target, effectData, i == 0, parent);
        }
    }

    public void UpdateIAvater(AvatarUnit avater, AvatarUnit target)
    {
        this.mAvatar = avater;
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.UpdateIAvater(avater, target);
        }
    }

    public void RemoveAttach()
    {
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.RemoveAttach();
        }
    }

    public bool IsSameEffect(int effectID)
    {
        CSSkillEffect effect = GetSelfSkillEffect();
        if(effect == null)
        {
            return false;
        }
        if(effect.Info == null || effect.Info.ID != effectID)
        {
            return false;
        }
        return true;
    }

    public CSSkillEffect GetSelfSkillEffect()
    {
        if (mList.Count > 0) return mList[0];
        return null;
    }

    public virtual void Play(float time)
    {
        if (this == null || mAvatar == null) return;

        CSCell attackPosition = null;

        int curDirection = (int)mAvatar.GetDirection();
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            if(t == null || t.Info == null)
            {
                continue;
            }
            CSSkillEffectData effectData = t.Info;
            if(effectData.targetCoordX != 0 || effectData.targetCoordY != 0)
            {
                CSCell cell = CSMesh.Instance.getCell(effectData.targetCoordX, effectData.targetCoordY);
                attackPosition = cell;
                t.AttackTarget = null;
            }
            else
            {
                CSMisc.Dot2 d = mAvatar.OldCell.Coord;
                CSCell cell = CSMesh.Instance.getCell(d.x, d.y);
                attackPosition = (t.AttackTarget != null)? t.AttackTarget.OldCell : cell;
            }

            t.curDirection = curDirection;
            t.attackPosition = attackPosition;
            t.Play(time);
        }
    }

    public void LoadRes(Action<CSResource> onLoadCallBack)
    {
        mList[0].LoadResource(onLoadCallBack);
        if (mList[0].Info != null && mList[0].Info.Num > 0)
        {
            for (int i = 0; i < mList.Count; i++)
            {
                if (i == 0) continue;
                mList[i].LoadResource(null);
            }
        }
    }

    public virtual void Update()
    {
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.Update();
        }
    }

    public virtual void Stop()
    {
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            t.Stop();
        }
    }

    public virtual void Destroy()
    {
        for (int i = 0; i < mList.Count; i++)
        {
            T t = mList[i];
            if (t.entity != null)
            {
                UnityEngine.Object.Destroy(t.entity);
            }
        }
    }

    protected void AnalysisString(string str)
    {
        if (string.IsNullOrEmpty(str)) return;

        string[] s = str.Split(',');

        for (int i = 0; i < s.Length; i++)
        {
            string[] m = s[i].Split('#');
            mPointData d = new mPointData();
            if (m.Length > 0) int.TryParse(m[0], out d.time);
            if (m.Length > 1) int.TryParse(m[1], out d.x);
            if (m.Length > 2) int.TryParse(m[2], out d.y);
            if (m.Length > 3) int.TryParse(m[3], out d.mlength);
            mListPoint.Add(d);
        }
    }

    protected void AnalysisString(string str, ref int _type, ref int _num)
    {
        if (string.IsNullOrEmpty(str)) return;

        string[] m = str.Split('#');

        for (int i = 0; i < m.Length; i++)
        {
            if (m.Length > 0) int.TryParse(m[0], out _num);
            if (m.Length > 1) int.TryParse(m[1], out _type);
        }
    }
}
