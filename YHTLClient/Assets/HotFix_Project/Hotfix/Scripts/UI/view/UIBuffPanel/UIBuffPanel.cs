using System.Collections;
using fight;
using System.Collections.Generic;
using UnityEngine;

public partial class UIBuffPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    CSBuffInfo buffInfo;
    List<BufferInfo> listBuffInfo = new List<BufferInfo>();
    List<UIItemBuff> listItemBuff = new List<UIItemBuff>();
    List<Schedule> listSchedule = new List<Schedule>();

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.Buff_Add, RefreshData);
        mClientEvent.Reg((uint) CEvent.Buff_Remove, RefreshData);
        mbtn_close.onClick = Close;
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void RefreshData(uint id, object data)
    {
        InitData();
    }

    void InitData()
    {
        buffInfo = CSMainPlayerInfo.Instance.BuffInfo;
        mobj_nonbuff.SetActive(false);
        mgrid_buff.gameObject.SetActive(true);
        listBuffInfo.Clear();

        if(buffInfo.buffIdList != null)
        {
            for(int i = 0; i < buffInfo.buffIdList.Count; ++i)
            {
                if (BufferTableManager.Instance.GetBufferShowOrHide(buffInfo.buffIdList[i]) == 1)
                {
                    BufferInfo info = buffInfo.GetBuff(buffInfo.buffIdList[i]);
                    if(info != null)
                    {
                        listBuffInfo.Add(info);
                    }
                }
            }
        }

        if (listBuffInfo.Count == 0)
        {
            mobj_nonbuff.SetActive(true);
            mgrid_buff.gameObject.SetActive(false);
            msp_bg.height = 177;
        }
        else
        {
            mgrid_buff.MaxCount = listBuffInfo.Count;
            GameObject gp;
            int height = 0;
            int allheight = 0;
            Vector3 initPosition = Vector3.zero;
            UIItemBuff itemBuff;
            for (int i = 0; i < listItemBuff.Count; i++)
            {
                if (Timer.Instance.IsInvoking(listItemBuff[i].itemSchedule))
                    Timer.Instance.CancelInvoke(listItemBuff[i].itemSchedule);
            }

            listItemBuff.Clear();
            for (int i = 0; i < mgrid_buff.MaxCount; i++)
            {
                gp = mgrid_buff.controlList[i];

                if (listItemBuff.Count <= i)
                {
                    itemBuff = new UIItemBuff(gp);
                    listItemBuff.Add(itemBuff);
                }

                listItemBuff[i].Refresh(listBuffInfo[i]);
                listItemBuff[i].ShowOrHideLine(!(i == mgrid_buff.MaxCount-1));

                if (listItemBuff[i].itemSchedule != null)
                {
                    listSchedule.Add(listItemBuff[i].itemSchedule);
                }

                //自适应高度
                if (i != 0)
                {
                    gp.transform.localPosition = initPosition - Vector3.up * height;
                }

                initPosition = gp.transform.localPosition;
                height = listItemBuff[i].Height;
                allheight += height;
            }

            int difference = 0;
            if (allheight <= 500)
            {
                difference = allheight - msp_bg.height;
                msp_bg.height = allheight;
                mgrid_buff.transform.localPosition += difference * Vector3.up;
            }
            else
            {
                difference = 500 - msp_bg.height;
                msp_bg.height = 500;
                mgrid_buff.transform.localPosition += difference * Vector3.up;
            }

            CSGame.Sington.StartCoroutine(SetScrollValue());
        }
    }

    IEnumerator SetScrollValue()
    {
        yield return null;
        mScrollView_buff.ResetPosition();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i < listItemBuff.Count; i++)
        {
            if (Timer.Instance.IsInvoking(listItemBuff[i].itemSchedule))
                Timer.Instance.CancelInvoke(listItemBuff[i].itemSchedule);
        }

        listItemBuff.Clear();
    }
}