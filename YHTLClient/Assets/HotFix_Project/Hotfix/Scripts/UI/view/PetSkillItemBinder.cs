using Google.Protobuf.Collections;
using UnityEngine;

public class PetSkillItemBinder : UIBinder
{
    PetSkillItemData mData;
    public override void Bind(object data)
    {
        mData = data as PetSkillItemData;
        if (null == mData)
            return;

        bool activeSkill = this.mData.IsActiveSkill;
        bool learned = this.mData.learned;
        bool locked = !this.mData.HasFlag(PetSkillItemData.SkillFlag.SF_UNLOCKED);
        bool special = this.mData.HasFlag(PetSkillItemData.SkillFlag.SF_SPECIAL);

        if(activeSkill)
        {
            //���ü��ܵȼ���ѧϰ����ʾ
            go_skill_lv.CustomActive(learned);

            //ͼ�����ص�
            if (null != btn_skill)
            {
                btn_skill.parameter = mData;
                btn_skill.onClick = OnSkillSelected;
            }

            //�������ֱ���
            if (null != sp_skill_name_bg)
                sp_skill_name_bg.enabled = true;

            //��������
            if (null != lb_name)
                lb_name.text = $"{this.mData.item.name.BBCode(ColorType.SecondaryText)}";

            //���ܵȼ�
            if (null != lb_level)
            {
                lb_level.text = $"{this.mData.item.level}".BBCode(ColorType.MainText);
            }

            //����ͼ��
            if (null != sp_icon)
            {
                sp_icon.spriteName = this.mData.item.icon;
                sp_icon.color = this.mData.learned ? Color.white : Color.black;
            }

            //��������
            sp_skill_up_arrow.CustomActive(this.mData.learned && this.mData.canUpgrade);
        }
        else
        {
            //���ü��ܵȼ���ѧϰ����ʾ
            go_skill_lv.CustomActive(null != this.mData.item);

            //ͼ�����ص�
            if (null != btn_skill)
            {
                btn_skill.parameter = mData;
                btn_skill.onClick = OnSkillSelected;
            }

            //��������ͼ��[����Ѿ�û����]
            sp_lock.CustomActive(false);

            //���ÿ������ܵ�ͼ��
            sp_add.CustomActive(null == this.mData.item);

            //����Ѿ�����
            if (null != this.mData.item)
            {
                //�������ֱ���
                if (null != sp_skill_name_bg)
                    sp_skill_name_bg.enabled = true;

                //��������
                if (null != lb_name)
                {
                    lb_name.text = $"{this.mData.item.name.BBCode(ColorType.SecondaryText)}";
                }

                if(this.mData.HasFlag(PetSkillItemData.SkillFlag.SF_SPECIAL))
                {
                    //���ܵȼ�
                    if (null != lb_level)
                    {
                        lb_level.text = $"{this.mData.item.level}".BBCode(ColorType.MainText);
                    }
                    go_skill_lv.CustomActive(true);
                }
                else
                {
                    go_skill_lv.CustomActive(false);
                }

                //����ͼ��
                sp_icon.CustomActive(true);
                if (null != sp_icon)
                {
                    sp_icon.spriteName = this.mData.item.icon;
                    sp_icon.color = this.mData.learned ? Color.white : Color.black;
                }

                //��������
                sp_skill_up_arrow.CustomActive(this.mData.canUpgrade && this.mData.learned);
            }
            else
            {
                //ͼ��
                sp_icon.CustomActive(false);
                sp_skill_up_arrow.CustomActive(false);

                //�������ֱ���
                if (null != sp_skill_name_bg)
                    sp_skill_name_bg.enabled = false;

                if (locked)
                {
                    //��������
                    if (null != lb_name)
                    {
                        //lb_name.text = CSString.Format(1536, 15).BBCode(ColorType.SecondaryText);
                        lb_name.text = CSString.Format(1537).BBCode(ColorType.SecondaryText);
                    }
                }
                else
                {
                    //�����ȡ����
                    if (null != lb_name)
                        lb_name.text = CSString.Format(1537).BBCode(ColorType.SecondaryText);
                }
            }
        }
    }

    public static bool HasChoiced()
    {
        return null != ms_selected;
    }

    public static bool IsActived()
    {
        if (null == ms_selected)
            return false;

        if (null == ms_selected.mData)
            return false;

        return !ms_selected.mData.HasFlag(PetSkillItemData.SkillFlag.SF_DEACTIVED);
    }

    public static int ChoicedSkillPos
    {
        get
        {
            if (null == ms_selected)
                return -1;

            if (null == ms_selected.mData)
                return -1;

            return ms_selected.mData.Pos;
        }
    }

    UISprite sp_skill_up_arrow;
    UISprite sp_icon;
    UISprite sp_lock;
    UISprite sp_add;
    UISprite sp_skill_name_bg;
    UILabel lb_name;
    UILabel lb_level;
    UIEventListener btn_skill;
    Transform go_skill_lv;
    UISprite sp_choiced;
    public override void Init(UIEventListener handle)
    {
        sp_icon = Get<UISprite>("sp_skill_icon");
        sp_lock = Get<UISprite>("sp_lock");
        sp_add = Get<UISprite>("sp_add");
        if (null != sp_add)
        {
            UIEventListener.Get(sp_add.gameObject).onClick = this.OnSkillSelected;
        }
        sp_skill_name_bg = Get<UISprite>("sp_skill_name");
        lb_name = Get<UILabel>("sp_skill_name/lb_skill_name");
        lb_level = Get<UILabel>("sp_skill_lv/lb_skill_lv");
        go_skill_lv = handle.transform.Find("sp_skill_lv");
        sp_choiced = Get<UISprite>("sp_choiced");
        sp_skill_up_arrow = Get<UISprite>("sp_skill_up_arrow");
        btn_skill = UIEventListener.Get(sp_icon.gameObject, null);

        HotManager.Instance.EventHandler.AddEvent(CEvent.PetSkillUpgradeSucceed, OnPetSkillUpgradeSucceed);
        HotManager.Instance.EventHandler.AddEvent(CEvent.PetSkillSelected, OnPetSkillSelected);
    }

    protected void OnPetSkillUpgradeSucceed(uint id, object argv)
    {
        PetSkillItemData newSkill = argv as PetSkillItemData;
        if (null != newSkill && null != this.mData && null != this.mData.item && this.mData.item.skillGroup == newSkill.item.skillGroup)
        {
            Bind(newSkill);
        }
    }

    protected void OnPetSkillSelected(uint id, object argv)
    {
        int skillId = (int)argv;
        if (skillId == 0)
        {
            SetSelected(false);
        }
        else
        {
            if (null != this.mData && null != this.mData.item && this.mData.item.id == skillId)
            {
                if (ms_selected != this)
                    ms_selected?.SetSelected(false);
                ms_selected = this;
                ms_selected?.SetSelected(true);
            }
        }
    }

    protected static PetSkillItemBinder ms_selected;
    public static void Clear()
    {
        ms_selected = null;
    }

    protected void SetSelected(bool selected)
    {
        sp_choiced.CustomActive(selected);
        CSEffectPlayMgr.Instance.ShowUIEffect(sp_choiced.gameObject, "effect_skillsel_add", 10, true, false);
    }

    protected void OnSkillSelected(GameObject go)
    {
        if(null != this.mData && null != this.mData.item)
        {
            if(this.mData.IsActiveSkill || this.mData.learned)
            {
                if (ms_selected != this)
                    ms_selected?.SetSelected(false);
                ms_selected = this;
                ms_selected?.SetSelected(true);
            }
        }
        mData?.onSkillClicked?.Invoke(mData);
    }

    public override void OnDestroy()
    {
        if (null != sp_choiced)
        {
            CSEffectPlayMgr.Instance.Recycle(sp_choiced.gameObject);
        }
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.PetSkillUpgradeSucceed, OnPetSkillUpgradeSucceed);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.PetSkillSelected, OnPetSkillSelected);
        sp_skill_up_arrow = null;
        sp_icon = null;
        lb_name = null;
        lb_level = null;
        if (null != btn_skill)
        {
            btn_skill.onClick = null;
            btn_skill = null;
        }
        go_skill_lv = null;
        sp_choiced = null;
    }
}

public class PetSkillItemData : IndexedItem
{
    public int Index { get; set; }

    public static void Compare(ref long sortValue, PetSkillItemData r)
    {
        sortValue = r.item.id;
    }

    public static int BdjnCompare(PetSkillItemData l, PetSkillItemData r)
    {
        if (l.HasFlag(SkillFlag.SF_SPECIAL) != r.HasFlag(SkillFlag.SF_SPECIAL))
            return l.HasFlag(SkillFlag.SF_SPECIAL) ? 1 : -1;
        return l.Pos - r.Pos;
    }

    public enum SkillFlag
    {
        SF_DEACTIVED = (1 << 0),//�������ܱ�־λ��1
        SF_UNLOCKED = (1<<1),//��λ�Ѿ�����
        SF_SPECIAL = (1 << 2),//���⼼��
        SF_LEARNED = (1 << 3),//�Ƿ��Ѿ�ѧϰ
        SF_POS = (0x0F << 4),//��λλ��
    }

    public int Flag;

    public int XilanId;
    public int SortId;

    public RepeatedField<pet.PetSkillAttr> PetSkillAttrs { get; set; }

    public WarPetSkillData warPetSkillData { get; set; }

    public int skillGroupId
    {
        get
        {
            if (null == item)
                return 0;
            return item.skillGroup;
        }
    }


    public void AddFlag(SkillFlag flag)
    {
        Flag |= (int)flag;
    }

    public void Reset()
    {
        Flag = 0;
        XilanId = 0;
        item = null;
        _nextItem = null;
        PetSkillAttrs = null;
        warPetSkillData = null;
    }

    public int Pos
    {
        get
        {
            return (Flag & (int)SkillFlag.SF_POS) >> 4;
        }
        set
        {
            Flag |= (value & 0x0F) << 4;
        }
    }

    public void RemoveFlag(SkillFlag flag)
    {
        Flag &= ~(int)flag;
    }

    public bool HasFlag(SkillFlag flag)
    {
        return (Flag & (int)flag) == (int)flag;
    }

    /// <summary>
    /// �Ƿ�����������
    /// </summary>
    public bool IsActiveSkill
    {
        get
        {
            return !HasFlag(SkillFlag.SF_DEACTIVED);
        }
    }

    public bool QualityFly
    {
        get
        {
            TABLE.SKILL nvi = nextItem;
            return null != nvi && !string.IsNullOrEmpty(nvi.Speciallv) && item != nvi;
        }
    }

    public bool hasSpecialEffect
    {
        get
        {
            return nextHighEffectDistance > 0;
        }
    }
    public int nextHighEffectDistance
    {
        get
        {
            var civ = item;
            if (null == civ)
                return -1;

            int nextDis = CSSkillInfo.Instance.GetNextSpecialDistance(civ.id);
            if (learned)
                return nextDis;
            return nextDis + 1;
        }
    }

    TABLE.SKILL _item;
    public TABLE.SKILL item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (null == _item)
                SortId = 0;
            else
                SortId = value.showorder;
        }
    }
    protected TABLE.SKILL _nextItem;
    public TABLE.SKILL nextItem
    {
        get
        {
            return learned ? _nextItem : item;
        }
        set
        {
            _nextItem = value;
        }
    }
    public bool isLevelFull
    {
        get
        {
            return null != item && learned && item == nextItem;
        }
    }

    public bool IsTalentEnough(bool callTips = false)
    {
        if (null == item)
            return false;
        
        if(!this.learned)
        {
            int curPetTalentLevel = CSSkillInfo.Instance.GetPetTalentLevel();
            int studyNeedTalentLevel = CSSkillInfo.Instance.GetStudySkillNeedTalentLevel(item.skillGroup);
            bool talentEnough = curPetTalentLevel >= studyNeedTalentLevel;

            if (!talentEnough)
            {
                if (callTips)
                    UtilityTips.ShowRedTips(CSString.Format(1535, studyNeedTalentLevel));
                return false;
            }
        }
        else
        {
            int curPetLevel = CSSkillInfo.Instance.GetPetLevel();
            bool petLevelEnough = curPetLevel >= nextItem.studyLevel;
            if(!petLevelEnough)
            {
                if (callTips)
                    UtilityTips.ShowRedTips(CSString.Format(1530, nextItem.studyLevel));
                return false;
            }
        }
        return true;
    }

    public bool callUpgrade
    {
        get
        {
            if(!IsActiveSkill)
            {
                return false;
            }

            if (null == nextItem || null == item)
                return false;

            if (isLevelFull)
            {
                UtilityTips.ShowRedTips(544);
                return false;
            }

            if (!IsTalentEnough(true))
                return false;

            if (!nextItem.cost.IsItemsEnough(543, true))
                return false;

            return true;
        }
    }
    public bool learned
    {
        get
        {
            if (IsActiveSkill)
            {
                return null != item && CSSkillInfo.Instance.HasPetSkillLearned(item.id);
            }
            else
            {
                return null != item && HasFlag(SkillFlag.SF_LEARNED);
            }
        }
    }

    public bool canUpgrade
    {
        get
        {
            if (null == nextItem || null == item)
                return false;

            if (!IsActiveSkill)
            {
                return false;
            }

            if (isLevelFull)
                return false;

            if (!IsTalentEnough())
                return false;

            if (!nextItem.cost.IsItemsEnough())
                return false;

            return true;
        }
    }

    public System.Action<PetSkillItemData> onSkillClicked;
}