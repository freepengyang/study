using Google.Protobuf;
using UnityEngine;

public class CSMapNpcInfo : CSMapAvatarInfo
{
    public override MapAvaterType AvatarType => MapAvaterType.Npc;

    

    protected override void Init(IMessage message)
    {
        map.RoundNPC info = message as map.RoundNPC;
        if(info == null) return;
        ID = info.npcId;
        AvatarName = info.name;
        mDotX = info.x;
        mDotY = info.y;
    }

    public override void Show()
    {
        base.Show();
    }
    
    public override void RefreshUI()
    {
        base.RefreshUI();
        if (mMapPoint != null)
        {
            mMapPoint.SetUIInfo(UIMiniMapType.NPC, AvatarName);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}