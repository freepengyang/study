using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSMonsterInfo : CSAvatarInfo
{
    public void Init(map.RoundMonster monster)
    {
        if(monster == null)
        {
            return;
        }
        ID = monster.monsterId;
        ConfigId = monster.monsterConfigId;
        Name = monster.monsterName;
        Level = monster.level;
        HP = monster.hp;
        RealHP = monster.hp;
        MaxHP = monster.maxHp;
        Coord = new CSMisc.Dot2(monster.x, monster.y);
        Speed = monster.speed;
        BodyModel = (int)ConfigId;
        AvatarType = EAvatarType.Monster;
        MonsterOwner = monster.owner;
        Quality = 0;
        BuffInfo.Init(monster.buffers);
    }

    public override int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if(hp != value)
            {
                hp = value;
                EventHandler.SendEvent(CEvent.HP_Change);
            }
        }
    }

    public override CSBuffInfo BuffInfo
    {
        get
        {
            if(base.BuffInfo == null)
            {
                base.BuffInfo = new CSBuffInfo();
            }
            return base.BuffInfo;
        }
        set
        {
            base.BuffInfo = value;
        }
    }

    public override float PublicCDTime
    {
        get { return 1.0f; }
        set { base.PublicCDTime = value; }
    }

    private string mMonsterOwner;
    public string MonsterOwner
    {
        get
        {
            return mMonsterOwner;
        }
        set
        {
            if (!String.Equals(mMonsterOwner,value))
            {
                mMonsterOwner = value;
            }
        }
    }

}
