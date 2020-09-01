using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using UnityEngine;

public class CSTimeExpManager : CSInfo<CSTimeExpManager>
{
    public CSTimeExpManager()
    {
        Initialize();
    }

    protected bool _has_notify = false;

    public bool HasNotify
    {
        get { return CanUpgradeStarOrRank(); }
    }

    bool IsInitialized = false;
    public List<int> stars = new List<int>(8);

    public void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        IsInitialized = true;
        stars.Clear();

        int rank = 1;
        int star = 0;
        while (true)
        {
            int id = PaoDianShenFuTableManager.Instance.make_id(star, rank);
            TABLE.PAODIANSHENFU item = null;
            if (!PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
            {
                if (star == 0)
                    break;
                else
                {
                    stars.Add(star);
                    star = 0;
                    ++rank;
                    continue;
                }
            }

            ++star;
        }
    }

    int _rank = 1;

    public int Rank
    {
        get { return _rank; }
        set { _rank = value; }
    }

    int _star = 0;

    public int Star
    {
        get { return _star; }
        set { _star = value; }
    }

    public int MaxStar(int rank)
    {
        int idx = rank - 1;
        if (idx >= 0 && idx < stars.Count)
            return stars[idx] - 1;
        return 0;
    }

    public override void Dispose()
    {
        _rank = 1;
        _star = 0;
        IsInitialized = false;
        stars.Clear();
        stars = null;

    }

    protected bool CanUpgradeStarOrRank()
    {
        TABLE.PAODIANSHENFU item = null;
        int id = PaoDianShenFuTableManager.Instance.make_id(_star, _rank);
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
        {
            return false;
        }

        var nextItem = NextLevel(_star, _rank);
        if (null == nextItem)
        {
            return false;
        }

        if (item.id == nextItem.id)
        {
            return false;
        }

        for (int i = 0; i < item.costItem.Count; ++i)
        {
            var owned = item.costItem[i].GetItemCount();
            if (i >= item.costNum.Count)
            {
                continue;
            }

            var needed = item.costNum[i];
            if (owned < needed)
            {
                return false;
            }
        }

        return true;
    }

    protected readonly IntArray defaultAttr = new IntArray();

    public IntArray GetAttrParamByOccur(TABLE.PAODIANSHENFU item, int career)
    {
        if (career == ECareer.Taoist)
        {
            return item.dsattrPara;
        }

        if (career == ECareer.Master)
        {
            return item.fsattrPara;
        }

        if (career == ECareer.Warrior)
        {
            return item.zsattrPara;
        }

        return defaultAttr;
    }

    public TABLE.PAODIANSHENFU NextLevel(int star, int rank)
    {
        TABLE.PAODIANSHENFU item = null;
        int id = PaoDianShenFuTableManager.Instance.make_id(star, rank);
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(id, out item))
        {
            return null;
        }

        TABLE.PAODIANSHENFU nextItem = null;
        int nextId = 0;
        if (item.Star + 1 >= stars[rank - 1])
        {
            nextId = PaoDianShenFuTableManager.Instance.make_id(0, rank + 1);
            if (!PaoDianShenFuTableManager.Instance.TryGetValue(nextId, out nextItem))
            {
                return item;
            }

            return nextItem;
        }

        nextId = PaoDianShenFuTableManager.Instance.make_id(star + 1, rank);
        if (!PaoDianShenFuTableManager.Instance.TryGetValue(nextId, out nextItem))
        {
            return item;
        }

        return nextItem;
    }

    public string GetAttributeStringValue(int id, int value)
    {
        TABLE.ATTRIBUTE attribute = null;
        if (!AttributeTableManager.Instance.TryGetValue(id, out attribute))
        {
            return string.Empty;
        }

        if (attribute.per == 0)
        {
            return value.ToString();
        }

        return string.Format("{0:F2}%", value * 100.0f / attribute.per);
    }

    public string GetAttributeName(int id)
    {
        TABLE.ATTRIBUTE attribute = null;
        if (!AttributeTableManager.Instance.TryGetValue(id, out attribute))
        {
            return string.Empty;
        }

        TABLE.CLIENTTIPS clientTips = null;
        if (!ClientTipsTableManager.Instance.TryGetValue((int) attribute.tipID, out clientTips))
        {
            return string.Empty;
        }

        return clientTips.context;
    }

    public void InitRankAndStar(int rank, int star)
    {
        _rank = rank;
        _star = star;
        HotManager.Instance.EventHandler.SendEvent(CEvent.TimeExpChanged);
    }

    public void SetRankAndStar(int rank, int star)
    {
        bool needShowEffect = rank > 1 && _rank + 1 == rank;
        int prevId = PaoDianShenFuTableManager.Instance.make_id(_star, _rank);
        _rank = rank;
        _star = star;
        int curId = PaoDianShenFuTableManager.Instance.make_id(_star, _rank);
        TABLE.PAODIANSHENFU prevItem = null;
        TABLE.PAODIANSHENFU currentItem = null;
        if (PaoDianShenFuTableManager.Instance.TryGetValue(prevId, out prevItem) &&
            PaoDianShenFuTableManager.Instance.TryGetValue(curId, out currentItem))
        {
            if (prevItem != currentItem && needShowEffect)
            {
                //表示升阶段
                HotManager.Instance.EventHandler.SendEvent(CEvent.TimeExpUprgaded, prevItem);
            }
        }

        //表示升星
        HotManager.Instance.EventHandler.SendEvent(CEvent.TimeExpUprgaded,null);
    }

    public int GetFixedAddTime()
    {
        int suffId = 90; //普通是90 会员是其他值
        int v = 0;
        TABLE.SUNDRY sundryItem = null;
        if (SundryTableManager.Instance.TryGetValue(suffId, out sundryItem) && int.TryParse(sundryItem.effect, out v))
        {

        }
        return v;
    }

    public int GetMonthCardAddTime()
    {
        return CSMonthCardInfo.Instance.GetShenFuTime();
    }
}