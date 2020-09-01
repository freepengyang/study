using UnityEngine;
using System.Collections;

public partial class UIAddFriendPanel : UIBasePanel
{
    private FastArrayElementFromPool<UIItemFriendShortInfoData> mSearchedList;

    public override void Init()
    {
        base.Init();

        mSearchedList = new FastArrayElementFromPool<UIItemFriendShortInfoData>(32, () =>
        {
            return mPoolHandleManager.GetSystemClass<UIItemFriendShortInfoData>();
        }, f =>
        {
            mPoolHandleManager.Recycle(f);
        });

        mBtnAddAll.onClick = OnClickAddAllBtn;
        mBtnSearch.onClick = OnClickSearchBtn;

        mClientEvent.AddEvent(CEvent.AddSearchFriendsInfo, ReceivesSeekFriend);
        mClientEvent.AddEvent(CEvent.DelSearchFriendsInfo, ReceivesSeekFriend);

        mBtnAddClose.onClick = this.Hide;
        UIEventListener.Get(this.UIPrefab).onClick = this.Hide;
    }

    public override void OnHide()
    {
        CSFriendInfo.Instance.SearchedFriends.Clear();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.AddSearchFriendsInfo, ReceivesSeekFriend);
        mClientEvent.RemoveEvent(CEvent.DelSearchFriendsInfo, ReceivesSeekFriend);

        CSFriendInfo.Instance.SearchedFriends.Clear();
        base.OnDestroy();
    }


    public override void Show()
    {
        base.Show();
        mInput.value = string.Empty;
        ShowGridList();
    }

    public void ShowGridList()
    {
        var searchedFriends = CSFriendInfo.Instance.SearchedFriends;
        mSearchedList.Count = searchedFriends.Count;
        for (int i = 0; i < searchedFriends.Count; ++i)
        {
            var applyInfo = mSearchedList[i];
            applyInfo.canChat = false;
            applyInfo.Info = searchedFriends[i];
            applyInfo.m_call = OnAddFriendByName;
            applyInfo.m_refusecall = null;
            applyInfo.number = i;
        }
        mGridList.Bind<UIItemFriendShortInfoBinder, UIItemFriendShortInfoData>(mSearchedList);
    }

    private void OnClickAddAllBtn(GameObject go)
    {
        for (int i = 0; i < CSFriendInfo.Instance.SearchedFriends.Count; i++)
        {
            OnAddFriendByName(CSFriendInfo.Instance.SearchedFriends[i].roleId, CSFriendInfo.Instance.SearchedFriends[i].name);
        }
        mInput.value = string.Empty;
    }

    private void OnClickSearchBtn(GameObject go)
    {
        Net.ReqFindPlayerByNameMessage(mInput.value,(int)FriendType.FT_FRIEND);
    }

    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="name"></param>
    private void OnAddFriendByName(long roleId, string name)
    {
        if (!string.IsNullOrEmpty(name))
        {         
            Net.ReqAddFriendByNameMessage(name);
        }
    }

    private void ReceivesSeekFriend(uint id,object argv)
    {
        ShowGridList();
    }
}