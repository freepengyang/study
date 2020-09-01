using UnityEngine;
using System.Collections.Generic;
public class DevideData : IndexedItem
{
	public int Index { get; set; }
	public union.UnionMember member;
	public int count;
	public System.Action<long> onValueChanged;
}

public class UIDivideBinder : UIBinder
{
	UILabel lb_name;
	UIEventListener btn_minus;
	UIEventListener btn_add;
	UILabel lb_value;
	UIInput lb_input;

	const int devide_unit = 100;
	public int Count
	{
		get;private set;
	}
	public static long mTotalCount = 0;
	DevideData mData;
	public override void Init(UIEventListener handle)
	{
		lb_name = Get<UILabel>("lb_name");
		btn_add = Get<UIEventListener>("money/btn_add");
		btn_minus = Get<UIEventListener>("money/btn_min");
		lb_value = Get<UILabel>("money/lb_value");
		lb_input = Get<UIInput>("money/lb_input");

		btn_add.onClick = this.OnAdd;
		btn_minus.onClick = this.OnMinus;
		lb_input.onValidate = this.OnValidateInput;
		lb_input.activeTextColor = CSColor.green;
		lb_input.label.color = CSColor.green;
		EventDelegate.Add(lb_input.onChange, this.OnPriceChange);
		Count = 0;
	}

	protected void OnPriceChange()
	{
		int oldValue = Count;
		int newValue = 0;
		int.TryParse(lb_input.value, out newValue);
		if (newValue == oldValue)
			return;

		long maxValue = (mTotalCount + Count);
		if (newValue > maxValue)
			newValue = (int)maxValue;

        mTotalCount = mTotalCount + oldValue - newValue;
		Count = newValue;
		mData.count = Count;

        RefreshValues();
        mData.onValueChanged?.Invoke(mTotalCount);
    }

	protected void OnAdd(GameObject go)
	{
		if (null != mData)
		{
			OnAdd(devide_unit);
        }
	}

    protected void OnAdd(int value)
    {
        int v = (int)Mathf.Min(value, mTotalCount);
        mTotalCount -= v;
        Count += v;
        mData.count = Count;
        RefreshValues();
        mData.onValueChanged?.Invoke(mTotalCount);
    }

    protected void OnMinus(GameObject go)
    {
		OnMinus(devide_unit);
	}

	protected void OnMinus(int value)
	{
        if (null != mData)
        {
            int v = Mathf.Min(value, Count);
            Count -= v;
            mTotalCount += v;
            mData.count = Count;
            RefreshValues();
            mData.onValueChanged?.Invoke(mTotalCount);
        }
    }

	protected void RefreshValues()
	{
		if (null != lb_value && null != mData)
		{
			lb_input.value = Count.ToString();
		}
	}

    char OnValidateInput(string text, int pos, char ch)
    {
        if (ch >= '0' && ch <= '9') return ch;
        return (char)0;
    }

    public override void Bind(object data)
	{
		mData = data as DevideData;
		if(null != mData)
		{
			if(null != lb_name && null != mData.member)
			{
				lb_name.text = CSString.Format(1052, mData.member.name, CSGuildInfo.Instance.GetPosColor(mData.member.position), CSGuildInfo.Instance.GetGuildPos(mData.member.position));
			}
			if (null != lb_value)
				lb_value.text = $"{mData.count}";
		}
	}

	public override void OnDestroy()
	{
		mData = null;
		if(null != lb_input)
		{
			lb_input.onValidate = null;
			EventDelegate.Remove(lb_input.onChange, OnPriceChange);
		}
		if (null != btn_add)
		{
			btn_add.onClick = null;
			btn_add = null;
		}
		if(null != btn_minus)
		{
			btn_minus.onClick = null;
			btn_minus = null;
		}
		lb_name = null;
		lb_value = null;
	}
}

public partial class UIGuildDevidePanel : UIBasePanel
{
	FastArrayElementFromPool<DevideData> mDevideDatas;
	Dictionary<long, int> mValues;

	public override void Init()
	{
		base.Init();

		mBtnClose.onClick = this.Close;
		mBtnSearch.onClick = this.OnFilter;
		mBtnDevide.onClick = this.OnDevide;
		mDevideDatas = mPoolHandleManager.CreateGeneratePool<DevideData>(32);
		mValues = new Dictionary<long, int>(32);

		Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
		mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);

		mYuanBaoCnt = CSGuildInfo.Instance.OwnedYuanbao;
		UIDivideBinder.mTotalCount = CSGuildInfo.Instance.OwnedYuanbao;
	}

	protected void OnFilter(GameObject go)
	{
		RefreshMoney();
	}

	protected void OnDevide(GameObject go)
	{
        if (!ApplyToDic(true))
        {
            return;
        }

        UtilityTips.ShowPromptWordTips(62,null,Apply);
	}

	protected void Apply()
	{
		var items = NetHelper.ToGoogleList<union.YuanbaoItem>();
		var it = mValues.GetEnumerator();
		while(it.MoveNext())
		{
			var item = new union.YuanbaoItem();
			item.roleId = it.Current.Key;
			item.num = it.Current.Value;
			items.Add(item);
		}
		Net.CSSplitYuanbaoMessage(items);
		mValues.Clear();
		this.Close();
    }

	long yuanbao = 0;
	long mYuanBaoCnt
	{
		get
		{
			return yuanbao;
		}
		set
		{
			yuanbao = value;
            if (null != mYuanBao)
                mYuanBao.text = $"{value}";
        }
	}
	protected bool ApplyToDic(bool tips = false)
    {
        var totalYuanBao = CSGuildInfo.Instance.OwnedYuanbao;
		for (int i = 0; i < mDevideDatas.Count; ++i)
        {
            long id = mDevideDatas[i].member.roleId;
            if (mValues.ContainsKey(id))
            {
                mValues[id] = mDevideDatas[i].count;
            }
            else
            {
                mValues.Add(id, mDevideDatas[i].count);
            }
		}

		long costed = 0L;
		for (var it = mValues.GetEnumerator();it.MoveNext();)
		{
			costed += it.Current.Value;
		}

		if (costed > CSGuildInfo.Instance.OwnedYuanbao || CSGuildInfo.Instance.OwnedYuanbao == 0)
		{
			if(tips)
				UtilityTips.ShowRedTips(1049);
			return false;
		}

		if(costed <= 0)
		{
			if (tips)
				UtilityTips.ShowRedTips(1050);
			return false;
		}

		mYuanBaoCnt = totalYuanBao - costed;
		if (mYuanBaoCnt < 0)
			mYuanBaoCnt = 0;
		return true;
    }

	protected void RefreshMoney()
	{
		ApplyToDic();
		mDevideDatas.Clear();
		var filter = mInput.value.Trim();
		var tabInfo = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionMemberInfo);
		if(null != tabInfo && null != tabInfo.unionInfo && null != tabInfo.unionInfo.members)
		{
			var members = tabInfo.unionInfo.members;
			for(int i = 0; i < members.Count; ++i)
			{
				if (null == members)
					continue;

				if (!string.IsNullOrEmpty(filter) && !members[i].name.Contains(filter))
					continue;

				var devideData = mDevideDatas.Append();
				devideData.count = 0;
				devideData.member = members[i];
				if (mValues.ContainsKey(members[i].roleId))
					devideData.count = mValues[members[i].roleId];
				devideData.onValueChanged = this.OnValueChagned;
			}
		}
		mDevideDatas.Sort(DevideDataComparer);
		mGridList.Bind<UIDivideBinder,DevideData>(mDevideDatas);
	}

	protected int DevideDataComparer(DevideData l,DevideData r)
	{
		if (l.member.isOnline != r.member.isOnline)
			return l.member.isOnline ? -1 : 1;
		if (l.member.position != r.member.position)
			return l.member.position - r.member.position;
		if (l.member.fighting != r.member.fighting)
			return r.member.fighting - l.member.fighting;
		return l.member.roleId.CompareTo(r.member.roleId);
	}

	protected void OnValueChagned(long value)
	{
		mYuanBaoCnt = value;
	}

	protected void OnGuildTabDataChanged(uint id,object argv)
	{
		RefreshMoney();
	}

	public override void Show()
	{
		base.Show();

		RefreshMoney();
	}
	
	protected override void OnDestroy()
	{
		UIDivideBinder.mTotalCount = 0;
		mClientEvent.RemoveEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
		mGridList?.UnBind<UIDivideBinder>();
		mGridList = null;
		mValues?.Clear();
		mValues = null;
		base.OnDestroy();
	}
}
