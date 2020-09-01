using UnityEngine;

public partial class UIWildEscapadePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    CSBetterLisHot<TABLE.ADVENTURE> configTemp = new CSBetterLisHot<TABLE.ADVENTURE>();
    TABLE.ADVENTURE curConfig;

    UIAdventureMonster monsterUI;
    UIAdventurePet petUI;
    UIAdventurePlayerAvatar avatarUI;


    Schedule monsterRefreshSch;


    UIAdventureRewardManager itemUIMgr = new UIAdventureRewardManager();


    public override void Init()
	{
		base.Init();
        AddCollider();

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_wildBg, "wild_bg");

        mClientEvent.Reg((uint)CEvent.WildAdventureInfoChange, AdventureInfoChange);
        mClientEvent.Reg((uint)CEvent.WildAdventureBossInfoChange, BossInfoChange);
        mClientEvent.Reg((uint)CEvent.GetPetStateInfo, PetStateInfoChange);//ս��״̬���

        mClientEvent.Reg((uint)CEvent.WildAdventureMonsterDead, MonsterDead);

        mbtn_close.onClick = CloseClick;
        mbtn_getEquip.onClick = GetEquipClick;
        mbtn_reward.onClick = OpenRewardsClick;

        //ģ��obj��
        monsterUI = mPoolHandleManager.GetCustomClass<UIAdventureMonster>();
        monsterUI.InitObj(mobj_monster);

        petUI = mPoolHandleManager.GetCustomClass<UIAdventurePet>();
        petUI.InitObj(mobj_pet);

        avatarUI = mPoolHandleManager.GetCustomClass<UIAdventurePlayerAvatar>();
        avatarUI.InitObj(mobj_player);

        if (itemUIMgr == null) itemUIMgr = new UIAdventureRewardManager();
        itemUIMgr.InitManager(mitemTemplate);
    }
	
	public override void Show()
	{
		base.Show();

        GetCurAdventureConfig();
        RefreshExpAndMoneyUI();
        RefreshRewardTime();

        RefreshMonsterModel(false);

        petUI.DoStandAnim();
        avatarUI.DoStandAnim();

        Net.CSWildAdventrueMessage();

    }
	
	protected override void OnDestroy()
    {
        Timer.Instance.CancelInvoke(monsterRefreshSch);
        CSEffectPlayMgr.Instance.Recycle(mtex_wildBg);
        configTemp?.Clear();
        configTemp = null;
        curConfig = null;

        monsterUI?.Dispose();
        petUI?.Dispose();
        avatarUI?.Dispose();

        itemUIMgr?.Dispose();
        itemUIMgr = null;

        base.OnDestroy();
	}


    void RefreshRewardTime()
    {
        int seconds = CSWildAdventureInfo.Instance.time;
        int showTime = Mathf.FloorToInt(seconds / 60);
        string unit = "";
        if (seconds < 3600)
        {
            unit = ClientTipsTableManager.Instance.GetClientTipsContext(420);
        }
        else
        {
            unit = ClientTipsTableManager.Instance.GetClientTipsContext(419);
            showTime = Mathf.FloorToInt(seconds / 3600);
        }
        mlb_time.text = CSString.Format("������{0}", $"{showTime}{unit}");
        mobj_full.SetActive(seconds > 0 && seconds >= CSWildAdventureInfo.Instance.timeLimit);
    }


    void GetCurAdventureConfig()
    {
        int wolongPetid = CSWoLongInfo.Instance.ReturnZhanHunSuitId();//ս��id
        
        mobj_Award.SetActive(wolongPetid > 0);
        mobj_Empty.SetActive(wolongPetid < 1);
        if (wolongPetid > 0)
        {
            int lv = CSMainPlayerInfo.Instance.Level;
            //AdventureTableManager.Instance.dic.Values.WhereToList(x => { return x.zhanhun == wolongPetid; }, configTemp);

            int lvOffset = 9999;
            var arr = AdventureTableManager.Instance.array.gItem.handles;
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var current = arr[i].Value as TABLE.ADVENTURE;
                if (curConfig.zhanhun != wolongPetid)
                    continue;
                if (lv >= current.level && lv - current.level < lvOffset)
                {
                    lvOffset = lv - current.level;
                    curConfig = current;
                }
            }

            //int lvOffset = 9999;
            //for (int i = 0; i < configTemp.Count; i++)
            //{
            //    if (lv >= configTemp[i].level && lv - configTemp[i].level < lvOffset)
            //    {
            //        lvOffset = lv - configTemp[i].level;
            //        curConfig = configTemp[i];
            //    }
            //}
        }
        else
        {
            curConfig = null;
        }
        RefreshPetModel();
    }


    void RefreshExpAndMoneyUI()
    {        
        if (curConfig == null) return;

        string minuteUnit = ClientTipsTableManager.Instance.GetClientTipsContext(420);
        string[] expConfigString = UtilityMainMath.StrToStrArr(curConfig.exp);
        if (expConfigString.Length > 1) mlb_earingB.text = $"{expConfigString[0]}/{expConfigString[1]}{minuteUnit}".BBCode(ColorType.Green);
        string[] moneyConfigString = UtilityMainMath.StrToStrArr(curConfig.yinzi);
        if (moneyConfigString.Length > 1) mlb_earingA.text = $"{moneyConfigString[0]}/{moneyConfigString[1]}{minuteUnit}".BBCode(ColorType.Green);
    }


    void RefreshMonsterModel(bool isBoss)
    {
        Timer.Instance.CancelInvoke(monsterRefreshSch);
        if (curConfig == null)
        {
            //petUI.model.SetActive(false);
            if(monsterUI.isPlayAnim) monsterUI.DoDeadAnim();
            return;
        }

        string modelId = "";
        int ran = Random.Range(0, 3);
        switch (ran)
        {
            case 0:
                modelId = isBoss ? AdventureTableManager.Instance.GetAdventureBossmodel1(curConfig.id) : AdventureTableManager.Instance.GetAdventureMmodel1(curConfig.id);
                break;
            case 1:
                modelId = isBoss ? AdventureTableManager.Instance.GetAdventureBossmodel2(curConfig.id) : AdventureTableManager.Instance.GetAdventureMmodel2(curConfig.id);
                break;
            case 2:
                modelId = isBoss ? AdventureTableManager.Instance.GetAdventureBossmodel3(curConfig.id) : AdventureTableManager.Instance.GetAdventureMmodel3(curConfig.id);
                break;
        }
        //modelId = "401001";
        if (string.IsNullOrEmpty(modelId) || monsterUI == null) return;

        //petUI.model.SetActive(true);
        monsterUI.ShowModel(modelId, isBoss);

        monsterRefreshSch = Timer.Instance.Invoke(10, ShowNormalMonster);
    }

    void ShowNormalMonster(Schedule sch)
    {
        RefreshMonsterModel(false);
    }


    void RefreshPetModel()
    {
        petUI?.SetModelId(CSWoLongInfo.Instance.ReturnZhanHunModel(1));
        petUI?.SetModelId(CSWoLongInfo.Instance.ReturnZhanHunModel(2), 2);
    }


    #region CEvent
    void AdventureInfoChange(uint id, object data)
    {
        RefreshRewardTime();
    }

    void BossInfoChange(uint id, object data)
    {
        GetCurAdventureConfig();
        RefreshMonsterModel(true);
    }

    void PetStateInfoChange(uint id, object data)
    {
        GetCurAdventureConfig();
        RefreshExpAndMoneyUI();
    }

    void MonsterDead(uint id, object data)
    {
        if (itemUIMgr == null) return;
        bool isBoss = (bool)data;
        if (isBoss)
        {
            for (int i = 0; i < CSWildAdventureInfo.Instance.bossRewards.Count; i++)
            {
                string icon = ItemTableManager.Instance.GetItemIcon(CSWildAdventureInfo.Instance.bossRewards[i].id);
                itemUIMgr.GetItem(icon, Random.Range(15f, 30f));
            }
        }
        else
        {
            itemUIMgr.GetItem("1993", Random.Range(15f, 30f));
        }
    }

    #endregion


    #region click
    void CloseClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIWildEscapadePanel>();
    }

    void GetEquipClick(GameObject go)
    {
        string getWatStr = SundryTableManager.Instance.GetSundryEffect(602);
        Utility.ShowGetWay(getWatStr);
    }

    void OpenRewardsClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIWildEscapadeAwardPanel>();
    }
    #endregion
}


public class UIAdventureMonster : IDispose
{
    public GameObject model;
    TweenPosition tweenPos;
    GameObject anim;


    string curModelId;

    Schedule schReset;

    public bool isPlayAnim;

    bool isBoss;

    const float ResetAfterDeadTime = 1f;


    EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    public void InitObj(GameObject go)
    {
        model = go;
        model.transform.localScale = new Vector3(1, 1, 1);
        tweenPos = go.GetComponent<TweenPosition>();
        anim = go.transform.GetChild(0).gameObject;
        tweenPos.onFinished.Clear();
        tweenPos.AddOnFinished(DoAttackAnim);

        tweenPos.ResetToBeginning();

        isPlayAnim = false;
        if (mClientEvent == null) mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    }


    public void ShowModel(string modelId, bool boss = false)
    {
        if (model == null) return;


        if (!isPlayAnim)
        {
            curModelId = modelId;
            isBoss = boss;
            InitAnims();
        }
        else
        {
            DoDeadAnim();
            curModelId = modelId;
            isBoss = boss;
            schReset = Timer.Instance.Invoke(ResetAfterDeadTime, ResetModelTimer);
        }
    }



    void InitAnims()
    {
        if (model == null) return;
        isPlayAnim = true;
        tweenPos.ResetToBeginning();
        tweenPos.PlayForward();
        CSEffectPlayMgr.Instance.ShowUIEffect(anim, $"{curModelId}_Walk_2", ResourceType.MonsterAtlas);
        //Debug.LogError("ModelId:" + curModelId);

    }


    void DoAttackAnim()
    {
        //Debug.LogError("ModelId:" + curModelId);
        CSEffectPlayMgr.Instance.ShowUIEffect(anim, $"{curModelId}_Attack_2", ResourceType.MonsterAtlas);
        mClientEvent.SendEvent(CEvent.WildAdventureMonsterAttack);
    }


    public void DoDeadAnim()
    {
        //Debug.LogError("ModelId:" + curModelId);
        CSEffectPlayMgr.Instance.ShowUIEffect(anim, $"{curModelId}_Dead_0", ResourceType.MonsterAtlas, 10, false);
        mClientEvent.SendEvent(CEvent.WildAdventureMonsterDead, isBoss);
    }
    

    void ResetModelTimer(Schedule sch)
    {
        isPlayAnim = false;
        InitAnims();
    }


    public void Dispose()
    {
        if (mClientEvent != null) mClientEvent.UnRegAll();
        mClientEvent = null;
        if (anim != null) CSEffectPlayMgr.Instance.Recycle(anim);
        model = null;
        anim = null;
        tweenPos = null;

        Timer.Instance.CancelInvoke(schReset);
    }
}



public class UIAdventurePet : IDispose
{
    public GameObject model;
    GameObject body;
    GameObject weapon;

    string curModelId = "";
    string curWeaponId = "";


    EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    int animState;//0�ޣ�1վ����2����

    public void InitObj(GameObject go)
    {
        model = go;
        body = go.transform.GetChild(0).gameObject;
        weapon = go.transform.GetChild(1).gameObject;

        model.transform.localScale = new Vector3(1, 1, 1);

        if (mClientEvent == null) mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
        mClientEvent.Reg((uint)CEvent.WildAdventureMonsterAttack, MonsterAttack);
        mClientEvent.Reg((uint)CEvent.WildAdventureMonsterDead, MonsterDead);

        animState = 0;
    }


    public void SetModelId(string id, int _type = 1)//_type 1����2�·�
    {
        if (_type == 1) curWeaponId = id;
        else curModelId = id;
        //Debug.LogError("@@@@@PetModelId:" + curModelId);
        if (animState == 2) DoAttackAnim();
        else DoStandAnim();
    }

    public void DoAttackAnim()
    {
        if (string.IsNullOrEmpty(curModelId) || model == null || body == null)
        {
            animState = 0;
            return;
        }
        CSEffectPlayMgr.Instance.ShowUIEffect(body, $"{curModelId}_Attack_1", ResourceType.PlayerAtlas);

        if (!string.IsNullOrEmpty(curWeaponId) && weapon != null)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(weapon, $"{curWeaponId}_Attack_1", ResourceType.WeaponAtlas);
        }

        animState = 2;
    }


    public void DoStandAnim()
    {
        if (string.IsNullOrEmpty(curModelId) || model == null || body == null)
        {
            animState = 0;
            return;
        }
        CSEffectPlayMgr.Instance.ShowUIEffect(body, $"{curModelId}_Stand_1", ResourceType.PlayerAtlas);
        if (!string.IsNullOrEmpty(curWeaponId) && weapon != null)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(weapon, $"{curWeaponId}_Stand_1", ResourceType.WeaponAtlas);
        }

        animState = 1;
    }



    void MonsterAttack(uint id, object data)
    {
        DoAttackAnim();
    }

    void MonsterDead(uint id, object data)
    {
        DoStandAnim();
    }


    public void Dispose()
    {
        if (mClientEvent != null) mClientEvent.UnRegAll();
        mClientEvent = null;
        if (body != null) CSEffectPlayMgr.Instance.Recycle(body);
        if (weapon != null) CSEffectPlayMgr.Instance.Recycle(weapon);
        model = null;
        body = null;
        weapon = null;
    }
}


public class UIAdventurePlayerAvatar : IDispose
{
    public GameObject model;
    GameObject body;
    GameObject weapon;

    EventHanlderManager mClientEvent;

    public void InitObj(GameObject go)
    {
        model = go;
        body = model.transform.GetChild(0).gameObject;
        weapon = model.transform.GetChild(1).gameObject;

        model.transform.localScale = new Vector3(1, 1, 1);

        if (mClientEvent == null) mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
        mClientEvent.Reg((uint)CEvent.WildAdventureMonsterAttack, MonsterAttack);
        mClientEvent.Reg((uint)CEvent.WildAdventureMonsterDead, MonsterDead);
    }

    public void DoAttackAnim()
    {
        CSMainPlayerInfo info = CSMainPlayerInfo.Instance;
        int motion = info.Career == ECareer.Master ? CSMotion.Attack2 : CSMotion.Attack;
        AvatarModelHelper.LoadSceneAvatarModel(body, info.BodyModel, info.FashionCloth,motion, CSDirection.Right_Down, ModelStructure.Body);
        AvatarModelHelper.LoadSceneAvatarModel(weapon, info.Weapon, info.FashionWeapon, motion, CSDirection.Right_Down, ModelStructure.Weapon,ResourceType.WeaponAtlas);
    }


    public void DoStandAnim()
    {
        CSMainPlayerInfo info = CSMainPlayerInfo.Instance;
        AvatarModelHelper.LoadSceneAvatarModel(body, info.BodyModel, info.FashionCloth,CSMotion.Stand, CSDirection.Right_Down, ModelStructure.Body);
        AvatarModelHelper.LoadSceneAvatarModel(weapon, info.Weapon, info.FashionWeapon, CSMotion.Stand, CSDirection.Right_Down, ModelStructure.Weapon, ResourceType.WeaponAtlas);
    }


    void MonsterAttack(uint id, object data)
    {
        DoAttackAnim();
    }

    void MonsterDead(uint id, object data)
    {
        DoStandAnim();
    }


    public void Dispose()
    {
        if (mClientEvent != null) mClientEvent.UnRegAll();
        mClientEvent = null;
        model = null;
        if (body != null) CSEffectPlayMgr.Instance.Recycle(body);
        if (weapon != null) CSEffectPlayMgr.Instance.Recycle(weapon);
    }
}