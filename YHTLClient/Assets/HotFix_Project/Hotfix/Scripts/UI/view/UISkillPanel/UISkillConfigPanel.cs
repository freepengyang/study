public class SkillConfigBinder : UIBinder
{
	public int SlotID { get; set; }
	public int Value { 
		get 
		{
			return skillId;
		} 
	}
	protected int skillId;
	protected TABLE.SKILL skillItem;
	protected UIEventListener btn_skill_icon;
	protected UnityEngine.BoxCollider skill_colider;
	protected UISprite sp_skill_icon;
	protected UISprite sp_lock;

	public override void Init(UIEventListener handle)
	{
		sp_skill_icon = Get<UISprite>("sp_skill_icon");
		btn_skill_icon = sp_skill_icon?.GetComponent<UIEventListener>();
		skill_colider = sp_skill_icon?.GetComponent<UnityEngine.BoxCollider>();
		if (null != btn_skill_icon)
		{
			btn_skill_icon.onDrop = handle.onDrop;
			btn_skill_icon.onDragStart = OnDragStart;
			btn_skill_icon.onDrag = OnDrag;
			btn_skill_icon.onDragEnd = OnDragEnd;
			btn_skill_icon.parameter = this;
		}
		sp_lock = Get<UISprite>("sp_lock");
	}

	static UnityEngine.Vector3 startPos = UnityEngine.Vector3.zero;
	protected void OnDragStart(UnityEngine.GameObject go)
	{
		//btn_skill_icon?.gameObject.SetActive(false);
		startPos = go.transform.localPosition;
	}

    protected void OnDragEnd(UnityEngine.GameObject go)
    {
		//btn_skill_icon?.gameObject.SetActive(true);
		go.transform.localPosition = startPos;
	}

    protected void OnDrag(UnityEngine.GameObject go,UnityEngine.Vector2 pos)
    {
		//go.transform.localPosition = new UnityEngine.Vector3(pos.x, pos.y,0);
	}

	public bool Locked
	{
		get
		{
			return !(null != skillItem ||
				SlotID < 4 ||
				CSSkillInfo.Instance.GetPlayerLearnedActivedSkill() > SlotID);
        }
	}

    public override void Bind(object data)
	{
		if(data is int newSkillId)
		{
			this.skillId = newSkillId;
			skillItem = null;
			SkillTableManager.Instance.TryGetValue(this.skillId, out skillItem);
			sp_skill_icon?.gameObject.SetActive(null != skillItem);
			sp_lock.CustomActive(Locked);
			if (null != skillItem && null != sp_skill_icon)
			{
				sp_skill_icon.spriteName = skillItem.icon;
			}
			if(null != skill_colider)
			{
				skill_colider.enabled = null != skillItem;
			}
        }
	}

	public override void OnDestroy()
	{
		skillId = 0;
        skillItem = null;
		btn_skill_icon.onDragStart = null;
		btn_skill_icon.onDrag = null;
		btn_skill_icon.onDrag = null;
		btn_skill_icon.onDragEnd = null;
		btn_skill_icon = null;
        skill_colider = null;
        sp_skill_icon = null;
        sp_lock = null;
    }
}

public partial class UISkillConfigPanel : UIBasePanel
{
	protected UIEventListener[] slots = new UIEventListener[Constant.CONST_SKILL_SHORTCUT_LENGTH];
	protected SkillConfigBinder[] mConfigBinders = new SkillConfigBinder[Constant.CONST_SKILL_SHORTCUT_LENGTH];
	public override void Init()
	{
		base.Init();

		InitSkillConfigs();
		ResetSkillSlots();
		mDragedSkill = null;
		mClientEvent.AddEvent(CEvent.OnSkillDragStart, OnSkillDragStart);
		mClientEvent.AddEvent(CEvent.OnSkillSlotChanged, OnSkillSlotChanged);
		mBtnResetSkill.onClick = OnBtnSkillResetClicked;
	}

	protected void OnBtnSkillResetClicked(UnityEngine.GameObject go)
	{
		CSSkillInfo.Instance.ResetSlotValues();
	}

	protected void InitSkillConfigs()
	{
		for(int i = 0; i < mConfigBinders.Length; ++i)
		{
			var tranform = ScriptBinder.transform.Find($"slot_{i + 1}");
			var eventListener = UIEventListener.Get(tranform.gameObject);
			eventListener.onDrop = this.OnDrop;
			mConfigBinders[i] = new SkillConfigBinder();
			mConfigBinders[i].Setup(eventListener);
			mConfigBinders[i].SlotID = i;
		}
	}

	protected void ResetSkillSlots()
	{
        for (int i = 0; i < Constant.CONST_SKILL_SHORTCUT_LENGTH; ++i)
        {
            mConfigBinders[i].Bind(CSSkillInfo.Instance.GetSlotValue(i));
        }
    }

	SkillInfoData mDragedSkill;
	protected void OnSkillDragStart(uint id,object argv)
	{
		if(argv is SkillInfoData skillInfo)
		{
			mDragedSkill = skillInfo;
		}
	}

	protected void OnSkillSlotChanged(uint id,object argv)
	{
		ResetSkillSlots();
	}

	protected void OnDrop(UnityEngine.GameObject to, UnityEngine.GameObject from)
	{
        if (!(UIEventListener.Get(to).parameter is SkillConfigBinder toBinder))
        {
            return;
        }

        if (toBinder.Locked)
        {
            UtilityTips.ShowRedTips(1801, toBinder.SlotID + 1);
            return;
        }

        //来源与技能配置SLOT
        if (UIEventListener.Get(from).parameter is SkillConfigBinder fromBinder)
		{
			if (fromBinder == toBinder || fromBinder.Value == 0)
				return;
            int temp = toBinder.Value;
			SetSlotValue(toBinder.SlotID, fromBinder.Value,false);
			SetSlotValue(fromBinder.SlotID, temp,true);
		}
		else
		{
            if (null == mDragedSkill)
            {
                return;
            }
            int slotId = toBinder.SlotID;
            int skillId = mDragedSkill.item.id;

			int existedSlotId = -1;
			for(int i = 0; i < Constant.CONST_SKILL_SHORTCUT_LENGTH; ++i)
			{
				int value = CSSkillInfo.Instance.GetSlotValue(i);
				if (value == skillId)
				{
					existedSlotId = i;
					break;
				}
			}

			if (existedSlotId == slotId)
				return;

			//取消已经存在的
			if(existedSlotId != -1)
				SetSlotValue(existedSlotId, 0,false);
			//设置当前值
			SetSlotValue(slotId, skillId,true);
            mDragedSkill = null;
        }
	}

	protected void SetSlotValue(int slotId,int value,bool sendMsg)
	{
        if (slotId < 0 || slotId >= mConfigBinders.Length)
            return;
		mConfigBinders[slotId].Bind(value);
		CSSkillInfo.Instance.SetSlotValue(slotId,value, sendMsg);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}

public static class SkillConfigHelper
{
	/// <summary>
	/// 技能快捷键 0-7位
	/// </summary>
	/// <param name="shortCut"></param>
	/// <returns></returns>
	public static int KeyCode(this long shortCut)
	{
		return (int)(shortCut & 0xFFFFFFFF);
	}

    /// <summary>
    /// 技能ID	8-15位
    /// </summary>
    /// <param name="shortCut"></param>
    /// <returns></returns>
    public static int KeyValue(this long shortCut)
    {
		return (int)(shortCut >> 32);
	}

    /// <summary>
    /// 技能快捷键 0-7位
    /// </summary>
    /// <param name="shortCut"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static long SetKeyCode(this long shortCut, int code)
    {
		return (shortCut >> 32 << 32) | (long)code;
    }

    /// <summary>
    /// 技能ID	8-15位
    /// </summary>
    /// <param name="shortCut"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long SetKeyValue(this long shortCut, int value)
    {
		return (shortCut << 32 >> 32) | (((long)value) << 32);
    }
}