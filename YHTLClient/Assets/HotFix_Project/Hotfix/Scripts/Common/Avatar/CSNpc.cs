using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSNpc : CSAvatarBase<CSNpcInfo>
{
    public TABLE.NPC tblNpc = null;
    public override bool IsCanBeSelectAttack()
    {
        return false;
    }

    public override void Init(object data, Transform transAnchor)
    {
        map.RoundNPC info = data as map.RoundNPC;
        if(info == null) return;
        base.Init(data, transAnchor);
        InitNpcInfo(info);
        InitHead();
    }

    private void InitNpcInfo(map.RoundNPC info)
    {
        if (Info == null) Info = new CSNpcInfo();
        if (ModelLoad == null) ModelLoad = new NpcModelLoad();
        AvatarType = EAvatarType.NPC;
        Info.Init(info);
        BaseInfo = Info;
        InitModel();
        ResetPosition(Info.Coord);
    }

    public override void InitModel()
    {
        base.InitModel();
        InitBox();
        if (Model == null)
        {
            Model = new CSModel(CacheRootTransform,box,AvatarType,IsDataSplit,mShaderType, replaceEquip);
        }

        if (NpcTableManager.Instance.TryGetValue(BaseInfo.BodyModel, out tblNpc))
        {
            mModel.InitAnimationFPS(tblNpc.model);
            Model.SetDirection(tblNpc.direction);
            SetAction(CSMotion.Stand);
        }
    }
    public override void InitAvatarGo()
    {
        base.InitAvatarGo();
        if (Go != null)
        {
            if (AvatarGo == null)
            {
                AvatarGo = Go.AddComponent<CSNpcGo>();
            }
            AvatarGo.Init(this);
            IsLoad = true;
            ReplaceEquip();
        }
    }

    public override void InitHead()
    {
        if (head == null)
        {
            CSResourceManager.Singleton.AddQueue("actor_npc",
                ResourceType.ResourceRes, OnLoadHead, ResourceAssistType.ForceLoad);
        }
        else
        {
            head.Init(this);
        }
    }

    public override void ReplaceEquip()
    {
        if ((!IsLoad) || (!isInView))
        {
            return;
        }

        TABLE.NPC tblNpc;
        if (NpcTableManager.Instance.TryGetValue((int)Info.ConfigId, out tblNpc))
        {
            ModelLoad.UpdateModel(mModel.Action,tblNpc.model, SetModelAtlas);
        }
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
        head = go.AddComponent<CSHeadNpc>();
        head.Init(this);
    }



    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Release()
    {
        base.Release();
    }
}
