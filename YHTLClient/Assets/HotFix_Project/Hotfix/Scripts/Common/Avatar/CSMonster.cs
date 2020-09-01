

//-------------------------------------------------------------------------
//怪物
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TABLE;

public class CSMonster : CSAvatarBase<CSMonsterInfo>
{
    public TABLE.MONSTERINFO tblMonsterInfo;

    public bool IsWall()
    {
        return (tblMonsterInfo != null && tblMonsterInfo.type == EMonsterType.Wall) ;
    }

    public bool IsPlayAttackAction()
    {
        return (tblMonsterInfo != null && tblMonsterInfo.noAttack == 0);
    }

    public CSMonster()
    {
    }

    public override void Init(object data, Transform transAnchor)
    {
        map.RoundMonster info = data as map.RoundMonster;
        if (info == null) return;
        base.Init(info, transAnchor);
        if (ModelLoad == null) ModelLoad = new MonsterModelLoad();
        InitMonsterInfo(info);
        InitSkillEngine();
        InitEvent();
        SetStepTime(info.speed);
        int modelHeight = (tblMonsterInfo != null ? tblMonsterInfo.height : 0);
        bool isDead = Info.HP <= 0;
        Initialize(true, modelHeight, isDead,false);
    }

    private void InitMonsterInfo(map.RoundMonster info)
    {
        if (Info == null)
        {
            Info = new CSMonsterInfo();
        }
        AvatarType = EAvatarType.Monster;
        Info.Init(info);
        BaseInfo = Info;
        if (mBehaviour == null)
        {
            mBehaviour = new CSMonsterBehavior(this);
        }
        if (mFSM == null)
        {
            mFSM = new FSMState();
        }
        mBehaviour.InitializeFSM(mFSM);
        mFSM.Start(CSMotion.Stand);
        InitModel();
        ResetPosition(Info.Coord);
    }

    public override void InitModel()
    {
        base.InitModel();
        InitBox();
        if (Model == null) Model = new CSModel(CacheRootTransform,box,AvatarType,IsDataSplit,mShaderType, replaceEquip);
        
        if (MonsterInfoTableManager.Instance.TryGetValue(BaseInfo.BodyModel, out tblMonsterInfo))
        {
            if(tblMonsterInfo != null)
            {
                mSpeed = (float) (CSCell.Size.x / ((float) tblMonsterInfo.moveInterval / (float) 1000));
                BaseInfo.Quality = tblMonsterInfo.quality;
                if(tblMonsterInfo.type == EMonsterType.Wall)
                {
                    Node node = CSMesh.Instance.getNode(Info.Coord);
                    if(node != null)
                    {
                        Node.isHaveNpcWall = true;
                    }
                }
                mModel.InitAnimationFPS(tblMonsterInfo.model);
                int direction = tblMonsterInfo.initDirection != CSDirection.None
                    ? (int) tblMonsterInfo.initDirection
                    : UnityEngine.Random.Range(0, 8);
                Model.SetDirection(direction);
                SetAction(CSMotion.Stand);
            }
        }
        else
        {
            FNDebug.Log("======> BaseInfo.BodyModel = " + BaseInfo.BodyModel + " is null");
        }
    }

    public override void InitAvatarGo()
    {
        base.InitAvatarGo();
        if(Go != null)
        {
            if(AvatarGo == null)
            {
                AvatarGo = Go.AddComponent<CSMonsterGo>();
            }
            AvatarGo.Init(this);
            bool isShow = !CSConfigInfo.Instance.GetBool(ConfigOption.HideMonsters);
            Show(isShow);
            //IsReplaceEquip = true;
            IsLoad = true;
            ReplaceEquip();
        }
    }

    public override void InitHead()
    {
        if (head == null)
        {
            //TODO:ddn
            CSResourceManager.Singleton.AddQueue("actor_monster",
                ResourceType.ResourceRes, OnLoadHead, ResourceAssistType.ForceLoad);
        }
        else
        {
            ShowHead();
        }
    }

    public void InitBottom()
    {
        if(tblMonsterInfo != null && tblMonsterInfo.bottomMark > 0 && Model.Effect != null)
        {
            CSAvatarEffectManager.Instance.Show(Model.Effect.GoTrans, ID, tblMonsterInfo.bottomMark);

        }
    }

    public void RemoveBottom()
    {
        CSAvatarEffectManager.Instance.Remove(ID); ;
    }

    public void InitEvent()
    {
        BaseInfo.EventHandler.AddEvent(CEvent.HP_Change, OnHpChange);
    }

    public override void ReplaceEquip()
    {
        if ((!IsLoad) || (!isInView))
        {
            return;
        }
        if (CSConfigInfo.Instance.GetBool(ConfigOption.HideMonsters))
        {
            return;
        }
        //TODO:ddn
        TABLE.MONSTERINFO table;
        if (MonsterInfoTableManager.Instance.TryGetValue((int)Info.ConfigId, out table))
        {
            ModelLoad.UpdateModel(mModel.Action, table.model, SetModelAtlas);
        }
    }

    public override void MoveInit()
    {
        PathData.PathArray = GetPath();

        base.MoveInit();

        MoaveInItBase1();
    }

    protected override void UpdatePosition()
    {
        base.UpdatePosition();

        UpdatePosition2();
    }

    protected override void UpdateViewModel()
    {
        if(CSConfigInfo.Instance.GetBool(ConfigOption.HideMonsters))
        {
            return;
        }
        CSScene.Sington.CheckAvatarInView(this);
    }

    public override void Show(bool isShow)
    {
        if (tblMonsterInfo == null || mModel == null)
        {
            return;
        }
        if (tblMonsterInfo.showType > 1)
        {
            GameObject sprite = mModel.Body != null ? mModel.Body.Go : null;
            GameObject sprite2 = mModel.Weapon != null ? mModel.Weapon.Go : null;
            GameObject sprite3 = mModel.Wing != null ? mModel.Wing.Go : null;
            if (sprite != null && sprite.activeSelf != true) sprite.SetActive(true);
            if (sprite2 != null && sprite2.activeSelf != true) sprite2.SetActive(true);
            if (sprite3 != null && sprite3.activeSelf != true) sprite3.SetActive(true);
        }
        else
        {
            GameObject sprite = mModel.Body != null ? mModel.Body.Go : null;
            GameObject sprite2 = mModel.Weapon != null ? mModel.Weapon.Go : null;
            GameObject sprite3 = mModel.Wing != null ? mModel.Wing.Go : null;
            if (sprite != null && sprite.activeSelf != isShow) sprite.SetActive(isShow);
            if (sprite2 != null && sprite2.activeSelf != isShow) sprite2.SetActive(isShow);
            if (sprite3 != null && sprite3.activeSelf != isShow) sprite3.SetActive(isShow);
        }
    }

    public override void ResetPosition(CSMisc.Dot2 coord)
    {
        mNextNode = null;
        base.ResetPosition(coord);
        BaseInfo.Coord = coord;
        ResetServerCell(coord.x,coord.y);
        ResetOldCell(coord.x, coord.y);
        NewCell = OldCell;

        SetPosition(NewCell.WorldPosition);
        OnOldCellChange();

    }

    public override void OnOldCellChange()
    {
        base.OnOldCellChange();

        if(IsWall())
        {
            if (OldCell != null)
            {
                ResetDepth(OldCell.Coord.x, OldCell.Coord.y-2);
            }
        }
    }

    public override bool IsCanBeSelectAttack()
    {
        if(IsServerDead || IsDead || !IsLoad)
        {
            return false;
        }
        return true;
    }


    public void ResetNpcMonster(bool isShowShadow, bool isReset)
    {
        if(tblMonsterInfo != null && tblMonsterInfo.type == EMonsterType.Wall)
        {
            if (mModel != null && mModel.Body != null)
            {
                mModel.Body.UpdateShoadow(isShowShadow);
            }
            if(isReset)
            {
                Node.isHaveNpcWall = false;
            }
        }
    }

    public override void OnHpChange(uint evtId, object obj)
    {
        if(head != null)
        {
            head.SetActive(true);
        }
        if (BaseInfo.HP <= 0)
        {
            Dead(); 
        }
        IsServerDead = (BaseInfo.HP <= 0);
    }

    public void OnLoadHead(CSResource res)
    {
        if (res == null || res.MirrorObj == null)
        {
            return;
        }
        GameObject go = res.GetObjInst() as GameObject;
        if (go == null)
        {
            return;
        }
        head = go.AddComponent<CSHeadMonster>();
        ShowHead();
    }

    private void ShowHead()
    {
        if(head != null)
        {
            bool isHideModel = CSConfigInfo.Instance.GetBool(ConfigOption.HideMonsters);
            bool isHideAllName = CSConfigInfo.Instance.GetBool(ConfigOption.HideAllName);
            head.Init(this,false, isHideModel, isHideAllName);
        }
    }

    public override void Release()
    {
        RemoveBottom();
        ResetNpcMonster(true,true);
        base.Release();
    }

    public override void Destroy()
    {
        //CSScene.RemoveView(this);
        RemoveBottom();
        ResetNpcMonster(true,true);
        base.Destroy();
    }
}
    
