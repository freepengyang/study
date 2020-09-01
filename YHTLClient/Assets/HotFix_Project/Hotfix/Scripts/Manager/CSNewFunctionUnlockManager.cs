using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public class CSNewFunctionUnlockManager : CSInfo<CSNewFunctionUnlockManager>
{
    Stack<System.Action> mNewFunctionOpenList = new Stack<System.Action>(32);
    public bool Poped
    {
        get;set;
    }

    int skillUnlockSundryId = 653;
    HashSet<int> mSkillGroupSet = new HashSet<int>();
    void InitSkillUnlockTags()
    {
        TABLE.SUNDRY sundryItem = null;
        if(!SundryTableManager.Instance.TryGetValue(skillUnlockSundryId,out sundryItem))
        {
            return;
        }
        var tokens = sundryItem.effect.Split('#');
        for(int i = 0; i < tokens.Length; ++i)
        {
            int v = 0;
            if(int.TryParse(tokens[i],out v))
            {
                mSkillGroupSet.Add(v);
            }
        }
    }

    public bool GetSkillGroupNeedExpress(int groupId)
    {
        return mSkillGroupSet.Contains(groupId);
    }

    public void Initialize()
    {
        InitSkillUnlockTags();
        HotManager.Instance.EventHandler.AddEvent(CEvent.MainPlayer_LevelChange,OnMainPlayer_LevelChange);
        mNewFunctionOpenList.Clear();
        Poped = false;
    }

    public void TriggerNextAction()
    {
        if(mNewFunctionOpenList.Count > 0 && !Poped)
        {
            mNewFunctionOpenList.Pop().Invoke();
        }
    }

    protected void OnMainPlayer_LevelChange(uint id,object argv)
    {
        int lv = CSMainPlayerInfo.Instance.Level;
        var openList = FuncOpenTableManager.Instance.GetNewFuncOpen(lv);
        if (null == openList || openList.Count <= 0)
            return;

        for (int i = 0; i < openList.Count; ++i)
        {
            var openItem = openList[i];
            if (null == openItem || openItem.unlockid <= 0)
                continue;

            if(CSMainPlayerInfo.Instance.ServerOpenDay < openItem.openday && openItem.openday > 0)
            {
                continue;
            }

            TriggerNewFuncOpen(openItem);
            break;
        }
    }

    public void TestTrigger(int functionId)
    {
        TABLE.FUNCOPEN funItem = null;
        if (!FuncOpenTableManager.Instance.TryGetValue(functionId, out funItem))
            return;

        TriggerNewFuncOpen(funItem);
    }

    public void PreviewFuncOpen(int functionId)
    {
        TABLE.FUNCOPEN funItem = null;
        if (!FuncOpenTableManager.Instance.TryGetValue(functionId, out funItem))
            return;

        UIManager.Instance.CreatePanel<UIComingFunctionPanel>(f =>
        {
            (f as UIComingFunctionPanel).Show(funItem);
        });
    }

    public void TriggerNewSkillGet(int skillId,int slotId)
    {
        TABLE.SKILL skillItem = null;
        if(!SkillTableManager.Instance.TryGetValue(skillId,out skillItem))
        {
            return;
        }

        mNewFunctionOpenList.Push(() =>
        {
            Poped = true;
            UIManager.Instance.CreatePanel<UINewSkillPanel>(f =>
            {
                (f as UINewSkillPanel).Show(skillItem, slotId);
            });
        });
        TriggerNextAction();
    }

    void TriggerNewFuncOpen(TABLE.FUNCOPEN funcItem)
    {
        mNewFunctionOpenList.Push(() =>
        {
            Poped = true;
            UIManager.Instance.CreatePanel<UINewFunctionPanel>(f =>
            {
                (f as UINewFunctionPanel).Show(funcItem);
            });
        });
        TriggerNextAction();
    }

    public void ClearCB()
    {
        mNewFunctionOpenList?.Clear();
    }

    public override void Dispose()
    {
        Poped = false;
        mNewFunctionOpenList?.Clear();
        mNewFunctionOpenList = null;
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_LevelChange, OnMainPlayer_LevelChange);
    }
}
