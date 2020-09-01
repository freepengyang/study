using UnityEngine;

public class UIItemMiniMapPoint : GridContainerBase
{
    private UISprite mSprPoint;
    private UILabel mlbPointName;
    private UIMiniMapPoint mMiniPoint;

    private int type2;
    private string mSpritNameOfType;
    private int mDepthAdd = 0;

    
    public void SetUIInfo(UIMiniMapType type, string avatarName, int _type2 = 0, string name = "")
    {
        if (type == UIMiniMapType.None) return;
        type2 = _type2;
        mPointType = type;

        SetPointName(name);
        SetPointSprite();

        if (!gameObject.activeSelf) gameObject.SetActive(true);
        gameObject.name = avatarName.ToString();
    }

    public override void Init()
    {
        base.Init();
        if (mSprPoint == null)
        {
            mSprPoint = transform.GetComponent<UISprite>();
        }

        if (mlbPointName == null)
        {
            mlbPointName = Get<UILabel>("lb_name");
        }
        if (mMiniPoint == null)
        {
            mMiniPoint = transform.GetComponent<UIMiniMapPoint>();
        }
    }

    public void SetFixedPos()
    {
        if (mMiniPoint != null)
        {
            mMiniPoint.SetFixedPos();
        }
    }

    public void SetTargetPos(int x, int y)
    {
        CSMisc.Dot2 interval = CSAvatarManager.MainPlayer.NewCell.Coord;
        if (mMiniPoint != null)
        {
            mMiniPoint.SetLocalPos(x, y, interval.x, interval.y, CSMapManager.mMapOneCellPos);
        }
    }

    public void SetMainPos(Vector2 mMainPlayerPos)
    {
        if (mMiniPoint != null)
        {
            mMiniPoint.SetMainPos(mMainPlayerPos);
        }
    }

    public void StartMove()
    {
        if (mMiniPoint != null)
        {
            mMiniPoint.BeginStartMove();
        }
    }

    public void SetSpeed(int speed)
    {
        if (mMiniPoint != null)
        {
            mMiniPoint.SetSpeed(speed);
        }
    }

    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    private void SetPointName(string name)
    {
        if (mlbPointName == null) return;
        if (!string.IsNullOrEmpty(name))
        {
            mlbPointName.text = name;
            mlbPointName.gameObject.SetActive(true);
        }
        else
            mlbPointName.gameObject.SetActive(false);
    }

    private void SetPointSprite()
    {
        if (mSprPoint == null) return;
        mSprPoint.spriteName = mSpritNameOfType;
        mSprPoint.depth = mDepthAdd;
        if (mlbPointName != null) mlbPointName.depth = mDepthAdd + 1;
        mSprPoint.MakePixelPerfect();
    }

    public override void Dispose()
    {
        mSpritNameOfType = "";
        if (mMiniPoint != null)
        {
            mMiniPoint.Dispose();
        }

        mlbPointName.text = "";
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }

    private UIMiniMapType mPointType
    {
        set
        {
            switch (value)
            {
                case UIMiniMapType.NPC:
                    mSpritNameOfType = "map_dian4";
                    mDepthAdd = 10;
                    break;
                case UIMiniMapType.Monster:
                    mPointMonsterType = (UIMiniMapMonsterType) type2;
                    break;
                case UIMiniMapType.WayPoint:
                    mSpritNameOfType = "map_teleport";
                    mDepthAdd = 10;
                    break;
                case UIMiniMapType.Player:
                    mPointPlayerType = (UIMiniMapPlayerType) type2;
                    break;
                case UIMiniMapType.SpecialTeam:
                    mSpritNameOfType = "map_dian3";
                    mDepthAdd = 12;
                    break;
            }
        }
    }

    private UIMiniMapPlayerType mPointPlayerType
    {
        set
        {
            switch (value)
            {
                case UIMiniMapPlayerType.None:
                case UIMiniMapPlayerType.OtherPlayer:
                    mSpritNameOfType = "map_dian5";
                    mDepthAdd = 20;
                    break;
                case UIMiniMapPlayerType.TeamPlayer:
                    mSpritNameOfType = "map_dian3";
                    mDepthAdd = 21;
                    break;
            }
        }
    }


    private UIMiniMapMonsterType mPointMonsterType
    {
        set
        {
            switch (value)
            {
                case UIMiniMapMonsterType.None:
                case UIMiniMapMonsterType.Normal:
                    mSpritNameOfType = "map_dian2";
                    mDepthAdd = 11;
                    break;
                case UIMiniMapMonsterType.Boss:
                    mSpritNameOfType = "map_boss2";
                    mDepthAdd = 11;
                    break;
                case UIMiniMapMonsterType.MapBoss:
                    mSpritNameOfType = "map_boss1";
                    mDepthAdd = 11;
                    break;
            }
        }
    }
}