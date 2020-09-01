using UnityEngine;

public partial class UIWarSoulPropDescPanel : UIBasePanel
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
		CSBetterLisHot<PetBasePropInfoData> allList = CSPetBasePropInfo.Instance.GetAllList();
		ScriptBinder tempBinder;
		GameObject bgObj;
		UILabel lb_name, lb_allNum, lb_baseNum, lb_lvNum, lb_otherNum;
		int id;
		mGrid.MaxCount = allList.Count;
		for(int i=0;i<mGrid.MaxCount;i++)
		{
			tempBinder = mGrid.controlList[i].GetComponent<ScriptBinder>();
			bgObj = tempBinder.GetObject("BG") as GameObject;
			lb_name = tempBinder.GetObject("LbName") as UILabel;
			lb_allNum = tempBinder.GetObject("LbAllValue") as UILabel;
			lb_baseNum = tempBinder.GetObject("LbBaseValue") as UILabel;
			lb_lvNum = tempBinder.GetObject("LbLvValue") as UILabel;
			lb_otherNum = tempBinder.GetObject("LBOtherValue") as UILabel;

			PetBasePropInfoData data = allList[i];
			id = data.id;
			if(data.maxValue > 0)
			{
				lb_name.text = data.specialName;
				lb_allNum.text = $"{data.value}-{data.maxValue}";
				lb_baseNum.text = $"{data.baseValue}-{data.maxBaseValue}";
				lb_lvNum.text = $"{data.lvValue}-{data.maxLvValue}";
				lb_otherNum.text = $"{data.otherValue}-{data.maxOtherValue}";
			}
			else
			{
				lb_name.text = data.propName;
				SetValue(lb_allNum, id, data.value);
				SetValue(lb_baseNum, id, data.baseValue);
				SetValue(lb_lvNum, id, data.lvValue);
				SetValue(lb_otherNum, id, data.otherValue);
			}
			bgObj.SetActive(i%2 == 0);
		}
		mScrollView.SetDynamicArrowVertical(mDownIcon);
	}
	private void SetValue(UILabel label, int id,int value)
	{
		label.text = CSPetBasePropInfo.Instance.GetDealWithValue(id, value);
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}