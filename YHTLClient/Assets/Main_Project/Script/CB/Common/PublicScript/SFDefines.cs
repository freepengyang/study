
using UnityEngine;
using System.Collections;

public class ResourceType
{
    public const int None = 0;
    public const int PlayerAtlas = 1;
    public const int MonsterAtlas = 2;
    public const int NpcAtlas = 3;
    public const int WeaponAtlas = 4;
    public const int MountAtlas = 5;
    public const int Effect = 6;
    public const int Skill = 7;
    public const int SkillAtlas = 8;
    public const int TableBytes = 9;
    public const int Map = 10;
    public const int MapBytes = 11;
    public const int MiniMap = 12;
    public const int WingAtlas = 13;
    public const int Audio = 14;
    public const int UIMount = 15;
    public const int ScaleMap = 16;
    public const int UIEffect = 17;
    public const int UIWing = 18;
    public const int UIPlayer = 19;
    public const int UIWeapon = 20;
    public const int ResourceRes = 22;
    public const int UITexture = 23;
    public const int UIMonster = 24;
}
public class ResourceAssistType //数字越大，优先级越高
{
    public const int None = 0;
    public const int OtherPet = 1; //宠物和法神
    public const int Player = 2; //其他玩家
    public const int Monster = 3; //怪物
    public const int Terrain = 4; //地形
    public const int NPC = 5; //NPC
    public const int CharactarPet = 6;
    public const int Charactar = 7; //较地形之前
    public const int UI = 8; //UI
    public const int QueueDeal = 9; //一个加载完，才加载下一次
    public const int ForceLoad = 10; //不管当前正在加载的是什么，都强制抛出加载
}

public class EAvatarType
{
    public const int None = 0;
    public const int MainPlayer = 1;
    public const int Player = 2;
    public const int Monster = 3;
    public const int NPC = 4;
    public const int Pet = 5;
    public const int Guard = 6;
    public const int Trigger = 7;
    public const int Grave = 8;
    public const int RoleMonster = 9;
    public const int ZhanHun = 10;
    public const int Item = 11;
    public const int Extend_1 = 12;
    public const int Extend_2 = 13;
    public const int Extend_3 = 14;
    public const int Extend_4 = 15;
    public const int Extend_5 = 16;
    public const int Extend_6 = 17;
    public const int Extend_7 = 18;
    public const int Extend_8 = 19;
    public const int Extend_9 = 20;
    public const int Extend_10 = 21;
}

/// <summary>
/// 怪物死亡和角色展示 是一个动画
/// </summary>
public class CSMotion 
{
    public const int Static = 0;         // 无
    public const int Stand = 1;          // 待机
    public const int Walk = 2;           // 走路
    public const int Attack = 3;         // 攻击
    public const int Attack2 = 4;        // 法攻击
    public const int Attack3 = 5;        // 弓手
    public const int BeAttack = 6;       // 被击
    public const int Dead = 7;           // 死亡
    public const int Mining = 8;         // 挖矿
    public const int ShowStand = 9;      // 展示
    public const int Run = 10;            // 跑步
    public const int RunToStand = 11;     // 跑步到待机
    public const int StandToRun = 12;     //待机到跑步
    public const int WaKuang = 13;        //挖矿
    public const int GuWu = 14;           //鼓舞
    public const int RunOverDoSmoething = 15;//跑过去做一些东西
}

/// <summary>
/// 编程者自己左右上下
/// </summary>
public class CSDirection //移动方向
{
    public const int Up = 0;                 // 上    0
    public const int Right_Up = 1;          // 右上   1
    public const int Right = 2;              // 右    2
    public const int Right_Down = 3;         // 右下   3
    public const int Down = 4;               // 下     4
    public const int Left_Down = 5;          // 左下   5
    public const int Left = 6;               // 左    6
    public const int Left_Up = 7;            // 左上   7
    public const int None = 8;
}

public class ModelBearing
{
    public const int Head = 0;
    public const int Body = 1;
    public const int UnderFoot = 2;
    public const int Hand = 3;
    public const int HandLeft = 4;
    public const int HandRight = 5;
    public const int Foot = 6;
    public const int FootLeft = 7;
    public const int FootRight = 8;
    public const int Bottom = 9;
    public const int BottomNPC = 10;
    public const int Front = 11;
    public const int Around = 12;
    public const int Back = 13;
}
/// <summary>
/// 结构
/// </summary>
public class ModelStructure
{
    public const int Structure = 0;
    public const int Shadow = 1;
    public const int Body = 2;
    public const int Weapon = 3;
    public const int Bottom = 4;
    public const int BottomNPC = 5;
    public const int Effect = 6;
    public const int Wing = 7;
    public const int Mount = 8;
    public const int MountHead = 9;
    public const int MountSaddle = 10;
}

public class EActionStopFrameType
{
    public const int None = 0;
    public const int First = 1;
    public const int End = 2;
    public const int LastFrame = 3;
}

public enum ELogToggleType
{
    NormalMSG,
    FrequencyMSG,
    Exception,
    NormalLog,
}

public enum ELogColorType
{
    White,
    Yellow,
    Red,
    Green,
}

public enum PlatformType
{
    EDITOR,
    ANDROID,
    IOS,
}

public enum DownloadUIType
{
    CheckUpdate,
    Download,
}
