using System.Collections.Generic;
using Google.Protobuf.Collections;

public class CSPetBasePropInfo : CSInfo<CSPetBasePropInfo>
{
	PetBaseInfoData petInfoData = new PetBaseInfoData();
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	CSBetterLisHot<int> clientTipsIdList = new CSBetterLisHot<int>();
	CSBetterLisHot<PetBasePropInfoData> allList = new CSBetterLisHot<PetBasePropInfoData>();
	CSBetterLisHot<PetBaseSkillData> petActiveSkillList = new CSBetterLisHot<PetBaseSkillData>();
	CSBetterLisHot<PetBaseSkillData> petPassiveSkillList = new CSBetterLisHot<PetBaseSkillData>();
	PetBaseSkillData specialSkill = new PetBaseSkillData();
	Dictionary<int, int> netPropDic = new Dictionary<int, int>();
	Dictionary<int, bag.BagItemInfo> equipNormalList = new Dictionary<int, bag.BagItemInfo>();
	CSBetterLisHot<PetBasePropInfoData> baseList = new CSBetterLisHot<PetBasePropInfoData>();
	CSBetterLisHot<PetBasePropInfoData> lvList = new CSBetterLisHot<PetBasePropInfoData>();
	CSBetterLisHot<PetBasePropInfoData> allTempList = new CSBetterLisHot<PetBasePropInfoData>();
	CSBetterLisHot<int> idxList = new CSBetterLisHot<int>();
	CSBetterLisHot<PetBasePropInfoData> newBaseList = new CSBetterLisHot<PetBasePropInfoData>();
	int curClickSuitId = 0;
	int specialSkillGroup = 206;
	int state = 0;
	long time = 0;//宠物死亡发送的时间戳
	int countDown = 0;
	int hp = 0;
	int maxHp = 0;
	bool isActivePvP = false;
	pet.PetAttr attr;
	string attStr, phyDefStr, magDefStr;
	List<int> attList, phyDefList, magDefList;

	public bool IsActivePvP
	{
		get { return isActivePvP; }
		set { isActivePvP = value; }
	}
	/// <summary>
	/// 设置卧龙战宠信息
	/// </summary>
	/// <param name="msg"></param>
	public void SetPetInfo(pet.WoLongPetInfo msg)
	{
		petInfoData.level = msg.level;
		petInfoData.exp = msg.exp;
		petInfoData.active = msg.active;
		petInfoData.suitId = msg.active ? msg.id : 0;
		attr = msg.attr;
		IsActivePvP = msg.activePvp;
		if (attr != null && attr.attrs != null)
		{
			RepeatedField<user.TupleProperty> attrs = attr.attrs;
			for (int i = 0; i < attrs.Count; i++)
			{
				if (netPropDic.ContainsKey(attrs[i].type))
					netPropDic[attrs[i].type] = attrs[i].value;
				else
					netPropDic.Add(attrs[i].type, attrs[i].value);
			}
		}

		if (clientTipsIdList.Count <= 0)
		{
			int id;
			string temp = SundryTableManager.Instance.GetSundryEffect(684);
			string[] tempArr = UtilityMainMath.StrToStrArr(temp);
			if (tempArr == null) return;
			for (int i = 0; i < tempArr.Length; i++)
			{
				int.TryParse(tempArr[i], out id);
				clientTipsIdList.Add(id);
			}
		}
		SetHavePetPropList();
		SetHavePetSkillList(msg.skills);
	}
	/// <summary>
	/// 设置战宠状态，时间戳
	/// </summary>
	/// <param name="msg"></param>
	public void SetPetStateInfo(pet.WoLongPetState msg)
	{
		state = msg.state;
		time = msg.time;
	}
	public long GetPetStateTime()
	{
		return time;
	}
	/// <summary>
	/// 获取宠物状态
	/// </summary>
	/// <returns></returns>
	public int GetPetState()
	{
		if (state == 0)
			state = (int)PetHeadState.NotActive;
		return state;
	}
	/// <summary>
	/// 获取宠物等级，若level == 0，宠物未激活
	/// </summary>
	/// <returns></returns>
	public int GetPetLevel()
	{
		return petInfoData.level;
	}
	/// <summary>
	/// 获取zhanHunSuit表，若suitId == 0，宠物未激活
	/// </summary>
	/// <returns></returns>
	public int GetZhanHunSuitId()
	{
		return petInfoData.suitId;
	}
	public void SetPetHpAndMaxHp(pet.PetHpInfo msg)
	{
		hp = msg.hp;
		maxHp = msg.maxHp;
	}
	public int GetPetHp()
	{
		return hp;
	}
	public int GetMaxHp()
	{
		return maxHp;
	}
	/// <summary>
	/// 获取宠物头像
	/// </summary>
	/// <returns></returns>
	public string GetPetIcon()
	{
		string icon = "";
		int suitId = petInfoData.suitId;
		int petid = 0;
		if (suitId > 0) //服务端发送suitId替换头像
			petid = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(suitId);
		else if (petid <= 0) //服务端没发送，读取本地数据，若也为0，则默认战宠1头像
			petid = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(1);
		else
			petid = CSMainPlayerInfo.Instance.WolongPetId;

		icon = MonsterInfoTableManager.Instance.GetMonsterInfoHead(petid).ToString();
		return icon;
	}
	/// <summary>
	/// 设置倒计时时间，目前死亡状态用到
	/// </summary>
	/// <param name="_countDown"></param>
	public void SetCountDown(int _countDown)
	{
		countDown = _countDown;
	}
	/// <summary>
	/// 获取倒计时时间
	/// </summary>
	/// <returns></returns>
	public int GetCountDwon()
	{
		return countDown;
	}
	/// <summary>
	/// 是否拥有宠物
	/// </summary>
	/// <returns></returns>
	public bool IsHasPet()
	{
		bool isHas = false;
		if (petInfoData.active && petInfoData.suitId > 0)
			isHas = true;
		return isHas;
	}
	/// <summary>
	/// 获取宠物信息
	/// </summary>
	/// <returns></returns>
	public PetBaseInfoData GetPetInfoData()
	{
		return petInfoData;
	}
	/// <summary>
	/// 根据ZhanHunSuit表，获取玩家身上穿戴的对应有几件装备
	/// </summary>
	/// <param name="suitId"></param>
	/// <returns></returns>
	public int GetEquipNum(int suitId)
	{
		int num = 0;
		CSBagInfo.Instance.GetNormalEquip(equipNormalList);      //自身装备id表
		if (equipNormalList != null)
		{
			var iter = equipNormalList.GetEnumerator();
			while (iter.MoveNext())
			{
				int itemId = iter.Current.Value.configId;
				int zhanHunSuitId = ItemTableManager.Instance.GetItemZhanHunSuit(itemId);
				if (suitId <= zhanHunSuitId)
					num += 1;
			}
		}
		return num;
	}
	/// <summary>
	/// 设置当前选中的战宠suitId
	/// </summary>
	/// <param name="_curClickSuitId"></param>
	public void SetCurClickSuitId(int _curClickSuitId)
	{
		curClickSuitId = _curClickSuitId;
	}
	/// <summary>
	/// 未激活的战魂时，战魂信息显示总属性
	/// </summary>
	public void ChangePetBasePropInfo()
	{
		int curSuitId = curClickSuitId;
		curSuitId = curSuitId < petInfoData.suitId ? petInfoData.suitId : curSuitId;
		int curPetId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(petInfoData.suitId);
		int nextPetId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(curSuitId);
		TABLE.MONSTERINFO monsterInfo = null;
		TABLE.MONSTERINFO curtMonsterInfo = null;
		MonsterInfoTableManager.Instance.TryGetValue(nextPetId, out monsterInfo);
		MonsterInfoTableManager.Instance.TryGetValue(curPetId, out curtMonsterInfo);
		if (monsterInfo == null) return;
		int value = 0;
		for (int i = 0; i < allList.Count; i++)
		{
			GetbaseProp(allList[i].id, monsterInfo, ref value);
			allList[i].value = allList[i].value - allList[i].baseValue + value;
			allList[i].baseValue = value;
			if (curtMonsterInfo != null)
			{
				if (allList[i].id == 1657)
				{
					if (allList[i].maxTempValue == 0)
						allList[i].maxTempValue = curtMonsterInfo.attMax;
					allList[i].maxValue = allList[i].maxValue - allList[i].maxTempValue + monsterInfo.attMax;
					allList[i].maxTempValue = monsterInfo.attMax;
					allList[i].specialName = CSString.Format(1732);
				}
				else if (allList[i].id == 1659)
				{
					if (allList[i].maxTempValue == 0)
						allList[i].maxTempValue = curtMonsterInfo.phyDefMax;
					allList[i].maxValue = allList[i].maxValue - allList[i].maxTempValue + monsterInfo.phyDefMax;
					allList[i].maxTempValue = monsterInfo.phyDefMax;
					allList[i].specialName = CSString.Format(1733);
				}
				else if (allList[i].id == 1661)
				{
					if (allList[i].maxTempValue == 0)
						allList[i].maxTempValue = curtMonsterInfo.magicDefMax;
					allList[i].maxValue = allList[i].maxValue - allList[i].maxTempValue + monsterInfo.magicDefMax;
					allList[i].maxTempValue = monsterInfo.magicDefMax;
					allList[i].specialName = CSString.Format(1734);
				}
			}
		}
	}
	/// <summary>
	/// 获得当前选中的战宠suitId
	/// </summary>
	/// <returns></returns>
	public int GetCurClickSuitId()
	{
		return curClickSuitId;
	}
	/// <summary>
	/// 战宠激活，设置属性
	/// </summary>
	/// <param name="attr"></param>
	private void SetHavePetPropList()
	{
		if (attr == null) return;
		baseList.Clear();
		lvList.Clear();
		allTempList.Clear();
		int petId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(petInfoData.suitId);
		for (int i = 0; i < clientTipsIdList.Count; i++)
		{
			SetbasePropList(clientTipsIdList[i], petId, ref baseList);
			SetLvPropList(clientTipsIdList[i], petInfoData.level, ref lvList);
			SetNetAllPropList(clientTipsIdList[i], ref allTempList);
		}
		for (int i = 0; i < allTempList.Count; i++)
		{
			allTempList[i].baseValue = baseList[i].value;
			allTempList[i].id = baseList[i].id;
			allTempList[i].lvValue = lvList[i].value;
			allTempList[i].otherValue = allTempList[i].value - baseList[i].value - lvList[i].value;
			allTempList[i].otherValue = allTempList[i].otherValue >= 0 ? allTempList[i].otherValue : 0;
		}
		SetFianllyAllPropList(allTempList);
	}
	/// <summary>
	/// 设置宠物技能信息
	/// </summary>
	private void SetHavePetSkillList(RepeatedField<int> skills)
	{
		petActiveSkillList.Clear();
		var arr = SkillTableManager.Instance.array.gItem.handles;
		int skillGroup = 0;
		int specialSkillId = 0;
		bool isHasSpecialSkill = false;
		petPassiveSkillList.Clear();
		for (int k = 0, max = arr.Length; k < max; ++k)
		{
			var item = arr[k].Value as TABLE.SKILL;
			if (item.usertype == 2 &&
				skillGroup != item.skillGroup &&
				item.show == 1)
			{
				//主动技能
				if (item.type != 6 && item.type != 7)
				{
					SetSkillList(item, skills, ref petActiveSkillList, ref skillGroup);
				}
				//特殊被动技能（护体）
				if (item.skillGroup == specialSkillGroup)
				{
					specialSkillId = item.id;
					skillGroup = item.skillGroup;
				}
			}
		}
		//被动技能
		petPassiveSkillList.Clear();
		int show, usertype, type, group;
		for (int i = 0; i < skills.Count; i++)
		{
			TABLE.SKILL skill = null;
			SkillTableManager.Instance.TryGetValue(skills[i], out skill);
			if (skill == null) return;
			group = (int)skill.skillGroup;
			if (group == specialSkillGroup)
			{
				specialSkillId = skills[i];
				isHasSpecialSkill = true;
			}
			else
			{
				type = skill.type;
				usertype = skill.usertype;
				show = skill.show;
				if (usertype == 2 && show == 1)
				{
					if (type == 6 || type == 7)
					{
						PetBaseSkillData passiveSkill = mPoolHandle.GetCustomClass<PetBaseSkillData>();
						passiveSkill.skillId = skills[i];
						passiveSkill.icon = skill.icon;
						passiveSkill.name = skill.name;
						passiveSkill.level = skill.level;
						passiveSkill.isGet = true;
						passiveSkill.showorder = skill.showorder;
						petPassiveSkillList.Add(passiveSkill);
					}
				}
			}
		}
		SetSpecialSkillData(isHasSpecialSkill, specialSkillId);
		SortSkillListByShowOrder(petActiveSkillList);
		SortSkillListByShowOrder(petPassiveSkillList);
	}
	private void SortSkillListByShowOrder(CSBetterLisHot<PetBaseSkillData> skillList)
	{
		skillList.Sort((x, y) => { return x.showorder.CompareTo(y.showorder); });
	}

	/// <summary>
	/// 设置技能列表数据
	/// </summary>
	/// <param name="tempData"></param>
	/// <param name="skillList"></param>
	/// <param name="skillGroup"></param>
	private void SetSkillList(TABLE.SKILL tempData, RepeatedField<int> skills, ref CSBetterLisHot<PetBaseSkillData> skillList, ref int skillGroup)
	{
		PetBaseSkillData petSkill = mPoolHandle.GetCustomClass<PetBaseSkillData>();
		int netSkillGroup, skillLv;
		petSkill.skillId = tempData.id;
		skillGroup = tempData.skillGroup;
		petSkill.icon = SkillTableManager.Instance.GetSkillIcon(tempData.id);
		petSkill.skillGroup = skillGroup;
		petSkill.name = tempData.name;
		petSkill.showorder = tempData.showorder;
		for (int i = 0; i < skills.Count; i++)
		{
			netSkillGroup = SkillTableManager.Instance.GetSkillSkillGroup(skills[i]);
			skillLv = SkillTableManager.Instance.GetSkillLevel(skills[i]);
			if (petSkill.skillGroup == netSkillGroup)
			{
				petSkill.level = skillLv;
				petSkill.isGet = true;
				break;
			}
		}
		skillList.Add(petSkill);
	}
	/// <summary>
	/// 被动技能添加特殊技能
	/// </summary>
	private void SetSpecialSkillData(bool isHasSpecialSkill, int specialSkillId)
	{
		TABLE.SKILL skills = null;
		SkillTableManager.Instance.TryGetValue(specialSkillId, out skills);
		if (skills == null) return;
		specialSkill.skillId = specialSkillId;
		specialSkill.icon = skills.icon;
		specialSkill.skillGroup = skills.skillGroup;
		specialSkill.name = skills.name;
		specialSkill.level = skills.level;
		specialSkill.isGet = isHasSpecialSkill;
	}
	/// <summary>
	/// 获取被动特殊技能
	/// </summary>
	/// <returns></returns>
	public PetBaseSkillData GetSpecialSkillData()
	{
		return specialSkill;
	}
	public CSBetterLisHot<PetBaseSkillData> GetAtcivePetBaseList()
	{
		return petActiveSkillList;
	}
	public CSBetterLisHot<PetBaseSkillData> GetPassivePetBaseList()
	{
		return petPassiveSkillList;
	}
	/// <summary>
	/// 宠物未激活，设置属性
	/// </summary>
	/// <param name="suitId"></param>
	public void SetAttProp(int suitId)
	{
		int petId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(suitId);
		baseList.Clear();
		lvList.Clear();
		allTempList.Clear();
		for (int i = 0; i < clientTipsIdList.Count; i++)
		{
			SetbasePropList(clientTipsIdList[i], petId, ref baseList);
			SetLvPropList(clientTipsIdList[i], petInfoData.level, ref lvList);
		}
		for (int i = 0; i < baseList.Count; i++)
		{
			PetBasePropInfoData tempData = mPoolHandle.GetCustomClass<PetBasePropInfoData>();
			tempData.propName = baseList[i].propName;
			tempData.id = baseList[i].id;
			tempData.value = baseList[i].value + lvList[i].value;
			tempData.baseValue = baseList[i].value;
			tempData.lvValue = lvList[i].value;
			allTempList.Add(tempData);
		}
		SetFianllyAllPropList(allTempList);
	}
	/// <summary>
	/// 设置最终总属性，值大于0显示，小于0不显示
	/// </summary>
	private void SetFianllyAllPropList(CSBetterLisHot<PetBasePropInfoData> allTempList)
	{
		allList.Clear();
		for (int i = 0; i < allTempList.Count; i++)
		{
			if (allTempList[i].value > 0)
			{
				allList.Add(allTempList[i]);
			}
		}
	}
	/// <summary>
	/// 基础属性设置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="petId"></param>
	/// <param name="_baseList"></param>
	private void SetbasePropList(int id, int petId, ref CSBetterLisHot<PetBasePropInfoData> _baseList)
	{
		int value = 0;
		TABLE.MONSTERINFO monsterInfo = null;
		MonsterInfoTableManager.Instance.TryGetValue(petId, out monsterInfo);
		PetBasePropInfoData basePropData = mPoolHandle.GetCustomClass<PetBasePropInfoData>();
		basePropData.propName = ClientTipsTableManager.Instance.GetClientTipsContext(id);
		basePropData.id = id;
		if (monsterInfo != null)
		{
			GetbaseProp(id, monsterInfo, ref value);
		}
		basePropData.value = value;
		_baseList.Add(basePropData);
	}
	private void GetbaseProp(int id, TABLE.MONSTERINFO monsterInfo, ref int value)
	{
		switch (id)
		{
			case 1655://战魂生命
				value = (int)monsterInfo.hp; break;
			case 1656://战魂魔法
				value = (int)monsterInfo.mp; break;
			case 1657://战魂最小攻击
				value = monsterInfo.att; break;
			case 1658://战魂最大攻击
				value = monsterInfo.attMax; break;
			case 1659://战魂最小物防
				value = monsterInfo.phyDef; break;
			case 1660://战魂最大物防
				value = monsterInfo.phyDefMax; break;
			case 1661://战魂最小魔防
				value = monsterInfo.magicDef; break;
			case 1662://战魂最大魔防
				value = monsterInfo.magicDefMax; break;
			case 1668://战魂暴击伤害
				value = monsterInfo.criticalDamage; break;
			case 1669://战魂暴击率
				value = monsterInfo.critical; break;
			case 1670://战魂闪避率
				value = monsterInfo.dodge; break;
			case 1671://战魂命中率
				value = monsterInfo.accurate; break;
			case 1687://战魂生命回复
				value = monsterInfo.heathRecover; break;
			case 1688://战魂魔法回复
				value = monsterInfo.magicRecover; break;
			default://默认为0
				value = 0; break;
		}
	}
	/// <summary>
	/// 等级属性设置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="lv"></param>
	/// <param name="_lvList"></param>
	private void SetLvPropList(int id, int lv, ref CSBetterLisHot<PetBasePropInfoData> _lvList)
	{
		int value = 0;
		lv = lv > 0 ? lv : 1;
		TABLE.ZHANHUNLEVEL zhanHunLevel = null;
		ZhanHunLevelTableManager.Instance.TryGetValue(lv, out zhanHunLevel);
		PetBasePropInfoData lvPropData = mPoolHandle.GetCustomClass<PetBasePropInfoData>();
		lvPropData.propName = ClientTipsTableManager.Instance.GetClientTipsContext(id);
		if (zhanHunLevel != null)
		{
			switch (id)
			{
				case 1655://战魂生命
					value = zhanHunLevel.hp; break;
				case 1656://法术值
					value = zhanHunLevel.mp; break;
				case 1657://战魂最小攻击
					value = zhanHunLevel.att; break;
				case 1658://战魂最大攻击
					value = zhanHunLevel.attMax; break;
				case 1659://战魂最小物防
					value = zhanHunLevel.phyDef; break;
				case 1660://战魂最大物防
					value = zhanHunLevel.phyDefMax; break;
				case 1661://战魂最小魔防
					value = zhanHunLevel.magicDef; break;
				case 1662://战魂最大魔防
					value = zhanHunLevel.magicDefMax; break;
				case 1670://战魂闪避率
					value = zhanHunLevel.dodge; break;
				case 1671://战魂命中率
					value = zhanHunLevel.accurate; break;
				case 1687://战魂生命回复
					value = zhanHunLevel.heathRecover; break;
				case 1688://战魂魔法回复
					value = zhanHunLevel.mpHeal; break;
			}
		}
		lvPropData.value = value;
		_lvList.Add(lvPropData);
	}
	private void SetNetAllPropList(int id, ref CSBetterLisHot<PetBasePropInfoData> _allTempList)
	{
		PetBasePropInfoData allPropData = mPoolHandle.GetCustomClass<PetBasePropInfoData>();
		allPropData.propName = ClientTipsTableManager.Instance.GetClientTipsContext(id);
		int value = GetNetAttValue(id);
		allPropData.value = value;
		_allTempList.Add(allPropData);
	}
	public int GetNetAttValue(int id)
	{
		int value = 0;
		if (netPropDic == null || netPropDic.Count <= 0) return value;
		switch (id)
		{
			case 1655://战魂生命
				value = netPropDic[11]; break;
			case 1656://战魂魔法
				value = netPropDic[12]; break;
			case 1657://战魂最小攻击
				value = netPropDic[2] + netPropDic[4] + netPropDic[6]; break;
			case 1658://战魂最大攻击
				value = netPropDic[1] + netPropDic[3] + netPropDic[5]; break;
			case 1659://战魂最小物防
				value = netPropDic[8]; break;
			case 1660://战魂最大物防
				value = netPropDic[7]; break;
			case 1661://战魂最小魔防
				value = netPropDic[10]; break;
			case 1662://战魂最大魔防
				value = netPropDic[9]; break;
			case 1663://战魂攻击加成
				value = netPropDic[19]; break;
			case 1664://战魂物防加成
				value = netPropDic[17]; break;
			case 1665://战魂魔防加成
				value = netPropDic[18]; break;
			case 1666://战魂生命加成
				value = netPropDic[22]; break;
			case 1667://战魂魔法加成
				value = netPropDic[23]; break;
			case 1668://战魂暴击伤害
				value = netPropDic[13]; break;
			case 1669://战魂暴击率
				value = netPropDic[14]; break;
			case 1670://战魂闪避率
				value = netPropDic[16]; break;
			case 1671://战魂命中率
				value = netPropDic[15]; break;
			case 1672://战魂增加伤害
				value = netPropDic[35]; break;
			case 1673://战魂减少伤害
				value = netPropDic[36]; break;
			case 1687://战魂生命回复
				value = netPropDic[48]; break;
			case 1688://战魂魔法回复
				value = netPropDic[51]; break;
		}
		return value;
	}
	/// <summary>
	/// 返回总属性
	/// </summary>
	/// <returns></returns>
	public CSBetterLisHot<PetBasePropInfoData> GetAllList()
	{
		return allList;
	}

	private void SetStr(int clientId, ref string str)
	{
		str = ClientTipsTableManager.Instance.GetClientTipsContext(clientId);
	}
	private void SetList(int id, ref List<int> list)
	{
		string sundryStr = SundryTableManager.Instance.GetSundryEffect(id);
		list = UtilityMainMath.SplitStringToIntList(sundryStr);
	}
	public CSBetterLisHot<PetBasePropInfoData> DealWithList(CSBetterLisHot<PetBasePropInfoData> list)
	{
		if (string.IsNullOrEmpty(attStr))
			SetStr(1732, ref attStr);
		if (string.IsNullOrEmpty(phyDefStr))
			SetStr(1733, ref phyDefStr);
		if (string.IsNullOrEmpty(magDefStr))
			SetStr(1734, ref magDefStr);
		if (attList == null)
			SetList(712, ref attList);
		if (phyDefList == null)
			SetList(713, ref phyDefList);
		if (magDefList == null)
			SetList(714, ref magDefList);
		CombineValueList(712, attStr, ref list);
		CombineValueList(713, phyDefStr, ref list);
		CombineValueList(714, magDefStr, ref list);
		return list;
	}
	private void CombineValueList(int sundryId, string propName, ref CSBetterLisHot<PetBasePropInfoData> list)
	{
		string sundryStr = SundryTableManager.Instance.GetSundryEffect(sundryId);
		List<int> tempList = UtilityMainMath.SplitStringToIntList(sundryStr);
		idxList.Clear();
		if (tempList.Count > 0)
		{
			for (int i = 0; i < tempList.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (tempList[i] == list[j].id)
					{
						idxList.Add(j);
					}
				}
			}
			if (idxList.Count > 0 && list.Count > 0 && list[idxList[0]].maxValue <= 0)
			{
				list[idxList[0]].maxValue = list[idxList[1]].value;
				list[idxList[0]].maxBaseValue = list[idxList[1]].baseValue;
				list[idxList[0]].maxLvValue = list[idxList[1]].lvValue;
				list[idxList[0]].maxOtherValue = list[idxList[1]].otherValue;
				list[idxList[0]].specialName = propName;
				list.RemoveAt(idxList[1]);
			}
		}
	}
	/// <summary>
	/// 设置百分比
	/// </summary>
	/// <param name="_value"></param>
	/// <returns></returns>
	public string GetDecimal(int _value)
	{
		string temp = "";
		temp = $"{(_value / 100f).ToString("F1")}%";
		return temp;
	}
	/// <summary>
	/// 数值转换字符串
	/// </summary>
	/// <param name="id"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public string GetDealWithValue(int id, int value)
	{
		if (id >= 1663 && id <= 1673)
			return GetDecimal(value);
		else if (id == 1687 || id == 1772)
			return CSString.Format(1680, value);
		else if (id == 1688 || id == 1773)
			return CSString.Format(1690, value);
		else
			return value.ToString();
	}
	public CSBetterLisHot<PetBasePropInfoData> GetBasePropDataBySuitId(int suitId)
	{
		int petId = ZhanHunSuitTableManager.Instance.GetZhanHunSuitSuitSummoned(suitId);
		newBaseList.Clear();
		baseList.Clear();
		for (int i = 0; i < clientTipsIdList.Count; i++)
		{
			SetbasePropList(clientTipsIdList[i], petId, ref baseList);
		}
		for (int i = 0; i < baseList.Count; i++)
		{
			if (baseList[i].value > 0)
				newBaseList.Add(baseList[i]);
		}
		return newBaseList;
	}
	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		petActiveSkillList.Clear();
		petPassiveSkillList.Clear();
		clientTipsIdList.Clear();
		allList.Clear();
		netPropDic.Clear();

		equipNormalList = null;
		mPoolHandle = null;
		petInfoData = null;
		petActiveSkillList = null;
		petPassiveSkillList = null;
		clientTipsIdList = null;
		allList = null;
		attr = null;
		specialSkill = null;
		netPropDic = null;
		baseList = null;
		lvList = null;
		allTempList = null;
		idxList = null;
		newBaseList = null;
		attList = null;
		phyDefList = null;
		magDefList = null;
		attStr = string.Empty;
		phyDefStr = string.Empty;
		magDefStr = string.Empty;

		curClickSuitId = 0;
		specialSkillGroup = 0;
		state = 0;
		countDown = 0;
		hp = 0;
		maxHp = 0;
		time = 0;
		isActivePvP = false;
	}
}

public class PetBaseInfoData : IDispose
{
	public PetBaseInfoData() { }
	public PetBaseInfoData(int _suitId, int _level, int _exp, bool _active)
	{
		suitId = _suitId;
		level = _level;
		exp = _exp;
		active = _active;
	}
	public int suitId = 0;
	public int level = 0;
	public int exp = 0;
	public bool active = false;

	public void Dispose()
	{
		suitId = 0;
		level = 0;
		exp = 0;
		active = false;
	}
}
//战宠属性相关信息
public class PetBasePropInfoData : IDispose
{
	public PetBasePropInfoData() { }
	public PetBasePropInfoData(string _propName, int _id, int _value, int _baseValue, int _lvValue,
		int _otherValue, int _maxValue, int _maxBaseValue, int _maxLvValue, int _maxTempValue, int _maxOtherValue,
		string _specialName)
	{
		propName = _propName;   //属性名
		id = _id;               //杂项表的684，战魂属性对应的id	
		value = _value;         //总属性
		baseValue = _baseValue; //基础属性
		lvValue = _lvValue;     //等级属性
		otherValue = _otherValue;//其他属性
		maxValue = _maxValue;   //上限属性，合并用
		maxBaseValue = _maxBaseValue;
		maxLvValue = _maxLvValue;
		maxTempValue = _maxTempValue;
		maxOtherValue = _maxOtherValue;
		specialName = _specialName; //合并属性名
	}
	public string propName = "";
	public int id = 0;
	public int value = 0;
	public int baseValue = 0;
	public int lvValue = 0;
	public int otherValue = 0;
	public int maxValue = 0;
	public int maxBaseValue = 0;
	public int maxLvValue = 0;
	public int maxTempValue = 0;
	public int maxOtherValue = 0;
	public string specialName = "";
	public void Dispose()
	{
		propName = "";
		id = 0;
		value = 0;
		baseValue = 0;
		lvValue = 0;
		otherValue = 0;
		maxValue = 0;
		maxBaseValue = 0;
		maxLvValue = 0;
		maxTempValue = 0;
		maxOtherValue = 0;
		specialName = "";
	}
}

/// <summary>
/// 战魂技能数据
/// </summary>
public class PetBaseSkillData : IDispose
{
	public PetBaseSkillData() { }
	public PetBaseSkillData(int _skillId, int _skillGroup, int _level, int _showorder, string _name, string _icon, bool _isGet)
	{
		skillId = _skillId;
		skillGroup = _skillGroup;
		level = _level;
		name = _name;
		icon = _icon;
		isGet = _isGet;
	}
	public int skillId = 0;
	public int skillGroup = 0;
	public int level = 1;
	public int showorder = 0;
	public string name = "";
	public string icon = "";
	public bool isGet = false;
	public void Dispose()
	{
		skillId = 0;
		skillGroup = 0;
		level = 0;
		showorder = 0;
		name = "";
		icon = "";
		isGet = false;
	}
}