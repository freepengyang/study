using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatBaseListViewData
{
    public ChatType eChatType;
    public FastArrayElementKeepHandle<ChatData> chatDatas;
}

public class UIChatBaseListViewBinder : UIBinder
{
    protected GameObject goTemplate;
    protected UISprite sp_face;

    protected const int MAX_BINDER_COUNTS = 8;
    protected Stack<UIChatBaseBinder> mPools = new Stack<UIChatBaseBinder>(MAX_BINDER_COUNTS);
    protected FixedCircleArray<UIChatBaseBinder> mMergedBinders = new FixedCircleArray<UIChatBaseBinder>(MAX_BINDER_COUNTS);
    protected Dictionary<int, UIChatBaseBinder> mCahedLogicBinders = new Dictionary<int, UIChatBaseBinder>(MAX_BINDER_COUNTS);
    public int TitleHeight
    {
        set
        {
            mPosYS = value;
            Reposition();
        }
    }
    protected int mHeight;

    public override void Init(UIEventListener handle)
    {
        goTemplate = Handle.transform.Find("uichat").gameObject;
        goTemplate.gameObject.SetActive(false);
        sp_face = Get<UISprite>("face");
        mHeight = 0;
    }

    protected UIChatBaseBinder LoadChatBinder(ChatData chatData)
    {
        UIChatBaseBinder handle = null;
        var pool = mPools;
        if (pool.Count > 0)
        {
            handle = pool.Pop();
        }
        else
        {
            var template = goTemplate;
            var go = Object.Instantiate(template, Handle.transform, true) as GameObject;
            go.transform.localPosition = Vector3.zero;
            var listener = UIEventListener.Get(go);
            handle = new UIChatBaseBinder();
            handle.Mode = UIChatBaseBinder.ChatVisibleMode.CVM_MAIN_PANEL;
            handle.Location = UIChatBinder.LocationType.LT_LEFT;
            handle.FaceTemplate = sp_face;
            handle.Setup(listener);
        }
        handle.BindData(null, chatData);
        handle.Handle.gameObject.SetActive(true);
        return handle;
    }
    
    protected void RecycleChatBinder(UIChatBaseBinder binder)
    {
        binder.Handle.gameObject.SetActive(false);
        mPools.Push(binder);
    }

    protected UIChatBaseListViewData mChatListViewData;
    protected ChatType mChatType = ChatType.CT_COMPREHENSIVE;

    public override void Bind(object data)
    {
        mChatListViewData = data as UIChatBaseListViewData;
        var chatDatas = mChatListViewData.chatDatas;

        mCahedLogicBinders.Clear();
        //回收至缓存
        for (int i = 0; i < mMergedBinders.Count; ++i)
        {
            var binder = mMergedBinders[i];
            if (!binder.Value.IsLinkedChannedl(mChatType))
            {
                //如果此消息已经被无情地回收了
                RecycleChatBinder(mMergedBinders[i]);
            }
            else
            {
                //如果此消息还有效 放入临时缓存
                mCahedLogicBinders.Add(binder.Value.guid, binder);
            }
        }
        //增量重建
        mMergedBinders.Clear();
        for (int i = Mathf.Max(0, chatDatas.Count - MAX_BINDER_COUNTS); i < chatDatas.Count; ++i)
        {
            var chatData = chatDatas[i];
            UIChatBaseBinder binder = null;
            if (mCahedLogicBinders.ContainsKey(chatDatas[i].guid))
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
        while (it.MoveNext())
        {
            RecycleChatBinder(it.Current.Value);
        }
    }

    protected int mPosYS = 0;
    protected int mPosYE = -106;
    protected int mLineOffsetY = 0;

    protected void Reposition()
    {
        bool reverse = false;
        int posy = mPosYS;
        for (int i = 0; i < mMergedBinders.Count; i++)
        {
            var binder = mMergedBinders[i];
            Vector3 pos = new Vector3(0, posy, 0);
            binder.Handle.transform.localPosition = pos;
            posy -= binder.Height;
            if (posy < mPosYE)
            {
                reverse = true;
                break;
            }
            posy -= mLineOffsetY;
        }

        if(reverse)
        {
            posy = mPosYE;
            for (int i = mMergedBinders.Count - 1; i >= 0; --i)
            {
                var binder = mMergedBinders[i];
                binder.Handle.transform.localPosition = new Vector3(0, posy += binder.Height, 0);
                posy += mLineOffsetY;
            }
        }
    }

    public override void OnDestroy()
    {
        while(mPools.Count > 0)
        {
            mPools.Pop().OnDestroy();
        }
        mPools = null;
        mMergedBinders.Dispose();
        mMergedBinders = null;
        goTemplate = null;
    }
}