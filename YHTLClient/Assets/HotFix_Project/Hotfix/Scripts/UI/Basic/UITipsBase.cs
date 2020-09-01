using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;
using UnityEngine;

public class UITipsBase : UIBasePanel
{
    protected GameObject obj_Left;
    protected GameObject obj_Middle;
    protected GameObject obj_Right;
    public override void Init()
    {
        base.Init();
        AddCollider(this.UIPrefab);
    }
    public virtual void ShowTip(TipsOpenType _type, TABLE.ITEM _cfg, bag.BagItemInfo _info = null, object data = null, System.Action _action = null)
    {
        //SetBgHeight();
        //SetBgWidth();
    }
    public virtual void CalPosiTion()
    {

    }
    protected void ReplaceLR()
    {

    }

    protected void SetSeparation(int count = 1, bool changeScroll = false)
    {

    }
    public void Reposition()
    {

    }

    public void TipBtnClick(GameObject _go)
    {
        TipsBtnData data = (TipsBtnData)UIEventListener.Get(_go).parameter;
        switch (data.type)
        {
            case TipBtnType.XiLian:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                if (CSBagInfo.Instance.IsWoLongEquip(data.cfg))
                {
                    UIManager.Instance.CreatePanel<UIWoLongXiLianCombinePanel>(p =>
                    {
                        (p as UIWoLongXiLianCombinePanel).SelectChildPanel(1);
                        (p as UIWoLongXiLianCombinePanel).SelectItem(data);
                    });

                }
                else
                {
                    UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                    {
                        (p as UIEquipCombinePanel).SelectChildPanel(2);
                        (p as UIEquipCombinePanel).SelectItem(data);
                    });
                }
                break;
            case TipBtnType.ChongZhu:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                {
                    (p as UIEquipCombinePanel).SelectChildPanel(1);
                    (p as UIEquipCombinePanel).SelectItem(data);
                });
                break;
            case TipBtnType.Wear:
                if (data.info == null) return;
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                if (data.cfg.type == 2)
                {
                    if (data.ex_data != null)
                    {
                        int pos = (int)data.ex_data;
                        //Debug.Log("穿上部位是  --- " + pos);
                        Net.ReqEquipItemMessage(data.info.bagIndex, pos, 0, data.info);
                    }
                    else
                    {
                        //Debug.Log("穿上部位是  --- " + data.cfg.subType);
                        Net.ReqEquipItemMessage(data.info.bagIndex, (int)data.cfg.subType, 0, data.info);
                    }
                }
                else
                {
                    Net.ReqUseItemMessage(data.info.bagIndex, 1, false, 0, data.info.id);
                }
                break;
            case TipBtnType.TakeOff:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                Net.ReqUnEquipItemMessage((int)data.ex_data);
                //Debug.Log("脱下部位是  --- " + data.cfg.subType);
                break;
            case TipBtnType.Replace:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                Net.ReqEquipItemMessage(data.info.bagIndex, CSBagInfo.Instance.GetEquipWearPos(data.cfg), 0, data.info);
                break;
            case TipBtnType.Use:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                CSBagInfo.Instance.UseItem(data.info, 1, false);
                break;
            case TipBtnType.Discard:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UtilityTips.ShowPromptWordTips(26, () => { Net.ReqDestroyItemMessage(data.info.bagIndex, 0, data.info.id); });
                break;
            case TipBtnType.ReplaceLeft:
                //Debug.Log("TipBtnType.ReplaceLeft   --  " + data.cfg.subType);
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                if (data.cfg.subType == 5)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 5, 0, data.info);
                }
                else if (data.cfg.subType == 6)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 7, 0, data.info);
                }
                else if (data.cfg.subType == 105)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 105, 0, data.info);
                }
                else if (data.cfg.subType == 106)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 107, 0, data.info);
                }
                break;
            case TipBtnType.ReplaceRight:
                //Debug.Log("TipBtnType.ReplaceRight   --  "+(data.cfg.subType+1));
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);

                if (data.cfg.subType == 5)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 6, 0, data.info);
                }
                else if (data.cfg.subType == 6)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 8, 0, data.info);
                }
                else if (data.cfg.subType == 105)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 106, 0, data.info);
                }
                else if (data.cfg.subType == 106)
                {
                    Net.ReqEquipItemMessage(data.info.bagIndex, 108, 0, data.info);
                }
                break;
            case TipBtnType.Split:
                //Debug.Log("TipBtnType.Split");
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.CreatePanel<UIPromptItemSplitPanel>(p =>
                {
                    (p as UIPromptItemSplitPanel).SetData(data.info);
                });
                break;
            case TipBtnType.PutIn:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);

                Net.ReqBagToStorehouseMessage(data.info.bagIndex);
                break;
            case TipBtnType.TakeOut:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                if (data.openType == TipsOpenType.WildAdventureRewardReceive)
                {
                    CSWildAdventureInfo.Instance.TryToTakeOutItem(data.info);
                }
                else
                {
                    Net.ReqStorehouseToBagMessage(data.info.bagIndex);
                }

                break;
            case TipBtnType.Donate:
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnTipsGuildDonate, data.info);
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                break;
            case TipBtnType.Exchange:
                HotManager.Instance.EventHandler.SendEvent(CEvent.OnTipsGuildExchange, data.info);
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                break;
            case TipBtnType.Recycle:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                TABLE.ITEM itemCfg = null;
                if (!ItemTableManager.Instance.TryGetValue(data.info.configId, out itemCfg))
                {
                    break;
                }

                HotManager.Instance.EventHandler.SendEvent(CEvent.BagItemDBClicked, data.info);
                
                // if (CSItemRecycleInfo.Instance.CanAsNormalRecycle(itemCfg))
                // {
                //     //加入回收列表
                //     HotManager.Instance.EventHandler.SendEvent(CEvent.BagItemDBClicked, data.info);
                // }
                // else
                // {
                //     CSItemRecycleInfo.Instance.RecycleNeigongEquip(data.info);
                // }
                break;
            case TipBtnType.CancelRecycle:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                HotManager.Instance.EventHandler.SendEvent(CEvent.TipsBtnRecycleUnSelectd, data.info);
                break;
            case TipBtnType.Compound:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng))
                {
                    UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                    {
                        (p as UIEquipCombinePanel).SelectChildPanel(5);
                        (p as UIEquipCombinePanel).SelectItem(data);
                    });
                }
                break;
            case TipBtnType.ZhuFu:
                HotManager.Instance.EventHandler.SendEvent(CEvent.OpenZhuFuYou);
                break;
            case TipBtnType.Inlaid:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UtilityPanel.JumpToPanel(11604);
                // UIManager.Instance.CreatePanel<UIEquipCombinePanel>((f) =>
                // {
                //     (f as UIEquipCombinePanel).SelectChildPanel(4);
                // });
                break;
            case TipBtnType.PearlUpgrade:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                UIManager.Instance.CreatePanel<UIPearlCombinedPanel>(f =>
                    {
                        (f as UIPearlCombinedPanel).OpenChildPanel((int)UIPearlCombinedPanel.ChildPanelType.CPT_Upgrade);
                    }
                );
                break;
            case TipBtnType.PearlEvolution:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                UIManager.Instance.CreatePanel<UIPearlCombinedPanel>(f =>
                    {
                        (f as UIPearlCombinedPanel).OpenChildPanel((int)UIPearlCombinedPanel.ChildPanelType.CPT_Evolution);
                    }
                );
                break;
            case TipBtnType.Intensify:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                UIManager.Instance.CreatePanel<UIEquipCombinePanel>((f) =>
                {
                    (f as UIEquipCombinePanel).SelectChildPanel(3);
                });
                break;
            case TipBtnType.BatchUse:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                CSBagInfo.Instance.UseItem(data.info, data.info.count, false);
                break;
            case TipBtnType.Forge:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.ClosePanel<UIRolePanel>();
                if (CSBagInfo.Instance.IsNormalEquip(data.cfg))
                {
                    if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_xiLian))
                    {
                        if (data.info.quality < 5)
                        {
                            UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                            {
                                (p as UIEquipCombinePanel).SelectChildPanel(1);
                                (p as UIEquipCombinePanel).SelectItem(data);
                            });
                        }
                    }
                    else
                    {
                        if (data.info.quality < 5)
                        {
                            UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                            {
                                (p as UIEquipCombinePanel).SelectChildPanel(1);
                                (p as UIEquipCombinePanel).SelectItem(data);
                            });
                        }
                        else
                        {
                            UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                            {
                                (p as UIEquipCombinePanel).SelectChildPanel(2);
                                (p as UIEquipCombinePanel).SelectItem(data);
                            });
                        }
                    }
                }
                if (CSBagInfo.Instance.IsWoLongEquip(data.cfg))
                {
                    if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_LongJi))
                    {
                        if (data.info.quality < 5)
                        {
                            UIManager.Instance.CreatePanel<UIWoLongXiLianCombinePanel>(p =>
                            {
                                (p as UIWoLongXiLianCombinePanel).SelectChildPanel(1);
                                (p as UIWoLongXiLianCombinePanel).SelectItem(data);
                            });
                        }
                    }
                }
                break;
            case TipBtnType.Deal:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                UIManager.Instance.ClosePanel<UIBagPanel>();
                UIManager.Instance.CreatePanel<UIAuctionPanel>(p =>
                {
                    (p as UIAuctionPanel).SelectChildPanel(2, data);
                });

                break;
            
            case TipBtnType.HuaijiuWear:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                if (CSNostalgiaEquipInfo.Instance.isEquipBagFull())
                {
                    UtilityTips.ShowRedTips(2009);
                    break;
                }
                if (data.info != null )
                    Net.ReqPutOnMemoryEquipMessage(data.info.id,0);
                break;
            case TipBtnType.HuaijiuTakeOff:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                Net.ReqPutOnMemoryEquipMessage(data.info.id,0);
                break;
            case TipBtnType.HuaijiuLevelUp:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                //打开升阶面板
                //UtilityPanel.JumpToPanel(110);
                UIManager.Instance.CreatePanel<UINostalgiaEquipPanel>((f) =>
                {
                    //FNDebug.Log();
                    var panel = (f as UINostalgiaEquipPanel).OpenChildPanel(2);
                    if ( panel is UINostalgiaUpLevelPanel childpanel)
                    {
                        childpanel.OpenPanel(data.info.id);
                    }
                });
                
                
                break;
            case TipBtnType.HuaijiuRemove:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                Net.ReqDiscardMemoryEquipMessage(data.info.id);
                //Net.ReqPutOnMemoryEquipMessage(data.info.id,10);
                break;
            case TipBtnType.Huaijiureplace:
                HotManager.Instance.EventHandler.SendEvent(CEvent.CloseTips);
                long oldid = CSNostalgiaEquipInfo.Instance.IsRepleace(data.cfg.huaiJiuSuit, data.cfg.subType);
                if (oldid != 0)
                {
                    Net.ReqPutOnMemoryEquipMessage(data.info.id,oldid);
                }
                
                // var list = CSNostalgiaEquipInfo.Instance.EquipList;
                // long oldid = 0;
                // for (var it = list.GetEnumerator(); it.MoveNext();)
                // {
                //     HUAIJIUSUIT huaijiusuit;
                //     
                //     if (HuaiJiuSuitTableManager.Instance.TryGetValue(data.cfg.huaiJiuSuit,out huaijiusuit))
                //     {
                //         var suit = it.Current.Value.Huaijiusuit;
                //         if (suit.type == huaijiusuit.type && suit.rank < huaijiusuit.rank && data.cfg.subType ==  it.Current.Value.item.subType)
                //         {
                //             oldid = it.Current.Value.bagiteminfo.id;
                //         } 
                //     }
                // }
                
                break;
            
        }
    }
}
