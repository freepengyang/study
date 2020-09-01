using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class UIPetTalentTipsPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
        AddCollider();
	}
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}


    public void RefreshUI(CSTalentData data)
    {
        if (data == null || data.config == null) return;
        TABLE.CHONGWUTIANFU cfg = data.config;
        int activePaging = CSPetTalentInfo.Instance.ActivatedPage;
        int activeStar = CSPetTalentInfo.Instance.ActivatedLvInCurPage;
        bool isActive = cfg.paging <= activePaging && cfg.starrating <= activeStar;

        string strColor = isActive ? UtilityColor.MainText : UtilityColor.WeakText;

        string talentLvStr = CSPetTalentInfo.Instance.talentLvStr;

        mlb_name.text = CSString.Format(talentLvStr, data.Id).BBCode(ColorType.SubTitleColor);

        if (string.IsNullOrEmpty(data.desc))
        {
            data.SetDesc();
        }

        if (cfg.type == 1)
        {
            string value = CSPetBasePropInfo.Instance.GetDealWithValue(cfg.tip, cfg.value);
            mlb_text.text = $"{strColor}{data.desc}+{value}";
        }
        else
        {
            if (data.desc == null) return;
            string newStr = data.desc.Replace("[u=skill]", "");
            mlb_text.text = $"{strColor}{newStr}";
        }
    }

}
