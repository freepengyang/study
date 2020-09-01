using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatListViewData
{
    public UIChatBinder.ChatVisibleMode eMode;
    public ChatType eChatType;
    public FixedCircleArray<ChatData> oldChatDatas;
    public FixedCircleArray<ChatData> chatDatas;
    public void Clear()
    {
        //注意这里的数据是有CSFriendInfo中的PrivateChat管理的 不能调用清除
        //oldChatDatas?.Clear();
        oldChatDatas = null;
        //chatDatas?.Clear();
        chatDatas = null;
    }
}

public class UIChatListViewBinder : UIBinder
{
    protected GameObject go_left_chat_binder;
    protected GameObject go_right_chat_binder;
    protected UISprite sp_face;

    protected const int MAX_BINDER_COUNTS = 32;
    protected const int MAX_MERGED_BINDER_COUNTS = MAX_BINDER_COUNTS << 1;
    protected Stack<UIChatBinder> mLeftBinders = new Stack<UIChatBinder>(MAX_BINDER_COUNTS);
    protected Stack<UIChatBinder> mRightBinders = new Stack<UIChatBinder>(MAX_BINDER_COUNTS);
    protected FixedCircleArray<UIChatBinder> mMergedBinders = new FixedCircleArray<UIChatBinder>(MAX_MERGED_BINDER_COUNTS);
    protected Dictionary<int, UIChatBinder> mCahedLogicBinders = new Dictionary<int, UIChatBinder>(MAX_MERGED_BINDER_COUNTS);
    int titleHeight = 0;
    public int TitleHeight
    {
        get
        {
            return titleHeight;
        }
        set
        {
            titleHeight = value;
            Reposition();
        }
    }
    protected int mHeight;

    public override void Init(UIEventListener handle)
    {
        var template = Handle.transform.parent.parent.parent.Find("template");
        go_left_chat_binder = template.Find("left").gameObject;
        go_right_chat_binder = template.Find("right").gameObject;
        sp_face = Get<UISprite>("face", template.transform);
        mHeight = 0;
    }

    protected UIChatBinder LoadChatBinder(ChatData chatData)
    {
        UIChatBinder handle = null;

        bool needLocatedLeft = true;
        if(chatData.IsSystemMessage())
        {
            needLocatedLeft = true;
        }
        else if(CSMainPlayerInfo.Instance.ID == chatData.msg.sender)
        {
            needLocatedLeft = false;
        }
        else
        {
            needLocatedLeft = true;
        }

        var pool = needLocatedLeft ? mLeftBinders : mRightBinders;
        if(pool.Count > 0)
        {
            handle = pool.Pop();
        }
        else
        {
            var template = needLocatedLeft ? go_left_chat_binder : go_right_chat_binder;
            var go = Object.Instantiate(template, Handle.transform,true) as GameObject;
            //go.transform.localPosition = Vector3.zero;
            var listener = UIEventListener.Get(go);
            handle = new UIChatBinder();
            handle.Mode = mChatListViewData.eMode;
            handle.Location = needLocatedLeft ? UIChatBinder.LocationType.LT_LEFT : UIChatBinder.LocationType.LT_RIGHT;
            handle.FaceTemplate = sp_face;
            handle.Setup(listener);
        }
        handle.Handle.CustomActive(true);
        handle.BindData(null, chatData);
        return handle;
    }//nihao[emoticon01]he[emoticon02]

    protected void RecycleChatBinder(UIChatBinder binder)
    {
        var pool = binder.Location == UIChatBinder.LocationType.LT_LEFT ? mLeftBinders : mRightBinders;
        binder.Handle.gameObject.SetActive(false);
        pool.Push(binder);
    }

    protected UIChatListViewData mChatListViewData;
    protected FixedCircleArray<ChatData> chatDatas;
    public override void Bind(object data)
    {
        var oldDatas = (mChatListViewData == null || mChatListViewData .oldChatDatas == null)? data as FixedCircleArray<ChatData> : mChatListViewData.oldChatDatas;
        mChatListViewData = data as UIChatListViewData;
        chatDatas = mChatListViewData.chatDatas;

        if (oldDatas != chatDatas)
        {
            //先回收
            for (int i = 0; i < mMergedBinders.Count; ++i)
            {
                RecycleChatBinder(mMergedBinders[i]);
            }
            mMergedBinders.Clear();
            //重建
            for (int i = Mathf.Max(0, chatDatas.Count - MAX_MERGED_BINDER_COUNTS); i < chatDatas.Count; ++i)
            {
                var chatData = chatDatas[i];
                var chatBinder = LoadChatBinder(chatData);
                mMergedBinders.Append(chatBinder);
            }
        }
        else
        {
            mCahedLogicBinders.Clear();
            //回收至缓存
            for (int i = 0; i < mMergedBinders.Count; ++i)
            {
                var binder = mMergedBinders[i];
                if(!binder.Value.IsLinkedChannedl(mChatListViewData.eChatType))
                {
                    //如果此消息已经被无情地回收了
                    RecycleChatBinder(mMergedBinders[i]);
                }
                else
                {
                    //如果此消息还有效 放入临时缓存
                    mCahedLogicBinders.Add(binder.Value.guid,binder);
                }
            }
            //增量重建
            mMergedBinders.Clear();
            for (int i = Mathf.Max(0, chatDatas.Count - MAX_MERGED_BINDER_COUNTS); i < chatDatas.Count; ++i)
            {
                var chatData = chatDatas[i];
                UIChatBinder binder = null;
                if(mCahedLogicBinders.ContainsKey(chatDatas[i].guid))
                {
                    binder = mCahedLogicBinders[chatDatas[i].guid];
                    mCahedLogicBinders.Remove(chatDatas[i].guid);
                    mMergedBinders.Append(binder);
                }
                else
                {
                    binder = LoadChatBinder(chatData);
                    mMergedBinders.Append(binder);
                }
            }
            //回收临时缓存
            var it = mCahedLogicBinders.GetEnumerator();
            while(it.MoveNext())
            {
                RecycleChatBinder(it.Current.Value);
            }
        }
    }

    protected void Reposition()
    {
        mHeight = titleHeight;
        for (int i = 0; i < mMergedBinders.Count; i++)
        {
            var binder = mMergedBinders[i];
            //float x = binder.Location == UIChatBaseBinder.LocationType.LT_LEFT ? 0 : -12;
            Vector3 pos = new Vector3(binder.Handle.transform.localPosition.x, -mHeight, 0);
            mHeight += binder.Height;
            binder.Handle.transform.localPosition = pos;
        }
    }

    public override void OnDestroy()
    {
        while(mLeftBinders.Count > 0)
        {
            mLeftBinders.Pop().OnDestroy();
        }
        mLeftBinders = null;
        while (mRightBinders.Count > 0)
        {
            mRightBinders.Pop().OnDestroy();
        }
        mRightBinders = null;
        mMergedBinders.Dispose();
        mMergedBinders = null;
        go_left_chat_binder = null;
        go_right_chat_binder = null;
    }
}