using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TABLE;
using UnityEngine;

public partial class UIDungeonSiegeInstancePanel : UIBasePanel
{
    int skillid;
    int score = 0;//玩家当前积分
    private instance.InstanceInfo _InstanceInfo;

    private instance.DiLaoInfo diLaoInfo;
    private int curWave = 0;
    private FastArrayElementFromPool<UIItemBase> items;
    private UIItemBase item;
    
    public override void Init()
    {
        //Debug.Log("UIDungeonSiegeInstancePanel");
        base.Init();
        //显示下次刷新时间
        //副本完成
        mClientEvent.Reg((uint)CEvent.DungeonInfo,ShowDungeonInfo);//显示波数信息
        mClientEvent.AddEvent(CEvent.ResInstanceInfo, ResInstanceInfo);
        mClientEvent.AddEvent(CEvent.LeaveInstance, OnLeaveInatance);
        UIEventListener.Get(mbtn_Inspire).onClick = InspireClick;
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_inspireEffect, 17701);
        // if (items == null)
        // {
        //     items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, mgrid_reward.transform,8,itemSize.Size50);
        //     items.Clear();
        //     item = items.Append();
        // }
    }

    private void OnLeaveInatance(uint uievtid, object data)
    {
        CSInstanceInfo.Instance.DiLaoInfo = null;
        //只有主角需要监听事件
        CSMainPlayerInfo.Instance.EventHandler.SendEvent(CEvent.ChangeHeadSkillInfo,"");
    }

    public override void Show()
    {
        base.Show();
        ShowDungeonInfo(); 
        // ScriptBinder.Invoke(1f, () =>
        // {
        //     
        // }
        // );
    }

    // private void CloseFinishPanel(uint uiEvtID, object data)
    // {
    //     //关闭面板
    //      UIManager.Instance.ClosePanel<UIRewardPromptPanel>();
    // }

    private void ResInstanceInfo(uint uiEvtID, object data)
    {
        
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        int instanceType = InstanceTableManager.Instance.GetInstanceType(_InstanceInfo.instanceId);
        if (instanceType == 7&& _InstanceInfo.success)
        {
            //Debug.Log("ShowFinishPanel");
            List<StringData> listData = CountRewardList();
            Utility.ShowRewardPanel(listData,()=> {
                Net.ReqLeaveInstanceMessage(true);
            });
        }

    }
    
    private void InspireClick(GameObject obj)
    {
        UIManager.Instance.CreatePanel<UIDungeonInspirePanel>((f) =>
        {
            (f as UIDungeonInspirePanel).GetGuWuInfo(skillid);
        });
    }

    
    
    private tip.BulletinResponse bulletinResponse;
    private void ShowDungeonInfo(uint uiEvtID = 0, object data = null)
    {
        diLaoInfo = CSInstanceInfo.Instance.DiLaoInfo;
        if (diLaoInfo == null)
            return;
        int wave = diLaoInfo.wave;
        CSStringBuilder.Clear();
        mlb_count.text = CSStringBuilder.Append(wave,"/",DiLaoMonsterTableManager.Instance.array.gItem.id2offset.Count).ToString();
        if (curWave != wave)
        {
            curWave = wave;
            if (bulletinResponse == null)
            {
                bulletinResponse = mPoolHandleManager.GetSystemClass<tip.BulletinResponse>();
            }
            bulletinResponse.count = 1;
            bulletinResponse.display = (int)NoticeType.CenterTop;
            bulletinResponse.msg = CSString.Format(1270,wave);
            bulletinResponse.bulletinId = 0;
            CSNoticeManager.Instance.ResBulletinMessage(bulletinResponse);
            
        }
        score = diLaoInfo.score;
        
        skillid = diLaoInfo.skillId;
        string level = "";
        if (skillid != 0 )
        {
            //int groupid = SkillTableManager.Instance.GetSkillSkillGroup(skillid);
            var arr = DiLaoSkillTableManager.Instance.array.gItem.handles;
            if (arr.Length>0)
            {
                for(int i = 0,max = arr.Length;i < max;++i)
                {
                    var item = arr[i].Value as TABLE.DILAOSKILL;
                    if (item.skill == skillid.ToString())
                    {
                        level = item.level;
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(level))
        {
            FNDebug.Log("level is null");
        }
        
        string skillName = SkillTableManager.Instance.GetSkillName(skillid*1000 + 1);
        string str = CSString.Format(1017, skillName, level);
        //mlb_hint.text = string.Format(CSString.Format(1017), skillName, level);
        
        
        //只有主角需要监听事件
        CSMainPlayerInfo.Instance.EventHandler.SendEvent(CEvent.ChangeHeadSkillInfo,str);
        
        
        string[] itemsInfo = SundryTableManager.Instance.GetSundryEffect(425).Split('&');
        mItemBase.SetActive(true);
        for (int i = 0; i < itemsInfo.Length; i++)
        {
            string[] itemdata = itemsInfo[i].Split('#');
            //mgrid_reward.MaxCount = 1;
            //判断如果获得的积分比表中的值都大 那么显示所有奖励完成
            if (itemsInfo[i] == itemsInfo[itemsInfo.Length - 1]&& score >= int.Parse(itemdata[0]))
            {
                mgrid_reward.gameObject.SetActive(false);
                string tip =CSString.Format(868);
                mlb_finish.text = tip;
                mItemBase.SetActive(false);
                //RefreshItem(itemdata[1], itemdata[2]);
                mlb_integral.text = $"{score}/{itemdata[0]}".BBCode(ColorType.Green);
                break;
            }
            //如果不是最大的显示最大值的下一个奖励
            if (score >= int.Parse(itemdata[0]))
                continue;
            else
            {
                RefreshItem(itemdata[1], itemdata[2]);
                mlb_integral.text = $"{score}/{itemdata[0]}".BBCode(ColorType.Red);
                break;
            }
        }
        
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_inspireEffect);
        mPoolHandleManager.Recycle(bulletinResponse);
        base.OnDestroy(); 
        
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    private void RefreshItem(string strid , string strnum)
    {
        int id;
        int.TryParse(strid, out id);
        int num;
        int.TryParse(strnum, out num);
                
        ITEM item;
        if (ItemTableManager.Instance.TryGetValue(id , out item))
        {
            msp_quality.spriteName = $"quality{item.quality}";
            msp_itemicon.spriteName = item.icon;
            mlb_itemcount.text = num.ToString();
            UIEventListener.Get(mItemBase,item).onClick = OnClickItem;
        }
    }

    private void OnClickItem(GameObject obj)
    {
        ITEM item = UIEventListener.Get(obj).parameter as ITEM;

        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item);
        }
    }

    private List<StringData> CountRewardList()
    {
        List<StringData> listData = new List<StringData>();
        string[] itemsInfo = SundryTableManager.Instance.GetSundryEffect(425).Split('&');
        foreach (var item in itemsInfo)
        {
            string[] itemdata = item.Split('#');

            if (score >= int.Parse(itemdata[0]))
            {
                listData.Add(new StringData(int.Parse(itemdata[1]), int.Parse(itemdata[2])));
            }
            
        }
        return listData;
    }
    
    
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }
}



