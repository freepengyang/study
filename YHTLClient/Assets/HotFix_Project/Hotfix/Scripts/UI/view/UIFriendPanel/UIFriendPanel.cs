using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Collections;

public partial class UIFriendPanel : UIBasePanel
{
    protected FastArrayElementKeepHandle<social.FriendInfo> mExpressFriendList = new FastArrayElementKeepHandle<social.FriendInfo>(8);
    protected FastArrayElementFromPool<PrivateFriend> mExpressPrivateChatFriendList = new FastArrayElementFromPool<PrivateFriend>();
    public int ItemID = 38000006;
    protected UIAddFriendPanel mAddFriendPanel;
    protected UIPrivateChatPanel mPrivateChatPanel;

    public enum FriendChildPanel
    {
        FCP_AddFriendPanel = 5,
        PCP_PrivateChatPanel = 6,
    }

    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mPattern, "pattern");
        ScriptBinder.InvokeRepeating(0, 3, CSFriendInfo.Instance.RequestAllSocial);

        UIEventListener.Get(mToggleBlackList.gameObject, FriendType.FT_BLACK_LIST).onClick = OnClickToggle;
        UIEventListener.Get(mToggleDebt.gameObject, FriendType.FT_ENEMY).onClick = OnClickToggle;
        UIEventListener.Get(mToggleFriend.gameObject, FriendType.FT_FRIEND).onClick = OnClickToggle;
        UIEventListener.Get(mToggleRelation.gameObject, FriendType.FT_NONE).onClick = OnClickToggle;

        mBtnAddFriend.onClick = OnClickAddFriendBtn;
        mBtnClearList.onClick = OnClickClearList;
        mBtnHelper.onClick = OnClickHelper;

        mClientEvent.AddEvent(CEvent.SocialInfoUpdate, OnFriendInfoChanged);
        mClientEvent.AddEvent(CEvent.RemoveTouchPlayer, OnRemoveTouchPlayer);
        mClientEvent.AddEvent(CEvent.OnRecvNewPrivateChatMsg, PrivateChatTimeChange);
        mClientEvent.AddEvent(CEvent.PrivateChatToMessage, OnPrivateToChatMessage);

        RegisterRed(mTouchListRedPoint, RedPointType.Friend);

        RegChildPanel<UIAddFriendPanel>((int)FriendChildPanel.FCP_AddFriendPanel, mAddFriendHandle, null);
        RegChildPanel<UIPrivateChatPanel>((int)FriendChildPanel.PCP_PrivateChatPanel, mPrivateChatHandle, null);
        //mClientEvent.Reg((int)CEvent.OnFriendRelationChanged, OnFriendRelationChanged);
        //SelectChildPanel((int)FriendType.FT_FRIEND);
    }

    public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    {
        if (type > 4)
            return base.OpenChildPanel(type, fromToggle);
        RefreshData(type, 0);
        return this;
    }

    FriendType eFriendType = FriendType.FT_FRIEND;

    private void OnClickToggle(GameObject go)
    {
        Net.ReqGetSocialInfoMessage();
        eFriendType = (FriendType)go.GetComponent<UIEventListener>().parameter;
        mFriendScrollView.SetDragAmount(0, 0, false);
        ResetUI();
        GetFriendList();
        RefreshUI();
    }

    protected void OnFriendInfoChanged(uint id,object argv)
    {
        GetFriendList();
        RefreshUI();
    }

    protected void OnRemoveTouchPlayer(uint id, object argv)
    {
        ResetUI();
        PrivateChatTimeChange(0, null);
    }

    private void PrivateChatTimeChange(uint id, object argv)
    {
        if (eFriendType != FriendType.FT_NONE && eFriendType != FriendType.FT_FRIEND)
        {
            return;
        }
        GetFriendList();
        RefreshUI();
    }

    protected void OnFriendRelationChanged(uint id, object argv)
    {
        GetFriendList();
        RefreshUI();
    }

    private void OnClickHelper(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Friend);
    }

    void OnClickClearList(GameObject go)
    {
        if(!CSFriendInfo.Instance.HasLatelyTouchPlayer())
        {
            UtilityTips.ShowRedTips(1528);
            return;
        }
        CSFriendInfo.Instance.ClearLatelyTouchPlayerList();
        GetFriendList();
        RefreshUI();
    }

    private void OnClickAddFriendBtn(GameObject go)
    {
        if(null == mAddFriendPanel)
        {
            mAddFriendPanel = OpenChildPanel((int)FriendChildPanel.FCP_AddFriendPanel) as UIAddFriendPanel;
        }
        else
        {
            mAddFriendPanel.Show();
        }
    }

    private void RefreshUI()
    {
        mBtnAddFriend.gameObject.SetActive(eFriendType == FriendType.FT_FRIEND);
        mBtnClearList.gameObject.SetActive(eFriendType == FriendType.FT_NONE);
        mRightDesc.text = CSFriendInfo.Instance.ChooseFriend == null || eFriendType == FriendType.FT_BLACK_LIST ? GetRightDesc() : "";
        mRightDesc.gameObject.SetActive(CSFriendInfo.Instance.ChooseFriend == null || eFriendType == FriendType.FT_BLACK_LIST);
        if (eFriendType == FriendType.FT_NONE)
        {
            mLeftDesc.text = mExpressPrivateChatFriendList.Count == 0 ? GetLeftDesc() : "";
        }
        else
        {
            mLeftDesc.text = mExpressFriendList.Count == 0 ? GetLeftDesc() : "";
        }
        mCount.text = string.Empty;
        mFriendCount.text = string.Empty;
        int maxCount = CSFriendInfo.Instance.GetFriendListMaxCount(eFriendType);
        bool isListFull = mExpressFriendList.Count >= maxCount;
        var content = $"{GetTypeToStr().BBCode(ColorType.ProperyColor)}{UtilityColor.GetColorString(isListFull ? ColorType.Red : ColorType.Green)}{mExpressFriendList.Count}/{maxCount}";
        switch (eFriendType)
        {
            case FriendType.FT_FRIEND:
                mCount.text = content;
                break;
            case FriendType.FT_ENEMY:
                mFriendCount.text = content;
                break;
            case FriendType.FT_BLACK_LIST:
                mFriendCount.text = content;
                break;
            default:
                break;
        }
        if (eFriendType == FriendType.FT_NONE)
        {
            mContainer.gameObject.SetActive(mExpressPrivateChatFriendList.Count <= 4);
        }
        else
        {
            mContainer.gameObject.SetActive(mExpressFriendList.Count <= 4);
        }
    }

    public override void RefreshData(params object[] obj)
    {
        if (obj.Length < 2) return;
        eFriendType = (FriendType)((int)obj[0] % 4);
        CSFriendInfo.Instance.ChooseFriendId = System.Convert.ToInt64(obj[1]);
        CSFriendInfo.Instance.ChooseFriend = CSFriendInfo.Instance.GetFriendInfoFromTouchList(CSFriendInfo.Instance.ChooseFriendId);
        mFriendListScroll.SetDragAmount(0, 0, false);
        if (CSFriendInfo.Instance.ChooseFriend != null)
            CSFriendInfo.Instance.MarkChatMessageRead(CSFriendInfo.Instance.ChooseFriend.roleId);
        SetBtnToggleValue();
        GetFriendList();
        if (CSFriendInfo.Instance.ChooseFriend != null)
            ShowPrivateChatPanel();
        RefreshUI();
    }

    private void GetFriendList()
    {
        if (eFriendType == FriendType.FT_NONE)
        {
            mExpressPrivateChatFriendList = CSFriendInfo.Instance.GetPrivateChatList();
            if (CSFriendInfo.Instance.ChooseFriend != null)
            {
                CSFriendInfo.Instance.ChooseFriend = CSFriendInfo.Instance.GetFriendInfoFromTouchList(CSFriendInfo.Instance.ChooseFriend.roleId);
                mPrivateChatHandle.SetActive(CSFriendInfo.Instance.ChooseFriend != null);
                if (CSFriendInfo.Instance.ChooseFriend != null) CSFriendInfo.Instance.MarkChatMessageRead(CSFriendInfo.Instance.ChooseFriend.roleId);
            }
        }
        else
        {
            mExpressFriendList = CSFriendInfo.Instance.GetFriendInfoByType(eFriendType);
            if (CSFriendInfo.Instance.ChooseFriend != null)
            {
                CSFriendInfo.Instance.ChooseFriend = CSFriendInfo.Instance.GetFriendInfoByGuid(CSFriendInfo.Instance.ChooseFriend.roleId);
                mPrivateChatHandle.SetActive(CSFriendInfo.Instance.ChooseFriend != null && (CSFriendInfo.Instance.ChooseFriend.relation == 1 || CSFriendInfo.Instance.ChooseFriend.relation == 2));
                if (eFriendType == FriendType.FT_FRIEND)
                {
                    if (CSFriendInfo.Instance.ChooseFriend != null) CSFriendInfo.Instance.MarkChatMessageRead(CSFriendInfo.Instance.ChooseFriend.roleId);
                }
            }
        }
        RefreshFriendList();
    }

    UIFriendItemBinderData mData = new UIFriendItemBinderData();
    private void RefreshFriendList()
    {
        mGridList.MaxCount = eFriendType == FriendType.FT_NONE ? mExpressPrivateChatFriendList.Count : mExpressFriendList.Count;
        for (int i = 0; i < (eFriendType == FriendType.FT_NONE ? mExpressPrivateChatFriendList.Count : mExpressFriendList.Count); i++)
        {
            UIFriendItemBinder binder = null;
            var eventHandle = UIEventListener.Get(mGridList.controlList[i]);
            if(eventHandle.parameter == null)
            {
                binder = new UIFriendItemBinder();
                binder.Setup(eventHandle);
            }
            else
            {
                binder = eventHandle.parameter as UIFriendItemBinder;
            }

            if (eFriendType == FriendType.FT_NONE)
            {
                mData.friendInfo = mExpressPrivateChatFriendList[i].Info;
                mData.itemId = ItemID;
                mData.showRedPoint = mExpressPrivateChatFriendList[i].HasNewMessageToRead;
                mData.action = OnClickFriendItem;
                mData.isON = CSFriendInfo.Instance.ChooseFriend != null && CSFriendInfo.Instance.ChooseFriend.roleId == mExpressPrivateChatFriendList[i].Info.roleId;
            }
            else
            {
                if (eFriendType == FriendType.FT_FRIEND)
                {
                    mData.friendInfo = mExpressFriendList[i];
                    mData.itemId = ItemID;
                    mData.showRedPoint = CSFriendInfo.Instance.GetPrivateChatFriend(mExpressFriendList[i].roleId) != null ? CSFriendInfo.Instance.GetPrivateChatFriend(mExpressFriendList[i].roleId).HasNewMessageToRead : false;
                    mData.action = OnClickFriendItem;
                    mData.isON = CSFriendInfo.Instance.ChooseFriend != null && CSFriendInfo.Instance.ChooseFriend.roleId == mExpressFriendList[i].roleId;
                }
                else
                {
                    mData.friendInfo = mExpressFriendList[i];
                    mData.itemId = ItemID;
                    mData.showRedPoint = false;
                    mData.action = OnClickFriendItem;
                    mData.isON = CSFriendInfo.Instance.ChooseFriend != null && CSFriendInfo.Instance.ChooseFriend.roleId == mExpressFriendList[i].roleId;
                }
            }

            binder.Bind(mData);
        }
    }

    private void OnClickFriendItem(social.FriendInfo chooseFriend)
    {
        if (chooseFriend == null || (chooseFriend != null && CSFriendInfo.Instance.ChooseFriend != null && chooseFriend.roleId == CSFriendInfo.Instance.ChooseFriend.roleId)) 
            return;
        CSFriendInfo.Instance.ChooseFriend = chooseFriend;
        CSFriendInfo.Instance.ChooseFriendId = 0;
        if (eFriendType == FriendType.FT_NONE)
        {
            CSFriendInfo.Instance.ChooseFriendId = CSFriendInfo.Instance.ChooseFriend.roleId;
        }
        if (eFriendType == FriendType.FT_FRIEND || eFriendType == FriendType.FT_NONE || eFriendType == FriendType.FT_ENEMY)
        {
            CSFriendInfo.Instance.MarkChatMessageRead(CSFriendInfo.Instance.ChooseFriend.roleId);
            ShowPrivateChatPanel();
        }
        else
        {
            mPrivateChatHandle.SetActive(false);
        }
        mRightDesc.text = CSFriendInfo.Instance.ChooseFriend == null || eFriendType == FriendType.FT_BLACK_LIST ? GetRightDesc() : "";
        mRightDesc.gameObject.SetActive(CSFriendInfo.Instance.ChooseFriend == null || eFriendType == FriendType.FT_BLACK_LIST);

        GetFriendList();
        RefreshUI();
    }

    /// <summary>
    /// 显示私聊
    /// </summary>
    private void ShowPrivateChatPanel()
    {
        if(null != CSFriendInfo.Instance.ChooseFriend)
        {
            if(null == mPrivateChatPanel)
            {
                mPrivateChatPanel = OpenChildPanel((int)FriendChildPanel.PCP_PrivateChatPanel) as UIPrivateChatPanel;
            }
            mPrivateChatPanel.RefreshUI(eFriendType);
        }
    }

    /// <summary>
    /// 收到给人私聊
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    private void OnPrivateToChatMessage(uint id, object argv)
    {
        if (!this.ScriptBinder.gameObject.activeSelf)
            return;

        social.FriendInfo info = argv as social.FriendInfo;
        if (null == info || eFriendType == FriendType.FT_NONE)
            return;

        mFriendListScroll.SetDragAmount(0, 0, false);
        CSFriendInfo.Instance.AddPlayerToTouchList(info);
        eFriendType = FriendType.FT_NONE;
        CSFriendInfo.Instance.ChooseFriend = info;
        CSFriendInfo.Instance.MarkChatMessageRead(CSFriendInfo.Instance.ChooseFriend.roleId);
        SetBtnToggleValue();
        GetFriendList();
        RefreshUI();
        ShowPrivateChatPanel();
    }

    private void PrivateChatTimeChange(uint id = 0, params object[] data)
    {
        if (eFriendType != FriendType.FT_NONE && eFriendType != FriendType.FT_FRIEND) return;
        GetFriendList();
        RefreshUI();
    }

    public override void SelectChildPanel(int type = 1)
    {
        RefreshData(type,0);
    }

    private void SetBtnToggleValue()
    {
        switch (eFriendType)
        {
            case FriendType.FT_FRIEND:
                mToggleFriend.Set(true);
                break;
            case FriendType.FT_ENEMY:
                mToggleDebt.Set(true);
                break;
            case FriendType.FT_BLACK_LIST:
                mToggleBlackList.Set(true);
                break;
            case FriendType.FT_NONE:
                mToggleRelation.Set(true);
                break;
            default:
                break;
        }
    }

    private string GetLeftDesc()
    {
        switch (eFriendType)
        {
            case FriendType.FT_FRIEND:
                return CSString.Format(600);
            case FriendType.FT_ENEMY:
                return CSString.Format(601);
            case FriendType.FT_BLACK_LIST:
                return CSString.Format(602);
            case FriendType.FT_NONE:
                return CSString.Format(603);
            default:
                break;
        }
        return "";
    }
    private string GetRightDesc()
    {
        switch (eFriendType)
        {
            case FriendType.FT_NONE:
            case FriendType.FT_FRIEND:
                return CSString.Format(604);
            case FriendType.FT_ENEMY:
                return CSString.Format(605);
            case FriendType.FT_BLACK_LIST:
                return CSString.Format(606);
            default:
                break;
        }
        return "";

    }
    private string GetTypeToStr()
    {
        switch (eFriendType)
        {
            case FriendType.FT_FRIEND:
                return CSString.Format(607);
            case FriendType.FT_ENEMY:
                return CSString.Format(608);
            case FriendType.FT_BLACK_LIST:
                return CSString.Format(609);
            default:
                break;
        }
        return "";
    }

    private void ResetUI()
    {
        mPrivateChatHandle.SetActive(false);
        CSFriendInfo.Instance.ChooseFriend = null;
        CSFriendInfo.Instance.ChooseFriendId = 0;
    }

    protected override void OnDestroy()
    {
        //mExpressFriendList.Clear();
        //mExpressPrivateChatFriendList.Clear();
        CSEffectPlayMgr.Instance.Recycle(mPattern);
        mGridList.UnBind<UIFriendItemBinder>();
        mGridList = null;
        CSFriendInfo.Instance.ChooseFriend = null;
        mAddFriendPanel = null;
        mPrivateChatPanel = null;
        mClientEvent.RemoveEvent(CEvent.SocialInfoUpdate, OnFriendInfoChanged);
        mClientEvent.RemoveEvent(CEvent.RemoveTouchPlayer, OnRemoveTouchPlayer);
        mClientEvent.RemoveEvent(CEvent.OnRecvNewPrivateChatMsg, PrivateChatTimeChange);
        mClientEvent.RemoveEvent(CEvent.PrivateChatToMessage, OnPrivateToChatMessage);
        //mClientEvent.UnReg((int)CEvent.OnFriendRelationChanged, OnFriendRelationChanged);
        base.OnDestroy();
    }
}
