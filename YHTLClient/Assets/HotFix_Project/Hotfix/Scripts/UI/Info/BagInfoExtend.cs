using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSBagInfo : CSInfo<CSBagInfo>
{
    #region  特殊ID
    Dictionary<int, Action<bag.BagItemInfo>> specialItemUse = new Dictionary<int, Action<bag.BagItemInfo>>();
    void RegisterAction(int _cfgid, Action<bag.BagItemInfo> _ac)
    {
        if (!specialItemUse.ContainsKey(_cfgid))
        {
            specialItemUse.Add(_cfgid, _ac);
        }
    }
    void InitActions()
    {
        //特殊ID注册
        RegisterAction(50000028, ZhuFuYou);
		RegisterAction(30030031, Move);					//随机寻宝券
		RegisterAction(50000031, Move);					//卧龙山庄门票

		RegisterAction(50000501, Move);                 //回城传送石
        //RegisterAction(50000500, Move);                 //随机传送石

        //特殊类型注册
        RegisterAction(5, 67, BossKuangHuanTicket);
        RegisterAction(3, 13, JingLiPropUse);//增加精力值道具，使用前须判断是否达到上限 弹窗
    }
    void ZhuFuYou(bag.BagItemInfo _info)
    {
        if (!mEquipData.ContainsKey(1))
        {
            UtilityTips.ShowRedTips(789);
        }
        else
        {
            bag.BagItemInfo t_info = mEquipData[1];
            UITipsManager.Instance.CreateTips(TipsOpenType.Bag, ItemTableManager.Instance.GetItemCfg(t_info.configId), t_info);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OpenZhuFuYou);
        }
    }

    void Move(bag.BagItemInfo _info)
    {
        int id = _info.configId;
        if (id == 30030031)
            UtilityPath.FindWithDeliverId(537);
        else if (id == 50000501)
            UtilityTips.ShowPromptWordTips(73, () => { Net.CSBackCityMessage(); });
        else if (id == 50000500)
        {
            UtilityTips.ShowPromptWordTips(98, () => { Net.ReqUseItemMessage(_info.bagIndex, 1, false, 0, _info.id); });
        }
        else if (id == 50000031)
            UtilityPath.FindWithDeliverId(16);
        UIManager.Instance.ClosePanel<UIBagPanel>();
    }

    void BackCityStone(bag.BagItemInfo _info)
    {

        Net.CSBackCityMessage();
    }
    #endregion

    #region 特殊类型判断
    Dictionary<int, Action<bag.BagItemInfo,int>> specialTypeItemUse = new Dictionary<int, Action<bag.BagItemInfo,int>>();
    void RegisterAction(int _type, int _subType, Action<bag.BagItemInfo,int> _ac)
    {
        int key = _type * 1000 + _subType;
        if (!specialTypeItemUse.ContainsKey(key))
        {
            specialTypeItemUse.Add(key, _ac);
        }
    }

    void BossKuangHuanTicket(bag.BagItemInfo _info,int _count)
    {
        int activityId = 10120;
        long leftTime = UIServerActivityPanel.GetEndTime(activityId);
        if (leftTime > 0)
        {
            long activeDay = SpecialActivityTableManager.Instance.GetSpecialActivityStarttime(activityId);
            long openDay = CSMainPlayerInfo.Instance.ServerOpenDay;
            if (openDay >= activeDay)
            {
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.CreatePanel<UIServerActivityPanel>(p =>
                {
                    (p as UIServerActivityPanel).OpenPanel(OpenActivityType.ChiefCarnival);
                });
            }
            else
                UtilityTips.ShowRedTips(1282);
        }
        else
            UtilityTips.ShowRedTips(1281);
    }
    void JingLiPropUse(bag.BagItemInfo _info,int _count)
    {
        if (CSVigorInfo.Instance.IsVigorFull())
        {
            if (!Constant.ShowTipsOnceList.Contains(99))
            {
                UtilityTips.ShowPromptWordTips(99,
                    () =>
                    {
                        UtilityPanel.JumpToPanel(12605);
                        UIManager.Instance.ClosePanel<UIBagPanel>();
                    },
                () =>
                {
                    UtilityPath.FindWithDeliverId(DeliverTableManager.Instance.GetSuggestDeliverId(CSMainPlayerInfo.Instance.Level));
                    UIManager.Instance.ClosePanel<UIBagPanel>();
                });
            }
            else
            {
                Net.ReqUseItemMessage(_info.bagIndex, _count, false, 0, _info.id);
            }
        }
        else
        {
            Net.ReqUseItemMessage(_info.bagIndex, _count, false, 0, _info.id);
        }
    }
    #endregion
}
