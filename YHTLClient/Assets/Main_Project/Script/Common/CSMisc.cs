
//------------------------------------------------------
//算法，公式，集合
//author jiabao
//time 2015.12.29
//------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class CSMisc 
{
    public struct Dot2
    {
        public int x, y;
        private static Dot2 mZero = new Dot2(0, 0);
        public static Dot2 Zero
        {
            get
            {
                return mZero;
            }
        }

        public Dot2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public void Clear()
        {
            x = y = 0;
        }

        public bool Equal(Dot2 dot)
        {
            return (dot.x == x && dot.y == y);
        }

        public bool Equal(int xx, int yy)
        {
            return (x == xx && y == yy);
        }

        public Dot2 Abs()
        {
            Dot2 d = new Dot2();
            d.x = (x > 0 ? x : -x);
            d.y = (y > 0 ? y : -y);
            return d;
        }

        public Dot2 Normal()
        {
            Dot2 d = new Dot2();
            d.x = NormalX();
            d.y = NormalY();
            return d;
        }

        public int NormalX()
        {
            return NomalInternal(x);
        }

        public int NormalY()
        {
            return NomalInternal(y);
        }

        int NomalInternal(int value)
        {
            if (value > 0) return 1;
            else if (value < 0) return -1;
            return 0;
        }

        public static Dot2 operator +(Dot2 f, Dot2 s)
        {
            Dot2 dot = new Dot2();
            dot.x = f.x + s.x;
            dot.y = f.y + s.y;
            return dot;
        }

        public static Dot2 operator -(Dot2 f, Dot2 s)
        {
            Dot2 dot = new Dot2();
            dot.x = f.x - s.x;
            dot.y = f.y - s.y;
            return dot;
        }

        public static Dot2 operator *(Dot2 f, int i)
        {
            Dot2 dot = new Dot2();
            dot.x = f.x * i;
            dot.y = f.y * i;
            return dot;
        }

        public static int operator *(Dot2 f, Dot2 s)
        {
            return f.x * s.x + f.y * s.y;
        }

        public int Pow2()
        {
            return x * x + y * y;
        }

        public static int DistancePow2(CSMisc.Dot2 dot0, CSMisc.Dot2 dot1)
        {
            CSMisc.Dot2 d = dot0 - dot1;
            return d.x * d.x + d.y * d.y;
        }
    }

    public const float FloatZero = 0.000001f;
    public static bool isUnityEditor = false;
    public static string buildSettingName = "Android";
    public static Color color = new Color(1, 1, 1, 1);
    public static Color blackColor = new Color(0f, 0f, 0f, 0.5f);
    public static Color greyColor = new Color(1, 1, 1, 1);
    public static Vector3[] MeshVertices = new Vector3[4];
    public static Vector2[] uvs = new Vector2[4];
    public static int[] array2 = new int[4 * 3];
    public static Color[] meshColorList = new Color[4] { Color.white, Color.white, Color.white, Color.white };
    public static Vector3 vec3;
    public static Color ColorRed = new Color(1, 0, 0);
    public static Color ColorGreen = new Color(0, 1, 0);
    public static Color ColorGrey = new Color(0.299f, 0.587f, 0.114f);

    public static string GetPlatformName()
    {
        if (isUnityEditor)
        {
            return buildSettingName;
        }
        else
        {
            return GetPlatformForAssetBundles(Application.platform);
        }
    }

    private static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }
    }

    public static void InitEditorData(bool isEditor, string buidlSetting)
    {
        isUnityEditor = isEditor;
        buildSettingName = buidlSetting;
    }
   

    static List<string> mSpecialColorList = new List<string>()
    {
        "ffd400",
        "00ccff",
        "cb55ff",
        "ff0000",
        "ffeebb",
        "81796a",
        "ffffff",
        "ff9900",
        "00ff00",
        "c4b88e",
        "dddda9",
        "ffff99",
        "928078",
        "785035",
    };

    public static Dictionary<uint, string> NameColorRGB = new Dictionary<uint, string>()
    {
         { 0, "[FFFFFF]"},
         { 1, "[00ADFE]"},
         { 2, "[FFCC00]"},
         { 3, "[00FF2F]"},
         { 4, "[FF00FF]"},
         { 5, "[FE8900]"},
         { 6, "[FF0000]"},
         { 7, "[D5D5D5]"},
    };

    public static Dictionary<uint, Color> ItemQulityColorDic = new Dictionary<uint, Color>()
    {
        {1u,Color.white},
        {2u,new Color(0f,173/255f,254/255f)},
        {3u,new Color(1f,204/255f,0f)},
        {4u,new Color(0f,1f,47/255f)},
        {5u,new Color(1f,0f,1f)},
        {6u,new Color(254/255f,137/255f,0f)},
        {7u,new Color(166/255f,77/255f,1f)},
    };

    public static Dictionary<int, Color> PetHeadNameColorDic = new Dictionary<int, Color>()
    {
        {1,Color.white},
        {2,new Color(0.1568628f,0.9254902f,0.8470588f)},//28ecd8
        {3,new Color(0.4392157f,0.5882353f,0.772549f)},//7096c5
        {4,new Color(0.1058824f,0.5843138f,0.7294118f)},//1b95ba
        {5,new Color(0.2078431f,0.3411765f,0.4784314f)},//35577a
        {6,new Color(0.1921569f,0.2f,0.3529412f)},//31335a
        {7,new Color(0.1490196f,0.08627451f,0.5647059f)},//261690
        {8,new Color(0.8784314f,0.5294118f,0.2f)},//e08733
        {9,new Color(0.5254902f,0.4745098f,0.4078431f)},//867968
    };

    public static Dictionary<int, int> avatarLoadProriDic = new Dictionary<int, int>()
    {
        {EAvatarType.MainPlayer,ResourceAssistType.Charactar},
        {EAvatarType.Monster,ResourceAssistType.Monster},
        {EAvatarType.Guard,ResourceAssistType.Monster},
        {EAvatarType.Trigger,ResourceAssistType.Monster},
        {EAvatarType.NPC,ResourceAssistType.NPC},
        {EAvatarType.Pet,ResourceAssistType.CharactarPet},
        {EAvatarType.Player,ResourceAssistType.Player},
        {EAvatarType.RoleMonster,ResourceAssistType.Player},
        {EAvatarType.ZhanHun,ResourceAssistType.Player},

    };

    public static Dictionary<int, Vector3> tanweiDeltaDic = new Dictionary<int, Vector3>()
    {
        {0,new Vector2(0,132)},
        {1,new Vector2(32,140)},
        {2,new Vector2(58,102)},
        {3,new Vector2(37,66)},
        {4,new Vector2(7,52)},
        {5,new Vector2(-28,62)},
        {6,new Vector2(-58,90)},
        {7,new Vector2(-55,136)},

    };

    public static Dictionary<int, int> motionNamsCount = new Dictionary<int, int>()
    {
        {CSMotion.Stand,5},
        {CSMotion.Walk,8},
        {CSMotion.Run,8},
        {CSMotion.RunToStand,8},
        {CSMotion.StandToRun,8},
        {CSMotion.Attack,8},
        {CSMotion.Attack2,8},
        {CSMotion.Attack3,6},
        {CSMotion.Dead,10},
    };

    public static float SkillStandDelay(int skillID)
    {
        return 0.3f;
    }

    /// <summary>
    /// 根据一个最表得到半径为r 的随机点
    /// </summary>
    /// <param name="dot"> 圆心</param>
    /// <param name="radius">半径</param>
    /// /// <param name="radiusCount">防止死循环， 外部不要传入值</param>
    /// <returns></returns>
    public static Dot2 GetRandomPoint(int x, int y, int radius, int radiusCount = 0)
    {
        Dot2 d = new Dot2();

        int xmin = x - radius;
        int xmax = x + radius;
        int ymin = y - radius;
        int ymax = y + radius;

        d.x = UnityEngine.Random.Range(xmin, xmax);
        d.y = UnityEngine.Random.Range(ymin, ymax);

        CSCell cell =CSMesh.Instance.getCell(d.x, d.y);

        if ((d.x == x && d.y == y)
            || (cell != null && cell.isAttributes(MapEditor.CellType.Resistance)))
        {
            if (radiusCount > Mathf.Pow((2 * radius - 1), 2))
            {
                radius++;
            }

            radiusCount++;
            return GetRandomPoint(x, y, radius, radiusCount);
        }

        return d;
    }

    //把随机点的区域分为4个象限
    public static Dot2 GetRandomQuadrantPoint(int x, int y, int radius, int type)
    {
        Dot2 d = new Dot2();

        int xmin = x - radius;
        int xmax = x + radius;
        int ymin = y - radius;
        int ymax = y + radius;
        switch (type)
        {
            case 1:
                xmin = x;
                xmax = x + radius;
                ymin = y;
                ymax = y + radius;
                break;
            case 2:
                xmin = x - radius;
                xmax = x;
                ymin = y;
                ymax = y + radius;
                break;
            case 3:
                xmin = x;
                xmax = x + radius;
                ymin = y - radius;
                ymax = y;
                break;
            case 4:
                xmin = x - radius;
                xmax = x;
                ymin = y - radius;
                ymax = y;
                break;
        }

        d.x = UnityEngine.Random.Range(xmin, xmax);
        d.y = UnityEngine.Random.Range(ymin, ymax);

        CSCell cell = CSMesh.Instance.getCell(d.x, d.y);

        if ((d.x == x && d.y == y)
            || (cell != null && cell.isAttributes(MapEditor.CellType.Resistance)))
        {
            return GetRandomQuadrantPoint(x, y, radius, type);
        }

        return d;
    }

   
    /// <summary>
    /// 得到距离一个点最近的一个点
    /// </summary>
    /// <param name="dot"> 圆心</param>
    /// <param name="radius">半径</param>
    /// <returns></returns>
    public static Dot2 GetNearPoint(int x, int y, int sX, int sY, int radius)
    {
        Dot2 d;
        d.x = sX - x;
        d.y = sY - y;
        d.x = d.NormalX() * radius + x;
        d.y = d.NormalY() * radius + y;
        
        
        CSCell cell =CSMesh.Instance.getCell(d.x, d.y);

        if ((d.x == x && d.y == y)
            || (cell != null && cell.isAttributes(MapEditor.CellType.Resistance)))
        {
            return GetRandomPoint(d.x, d.y, radius);
        }

        return d;
    }

    public static void FixBrokenWord(UnityEngine.Font font, int fontSize = 16)
    {
        if (font == null) return;
        TextAsset txt = Resources.Load("Font/PreChinese") as TextAsset;
        string chineseTxt = txt.ToString();
        font.RequestCharactersInTexture(chineseTxt, fontSize);
    }

    public static Dictionary<int, Dot2> dirMove = new Dictionary<int, Dot2>()
    {
     {CSDirection.Right, new Dot2(1,0)},
     {CSDirection.Right_Up, new Dot2(1,-1)},
     {CSDirection.Up, new Dot2(0,-1)},

     {CSDirection.Left_Up, new Dot2(-1,-1)},
     {CSDirection.Left, new Dot2(-1,0)},
     {CSDirection.Left_Down, new Dot2(-1,1)},

     {CSDirection.Down, new Dot2(0,1)},
     {CSDirection.Right_Down, new Dot2(1,1)},
    };

    /// <summary>
    /// 获得最近curPos最佳位置（怪物移动后，人物在释放技能前需要判断，如何返回和人物当前位置相等，则释放，反之，使用获得的坐标，重新移动在最佳位置点）
    /// 如果在检测中，curPos符合释放技能条件，则直接返回，无需再寻找最佳位置
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static List<Dot2> dirList = new List<Dot2>()
    {
        new Dot2(1,0),
        new Dot2(1,-1),
        new Dot2(0,-1),
        new Dot2(-1,-1),
        new Dot2(-1,0),
        new Dot2(-1,1),
        new Dot2(0,1),
        new Dot2(1,1),
    };

    public static Dictionary<int, int> dirDic = new Dictionary<int, int>()
    {
        {0,CSDirection.Left_Down},
        {1,CSDirection.Left},
        {2,CSDirection.Left_Up},
        {10,CSDirection.Down},
        {12,CSDirection.Up},
        {21,CSDirection.Right},
        {20,CSDirection.Right_Down},
        {22,CSDirection.Right_Up},
    };

    public static Dictionary<int, Dictionary<int, int>> partsFPS = new Dictionary<int, Dictionary<int, int>>()
    {
        {EAvatarType.MainPlayer,new Dictionary<int,int>()},
        {EAvatarType.Player,new Dictionary<int,int>()},
        {EAvatarType.Monster,new Dictionary<int,int>()},
        {EAvatarType.Guard,new Dictionary<int,int>()},
        {EAvatarType.Pet,new Dictionary<int,int>()},
        {EAvatarType.NPC,new Dictionary<int,int>()},
        {EAvatarType.RoleMonster,new Dictionary<int,int>()},
    };

   public static Dictionary<int, int> DephtLeft = new Dictionary<int, int>()
   {
     {CSDirection.Down, -1},
     {CSDirection.Up, 1},

     {CSDirection.Left_Down, -1},
     {CSDirection.Left_Up, 1},
     {CSDirection.Left, 1},

     {CSDirection.Right, 1},
     {CSDirection.Right_Down, -1},
     {CSDirection.Right_Up, 1},
   };

    //深度（前加-后减）
    //--------------------
    //        ↑         //
    //        -1        //
    //        0         //
    //        1         //
    //        ↓         //
    //--------------------

    //深度变换原理--模型的左右-左手
    //----------------//
    //  前   后  后 
    //    ↖ ↑ ↗   
    // 前←中心点→ 后
    //    ↙ ↓ ↘   
    //  前   后  后 

    //深度变换原理--模型的左右-右手
    //----------------//
    // 后   后  前
    //   ↖ ↑ ↗  
    //后←中心点→ 前
    //   ↙ ↓ ↘  
    //  后  后  前 
    public static Dictionary<int, int> DephtRight = new Dictionary<int, int>()
   {
     {CSDirection.Up, -1},            //正的
     {CSDirection.Down, 1},          //后面

     {CSDirection.Left_Down, -1},     //后面
     {CSDirection.Left_Up, -1},       //后面
     {CSDirection.Left, -1},          //后面

     {CSDirection.Right, -1},         //正的
     {CSDirection.Right_Down, -1},    //正的
     {CSDirection.Right_Up, -1},      //正的
   };

   public static Dictionary<int, int> BackRight = new Dictionary<int, int>()
   {
     {CSDirection.Down, 2},           //后面
     {CSDirection.Up, -2},            //正的

     {CSDirection.Left_Down, 2},      //后面
     {CSDirection.Left_Up, -2},       //后面
     {CSDirection.Left, -2},          //后面

     {CSDirection.Right, -2},         //正的
     {CSDirection.Right_Down, 2},     //正的
     {CSDirection.Right_Up, -2},      //正的
   };

    ///特殊深度 
    ///道具0 CSItem  public virtual Vector3 getPosition()
    ///火墙-10 CSRoundEffect
    ///阴影-20 CSSpriteAnimation SetShodowDepath
    /// <summary>
    /// 每个格子间距的深度距离是100（像素）
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="depthType">0:格子深度 1：格子下面 2：格子深度上面 3 最小格子深度 4：最大格子深度 100-110：最小格子深度下面(x-100)*100 
    /// 200-210：最大格子深度上面（x-200）*100</param>
    /// <returns></returns>
    public static float GetDepth(CSCell cell, int depthType, int avatarType = EAvatarType.None)
    {
        //数字越大，层次越低
        if (cell == null) return 0;
        float depth = 0;
        float p_x = cell.Coord.x * 0.1f;

        if (depthType == 0)
        {
            depth = cell.LocalPosition2.y + p_x;

            switch (avatarType)
            {
                case EAvatarType.MainPlayer:
                    depth -= 10;
                    break;
                case EAvatarType.NPC:
                    depth -= 10;
                    break;
                case EAvatarType.Player:
                    depth -= 6;
                    break;
            }
        }
        else if (depthType == 1)
        {
            depth = cell.LocalPosition2.y + p_x + 25.0f;
        }
        else if (depthType == 2)
        {
            depth = cell.LocalPosition2.y + p_x - 25.0f;
        }
        else
        {

            CSCell ss_cell =CSMesh.Instance.getCell(0, 0);
            CSCell e_cell= CSMesh.Instance.getCell(0, CSMesh.HorizontalCount-1);
            if(ss_cell != null && e_cell != null)
            {
                int maxY = (int)Mathf.Min(ss_cell.LocalPosition2.y, e_cell.LocalPosition2.y);
                int minY = (int)Mathf.Max(ss_cell.LocalPosition2.y, e_cell.LocalPosition2.y);
                if (depthType == 3)
                {
                    depth = minY + 25;
                }
                else if (depthType == 4)
                {
                    depth = maxY - 25;
                }
                else if (depthType >= 100 && depthType <= 110)
                {
                    depth = minY + (depthType - 100) * 100;
                }
                else if (depthType >= 200 && depthType <= 210)
                {
                    depth = maxY - (depthType - 200) * 100;
                }
            }
        }
        depth = depth - 10000.0f;
        return depth;
    }

    public static Dictionary<int, string> stringMotionDic = new Dictionary<int, string>()
    {
        {CSMotion.Static,"Static"},
        {CSMotion.Stand,"Stand"},
        {CSMotion.Walk,"Walk"},
        {CSMotion.Attack,"Attack"},
        {CSMotion.Attack2,"Attack2"},
        {CSMotion.Attack3,"Attack3"},
        {CSMotion.BeAttack,"BeAttack"},
        {CSMotion.Dead,"Dead"},
        {CSMotion.Mining,"Mining"},
        {CSMotion.ShowStand,"ShowStand"},
        {CSMotion.Run,"Run"},
        {CSMotion.RunToStand,"RunToStand"},
        {CSMotion.StandToRun,"StandToRun"},
        {CSMotion.WaKuang,"WaKuang"},
        {CSMotion.GuWu,"GuWu"},
        {CSMotion.RunOverDoSmoething,"RunOverDoSmoething"},
    };

    public static string[] stringtoDirection = new string[]
    {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
    };


    public static long GetKey(int modelID, int motion, int direction)
    {
        int d = direction;
        if (motion == CSMotion.Dead)
        {
            d = CSDirection.Up;
        }
        else
        {
            switch (d)
            {
                case CSDirection.Left:
                    d = CSDirection.Right;
                    break;
                case CSDirection.Left_Up:
                    d = CSDirection.Right_Up;
                    break;
                case CSDirection.Left_Down:
                    d = CSDirection.Right_Down;
                    break;
            }
        }
        return (long)modelID * 10000 + motion * 100 + d;
    }

    public static string GetCombineModel(int model, int avaterMotion, int avaterDirection)
    {
        if (avaterMotion == 0 && avaterDirection == 0) return model.ToString();

        int direction = avaterDirection;

        int d = avaterDirection;

        if (avaterMotion == CSMotion.Dead)
        {
            d = CSDirection.Up;
        }
        else
        {
            if (d == CSDirection.Left)
                d = CSDirection.Right;
            else if (d == CSDirection.Left_Up)
                d = CSDirection.Right_Up;
            else if (d == CSDirection.Left_Down)
                d = CSDirection.Right_Down;
        }
        direction = d;
        CSStringBuilder.Clear();
        CSStringBuilder.Append(model.ToString(), "_", CSMisc.stringMotionDic.ContainsKey(avaterMotion) ? CSMisc.stringMotionDic[avaterMotion] : "", "_", stringtoDirection[direction]);
        return CSStringBuilder.ToString();
    }

    /// <summary>
    /// 得到反方向
    /// </summary>
    /// <param name="direction">朝向</param>
    /// <returns></returns>
    public static int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case CSDirection.Up:
                return CSDirection.Down;
            case CSDirection.Right_Up:
                return CSDirection.Left_Down;
            case CSDirection.Right:
                return CSDirection.Left;
            case CSDirection.Right_Down:
                return CSDirection.Left_Up;
            case CSDirection.Down:
                return CSDirection.Up;
            case CSDirection.Left_Down:
                return CSDirection.Right_Up;
            case CSDirection.Left:
                return CSDirection.Right;
            case CSDirection.Left_Up:
                return CSDirection.Right_Down;
        }

        return CSDirection.None;
    }

    public static float getActionAngle(int direction)
    {
        float angle = -90f;
        switch(direction)
        {
            case CSDirection.Up:
                angle = 90f;
                break;
            case CSDirection.Right_Up:
                angle = 45.0f;
                break;
            case CSDirection.Right:
                angle = 0f;
                break;
            case CSDirection.Right_Down:
                angle = -45.0f;
                break;
            case CSDirection.Down:
                angle = -90.0f;
                break;
            case CSDirection.Left_Down:
                angle = -145.0f;
                break;
            case CSDirection.Left:
                angle = 180.0f;
                break;
            case CSDirection.Left_Up:
                angle = 145.0f;
                break;
        }
        return angle;
    }
}
