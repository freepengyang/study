using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDungeonPanel : UIBasePanel
{
    private int lv;
    private int playlv;
    int limitTimes;
    TABLE.INSTANCE data;
    FastArrayElementFromPool<UIItemBase> items;
    public override void Init()
    {
        base.Init();
        AddCollider();
        data = InstanceTableManager.Instance.GetTableDataByType(7)[0]; //instance table数据
        TABLE.INSTANCE instanceData = data;
        //活动时间
        //string time = TimerTableManager.Instance.GetTimerDesc(13);
        //mlb_time.text = time;
        //进入等级
        lv = instanceData.openLevel;
        playlv = CSMainPlayerInfo.Instance.Level;
        ColorType type = lv > playlv ? ColorType.Red : ColorType.Green;
        mlb_condition.text = string.Format(mlb_condition.FormatStr, lv).BBCode(type);
        //进入界面信息
        string desc = MapInfoTableManager.Instance.GetMapInfoDesc(instanceData.mapId);
        UIEventListener.Get(mbtn_go).onClick = EnterClick;
        UIEventListener.Get(mbtn_close).onClick = Close;
        mlb_activityEntrance.text = desc;
        //string[] cost = requireItems.Split('#');
        //每天挑战次数
        limitTimes = instanceData.limitTimes;
        int Count = CSInstanceInfo.Instance.GetInstanceCount(data.mapId);
        CSStringBuilder.Clear();
        string numstr = CSStringBuilder.Append(Count,"/", limitTimes).ToString().BBCode(Count>0 ? ColorType.Green: ColorType.Red);
        CSStringBuilder.Clear();
        mlb_remain.text = CSStringBuilder.Append(CSString.Format(1169).BBCode(ColorType.SecondaryText), numstr).ToString();
        
        string[] itemsInfo = SundryTableManager.Instance.GetSundryEffect(425).Split('&');
        
        items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_reward.transform,8,itemSize.Size44);
        //mgrid_reward.MaxCount = itemsInfo.Length;
        items.Clear();
        for (int i = 0; i < itemsInfo.Length; i++)
        {
            string[] iteminfo = itemsInfo[i].Split('#');
            var item = items.Append();
            if (iteminfo.Length > 2)
            {
                item.Refresh(int.Parse(iteminfo[1]));
                item.SetCount(int.Parse(iteminfo[2]));
            }        
            // GameObject gp = mgrid_reward.controlList[i];
            // TABLE.ITEM itemCfg = ItemTableManager.Instance.GetItemCfg(int.Parse(item[1]));
            // UIItemBase uiItemBase = new UIItemBase(gp, PropItemType.Normal);
            // uiItemBase.Refresh(itemCfg, Utility.ItemClick);
            // UILabel lb_count = gp.transform.Find("lb_count").gameObject.GetComponent<UILabel>();
            // lb_count.text = item[2];
        }

        mgrid_label.MaxCount = itemsInfo.Length;
        for (int i = 0; i < itemsInfo.Length; i++)
        {
            string[] item = itemsInfo[i].Split('#');
            mgrid_label.controlList[i].GetComponent<UILabel>().text = item[0];
        }
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg.gameObject, "dungeon_bg");

    }
    private void EnterClick(GameObject go)
    {
        //UIManager.Instance.CreatePanel<UIDungeonInspirePanel>();
        if (lv > playlv)
        {
            UtilityTips.ShowRedTips(CSString.Format(1020));
            return;
        }
        int Count = CSInstanceInfo.Instance.GetInstanceCount(data.mapId);
        if (Count > 0)
        {
            Net.ReqEnterInstanceMessage(InstanceTableManager.Instance.GetInstanceMapId(data.id));
            UIManager.Instance.ClosePanel<UIWoLongActivityPanel>();
            Close();
        }
        else
        {
            UtilityTips.ShowRedTips(CSString.Format(1019));
        }

    }
    // public override bool ShowGaussianBlur
    // {
    //     get { return false; }
    // }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
}
