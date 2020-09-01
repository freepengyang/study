using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TweenPosData
{
    public Vector3 targetPos = Vector3.zero;
    public float duration = 0.50f;
}

public partial class UIRelationCombinedPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public enum ChildPanelType
    {
        CPT_MAIL = 1,
        CPT_FRIEND = 2,
        CPT_TEAM = 3,
    }

    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = this.Close;
        RegChildPanel<UIMailPanel>((int)ChildPanelType.CPT_MAIL, mMailPanel.gameObject, mTogMail);
        RegChildPanel<UIFriendPanel>((int)ChildPanelType.CPT_FRIEND, mFriendPanel.gameObject, mTogFriend);
        RegChildPanel<UITeamPanel>((int)ChildPanelType.CPT_TEAM, mTeamPanel.gameObject, mTogTeam);

        RegisterRed(mFriendRedPoint, RedPointType.Friend);
        mClientEvent.AddEvent(CEvent.OnPrivateChatTween, OnPrivateChatTween);
    }

    protected void OnPrivateChatTween(uint id,object argv)
    {
        if(argv is TweenPosData tweenPos)
        {
            if(null != mMainTween)
            {
                mMainTween.from = mMainTween.value;
                mMainTween.to = tweenPos.targetPos;
                mMainTween.duration = tweenPos.duration;
                mMainTween.ResetToBeginning();
                mMainTween.PlayForward();
            }
        }
    }
}