using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MailData
{
    public int configId;
    public int count;
    public int endTime;
}
public partial class UIMailPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();

        mBtnOneKeyDelete.onClick = OnOneKeyDelete;
        mBtnDelete.onClick = OnDelete;
        mBtnOneKeyAcquire.onClick = OnOneKeyAcquire;
        mBtnAcquire.onClick = OnAcquire;
        if (CSMailManager.Instance.DebugMode)
        {
            mBtnSendMail.onClick = OnSendMail;
        }
        else
        {
            mBtnSendMail.onClick = OnSendMail;
            mBtnSendMail.gameObject.SetActive(false);
        }

        if (CSMailManager.Instance.DebugMode)
        {
            mBtnRecreate.onClick = OnRebuild;
        }
        else
        {
            mBtnRecreate.gameObject.SetActive(false);
        }
        mAwardItems = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mAwardParent.transform);
        mItemDatas = mPoolHandleManager.CreateGeneratePool<MailData>(8);

        CSEffectPlayMgr.Instance.ShowUITexture(mTexture, "pattern");

        mClientEvent.AddEvent(CEvent.MailListChanged, OnMailListChanged);
        mClientEvent.AddEvent(CEvent.OnMailStateChanged, OnMailStateChanged);

        mMailContent.SetupLink();
    }

    protected void RefreshMailListItems()
    {
        var SortedMailList = CSMailManager.Instance.SortedMailList;

        mAllMailRedPoint.SetActive(CSMailManager.Instance.HasUnAcquiredMail);
        mMailCount.text = string.Format(mMailCount.FormatStr,SortedMailList.Count, CSMailManager.Instance.MaxMailCount);

        long focusId = 0;
        if (null != MailItemData.FocusItem)
        {
            focusId = MailItemData.FocusItem.data.mailId;
            if (!CSMailManager.Instance.Mails.ContainsKey(focusId))
            {
                focusId = 0;
            }
            MailItemData.FocusItem = null;
        }

        if(focusId == 0 && SortedMailList.Count > 0)
        {
            focusId = (SortedMailList[0] as mail.MailInfo).mailId;
        }

        if (SortedMailList.Count == 0)
        {
            ScriptBinder._SetAction("Empty");
            OnMailSelected(null);
        }
        else
        {
            ScriptBinder._SetAction("NotEmpty");
        }

        CSMailManager.Instance.MailItemDataPool.RecycleAllItems();
        mGridMailContainer.MaxCount = SortedMailList.Count;
        for (int i = 0; i < mGridMailContainer.controlList.Count; ++i)
        {
            var mailInfo = SortedMailList[i] as mail.MailInfo;
            var mailData = CSMailManager.Instance.MailItemDataPool.Get();
            mailData.onMailItemSelected = this.OnMailSelected;
            mailData.OnItemVisible(mGridMailContainer.controlList[i], mailInfo, focusId == mailInfo.mailId);
        }
    }

    protected void RefreshMailContainedItems(MailItemData itemData)
    {
        var content = string.Empty;
        if (null != itemData)
        {
            content = itemData.data.items;
        }

        bool hasFetched = null != itemData && itemData.HasFetched;
        mAwardStatus.SetActive(hasFetched);

        mItemDatas.Clear();

        if (!string.IsNullOrEmpty(content))
        {
            var tokens = content.Split(';');
            for (int i = 0; i < tokens.Length; ++i)
            {
                var values = tokens[i].Split('#');
                int cfgId = 0;
                int num = 0;
                int endTime = 0;
                if (values.Length >= 2)
                {
                    if (!int.TryParse(values[0], out cfgId))
                        continue;
                    if (!int.TryParse(values[1], out num))
                        continue;
                }
                if (values.Length >= 3)
                {
                    if (!int.TryParse(values[2], out endTime))
                        continue;
                }
                var data = mItemDatas.Append();
                data.configId = cfgId;
                data.count = num;
                data.endTime = endTime;
            }
        }

        //不足六个的补齐六个
        if (mItemDatas.Count < 6)
        {
            int idx = mItemDatas.Count;
            mItemDatas.Count = 6;
            for (int i = idx; i < mItemDatas.Count; ++i)
            {
                mItemDatas[i].configId = 0;
                mItemDatas[i].count = 0;
                mItemDatas[i].endTime = 0;
            }
        }
        mAwardItems.Count = mItemDatas.Count;

        for (int i = 0; i < mAwardItems.Count; ++i)
        {
            var data = mItemDatas[i];
            if(data.configId == 0)
            {
                mAwardItems[i].UnInit();
            }
            else
            {
                mAwardItems[i].Refresh(data.configId);
                mAwardItems[i].Gray(hasFetched);
                mAwardItems[i].SetCount(data.count);
            }
        }

        mgrid_award.Reposition();
        mScrollView.ResetPosition();
        mgrid_award.repositionNow = true;
    }

    protected FastArrayElementFromPool<UIItemBase> mAwardItems;
    protected FastArrayElementFromPool<MailData> mItemDatas;

    protected void OnMailSelected(MailItemData data)
    {
        mBtnDelete.CustomActive(null != data);
        mBtnAcquire.CustomActive(null != data);
        if (null != data)
        {
            mMailContent.text = data.data.content;
            mMailTheme.text = data.data.title;
            bool hasItem = CSMailManager.Instance.HasUnAcquiredItemsInMail(data.data);
            mOneMailRedPoint.SetActive(hasItem);
            mRightDesc.CustomActive(false);
        }
        else
        {
            mMailContent.text = string.Empty;
            mMailTheme.text = string.Empty;
            mOneMailRedPoint.SetActive(false);
            mRightDesc.CustomActive(true);
        }
        mContentScrollView.ScrollImmidate(0);
        RefreshMailContainedItems(data);
    }

    protected void OnMailListChanged(uint eventId,object args)
    {
        RefreshMailListItems();
        mMailScrollView.ResetPosition();
    }

    protected void OnMailStateChanged(uint eventId, object args)
    {
        RefreshMailListItems();
    }

    protected void OnRevedNewMail(uint eventId, object args)
    {

    }

    protected void OnOneKeyDelete(GameObject go)
    {
        if(!CSMailManager.Instance.DeleteAllMails(mPoolHandleManager))
        {
            UtilityTips.ShowRedTips(340);
            //CSMailManager.Instance.LogFormat("当前没有可删除邮件");
        }
    }

    protected void OnDelete(GameObject go)
    {
        var data = MailItemData.FocusItem;
        if (null != data && null != data.data)
        {
            if (CSMailManager.Instance.HasUnAcquiredItemsInMail(data.data))
            {
                UtilityTips.ShowRedTips(341);
                //CSMailManager.Instance.LogFormat("该邮件未领取:id:{0} title:{1}", data.data.mailId, data.data.title);
                return;
            }
            CSMailManager.Instance.DeleteOneMail(data.data.mailId);
        }
        else
        {
            UtilityTips.ShowRedTips(340);
        }
    }

    protected void OnOneKeyAcquire(GameObject go)
    {
        if(!CSMailManager.Instance.AcquireAllMails(mPoolHandleManager))
        {
            UtilityTips.ShowRedTips(342);
            //CSMailManager.Instance.LogFormat("当前没有可领取附件");
            return;
        }
    }

    protected void OnAcquire(GameObject go)
    {
        var data = MailItemData.FocusItem;
        if (null != data && null != data.data)
        {
            if(!CSMailManager.Instance.HasUnAcquiredItemsInMail(data.data))
            {
                UtilityTips.ShowRedTips(342);
                //CSMailManager.Instance.LogFormat("当前没有可领取附件");
                return;
            }
            CSMailManager.Instance.AcquireOneMail(data.data.mailId);
        }
        else
        {
            UtilityTips.ShowRedTips(342);
        }
    }

    protected void OnSendMail(GameObject go)
    {
        int id = UnityEngine.Random.Range(100, 115);
        var command = string.Format($"@sendmail 111 1 80");
        FNDebug.Log(command);
        Net.GMCommand(command);
    }

    protected void OnRebuild(GameObject go)
    {
        CSMailManager.Instance.Initialize();
        RefreshMailListItems();
    }

    public override void Show()
    {
        base.Show();
        mScrollView.SetDynamicArrowHorizontal(mrArrow, mlArrow);
        MailItemData.FocusItem = null;
        RefreshMailListItems();
    }

    protected override void OnDestroy()
    {
        MailItemData.FocusItem = null;
        mAwardItems.Clear();
        mItemDatas.Clear();
        base.OnDestroy();
    }
}