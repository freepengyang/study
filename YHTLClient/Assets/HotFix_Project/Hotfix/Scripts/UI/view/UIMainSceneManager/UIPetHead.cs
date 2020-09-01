using UnityEngine;

public enum PetHeadState
{
	Battle = 1,//出战中
	Fit = 2,//合体中
	Dead = 3,//已死亡
	NotActive = 4,//未激活
	NotBattle = 5,//未出战
}

public class UIPetHead : UIBase
{
	private GameObject _parentObj;
	private GameObject parentObj
	{
		get { return _parentObj ?? (_parentObj = UIPrefab); }
	}
	private GameObject _btn_pet;
	private GameObject btn_pet
	{
		get { return _btn_pet ?? (_btn_pet = Get("headSprite/petheadBtn").gameObject); }
	}
	private GameObject _mask_obj;
	private GameObject mask_obj
	{
		get { return _mask_obj ?? (_mask_obj = Get("headSprite/mask").gameObject); }
	}
	private UILabel _state_label;
	private UILabel state_label
	{
		get { return _state_label ?? (_state_label = Get("headSprite/state/label").gameObject.GetComponent<UILabel>()); }
	}
	private UILabel _state_custom;
	private UILabel state_custom
	{
		get { return _state_custom ?? (_state_custom = Get("headSprite/state/custom").gameObject.GetComponent<UILabel>()); }
	}
	private UISprite _state_img;
	private UISprite state_img
	{
		get { return _state_img ?? (_state_img = Get("headSprite/state/img").gameObject.GetComponent<UISprite>()); }
	}
	private UISprite _sp_hp;
	private UISprite sp_hp
	{
		get { return _sp_hp ?? (_sp_hp = Get("headSprite/sp_hp").gameObject.GetComponent<UISprite>()); }
	}

	private CSInvoke _csInvoke;
	private CSInvoke csInvoke
	{
		get { return _csInvoke ?? (_csInvoke = Get("CSInvoke").GetComponent<CSInvoke>()); }
	}

	ItemPicker _ItemPicker;
	ItemPicker ItemPicker
	{
		get { return _ItemPicker ? _ItemPicker : (_ItemPicker = Get<ItemPicker>("ItemPicker")); }
	}

	int countDown = 0;
	bool isWithHero = false;
	UISprite petHead;
	public override void Init()
	{
		base.Init();

		petHead = btn_pet.GetComponent<UISprite>();
		UIEventListener.Get(btn_pet).onClick = PetHeadClick;
		mClientEvent.AddEvent(CEvent.GetPetStateInfo, GetPetStateInfo);                         //出战等状态					
		mClientEvent.AddEvent(CEvent.GetWarPetBaseHpInfo, GetWarPetBaseHpInfo);                 //血量刷新
		mClientEvent.AddEvent(CEvent.GetWarPetBaseActiveEffect, GetWarPetBaseActiveEffect);     //激活特效
		mClientEvent.AddEvent(CEvent.GetWarPetBaseActive, RefreshPetHeadUI);                    //激活卧龙战魂
		HotManager.Instance.EventHandler.AddEvent(CEvent.Role_ChangeMapId, RefreshPetStateLeavelMap);      //玩家地图更改
	}
	public override void Show()
	{
		//base.Show();
		UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_petHead, parentObj);
		SetState();
		RefreshPetHeadUI(0, null);
		GetPetStateInfo(0, null);
	}
	private void GetPetStateInfo(uint id, object data)
	{
		csInvoke?.StopInvokeRepeating();
		SetState();
	}
	private void RefreshPetStateLeavelMap(uint id, object data)
	{
		SetState();
	}
	private void RefreshPetHeadUI(uint id, object data)
	{
		string petHeadIcon = CSPetBasePropInfo.Instance.GetPetIcon();
		petHead.spriteName = petHeadIcon;
	}

	private void GetWarPetBaseHpInfo(uint evtId, object obj)
	{
		int hp, maxHp;
		float fillAmount = 0;
		hp = CSPetBasePropInfo.Instance.GetPetHp();
		maxHp = CSPetBasePropInfo.Instance.GetMaxHp();
		if (hp > 0 && hp < maxHp)
			fillAmount = (float)hp / maxHp;
		else if (hp >= maxHp)
			fillAmount = 1f;
		sp_hp.fillAmount = fillAmount;
	}
	private void GetWarPetBaseActiveEffect(uint id, object argv)
	{
		if (argv != null)
		{
			const string flyEffect = @"spr_effect";
			Vector3 startPos = (Vector3)argv;
			Vector3 targetPos = parentObj.transform.position;
			ItemPicker.Pick(startPos, targetPos, f =>
			{
				if (null == f)
				{
					FNDebug.LogError("[Pick]:picker onCreate failed");
					return;
				}
				var spriteAnimation = f.transform.Find(flyEffect);
				if (null != spriteAnimation)
				{
					spriteAnimation.CustomActive(true);
					CSEffectPlayMgr.Instance.ShowParticleEffect(spriteAnimation.gameObject, 17102, 0, true, 1, false, Vector3.zero);
				}
			}, 0.60f);
		}
	}
	private void PetHeadClick(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UIWarPetCombinedPanel>(p =>
		{
			(p as UIWarPetCombinedPanel).OpenChildPanel((int)UIWarPetCombinedPanel.ChildPanelType.CPT_WARSOUL);
		});
	}
	//1出战中，2合体中，3死亡，4未激活，5未出战
	private void SetState()
	{
		int id = CSPetBasePropInfo.Instance.GetPetState();
		switch (id)
		{
			case (int)PetHeadState.Battle:
				SetBattle(false, CSString.Format(847).BBCode(ColorType.Green), id, true);
				break;
			case (int)PetHeadState.Fit:
				SetBattle(false, CSString.Format(848).BBCode(ColorType.Green), id, true);
				break;
			case (int)PetHeadState.Dead:
				SetBattle(true, CSString.Format(1720).BBCode(ColorType.Red), id, false);
				break;
			case (int)PetHeadState.NotActive:
				SetBattle(false, CSString.Format(1693).BBCode(ColorType.WeakText), id, false);
				break;
			case (int)PetHeadState.NotBattle:
				SetBattle(false, CSString.Format(1721).BBCode(ColorType.WeakText), id, false);
				break;
		}
	}
	private void SetBattle(bool _isMask, string _label, int _id, bool _isWhitePetHead)
	{
		mask_obj.CustomActive(_isMask);
		state_label.text = _label;
		state_custom.text = "";
		state_img.spriteName = "main_pet_state";
		//countDownId = _id;
		if (_id == (int)PetHeadState.Dead)
		{
			long time = CSPetBasePropInfo.Instance.GetPetStateTime();
			long tempTime = time - CSServerTime.Instance.TotalSeconds;
			if (tempTime > 0)
			{
				countDown = (int)tempTime;
				csInvoke?.InvokeRepeating(0f, 1f, ScheduleReapeat);
			}
			else
				countDown = 0;
			state_custom.text = $"{countDown}{CSString.Format(423)}".BBCode(ColorType.SecondaryText);
		}
		else if (_id == (int)PetHeadState.NotActive || _id == (int)PetHeadState.Battle)
		{
			GetWithHeroByMap(ref isWithHero);
			if (isWithHero)
				state_label.text = CSString.Format(1707);
			else
				state_label.text = _label;
		}
		if (_isWhitePetHead)
			petHead.color = Color.white;
		else
			petHead.color = Color.black;
	}
	private void ScheduleReapeat()
	{
		if (countDown > 0)
		{
			state_custom.text = $"{countDown}{CSString.Format(423)}".BBCode(ColorType.SecondaryText);
			CSPetBasePropInfo.Instance.SetCountDown(countDown);
		}
		else
		{
			GetWithHeroByMap(ref isWithHero);
			csInvoke?.StopInvokeRepeating();
			state_custom.text = "";
			CSPetBasePropInfo.Instance.SetCountDown(0);
			if (isWithHero)
			{
				state_label.text = CSString.Format(1707);
				mask_obj.CustomActive(false);
				petHead.color = Color.white;
			}
		}
		countDown -= 1;
	}
	private void GetWithHeroByMap(ref bool isWithHero)
	{
		int mapId = CSMainPlayerInfo.Instance.MapID;
		int withHero = MapInfoTableManager.Instance.GetMapInfoWithHero(mapId);
		bool isActivePvP = CSPetBasePropInfo.Instance.IsActivePvP;
		if (withHero == 2)
			isWithHero = !isActivePvP;
		else
			isWithHero = withHero > 2;
	}
	protected override void OnDestroy()
	{
		csInvoke?.StopInvokeRepeating();
		countDown = 0;
		isWithHero = false;
		petHead = null;
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.GetPetStateInfo, GetPetStateInfo);
			mClientEvent.RemoveEvent(CEvent.GetWarPetBaseHpInfo, GetWarPetBaseHpInfo);
			mClientEvent.RemoveEvent(CEvent.GetWarPetBaseActiveEffect, GetWarPetBaseActiveEffect);
			mClientEvent.RemoveEvent(CEvent.GetWarPetBaseActive, RefreshPetHeadUI);
		}
		HotManager.Instance.EventHandler.RemoveEvent(CEvent.Role_ChangeMapId, RefreshPetStateLeavelMap);
		base.OnDestroy();
	}
}