using fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Attribute = 1,//属性
    Damage = 2,//伤害
    Experience = 3,//经验
    Vertigo = 4,//眩晕
    SlowDown = 5,//减速
    Confusion = 6,//混乱
    Invincible = 7,//无敌
    Invisible = 8,//隐身
    ShieldValue = 9,//护盾值
    HPRecovery = 10,//血量回复
    MPRecovery = 11,//蓝量回复
    ShieldRecovery = 12,//护盾回复
    FixedBody = 13,//定身
    Silent = 14,//沉默
    ReboundDamage = 15,//反弹伤害
    //ReduceTreatment = 16,//降低治疗
    UnableHPRecovery = 17,//无法回血
    BlueShield = 18,//蓝盾
    EnemyConsumptionBlue = 19,//敌人耗蓝会受到等比伤害
    //ContinuousSkills = 20,//被连续释放技能加深伤害
    OpposeInvisible = 21,//反隐
    //NormalExperience = 30,//普通经验
    SealExperience = 31,//封印经验
}

public enum BuffEffectOpportunityType
{
    Immediately=1,//立即执行
    AfterDeath=2,//死亡后
    AfterRelive=3,//复活后
    Period=4,//时间周期
    BeHurt=5,//受到伤害
    BeTreatment=6,//收到治疗
    Delay=7,//固定延迟执行
}

public class UIItemBuff : UIBase
{
    public UIItemBuff(GameObject go)
    {
        obj = go;
    }

    GameObject obj;
    UISprite sp_item_bg { get { return obj ? (obj.transform.Find("sp_item_bg").GetComponent<UISprite>()) : null; } }
    UISprite sp_buff { get { return obj ? (obj.transform.Find("sp_buff").GetComponent<UISprite>()) : null; } }
    UISprite sp_time { get { return obj ? (obj.transform.Find("sp_time").GetComponent<UISprite>()) : null; } }
    UILabel lb_name { get { return obj ? (obj.transform.Find("lb_name").GetComponent<UILabel>()) : null; } }
    UILabel lb_total_time { get { return obj ? (obj.transform.Find("lb_total_time").GetComponent<UILabel>()) : null; } }
    UIGridContainer grid_addtion{ get { return obj ? (obj.transform.Find("grid_addtion").GetComponent<UIGridContainer>()) : null; } }
    UISprite line{ get { return obj ? (obj.transform.Find("line").GetComponent<UISprite>()) : null; } }

    BufferInfo buffInfo;
    TABLE.BUFFER tableBuffer;
    string[] arrContent;
    long remainingTime = 0;
    float percentage = 0;

    Schedule schedule;
    public Schedule itemSchedule { get => schedule; }

    int height = 0;
    public int Height{ get => height; }
    
    public void ShowOrHideLine(bool isShow)
    {
        line.gameObject.SetActive(isShow);
    }

    // public Action<GameObject> buffItemAction = null; 

    public void Refresh(BufferInfo msg, Action<UIItemBuff> action = null)
    {
        if (msg == null)
        {
            return;
        }
        height = 0;
        buffInfo = msg;
        tableBuffer = BufferTableManager.Instance[msg.bufferId];
        ShowTotalTime();
        ShowIcon();
        ShowName();
        ShowAttributeBuff();
        CalculateHeight();
    }

    void ShowIcon()
    {
        if (tableBuffer!=null)
        {
            sp_buff.spriteName = tableBuffer.icon;
        }
    }

    void ShowName()
    {
        if (tableBuffer!=null)
        {
            lb_name.text = tableBuffer.name;
        }
    }

    void ShowTotalTime()
    {
        if (tableBuffer == null) return;
        //倒计时（处理时间显示和图片遮挡弧度）
        arrContent = lb_total_time.FormatStr.Split('#');
        if (buffInfo.totalTime == 0)//如果永久，显示永久
        {
            CSStringBuilder.Clear();
            lb_total_time.text = CSStringBuilder.Append("[dcd5b8]",arrContent[0],"[-]").ToString();
            sp_time.gameObject.SetActive(false);
        }
        else if(buffInfo.totalTime >0)
        {
            remainingTime = (buffInfo.totalTime - (CSServerTime.Instance.TotalMillisecond - buffInfo.addTime))/1000;
            if (remainingTime < 0) return;
            sp_time.gameObject.SetActive(true);
            percentage = (float)remainingTime*1000 / buffInfo.totalTime;
            schedule = Timer.Instance.InvokeRepeating(0f, 1f, OnDesParticle);
        }
    }

    void OnDesParticle(Schedule schedule)
    {
        if (remainingTime<0)
        {
            if (Timer.Instance.IsInvoking(schedule))
                Timer.Instance.CancelInvoke(schedule);
            // buffItemAction?.Invoke(obj);
            return;
        }
        CSStringBuilder.Clear();
        string time = CSServerTime.Instance.FormatLongToTimeStr(remainingTime, 3);
        CSStringBuilder.Clear();
        lb_total_time.text = CSStringBuilder.Append("[ff9000]",/*arrContent[1],*/time,"[-]").ToString();
        remainingTime --;
        sp_time.fillAmount = percentage;
        percentage = (float)remainingTime*1000 / buffInfo.totalTime;
    }

    ILBetterList<string> listBuffEffect = new ILBetterList<string>();
    void ShowAttributeBuff()
    {
        Utility.SetAttributeBuff(listBuffEffect, buffInfo.bufferId);
        grid_addtion.MaxCount = listBuffEffect.Count;
        GameObject gp;
        UILabel lb_addtion;
        for (int i = 0; i < grid_addtion.MaxCount; i++)
        {
            gp = grid_addtion.controlList[i];
            lb_addtion = gp.transform.Find("lb_addtion").gameObject.GetComponent<UILabel>();
            lb_addtion.text = listBuffEffect[i];
        }
        UITable uiTable = grid_addtion.GetComponent<UITable>();
        uiTable.repositionNow = true;
        uiTable.Reposition();
    }

    void CalculateHeight()
    {
        //计算高度
        if (grid_addtion != null)
        {
            height = 55;//图片加两端高度
        }
        
        if (grid_addtion.MaxCount>0)
        {
            GameObject gp;
            UILabel lb;
            for (int i = 0; i < grid_addtion.MaxCount; i++)
            {
                gp = grid_addtion.controlList[i];
                lb = gp.transform.GetChild(0).GetComponent<UILabel>();
                height += lb.height;
            }
            height += 12;
        }
        sp_item_bg.height = height;
    }
    
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Timer.Instance.IsInvoking(schedule))
            Timer.Instance.CancelInvoke(schedule);
    }
}
