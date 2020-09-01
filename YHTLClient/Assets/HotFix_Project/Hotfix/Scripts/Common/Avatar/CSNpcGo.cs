using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSNpcGo : CSAvatarGo
{
    public override void Init(CSAvatar avatar)
    {
        base.Init(avatar);
        Transform root = avatar.CacheRootTransform;
        if (root != null)
        {
            CSModelModule module = avatar.ModelModule;
            if (module != null)
            {
                Owner.Model.InitPart(module);
            }
            
            CSNpc npc = Owner as CSNpc;
            if (npc != null)
            {
                npc.CacheTransform.localPosition = npc.GetPosition();
                npc.InitHead();
                // npc.InitBottom();
            }
        }
    }
    
    public override void OnHit(CSAvatar clicker)
    {
        base.OnHit(clicker);
        // CSAvatarEffectManager.Instance.Show(Owner.Model.Effect.GoTrans, clicker.ID, 6034);
        CSModel clickerModel = clicker.Model;
        if(clickerModel != null && clickerModel.BottomNPC.Go != null)
        {
            Owner.Model.AttachBottomNPC(clickerModel.BottomNPC);
            clickerModel.ShowSelectAndHideOtherBottom(ModelStructure.BottomNPC);
            Owner.head.Show();
        }
        WaitOpenNpcFunc((int) Owner.BaseInfo.ConfigId, clicker.NewCell.Coord);
    }

    //npc坐标为0时，默认视野寻路，，此条件只能用于，npc只出现在视野内的寻路
    public static void OpenNpcFun(TABLE.NPC npc)
    {
        if(npc.bornX == 0 && npc.bornY == 0)
        {
            var avatarList = CSAvatarManager.Instance.GetAvatarList(EAvatarType.NPC);
            CSMisc.Dot2 dot2 = new CSMisc.Dot2();
            for (var i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i] != null && avatarList[i].BaseInfo.ConfigId == npc.id)
                {
                    dot2 = avatarList[i].NewCell.Coord;
                    break;
                }
            }
            CSScene.AddWaitDeal_Object_Insert(0, npc.id.ToString(), WaitOpenNpcFunc, 0.1f, dot2);
        }
        CSScene.AddWaitDeal_Object_Insert(0, npc.id.ToString(), WaitOpenNpcFunc, 0.1f);
    }

    /// <summary>
    /// 点击npc
    /// </summary>
    private static bool WaitOpenNpcFunc(object obj, object param)
    {
        int npcID = System.Convert.ToInt32(obj);
        UtilityPath.ReSetPath();
        if (!CSMissionManager.Instance.DoNpcMission(npcID))
        {
            if (NpcTableManager.Instance.TryGetValue(npcID, out TABLE.NPC tbl_npc))
            {
                if (!CSScene.IsLanuchMainPlayer) return true;

                if (tbl_npc.sceneId != 0 && tbl_npc.sceneId != CSScene.GetMapID()) return true;
                CSMisc.Dot2 dot2;
                if(param != null && param is CSMisc.Dot2)
                {
                    dot2 =  (CSMisc.Dot2) param;
                }else
                {
                    dot2.x = tbl_npc.bornX;
                    dot2.y = tbl_npc.bornY;
                }
                if (!Utility.IsNearPlayerInMap(dot2.x, dot2.y)) return true;

                if (!CSSpeNpcOperaMgr.DoSpecialNpc(npcID, tbl_npc.func))
                {
                    UIManager.Instance.CreatePanel<UINPCDialogPanel>((f) =>
                    {
                        CSNPCDialogData data = new CSNPCDialogData();
                        data.NpcId = npcID;
                        UINPCDialogPanel panel = f as UINPCDialogPanel;
                        panel?.Show(data);
                    });
                }
            }
        }
        return true;
    }
}