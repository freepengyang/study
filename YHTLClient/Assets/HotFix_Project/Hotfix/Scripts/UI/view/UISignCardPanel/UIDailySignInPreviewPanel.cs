using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIDailySignInPreviewPanel : UIBasePanel
{
    CSBetterLisHot<CommonAchievementData> commonAchs = new CSBetterLisHot<CommonAchievementData>();
    UltimateAchievementData ultAch = new UltimateAchievementData();

    List<SignCardCollectionData> temp = new List<SignCardCollectionData>();

    Dictionary<int, GameObject> redpoints = new Dictionary<int, GameObject>();

    int curSelectAchievementId;

    string greenColor;
    string redColor;
    string mainColor;
    string subColor;
    string weakColor;


    int hasReachedCount = 0;
    int maxCount = 0;

    public override void Init()
    {
        base.Init();
        AddCollider();

        commonAchs = CSSignCardInfo.Instance.playerAchievement;
        ultAch = CSSignCardInfo.Instance.ultAchievementData;

        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_giftFx, "effect_activity_select_add");

        mClientEvent.Reg((int)CEvent.CollectionReachedInfoChange, CollectionReachedInfoChange);
        mClientEvent.Reg((int)CEvent.HonorChange, SC_HonorChange);
        mClientEvent.Reg((int)CEvent.UltHonorReceive, UltHonorReceive);

        mbtn_close.onClick = CloseClick;
        UIEventListener.Get(msp_icon.gameObject).onClick = GetRewardClick;

        greenColor = UtilityColor.GetColorString(ColorType.Green);
        redColor = UtilityColor.GetColorString(ColorType.Red);
        mainColor = UtilityColor.GetColorString(ColorType.MainText);
        subColor = UtilityColor.GetColorString(ColorType.SecondaryText);
        weakColor = UtilityColor.GetColorString(ColorType.WeakText);

        InitLeftUI();
    }

    public override void Show()
    {
        base.Show();


        OpenPanel();
    }


    void OpenPanel()
    {
        curSelectAchievementId = ultAch.config.id;

        mobj_common.SetActive(false);
        mobj_ult.SetActive(true);
        RefreshLeftUI();
        RefreshUltGrid();
        RefreshTopRightInfo();
    }


    void InitLeftUI()
    {
        if (redpoints == null) redpoints = new Dictionary<int, GameObject>();
        else redpoints.Clear();
        mGrid_left.MaxCount = commonAchs.Count + 1;
        for (int i = 0; i < mGrid_left.MaxCount; i++)
        {
            var go = mGrid_left.controlList[i];
            var trans = go.transform;
            UILabel name = trans.GetChild(0).GetComponent<UILabel>();
            UISprite icon = trans.GetChild(1).GetChild(0).GetComponent<UISprite>();
            UISprite quality = trans.GetChild(1).GetChild(2).GetComponent<UISprite>();

            var config = i == 0 ? ultAch.config : commonAchs[i - 1].config;
            name.text = config.name;
            icon.spriteName = config.pic.ToString();
            quality.spriteName = $"quality{config.quality}";
            int param = config.id;
            UIEventListener.Get(go, param).onClick = SwitchAchievementBtnClick;
            UIEventListener.Get(icon.gameObject, param).onClick = LeftGridIconClick;

            var red = trans.Find("redpoint").gameObject;
            redpoints.Add(param, red);
        }
    }

    void RefreshLeftUI()
    {
        mtrans_select.localPosition = Vector2.zero;
        mscroll_left.ResetPosition();
        RefreshRedpoints();
    }

    void RefreshRedpoints()
    {
        if (redpoints == null || redpoints.Count < 1) return;
        for (var it = redpoints.GetEnumerator(); it.MoveNext();)
        {
            var go = it.Current.Value;
            var id = it.Current.Key;
            if (id == ultAch.config.id)
            {
                var reachedCount = commonAchs.WhereCount((x) => { return x.receivedReward; });
                maxCount = commonAchs.Count;
                go.CustomActive(reachedCount >= commonAchs.Count);
            }
            else
            {
                var data = commonAchs.FirstOrNull((x) => { return x.id == id; });
                if (data == null) return;
                var reachedCount = data.GetReachedCollectionsCount();
                maxCount = data.collections.Count;
                go.CustomActive(reachedCount >= maxCount && !data.receivedReward);
            }
        }
    }


    void RefreshUltGrid()
    {
        mGrid_ult.MaxCount = commonAchs.Count;
        for (int i = 0; i < mGrid_ult.MaxCount; i++)
        {
            var trans = mGrid_ult.controlList[i].transform;
            UILabel name = trans.Find("lb_name").GetComponent<UILabel>();
            GameObject btn = trans.Find("btn_show").gameObject;
            UIGridContainer grid = trans.Find("Grid").GetComponent<UIGridContainer>();
            GameObject obj_check = trans.Find("sp_check").gameObject;

            int reached = commonAchs[i].collections.WhereCount(x => { return x.hasReached; });
            int max = commonAchs[i].collections.Count;
            string nameAndCount = $"{commonAchs[i].config.name}{(reached >= max ? greenColor : redColor)}({reached}/{max})";
            name.text = CSString.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1068), nameAndCount);
            obj_check?.CustomActive(commonAchs[i].receivedReward);

            UIEventListener.Get(btn, commonAchs[i].id).onClick = SwitchAchievementBtnClickAuto;
            //UIEventListener.Get(mGrid_ult.controlList[i], commonAchs[i].id).onClick = SwitchAchievementBtnClickAuto;

            grid.MaxCount = commonAchs[i].collections.Count;
            for (int j = 0; j < grid.MaxCount; j++)
            {
                var itemTrans = grid.controlList[j].transform;
                UILabel c_name = itemTrans.Find("Label").GetComponent<UILabel>();
                GameObject c_toggle = itemTrans.Find("bar").gameObject;
                c_name.text = commonAchs[i].collections[j].config.name.BBCode(commonAchs[i].collections[j].hasReached ? ColorType.MainText : ColorType.WeakText);
                c_toggle.SetActive(commonAchs[i].collections[j].hasReached);
            }
        }
    }

    void RefreshCommonGrid(int achievementId)
    {
        CommonAchievementData data = commonAchs.FirstOrNull((x) => { return x.id == achievementId; });
        if (data == null || data.collections == null) return;

        mPoolHandleManager.RecycleAll();

        if (temp == null) temp = new List<SignCardCollectionData>();
        else temp.Clear();
        for (int i = 0; i < data.collections.Count; i++)
        {
            temp.Add(data.collections[i]);
        }

        temp.Sort((a, b) =>
        {
            if (a.hasReached == b.hasReached)
            {
                int countA = a.notActiveList.Count;
                int countB = b.notActiveList.Count;
                return countA != countB ? countA - countB : a.id - b.id;
            }
            else
            {
                return a.hasReached ? 1 : -1;
            }
        });
        mGrid_common.Bind<SignCardCollectionData, UICollectionInAchItem>(temp, mPoolHandleManager);        
        
    }


    void RefreshTopRightInfo()
    {
        if (curSelectAchievementId == ultAch.config.id)
        {
            msp_icon.spriteName = ultAch.config.pic.ToString();
            //hasReachedCount = commonAchs.WhereCount((x) => { return x.GetReachedCollectionsCount() >= x.collections.Count; });
            hasReachedCount = commonAchs.WhereCount((x) => { return x.receivedReward; }); 
            maxCount = commonAchs.Count;
            mobj_giftFx.CustomActive(hasReachedCount >= commonAchs.Count);
            mobj_boxCheck.CustomActive(false);
            msp_icon.color = CSColor.white;
        }
        else
        {
            CommonAchievementData data = commonAchs.FirstOrNull((x) => { return x.id == curSelectAchievementId; });
            if (data == null) return;
            msp_icon.spriteName = data.config.pic.ToString();
            hasReachedCount = data.GetReachedCollectionsCount();
            maxCount = data.collections.Count;
            mobj_giftFx.CustomActive(hasReachedCount >= data.collections.Count && !data.receivedReward);
            mobj_boxCheck.CustomActive(data.receivedReward);
            msp_icon.color = data.receivedReward ? Color.black : CSColor.white;
        }

        mlb_slider.text = $"{hasReachedCount}/{maxCount}";
        mslider_reached.value = (float)hasReachedCount / maxCount;

    }


    void SwitchAchievementBtnClick(GameObject go)
    {
        SwitchAchievement(go, false);
    }


    void SwitchAchievementBtnClickAuto(GameObject go)
    {
        SwitchAchievement(go, true);
    }


    void SwitchAchievement(GameObject go, bool autoScroll)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        curSelectAchievementId = param;
        
        if (param == ultAch.config.id)
        {
            mtrans_select.localPosition = Vector2.zero;
            if (autoScroll)
            {
                mscroll_left.ScrollImmidate(0);
            }
        }
        else
        {
            int key = commonAchs.FirstKey(x => { return x.config.id == param; });
            mtrans_select.localPosition = new Vector2(0, 0 - 99 * (key + 1));
            if (autoScroll)
            {
                var showHeight = mscroll_left.panel.height;
                if ((commonAchs.Count - key) * 90 < showHeight)//90从scroll下的grid高度间隔得来
                {
                    mscroll_left.ScrollImmidate(1);
                }
                //else
                //{
                //    var notShowHeight = (commonAchs.Count - key) * 90 - showHeight;
                //    float value = notShowHeight / (float)(commonAchs.Count + 1) * 90;
                //    Debug.LogError("@@@@Value:" + value + ", notShowHeight:" + notShowHeight + ", aa:" + (float)(commonAchs.Count + 1) * 90);
                //    mscroll_left.ScrollImmidate(1 - value);
                //}
            }
        }

        int isLast = SignCardHonorTableManager.Instance.GetSignCardHonorIsLast(param);
        mobj_common.SetActive(isLast != 1);
        mobj_ult.SetActive(isLast == 1);
        if (isLast != 1) RefreshCommonGrid(param);

        RefreshTopRightInfo();
    }


    void GetRewardClick(GameObject go)
    {
        if (curSelectAchievementId == ultAch.config.id)
        {
            if (hasReachedCount >= maxCount)
            {
                Net.ReqSignAchievementMessage(ultAch.config.id);
            }
            else
            {
                UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
                {
                    (f as UIDailySignInAwardPanel).OpenPanel(ClientTipsTableManager.Instance.GetClientTipsContext(1148), ultAch.honorRewardDic);
                });
            }
        }
        else
        {
            CommonAchievementData data = commonAchs.FirstOrNull((x) => { return x.id == curSelectAchievementId; });
            if (data == null) return;
            if (hasReachedCount >= maxCount && !data.receivedReward)
            {
                Net.ReqSignAchievementMessage(data.config.id);
            }
            else
            {
                UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
                {
                    (f as UIDailySignInAwardPanel).OpenPanel(ClientTipsTableManager.Instance.GetClientTipsContext(1148), data.honorRewardDic, data.rewardSlots);
                });
            }
        }
    }


    //左侧列表icon点击显示的是组合奖励信息
    void LeftGridIconClick(GameObject go)
    {
        int param = System.Convert.ToInt32(UIEventListener.Get(go).parameter);
        if (param == ultAch.config.id)
        {
            UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
            {
                (f as UIDailySignInAwardPanel).OpenPanel(ultAch.config.name, ultAch.honorRewardDic);
            });
        }
        else
        {
            CommonAchievementData data = commonAchs.FirstOrNull((x) => { return x.id == param; });
            if (data != null)
            {
                UIManager.Instance.CreatePanel<UIDailySignInAwardPanel>((f) =>
                {
                    (f as UIDailySignInAwardPanel).OpenPanel(data.config.name, data.collectionRewardDic);
                });
            }
        }
    }



    void CloseClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIDailySignInPreviewPanel>();
    }


    void CollectionReachedInfoChange(uint id, object data)
    {
        mGrid_common.UnBind<UICollectionInAchItem>();
        OpenPanel();
    }

    void SC_HonorChange(uint id, object data)
    {
        mGrid_common.UnBind<UICollectionInAchItem>();
        OpenPanel();
    }

    void UltHonorReceive(uint id, object data)
    {
        CloseClick(null);
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_giftFx);
        mGrid_common.UnBind<UICollectionInAchItem>();
        ultAch = null;
        redpoints?.Clear();
        redpoints = null;

        base.OnDestroy();
    }

}



public class UICollectionInAchItem : UIBinder
{
    UILabel lb_name;
    GameObject btn_reward;
    GameObject btn_lock;
    UILabel lb_lock;
    UISprite sp_lock;
    GameObject obj_reached;
    UIGridContainer grid_miniCards;

    SignCardCollectionData collection;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        btn_reward = Get<GameObject>("btn_show");
        btn_lock = Get<GameObject>("btn_lock");
        sp_lock = Get<UISprite>("btn_lock");
        lb_lock = Get<UILabel>("btn_lock/lb_desc");
        obj_reached = Get<GameObject>("sp_check");
        grid_miniCards = Get<UIGridContainer>("ScrollView/Grid");

        HotManager.Instance.EventHandler.AddEvent(CEvent.CollectionLockInfoChange, CollectionLockInfoChange);

        UIEventListener.Get(btn_lock).onClick = LockCollectionBtnClick;
        UIEventListener.Get(btn_reward).onClick = GetCollectionRewardBtnClick;
    }


    public override void Bind(object data)
    {
        collection = data as SignCardCollectionData;

        RefreshUI();
    }


    void RefreshUI()
    {
        if (collection == null) return;

        string greenColor = UtilityColor.GetColorString(ColorType.Green);
        string redColor = UtilityColor.GetColorString(ColorType.Red);
        string mainColor = UtilityColor.GetColorString(ColorType.MainText);
        string subColor = UtilityColor.GetColorString(ColorType.SecondaryText);
        string weakColor = UtilityColor.GetColorString(ColorType.WeakText);

        int max = 5;
        int reached = max - collection.notActiveList.Count;
        string nameAndCount = $"{mainColor}{collection.config.name}   {collection.config.desc}{(reached >= max ? greenColor : redColor)}({reached}/{max})";//{subColor}
        lb_name.text = nameAndCount;

        btn_reward.SetActive(collection.notActiveList.Count < 1);
        btn_lock.SetActive(collection.notActiveList.Count > 0 && collection.config.require == 1);
        sp_lock.spriteName = collection.isLocked ? "btn_samll1" : "btn_samll2";
        lb_lock.text = collection.isLocked ? "[B0BBCF]解 锁" : "[CFBFB0]锁 定";
        obj_reached.SetActive(collection.hasReached);

        var miniCards = collection.GetMiniCardPreview();
        if (miniCards != null)
        {
            grid_miniCards.Bind<MiniCardSlot, UISignCardMiniItem>(miniCards);            
        }

    }


    public override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.CollectionLockInfoChange, CollectionLockInfoChange);
        grid_miniCards.UnBind<UISignCardMiniItem>();
        collection = null;
        lb_name = null;
        btn_reward = null;
        btn_lock = null;
        obj_reached = null;
        grid_miniCards = null;
    }


    void LockCollectionBtnClick(GameObject go)
    {
        if (collection != null)
        {
            collection.TryToLockOrUnLock();
        }
    }


    void GetCollectionRewardBtnClick(GameObject go)
    {
        if (collection != null)
        {
            UIManager.Instance.CreatePanel<UIDailySignInTipsPanel>((f) =>
            {
                (f as UIDailySignInTipsPanel).OpenPanel(collection);
            });
        }
    }


    void CollectionLockInfoChange(uint id, object eData)
    {
        RefreshUI();
    }
}