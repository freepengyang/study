using Google.Protobuf;
using TABLE;
using UnityEngine;

public class CSMapMonsterInfo : CSMapAvatarInfo
{
    public override MapAvaterType AvatarType
    {
        get { return MapAvaterType.Monster; }
    }

    private MONSTERINFO _monsterinfo;

    public MONSTERINFO Monsterinfo
    {
        get { return _monsterinfo; }
        set { _monsterinfo = value; }
    }

    public int monsterSpeed;

    public int monsterConfigId;

    protected override void Init(IMessage message)
    {
        map.RoundMonster info = message as map.RoundMonster;
        if (info == null) return;
        monsterConfigId = info.monsterConfigId;
        MonsterInfoTableManager.Instance.TryGetValue(monsterConfigId, out _monsterinfo);
        ID = info.monsterId;
        mDotX = info.x;
        mDotY = info.y;
        AvatarName = info.monsterName;
        
        monsterSpeed = info.speed;
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
                monsterType = _monsterinfo.type == 2 || _monsterinfo.type == 5
                    ? (int) UIMiniMapMonsterType.Boss
                    : (int) UIMiniMapMonsterType.Normal;
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