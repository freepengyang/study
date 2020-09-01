using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPlayer : CSAvatarBase<CSPlayerInfo>
{
    private CSSceneEffect effect6023;
    private const int ActiveEffect6023 = 6023;
    private const int ActiveEffect6024 = 6024;
    private const int PermanentEffect6022 = 6022;
    private CSSceneEffect effect6022;
    private bool isActivePet = false;

    public override void Init(object data, Transform transAnchor)
    {
        map.RoundPlayer roundPlayer = data as map.RoundPlayer;
        if(roundPlayer == null)
        {
            return;
        }
        base.Init(data, transAnchor);

        if (Info == null)
        {
            Info = new CSPlayerInfo(roundPlayer.player);
        }
        else
        {
            Info.UpdateRoleBrief(roundPlayer.player);
        }
        BaseInfo = base.Info;

        AvatarType = EAvatarType.Player;

        if (ModelLoad == null) ModelLoad = new PlayerModelLoad();

        if (mFSM == null) mFSM = new FSMState();
  
        if (mBehaviour == null) mBehaviour = new CSPlayerBehavior(this);
 
        mBehaviour.InitializeFSM(mFSM);
        mFSM.Start(CSMotion.Stand);
        InitModel();
        ResetPosition(BaseInfo.Coord);
        InitSkillEngine();
        InitAvatarGo();
        InitEvent();
        SetStepTime(roundPlayer.player.speed);
        bool isDead = Info.HP <= 0;
        Initialize(true,0, isDead,false);
    }

    public override void InitModel()
    {
        base.InitModel();
        InitBox();
        if (Model == null) Model = new CSModel(CacheRootTransform,box,AvatarType,IsDataSplit,mShaderType,replaceEquip);
        SetAction(CSMotion.Stand);
        int dir = Random.Range(0, 7);
        Model.SetDirection(dir);
        Model.InitAnimationFPS(Info.BodyModel);
    }
    
    public override void InitAvatarGo()
    {
        if (Go != null)
        {
            base.InitAvatarGo();
            if (AvatarGo == null)
            {
                AvatarGo = Go.AddComponent<CSPlayerGo>();
            }
            AvatarGo.Init(this);
            bool isShow = !CSConfigInfo.Instance.IsHidePlayer(Info.GuildId);
            Show(isShow);
            IsLoad = true;
        }
    }

    public override void InitHead()
    {
        if (head == null)
        {
            //TODO:ddn
            CSResourceManager.Singleton.AddQueue("actor_player",
                ResourceType.ResourceRes, OnLoadHead, ResourceAssistType.ForceLoad);
        }
        else
        {
            ShowHead();
        }
    }

    public void InitEvent()
    {
        ClientHanlderManager clientEventHandler = BaseInfo.EventHandler;
        clientEventHandler.AddEvent(CEvent.HP_Change, OnHpChange);
        clientEventHandler.AddEvent(CEvent.Player_Relive, OnRelive);
        clientEventHandler.AddEvent(CEvent.Player_ReplaceEquip, OnReplaceEquip);
        clientEventHandler.AddEvent(CEvent.ActiveShield, OnActiveShield);
        clientEventHandler.AddEvent(CEvent.ShieldChange,OnInitShieldEffect);
    }
    
    /// <summary>
    /// 护盾常驻特效
    /// </summary>
    public void InitShieldEffect(bool isShield)
    {
        if (Info.HasShield && !isActivePet)
        {
            if (effect6022 == null)
            {
                Transform anchor = Model.Effect.GoTrans;
                effect6022 = CSSceneEffectMgr.Instance.Create(anchor, PermanentEffect6022, Vector3.zero);
            }
            effect6022.SetAvtive(isShield);
        }
    }

    public override bool IsCanBeSelectAttack()
    {
        if (IsServerDead || IsDead || !IsLoad)
        {
            return false;
        }
        return true;
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

    public override void Dead()
    {
        CSSceneEffectMgr.Instance.Destroy(effect6022);
        effect6022?.Destroy();
        if (effect6022!=null)
            effect6022 = null;
        base.Dead();
    }

    public override void ReplaceEquip()
    {
        if ((!IsLoad) || (!isInView))
        {
            return;
        }
        if(CSConfigInfo.Instance.IsHidePlayer(Info.GuildId))
        {
            return;
        }

        int bodyModel = Utility.GetBodyModel(Info.BodyModel, Info.FashionCloth, Info.Sex, Info.Career);
        int weaponModel = Utility.GetWeaponModel(Info.Weapon, Info.FashionWeapon, Info.Sex, Info.Career, Info.AvatarType);
        int wingID = Utility.GetWingModelId(Info.WingID);
        ModelLoad.eAvatarType = AvatarType;
        ModelLoad.UpdateModel(mModel.Action, bodyModel, weaponModel, wingID, SetModelAtlas);
    }

    public override void ResetPosition(CSMisc.Dot2 coord)
    {
        BaseInfo.Coord = coord;
        ResetServerCell(coord.x, coord.y);
        ResetOldCell(coord.x, coord.y);
        NewCell = OldCell;
        SetPosition(NewCell.WorldPosition);
        OnOldCellChange();
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
        if (CSConfigInfo.Instance.IsHidePlayer(Info.GuildId))
        {
            return;
        }
        //TODO:
        CSScene.Sington.CheckAvatarInView(this);
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
        head = go.AddComponent<CSHeadPlayer>();
        ShowHead();
    }

    public void ShowHead()
    {
        if(head != null && Info != null)
        {
            bool isHideModel = CSConfigInfo.Instance.IsHidePlayer(Info.GuildId);
            bool isHideAllName = CSConfigInfo.Instance.GetBool(ConfigOption.HideAllName);
            head.Init(this, true,isHideModel, isHideAllName);
        }
    }

    public void OnRelive(uint evtId, object obj)
    {
        SetAction(CSMotion.Stand);
        if (head != null) head.Show();
    }
    
    public void OnReplaceEquip(uint evtId, object obj)
    {
        IsReplaceEquip = true;
    }
    
    private void OnInitShieldEffect(uint id, object data)
    {
        if (data is bool isShield )
        {
            InitShieldEffect(isShield);
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



