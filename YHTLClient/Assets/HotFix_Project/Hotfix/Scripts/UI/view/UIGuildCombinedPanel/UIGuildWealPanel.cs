using Google.Protobuf.Collections;
using MapEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGuildWealPanel : UIBasePanel
{
    private float mfAnimPlayTime = 0.5f;
    //private float mfOpenPanelTime = 0.5f;
    private int nGoldMinNum = 1000;
    private int nGoldMaxNum = 100000; //金子单位文，显示单位两
    private int nRedPacketMinNum = 1;
    private int nRedPacketMaxNum = 50;
    private const int sTips1 = 973;
    private const int sTips2 = 974;
    private const int sTips3 = 975;
    private const int sTips4 = 976;
    private bool isShowGetAnim = true;
    private int moneyId = (int)MoneyType.yuanbao;

    public override void Init()
    {
        base.Init();

        mClientEvent.AddEvent(CEvent.OnRecievedRedPackges, OnResGetRedPacket);
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mBtnSendRedPacket.onClick = OnClickSendRedPacket;
        mBtnCloseSendRedPacket.onClick = p => { CloseSendRedPacketPanel(); };
        UIEventListener.Get(mGoSendRedPacketBg.gameObject).onClick = p => { CloseSendRedPacketPanel(); };
        mBtnMakeSureSendRedPacket.onClick = OnClickMakeSureSendRedPacket;
        mBtnOpenRedPacketClose.onClick = p => { CloseOpenRedPacketPanel(); };
        UIEventListener.Get(mGoOpenRedPacketBg.gameObject).onClick = p => { CloseOpenRedPacketPanel(); };
        mBtnAddGold.onClick = OnClickAddGold;
        mBtnReduceGold.onClick = OnClickReduceGold;
        mBtnAddNum.onClick = OnClickAddNum;
        mBtnReduceNum.onClick = OnClickReduceNum;

        SetInputGold(nGoldMinNum);
        SetInputPacketNum(10);
        EventDelegate.Add(minput_gold_num.onChange, OnRefreshGoldCount);
        minput_gold_num.onValidate = OnValidateInput;
        EventDelegate.Add(minput_package_num.onChange, OnRefreshPacketCount);
        minput_package_num.onValidate = OnValidateInput;

        if (null != mMoneyName)
            mMoneyName.text = moneyId.ItemName();

        if (null != mMoneyIcon)
            mMoneyIcon.spriteName = moneyId.SmallIcon();

        mRedPkgInfos.Clear();

        CSGuildInfo.Instance.Tab = UnionTab.SouvenirWealthPacks;
        //mInputContent.CenterChild();
    }

    char OnValidateInput(string text, int pos, char ch)
    {
        if (ch >= '0' && ch <= '9') return ch;
        return (char)0;
    }

    public override void Show()
    {
        base.Show();
        nGoldMinNum = CSGuildInfo.Instance.RedGoldMinValue;
        nGoldMaxNum = CSGuildInfo.Instance.RedGoldMaxValue;
        nRedPacketMinNum = CSGuildInfo.Instance.RedPacketMinNum;
        nRedPacketMaxNum = CSGuildInfo.Instance.RedPacketMaxNum;

        Net.CSGetUnionTabMessage((int)UnionTab.SouvenirWealthPacks);
        Refresh(mRedPkgInfos);
    }

    void Refresh(FastArrayElementKeepHandle<union.UnionRedPackageInfo> unionSouvenirQianPackInfos)
    {
        if (unionSouvenirQianPackInfos.Count == 0)
        {
            mNoRedPackets.CustomActive(true);
            CSEffectPlayMgr.Instance.ShowUITexture(mTexture, "pattern");
        }
        else
        {
            mNoRedPackets.CustomActive(false);
            unionSouvenirQianPackInfos.Sort(CSGuildInfo.Instance.SortRedPacketList);
            RefreshRedPacketListUI(unionSouvenirQianPackInfos);
        }
    }

    private void RefreshRedPacketListUI(FastArrayElementKeepHandle<union.UnionRedPackageInfo> unionSouvenirianPackInfos)
    {
        mGridRedPacket.MaxCount = unionSouvenirianPackInfos.Count;
        for (int i = 0; i < unionSouvenirianPackInfos.Count; i++)
        {
            union.UnionRedPackageInfo info = unionSouvenirianPackInfos[i];
            UITexture texBg = mGridRedPacket.controlList[i].transform.Find("bg").GetComponent<UITexture>();
            UISprite sprHead = mGridRedPacket.controlList[i].transform.Find("Head/icon").GetComponent<UISprite>();
            GameObject goHead = mGridRedPacket.controlList[i].transform.Find("Head").gameObject;
            UILabel labName = mGridRedPacket.controlList[i].transform.Find("lb_name").GetComponent<UILabel>();
            UILabel labNum = mGridRedPacket.controlList[i].transform.Find("lb_num").GetComponent<UILabel>();
            UILabel labPacketNum = mGridRedPacket.controlList[i].transform.Find("lab_PacketNum").GetComponent<UILabel>();
            GameObject goOpenRedPacket = mGridRedPacket.controlList[i].transform.Find("btn_rob").gameObject;
            GameObject goGet = mGridRedPacket.controlList[i].transform.Find("lab_Get").gameObject;
            GameObject goEmpty = mGridRedPacket.controlList[i].transform.Find("lab_Empty").gameObject;

            string texName = (info.haveTaken || info.empty) ? "guild_bag_mini2" : "guild_bag_mini1";

            CSEffectPlayMgr.Instance.ShowUITexture(texBg.gameObject, texName);

            sprHead.spriteName = Utility.GetPlayerIcon(info.sex,info.career);

            goHead.CustomActive(info.haveTaken || info.empty);

            labName.text = info.name;

            labNum.text = $"{info.totalWealth}";

            labPacketNum.text = string.Empty;

            goOpenRedPacket.CustomActive(!info.haveTaken && !info.empty);
            goGet.SetActive(info.haveTaken);
            goEmpty.SetActive(!info.haveTaken && info.empty);
            mGridRedPacket.controlList[i].GetComponent<UIWidget>().alpha = (info.haveTaken || info.empty) ? 0.8f : 1f;

            UIEventListener.Get(goOpenRedPacket, info).onClick = OnClickGetRedPacket;

            if (info.haveTaken || info.empty)
                UIEventListener.Get(texBg.gameObject, info).onClick = OnClickShowRedPacketDetail;
            else
                UIEventListener.Get(texBg.gameObject, info).onClick = OnClickGetRedPacket;
        }
    }

    FastArrayElementKeepHandle<union.UnionRedPackageInfo> mRedPkgInfos = new FastArrayElementKeepHandle<union.UnionRedPackageInfo>(16);
    #region Event
    protected void OnGuildTabDataChanged(uint id,object argv)
    {
        if(argv is UnionTab tab && tab == UnionTab.SouvenirWealthPacks)
        {
            var tabInfo = CSGuildInfo.Instance.GetTabInfo(tab);
            if(null != tabInfo)
            {
                mRedPkgInfos.Clear();
                for (int i = 0; i < tabInfo.redPackageInfos.Count; ++i)
                    mRedPkgInfos.Append(tabInfo.redPackageInfos[i]);
                Refresh(mRedPkgInfos);
            }
        }
    }

    private void OnClickGetRedPacket(GameObject go)
    {
        isShowGetAnim = true;
        union.UnionRedPackageInfo info = go.GetComponent<UIEventListener>().parameter as union.UnionRedPackageInfo;
        RefreshAnimUI(info);
        Net.CSGetSouvenirWealthMessage(info.id);
    }

    private void OnClickShowRedPacketDetail(GameObject go)
    {
        isShowGetAnim = false;
        union.UnionRedPackageInfo info = go.GetComponent<UIEventListener>().parameter as union.UnionRedPackageInfo;
        Net.CSGetSouvenirWealthMessage(info.id);
    }

    private void CloseSendRedPacketPanel()
    {
        SetSendRedPacketPanelVisible(false);
    }

    private void CloseOpenRedPacketPanel()
    {
        mOpenEffect.gameObject.SetActive(false);
        SetOpenRedPacketPanelVisible(false);
    }

    private void OnClickSendRedPacket(GameObject go)
    {
        if (CSGuildInfo.Instance.IsCanSendRedPacket())
        {
            SetSendRedPacketPanelVisible(true);
        }
        else
        {
            UtilityTips.ShowRedTips(sTips4);
        }
    }

    private void OnClickMakeSureSendRedPacket(GameObject go)
    {
        int gold = GetInputGoldNum();
        int num = GetInputPacketNum();

        long owned = CSItemCountManager.Instance.GetItemCount(moneyId);
        if (gold > owned)
        {
            Utility.ShowGetWay(moneyId);
            return;
        }

        if (gold < CSGuildInfo.Instance.RedGoldMinValue) 
        {
            UtilityTips.ShowTips(895, 1.5f, ColorType.Yellow, CSGuildInfo.Instance.RedGoldMinValue,moneyId.ItemName()); 
            return; 
        }

        if (gold > CSGuildInfo.Instance.RedGoldMaxValue)
        {
            UtilityTips.ShowTips(896, 1.5f, ColorType.Yellow, CSGuildInfo.Instance.RedGoldMaxValue, moneyId.ItemName());
            return; 
        }

        if (num < CSGuildInfo.Instance.RedPacketMinNum) 
        {
            UtilityTips.ShowTips(897, 1.5f, ColorType.Yellow, CSGuildInfo.Instance.RedPacketMinNum);
            return; 
        }

        if (num > CSGuildInfo.Instance.RedPacketMaxNum)
        { 
            UtilityTips.ShowTips(898, 1.5f, ColorType.Yellow, CSGuildInfo.Instance.RedPacketMaxNum);
            return; 
        }

        Net.CSSendSouvenirWealthMessage(gold, num);
        CloseSendRedPacketPanel();
    }

    private void OnClickAddGold(GameObject go)
    {
        int nGold = GetInputGoldNum();

        nGold += nGoldMinNum;
        if (nGold <= nGoldMaxNum)
        {
            SetInputGold(nGold);
        }
        else
        {
            UtilityTips.ShowRedTips(sTips2);
            SetInputGold(nGoldMaxNum);
        }
    }

    private void OnClickReduceGold(GameObject go)
    {
        int nGold = GetInputGoldNum();

        nGold -= nGoldMinNum;
        if (nGold >= nGoldMinNum)
            SetInputGold(nGold);
    }

    private void OnClickAddNum(GameObject go)
    {
        int nPacketNum = GetInputPacketNum();

        nPacketNum += nRedPacketMinNum;
        if (nPacketNum <= nRedPacketMaxNum)
        {
            SetInputPacketNum(nPacketNum);
        }
        else
        {
            UtilityTips.ShowRedTips(sTips3);
            SetInputPacketNum(nRedPacketMaxNum);
        }
    }

    private void OnClickReduceNum(GameObject go)
    {
        int nPacketNum = GetInputPacketNum();

        nPacketNum -= nRedPacketMinNum;
        if (nPacketNum >= nRedPacketMinNum)
        {
            SetInputPacketNum(nPacketNum);
        }
    }

    private int GetInputGoldNum()
    {
        int nGold = 0;
        int.TryParse(minput_gold_num.value, out nGold);

        return nGold;
    }

    private void SetInputGold(int nGold)
    {
        minput_gold_num.value = nGold.ToString();
    }

    private int GetInputPacketNum()
    {
        int nPacketNum;
        int.TryParse(minput_package_num.value, out nPacketNum);

        return nPacketNum;
    }

    private void SetInputPacketNum(int nNum)
    {
        minput_package_num.value = $"{nNum}";
    }

    private void OnRefreshGoldCount()
    {
        if (string.IsNullOrEmpty(minput_gold_num.value)) return;

        int nGold = GetInputGoldNum();

        if (nGold > nGoldMaxNum)
        {
            UtilityTips.ShowRedTips(sTips2);
            SetInputGold(nGoldMaxNum);
        }

        //mInputContent.CenterChild();
    }

    private void OnRefreshPacketCount()
    {
        if (string.IsNullOrEmpty(minput_package_num.value)) return;

        int nPacketNum = GetInputPacketNum();

        if (nPacketNum > nRedPacketMaxNum)
        {
            UtilityTips.ShowRedTips(sTips3);
            SetInputPacketNum(nRedPacketMaxNum);
        }
    }

    union.UnionRedPackageDetailInfo mDetailInfo;
    private void OnResGetRedPacket(uint eventId, object argv)
    {
        mDetailInfo = argv as union.UnionRedPackageDetailInfo;

        if (isShowGetAnim)
            SetRedPacketAnimIsShow(true);
        else
            ShowOpenRedPacketPanel();
    }
    #endregion

    private void RefreshRedPacketDetailPanel(union.UnionRedPackageDetailInfo info)
    {
        if (info == null) return;
        UISprite sprHead = mGoOpenRedPacketPanel.transform.Find("Head/icon").GetComponent<UISprite>();
        UILabel labSendName = mGoOpenRedPacketPanel.transform.Find("name").GetComponent<UILabel>();
        UILabel labGoldNum = mGoOpenRedPacketPanel.transform.Find("DrawedIngot").GetComponent<UILabel>();
        UILabel labGetRedPacketNum = mGoOpenRedPacketPanel.transform.Find("RedPackrec").GetComponent<UILabel>();
        UILabel labGetGoldNum = mGoOpenRedPacketPanel.transform.Find("RedPackhave").GetComponent<UILabel>();
        GameObject goRedPacketOver = mGoOpenRedPacketPanel.transform.Find("redPacketOver").gameObject;

        labGoldNum.gameObject.SetActive(!(info.wealth == 0 && info.curNumber == info.totalNumber));
        goRedPacketOver.gameObject.SetActive(info.wealth == 0 && info.curNumber == info.totalNumber);

        sprHead.spriteName = Utility.GetPlayerIcon(info.sex, info.career);
        labSendName.text = info.senderName;
        labGoldNum.text = $"{info.wealth}";
        mlb_Igot.transform.localPosition = new Vector3(-labGoldNum.localSize.x / 3, mlb_Igot.transform.localPosition.y, mlb_Igot.transform.localPosition.z); //为了红包界面金子居中处理

        labGetRedPacketNum.text = CSString.Format(894, info.curNumber, info.totalNumber);

        labGetGoldNum.text = "";

        UIGridContainer grid = mGoOpenRedPacketPanel.transform.Find("OpenRedPackScrollView/GridList").GetComponent<UIGridContainer>();
        grid.MaxCount = info.infos.Count;

        for (int i = 0; i < info.infos.Count; i++)
        {
            var tempInfo = info.infos[i];
            UISprite sprTempHead = grid.controlList[i].transform.Find("Head/icon").GetComponent<UISprite>();
            UILabel labTempGold = grid.controlList[i].transform.Find("num").GetComponent<UILabel>();
            UILabel labTempName = grid.controlList[i].transform.Find("name").GetComponent<UILabel>();
            UILabel labTempTime = grid.controlList[i].transform.Find("time").GetComponent<UILabel>();
            DateTime sendTime = CSServerTime.StampToDateTime(tempInfo.time);

            sprTempHead.spriteName = Utility.GetPlayerIcon(tempInfo.sex, tempInfo.career);
            labTempGold.text = $"{tempInfo.wealth}";
            labTempName.text = tempInfo.name;

            CSStringBuilder.Clear();
            labTempTime.text = CSStringBuilder.Append(sendTime.Month, "-", sendTime.Day, " ", string.Format("{0:00}", sendTime.Hour), ":", string.Format("{0:00}", sendTime.Minute)).ToString();
        }
    }

    private void SetSendRedPacketPanelVisible(bool isShow)
    {
        mGoSendRedPacketPanel.SetActive(isShow);
        if (isShow)
        {
            UITexture tex = mGoSendRedPacketPanel.transform.Find("bgs/tex_bag").GetComponent<UITexture>();
            CSEffectPlayMgr.Instance.ShowUITexture(tex.gameObject, "guild_bag_bg");
            SetInputGold(nGoldMinNum);
            SetInputPacketNum(10);
        }
    }

    private void SetOpenRedPacketPanelVisible(bool isShow)
    {
        mGoOpenRedPacketPanel.SetActive(isShow);
        if (isShow)
        {
            UITexture tex = mGoOpenRedPacketPanel.transform.Find("tex_bag").GetComponent<UITexture>();
            CSEffectPlayMgr.Instance.ShowUITexture(tex.gameObject, "guild_bag");
        }
    }

    #region Effect

    private void SetRedPacketAnimIsShow(bool isShow)
    {
        mGoRedPacketAnim.SetActive(isShow);

        if (isShow)
        {
            mTweenScale.PlayTween();
            mTweenScale.onFinished.Clear();
            mTweenScale.onFinished.Add(new EventDelegate(OnScaleAnimFinish));
        }
    }

    private void RefreshAnimUI(union.UnionRedPackageInfo info)
    {
        UITexture texBg = mGoRedPacketAnim.transform.Find("bg").GetComponent<UITexture>();
        UILabel labName = mGoRedPacketAnim.transform.Find("lb_name").GetComponent<UILabel>();
        UILabel labNum = mGoRedPacketAnim.transform.Find("lb_num").GetComponent<UILabel>();
        UILabel labPacketNum = mGoRedPacketAnim.transform.Find("lab_PacketNum").GetComponent<UILabel>();

        CSEffectPlayMgr.Instance.ShowUITexture(texBg.gameObject, "guild_bag_mini1");

        labName.text = info.name;

        labNum.text = $"{info.totalWealth}";

        labPacketNum.text = CSString.Format(893,info.drewNum,info.totalNum);
    }

    private void OnScaleAnimFinish()
    {
        mTweenRotation.PlayTween();
        ScriptBinder.Invoke(mfAnimPlayTime, OnAnimFinish);
    }

    private void OnAnimFinish()
    {
        SetRedPacketAnimIsShow(false);
        mGoRedPacketAnim.transform.localRotation = new Quaternion(0, 0, 0, 0);

        ShowOpenRedPacketPanel();
        mOpenEffect.CustomActive(true);
        CSEffectPlayMgr.Instance.ShowParticleEffect(mOpenEffect, "fx_lqhongbao",position: new Vector3(20, 34, 0));
    }

    private void ShowOpenRedPacketPanel()
    {
        if (mDetailInfo != null)
        {
            SetOpenRedPacketPanelVisible(true);
            RefreshRedPacketDetailPanel(mDetailInfo);
        }
    }
    #endregion

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mTexture);

        if (null != mRedPkgInfos)
        {
            mRedPkgInfos.Clear();
            mRedPkgInfos = null;
        }
        mClientEvent.RemoveEvent(CEvent.OnRecievedRedPackges, OnResGetRedPacket);
        mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        base.OnDestroy();
    }
}