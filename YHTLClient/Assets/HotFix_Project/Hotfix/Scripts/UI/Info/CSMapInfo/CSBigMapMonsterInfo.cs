using Google.Protobuf;
using TABLE;
using UnityEngine;

public class CSBigMapMonsterInfo : CSMapAvatarInfo
{
    public override MapAvaterType AvatarType => MapAvaterType.MapMonster;
    
    private MONSTERINFO _monsterinfo;

    public MONSTERINFO Monsterinfo
    {
        get => _monsterinfo;
        set => _monsterinfo = value;
    }

    public int monsterSpeed;

    public int monsterConfigId;

    protected override void Init(IMessage message)
    {
        map.RoundMonster info = message as map.RoundMonster;
        if(info == null) return;
        monsterConfigId = info.monsterConfigId;
        MonsterInfoTableManager.Instance.TryGetValue(monsterConfigId, out _monsterinfo);
        ID = info.monsterId;
        AvatarName = info.monsterName;
        mDotX = info.x;
        mDotY = info.y;
        monsterSpeed = info.speed;
    }
    
    protected override void Init(CSMapAvatarInfo info)
    {
        if(info == null) return;
        CSMapMonsterInfo monsterInfo = info as CSMapMonsterInfo;
        if(monsterInfo == null) return;
        monsterConfigId = monsterInfo.monsterConfigId;
        MonsterInfoTableManager.Instance.TryGetValue(monsterConfigId, out _monsterinfo);
        ID = info.ID;
        mDotX = info.mDotX;
        mDotY = info.mDotY;
        AvatarName = info.AvatarName;
        monsterSpeed = monsterInfo.monsterSpeed;
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
            mMapPoint.SetSpeed(monsterSpeed);
            int monsterType = 1;
            if (_monsterinfo != null)
            {
                monsterType = _monsterinfo.type == 2 || _monsterinfo.type == 5 ? (int)UIMiniMapMonsterType.Boss : (int)UIMiniMapMonsterType.Normal;
            }            
            mMapPoint.SetUIInfo(UIMiniMapType.Monster, AvatarName, monsterType);
        }
    }


    public override void Dispose()
    {
        Monsterinfo = null;
        base.Dispose();
    }
}