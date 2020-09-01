using UnityEngine;

public partial class UIGuildDonatePanel : UIBasePanel
{
    private int mCurInputValue = 0;
    private long mCurContribute = 0;
    public override void Init()
    {
        base.Init();

        mCurInputValue = CSGuildInfo.Instance.DonateMinValue;
        mCurContribute = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
        SetDonateLabText(mCurInputValue);
        SetMyCoin();

        if(null != mlab_MyCoin)
        {
            mlab_MyCoin.text = CSString.Format(787, CSGuildInfo.Instance.DonateMoneyID.ItemName());
        }

        if(null != msp_moneyIcon)
        {
            msp_moneyIcon.spriteName = CSGuildInfo.Instance.DonateMoneyID.SmallIcon();
        }

        if(null != mlab_Desc)
        {
            mlab_Desc.text = $"{CSGuildInfo.Instance.DonateMinValue}{CSGuildInfo.Instance.DonateMoneyID.ItemName()}";
        }

        if(null != mlab_Desc2)
        {
            mlab_Desc2.text = CSString.Format(786,CSGuildInfo.Instance.DonateGetGuildAttribute,CSGuildInfo.Instance.DonateGetGuildGold);
        }

        mbtn_Close.onClick = OnClickClosePanel;
        mbtn_Cancel.onClick = OnClickClosePanel;
        mbtn_Bag.onClick = OnClickClosePanel;
        mbtn_Add.onClick = OnClickBtnAdd;
        mbtn_Reduce.onClick = OnClickBtnReduce;
        mbtn_Submit.onClick = OnDonateClick;
        UIEventListener.Get(mlab_DonateInput.gameObject).onSelect = OnSelectDonateValue;
        mlab_DonateInput.onValidate = OnValidateInput;

        mClientEvent.AddEvent(CEvent.OnGuildDonateSucceed, OnGuildDonateSucceed);
        mClientEvent.AddEvent(CEvent.ItemListChange, OnItemCounterChanged);
        mClientEvent.AddEvent(CEvent.MoneyChange, OnItemCounterChanged);
    }

    char OnValidateInput(string text, int pos, char ch)
    {
        if (ch >= '0' && ch <= '9') return ch;
        return (char)0;
    }

    public override void Show()
    {
        base.Show();
    }

    private void OnClickClosePanel(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIGuildDonatePanel>();
    }

    private void OnClickBtnAdd(GameObject go)
    {
        mCurInputValue += CSGuildInfo.Instance.DonateMinValue;
        SetDonateLabText(mCurInputValue);
    }

    private void OnClickBtnReduce(GameObject go)
    {
        mCurInputValue -= CSGuildInfo.Instance.DonateMinValue;
        if (mCurInputValue < CSGuildInfo.Instance.DonateMinValue)
        {
            SetDonateLabText(CSGuildInfo.Instance.DonateMinValue);
        }
        else
        {
            SetDonateLabText(mCurInputValue);
        }
    }

    private void OnItemCounterChanged(uint eventId,object argv)
    {
        SetMyCoin();
    }

    private void OnGuildDonateSucceed(uint id,object argv)
    {
        mCurContribute = CSItemCountManager.Instance.GetItemCount((int)MoneyType.unionAttribute);
    }

    private void SetMyCoin()
    {
        if (mlab_MyCoinNum != null)
        {
            mlab_MyCoinNum.text = $"{CSItemCountManager.Instance.GetItemCount(CSGuildInfo.Instance.DonateMoneyID)}";
        }
    }

    private void SetDonateInputValue(string strValue)
    {
        if(null != mlab_DonateInput)
            mlab_DonateInput.value = strValue;
    }

    private string GetDonateInputValue()
    {
        return mlab_DonateInput?.value;
    }

    private void SetDonateLabText(int num)
    {
        long owned = CSItemCountManager.Instance.GetItemCount(CSGuildInfo.Instance.DonateMoneyID);
        if (null != mlab_DonateNum)
        {
            mlab_DonateNum.text = num.ToString();
            mlab_DonateNum.color = UtilityColor.GetColor(owned >= num ? ColorType.ProperyColor : ColorType.Red);
        }
        mCurInputValue = num;
    }

    private void OnDonateClick(GameObject gp)
    {
        if (mCurInputValue < CSGuildInfo.Instance.DonateMinValue) 
        {
            UtilityTips.ShowTips(752); 
            return; 
        }

        long owned = CSItemCountManager.Instance.GetItemCount(CSGuildInfo.Instance.DonateMoneyID);
        if(mCurInputValue > owned)
        {
            Utility.ShowGetWay(CSGuildInfo.Instance.DonateMoneyID);
            return;
        }
        Net.CSUnionDonateGoldMessage(mCurInputValue);
        this.Close();
    }

    private void OnSelectDonateValue(GameObject go, bool isSelect)
    {
        if (isSelect)
        {

        }
        else
        {
            string strValue = GetDonateInputValue();
            int num = 0;
            if (int.TryParse(strValue, out num))
            {
                if (num >= CSGuildInfo.Instance.DonateMinValue)
                {
                    SetDonateLabText(num);
                }
                else
                {
                    SetDonateLabText(CSGuildInfo.Instance.DonateMinValue);
                }
            }
            else
            {
                SetDonateLabText(CSGuildInfo.Instance.DonateMinValue);
            }
        }
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnGuildDonateSucceed, OnGuildDonateSucceed);
        mClientEvent.RemoveEvent(CEvent.ItemListChange, OnItemCounterChanged);
        mClientEvent.RemoveEvent(CEvent.MoneyChange, OnItemCounterChanged);
    }
}