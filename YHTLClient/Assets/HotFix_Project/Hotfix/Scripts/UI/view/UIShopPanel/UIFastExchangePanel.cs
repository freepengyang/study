using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ŀǰ�߻���û�и���ͨ�ù���������ֻ�����ܶһ�ȯ�һ�����ȯ��һ���������Ŀǰд��
/// </summary>
public partial class UIFastExchangePanel : UIBasePanel
{

    ILBetterList<GetWayData> getWayList = new ILBetterList<GetWayData>();

    /// <summary>
    /// ��ǰ����
    /// </summary>
    private int CurCount;

    /// <summary>
    /// ��������
    /// </summary>
    private int CountLimit;

    TABLE.ITEM itemOrigin;
    TABLE.ITEM exchangeTarget;


    public override void Init()
	{
		base.Init();
        AddCollider();

        mClientEvent.AddEvent(CEvent.FastAccessJumpToPanel, CloseEvent);
        mClientEvent.AddEvent(CEvent.FastAccessTransferNpc, CloseEvent);

        mbtn_close.onClick = Close;
        mbtn_add.onClick = AddClick;
        mbtn_sub.onClick = SubClick;
        minput_num.onChange.Add(new EventDelegate(CountInputChanged));
        mbtn_exchange.onClick = ExchangeBtnClick;
    }
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
        mgrid_bottom.UnBind<GetWayBtn>();
        getWayList?.Clear();
        itemOrigin = null;
        exchangeTarget = null;

        base.OnDestroy();
	}


    /// <summary>
    /// ���ܶһ�ȯ�һ�������ȯ
    /// </summary>
    /// <param name="target"></param>
    public void UniversalTicketExchange(TABLE.ITEM universal, TABLE.ITEM target)
    {
        if (universal == null || target == null || string.IsNullOrEmpty(target.getWay)) return;

        int universalCount = (int)universal.id.GetItemCount();

        itemOrigin = universal;
        exchangeTarget = target;

        mlb_itemName.text = universal.name;
        mlb_itemNum.text = universalCount.ToString().BBCode(universalCount < 1 ? ColorType.Red : ColorType.MainText);

        mlb_hint1.text = $"1{universal.name}";
        mlb_hint2.text = $"1{target.name}";

        CurCount = universalCount > 0 ? 1 : 0;
        CountLimit = universalCount;

        minput_num.value = CurCount.ToString();

        GetGetWaysInfo(target.getWay);
    }


    /// <summary>
    /// ͨ��GetWay��id�ַ�����ȡGetWay�б�
    /// </summary>
    /// <param name="idStr">GetWay��id�ַ�����ͨ��#����</param>
    /// <returns></returns>
    void GetGetWaysInfo(string idStr)
    {
        if (getWayList == null) getWayList = new ILBetterList<GetWayData>();
        else getWayList.Clear();

        CSGetWayInfo.Instance.GetGetWays(idStr, ref getWayList);

        mgrid_bottom.Bind<GetWayData, GetWayBtn>(getWayList, mPoolHandleManager);

        int offset = getWayList.Count >= 4 ? 0 : (int)mgrid_bottom.CellHeight * (4 - getWayList.Count);
        msp_bg.height = 502 - offset;
    }


    void AddClick(GameObject go)
    {
        CurCount = CurCount >= CountLimit ? CountLimit : CurCount + 1;
        minput_num.value = CurCount.ToString();
    }


    void SubClick(GameObject go)
    {
        CurCount = CurCount <= 1 ? 1 : CurCount - 1;
        minput_num.value = CurCount.ToString();
    }

    void CountInputChanged()
    {
        if (string.IsNullOrEmpty(minput_num.value)) return;
        int count = 0;
        if (!int.TryParse(minput_num.value, out count) || count < 0)
        {
            return;
        }

        if (count > CountLimit) count = CountLimit;

        minput_num.value = count.ToString();
        CurCount = count;

    }


    void ExchangeBtnClick(GameObject go)
    {
        if (itemOrigin == null) return;

        if (itemOrigin.id.GetItemCount() < 1)
        {
            Utility.ShowGetWay(itemOrigin.id);
            //Close();
            return;
        }

        Net.CSDuiHuanQuanMessage(exchangeTarget.id, CurCount);
        Close();
    }


    void CloseEvent(uint id, object param)
    {
        Close();
    }

}
