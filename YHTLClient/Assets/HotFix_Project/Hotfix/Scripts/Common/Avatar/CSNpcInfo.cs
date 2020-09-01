using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSNpcInfo : CSAvatarInfo
{
    public int OlderFlag { get; set; }
    public void Init(map.RoundNPC npc)
    {
        if (npc == null)
        {
            return;
        }
        ID = npc.npcId;
        ConfigId = npc.npcConfigId;
        Name = npc.name;
        HP = npc.hp;
        MaxHP = npc.maxHp;
        Coord = new CSMisc.Dot2(npc.x, npc.y);
        BodyModel = (int)ConfigId;
        AvatarType = EAvatarType.NPC;
        OlderFlag = npc.olderFlag;
    }
}
