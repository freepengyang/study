using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIHandBookTipsPanel : UIBasePanel
{
    public override UILayerType PanelLayerType => UILayerType.Tips;
    public enum MenuType
    {
        MT_UPGRADE_LEVEL = 0,//升级
        MT_UPGRADE_QUALITY = 1,//升品
        MT_REPLACE = 2,//替换
        MT_UNLOAD = 3,//卸下
        MT_MENU_FROM_GETWAY = 4,//从GETWAYLIST列表中获得
        MT_NO_MENU = 31,//不显示菜单
    }

    public override void Init()
    {
        base.Init();

        mBG.onClick = this.Close;
        if (null != mScrollBar)
            EventDelegate.Add(mScrollBar.onChange, InitArrow);
        Panel.alpha = 0.0f;
    }

    protected TABLE.HANDBOOK handbook;
    protected TABLE.ITEM item;
    protected HandBookSlotData mSlotData;
    protected int mFilterFlag = -1;

    protected override void OnDestroy()
    {
        mFunctionTriggered = null;
        if (null != mEventParam)
        {
            System.Array.Clear(mEventParam, 0, mEventParam.Length);
            mEventParam = null;
        }
        handbook = null;
        item = null;
        mSlotData = null;
        base.OnDestroy();
    }

    const int defaultFlag = ~(1 << (int)MenuType.MT_NO_MENU | 1 << (int)MenuType.MT_MENU_FROM_GETWAY);
    System.Action mFunctionTriggered;

    public void Show(int handbookId,long guid,int filterFlag = defaultFlag,System.Action cbFunctionTriggered = null)
    {
        mFunctionTriggered = cbFunctionTriggered;
        mFilterFlag = filterFlag;
        mSlotData = CSHandBookManager.Instance.GetOwnedHandBook(guid);
        if (!HandBookTableManager.Instance.TryGetValue(handbookId, out handbook) || null == handbook)
        {
            return;
        }
        if (!ItemTableManager.Instance.TryGetValue(handbook.ItemID, out item))
            return;

        BindCoroutine(9527, PopTips());
    }

    public void Show(int handbookId, HandBookSlotData slotData, int filterFlag = ~(1 << (int)MenuType.MT_NO_MENU))
    {
        mFilterFlag = filterFlag;
        mSlotData = slotData;
        if (!HandBookTableManager.Instance.TryGetValue(handbookId, out handbook) || null == handbook)
        {
            return;
        }
        if (!ItemTableManager.Instance.TryGetValue(handbook.ItemID, out item))
            return;

        BindCoroutine(9527, PopTips());
    }

    protected IEnumerator PopTips()
    {
        Panel.alpha = 0.05f;
        InitContents();
        InitArrow();
        InitButtons();
        yield return null;
        Panel.alpha = 1.0f;
    }

    protected void InitArrow()
    {
        mDownArrow.CustomActive(mScrollBar.value < 1.0f && mScrollView.shouldMoveVertically);
        mUpArrow.CustomActive(mScrollBar.value > 0 && mScrollView.shouldMoveVertically);
    }

    bool HasFlag(MenuType menuType)
    {
        return (mFilterFlag & (1 << (int)menuType)) == (1 << (int)menuType);
    }

    protected void InitButtonsFromGetWay()
    {
        var getWayList = item.getWay.Split('#');
        int cnt = 0;
        for(int i = 0,max = getWayList.Length;i < max;++i)
        {
            if(int.TryParse(getWayList[i],out int Id) && GetWayTableManager.Instance.TryGetValue(Id,out TABLE.GETWAY getWayItem))
            {
                var go = Object.Instantiate(mBtnTemp.gameObject, mBtnList.transform) as GameObject;
                var text = go.transform.Find("lb_text").gameObject.GetComponent<UILabel>();
                var bgSprite = go.GetComponent<UISprite>();
                if (null != bgSprite)
                {
                    bgSprite.spriteName = "btn_samll2";
                }
                text.text = $"[cfbfb0]{getWayItem.name}[-]";
                //text.text = $"[b0bbcf]{getWayItem.name}[-]";
                UIEventListener.Get(go, getWayItem).onClick = f =>
                {
                    OnClick(getWayItem);
                };
                mBtnList.AddChild(go.transform);
                ++cnt;
            }
        }
        mBtnList.Reposition();
        mBtnTemp.CustomActive(false);
        mButtonRoot.CustomActive(cnt > 0);
    }

    protected void OnClick(TABLE.GETWAY getWayItem)
    {
        Utility.DoGetWayFunc(getWayItem.id);
        this.Close();
    }

    protected void InitButtons()
    {
        if(null == mSlotData || null == mBtnList || null == mBtnTemp)
        {
            mButtonRoot.CustomActive(false);
            return;
        }

        if(HasFlag(MenuType.MT_NO_MENU))
        {
            mButtonRoot.CustomActive(false);
            return;
        }

        if(HasFlag(MenuType.MT_MENU_FROM_GETWAY))
        {
            InitButtonsFromGetWay();
            return;
        }

        List<MenuType> menuItems = mPoolHandleManager.GetSystemClass<List<MenuType>>();
        if (/*mSlotData.CanUpgrade && BUG12149*/HasFlag(MenuType.MT_UPGRADE_LEVEL))
        {
            menuItems.Add(MenuType.MT_UPGRADE_LEVEL);
        }
        if (/*mSlotData.CanUpgradeQuality && BUG12149*/HasFlag(MenuType.MT_UPGRADE_QUALITY))
        {
            menuItems.Add(MenuType.MT_UPGRADE_QUALITY);
        }
        if (mSlotData.Setuped && HasFlag(MenuType.MT_UNLOAD))
        {
            menuItems.Add(MenuType.MT_UNLOAD);
        }
        if (mSlotData.SlotID != 0 && HasFlag(MenuType.MT_REPLACE))
        {
            menuItems.Add(MenuType.MT_REPLACE);
        }

        int sundryId = 403;
        TABLE.SUNDRY sundryItem;
        if (SundryTableManager.Instance.TryGetValue(sundryId, out sundryItem))
        {
            var tokens = sundryItem.effect.Split('#');
            for (int i = 0; i < menuItems.Count; ++i)
            {
                var go = Object.Instantiate(mBtnTemp.gameObject, mBtnList.transform) as GameObject;
                var text = go.transform.Find("lb_text").gameObject.GetComponent<UILabel>();
                var menuType = menuItems[i];
                var bgSprite = go.GetComponent<UISprite>();
                if (null != bgSprite)
                {
                    if(menuType == MenuType.MT_REPLACE)
                    {
                        bgSprite.spriteName = "btn_samll1";
                    }
                    else
                    {
                        bgSprite.spriteName = "btn_samll2";
                    }
                }
                if(menuType != MenuType.MT_REPLACE)
                {
                    text.text = $"[cfbfb0]{tokens[(int)menuItems[i]]}[-]";
                }
                else
                {
                    text.text = $"[b0bbcf]{tokens[(int)menuItems[i]]}[-]";
                }
                UIEventListener.Get(go, menuItems[i]).onClick = f =>
                {
                    OnClick(menuType);
                };
                mBtnList.AddChild(go.transform);
            }
            mBtnList.Reposition();
            mBtnTemp.CustomActive(false);
        }

        if (menuItems.Count <= 0)
        {
            mButtonRoot.CustomActive(false);
        }

        menuItems.Clear();
        mPoolHandleManager.Recycle(menuItems);
    }

    protected object[] mEventParam = new object[2];
    protected void OnClick(MenuType menuType)
    {
        switch (menuType)
        {
            case MenuType.MT_UPGRADE_LEVEL:
                if(null != UIManager.Instance.GetPanel<UIHandBookCombinedPanel>())
                {
                    mEventParam[0] = UIHandBookCombinedPanel.ChildPanelType.CPT_UPGRADE;
                    mEventParam[1] = mSlotData;
                    mClientEvent.SendEvent(CEvent.OnHandBookTabChanged, mEventParam);
                }
                else
                {
                    UIManager.Instance.CreatePanel<UIHandBookCombinedPanel>(f =>
                    {
                        (f as UIHandBookCombinedPanel).OpenChildPanel((int)UIHandBookCombinedPanel.ChildPanelType.CPT_UPGRADE);
                    });
                }
                break;
            case MenuType.MT_UPGRADE_QUALITY:
                if (null != UIManager.Instance.GetPanel<UIHandBookCombinedPanel>())
                {
                    mEventParam[0] = UIHandBookCombinedPanel.ChildPanelType.CPT_MERGE;
                    mEventParam[1] = mSlotData;
                    mClientEvent.SendEvent(CEvent.OnHandBookTabChanged, mEventParam);
                }
                else
                {
                    UIManager.Instance.CreatePanel<UIHandBookCombinedPanel>(f =>
                    {
                        (f as UIHandBookCombinedPanel).OpenChildPanel((int)UIHandBookCombinedPanel.ChildPanelType.CPT_MERGE);
                    });
                }
                break;
            case MenuType.MT_REPLACE:
                UIManager.Instance.CreatePanel<UIHandBookCardSelectPanel>();
                break;
            case MenuType.MT_UNLOAD:
                Net.CSTujianInlayMessage(mSlotData.Guid,0);
                break;
        }
        mFunctionTriggered?.Invoke();
        mFunctionTriggered = null;
        this.Close();
    }

    protected void InitContents()
    {
        mlb_bind.CustomActive(null != mSlotData && mSlotData.Bind);

        if(null != mtx_quality)
        {
            CSEffectPlayMgr.Instance.ShowUITexture(mtx_quality.gameObject, $"qualitybg{handbook.Quality}");
        }

        if(null != msp_quality)
        {
            msp_quality.spriteName = handbook.Quality.QualityIcon();
        }

        if (null != msp_icon)
            msp_icon.spriteName = handbook.ItemID.Icon();

        if (null != mlb_name && null != handbook)
            mlb_name.text = handbook.ItemID.ItemName().BBCode(handbook.Quality);

        if (null != mlb_level)
            mlb_level.text = handbook.TipsLevel();

        if (null != mlb_quality)
            mlb_quality.text = CSString.Format(702, handbook.QualityName());

        if (null != mlb_camp_name)
            mlb_camp_name.text = handbook.CampShortName();

        if (null != mlb_card_level)
            mlb_card_level.text = handbook.NumericLevel();

        if (null != mlb_city_name)
            mlb_city_name.text = handbook.MapShortName();

        mgrid_attributes.MaxCount = 0;
        var attributes = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager,handbook.parameter,handbook.factor);
        CSAttributeInfo.Instance.SortByOrder(attributes);

        if(null != attributes && null != mlb_attr)
        {
            mgrid_attributes.MaxCount = attributes.Count;
            for (int i = 0; i < attributes.Count; ++i)
            {
                var kv = attributes[i];
                UILabel label = mgrid_attributes.controlList[i].GetComponent<UILabel>();
                if (null != label)
                {
                    if(kv.AttrType == AttrType.SkillDesc)
                    {
                        label.text = CSString.Format(696, NGUIText.StripSymbols(kv.Value));
                    }
                    else
                    {
                        label.text = CSString.Format(694, kv.Key, kv.Value);
                    }
                }
            }
        }
        mPoolHandleManager.RecycleAttributes(attributes);
        mlb_attr.CustomActive(false);

        var handBookMgr = CSHandBookManager.Instance;
        mgrid_skills.MaxCount = 3;
        if(true)
        {
            UILabel label = mgrid_skills.controlList[0].GetComponent<UILabel>();
            if(null != label)
            {
                //地图
                label.text = CSString.Format(703, handBookMgr.GetSuitConditionName(2, 0), handBookMgr.GetSuitConditionName(2, handbook.sitemap));
            }
        }
        if (true)
        {
            UILabel label = mgrid_skills.controlList[1].GetComponent<UILabel>();
            if (null != label)
            {
                //阵营
                label.text = CSString.Format(703, handBookMgr.GetSuitConditionName(1, 0), handBookMgr.GetSuitConditionName(1, handbook.camp));
            }
        }
        if (true)
        {
            UILabel label = mgrid_skills.controlList[2].GetComponent<UILabel>();
            if (null != label)
            {
                //地位
                label.text = CSString.Format(703, handBookMgr.GetSuitConditionName(3, 0), handBookMgr.GetSuitConditionName(3, handbook.status));
            }
        }

        if(true)
        {
            //底部描述 1
            int descHeight = 0;
            if (null != mlb_desc)
            {
                mlb_desc.text = item.tips;
                Bounds boundsDesc = NGUIMath.CalculateRelativeWidgetBounds(mlb_desc.transform);
                descHeight = (int)boundsDesc.size.y;
            }

            if (null != mDesc)
            {
                mDesc.height = descHeight;
            }
        }

        if (true)
        {
            //底部描述2
            int descHeight = 0;
            if (null != mlb_desc2)
            {
                mlb_desc2.text = item.tips2;
                Bounds boundsDesc = NGUIMath.CalculateRelativeWidgetBounds(mlb_desc2.transform);
                descHeight = (int)boundsDesc.size.y;
            }

            if (null != mDesc2)
            {
                mDesc2.height = descHeight;
            }
        }

        mgroups.Reposition();

        var bounds = NGUIMath.CalculateRelativeWidgetBounds(mWidge.transform);
        mView.height = Mathf.Min(640, (int)bounds.size.y + 135 + 10);

        mWidge.height = (int)bounds.size.y;
        mgroups.Reposition();
    }
}