using Google.Protobuf;
using UnityEngine;

public class CSMapPlayerInfo : CSMapAvatarInfo
{
    public override MapAvaterType AvatarType
    {
        get { return MapAvaterType.Player; }
    }

    private CSAvatar avatar;

    //队伍
    private long mTeamid;

    public long Teamid
    {
        get { return mTeamid; }
        set
        {
            if (mTeamid != value)
            {
                mTeamid = value;
                RefreshUI();
            }
        }
    }

    protected override void Init(IMessage message)
    {
        user.RoleBrief info = message as user.RoleBrief;
        if (info == null) return;
        mDotX = info.x;
        mDotY = info.y;
        ID = info.roleId;

        AvatarName = info.roleName;
        SetProperty(info);
        AddEvent();
    }

    private void AddEvent()
    {
        avatar = CSAvatarManager.Instance.GetAvatar(ID);
        if (avatar != null && avatar.BaseInfo != null)
        {
            avatar.BaseInfo.EventHandler.AddEvent(CEvent.Player_TeamIdChange, TeamChange);
        }
    }

    public void SetProperty(user.RoleBrief info)
    {
        Teamid = info.teamId;
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
            int type2 = CSMapManager.Instance.GetPlayerType(this);
            mMapPoint.SetUIInfo(UIMiniMapType.Player, AvatarName, type2);
        }
    }

    private void TeamChange(uint id, object data)
    {
        if (data == null) return;
        Teamid = (long) data;
    }

    public override void Dispose()
    {
        if (avatar != null && avatar.BaseInfo != null)
        {
            avatar.BaseInfo.EventHandler.RemoveEvent(CEvent.Player_TeamIdChange, TeamChange);
        }


        base.Dispose();
    }
}