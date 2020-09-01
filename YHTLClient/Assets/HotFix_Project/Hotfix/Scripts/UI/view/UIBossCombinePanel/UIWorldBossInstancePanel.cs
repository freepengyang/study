using instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIWorldBossInstancePanel : UIBasePanel
{
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    #region  variable
    worldboss.DamageRank Damagemsg;
    worldboss.BlessInfo blessInfo;
    worldboss.BossInfo bossInfo;
    List<WorldBossDamage> damageItemList = new List<WorldBossDamage>();
    List<BuffItem> buffitemList = new List<BuffItem>();
    int itemMaxCount = 20;
    Schedule Rankschedule;
    //Schedule countDownSchaedule;
    bool isRankOpen = false;
    int coinInspireNum = 0;
    int goldInspireNum = 0;
    int buffCount = 0;
    #endregion
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.MainFuncShowRanking, GetRankPartChange);
        mClientEvent.Reg((uint)CEvent.SCNotifyWorldBossRankInfoMessage, GetDamage);
        mClientEvent.Reg((uint)CEvent.ECM_SCWorldBossBlessInfoMessage, GetBlessChange);
        mClientEvent.Reg((uint)CEvent.ECM_SCWorldBossBossInfoMessage, GetBossInfo);
        mClientEvent.Reg((uint)CEvent.GetEnterInstanceInfo, GetInstanceInfo);
        mClientEvent.Reg((uint)CEvent.ECM_SCInstanceFinishMessage, GetInstanceFinish);
        mClientEvent.Reg((uint)CEvent.ResInstanceInfo, GetInstanceChange);

        UIEventListener.Get(mbtn_inspire).onClick = InspireClick;
        UIEventListener.Get(mbtn_exit).onClick = ExitClick;
        mgrid_rankItems.MaxCount = itemMaxCount;
        for (int i = 0; i < itemMaxCount; i++)
        {
            damageItemList.Add(new WorldBossDamage(mgrid_rankItems.controlList[i]));
        }
        if (!Timer.Instance.IsInvoking(Rankschedule))
        {
            Rankschedule = Timer.Instance.InvokeRepeating(0, 5f, ReqRankInfo);
        }
        RefreshTime();
        //if (!Timer.Instance.IsInvoking(countDownSchaedule))
        //{
        //    countDownSchaedule = Timer.Instance.InvokeRepeating(1, 1f, CountDown);
        //}
        for (int i = 0; i < mobj_buffs.transform.childCount; i++)
        {
            buffitemList.Add(new BuffItem(mobj_buffs.transform.GetChild(i).gameObject));
        }
        mlb_bossName.text = MonsterInfoTableManager.Instance.GetWorldBossName();
        blessInfo = CSInstanceInfo.Instance.GetWorldBossBlessInfo();
        if (blessInfo != null)
        {
            int coinEff = 0;
            int.TryParse(SundryTableManager.Instance.GetSundryEffect(1014), out coinEff);
            int goldEff = 0;
            int.TryParse(SundryTableManager.Instance.GetSundryEffect(1015), out goldEff);
            coinInspireNum = blessInfo.goldTimes;
            goldInspireNum = blessInfo.yuanbaoTimes;
            mlb_inspire.text = $"攻击+{(blessInfo.goldTimes * coinEff) / 100 + (blessInfo.yuanbaoTimes * goldEff) / 100}%";
            if (blessInfo.goldTimes != 0 || blessInfo.yuanbaoTimes != 0)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(mobj_inspireEff, "effect_inspire_add");
            }
        }
        bossInfo = CSInstanceInfo.Instance.GetWorldBossBuffInfo();
        if (bossInfo != null)
        {
            buffCount = bossInfo.buffers.Count;
        }
    }

    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        UIManager.Instance.ClosePanel<UIWorldInspirePanel>();
        CSEffectPlayMgr.Instance.Recycle(mobj_inspireEff);
        Timer.Instance.CancelInvoke(Rankschedule);
        //Timer.Instance.CancelInvoke(countDownSchaedule);
        base.OnDestroy();
    }
    void ReqRankInfo(Schedule _schedule)
    {
        Net.CSWorldBossRankInfoMessage();
        Net.CSWorldBossBossInfoMessage();
    }
    void CountDown(Schedule _schedule)
    {
        if (leftTime <= 0)
        {
            leftTime = 0;
            // if (Timer.Instance.IsInvoking(countDownSchaedule))
            // {
            //     Timer.Instance.CancelInvoke(countDownSchaedule);
            // }
            mlb_lefttime.text = "";
            return;
        }
        if (mlb_lefttime != null)
        {
            mlb_lefttime.text = CSServerTime.Instance.FormatLongToTimeStr(leftTime, 3);
        }
        leftTime--;

    }
    void ShowRank(GameObject _go)
    {
        mobj_rankPart.SetActive(!mobj_rankPart.activeSelf);
        isRankOpen = mobj_rankPart.activeSelf;
        if (isRankOpen) { Refresh(); }
    }
    void InspireClick(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIWorldInspirePanel>(p =>
        {
            (p as UIWorldInspirePanel).SetData(coinInspireNum, goldInspireNum);
        });
    }
    void ExitClick(GameObject _go)
    {
        Net.ReqLeaveInstanceMessage(true);
    }
    void Refresh()
    {
        if (null == Damagemsg)
            return;
        //Debug.Log("我的排名 --   " + Damagemsg.myRank);
        if(null != mlb_selfNum)
        {
            mlb_selfNum.text = (Damagemsg.myRank < 0 ? ClientTipsTableManager.Instance.GetClientTipsContext(745) : (Damagemsg.myRank + 1).ToString());
            mlb_selfNum.color = (Damagemsg.myRank < 0 ? CSColor.beige : CSColor.green);
        }
        if (isRankOpen)
        {
            for (int i = 0; i < itemMaxCount; i++)
            {
                if (i >= Damagemsg.ranks.Count)
                {
                    damageItemList[i].SetState(false);
                }
                else
                {
                    damageItemList[i].SetState(true);
                    damageItemList[i].RefreshItem(Damagemsg.ranks[i]);
                }
            }
        }

    }
    void GetDamage(uint id, object data)
    {
        if (data == null) { return; }
        Damagemsg = (worldboss.DamageRank)data;
        Refresh();
    }
    void GetBlessChange(uint id, object data)
    {
        if (data == null) { return; }
        blessInfo = (worldboss.BlessInfo)data;
        int coinEff = 0;
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1014), out coinEff);
        int goldEff = 0;
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1015), out goldEff);
        coinInspireNum = blessInfo.goldTimes;
        goldInspireNum = blessInfo.yuanbaoTimes;
        //Debug.Log($"{blessInfo.goldTimes} ---  {coinEff}  ------ {blessInfo.yuanbaoTimes} ----  {goldEff}");
        mlb_inspire.text = $"攻击+{(blessInfo.goldTimes * coinEff) / 100 + (blessInfo.yuanbaoTimes * goldEff) / 100}%";

        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_inspireEff, "effect_inspire_add");
        UtilityTips.ShowTips(1653, 1.5f, ColorType.Green);
    }
    void GetBossInfo(uint id, object data)
    {
        if (data == null) { return; }
        bossInfo = (worldboss.BossInfo)data;
        if (bossInfo.buffers.Count > buffCount)
        {
            if (UIManager.Instance.GetPanel<UIBloodMaskPanel>() != null)
            {
                mClientEvent.SendEvent(CEvent.BreathRedMask);
            }
            else
            {
                UIManager.Instance.CreatePanel<UIBloodMaskPanel>();
                UIManager.Instance.CreatePanel<UITotemPanel>();
            }
        }
        buffCount = bossInfo.buffers.Count;

        msli_bossHp.value = bossInfo.hp / (bossInfo.maxHp * 1f);
        mlb_hpPro.text = $"{(msli_bossHp.value * 100).ToString("F2")}%";
        List<int> buffIds = new List<int>();
        for (int i = 0; i < bossInfo.buffers.Count; i++)
        {
            //Debug.Log(i + "   " + bossInfo.buffers[i].layer + " buff数量是  " + bossInfo.buffers.Count);
            for (int j = 1; j <= bossInfo.buffers[i].layer; j++)
            {
                buffIds.Add(bossInfo.buffers[i].bufferId);
            }
        }
        for (int i = 0; i < buffitemList.Count; i++)
        {
            if (i >= buffIds.Count)
            {
                buffitemList[i].des.gameObject.SetActive(false);
            }
            else
            {
                buffitemList[i].des.gameObject.SetActive(true);
                buffitemList[i].SetValue(BufferTableManager.Instance.GetBufferTips(buffIds[i]));
            }
        }
        mobj_hint.SetActive(!(buffIds.Count > 0));
    }
    long leftTime = 0;
    void GetInstanceInfo(uint id, object data)
    {
        RefreshTime();
    }
    void GetInstanceFinish(uint id, object data)
    {
        Timer.Instance.CancelInvoke(Rankschedule);
        //Timer.Instance.CancelInvoke(countDownSchaedule);
        mlb_lefttime.text = "";
        InstanceInfo info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info != null && info.state == 3)
        {
            mobj_died.SetActive(true);
        }
        else
        {
            mobj_died.SetActive(false);
        }
        mobj_buffs.SetActive(false);
        mobj_hint.SetActive(false);
        msli_bossHp.gameObject.SetActive(false);
    }
    void GetInstanceChange(uint id, object data)
    {
        InstanceInfo info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info != null && info.state == 3)
        {
            Timer.Instance.CancelInvoke(Rankschedule);
            //Timer.Instance.CancelInvoke(countDownSchaedule);
            mlb_lefttime.text = "";
            mobj_died.SetActive(true);
            mobj_buffs.SetActive(false);
            mobj_hint.SetActive(false);
            msli_bossHp.gameObject.SetActive(false);
        }
    }
    void RefreshTime()
    {

        instance.InstanceInfo info = CSInstanceInfo.Instance.GetInstanceInfo();
        if (info != null)
        {
            leftTime = InstanceTableManager.Instance.GetWorldBossTotalTime() - (info.usedTime / 1000);
            //Debug.Log(InstanceTableManager.Instance.GetWorldBossTotalTime() + "  ****   " + info.usedTime + "   " + mlb_lefttime.text);
        }
        //if (leftTime != 0)
        //{
        //    mlb_lefttime.text = CSServerTime.Instance.FormatLongToTimeStr(leftTime, 3);
        //}
    }
    void GetRankPartChange(uint id, object data)
    {
        if (data == null) { return; }
        bool state = (bool)data;
        mobj_rankPart.SetActive(state);
        mobj_bossInfo.SetActive(!state);
        isRankOpen = state;
        if (isRankOpen) { Refresh(); }
    }
    public class WorldBossDamage
    {
        public GameObject go;
        public UILabel name;
        public UILabel rankNum;
        public WorldBossDamage(GameObject _go)
        {
            go = _go;
            name = go.transform.Find("name").GetComponent<UILabel>();
            rankNum = go.transform.Find("num").GetComponent<UILabel>();
        }
        public void RefreshItem(worldboss.DamageRankItem data)
        {
            name.text = data.roleName;
            rankNum.text = data.damage.ToString();
            if (CSMainPlayerInfo.Instance.ID == data.roleId)
            {
                name.color = CSColor.green;
                rankNum.color = CSColor.green;
            }
            else
            {
                name.color = CSColor.beige;
                rankNum.color = CSColor.beige;
            }
            //Debug.Log(data.roleName + "   " + data.rank + "   " + data.damage);
            //MenuInfo info = new MenuInfo();
            //info.sundryId = (int)PanelSelcetType.RoleTeam;
            //info.SetTeamTips(
            //    data.roleId,
            //    data.roleName,
            //    1,
            //    0,
            //    0,
            //    1, 0
            //    );
            //info.SetTeamTips(
            //    teamMember.roleId,
            //    teamMember.name,
            //    teamMember.sex,
            //    teamMember.level,
            //    0,
            //    teamMember.career,0
            //    );
            //CSSelectionManger.Instance.OpenSelectionPanel(info);
        }
        public void SetState(bool _state)
        {
            go.SetActive(_state);
        }
    }

    public class BuffItem
    {
        public UILabel des;
        public BuffItem(GameObject _go)
        {
            des = _go.GetComponent<UILabel>();
        }
        public void SetValue(string _str)
        {
            des.text = _str;
        }
    }

}
