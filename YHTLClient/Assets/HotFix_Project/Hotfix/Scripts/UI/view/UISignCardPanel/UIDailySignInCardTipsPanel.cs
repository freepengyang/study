using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIDailySignInCardTipsPanel : UIBasePanel
{
	protected UILabel mlb_cardName;
	protected UIEventListener mbtn_break;
	protected UISprite msp_frame;
	protected UISprite msp_card;
    protected UIGridContainer mGrid_collection;
    protected GameObject mobj_arrow;
    protected UIScrollBar mscrollBar;
    protected UIScrollView mscroll;
    protected GameObject mobj_lock;
    protected UIWrapContent mwrap;
    protected UISprite mbg;

    SignCardData data;

    EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData> endLessList;


    protected override void _InitScriptBinder()
	{
		mlb_cardName = ScriptBinder.GetObject("lb_cardName") as UILabel;
        mbtn_break = ScriptBinder.GetObject("btn_lock") as UIEventListener;
		msp_frame = ScriptBinder.GetObject("sp_frame") as UISprite;
		msp_card = ScriptBinder.GetObject("sp_card") as UISprite;
        mGrid_collection = ScriptBinder.GetObject("Grid_collection") as UIGridContainer;
        mobj_arrow = ScriptBinder.GetObject("obj_arrow") as GameObject;
        mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
        mscroll = ScriptBinder.GetObject("ScrollView") as UIScrollView;
        mobj_lock = ScriptBinder.GetObject("obj_lock") as GameObject;
        mwrap = ScriptBinder.GetObject("wrap") as UIWrapContent;
        mbg = ScriptBinder.GetObject("bg") as UISprite;
    }

    public override void Init()
    {
        base.Init();
        AddCollider();

        mClientEvent.Reg((uint)CEvent.CollectionLockInfoChange, CollectionLockInfoChange);
        mClientEvent.Reg((int)CEvent.PlayerCardChange, PlayerCardChange);

        mbtn_break.onClick = BreakBtnClick;
        

    }


    public void OpenPanel(SignCardData cardData)
    {
        if (cardData == null || cardData.config == null || cardData.config.perfert == 1) return;
        data = cardData;
        TABLE.SIGNCARD cfg = cardData.config;
        mlb_cardName.text = cfg.name;
        msp_card.spriteName = cfg.pic.ToString();
        msp_frame.spriteName = CSSignCardInfo.Instance.GetCardFrameSp(cfg.quality);

        var collection = cardData.relatedCollection;
        if (collection == null) return;

        mobj_lock.SetActive(collection.Any(x => { return x.isLocked; }));

        mPoolHandleManager.RecycleAll();
        collection.Sort((a, b) =>
        {
            if (a.hasReached == b.hasReached)
            {
                int countA = a.notActiveList.Count;
                int countB = b.notActiveList.Count;
                return countA != countB ? countA - countB : a.id - b.id;
            }
            else
            {
                return a.hasReached ? -1 : 1;
            }
        });

        int length = collection.Count > 5 ? 5 : collection.Count;

        if (endLessList == null)
        {
            endLessList = new EndLessKeepHandleList<UISignCardTipsCollectionItem, SignCardCollectionData>(SortType.Vertical, mwrap, mPoolHandleManager, length, ScriptBinder);
        }
        endLessList.Clear();

        for (int i = 0; i < collection.Count; i++)
        {
            var data = collection[i];
            endLessList.Append(data);
        }

        endLessList.Bind();

        //mGrid_collection.Bind<UISignCardTipsCollectionItem, SignCardCollectionData>(collection, mPoolHandleManager);

        int offset = collection.Count > 2 ? 0 : (3 - collection.Count) * 92 - 30;
        mbg.height = collection.Count > 2 ? 402 : 402 - offset;
        mobj_arrow.transform.localPosition = new Vector2(14, -212 + offset);

        if (collection.Count > 0)
        {
            int maxlength = collection.Count * mwrap.itemSize;
            mscroll.SetDynamicArrowVerticalWithWrap(maxlength, mobj_arrow);
        }
        else mscroll.SetDynamicArrowVertical(mobj_arrow);
    }

    void CollectionLockInfoChange(uint id, object eData)
    {
        if (data != null) OpenPanel(data);
    }

    void PlayerCardChange(uint id, object eData)
    {
        UIManager.Instance.ClosePanel<UIDailySignInCardTipsPanel>();
    }

    void BreakBtnClick(GameObject go)
    {
        if (data != null)
        {
            data.TryToBreak();
        }
    }
    protected override void OnDestroy()
    {
        data = null;
        endLessList?.Destroy();
        endLessList = null;
        base.OnDestroy();
    }
}


public class UISignCardTipsCollectionItem : UIBinder
{
    UILabel lb_name;
    UILabel lb_num;
    UISprite sp_lock;
    GameObject obj_lock;
    UIGridContainer grid_card;
    UISprite sp_icon;
    GameObject obj_flag;
    GameObject obj_itemLock;
    GameObject obj_click;
    UISprite sp_quality;
    GameObject obj_redpoint;

    SignCardCollectionData collectionData;


    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_num = Get<UILabel>("lb_name/lb_num");
        sp_lock = Get<UISprite>("sp_lock");
        obj_lock = Get<GameObject>("sp_lock");
        grid_card = Get<UIGridContainer>("ScrollView/Grid");
        sp_icon = Get<UISprite>("ItemBase/sp_icon");
        obj_flag = Get<GameObject>("sp_flag");
        obj_itemLock = Get<GameObject>("ItemBase/sp_lock");
        obj_click = Get<GameObject>("click");
        sp_quality = Get<UISprite>("ItemBase/sp_quality");
        obj_redpoint = Get<GameObject>("redpoint");
    }

    public override void Bind(object _data)
    {
        SignCardCollectionData data = _data as SignCardCollectionData;
        if (data == null || data.config == null) return;
        collectionData = data;

        lb_name.text = data.config.name;

        var miniCards = data.GetMiniCardPreview();
        //满足数量要算上万能卡。但目前需求中预览的迷你卡不算万能卡。
        int activeCount = CSSignCardInfo.Instance.CollectionActivedCardCount(data);
        //int activeCount = 5 - data.notActiveList.Count;

        if (lb_num != null)
        {
            lb_num.text = $"({activeCount}/5)".BBCode(activeCount >= 5 ? ColorType.Green : ColorType.Red);
        }
        if (obj_redpoint != null)
        {
            obj_redpoint.SetActive(activeCount >= 5);
        }

        if (sp_lock != null)
        {
            sp_lock.spriteName = data.isLocked ? "lcok2" : "unlock";
        }
        if (obj_itemLock != null)
        {
            obj_itemLock.SetActive(data.isLocked);
        }

        if (obj_flag != null)
        {
            obj_flag.SetActive(/*data.hasReached*/false);
        }

        if (miniCards != null)
        {
            grid_card.Bind<MiniCardSlot, UISignCardMiniItem>(miniCards, PoolHandle);
        }

        if (obj_lock != null)
        {
            obj_lock.SetActive(data.config.require == 1);
            if (data.config.require == 1) UIEventListener.Get(obj_lock).onClick = LockBtnClick;
        }

        if (sp_icon != null)
        {
            sp_icon.spriteName = data.honorCfg.pic.ToString();
        }

        if (sp_quality != null)
        {
            sp_quality.spriteName = $"quality{data.honorCfg.quality}";
        }

        UIEventListener.Get(sp_icon.gameObject).onClick = ShowRewardClick;

        if (obj_click != null)
        {
            UIEventListener.Get(obj_click).onClick = CollectionClick;
        }
    }

    public override void OnDestroy()
    {
        if (grid_card != null)
            grid_card.UnBind<UISignCardMiniItem>();
        lb_name = null;
        lb_num = null;
        sp_lock = null;
        obj_lock = null;
        grid_card = null;
        sp_icon = null;
        obj_flag = null;
        obj_itemLock = null;
        obj_click = null;
        sp_quality = null;
        obj_redpoint = null;
    }


    void LockBtnClick(GameObject go)
    {
        if (collectionData == null) return;
        collectionData.TryToLockOrUnLock();
    }


    void ShowRewardClick(GameObject go)
    {
        if (collectionData == null) return;
        string rewardName = collectionData.honorCfg.name;
        UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
        {
            (f as UIDailySignInAwardPanel).OpenPanel(rewardName, collectionData.rewardDic);
        });
    }

    void CollectionClick(GameObject go)
    {
        if (collectionData != null)
        {
            UIManager.Instance.CreatePanel<UIDailySignInTipsPanel>((f) =>
            {
                (f as UIDailySignInTipsPanel).OpenPanel(collectionData);
            });
        }
    }

}