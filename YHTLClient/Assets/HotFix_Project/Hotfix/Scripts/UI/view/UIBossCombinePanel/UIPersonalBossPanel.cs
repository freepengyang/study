using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public partial class UIPersonalBossPanel : UIBasePanel
{
    #region variable
    CSBetterLisHot<TABLE.INSTANCE> dataList;
    List<ChallengeItem> itemList;
    ChallengeItem currentItem;
    instance.OneInstanceCount leftCount;
    bool isMoneyEnough = false;
    int t_ind = 0;
    #endregion

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.ItemListChange, GetItemChange);
        itemList = new List<ChallengeItem>();
        //个人boss类型13
        dataList = InstanceTableManager.Instance.GetTableDataByType(13);
        mitemPar.MaxCount = dataList.Count;
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;
        UIEventListener.Get(mbtn_enter).onClick = EnterBtnClick;
        UIEventListener.Get(mbtn_BuyCount).onClick = BuyCountClick;
        mobj_scrollBar.onChange.Add(new EventDelegate(OnChange));

        t_ind = 0;
        base.Show();
        itemList.Clear();
        for (int i = 0; i < mitemPar.controlList.Count; i++)
        {
            ChallengeItem item = mPoolHandleManager.GetCustomClass<ChallengeItem>();
            item.SetData(mitemPar.controlList[i], dataList[i], ItemClick);
            itemList.Add(item);
            itemList[i].Refresh();
            if (itemList[i].GetLevelState())
            {
                t_ind = i;
            }
        }
        msc_Scroll.ResetPosition();
        if (t_ind >= 3)
        {
            TweenPosition.Begin(mitemPar.gameObject, 0.2f, new Vector3(-305 - (303 * (t_ind - 2)), 5, 0));
        }
        ItemClick(itemList[t_ind]);
        OnChange();
    }
    public override void Show()
    {
        ShowChallengeCount();
    }
    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    protected override void OnDestroy()
    {

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].ResetPreRes();
            for (int j = 0; j < itemList[i].itemList.Count; j++)
            {
                UIItemManager.Instance.RecycleSingleItem(itemList[i].itemList[j]);
            }
            itemList[i].Dispose();
        }
        mPoolHandleManager.RecycleAll();
        itemList.Clear();
        itemList = null;
        dataList.Clear();
        dataList = null;
        currentItem = null;
        base.OnDestroy();
    }
    int costId = 0;
    void GetItemChange(uint id, object _data)
    {
        ShowCost();
    }
    void ShowChallengeCount()
    {
        leftCount = CSInstanceInfo.Instance.GetCountDataByType(13001);
        mlb_count.text = $"{leftCount.leftCount}/{leftCount.totalTimes}";
        mlb_count.color = (leftCount.leftCount > 0) ? CSColor.green : CSColor.red;
        if (CSMonthCardInfo.Instance.HasMonthCard(1) && CSMonthCardInfo.Instance.HasMonthCard(2))
        {
            mbtn_BuyCount.SetActive(false);
        }
        else
        {
            mbtn_BuyCount.SetActive(true);
        }
    }
    void ShowCost()
    {
        string[] cost = currentItem.data.requireItems.Split('#');
        costId = int.Parse(cost[0]);
        msp_costIcon.spriteName = $"tubiao{costId}";
        long num = CSBagInfo.Instance.GetItemCount(costId);
        CSStringBuilder.Clear();
        mlb_costNum.text = CSStringBuilder.Append(num, "/", cost[1]).ToString();
        mlb_costNum.color = (num >= int.Parse(cost[1]) ? CSColor.green : CSColor.red);
        isMoneyEnough = (num >= int.Parse(cost[1])) ? true : false;
        UIEventListener.Get(mbtn_buy).onClick = (p => { Utility.ShowGetWay(costId); });
        UIEventListener.Get(msp_costIcon.gameObject).onClick = (p => { UITipsManager.Instance.CreateTips(TipsOpenType.Normal, costId); });
    }
    void BuyBtnClick(GameObject _go)
    {

    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.PERSONAL_BOSS);
    }
    void EnterBtnClick(GameObject _go)
    {
        if (!isMoneyEnough)
        {
            Utility.ShowGetWay(costId);
            //UtilityTips.ShowRedTips(532);
            return;
        }
        if (leftCount.leftCount <= 0)
        {
            UtilityTips.ShowRedTips(546);
            return;
        }
        if (currentItem.levelType == 1 && !currentItem.LevelNot)
        {
            UtilityTips.ShowRedTips(746);
            return;
        }
        else
        {
            if (currentItem.levelType == 2 && !currentItem.LevelNot)
            {
                UtilityTips.ShowRedTips(882);
                return;
            }
        }
        if (currentItem != null)
        {
            Net.ReqEnterInstanceMessage(currentItem.data.id);
        }
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
    }
    void BuyCountClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
        UtilityPanel.JumpToPanel(12600);
    }
    void ItemClick(ChallengeItem _go)
    {
        if (currentItem != null)
        {
            currentItem.ChangeSelected(false);
        }
        currentItem = _go;
        currentItem.ChangeSelected(true);
        ShowCost();
    }
    void OnChange()
    {
        if (1 >= mobj_scrollBar.value && mobj_scrollBar.value >= 0.95)
        {
            mobj_arrowLeft.SetActive(true);
        }
        else if (0 <= mobj_scrollBar.value && mobj_scrollBar.value <= 0.05)
        {
            mobj_arrowRight.SetActive(true);
        }
        else
        {
            mobj_arrowLeft.SetActive(false);
            mobj_arrowRight.SetActive(false);
        }
    }
}

public class ChallengeItem : IDispose
{
    GameObject go;
    GameObject selected;
    UILabel name;
    UISprite icon;
    UIGrid rewards;
    public TABLE.INSTANCE data;
    public List<UIItemBase> itemList;
    bool isChoose = false;
    Action<ChallengeItem> action;
    public int levelType = 0; //1卧龙等级 2人物等级
    public bool LevelNot = false;
    public ChallengeItem()
    {

    }
    public ChallengeItem(GameObject _go, TABLE.INSTANCE _data, Action<ChallengeItem> _action = null)
    {
        go = _go;
        data = _data;
        action = _action;
        InitComponent();
    }
    public void SetData(GameObject _go, TABLE.INSTANCE _data, Action<ChallengeItem> _action = null)
    {
        go = _go;
        data = _data;
        action = _action;
        InitComponent();
    }
    void InitComponent()
    {
        selected = go.transform.Find("selected").gameObject;
        name = go.transform.Find("name").GetComponent<UILabel>();
        icon = go.transform.Find("icon").GetComponent<UISprite>();
        rewards = go.transform.Find("Grid").GetComponent<UIGrid>();
    }
    public void Refresh()
    {
        CSEffectPlayMgr.Instance.ShowUITexture(go, "bpssbggeren");
        string lv;
        if (data.reincarnation <= 0)
        {
            levelType = 2;
            lv = CSString.Format(881, data.openLevel);
            if (data.openLevel <= CSMainPlayerInfo.Instance.Level)
            {
                LevelNot = true;
                name.text = $"{data.mapName}\n{UtilityColor.GetColorString(ColorType.Green)}{lv}";
            }
            else
            {
                LevelNot = false;
                name.text = $"{data.mapName}\n{UtilityColor.GetColorString(ColorType.Red)}{lv}";
            }
        }
        else
        {
            levelType = 1;
            lv = CSString.Format(434, data.reincarnation);
            if (data.reincarnation <= CSWoLongInfo.Instance.GetWoLongLevel())
            {
                LevelNot = true;
                name.text = $"{data.mapName}\n{UtilityColor.GetColorString(ColorType.Green)}{lv}";
            }
            else
            {
                LevelNot = false;
                name.text = $"{data.mapName}\n{UtilityColor.GetColorString(ColorType.Red)}{lv}";
            }
        }

        name.transform.localPosition = new Vector3(0, 170, 0);
        name.spacingY = 6;
        icon.spriteName = data.img;
        string[] count = data.show.Split('&');
        if (itemList == null)
        {
            itemList = new List<UIItemBase>();
            for (int i = 0; i < count.Length; i++)
            {
                itemList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, rewards.transform));
                string[] mes = count[i].Split('#');
                itemList[i].Refresh(ItemTableManager.Instance.GetItemCfg(int.Parse(mes[0])));
                itemList[i].obj.transform.localScale = new Vector3(0.84f, 0.84f, 0.84f);
            }
        }
        UIEventListener.Get(go).onClick = Click;
        CSEffectPlayMgr.Instance.ShowUIEffect(icon.gameObject, $"{data.img}", ResourceType.UIMonster, 5, true, true, () =>
           {
               icon.material.shader = Shader.Find("Unlit/Transparent Colored Gray");
               icon.color = LevelNot ? Color.white : Color.black;
           });
    }

    void Click(GameObject _go)
    {
        if (action != null)
        {
            action(this);
        }
    }
    public void ResetPreRes()
    {
        CSEffectPlayMgr.Instance.Recycle(go);
        CSEffectPlayMgr.Instance.Recycle(icon.gameObject);
    }
    public void ChangeSelected(bool _state)
    {
        isChoose = _state;
        selected.SetActive(isChoose);
    }
    public bool GetLevelState()
    {
        return LevelNot;
    }
    public void UnInt()
    {

    }
    public void OnRecycle()
    {
        go = null;
        selected = null;
        name = null;
        icon = null;
        rewards = null;
        data = null;
    }
    public void Dispose()
    {
        itemList = null;
        go = null;
        selected = null;
        name = null;
        icon = null;
        rewards = null;
        data = null;
        action = null;
    }
}
