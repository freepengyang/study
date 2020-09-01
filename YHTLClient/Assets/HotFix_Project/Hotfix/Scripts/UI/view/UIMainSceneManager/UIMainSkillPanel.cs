using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.XR;
using FlyBirds.Model;

public class CombinedSkillData
{
    int _energyPoint = 0;
    public int energyPoint
    {
        get
        {
            return _energyPoint;
        }
        set
        {
            if (_energyPoint < value && _energyPoint / 1000  + 1 <= value / 1000)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.PlayHejiEffect);
            }
            _energyPoint = value;
        }
    }
    public int eachEnergyPoints = 1000;
    public int fullEnergyPoints = 3000;
    public TABLE.HEJISKILL petConfig;
    public TABLE.HEJISKILL roleConfig;
    public delegate void OnSkillClicked(CombinedSkillData data);
    public OnSkillClicked onSkillClicked;
    public int playerSkillId
    {
        get
        {
            if (null == roleConfig)
                return 0;
            return roleConfig.skillGroup * 1000 + roleConfig.level;
        }
    }
    public int petSkillId
    {
        get
        {
            if (null == petConfig)
                return 0;
            return petConfig.skillGroup * 1000 + petConfig.level;
        }
    }
    public float frontRadio
    {
        get
        {
            if(energyPoint % eachEnergyPoints == 0)
            {
                if (energyPoint <= 0/*energyPoint < fullEnergyPoints*/)
                    return 0.0f;
                return 1.0f;
            }
            return (energyPoint % eachEnergyPoints) * 1.0f / eachEnergyPoints;
        }
    }
    public int fillNum
    {
        get
        {
            return energyPoint / eachEnergyPoints;
        }
    }
    public float backRadio
    {
        get
        {
            return energyPoint > eachEnergyPoints ? 1.0f : 0.0f;
        }
    }
    public string frontSpriteName
    {
        get
        {
            return $"circle_{(int)(energyPoint / eachEnergyPoints)}";
        }
    }
    public string backSpriteName
    {
        get
        {
            return $"circle_{Mathf.Max(0, (int)(energyPoint / eachEnergyPoints) - 1)}";
        }
    }
    public bool HasCombinedSkill
    {
        get
        {
            return null != roleConfig && null != petConfig;
        }
    }
    public bool IsEnergyEnough
    {
        get
        {
            return energyPoint >= eachEnergyPoints;
        }
    }
}

public class UICombinedSkillBinder : UIBinder
{
    UISprite sp_icon;
    UIEventListener btnCombinedSkill;
    //UISprite sp_back_radio;
    UISprite sp_front_radio;
    UILabel lb_count;
    GameObject go_effect_bg_circle;
    GameObject go_effect_bg_progress;
    GameObject go_effect_bg_up;
    CombinedSkillData combinedSkillData;
    CombinedSkillData.OnSkillClicked action;

    public void AddAction(CombinedSkillData.OnSkillClicked action)
    {
        this.action = action;
    }

    public override void Init(UIEventListener handle)
    {
        sp_icon = Get<UISprite>("petheadBtn");
        btnCombinedSkill = sp_icon.GetComponent<UIEventListener>();
        //sp_back_radio = Get<UISprite>("back_radio");
        //sp_front_radio = Get<UISprite>("front_radio");
        lb_count = Get<UILabel>("lb_count");
        lb_count.CustomActive(false);
        go_effect_bg_circle = Get<GameObject>("go_effect_bg_circle");
        go_effect_bg_progress = Get<GameObject>("go_effect_bg_progress");
        go_effect_bg_up = Get<GameObject>("go_effect_bg_up");
        //sp_back_radio.invert = true;
        sp_front_radio = go_effect_bg_progress.GetComponent<UISprite>();
        sp_front_radio.type = UIBasicSprite.Type.Filled;
        sp_front_radio.invert = true;
        btnCombinedSkill.onClick = this.OnClick;

        HotManager.Instance.EventHandler.AddEvent(CEvent.PlayHejiEffect, OnPlayHejiEffect);
    }

    protected void OnPlayHejiEffect(uint id,object argv)
    {
        go_effect_bg_up.PlayEffect(17935, 10, true, () =>
           {
               go_effect_bg_up.StopEffect();
           });
    }

    public override void Bind(object data)
    {
        combinedSkillData = data as CombinedSkillData;
        if(null != combinedSkillData)
        {
            //sp_back_radio.fillAmount = combinedSkillData.backRadio;
            sp_front_radio.fillAmount = combinedSkillData.frontRadio;

            combinedSkillData.onSkillClicked = this.action;

            go_effect_bg_progress.PlayEffect(17934);
            int fillNum = combinedSkillData.fillNum;
            if(fillNum > 0)
            {
                if (null != lb_count)
                    lb_count.text = fillNum.ToString();
                go_effect_bg_circle.PlayEffect(17933);
            }
            else
            {
                go_effect_bg_circle.StopEffect();
            }
            lb_count.CustomActive(fillNum > 0);
            if (SkillTableManager.Instance.TryGetValue(combinedSkillData.playerSkillId, out TABLE.SKILL skillItem))
            {
                sp_icon.spriteName = skillItem.icon;
            }
            //sp_back_radio.spriteName = combinedSkillData.backSpriteName;
            //sp_front_radio.spriteName = combinedSkillData.frontSpriteName;
        }
    }

    protected void OnClick(GameObject go)
    {
        if (null == combinedSkillData)
            return;

        if(!combinedSkillData.IsEnergyEnough)
        {
            UtilityTips.ShowRedTips(1999);
            return;
        }

        combinedSkillData.onSkillClicked?.Invoke(combinedSkillData);
    }

    public override void OnDestroy()
    {
        var eftMgr = CSEffectPlayMgr.Instance;
        if(null != go_effect_bg_circle)
        {
            eftMgr.Recycle(go_effect_bg_circle);
            go_effect_bg_circle = null;
        }
        if (null != go_effect_bg_progress)
        {
            eftMgr.Recycle(go_effect_bg_progress);
            go_effect_bg_progress = null;
        }
        if (null != go_effect_bg_up)
        {
            eftMgr.Recycle(go_effect_bg_up);
            go_effect_bg_up = null;
        }

        HotManager.Instance.EventHandler.RemoveEvent(CEvent.PlayHejiEffect, OnPlayHejiEffect);
        this.action = null;
        btnCombinedSkill.onClick = null;
        btnCombinedSkill = null;
        sp_icon = null;
        //sp_back_radio = null;
        sp_front_radio = null;
        if (null != combinedSkillData)
            combinedSkillData.onSkillClicked = null;
        combinedSkillData = null;
    }
}

public class SkillJoysticBinder : UIBinder
{
    public int skillGroup = 0;
    private Transform mJoystickTrans;
    private Transform mTouch;
    private GameObject mJoystickGo;
    private UISprite mSprArea;
    private Vector3 mMouseLocalPos;
    private float mAreaRadius;
    private bool mIsCancle;
    private float mPixelSizeAdjustment = 0;
    private float PixelSizeAdjustment
    {
        get
        {
            if (mPixelSizeAdjustment == 0)
            {
                if(Handle != null)
                {
                    mPixelSizeAdjustment = NGUITools.FindInParents<UIRoot>(Handle.gameObject).pixelSizeAdjustment;
                }
            }
            return mPixelSizeAdjustment;
        }
    }
    
    public void SetCancle(bool value)
    {
        if (mIsCancle != value)
        {
            mIsCancle = value;
            mSprArea.spriteName = mIsCancle ? "joystic_cancle" : "joystic_select";
        }
    }

    public override void Bind(object data)
    {
        
    }

    public override void Init(UIEventListener handle)
    {
        mJoystickTrans = handle.transform;
        mJoystickGo = handle.gameObject;
        mTouch = Get<Transform>("touch");
        mSprArea = Get<UISprite>("area");
        if(mSprArea != null)
        {
            mAreaRadius = mSprArea.width / 2;
        }

        if (CSMainPlayerInfo.Instance.Career == ECareer.Warrior)
        {
            HotManager.Instance.MainEventHandler.AddEvent(MainEvent.MainPlayer_DirectionChange, OnChangeDirection);
        }
    }

    public void Attach(Transform anchor, int group)
    {
        if(mJoystickTrans == null)
        {
            return;
        }
        if(skillGroup != group)
        {
            mJoystickTrans.parent = anchor;
            mJoystickTrans.transform.localPosition = Vector3.zero;
            mMouseLocalPos = Vector3.zero;
        }
        SetActive(true);
        SetCancle(false);
    }

    public void SetActive(bool value)
    {
        if (mJoystickGo.activeSelf != value)
        {
            mJoystickGo.SetActive(value);
        }
    }

    public void OnDragTouch(GameObject go, Vector2 delta)
    {
        //Debug.LogFormat("<color=#00ff00> delta = {0}  mouseLocalPos.magnitude = {1}   normalized = {2}  mAreaRadius = {3}</color>",
        //    delta, mMouseLocalPos.magnitude, mMouseLocalPos.normalized, mAreaRadius);
        mMouseLocalPos += (Vector3)delta * PixelSizeAdjustment;
        SetTouchPos(mMouseLocalPos);
        SetCancle(CSSkillFlagManager.Instance.IsCancle());
        //Debug.LogFormat("<color=#00ff00> OnDragTouch: {0}</color>",CSAvatarManager.MainPlayer.TouchMove.ToString());
    }

    private void SetTouchPos(Vector3 mouseLocalPos)
    {
        if (mouseLocalPos.magnitude > mAreaRadius) mTouch.localPosition = mouseLocalPos.normalized * mAreaRadius;
        else mTouch.localPosition = mouseLocalPos;
        CSSkillFlagManager.Instance.OnDrag(mouseLocalPos,CSAvatarManager.MainPlayer.GetDirection());

    }

    private void OnChangeDirection(uint evtId, object obj)
    {
        CSSkillFlagManager.Instance.OnDrag(Vector3.zero,CSAvatarManager.MainPlayer.GetDirection());
    }

    public override void OnDestroy()
    {
        skillGroup = 0;
        mMouseLocalPos = Vector3.zero;
        HotManager.Instance.MainEventHandler.UnReg((uint)MainEvent.MainPlayer_DirectionChange, OnChangeDirection);
    }
}

public class ShortCutSkillBinder : UIBinder
{
    UISprite effectPlay;
    UISprite skillIcon;
    UISprite skillLock;
    UISprite skillFrame;
    UISprite mpMask;
    UISprite cdMask;
    UISprite spAutoPlayEffect;
    UISpriteAnimation spAutoPlayAnimation;
    SkillJoysticBinder skillJoystic;

    public System.Action<int> onLaunchSkill { get; set; }

    public int SlotID { get; set; }

    public override void Init(UIEventListener handle)
    {
        spAutoPlayEffect = Handle?.GetComponent<UISprite>();
        spAutoPlayAnimation = Handle?.GetComponent<UISpriteAnimation>();
        effectPlay = Get<UISprite>("EffectPlay");
        skillIcon = Get<UISprite>("spr_skillIcon");
        skillLock = Get<UISprite>("skillFrame/lock");
        skillFrame = Get<UISprite>("skillFrame");
        mpMask = Get<UISprite>("spr_nomp");
        cdMask = Get<UISprite>("spr_cdMask");
        //handle.onClick = OnSkillClicked;
        handle.onPress = OnSkillPress;
        handle.onDrag = OnDragTouch;
        if (null != spAutoPlayAnimation)
            spAutoPlayAnimation.enabled = false;
        if (null != spAutoPlayEffect)
            spAutoPlayEffect.enabled = true;
	}

    public void SetSkillJoystic(SkillJoysticBinder skillJoysticBinder)
    {
        skillJoystic = skillJoysticBinder;
    }

    protected void OnSkillPress(GameObject go, bool state)
    {
        if(!state)
        {
            OnSkillClicked(go);
            if (skillJoystic != null)
            {
                skillJoystic.SetActive(false);
                CSSkillFlagManager.Instance.Hide();
            }
        }
        else
        {
            if(skillJoystic != null)
            {
                TABLE.SKILL skill = null;
                if (SkillTableManager.Instance.TryGetValue(this.skillId, out skill))
                {
                    if (UtilityFight.IsFlagSkill(skill.skillGroup) || skill.skillGroup == (int)ESkillGroup.YeMan)
                    {
                        skillJoystic.Attach(go.transform, 1);
                        CSSkillFlagManager.Instance.Show(this.skillId,CSMainPlayerInfo.Instance.Career,CSAvatarManager.MainPlayer.GetDirection());
                    }
                }
            }
        }
    }
        
    private void OnDragTouch(GameObject go, Vector2 delta)
    {
        if(skillJoystic != null)
        {
            skillJoystic.OnDragTouch(go,delta);
        }
    }

    protected void OnSkillClicked(GameObject go)
    {
        TABLE.SKILL skill = null;
        if (!SkillTableManager.Instance.TryGetValue(this.skillId, out skill))
            return;

        bool autoSwitch = CSSkillInfo.Instance.IsSwitchTypeSkill(this.skillId);
        if(autoSwitch)
        {
            bool autoPlay = CSSkillInfo.Instance.GetSkillAutoPlay(this.skillId);
            Net.ReqSetSkillAutoStateMessage(this.skillId,!autoPlay);
            return;
        }
        //if (InPublicCD)
        //{
        //    return;
        //}
        if (InCD)
        {
            //����CD����ʾ
            UtilityTips.ShowRedTips(547);
            return;
        }

        if (!MpEnough)
        {
            //���������Ĳ�����ʾ
            UtilityTips.ShowRedTips(548);
            return;
        }

        this.onLaunchSkill?.Invoke(this.skillId);
    }

    public bool InCD
    {
        get
        {
            return CSSkillInfo.Instance.IsSkillInCD(this.skillId);
        }
    }

    public bool MpEnough
    {
        get
        {
            if (null == skill)
                return false;
            var cur = CSSkillInfo.Instance.GetValidSkillItem(skill.id,false);
            if (null == cur)
                return false;

            if (!CSSkillInfo.Instance.GetSkillNeedCostMp(cur.skillGroup))
                return true;

            return (!(cur.mpCost > 0 && cur.mpCost > CSMainPlayerInfo.Instance.MP));
        }
    }

    public bool InPublicCD
    {
        get
        {
            return CSSkillInfo.Instance.IsSkillInPublicCD();
        }
    }

    protected int skillId;
    protected TABLE.SKILL skill;
    protected TimerEventHandle timer;
    public override void Bind(object data)
    {
        if(null != timer)
        {
            CSTimer.Instance.remove_timer(timer);
            timer = null;
        }
        if (data is int id)
        {
            skill = null;
            this.skillId = id;
            SkillTableManager.Instance.TryGetValue(this.skillId, out skill);
            bool isSwitchSkill = CSSkillInfo.Instance.IsSwitchTypeSkill(this.skillId);
            bool inCd = InCD;
            //策划说不显示锁了
            skillLock.CustomActive(null == skill && false);
            cdMask.CustomActive(null != skill && inCd && !isSwitchSkill);
            if (null != skillIcon)
            {
                if (null != skill) 
                    skillIcon.spriteName = skill.icon.ToString();
                else
                    skillIcon.spriteName = string.Empty;
            }
            skillIcon?.gameObject.SetActive(null != skill);
            mpMask?.gameObject.SetActive(!MpEnough && this.skillId > 0);

            if (SlotID >= 4)
            {
                Handle.CustomActive(null != skill);
            }
            else
            {
                if (null != skillFrame)
                {
                    skillFrame.alpha = null != skill ? 1.0f : 0.50f;
                }
            }

            if (inCd)
            {
                if(null == timer)
                    timer = CSTimer.Instance.InvokeRepeating(0.0f, 0.03f, CDSchedule);
            }
            else
            {
                if (null != timer)
                {
                    CSTimer.Instance.remove_timer(timer);
                    timer = null;
                }
            }

            if(null != Handle && null != spAutoPlayAnimation && null != spAutoPlayEffect)
            {
                //这里只是给开关类型技能打开特效
                bool needEffect = isSwitchSkill && CSSkillInfo.Instance.GetSkillAutoPlay(this.skillId);
                if (needEffect)
                {
                    if(!spAutoPlayAnimation.enabled)
                    {
                        spAutoPlayAnimation.enabled = spAutoPlayEffect.enabled = true;
                        CSEffectPlayMgr.Instance.ShowUIEffect(Handle.gameObject, "effect_skillsel_add", 10, true, false);
                    }
                }
                else
                {
                    spAutoPlayAnimation.enabled = false;
                    spAutoPlayEffect.enabled = true;
                    spAutoPlayEffect.atlas = null;
                    CSEffectPlayMgr.Instance.Recycle(Handle.gameObject);
                }
            }
        }
    }

    protected void CDSchedule()
    {
        cdMask.fillAmount = CSSkillInfo.Instance.GetSkillCDAmount(this.skillId);
    }

    public override void OnDestroy()
    {
        if (null != Handle)
        {
            CSEffectPlayMgr.Instance.Recycle(Handle.gameObject);
        }
        this.onLaunchSkill = null;
        if (null != timer)
        {
            CSTimer.Instance.remove_timer(timer);
            timer = null;
        }
        effectPlay = null;
        skillIcon = null;
        skillLock = null;
        cdMask = null;
        skillId = 0;
        skillJoystic = null;
    }
}

public partial class UIMainSkillPanel : UIBasePanel
{
    ShortCutSkillBinder[] mSkillItems = new ShortCutSkillBinder[Constant.CONST_SKILL_SHORTCUT_LENGTH];
    SkillJoysticBinder mSkillJoystic = new SkillJoysticBinder();
    UICombinedSkillBinder mCombinedSkillBinder;
    CombinedSkillData mCombinedSkillData = new CombinedSkillData();
    public override void Init()
    {
        base.Init();

        CSSkillInfo.Instance.GetSkillInstalled(0);
        mBtnCommonAtk.onClick = OnClickCommonAttack;
        mBtnAutoFight.onClick = OnClickAutoFight;
        mBtnSelectPlayer.onClick = OnClickSelectPlayer;
        mBtnSelectMonster.onClick = OnClickSelectMonster;

        InitSkillJoystic();
        InitShortCutSkillItems();
        ResetShortCutSkillItems();

        mCombinedSkillBinder = mcombinedSkill.GetOrAddBinder<UICombinedSkillBinder>(mPoolHandleManager);
        mCombinedSkillBinder.AddAction(OnCombinedSkillClicked);
        RefreshCombinedSkill();

        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MP_Change, OnPlayerMpChanged);
        mClientEvent.AddEvent(CEvent.OnSkillSlotChanged, OnSkillSlotChanged);
        mClientEvent.AddEvent(CEvent.OnSkillCDChanged, OnSkillCDChanged);
        mClientEvent.AddEvent(CEvent.AutoFight_Change, OnAutoFightChange);
        mClientEvent.AddEvent(CEvent.OnSkillAutoPlayChanged, OnSkillAutoPlayChanged);
        mClientEvent.AddEvent(CEvent.OnSkillAdded, OnSkillAdded);
        mClientEvent.AddEvent(CEvent.OnSkillRemoved, OnSkillRemoved);
		mClientEvent.AddEvent(CEvent.GetRandomThingInfo, SetPanel);
        mClientEvent.AddEvent(CEvent.GetWarPetCombinedSkill, UpdateCombinedSkill);
    }

    void OnCombinedSkillClicked(CombinedSkillData skillData)
    {
        if(null != skillData)
        {
            int playerSkillId = skillData.playerSkillId;
            if (playerSkillId > 0)
            {
                FNDebug.Log($"<color=#00ff00>[施放合体技能]:{playerSkillId}</color>");
                this.OnLaunchSkill(playerSkillId);
            }
        }
    }

    void RefreshCombinedSkill()
    {
        if(null != mCombinedSkillBinder)
        {
            var combinedData = CSSkillInfo.Instance.GetPetCombinedSkill();
            mCombinedSkillBinder.Handle.CustomActive(combinedData.HasCombinedSkill);
            if (combinedData.HasCombinedSkill)
            {
                mCombinedSkillBinder.Bind(combinedData);
            }
        }
    }

    protected void OnSkillAdded(uint id, object argv)
    {
        ResetShortCutSkillItems();
    }

    protected void OnSkillRemoved(uint id, object argv)
    {
        ResetShortCutSkillItems();
    }

    protected void OnSkillSlotChanged(uint id,object argv)
    {
        ResetShortCutSkillItems();
    }

    protected void OnPlayerMpChanged(uint id, object argv)
    {
        ResetShortCutSkillItems();
    }

    protected void OnSkillCDChanged(uint id, object argv)
    {
        ResetShortCutSkillItems();
    }

    protected void OnSkillAutoPlayChanged(uint id, object argv)
    {
        ResetShortCutSkillItems();
    }

    private void OnAutoFightChange(uint evtId, object data)
    {
        PlayAutoFightEffect();
    }

    private void UpdateCombinedSkill(uint evtId, object data)
    {
        RefreshCombinedSkill();
    }


    private void SetPanel(uint evtId, object data)
	{
		mSkillShortParent.SetActive(!(bool)data);
		mBtnSelectPlayer.gameObject.SetActive(!(bool)data);
		mBtnSelectMonster.gameObject.SetActive(!(bool)data);
	}

    protected void InitSkillJoystic()
    {
        Transform trans = mSkillShortParent.transform.Find("joystick");
        if(trans != null)
        {
            UIEventListener eventListener = UIEventListener.Get(trans.gameObject);
            mSkillJoystic.Setup(eventListener);
        }
    }
	protected void InitShortCutSkillItems()
    {
        for(int i = 0; i < mSkillItems.Length; ++i)
        {
            mSkillItems[i] = new ShortCutSkillBinder();
            mSkillItems[i].SlotID = i;
            mSkillItems[i].onLaunchSkill = this.OnLaunchSkill;
            mSkillItems[i].SetSkillJoystic(mSkillJoystic);
            var handle = mSkillShortParent.transform.Find($"initiativeSkill{i+1}");
            if(null == handle)
            {
                continue;
            }
            var eventListener = UIEventListener.Get(handle.gameObject);
            mSkillItems[i].Setup(eventListener);
        }
    }

    protected void ResetShortCutSkillItems()
    {
        for (int i = 0; i < mSkillItems.Length; ++i)
        {
            mSkillItems[i].Bind(CSSkillInfo.Instance.GetSlotValue(i));
        }
    }

    public override void Show()
    {
        base.Show();
    }

    private void OnClickCommonAttack(GameObject go)
    {
        CSSkillLaunchSystem.Instance.OnCommonAttack();
    }

    private void OnClickAutoFight(GameObject go)
    {
        CSSkillLaunchSystem.Instance.OnAutoFight();
    }

    private void OnClickSelectPlayer(GameObject go)
    {
        CSAutoFightManager.Instance.Stop();
        CSSelectTargetManager.Instance.SwitchSelectPlayer(10);
    }

    private void OnClickSelectMonster(GameObject go)
    {
        CSAutoFightManager.Instance.Stop();
        CSSelectTargetManager.Instance.SwitchSelectMonster(10);
    }

    protected void OnLaunchSkill(int skillId)
    {
        CSSkillLaunchSystem.Instance.LaunchSkill(skillId);
    }

    private void PlayAutoFightEffect()
    {
        if(mAutoFightEffect == null)
        {
            return;
        }
        if (!CSEffectPlayMgr.Instance.IsContains(mAutoFightEffect.GetHashCode()))
        {
            CSEffectPlayMgr.Instance.ShowEffectPlay(mAutoFightEffect, 17002);
        }
        mAutoFightEffect.SetActive(CSAutoFightManager.Instance.IsAutoFight);
    }


    protected override void OnDestroy()
    {
        mCombinedSkillBinder?.Destroy();
        mCombinedSkillBinder = null;

        mBtnCommonAtk.onClick = null;
        mBtnCommonAtk = null;

        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MP_Change, OnPlayerMpChanged);
        mClientEvent.RemoveEvent(CEvent.OnSkillSlotChanged, OnSkillSlotChanged);
        mClientEvent.RemoveEvent(CEvent.OnSkillCDChanged, OnSkillCDChanged);
        mClientEvent.RemoveEvent(CEvent.AutoFight_Change, OnAutoFightChange);
        mClientEvent.RemoveEvent(CEvent.OnSkillAutoPlayChanged, OnSkillAutoPlayChanged);
        mClientEvent.RemoveEvent(CEvent.OnSkillAdded, OnSkillAdded);
        mClientEvent.RemoveEvent(CEvent.OnSkillRemoved, OnSkillRemoved);
        mClientEvent.RemoveEvent(CEvent.GetWarPetCombinedSkill, UpdateCombinedSkill);

        for (int i = 0; i < mSkillItems.Length; ++i)
        {
            mSkillItems[i].OnDestroy();
            mSkillItems[i] = null;
        }
        mSkillItems = null;
        mSkillJoystic = null;
        base.OnDestroy();
        if(mAutoFightEffect != null)
        {
            CSEffectPlayMgr.Instance.Recycle(mAutoFightEffect);
        }
    }
}