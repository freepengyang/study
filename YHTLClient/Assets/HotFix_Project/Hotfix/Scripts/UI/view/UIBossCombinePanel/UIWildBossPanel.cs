using FlyBirds.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIWildBossPanel : UIBasePanel
{
    #region variable

    GameObject curCheckMark;
    List<BossShowItem> defeatList = new List<BossShowItem>();
    List<BossShowItem> previewList = new List<BossShowItem>();
    List<UIItemBase> rewardList = new List<UIItemBase>();
    BossShowItem curBossitem;
    TimerEventHandle timer;
    #endregion

    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "challengeboss_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_noBossbg, "pattern");
        UIEventListener.Get(mbtn_defeat, 1).onClick = ShowTypeChange;
        UIEventListener.Get(mbtn_preview, 2).onClick = ShowTypeChange;
    }

    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
        Net.CSBossInfoMessage();
        mClientEvent.Reg((uint)CEvent.ECM_SCBossInfoMessage, GetDefeatData);
        timer = CSTimer.Instance.InvokeRepeating(0f, 1f, CountDown);
    }

    public override void OnHide()
    {
        base.OnHide();
        defeatList.Clear();
        previewList.Clear();
        mClientEvent.RemoveEvent(CEvent.ECM_SCBossInfoMessage, GetDefeatData);
        if (timer != null)
        {
            CSTimer.Instance.remove_timer(timer);
            timer = null;
        }
    }

    protected override void OnDestroy()
    {
        if (timer != null)
        {
            CSTimer.Instance.remove_timer(timer);
            timer = null;
        }
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        CSEffectPlayMgr.Instance.Recycle(mtex_noBossbg);
        CSEffectPlayMgr.Instance.Recycle(mlb_bossSprite.gameObject);
        defeatList.Clear();
        previewList.Clear();
        if (rewardList.Count > 0)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(rewardList);
            rewardList.Clear();
        }
        base.OnDestroy();
    }
    #region 倒计时
    void CountDown()
    {
        for (int i = 0; i < mapBtnList.Count; i++)
        {
            mapBtnList[i].UpdateTime();
        }
    }
    #endregion
    void GetDefeatData(uint id, object data)
    {
        boss.ChallengeBossInfoResponse msg = (boss.ChallengeBossInfoResponse)data;
        RefreshDefeat(msg);
        RefreshPreview();
        ShowTypeChange(mbtn_defeat);
    }
    #region 切换
    void ShowTypeChange(GameObject _go)
    {
        int type = (int)UIEventListener.Get(_go).parameter;
        if (curCheckMark != null)
        {
            curCheckMark.SetActive(false);
        }
        curCheckMark = _go.transform.Find("Checkmark").gameObject;
        curCheckMark.SetActive(true);
        RefreshBossList(type);
    }
    void RefreshBossList(int _type)
    {
        mscr_defeat.gameObject.SetActive(_type == 1);
        mscr_preview.gameObject.SetActive(_type == 2);
        if (_type == 1)
            SetShowAndClick(mgrid_defeat);
        else
            SetShowAndClick(mgrid_preview);
    }
    private void SetShowAndClick(UIGridContainer grid)
    {
        if (grid.controlList.Count > 0)
        {
            ShowNoBoss(false);
            ItemClick(grid.controlList[0]);
        }
        else
            ShowNoBoss(true);
    }
    void ShowNoBoss(bool _state)
    {
        mobj_noBoss.SetActive(_state);
        mobj_right.SetActive(!_state);
    }
    #endregion


    Dictionary<int, BossShowData> bossItemDataDic = new Dictionary<int, BossShowData>();
    void RefreshDefeat(boss.ChallengeBossInfoResponse msg)
    {
        TABLE.BOSS bossTable = null;
        for (int i = 0; i < msg.challengeBossInfo.Count; i++)
        {
            int id = msg.challengeBossInfo[i].id;
            long refreshTime = msg.challengeBossInfo[i].refreshTime;
            BossTableManager.Instance.TryGetValue(id, out bossTable);
            if (bossTable == null) return;
            int groupId = bossTable.group;
            if (bossTable.bossType == 1)
            {
                if (!bossItemDataDic.ContainsKey(groupId))
                {
                    bossItemDataDic.Add(groupId, new BossShowData(groupId));
                }
                bossItemDataDic[groupId].AddBossData(msg.challengeBossInfo[i]);
            }
        }
        mgrid_defeat.MaxCount = bossItemDataDic.Count;
        int k = 0;
        //int monsterId = 0;
        var iter = bossItemDataDic.GetEnumerator();
        while (iter.MoveNext())
        {
            defeatList.Add(new BossShowItem(mgrid_defeat.controlList[k], ItemClick));
            //monsterId = BossTableManager.Instance.GetBossMonsterid(iter.Current.Value.showId);
            defeatList[k].RefreshByMonsterId(1, iter.Current.Value);
            k++;
        }
    }

    Dictionary<int, List<int>> monsterDic = new Dictionary<int, List<int>>();
    void RefreshPreview()
    {
        monsterDic.Clear();
        monsterDic = BossTableManager.Instance.GetBossPreviewMes(1);
        mgrid_preview.MaxCount = monsterDic.Count;
        var iter = monsterDic.GetEnumerator();
        int i = 0;
        while (iter.MoveNext())
        {
            previewList.Add(new BossShowItem(mgrid_preview.controlList[i], ItemClick));
            previewList[i].RefreshByMonsterId(2, iter.Current.Key, iter.Current.Value);
            i++;
        }
    }

    void ItemClick(GameObject _go)
    {
        if (curBossitem != null)
        {
            curBossitem.ChangeSelectState(false);
        }

        curBossitem = (BossShowItem)UIEventListener.Get(_go).parameter;
        curBossitem.ChangeSelectState(true);
        ShowModel();
        ShowMapBtn();
        ShowReward();
    }


    ILBetterList<BossMapBtnData> mapDic = new ILBetterList<BossMapBtnData>(5);
    ILBetterList<MapBtnItem> mapBtnList = new ILBetterList<MapBtnItem>(5);
    void ShowMapBtn()
    {
        //mgrid_mapbtns.MaxCount = curBossitem.bossData.infoDic.Count;
        if (curBossitem.type == 1)
        {
            mgrid_mapbtns.MaxCount = curBossitem.bossData.GetDataList().Count;
            int gap = mgrid_mapbtns.MaxCount - mapBtnList.Count;
            if (gap > 0)
            {
                for (int i = 0; i < gap; i++)
                {
                    mapBtnList.Add(new MapBtnItem());
                }
            }
            for (int i = 0; i < mgrid_mapbtns.MaxCount; i++)
            {
                TABLE.BOSS bossCfg;
                if (BossTableManager.Instance.TryGetValue(curBossitem.bossData.GetDataList()[i].id, out bossCfg))
                {
                    if (bossCfg.gameModel != 0)
                    {
                        mapBtnList[i].SetGo(mgrid_mapbtns.controlList[i]);
                        mapBtnList[i].Refresh(curBossitem.type, bossCfg, curBossitem.bossData.GetDataList()[i]);
                    }
                    if (bossCfg.deliver != 0)
                    {
                        mapBtnList[i].SetGo(mgrid_mapbtns.controlList[i]);
                        mapBtnList[i].Refresh(curBossitem.type, bossCfg, curBossitem.bossData.GetDataList()[i]);
                    }
                }
            }
        }
        if (curBossitem.type == 2)
        {
            mgrid_mapbtns.MaxCount = curBossitem.BossIds().Count;
            int gap = mgrid_mapbtns.MaxCount - mapBtnList.Count;
            if (gap > 0)
            {
                for (int i = 0; i < gap; i++)
                {
                    mapBtnList.Add(new MapBtnItem());
                }
            }
            for (int i = 0; i < mgrid_mapbtns.MaxCount; i++)
            {
                TABLE.BOSS bossCfg;
                if (BossTableManager.Instance.TryGetValue(curBossitem.BossIds()[i], out bossCfg))
                {
                    if (bossCfg.gameModel != 0)
                    {
                        mapBtnList[i].SetGo(mgrid_mapbtns.controlList[i]);
                        mapBtnList[i].Refresh(curBossitem.type, bossCfg);
                    }
                    if (bossCfg.deliver != 0)
                    {
                        mapBtnList[i].SetGo(mgrid_mapbtns.controlList[i]);
                        mapBtnList[i].Refresh(curBossitem.type, bossCfg);
                    }
                }
            }
        }


    }
    #region 奖励 模型
    void ShowModel()
    {
        mlb_bossName.text = MonsterInfoTableManager.Instance.GetMonsterInfoName(curBossitem.monsterid);
        mlb_bossName.color =
            UtilityCsColor.Instance.GetColor(
                MonsterInfoTableManager.Instance.GetMonsterInfoQuality(curBossitem.monsterid));

        CSEffectPlayMgr.Instance.ShowUIEffect(mlb_bossSprite.gameObject,
            $"{BossTableManager.Instance.GetBossShow(curBossitem.Id)}",
            ResourceType.UIMonster, 5);
    }
    void ShowReward()
    {
        mlb_bossLv.text = $"{MonsterInfoTableManager.Instance.GetMonsterInfoLevel(curBossitem.monsterid)}";
        mlb_refreshTime.text = BossTableManager.Instance.GetBossTime(curBossitem.Id);
        int pro = MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit(curBossitem.monsterid);
        CSBetterLisHot<int> infoList = new CSBetterLisHot<int>();
        MDropItemsTableManager.Instance.GetDropItemsByMonsterId(curBossitem.monsterid, infoList, 9);
        if (infoList.Count > rewardList.Count)
        {
            int gap = infoList.Count - rewardList.Count;
            for (int i = 0; i < gap; i++)
            {
                rewardList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_rewards.transform,
                    itemSize.Size60));
            }
        }
        for (int i = 0; i < rewardList.Count; i++)
        {
            if (i >= infoList.Count)
            {
                rewardList[i].obj.SetActive(false);
            }
            else
            {
                rewardList[i].obj.SetActive(true);
                rewardList[i].Refresh(infoList[i]);
            }
        }
        mgrid_rewards.Reposition();
        mscr_rewardScr.ResetPosition();
    }
    #endregion


    public class BossShowItem
    {
        GameObject go;
        GameObject select;
        UISprite icon;
        UILabel lv;
        UILabel name;
        Action<GameObject> action;

        public int monsterid;
        public int groupId;
        public int Id;
        List<int> mapIds = new List<int>(20);
        /// <summary>
        /// 1可击败  2可预览
        /// </summary>
        public int type = 0;
        public BossShowData bossData;

        public BossShowItem(GameObject _go, Action<GameObject> _dele)
        {
            go = _go;
            action = _dele;
            select = go.transform.Find("select").gameObject;
            icon = go.transform.Find("headitem").GetComponent<UISprite>();
            name = go.transform.Find("lb_name").GetComponent<UILabel>();
            lv = go.transform.Find("lb_lv").GetComponent<UILabel>();
            UIEventListener.Get(go, this).onClick = action;
        }

        public void RefreshByMonsterId(int _type, int _groupId, List<int> _bossId)
        {
            type = _type;
            Id = _bossId[0];
            mapIds = _bossId;
            groupId = BossTableManager.Instance.GetBossGroup(Id);
            monsterid = BossTableManager.Instance.GetBossMonsterid(Id); ;
            MonsterInfoTableManager ins = MonsterInfoTableManager.Instance;
            icon.spriteName = ins.GetMonsterInfoHead(monsterid).ToString();
            int monsterLv = 0;
            if (ins.GetMonsterInfoPropertiesSuit(monsterid) == 1)
                monsterLv = (int)ins.GetMonsterInfoLevel(monsterid);
            else if (ins.GetMonsterInfoPropertiesSuit(monsterid) == 3)
                monsterLv = CSMainPlayerInfo.Instance.Level;
            else
                monsterLv = (int)MonsterInfoTableManager.Instance.GetMonsterInfoLevel(monsterid);

            lv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1166), monsterLv);
            name.text = ins.GetMonsterInfoName(monsterid);
            name.color = UtilityCsColor.Instance.GetColor(ins.GetMonsterInfoQuality(monsterid));
        }
        public void RefreshByMonsterId(int _type, BossShowData _data)
        {
            bossData = _data;
            Id = bossData.showId;
            type = _type;
            groupId = bossData.groupId;
            monsterid = BossTableManager.Instance.GetBossMonsterid(bossData.showId);
            MonsterInfoTableManager ins = MonsterInfoTableManager.Instance;
            icon.spriteName = ins.GetMonsterInfoHead(monsterid).ToString();
            int monsterLv = 0;
            if (ins.GetMonsterInfoPropertiesSuit(monsterid) == 1)
                monsterLv = (int)ins.GetMonsterInfoLevel(monsterid);
            else if (ins.GetMonsterInfoPropertiesSuit(monsterid) == 3)
                monsterLv = CSMainPlayerInfo.Instance.Level;
            else
                monsterLv = (int)MonsterInfoTableManager.Instance.GetMonsterInfoLevel(monsterid);

            lv.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1166), monsterLv);
            name.text = ins.GetMonsterInfoName(monsterid);
            name.color = UtilityCsColor.Instance.GetColor(ins.GetMonsterInfoQuality(monsterid));
        }

        long reviveTime = 0;
        public long ReviveTime
        {
            get
            {
                return reviveTime;
            }
            set
            {
                reviveTime = value;
            }
        }
        public List<int> BossIds()
        {
            return mapIds;
        }

        public void ChangeSelectState(bool _state)
        {
            select.SetActive(_state);
        }
    }

    public class MapBtnItem
    {
        public GameObject go;
        UILabel name, lb_time;
        UISprite sp_enter;

        TABLE.BOSS data;
        boss.ChallengeBossInfo bossInfo;
        /// <summary>
        /// 1可击败  2可预览
        /// </summary>
        public int type;
        public MapBtnItem()
        {

        }
        public void SetGo(GameObject _go)
        {
            go = _go;
            name = go.transform.Find("Label").GetComponent<UILabel>();
            lb_time = go.transform.Find("lb_time").GetComponent<UILabel>();
            sp_enter = go.GetComponent<UISprite>();

            UIEventListener.Get(go).onClick = Click;
        }
        public void Refresh(int _type, TABLE.BOSS _data, boss.ChallengeBossInfo _bossInfo = null)
        {
            type = _type;
            data = _data;
            bossInfo = _bossInfo;
            UpdateTime();
        }
        public void UpdateTime()
        {
            if (name == null)
            {
                return;
            }
            if (type == 1)
            {
                long curTime = CSServerTime.Instance.TotalMillisecond;
                if (bossInfo != null && bossInfo.refreshTime > 0)
                {
                    lb_time.text = CSString.Format(2030, CSServerTime.Instance.FormatLongToTimeStr((bossInfo.refreshTime - curTime) / 1000, 19));
                    sp_enter.spriteName = "tab_chat1";
                    name.text = MapInfoTableManager.Instance.GetMapInfoName(data.mapId).BBCode(ColorType.TabBackground);
                }
                else
                {
                    lb_time.text = "";
                    sp_enter.spriteName = "btn_number3";
                    name.text = MapInfoTableManager.Instance.GetMapInfoName(data.mapId).BBCode(ColorType.TabCheck);
                }
            }
            else
            {
                name.text = MapInfoTableManager.Instance.GetMapInfoName(data.mapId).BBCode(ColorType.TabCheck);
            }
        }
        public void Click(GameObject _go)
        {
            UIManager.Instance.ClosePanel<UIBossCombinePanel>();
            if (data.gameModel != 0)
            {
                UtilityPanel.JumpToPanel(data.gameModel);
            }
            if (data.deliver != 0)
            {
                Net.ReqTransferByDeliverConfigMessage(data.deliver, false, 0, false, 0);
            }
        }
    }

}
public class BossMapBtnData
{
    public int type;
    public int id;
    public int bossId;
    public int mapId;
    public int monsterId;
    public BossMapBtnData(int _id, int _type)
    {
        id = _id;
        type = _type;
    }
    public BossMapBtnData()
    {

    }
}
public class BossShowData
{
    public int groupId;
    Dictionary<int, boss.ChallengeBossInfo> infoDic;
    ILBetterList<boss.ChallengeBossInfo> infoList;
    public int showId;
    public BossShowData(int _groupId)
    {
        groupId = _groupId;
        infoDic = new Dictionary<int, boss.ChallengeBossInfo>(5);
        infoList = new ILBetterList<boss.ChallengeBossInfo>(5);
    }
    public void AddBossData(boss.ChallengeBossInfo _mes)
    {
        showId = _mes.id;
        if (!infoDic.ContainsKey(_mes.id))
        {
            infoDic.Add(_mes.id, _mes);
            infoList.Add(_mes);
        }
    }
    public ILBetterList<boss.ChallengeBossInfo> GetDataList()
    {
        infoList.Sort((a, b) =>
        {
            return (int)(a.refreshTime - b.refreshTime);
        });
        return infoList;
    }
}