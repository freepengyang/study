using UnityEngine;

public partial class UIPetSuitTipPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		AddCollider();
	}
	public override void Show()
	{
		base.Show();
		RefreshProp();
	}
	private void RefreshProp()
	{
		PetBaseInfoData petInfoData = CSPetBasePropInfo.Instance.GetPetInfoData();
		int suitIdx = ZhanHunSuitTableManager.Instance.array.gItem.id2offset.Count;
		int maxSuitId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitId(suitIdx);
		int curPetSuitId = CSPetBasePropInfo.Instance.GetCurClickSuitId();
		bool isShowSingle,isActive;
		//最高级
		if (curPetSuitId >= maxSuitId)
			isShowSingle = true;
		//激活状态
		else if (curPetSuitId == petInfoData.suitId)
			isShowSingle = false;
		//未激活状态
		else
			isShowSingle = true;

		isActive = curPetSuitId == petInfoData.suitId;
		mNextInfo.SetActive(!isShowSingle);
		if (!isShowSingle)
		{
			SetPropValue(curPetSuitId + 1, mNextInfoSuitName, mNextInfoSuitSubName,mNextInfoGrid ,false);
			mNextInfoScrollView.SetDynamicArrowVertical(mNextInfoDownIcon);
		}
		else
		{
			UIWidget wid = mBG.GetComponent<UIWidget>();
			wid.height = 250;
			mTip.localPosition = new Vector3(mTip.localPosition.x,-25f ,mTip.localPosition.z);
		}
		SetPropValue(curPetSuitId, mCurInfoSuitName,mCurInfoSuitSubName,mCurInfoGrid,isActive);
		mCurInfoScrollView.SetDynamicArrowVertical(mCurInfoDownIcon);
	}
	/// <summary>
	/// 拼接文字和展示属性
	/// </summary>
	/// <param name="suitId"></param>
	/// <param name="suitName"></param>
	/// <param name="suitSubName"></param>
	/// <param name="isActive"></param>
	private void SetPropValue(int suitId,UILabel suitName,UILabel suitSubName,UIGridContainer grid ,bool isActive)
	{
		ScriptBinder tempBinder;
		UILabel lb_name, lb_num;
		int id;
		int petId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(suitId);
		int havaNum = CSPetBasePropInfo.Instance.GetEquipNum(suitId);
		int allNum = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitNum(suitId);
		uint lv = MonsterInfoTableManager.Instance.GetMonsterInfoLevel(petId);
		string name = CSString.Format(1692, lv);
		string subName = CSString.Format(1695, lv);
		string state = isActive ? CSString.Format(1694) : CSString.Format(1693);
		string subNumStr = $"{havaNum}/{allNum}";
		subName = suitId == 1 ? CSString.Format(1696) : subName;
		
		ColorType stateColor = isActive ? ColorType.Green : ColorType.Red;
		ColorType titColor = isActive ? ColorType.ImportantText : ColorType.WeakText;
		ColorType textColor = isActive ? ColorType.MainText : ColorType.WeakText;
		ColorType nameColor = isActive ? ColorType.SecondaryText : ColorType.WeakText;
		CSBetterLisHot <PetBasePropInfoData> baseList = CSPetBasePropInfo.Instance.GetBasePropDataBySuitId(suitId);
		baseList = CSPetBasePropInfo.Instance.DealWithList(baseList);
		state = $"({state})".BBCode(stateColor);
		suitName.text = $"{name.BBCode(titColor)}{state}";

		subNumStr = $"({subNumStr})".BBCode(stateColor);
		subNumStr = suitId == 1 ? "" : subNumStr;
		suitSubName.text = $"{subName.BBCode(textColor)}{subNumStr}";

		grid.MaxCount = baseList.Count;
		for(int i=0;i<grid.MaxCount;i++)
		{
			tempBinder = grid.controlList[i].GetComponent<ScriptBinder>();
			lb_name = tempBinder.GetObject("LbName") as UILabel;
			lb_num = tempBinder.GetObject("LbNum") as UILabel;
			id = baseList[i].id;
			if(baseList[i].maxValue > 0)
			{
				lb_name.text = baseList[i].specialName.BBCode(nameColor);
				lb_num.text = $"{baseList[i].value}-{baseList[i].maxValue}".BBCode(textColor);
			}
			else
			{
				lb_name.text = baseList[i].propName.BBCode(nameColor);
				lb_num.text = CSPetBasePropInfo.Instance.GetDealWithValue(id, baseList[i].value).BBCode(textColor);
			}
			
		}
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}