using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UITipsManager
{
    static UITipsManager _insrance;
    public static UITipsManager Instance
    {
        get
        {
            if (_insrance == null)
            {
                _insrance = new UITipsManager();
            }
            return _insrance;
        }
        set { _insrance = value; }
    }
    Dictionary<int, Type> tipsDic = new Dictionary<int, Type>();
    UITipsManager()
    {
        Init();
    }
    void Init()
    {
        tipsDic.Add((int)ItemType.Money, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Equip, typeof(UIEquipTipPanel));
        tipsDic.Add((int)ItemType.Box, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Medicine, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Others, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.SkillBook, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Meterials, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Handbook, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.Gem, typeof(UIPropItemPanel));
        tipsDic.Add((int)ItemType.NostalgicEquip, typeof(UINostalgiaTipPanel));
    }

    public void CreateTips(TipsOpenType _type, bag.BagItemInfo _info, object data = null)
    {
        if (null == _info)
        {
            return;
        }
        TABLE.ITEM _cfg = null;
        if (!ItemTableManager.Instance.TryGetValue(_info.configId, out _cfg))
            return;
        if (_cfg.type == (int)ItemType.Handbook)
        {
            UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
            {
                (f as UIHandBookTipsPanel).Show(HandBookTableManager.Instance.make_id(_cfg.id, 1, 1), 0, 0);
            });
        }
        else
        {
            CreateTips(_type, _cfg, _info, data);
        }
    }

    public void CreateTips(TipsOpenType _type, TABLE.ITEM _cfg, bag.BagItemInfo _info = null, object data = null, System.Action _action = null)
    {
        if (_cfg.type == (int)ItemType.Handbook)
        {
            UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
            {
                (f as UIHandBookTipsPanel).Show(HandBookTableManager.Instance.make_id(_cfg.id, 1, 1), 0, 0);
            });
        }
        else
        {
            Type t = tipsDic[(int)_cfg.type];
            UIManager.Instance.CreatePanel(t, p =>
            {
                (p as UITipsBase).ShowTip(_type, _cfg, _info, data, _action);
            });
        }
    }
    public void CreateTips(TipsOpenType _type, int _cfgId, System.Action _action = null)
    {
        TABLE.ITEM _cfg = ItemTableManager.Instance.GetItemCfg(_cfgId);
        if (_cfg.type == (int)ItemType.Handbook)
        {
            UIManager.Instance.CreatePanel<UIHandBookTipsPanel>(f =>
            {
                (f as UIHandBookTipsPanel).Show(HandBookTableManager.Instance.make_id(_cfg.id, 1, 1), 0, 0);
            });
        }
        else
        {
            Type t = tipsDic[(int)_cfg.type];
            UIManager.Instance.CreatePanel(t, p =>
            {
                (p as UITipsBase).ShowTip(_type, _cfg, null, null, _action);
            });
        }
    }
}
