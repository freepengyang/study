using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class UIFriendResponseTipsPanel : UIBasePanel
{
    private UIFriendResponseTipType m_panelType = UIFriendResponseTipType.AddResponse;
    private FastArrayElementFromPool<UIItemFriendShortInfoData> mApplyList;

    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Tips;
        }
    }

    public override void Init()
    {
        AddCollider();
        base.Init();

        mApplyList = new FastArrayElementFromPool<UIItemFriendShortInfoData>(32, () =>
         {
             return mPoolHandleManager.GetSystemClass<UIItemFriendShortInfoData>();
         }, f =>
         {
             mPoolHandleManager.Recycle(f);
         });
        mClientEvent.AddEvent(CEvent.OnApplyListChanged, OnApplyListChanged);
        //mClientEvent.AddEvent(CEvent.ReqAddFriendList, OnResRecommendFriendsMessage);
        mBtnClose.onClick = this.OnClickClose;
        mBtnAddAll.onClick = this.OnClickAddAll;
        mBtnRejectAll.onClick = f =>
        {
            Net.ReqRejectRelationsMessage();
            OnClickClose(null);
        };

        //Net.ReqGetRecommendFriendsMessage();
    }

    protected override void OnDestroy()
    {
        mApplyList?.Clear();
        mApplyList = null;
        mClientEvent.RemoveEvent(CEvent.OnApplyListChanged, OnApplyListChanged);
        base.OnDestroy();
    }

    public override void Show()
    {
        base.Show();
        Show(UIFriendResponseTipType.AddResponse);
    }

    void Show(UIFriendResponseTipType panelType)
    {
        m_panelType = panelType;
        if (m_panelType == UIFriendResponseTipType.Recommend)
        {
            InitTitleAndTips(CSString.Format(573), CSString.Format(574));
        }
        else if (m_panelType == UIFriendResponseTipType.AddResponse)
        {
            InitTitleAndTips(CSString.Format(575), CSString.Format(576));
        }
        RefreshApplyList();
    }

    protected void RefreshApplyList()
    {
        if(null != mfriendItemList)
        {
            mApplyList.Clear();
            var list = CSFriendInfo.Instance.ApplyList();
            mApplyList.Count = list.Count;
            for(int i = 0; i < list.Count; ++i)
            {
                var applyInfo = mApplyList[i];
                applyInfo.canChat = false;
                applyInfo.Info = list[i];
                applyInfo.m_call = OnItemClickCallBack;
                applyInfo.m_refusecall = OnItemClickRejectCallBack;
                applyInfo.number = i;
            }
            mfriendItemList.Bind<UIItemFriendShortInfoBinder,UIItemFriendShortInfoData>(mApplyList);
        }
    }

    private void InitTitleAndTips(string title, string tip)
    {
        if(null != mtitle)
            mtitle.text = "[fbd671]" + title;
        if(null != mtip)
            mtip.text = "[e8a657]" + tip;
    }

    #region event
    private void OnResRecommendFriendsMessage(uint id, params object[] data)
    {
        RefreshApplyList();
    }

    //单个添加好友
    private void OnItemClickCallBack(long roleId, string fariendName)
    {
        Net.ReqAddRelationMessage(roleId.ToGoogleList(),(int)FriendType.FT_FRIEND);
    }
    //申请列表 -> 拒绝添加好友
    private void OnItemClickRejectCallBack(long roleId, string fariendName)
    {
        Net.RejectSingleReqMessage(roleId);
    }

    private void OnApplyListChanged(uint id, object argv)
    {
        if(CSFriendInfo.Instance.ApplyList().Count > 0)
            RefreshApplyList();
        else
            this.OnClickClose(null);
    }

    //一键添加
    private void OnClickAddAll(GameObject btn)
    {
        var roleIdList = CSNetRepeatedFieldPool.Get<long>();
        roleIdList.Clear();
        var applyList = CSFriendInfo.Instance.ApplyList();
        for (int i = 0; i < applyList.Count; i++)
        {
            roleIdList.Add(applyList[i].roleId);
        }
        Net.ReqAddRelationMessage(roleIdList,1);
        OnClickClose(null);
    }

    private void OnClickClose(GameObject btn)
    {
        UIManager.Instance.ClosePanel<UIFriendResponseTipsPanel>();
        CSFriendInfo.Instance.ClearApplyList();
    }
    #endregion

    public enum UIFriendResponseTipType
    {
        Recommend = 1,      //好友推荐
        AddResponse = 2,    //好友添加响应
    }
}
