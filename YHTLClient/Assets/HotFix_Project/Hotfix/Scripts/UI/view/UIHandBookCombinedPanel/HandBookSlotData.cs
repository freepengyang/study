using CSPool;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;
using UnityEngine.XR;

public class HandBookSlotData : IndexedItem
{
    public int Index { get; set; }

    public enum CardFlag
    {
        CF_SELECTED = (1 << 1),
        CF_LOCKED = (1 << 2),
        CF_ADDED = (1 << 3),
        CF_ENABLE_SELECTED = (1 << 4),
        CF_ENABLE_UNSELETED = (1 << 5),
        CF_SETUP_MODE = (1 << 6),
        CF_DISABLE_MASK_MODE = (1 << 7),
        CF_UPGRADE_LEVEL = (1 << 8),
        CF_UPGRADE_QUALITY = (1 << 9),
        CF_ENABLE_CLICKED = (1 << 10),
        CF_REDPOINT = (1 << 11),
        CF_UNLOCK_EFFECT = (1 << 12),
        CF_ENABLE_UPGRADE_QUALITY_FLAG = (1 << 13),
        CF_ENABLE_UPGRADE_QUALITY_FILTER = (1 << 14),
        CF_ENABLE_UPGRADE_LEVEL_FLAG = (1 << 15),
        CF_BOOKMARK_REMOVED_FLAG = CF_ENABLE_SELECTED | CF_ENABLE_UNSELETED| CF_REDPOINT| CF_UNLOCK_EFFECT,
    }

    public const int CONST_BOOKMARK_REMOVED_FLAG = (int)(CardFlag.CF_ENABLE_SELECTED | CardFlag.CF_ENABLE_UNSELETED | CardFlag.CF_REDPOINT | CardFlag.CF_UNLOCK_EFFECT);

    public bool ApplyDirty { get; set; }

    long _guid;

    public bool Bind;

    public long Guid
    {
        get { return _guid; }
        set { _guid = value; }
    }

    int _flag;

    public int Flag
    {
        get { return _flag; }
    }

    public int maked_key;

    int _handBookId;

    public int HandBookId
    {
        get { return _handBookId; }
        set
        {
            _handBookId = value;
            bookItem = null;
            HandBookTableManager.Instance.TryGetValue(_handBookId, out bookItem);
            linkedItem = null;

            if (null != bookItem)
            {
                //品级相同、阵营相同、地图相同、地位相同
                ItemTableManager.Instance.TryGetValue(bookItem.ItemID, out linkedItem);
                //maked_key = ((bookItem.Quality & 0xFF) << 24) | ((bookItem.camp & 0xFF) << 16) | ((bookItem.sitemap & 0xFF) << 8) | (bookItem.status & 0xFF);
                //品级相同、itemID相同
                maked_key = (((bookItem.Quality & 0xFF) << 8) | (bookItem.ItemID & 0xFF));
            }
            else
                maked_key = 0;

            if (null == bookItem)
            {
                AddFlag(CardFlag.CF_ADDED);
            }
            else
            {
                RemoveFlag(CardFlag.CF_ADDED);
            }
        }
    }

    public void SetupHandBook(TABLE.HANDBOOK handBook)
    {
        _handBookId = handBook.id;
        bookItem = handBook;
    }

    public bool levelFull
    {
        get { return null != bookItem && CSHandBookManager.Instance.NextLevel(bookItem) == bookItem; }
    }

    public int maxLv
    {
        get
        {
            if (null == bookItem)
                return 0;
            HANDBOOK current = bookItem;
            HANDBOOK next = CSHandBookManager.Instance.NextLevel(bookItem);
            while (current != next && next != null)
            {
                current = next;
                next = CSHandBookManager.Instance.NextLevel(current);
            }

            return current.Level;
        }
    }

    public bool qualityFull
    {
        get { return null != bookItem && CSHandBookManager.Instance.NextQuality(bookItem) == bookItem; }
    }

    int _slotId;

    public int SlotID
    {
        get { return _slotId; }
        set
        {
            _slotId = value;
            slot = null;

            HandBookSlotTableManager.Instance.TryGetValue(_slotId, out slot);
        }
    }

    protected int make_key(int qua, int camp, int map, int position)
    {
        int key = ((qua & 0xFF) << 24) | ((camp & 0xFF) << 16) | ((map & 0xFF) << 8) | (position & 0xFF);
        return key;
    }

    public bool SetupedChoiceFilter()
    {
        if (this.Setuped)
        {
            UtilityTips.ShowPromptWordTips(89, () => { }, () => { this.onChoiceChanged?.Invoke(this, true); });
            return true;
        }

        return false;
    }


    public void ResetSlotData()
    {
        SlotID = 0;
        HandBookId = 0;
        Guid = 0;
    }

    bool _opened;

    public bool Opened
    {
        get { return _opened; }
        set
        {
            _opened = value;
            if (!_opened)
                AddFlag(CardFlag.CF_LOCKED);
            else
                RemoveFlag(CardFlag.CF_LOCKED);
        }
    }

    public bool CanUpgrade
    {
        get
        {
            if (null == bookItem)
                return false;
            if (levelFull)
                return false;
            if (!bookItem.levelUpCost.IsItemsEnough())
                return false;
            return true;
        }
    }

    //是否可以安装图鉴
    public bool CheckCanSetupHandBookOnIt
    {
        get
        {
            if (Guid > 0 && null != HandBook)
                return false;
            if (!Opened)
                return false;
            HandBookSlotData slot,card;
            return CSHandBookManager.Instance.CheckHandBookSetupRedPoint(out slot,out card);
        }
    }

    public bool CheckCanUnlockForSlot
    {
        get
        {
            if (Opened)
                return false;

            if (null == HandBookSlot)
                return false;

            if (!CSHandBookManager.Instance.CanUnlockSlot(SlotID, false))
            {
                return false;
            }

            if (!CSHandBookManager.Instance.HandBookOpenSlotCostId.IsItemEnough(HandBookSlot.cost, 0, false))
            {
                return false;
            }

            return true;
        }
    }

    public bool CheckUpgrade
    {
        get
        {
            if (null == bookItem)
                return false;
            if (levelFull)
            {
                UtilityTips.ShowRedTips(659);
                return false;
            }

            if (!bookItem.levelUpCost.IsItemsEnough(667, true))
                return false;
            return true;
        }
    }

    public bool CanUpgradeQuality
    {
        get
        {
            if (!CanUpgradeQualityIgnoreItemEnough)
                return false;
            if (!bookItem.qualityUpCost.IsItemsEnough())
                return false;
            return true;
        }
    }

    public bool CanUpgradeQualityIgnoreItemEnough
    {
        get
        {
            if (null == bookItem)
                return false;
            if (qualityFull)
                return false;
            int ownedSameQualityCount = CSHandBookManager.Instance.GetHandBookCountByMergeKey(maked_key);
            if (ownedSameQualityCount < 3)
                return false;
            return true;
        }
    }

    public bool CallUpgradeQuality
    {
        get
        {
            if (null == bookItem)
                return false;
            if (qualityFull)
            {
                UtilityTips.ShowRedTips(660);
                return false;
            }

            int ownedSameQualityCount = CSHandBookManager.Instance.GetHandBookCountByMergeKey(maked_key);
            if (ownedSameQualityCount < 3)
            {
                UtilityTips.ShowRedTips(675);
                return false;
            }

            if (!bookItem.qualityUpCost.IsItemsEnough(668, true))
                return false;
            return true;
        }
    }

    public bool Setuped
    {
        get
        {
            //装配模式不显示已经装配
            return SlotID > 0 && null != HandBook && !HasFlag(CardFlag.CF_SETUP_MODE);
        }
    }

    public bool CanMergeWith(HandBookSlotData other)
    {
        if (null == other)
            return false;

        if (maked_key != other.maked_key)
            return false;

        return maked_key > 0;
    }

    protected TABLE.HANDBOOK bookItem;
    protected TABLE.ITEM linkedItem;
    protected TABLE.HANDBOOKSLOT slot;

    public TABLE.HANDBOOKSLOT HandBookSlot
    {
        get { return slot; }
    }

    public TABLE.HANDBOOK HandBook
    {
        get { return bookItem; }
    }

    public TABLE.ITEM LinkedItem
    {
        get { return linkedItem; }
    }

    public System.Func<bool> onChoiceFilter;
    public System.Action<HandBookSlotData, bool> onChoiceChanged;
    public System.Action<HandBookSlotData> onClicked;
    public System.Action<HandBookSlotData> onKeepPressed;

    public void Reset()
    {
        Bind = false;
        _guid = 0;
        _flag = 0;
        _handBookId = 0;
        bookItem = null;
        linkedItem = null;
        onClicked = null;
        onKeepPressed = null;
        onChoiceChanged = null;
        onChoiceFilter = null;
        slot = null;
        _opened = false;
        ApplyDirty = false;
    }

    public bool HasFlag(CardFlag cardFlag)
    {
        int v = (int) cardFlag;
        return (_flag & v) == v;
    }

    public void AddFlag(CardFlag cardFlag)
    {
        int v = (int) cardFlag;
        _flag |= v;
    }

    public void RemoveFlag(CardFlag cardFlag)
    {
        int v = (int) cardFlag;
        _flag &= ~v;
    }

    public void RemoveFlag(int v)
    {
        _flag &= ~v;
    }

    public void AddExtraFlag(int cardFlag)
    {
        _flag |= cardFlag;
    }

    public void RemoveExtraFlag(int cardFlag)
    {
        _flag &= ~cardFlag;
    }

    public string GetBgSprite()
    {
        return bookItem.BgIcon();
    }

    public string GetQualitySprite()
    {
        return bookItem.QualityIcon();
    }

    public string GetCampShortName()
    {
        return bookItem.CampShortName();
    }

    public string GetMapShortName()
    {
        return bookItem.MapShortName();
    }

    public string GetPositionShortName()
    {
        return bookItem.PositionShortName();
    }

    public string GetLevel()
    {
        return bookItem.NumericLevel();
    }
}

public static class HandBookExtend
{
    public static string CampName(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSHandBookManager.Instance.GetSuitConditionName((int) CSHandBookManager.HandBookOpMode.HBOM_CAMP + 1,
            bookItem.camp);
    }

    public static string CampShortName(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSHandBookManager.Instance.GetCampShortName(bookItem.camp - 1);
    }

    public static string MapName(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSHandBookManager.Instance.GetSuitConditionName((int) CSHandBookManager.HandBookOpMode.HBOM_MAP + 1,
            bookItem.sitemap);
    }

    public static string MapShortName(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSHandBookManager.Instance.GetMapShortName(bookItem.sitemap - 1);
    }

    public static string PositionShortName(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSHandBookManager.Instance.GetPositionShortName(bookItem.status - 1);
    }

    public static string NumericLevel(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return $"{bookItem.Level}";
    }

    public static string TipsLevel(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return CSString.Format(721, bookItem.Level);
    }

    public static string QualityIcon(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return $"edge{bookItem.Quality}";
    }

    public static string QualityEffect(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return "effect_handbook_quality1";
        return $"effect_handbook_quality{bookItem.Quality}";
    }

    public static string QualityName(this HANDBOOK bookItem)
    {
        return CSHandBookManager.Instance.GetHandBookQualityName(bookItem);
    }

    public static string BgIcon(this HANDBOOK bookItem)
    {
        if (null == bookItem)
            return string.Empty;
        return bookItem.ItemID.Icon();
    }

    public static bool Actived(this HANDBOOKSUIT suit)
    {
        if (null != suit)
        {
            int need = suit.requirenum;
            int owned = CSHandBookManager.Instance.GetSetupedSuitCount(suit.judgeType, suit.judgeValue);
            return owned >= need;
        }

        return true;
    }
}