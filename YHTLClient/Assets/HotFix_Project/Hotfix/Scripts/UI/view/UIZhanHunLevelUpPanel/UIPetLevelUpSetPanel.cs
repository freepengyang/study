using System.Collections.Generic;
using UnityEngine;



public partial class UIPetLevelUpSetPanel : UIBasePanel
{
    private CSPetLevelUpInfo Info;
    CSPopList mbenyuanqualityFilter;
    CSPopList mbenyuanlevelFilter;
    CSPopList mwolonglevelFilter;
    private Dictionary<UIToggle, bool> dicBoolItem;


    public override void Init()
    {
        base.Init();
        AddCollider();
        Info = CSPetLevelUpInfo.Instance;
        mbenyuanqualityFilter = new CSPopList(mobj_benyuanquality,mPoolHandleManager);
        mbenyuanlevelFilter = new CSPopList(mobj_benyuanlevel,mPoolHandleManager);
        mwolonglevelFilter = new CSPopList(mobj_wolonglevel,mPoolHandleManager);
        //var setPopStrs = CSPetLevelUpInfo.Instance.SetPopStrs;
        // 初始化设置信息
        var list = Info.callBackSetList;
        SetPopData(685,mbenyuanqualityFilter,1);
        SetPopData(686,mbenyuanlevelFilter,2);
        SetPopData(687,mwolonglevelFilter,3);

        if (dicBoolItem == null)
        {
            dicBoolItem = mPoolHandleManager.GetSystemClass<Dictionary<UIToggle, bool>>();
            dicBoolItem.Clear();
        }
        
        OnInitTick(mobj_tick_normal1,0);
        OnInitTick(mobj_tick_normal2,1);
        OnInitTick(mobj_tick_wolong1,2);
        OnInitTick(mobj_tick_wolong2,3);
        OnInitVipTick();
        UIEventListener.Get(mbtn_quesition).onClick = OnClickQuestion;
    }

    private void OnClickQuestion(GameObject obj)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.PetLevelUpSet);
    }

    
    protected void OnInitTick(UIToggle toggle,int index)
    {
        if (Info.SetBoolList.Count > index)
        {
            bool isToggle = Info.SetBoolList[index];
            toggle.value = isToggle;
        
            EventDelegate.Add(toggle.onChange, () =>
            {
                    Info.SetBoolList[index] = toggle.value;
            });
        }
        
    }

    protected void OnInitVipTick()
    {
        if (Info.SetBoolList.Count > 4)
        {
            mobj_tick_vip.value = Info.SetBoolList[4];
            EventDelegate.Add(mobj_tick_vip.onChange, () =>
            {
                if (mobj_tick_vip.value)
                {
                    if (CSMainPlayerInfo.Instance.VipLevel < Info.AutoRecycleVipLevel)
                    {
                        mobj_tick_vip.value = false;
                        Info.SetBoolList[4] = false;
                        UtilityTips.ShowPromptWordTips(95, ShowVIP); 
                    }
                    else
                        Info.SetBoolList[4] = mobj_tick_vip.value;
                }
            
            });
        }
        
        
    }


    protected void ShowVIP()
    {
        UtilityPanel.JumpToPanel(19000);
        UIManager.Instance.ClosePanel<UIPetLevelUpPanel>();
        Close();
    }

    public void SetPopData(int id,CSPopList list,int index)
    {
        string temp = SundryTableManager.Instance.GetSundryEffect(id);
        if (temp == null)
            return;
        string[] strList = temp.Split('&');
        list.MaxCount = strList.Length;
        
        for (int i = 0; i < strList.Length; i++)
        {
            var data = strList[i].Split('#');
            if (data.Length>=2)
            {
                list.mDatas[i].idxValue = i;
                list.mDatas[i].value = data[1];
            }
        }

        list.InitList((current) =>
        {
            if (Info.callBackSetList.Count > index)
                Info.callBackSetList[index] = current.idxValue;
        },false,7,false);
        int value = 0;
        if (Info.callBackSetList.Count > index)
            value = Info.callBackSetList[index];     
        list.SetCurValue(value,false);
    }

    protected override void OnDestroy()
    {
        mbenyuanqualityFilter = null;
        mbenyuanlevelFilter= null; 
        mwolonglevelFilter= null;
        mPoolHandleManager.Recycle(dicBoolItem);
        //保存设置
        Info.SendSetting();
        
        base.OnDestroy();
    }
}