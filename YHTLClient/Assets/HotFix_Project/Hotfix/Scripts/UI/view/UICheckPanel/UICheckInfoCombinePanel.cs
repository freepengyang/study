using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UICheckInfoCombinePanel : UIBasePanel
{
	
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}

	public enum ChildPanelType
	{
		CPT_Role = 1,
		CPT_Dragon = 2,
	}

	public override void Init()
	{
		base.Init();
		AddCollider();
		mbtn_close.onClick = Close;
		RegChildPanel<UICheckAttrPanel>((int)ChildPanelType.CPT_Role, mUICheckAttrPanel, mtog_role);
		RegChildPanel<UICheckDragonPanel>((int)ChildPanelType.CPT_Dragon, mUICheckDragonPanel, mtog_wolong);
	}
}
