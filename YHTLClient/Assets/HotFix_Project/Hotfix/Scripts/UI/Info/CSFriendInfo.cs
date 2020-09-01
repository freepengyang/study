using Google.Protobuf.Collections;
using social;
using System.Collections.Generic;
using UnityEngine;

public enum FriendType
{
    FT_NONE = 0,//陌生人,最近联系人
    FT_FRIEND = 1,//好朋友
    FT_ENEMY = 2,//仇人
    FT_BLACK_LIST = 3,//黑名单
}

public class PrivateFriend
{
    public long ReceivesTime;
    public FriendInfo Info;
    public bool HasNewMessageToRead;
    public PrivateFriend()
    {
        ReceivesTime = 0;
        Info = null;
        this.HasNewMessageToRead = false;
    }
}

public partial class CSFriendInfo : CSInfo<CSFriendInfo>
{
	protected Dictionary<int, FastArrayElementKeepHandle<FriendInfo>> mType2Infos = new Dictionary<int, FastArrayElementKeepHandle<FriendInfo>>();
	protected Dictionary<long, FriendInfo> mGUID2Infos = new Dictionary<long, FriendInfo>(256);
    protected FastArrayElementKeepHandle<FriendInfo> mShadow = new FastArrayElementKeepHandle<FriendInfo>(8);
    protected FastArrayElementKeepHandle<FriendInfo> mApplyList = new FastArrayElementKeepHandle<FriendInfo>();
    protected RepeatedField<social.FriendInfo> mSearchedFriends = new RepeatedField<FriendInfo>();
    public RepeatedField<social.FriendInfo> SearchedFriends
    {
        get
        {
            return mSearchedFriends;
        }
    }
    protected FastArrayElementFromPool<PrivateFriend> mPrivateChat;
    public long ChooseFriendId { get; set; }
    protected int[] mFriendInfoMaxCount = new int[4];
    private int PrivateFriendMaxCount
    {
        get
        {
            return mFriendInfoMaxCount[(int)FriendType.FT_NONE];
        }
    }

    public int GetFriendListMaxCount(FriendType eFriendType)
    {
        int idx = (int)eFriendType;
        if (idx >= 0 && idx < mFriendInfoMaxCount.Length)
            return mFriendInfoMaxCount[idx];
        return 0;
    }

    public CSFriendInfo()
	{
        ChooseFriend = null;
        ChooseFriendId = 0;
        mType2Infos.Clear();
        mGUID2Infos.Clear();
        mApplyList.Clear();
        mSearchedFriends.Clear();
        mPrivateChat = new FastArrayElementFromPool<PrivateFriend>(32, CreatePrivateFriend,RecyclePrivateFriend);
        mPrivateChat.Clear();
        InitAddFriendLvLimit();
        InitializeFriendInfoMaxCounts();
    }

    protected List<KeyValuePair<int,int>> mDay2Level = new List<KeyValuePair<int, int>>(8);
    protected void InitAddFriendLvLimit()
    {
        mDay2Level.Clear();
        int sundryId = 373;
        TABLE.SUNDRY sundry = null;
        if(!SundryTableManager.Instance.TryGetValue(sundryId,out sundry) || string.IsNullOrEmpty(sundry.effect))
        {
            return;
        }

        var dayAndLvs = sundry.effect.Split('&');
        for(int i = 0; i < dayAndLvs.Length; ++i)
        {
            int day = 0;
            int lv = 0;
            if(dayAndLvs.Length == 2 && int.TryParse(dayAndLvs[0],out day) && int.TryParse(dayAndLvs[1],out lv))
            {
                mDay2Level.Add(new KeyValuePair<int, int>(day,lv));
            }
        }
    }

    protected void InitializeFriendInfoMaxCounts()
    {
        TABLE.SUNDRY Sundry;
        if (!SundryTableManager.Instance.TryGetValue(326, out Sundry))
            return;
        if (string.IsNullOrEmpty(Sundry.effect))
            return;
        var tokens = Sundry.effect.Split('#');
        if (null == tokens || tokens.Length != 4)
            return;
        int.TryParse(tokens[0], out mFriendInfoMaxCount[(int)FriendType.FT_FRIEND]);
        int.TryParse(tokens[1], out mFriendInfoMaxCount[(int)FriendType.FT_ENEMY]);
        int.TryParse(tokens[2], out mFriendInfoMaxCount[(int)FriendType.FT_BLACK_LIST]);
        int.TryParse(tokens[3], out mFriendInfoMaxCount[(int)FriendType.FT_NONE]);
    }

    protected PoolHandleManager mPoolHandle = new PoolHandleManager();
    protected PrivateFriend CreatePrivateFriend()
    {
        return mPoolHandle.GetSystemClass<PrivateFriend>();
    }
    protected void RecyclePrivateFriend(PrivateFriend f)
    {
        mPoolHandle.Recycle(f);
    }


    public void RequestAllSocial()
	{
        //请求所有社会关系
        Net.ReqGetSocialInfoMessage();
    }

    //添加到好友查询列表
    public void AddSearchFriends(RepeatedField<social.FriendInfo> searchFriends)
    {
        mSearchedFriends.Clear();
        mSearchedFriends.AddRange(searchFriends);
        HotManager.Instance.EventHandler.SendEvent(CEvent.AddSearchFriendsInfo);
    }

    //添加好友后从对应的搜索好友列表中删除
    public void RemoveSearchFriends(RepeatedField<social.FriendInfo> friendsAdded)
    {
        for (int i = 0; i < friendsAdded.Count; i++)
        {
            for (int j = 0; j < mSearchedFriends.Count; j++)
            {
                if (mSearchedFriends[j].roleId == friendsAdded[i].roleId)
                {
                    mSearchedFriends.RemoveAt(j);
                    break;
                }
            }
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.DelSearchFriendsInfo);
    }

    public void OnResetFriendInfos(RepeatedField<FriendInfo> infos)
	{
		mGUID2Infos.Clear();
        for (int i = 0; i < infos.Count; ++i)
        {
            var info = infos[i];
            if (null == info)
            {
                continue;
            }

            if(!mGUID2Infos.ContainsKey(info.roleId))
            {
                mGUID2Infos.Add(info.roleId, info);
            }
            else
            {
                mGUID2Infos[info.roleId] = info;
            }
        }

        var itArray = mType2Infos.GetEnumerator();
        while (itArray.MoveNext())
            itArray.Current.Value.Clear();
        mType2Infos.Clear();

        var it = mGUID2Infos.GetEnumerator();
        while(it.MoveNext())
        {
            var info = it.Current.Value;
            FastArrayElementKeepHandle<FriendInfo> relations = null;
            if (!mType2Infos.ContainsKey(info.relation))
            {
                relations = new FastArrayElementKeepHandle<FriendInfo>(128);
                mType2Infos.Add(info.relation, relations);
            }
            else
            {
                relations = mType2Infos[info.relation];
            }
            relations.Append(it.Current.Value);
        }

        itArray = mType2Infos.GetEnumerator();
        while (itArray.MoveNext())
        {
            itArray.Current.Value.Sort(RelationComparer);
        }
    }

    public FastArrayElementKeepHandle<FriendInfo> GetFriendInfoByType(FriendType eType)
    {
        FastArrayElementKeepHandle<FriendInfo> ret = mShadow;
        if (mType2Infos.ContainsKey((int)eType))
            ret = mType2Infos[(int)eType];

        ret.Sort(ComparerForFriend);

        return ret;
    }

    public bool AcceptStrangerMsg
    {
        get
        {
            return !CSConfigInfo.Instance.GetBool(ConfigOption.ForbidStranger);
        }
    }

    public bool AcceptFriendInvite
    {
        get
        {
            return !CSConfigInfo.Instance.GetBool(ConfigOption.ForbidFriend);
        }
    }

    int ComparerForFriend(FriendInfo l, FriendInfo r)
    {
        if (l.isOnline != r.isOnline)
            return l.isOnline ? -1 : 1;
        if (l.level != r.level)
            return r.level - l.level;

        if (l.relation == 1)
        {
            if (l.friendLove != r.friendLove)
                return r.friendLove - l.friendLove;
        }
        else if(l.relation == 2)
        {
            if (l.enemy != r.enemy)
                return r.enemy - l.enemy;
        }

        return l.name.CompareTo(r.name);
    }

    public FriendInfo GetFriendInfoByGuid(long guid)
    {
        FriendInfo friendInfo = null;
        if(mGUID2Infos.ContainsKey(guid))
        {
            friendInfo = mGUID2Infos[guid];
        }
        return friendInfo;
    }

    public FriendType GetRelation(long roleId)
    {
        var relation = GetFriendInfoByGuid(roleId);
        if (null != relation)
            return (FriendType)relation.relation;
        return FriendType.FT_NONE;
    }

    public bool CanAddFriend()
    {
        var openDays = CSMainPlayerInfo.Instance.RoleExtraValues?.openServerDays;
        int lv = CSMainPlayerInfo.Instance.Level;
        for(int i = 0; i < mDay2Level.Count; ++i)
        {
            if(i == mDay2Level.Count - 1)
            {
                if (mDay2Level[i].Key <= openDays && mDay2Level[i].Value > lv)
                    return false;
            }
            else
            {
                if (mDay2Level[i].Key == openDays && mDay2Level[i].Value > lv)
                    return false;
            }
        }
        return true;
    }
    public social.FriendInfo ChooseFriend { get; set; }

    public bool IsPlayerInBlackList(long guid)
    {
        var relation = GetFriendInfoByGuid(guid);
        return null != relation && relation.relation == (int)FriendType.FT_BLACK_LIST;
    }

    public void AddFriends(RepeatedField<FriendInfo> friendInfos)
    {
        if (null == friendInfos)
            return;

        for(int i = 0; i < friendInfos.Count; ++i)
        {
            var friendInfo = friendInfos[i];
            AddRelationToTypeInfoDic(friendInfo);
            AddRelationToGuidDic(friendInfo);
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.SocialInfoUpdate);
    }

    public void DeleteFriends(RepeatedField<long> roleIds)
    {
        for (int i = 0; i < roleIds.Count; ++i)
        {
            var roleId = roleIds[i];
            FriendInfo relation = null;
            if(!mGUID2Infos.ContainsKey(roleId))
            {
                continue;
            }
            relation = mGUID2Infos[roleId];
            mGUID2Infos.Remove(relation.roleId);
            RemoveRelationFromTypeInfoDic(relation.relation,relation.roleId);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.SocialInfoUpdate);
    }

    public void RequestAddFriend(long guid)
    {
        var relation = GetFriendInfoByGuid(guid);
        if(null != relation)
        {
            if (relation.relation == (int)FriendType.FT_FRIEND)
            {
                UtilityTips.ShowRedTips(564, relation.name);
                return;
            }
            if (relation.relation == (int)FriendType.FT_ENEMY)
            {
                UtilityTips.ShowRedTips(565);
                return;
            }
            if (relation.relation == (int)FriendType.FT_BLACK_LIST)
            {
                UtilityTips.ShowRedTips(566);
                return;
            }
            return;
        }

        Net.ReqAddRelationMessage(guid.ToGoogleList(), (int)FriendType.FT_FRIEND);
    }

    public void OnResetFriendRelation(RepeatedField<RelationInfo> infos)
    {
        for(int i = 0; i < infos.Count; ++i)
        {
            var info = infos[i];
            FriendInfo relation = null;
            if (mGUID2Infos.ContainsKey(info.roleId))
            {
                relation = mGUID2Infos[info.roleId];
                if (relation.relation != info.relationType)
                {
                    int oldRelation = relation.relation;
                    RemoveRelationFromTypeInfoDic(oldRelation,relation.roleId);
                    relation.relation = info.relationType;
                    AddRelationToTypeInfoDic(relation);
                }
            }
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.OnFriendRelationChanged);
    }

    protected void RemoveRelationFromTypeInfoDic(int type,long roleId)
    {
        if (mType2Infos.ContainsKey(type))
        {
            var relations = mType2Infos[type];
            for (int j = 0; j < relations.Count; ++j)
            {
                if(relations[j].roleId == roleId)
                {
                    relations.SwapErase(j--);
                    break;
                }
            }
        }
    }

    public void UpdateLatelyTouchList(RepeatedField<social.FriendInfo> touchList)
    {
        if(null != touchList)
        {
            for (int i = 0; i < touchList.Count; ++i)
            {
                var touch = touchList[i];
                for (int j = 0; j < mPrivateChat.Count; ++j)
                {
                    if (mPrivateChat[j].Info.roleId == touch.roleId)
                    {
                        mPrivateChat[j].Info = touch;
                        break;
                    }
                }
            }
        }
    }

    public void AddPrivateChat(long id, string Name, int career = 0, int level = 0,int sex = 0,bool isOnLine = true,bool isSelf = false)
    {
        for (int i = 0; i < mPrivateChat.Count; i++)
        {
            if (mPrivateChat[i].Info.roleId == id)
            {
                mPrivateChat[i].ReceivesTime = CSServerTime.Instance.TotalSeconds;
                mPrivateChat[i].HasNewMessageToRead = !isSelf;
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecvNewPrivateChatMsg);
                return;
            }
        }

        social.FriendInfo info = new social.FriendInfo();
        info.roleId = id;
        info.name = Name;
        info.career = career;
        info.level = level;
        info.sex = sex;
        //info.nationId = Nation;
        //info.photo = Photo;
        info.isOnline = isOnLine;
        mPrivateChat.Count = mPrivateChat.Count + 1;
        var handle = mPrivateChat[mPrivateChat.Count - 1];
        handle.Info = info;
        handle.ReceivesTime = CSServerTime.Instance.TotalSeconds;
        handle.HasNewMessageToRead = !isSelf;
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecvNewPrivateChatMsg);
    }

    public social.FriendInfo GetFriendInfoFromTouchList(long roleid)
    {
        var chatList = GetPrivateChatList();
        for (int i = 0; i < chatList.Count; i++)
        {
            if (chatList[i].Info != null && roleid == chatList[i].Info.roleId)
            {
                return chatList[i].Info;
            }
        }
        return null;
    }

    public PrivateFriend GetPrivateChatFriend(long roleid)
    {
        var chatList = GetPrivateChatList();
        for (int i = 0; i < chatList.Count; i++)
        {
            if (chatList[i].Info != null && roleid == chatList[i].Info.roleId)
            {
                return chatList[i];
            }
        }
        return null;
    }

    public FastArrayElementFromPool<PrivateFriend> GetPrivateChatList()
    {
        for (int i = mPrivateChat.Count - 1; i >= 0; i--)
        {
            var relation = GetFriendInfoByGuid(mPrivateChat[i].Info.roleId);
            if(null != relation)
            {
                if (relation.relation == 3)
                    mPrivateChat.RemoveAt(i);
                else
                    mPrivateChat[i].Info = relation;
            }
            else
            {
                mPrivateChat[i].Info.relation = 0;
            }
        }
        mPrivateChat.Sort(PrivateFriendComparer);
        for (int i = mPrivateChat.Count - 1; i >= 0; i--)
        {
            if (mPrivateChat[i].Info != null && mPrivateChat[i].Info.roleId != ChooseFriendId && mPrivateChat.Count > PrivateFriendMaxCount)
            {
                mPrivateChat.RemoveAt(i);
            }
        }
        return mPrivateChat;
    }

    protected int PrivateFriendComparer(PrivateFriend l,PrivateFriend r)
    {
        if(l.ReceivesTime != r.ReceivesTime)
            return -l.ReceivesTime.CompareTo(r.ReceivesTime);
        return l.Info.name.CompareTo(r.Info.name);
    }

    public void ClearLatelyTouchPlayerList()
    {
        mPrivateChat.Clear();
    }

    public bool HasLatelyTouchPlayer()
    {
        return mPrivateChat.Count > 0;
    }

    public void AddPlayerToTouchList(social.FriendInfo info)
    {
        for (int i = 0; i < mPrivateChat.Count; i++)
        {
            if (mPrivateChat[i].Info != null && info.roleId == mPrivateChat[i].Info.roleId)
            {
                mPrivateChat[i].ReceivesTime = CSServerTime.Instance.TotalSeconds;
                mPrivateChat[i].HasNewMessageToRead = true;
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnRecvNewPrivateChatMsg);
                return;
            }
        }
        mPrivateChat.Count += 1;
        var handle = mPrivateChat[mPrivateChat.Count - 1];
        handle.Info = info;
        handle.ReceivesTime = CSServerTime.Instance.TotalSeconds;
        handle.HasNewMessageToRead = true;
        //这里原来没有不知道为啥
        //CSGame.Sington.EventHandler.SendEvent(CEvent.PrivateChatTimeChange);
    }

    public void RemovePlayerFromTouchList(long roleId)
    {
        for (int i = 0; i < mPrivateChat.Count; i++)
        {
            if (mPrivateChat[i].Info != null && roleId == mPrivateChat[i].Info.roleId)
            {
                mPrivateChat.RemoveAt(i--);
                return;
            }
        }
    }

    protected void AddRelationToTypeInfoDic(FriendInfo relation)
    {
        if (null != relation)
        {
            FastArrayElementKeepHandle<FriendInfo> relations = null;
            if (!mType2Infos.ContainsKey(relation.relation))
            {
                relations = new FastArrayElementKeepHandle<FriendInfo>(128);
                mType2Infos.Add(relation.relation, relations);
            }
            else
            {
                relations = mType2Infos[relation.relation];
            }
            relations.Append(relation);
        }
    }

    protected void AddRelationToGuidDic(FriendInfo relation)
    {
        if(null != relation)
        {
            if(!mGUID2Infos.ContainsKey(relation.roleId))
            {
                mGUID2Infos.Add(relation.roleId, relation);
            }
            else
            {
                mGUID2Infos[relation.roleId] = relation;
            }
        }
    }

    protected int RelationComparer(FriendInfo l,FriendInfo r)
    {
        if (l.isOnline != r.isOnline)
            return l.isOnline ? -1 : 1;
        if (l.level != r.level)
            return r.level - l.level;
        return string.Compare(l.name, r.name);
    }

    public FastArrayElementKeepHandle<FriendInfo> ApplyList()
    {
        return mApplyList;
    }

    public bool IsApplyListEmpty()
    {
        return null == mApplyList || mApplyList.Count <= 0;
    }

    public void ResetApplyList(RepeatedField<social.FriendInfo> applyList)
    {
        mApplyList.Clear();
        if (null != applyList)
        {
            for(int i = 0; i < applyList.Count; ++i)
            {
                mApplyList.Append(applyList[i]);
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnApplyListChanged);
    }

    public void RemoveApplyFromList(long roleId)
    {
        for (int i = 0; i < mApplyList.Count; ++i)
        {
            if (mApplyList[i].roleId == roleId)
            {
                mApplyList.RemoveAt(i--);
                break;
            }
        }
    }

    public void ClearApplyList()
    {
        for(int i = 0; i < mApplyList.Count; ++i)
        {
            Net.RejectSingleReqMessage(mApplyList[i].roleId);
        }
        mApplyList.Clear();
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnApplyListChanged);
    }

    public void MarkChatMessageRead(long roleid)
    {
        var chatList = GetPrivateChatList();
        for (int i = 0; i < chatList.Count; i++)
        {
            if (chatList[i].Info != null && roleid == chatList[i].Info.roleId)
            {
                chatList[i].HasNewMessageToRead = false;
                break;
            }
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.OnPrivateChatMessageBeRead);
    }

    public bool HasNewChatMessageToRead()
    {
        var chatList = GetPrivateChatList();
        for (int i = 0; i < chatList.Count; i++)
        {
            if (chatList[i].HasNewMessageToRead)
            {
                return true;
            }
        }
        return false;
    }

    public override void Dispose()
	{
        mType2Infos.Clear();
        mType2Infos = null;
        mGUID2Infos.Clear();
        mGUID2Infos = null;
        mApplyList.Clear();
        mApplyList = null;
    }
}