using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SummonType
{
    None,
    Sabac,
    TimeLimitActivity,
    GuildCombat,
    GuildBoss,
    NostalgiaTeam,
    GuildCall,//行会召集令
}

public class CSSummonMgr : CSInfo<CSSummonMgr>
{


    PoolHandleManager mPoolHandle = new PoolHandleManager();

    CSBetterLisHot<CSSummonData> summonQuene = new CSBetterLisHot<CSSummonData>();//由于需求更改，不使用队列了

    CSBetterLisHot<CSSummonData> checkRepeatCache = new CSBetterLisHot<CSSummonData>();

    bool isShow;

    CSSummonData curShowSummon;


    public override void Dispose()
    {
        mPoolHandle?.OnDestroy();
        mPoolHandle = null;
        summonQuene?.Clear();
        summonQuene = null;
        checkRepeatCache?.Clear();
        checkRepeatCache = null;

        curShowSummon = null;

        
    }


    public void ShowSummon(string desc, System.Action<int, bool> callback, SummonType summonType = SummonType.None, int time = 8, long ID = 0)
    {
        if (!isShow)
        {
            isShow = true;
            CSSummonData data = mPoolHandle.GetCustomClass<CSSummonData>();
            data.Set(desc, callback, time, ID, summonType);
            checkRepeatCache.Add(data);
            UIManager.Instance.CreatePanel("UISummonPanel", UIManager.Instance.GetRoot(), (f) =>
            {
                RefreshPanel((f as UISummonPanel), data);
            });
        }
        else
        {
            if (checkRepeatCache.Any(x => { return x.summonType == summonType && x.ID == ID; }))//连续重复消息检测
            {
                return;
            }
            CSSummonData data = mPoolHandle.GetCustomClass<CSSummonData>();
            data.Set(desc, callback, time, ID, summonType);
            AddData(data);
            checkRepeatCache.Add(data);
        }
    }



    void RefreshPanel(UISummonPanel panel, CSSummonData data)
    {
        if (data == null) return;
        panel.RefreshUI(data.desc, data.callback, data.time, data.ID);
        curShowSummon = data;
    }


    public void TryToClose(UISummonPanel panel)
    {
        CSSummonData data = GetData();
        if (data != null)
        {            
            RefreshPanel(panel, data);
            if (checkRepeatCache.Contains(data))
            {
                checkRepeatCache.Remove(data);
            }
            mPoolHandle.Recycle(data);
        }
        else
        {
            curShowSummon = null;
            UIManager.Instance.ClosePanel<UISummonPanel>(true);
            isShow = false;
            for (int i = 0; i < checkRepeatCache.Count; i++)
            {
                mPoolHandle.Recycle(checkRepeatCache[i]);
            }
            checkRepeatCache.Clear();
        }
    }


    public void StopSummon(SummonType summonType = SummonType.None, long ID = 0)
    {
        if (curShowSummon != null && curShowSummon.summonType == summonType && curShowSummon.ID == ID)
        {
            mClientEvent.SendEvent(CEvent.CloseSummonPanel);
        }
        else
        {
            if (summonQuene != null)
            {
                CSSummonData data = summonQuene.FirstOrNull(x => { return x.summonType == summonType && x.ID == ID; });
                if (data == null) return;
                summonQuene.Remove(data);
                if (checkRepeatCache.Contains(data))
                {
                    checkRepeatCache.Remove(data);
                }
            }
        }
    }


    CSSummonData GetData()
    {
        if (summonQuene == null | summonQuene.Count < 1) return null;
        CSSummonData data = summonQuene[0];
        summonQuene.RemoveAt(0);
        return data;
    }
    
    void AddData(CSSummonData data)
    {
        if (data == null) return;
        if (summonQuene == null) summonQuene = new CSBetterLisHot<CSSummonData>();
        summonQuene.Add(data);
    }

}


public class CSSummonData : IDispose
{
    public string desc;
    public System.Action<int, bool> callback;
    public int time;
    public long ID;
    public SummonType summonType;


    public void Dispose()
    {
        callback = null;
    }

    public void Set(string _desc, System.Action<int, bool> _callback, int _time, long _ID, SummonType _summonType)
    {
        desc = _desc;
        callback = _callback;
        time = _time;
        ID = _ID;
        summonType = _summonType;
    }
}