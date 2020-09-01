using System;
using System.Collections;
using System.Collections.Generic;
using instance;
using UnityEngine;

public partial class UIDungeonInspirePanel : UIBasePanel
{
    int CostNum;//花费元宝数量
    //FastArrayElementKeepHandle<SkillInfoData> mJobRelativedSkills;
    private DiLaoInfo diLaoInfo;

    private int curSkill;
    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.Reg((uint)CEvent.LeaveInstance, GetLeaveInstance);
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_refresh).onClick = ReFreshClick;
        UIEventListener.Get(mbtn_add, MoneyType.yuanbao).onClick = AddClick;
        mClientEvent.Reg((uint)CEvent.MoneyChange, OnMoneyChange);
        //HotManager.Instance.EventHandler.AddEvent(CEvent.SetSkilInfo, ShowSkillInfo);
        //mJobRelativedSkills = new FastArrayElementKeepHandle<SkillInfoData>(32);
        mClientEvent.Reg((uint)CEvent.DungeonInfo, ShowDungeonInfo);//显示波数信息
        CostNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(426));
        _dilaoSkillInfos = mPoolHandleManager.GetSystemClass<List<DilaoSkillInfo>>();
        _dilaoSkillInfos.Clear();
        //ShowDungeonInfo();
        RefreshUI();
    }

    private void OnMoneyChange(uint uievtid, object data)
    {
        mlb_value.text = CostNum.ToString().BBCode(((int)MoneyType.yuanbao).GetItemCount() < CostNum?ColorType.Red : ColorType.Green);
        //RefreshUI(false);
    }

    private void GetLeaveInstance(uint uiEvtID, object data)
    {
        Close();
    }


    
    
    private void RefreshUI(bool isFirst = true)
    {
        diLaoInfo = CSInstanceInfo.Instance.DiLaoInfo;
        int skillgroup = diLaoInfo.skillId;
        
        if (curSkill == skillgroup)
        {
            return;
        }
        curSkill = skillgroup;
        
        string level = "";
        if (skillgroup != 0)
        {
            //int groupid = SkillTableManager.Instance.GetSkillSkillGroup(skillgroup);
            var arr = DiLaoSkillTableManager.Instance.array.gItem.handles;
            for (int k = 0, max = arr.Length; k < max; ++k)
            {
                var item = arr[k].Value as TABLE.DILAOSKILL;
                if (item.skill == skillgroup.ToString())
                {
                    level = item.level;
                }
            }

        }

        if (string.IsNullOrEmpty(level))
        {
            FNDebug.Log("level is null");
        }
        string skillName = SkillTableManager.Instance.GetSkillName(skillgroup*1000 + 1);

        if (!isFirst)
        {
            UtilityTips.ShowGreenTips(CSString.Format(1018, skillName, level));
        }
        
        mlb_value.text = CostNum.ToString().BBCode(((int)MoneyType.yuanbao).GetItemCount() < CostNum?ColorType.Red : ColorType.Green);
        var datas = CSSkillInfo.Instance.GetJobRelativeSkills();
        //mJobRelativedSkills.Clear();
        //mJobRelativedSkills.AddRange(datas);
        _dilaoSkillInfos.Clear();
        
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].learned)
            {
                var info = mPoolHandleManager.GetSystemClass<DilaoSkillInfo>();
                info.Init(skillgroup,datas[i]);
                _dilaoSkillInfos.Add(info);
                //mJobRelativedSkills.RemoveAt(i);
                //i--;
            }
        }
        
        mgird_skill.MaxCount = _dilaoSkillInfos.Count;
        mgird_skill.MaxPerLine = 5;
        mgird_skill.Bind<DilaoSkillInfo,UIDungeonSkillBinder>(_dilaoSkillInfos,mPoolHandleManager);
    }

    private List<DilaoSkillInfo> _dilaoSkillInfos;
    
    
    private void ShowDungeonInfo(uint uiEvtID = 0, object data= null)
    {
        
        RefreshUI(false);
        GetGuWuInfo(diLaoInfo.skillId);
        
    }

    public void GetGuWuInfo(int skillgroup) {
        mClientEvent.SendEvent(CEvent.GuWuSkill, skillgroup);
        //int groupid = SkillTableManager.Instance.GetSkillSkillGroup(skillid);
        //DiLaoSkillTableManager.Instance.dic.GetEnumerator()
        var arr = DiLaoSkillTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.DILAOSKILL;
            if (item.skill == skillgroup.ToString())
            {
                string str =  string.Format(mlb_hint.FormatStr, item.level);
                mlb_hint.text = str;
                //mClientEvent.SendEvent(CEvent.ChangeHeadSkillInfo,str);
            }
        }
    }
    
    
    private void AddClick(GameObject obj)
    {
        //获取类型
        Utility.ShowGetWay((int)MoneyType.yuanbao);
    }
    private void ClosePanel(GameObject obj) {
        //OnDestroy();
        Close();
    }
    //刷新鼓舞技能
    private void ReFreshClick(GameObject obj)
    {
        
        if (!Utility.JudgeCharge<UIDungeonInspirePanel>(CostNum))
        {
            //Utility.ShowGetWay((int)MoneyType.yuanbao);
            return;
        }
        else
        {
            Net.ReqDiLaoGuWuMessage();
        }
        
    }
    protected override void OnDestroy()
    {
        
        if (destroyed)
            return;
        
        mgird_skill.UnBind<UIDungeonSkillBinder>();
        mgird_skill = null;
        base.OnDestroy();
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    } 
}

public class DilaoSkillInfo : IDispose
{
    public int skillgroup;
    public SkillInfoData skillInfoData;

    public void Init(int group, SkillInfoData data)
    {
        skillgroup = group;
        skillInfoData = data;
    }

    public void Dispose()
    {
        skillInfoData = null;
    }
}
