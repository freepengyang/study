using UnityEngine;
using System.Collections.Generic;
using System;
using user;
using FlyBirds.Model;

//游戏UI主场景管理
public class UIMainSceneManager : UIBase
{
    #region forms
    private GameObject _uiMainSkillPanel;

    private GameObject uiMainSkillPanelObj
    {
        get
        {
            return _uiMainSkillPanel
                ? _uiMainSkillPanel
                : (_uiMainSkillPanel = Get<GameObject>("bottom_right/Root/UIMainSkillPanel"));
        }
    }

    private GameObject _uIJoystickPanel;

    private GameObject uIJoystickPanelObj
    {
        get
        {
            return _uIJoystickPanel
                ? _uIJoystickPanel
                : (_uIJoystickPanel = Get<GameObject>("bottom_left/Root/UIJoystickPanel"));
        }
    }

    private GameObject _uIRoleHead;

    private GameObject uIRoleHeadObj
    {
        get { return _uIRoleHead ? _uIRoleHead : (_uIRoleHead = Get<GameObject>("bottom/Root/UIRoleHead")); }
    }

    private GameObject _uIPetHead;

    private GameObject uIPetHeadObj
    {
        get { return _uIPetHead ? _uIPetHead : (_uIPetHead = Get<GameObject>("top_left/Root/UIPetHead")); }
    }
	private GameObject _uIIngotHead;

	private GameObject uIIngotHead
	{
		get { return _uIIngotHead ? _uIIngotHead : (_uIIngotHead = Get<GameObject>("top_left/Root/UIIngotHeadPanel")); }
	}

	private GameObject _uIActivities;

    private GameObject uIActivitiesObj
    {
        get
        {
            return _uIActivities
                ? _uIActivities
                : (_uIActivities = Get<GameObject>("top_right/Root/UIActivitiesPanel"));
        }
    }

    private GameObject _uIFeatureTurnTable;

    private GameObject uIFeatureTurnTableObj
    {
        get
        {
            return _uIFeatureTurnTable
                ? _uIFeatureTurnTable
                : (_uIFeatureTurnTable = Get<GameObject>("bottom_right/Root/UIFeatureTurnTable"));
        }
    }

    private GameObject _uIPhoneState;

    private GameObject uIPhoneStateObj
    {
        get
        {
            return _uIPhoneState ? _uIPhoneState : (_uIPhoneState = Get<GameObject>("bottom/Root/UIPhoneState"));
        }
    }

    private GameObject _uIShortcutItem;

    private GameObject uIShortcutItemObj
    {
        get
        {
            return _uIShortcutItem
                ? _uIShortcutItem
                : (_uIShortcutItem = Get<GameObject>("bottom/Root/UIShortcutItemPanel"));
        }
    }

    private GameObject _uIMiniMapPanel;

    private GameObject uIMiniMapPanelObj
    {
        get
        {
            return _uIMiniMapPanel
                ? _uIMiniMapPanel
                : (_uIMiniMapPanel = Get<GameObject>("top_right/Root/UIMiniMapPanel"));
        }
    }

    private GameObject _UISceneChat;

    private GameObject uISceneChatObj
    {
        get { return _UISceneChat ? _UISceneChat : (_UISceneChat = Get<GameObject>("bottom/Root/UISceneChat")); }
    }

    private GameObject _uIRoleInfo;

    private GameObject uIRoleInfoObj
    {
        get { return _uIRoleInfo ? _uIRoleInfo : (_uIRoleInfo = Get<GameObject>("bottom/Root/UIRoleInfo")); }
    }

    private GameObject _uIFlyShoe;

    private GameObject uIFlyShoe
    {
        get { return _uIFlyShoe ? _uIFlyShoe : (_uIFlyShoe = Get<GameObject>("bottom/Root/UIFlyShoe")); }
    }

    private GameObject _uIMissionHint;

    private GameObject uIMissionHint
    {
        get { return _uIMissionHint ? _uIMissionHint : (_uIFlyShoe = Get<GameObject>("bottom/Root/UIMissionHint")); }
    }

    private GameObject _btn_bagBtn;

    public GameObject btn_bagBtn
    {
        get { return _btn_bagBtn ? _btn_bagBtn : (_btn_bagBtn = Get<GameObject>("btn_bag", uIRoleInfoObj.transform)); }
    }

    private GameObject _chatVoiceMenuHandle;

    private GameObject ChatVoiceMenuHandle
    {
        get
        {
            return _chatVoiceMenuHandle
                ? _chatVoiceMenuHandle
                : (_chatVoiceMenuHandle =
                    Get<GameObject>("left/Root/UIChatVoiceMenuPanel"));
        }
    }

    GameObject _UIFunctionPrompt;

    private GameObject UIFunctionPrompt
    {
        get
        {
            return _UIFunctionPrompt
                ? _UIFunctionPrompt
                : (_UIFunctionPrompt = Get<GameObject>("bottom/Root/FunctionPrompt"));
        }
    }

    private TweenPosition[] _tweenPositions;

    private TweenPosition[] tweenPositions
    {
        get { return _tweenPositions ?? (_tweenPositions = UIPrefabTrans.GetComponentsInChildren<TweenPosition>()); }
    }

    GameObject _PkMode;

    GameObject PkMode
    {
        get { return _PkMode ? _PkMode : (_PkMode = Get<GameObject>("bottom/Root/UIPkModePanel")); }
    }

    GameObject _UpcomingActive;

    GameObject UpcomingActive
    {
        get
        {
            return _UpcomingActive ? _UpcomingActive : (_UpcomingActive = Get<GameObject>("top_right/Root/UIUpcomingActivitiesPanel"));
        }
    }

    GameObject _UIMainSceneTopLeftPanel;

    GameObject UIMainSceneTopLeftPanel
    {
        get
        {
            return _UIMainSceneTopLeftPanel ? _UIMainSceneTopLeftPanel : (_UIMainSceneTopLeftPanel = Get<GameObject>("top_left/Root/UIMainSceneTopLeftPanel"));
        }
    }

    GameObject _UIForceIncrease;

    GameObject uIForceIncrease
    {
        get
        {
            return _UIForceIncrease ? _UIForceIncrease : (_UIForceIncrease = Get<GameObject>("bottom_right/Root/UIForceIncrease"));
        }
    }

    ItemPicker _ItemPicker;
    ItemPicker ItemPicker
    {
        get
        {
            return _ItemPicker ? _ItemPicker : (_ItemPicker = Get<ItemPicker>("bottom/Root/UIRoleInfo/ItemPicker"));
        }
    }
    GameObject _mainSceneCenter;
    GameObject mainSceneCenter { get { return _mainSceneCenter ? _mainSceneCenter : (_mainSceneCenter = Get<GameObject>("center")); } }

    private TweenAlpha[] _tweenAlphas;

    private TweenAlpha[] tweenAlphas
    {
        get { return _tweenAlphas ?? (_tweenAlphas = UIPrefabTrans.GetComponentsInChildren<TweenAlpha>()); }
    }

    #endregion
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public Map<Type, UIBase> _RegisterPanel = new Map<Type, UIBase>(32);

    public const int MAINSCENE_TWEEN_GROUP = 111;

    public override void Init()
    {
        base.Init();
        InitComponent();
        mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
        mClientEvent.AddEvent(CEvent.OnPickupItemPlayEffect, OnPickupItemPlayEffect);
        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_11);

#if GODLIKE
        CSTimer.Instance.InvokeRepeating(0.0f, 0.1f, () =>
          {
              Net.GMCommand("@attr 2147483647 2147483647 2147483647");
          });
#endif
    }

    public override void Show()
    {
        base.Show();
    }

    private void MoveUIMainScenePanel(uint id, object data)
    {
        if (data == null) return;
        if ((bool)data)
        {
            for (int i = 0; i < tweenPositions.Length; i++)
            {
                if (tweenPositions[i].tweenGroup == MAINSCENE_TWEEN_GROUP)
                    tweenPositions[i].PlayForward();
            }
            for (int i = 0; i < tweenAlphas.Length; i++)
            {
                if (tweenAlphas[i].tweenGroup == MAINSCENE_TWEEN_GROUP)
                    tweenAlphas[i].PlayForward();
            }
        }
        else
        {
            for (int i = 0; i < tweenPositions.Length; i++)
            {
                if (tweenPositions[i].tweenGroup == MAINSCENE_TWEEN_GROUP)
                    tweenPositions[i].PlayReverse();
            }
            for (int i = 0; i < tweenAlphas.Length; i++)
            {
                if (tweenAlphas[i].tweenGroup == MAINSCENE_TWEEN_GROUP)
                    tweenAlphas[i].PlayReverse();
            }
        }
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnPickupItemPlayEffect, OnPickupItemPlayEffect);
        if (_RegisterPanel != null && _RegisterPanel.Count > 0)
        {
            for (_RegisterPanel.Begin(); _RegisterPanel.Next();)
            {
                UIBase uiBase = _RegisterPanel.Value;
                if (uiBase != null)
                    uiBase.Destroy();
            }

            _RegisterPanel.Clear();
        }

        _RegisterPanel = null;

        base.OnDestroy();
    }

#region 初始化面板

    private void InitComponent()
    {
        RegisterPanel<UIMainSkillPanel>(uiMainSkillPanelObj);
        RegisterPanel<UIRoleHead>(uIRoleHeadObj);
        RegisterPanel<UIActivitiesPanel>(uIActivitiesObj);
        RegisterPanel<UIFeatureTurnTable>(uIFeatureTurnTableObj);
        RegisterPanel<UIPhoneState>(uIPhoneStateObj);
        RegisterPanel<UIShortcutItemPanel>(uIShortcutItemObj);
        RegisterPanel<UIMiniMapPanel>(uIMiniMapPanelObj);
        RegisterPanel<UIJoystickPanel>(uIJoystickPanelObj);
        RegisterPanel<UISceneChat>(uISceneChatObj);
        RegisterPanel<UIRoleInfo>(uIRoleInfoObj);
        RegisterPanel<UIChatVoiceMenuPanel>(ChatVoiceMenuHandle);
        RegisterPanel<UIFunctionPrompt>(UIFunctionPrompt);
        RegisterPanel<UIPkModePanel>(PkMode);
        RegisterPanel<UIUpcomingActivitiesPanel>(UpcomingActive);
        RegisterPanel<UIMainSceneTopLeftPanel>(UIMainSceneTopLeftPanel);
        RegisterPanel<UIFlyShoe>(uIFlyShoe);
        RegisterPanel<UIMissionHint>(uIMissionHint);
        RegisterPanel<UIPetHead>(uIPetHeadObj);
        RegisterPanel<UIForceIncrease>(uIForceIncrease);
		RegisterPanel<UIIngotHeadPanel>(uIIngotHead);
	}

    void OnPickupItemPlayEffect(uint id, object argv)
    {
        bag.PickupMsg info = argv as bag.PickupMsg;
        if (info == null)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed info is Null");
            return;
        }
        var instance = CSDropManager.Instance;
        if (null == instance)
        {
            FNDebug.LogErrorFormat("[PickItem]:CSDropManager is Null");
            return;
        }
        CSItem item = instance.GetItem(info.id);
        if (null == item)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed id = {0} itemTbl is Null", info.id);
            return;
        }

        if (item.OldCell == null)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed id = {0} OldCell is Null", info.id);
            return;
        }

        if (null == item.itemTbl)
        {
            FNDebug.LogErrorFormat("[PickItem]:Failed id = {0} itemTbl is Null", info.id);
            return;
        }

        Vector3 pos = CSScene.Sington.WorldTransformPoint(item.OldCell.LocalPosition2);
        Pick(pos, item.itemTbl.id);
    }

    public bool GetItemNeedPickEffect(int itemId)
    {
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(itemId, out item))
        {
            return false;
        }

        if (item.quality >= (int)ItemQualityFilterType.IQFT_ORANGE)
            return true;

        bool isWolongEquip = CSBagInfo.Instance.IsWoLongEquip(item);
        if (isWolongEquip && item.quality >= (int)ItemQualityFilterType.IQFT_PURPLE)
            return true;

        return false;
    }

    const string flyIconPath = @"spr_icon";
    const string flyEffect = @"spr_effect";
    public void Pick(Vector3 itemWorldPosition, int itemId)
    {
        //Debug.LogFormat("[Pick]: ready pick [{0}]", itemId);
        TABLE.ITEM item = null;
        if (!ItemTableManager.Instance.TryGetValue(itemId, out item) || null == item)
        {
            FNDebug.LogErrorFormat("[Pick]:item can not be found in item table id = {0}", itemId);
            return;
        }

        if (null == ItemPicker)
        {
            FNDebug.LogError("[Pick]:picker is null");
            return;
        }

        ItemPicker.Pick(itemWorldPosition, f =>
        {
            if (null == f)
            {
                FNDebug.LogError("[Pick]:picker onCreate failed");
                return;
            }

            var sp_transform = f.transform.Find(flyIconPath);
            if (null == sp_transform)
            {
                FNDebug.LogErrorFormat("[Pick]:picker flyIcon find failed path = {0}", flyIconPath);
                return;
            }

            var sprite = sp_transform.GetComponent<UISprite>();
            if (null != sprite)
            {
                if (null == sprite.atlas)
                {
                    GameObject goItemIcon = CSGameManager.Instance.GetStaticObj("ItemIcon") as GameObject;
                    if (null != goItemIcon)
                        sprite.atlas = goItemIcon.GetComponent<UIAtlas>();
                }
                sprite.transform.localScale = Vector3.one * 0.50f;
                sprite.spriteName = itemId.Icon();
            }
            else
            {
                FNDebug.LogError("[Pick]:picker flyIcon Get CSSprite Failed");
            }
            var spriteAnimation = f.transform.Find(flyEffect);
            if (null != spriteAnimation)
            {
                if (GetItemNeedPickEffect(itemId))
                {
                    spriteAnimation.CustomActive(true);
                    CSEffectPlayMgr.Instance.ShowParticleEffect(spriteAnimation.gameObject, 17101, 0, true, 1, false, Vector3.zero);
                    //17101 17906
                    //spriteAnimation.gameObject.PlayEffect(17101);
                }
                else
                {
                    spriteAnimation.CustomActive(false);
                    //spriteAnimation.gameObject.StopEffect();
                }
            }
        }, 0.60f);
    }

    private void RegisterPanel<T>(GameObject go) where T : UIBase, new()
    {
        UIBase type;
        if (!_RegisterPanel.ContainsKey(typeof(T)))
        {
            type = new T()
            {
                UIPrefab = go,
            };
            type.Init();
            type.Show();

            _RegisterPanel.Add(typeof(T), type);
        }
        else
        {
            type = _RegisterPanel[typeof(T)];
            type.Show();
        }
    }

#endregion
}