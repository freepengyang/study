using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIEquipBossPanel : UIBasePanel
{
    #region variable

    GameObject curCheckMark;
    List<BossShowItem> defeatList = new List<BossShowItem>();
    List<BossShowItem> previewList = new List<BossShowItem>();
    List<UIItemBase> rewardList = new List<UIItemBase>();
    BossShowItem curBossitem;
    #endregion

    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, "challengeboss_bg");
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_noBossbg, "pattern");
        UIEventListener.Get(mbtn_defeat, 1).onClick = ShowTypeChange;
        UIEventListener.Get(mbtn_preview, 2).onClick = ShowTypeChange;
    }
    public override void Show()
    {
        base.Show();
        Net.CSBossInfoMessage();
        mClientEvent.Reg((uint)CEvent.ECM_SCBossInfoMessage, GetDefeatData);
    }

    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
    }

    public override void OnHide()
    {
        base.OnHide();

        defeatList.Clear();
        previewList.Clear();
        if (rewardList.Count > 0)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(rewardList);
            rewardList.Clear();
        }

        mClientEvent.RemoveEvent(CEvent.ECM_SCBossInfoMessage, GetDefeatData);
        //CSEffectPlayMgr.Instance.Recycle(mlb_bossSprite.gameObject);
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        CSEffectPlayMgr.Instance.Recycle(mtex_noBossbg);
        CSEffectPlayMgr.Instance.Recycle(mlb_bossSprite.gameObject);
        if (rewardList.Count > 0)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(rewardList);
            rewardList.Clear();
        }

        base.OnDestroy();
    }

    void GetDefeatData(uint id, object data)
    {
        boss.ChallengeBossInfoResponse msg = (boss.ChallengeBossInfoResponse)data;
        RefreshDefeat(msg);
        RefreshPreview();
        ShowTypeChange(mbtn_defeat);
    }

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
    Dictionary<int, int> dic = new Dictionary<int, int>();
    Dictionary<int, List<int>> dic2 = new Dictionary<int, List<int>>();
    void RefreshDefeat(boss.ChallengeBossInfoResponse msg)
    {
        dic.Clear();
        dic2.Clear();

        TABLE.BOSS bossTable = null;
        int tempGroupId = 0;
        for (int i = 0; i < msg.challengeBossInfo.Count; i++)
        {
            int id = msg.challengeBossInfo[i].id;
            BossTableManager.Instance.TryGetValue(id, out bossTable);
            if (bossTable == null) return;
            int bossType = bossTable.bossType;
            int groupId = bossTable.group;
            if (bossType == 3)
            {
                if (!dic.ContainsKey(groupId))
                    dic.Add(groupId, id);
                if (!dic2.ContainsKey(groupId))
                {
                    tempGroupId = groupId;
                    List<int> idList = mPoolHandleManager.GetSystemClass<List<int>>();
                    //idList.Clear();
                    dic2.Add(tempGroupId, idList);
                    //mPoolHandleManager.Recycle(idList);
                }
                if (tempGroupId == groupId)
                {
                    //Debug.Log("tempGroupId:" + tempGroupId + ",id:" + id);
                    dic2[tempGroupId].Add(id);
                }
            }
        }

        mgrid_defeat.MaxCount = dic.Count;
        int k = 0;
        int monsterId = 0;
        int groupIdx = 0;
        var iter = dic.GetEnumerator();
        while (iter.MoveNext())
        {
            defeatList.Add(new BossShowItem(mgrid_defeat.controlList[k], ItemClick));
            monsterId = BossTableManager.Instance.GetBossMonsterid(iter.Current.Value);
            groupIdx = BossTableManager.Instance.GetBossGroup(iter.Current.Value);
            defeatList[k].RefreshByMonsterId(monsterId, iter.Current.Value, dic2[groupIdx]);
            k++;
        }
    }

    void RefreshPreview()
    {
        Dictionary<int, int> monsterDic = BossTableManager.Instance.GetPreviewBossMes(3);
        mgrid_preview.MaxCount = monsterDic.Count;
        var iter = monsterDic.GetEnumerator();
        int i = 0;
        while (iter.MoveNext())
        {
            previewList.Add(new BossShowItem(mgrid_preview.controlList[i], ItemClick));
            previewList[i].RefreshByMonsterId(iter.Current.Key, iter.Current.Value);
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
        ShowMapBtn(curBossitem.idList);
        ShowReward();
    }

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
    Dictionary<int, BossMapBtnData> mapDic = new Dictionary<int, BossMapBtnData>(20);
    List<int> needScreenMapIds;
    void ShowMapBtn(List<int> _idList)
    {
        if (needScreenMapIds == null)
        {
            needScreenMapIds = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(1159));
        }
        List<int> idList;
        if (_idList != null)
            idList = _idList;
        else
            idList = BossTableManager.Instance.GetIdsByGroupsToId(curBossitem.Id);

        BossTableManager.Instance.GetMapDic(idList, mapDic, needScreenMapIds);
        mgrid_mapbtns.MaxCount = mapDic.Count;
        int mapId = 0;
        var dic = mapDic.GetEnumerator();
        int k = 0;
        while (dic.MoveNext())
        {
            UILabel mapName = mgrid_mapbtns.controlList[k].transform.Find("Label").GetComponent<UILabel>();
            mapId = dic.Current.Key;
            mapName.text = MapInfoTableManager.Instance.GetMapInfoName(mapId);
            UIEventListener.Get(mgrid_mapbtns.controlList[k], dic.Current.Value).onClick = MapBtnClick;
            k++;
        }
        mscr_mapbtn.ResetPosition();
    }

    void ShowReward()
    {
        mlb_bossLv.text = $"{MonsterInfoTableManager.Instance.GetMonsterInfoLevel(curBossitem.monsterid)}";
        mlb_refreshTime.text = BossTableManager.Instance.GetBossTime(curBossitem.Id);
        int pro = MonsterInfoTableManager.Instance.GetMonsterInfoPropertiesSuit(curBossitem.monsterid);

        CSBetterLisHot<int> info = new CSBetterLisHot<int>();
        MDropItemsTableManager.Instance.GetDropItemsByMonsterId(curBossitem.monsterid, info, 9);
        if (info == null) return;
        if (info.Count > rewardList.Count)
        {
            int gap = info.Count - rewardList.Count;
            for (int i = 0; i < gap; i++)
            {
                rewardList.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, mgrid_rewards.transform,
                    itemSize.Size60));
            }
        }

        for (int i = 0; i < rewardList.Count; i++)
        {
            if (i >= info.Count)
            {
                rewardList[i].obj.SetActive(false);
                //rewardList[i].UnInit();
            }
            else
            {
                rewardList[i].obj.SetActive(true);
                rewardList[i].Refresh(info[i]);
            }
        }

        mgrid_rewards.Reposition();
        mscr_rewardScr.ResetPosition();
    }

    void MapBtnClick(GameObject _go)
    {
        BossMapBtnData deliverId = (BossMapBtnData)UIEventListener.Get(_go).parameter;
        UIManager.Instance.ClosePanel<UIBossCombinePanel>();
        if (deliverId.type == 1)
        {
            UtilityPanel.JumpToPanel(deliverId.id);
        }
        else
        {
            Net.ReqTransferByDeliverConfigMessage(deliverId.id, false, 0, false, 0);
        }
    }
    public class BossShowItem
    {
        GameObject go;
        GameObject select;
        UISprite icon;
        UILabel lv;
        UILabel name;
        Action<GameObject> action;
        public int monsterid;
        public int Id;
        public List<int> idList;

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

        public void RefreshByMonsterId(int _bossId, int _id = 0, List<int> _idList = null)
        {
            MonsterInfoTableManager ins = MonsterInfoTableManager.Instance;
            monsterid = _bossId;
            if (_id > 0)
            {
                Id = _id;
                idList = _idList;
            }
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

        public void ChangeSelectState(bool _state)
        {
            select.SetActive(_state);
        }
    }
}
