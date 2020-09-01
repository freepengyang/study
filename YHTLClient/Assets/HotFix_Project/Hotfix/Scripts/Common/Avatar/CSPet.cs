using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPet : CSAvatarBase<CSPetInfo>
{
    public TABLE.MONSTERINFO mTblMonsterInfo;
    private bool mIsSelfPet;
    private CSSceneEffect effect6022;
    private bool isActivePet = false;
    private CSSceneEffect effect6023;
    private const int ActiveEffect6023 = 6023;
    private const int ActiveEffect6024 = 6024;
    private const int PermanentEffect6022 = 6022;

    public bool IsSelfPet
    {
        get
        {
            return mIsSelfPet;
        }
    }

    public override void Init(object data, Transform transAnchor)
    {
        map.RoundPet info = data as map.RoundPet;
        if (info == null) return;
        base.Init(data, transAnchor);
        InitPetInfo(info);
        InitBehaviour();
        InitModelLoad();
        InitModel();
        InitSkillEngine();
        InitEvent();
        ResetPosition(Info.Coord);
        SetStepTime(info.speed);
        int modelHeight = (mTblMonsterInfo != null ? mTblMonsterInfo.height : 0);
        bool isDead = Info.HP <= 0;
        Initialize(true, modelHeight, isDead, mIsSelfPet);
    }

    public void InitPetInfo(map.RoundPet info)
    {
        if (Info == null)
        {
            Info = new CSPetInfo();
        }
        AvatarType = EAvatarType.Pet;
        Info.Init(info);
        BaseInfo = Info;
        mIsSelfPet = (CSMainPlayerInfo.Instance.ID == info.masterId);
    }

    public void InitBehaviour()
    {
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
    }

    public void InitModelLoad()
    {
        if(ModelLoad == null)
        {
            ModelLoad = new PetModelLoad();
        }
    }

    public void InitEvent()
    {
        BaseInfo.EventHandler.AddEvent(CEvent.HP_Change, OnHpChange);
        BaseInfo.EventHandler.AddEvent(CEvent.Pet_StateChange, OnStateChange);
        BaseInfo.EventHandler.AddEvent(CEvent.ActiveShield, OnActiveShield);
        BaseInfo.EventHandler.AddEvent(CEvent.ShieldChange,OnInitShieldEffect);
    }

    private void OnInitShieldEffect(uint id, object data)
    {
        if (data is bool isShield)
        {
            InitShieldEffect(isShield);
        }
    }
    
    /// <summary>
    /// 护盾常驻特效
    /// </summary>
    public void InitShieldEffect(bool isShield)
    {
        if (Info.HasShield && !isActivePet)
        {
            //Debug.Log("--------------护盾常驻@宠物");
            if (effect6022 == null)
            {
                Transform anchor = Model.Effect.GoTrans;
                effect6022 = CSSceneEffectMgr.Instance.Create(anchor, PermanentEffect6022, Vector3.zero);
            }
            effect6022.SetAvtive(isShield);
        }
    }
    
    /// <summary>
    /// 护盾激活
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void OnActiveShield(uint id, object data)
    {
        isActivePet = true;
        Transform anchor = Model.Effect.GoTrans;
        if (effect6023 == null)
        {
            effect6023 = CSSceneEffectMgr.Instance.Create(anchor, ActiveEffect6023, Vector3.zero);
            CSSceneEffectMgr.Instance.PlayEffect(anchor, ActiveEffect6024);
            effect6023.SetPlayFinishedCallBack(() =>
            {
                if (effect6022==null)
                    effect6022 = CSSceneEffectMgr.Instance.Create(anchor, PermanentEffect6022, Vector3.zero);
                isActivePet = false;
            });
        }
    }

    public override void InitModel()
    {
        base.InitModel();

        InitBox();
        
        if (MonsterInfoTableManager.Instance.TryGetValue(BaseInfo.BodyModel, out mTblMonsterInfo))
        {
            mSpeed = (float)(CSCell.Size.x / ((float)mTblMonsterInfo.moveInterval / (float)1000));
            BaseInfo.Quality = mTblMonsterInfo.quality;
            AvatarType = (mTblMonsterInfo.type == 4) ? EAvatarType.ZhanHun : AvatarType;
            BaseInfo.AvatarType = AvatarType;
        }
        else
        {
            FNDebug.Log("======> BaseInfo.BodyModel = " + BaseInfo.BodyModel + " is null");
        }
        if (Model == null) Model = new CSModel(CacheRootTransform, box, AvatarType, IsDataSplit, mShaderType, replaceEquip);
        if (mTblMonsterInfo != null)
        {
            mModel.InitAnimationFPS(mTblMonsterInfo.model);
            int direction = mTblMonsterInfo.initDirection != (int)(CSDirection.None) ?
                (int)mTblMonsterInfo.initDirection : UnityEngine.Random.Range(0, 8);
            Model.SetDirection(direction);
            SetAction(CSMotion.Stand);
        }
    }

    public override void Dead()
    {
        CSSceneEffectMgr.Instance.Destroy(effect6022);
        effect6022?.Destroy();
        effect6022 = null;
        base.Dead();
    }

    public override void InitAvatarGo()
    {
        base.InitAvatarGo();
        if (Go != null)
        {
            if (AvatarGo == null)
            {
                AvatarGo = Go.AddComponent<CSPetGo>();
            }
            AvatarGo.Init(this);
            //IsReplaceEquip = true;
            bool isShow =(AvatarType == EAvatarType.Pet) ? (!CSConfigInfo.Instance.GetBool(ConfigOption.HideTaoistMonster))
                : (!CSConfigInfo.Instance.GetBool(ConfigOption.HideWarPet));
            Show(isShow);
            IsLoad = true;
            ReplaceEquip();
        }
    }

    public override void InitHead()
    {
        if (head == null)
        {
            CSResourceManager.Singleton.AddQueue("actor_pet",
                ResourceType.ResourceRes, OnLoadHead, ResourceAssistType.ForceLoad);
        }
        else
        {
            ShowHead();
        }
    }

    public override void ReplaceEquip()
    {
        if ((!IsLoad) || (!isInView))
        {
            return;
        }
        int bodyID = 0;
        int weaponID = 0;
        ModelAnalyze.GetPetBody(this.mTblMonsterInfo,Info.State,AvatarType,ref bodyID,ref weaponID);
        ModelLoad.eAvatarType = AvatarType;
        ModelLoad.UpdateModel(mModel.Action, bodyID, weaponID, SetModelAtlas);
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
        if(IsSelfPet)
        {
            return;
        }
        CSScene.Sington.CheckAvatarInView(this);
    }

    public override void ResetPosition(CSMisc.Dot2 coord)
    {
        base.ResetPosition(coord);
        BaseInfo.Coord = coord;
        ResetServerCell(coord.x, coord.y);
        ResetOldCell(coord.x, coord.y);
        NewCell = OldCell;
        SetPosition(NewCell.WorldPosition);
        OnOldCellChange();
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
        head = go.AddComponent<CSHeadPet>();
        ShowHead();
    }

    private void ShowHead()
    {
        if(head != null)
        {
            bool isHideModel = (AvatarType == EAvatarType.Pet) ? 
                CSConfigInfo.Instance.GetBool(ConfigOption.HideTaoistMonster) : CSConfigInfo.Instance.GetBool(ConfigOption.HideWarPet);
            bool isHideAllName = CSConfigInfo.Instance.GetBool(ConfigOption.HideAllName);
            head.Init(this,true,isHideModel, isHideAllName);
        }
    }

    private void OnStateChange(uint evetId, object obj)
    {
        IsReplaceEquip = true;
        if (Info.State == EPetChangeState.Fight)
        {
            RunToStand();
        }
        else
        {
            StandToRun();
        }
    }

    public override void Destroy()
    {
        CSSceneEffectMgr.Instance.Destroy(effect6022);
        CSSceneEffectMgr.Instance.Destroy(effect6023);
        effect6022?.Destroy();
        effect6022 = null;
        effect6023?.Destroy();
        effect6023 = null;
        base.Destroy();
    }

    public override void Release()
    {
        effect6022?.Release();
        effect6022 = null;
        effect6023?.Release();
        effect6023 = null;
        base.Release();
    }
}
