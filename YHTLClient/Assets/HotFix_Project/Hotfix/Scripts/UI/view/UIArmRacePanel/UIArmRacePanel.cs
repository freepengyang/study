using activity;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIArmRacePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    #region  variable
    int armRank = 4;
    FastArrayElementFromPool<ArmRaceItem> armItems;
    List<GoalDatas> curGroupData = new List<GoalDatas>();
    List<ArmTaskItem> tasksList = new List<ArmTaskItem>();
    int curGroup = 0;
    ArmRaceItem curArmitem;
    int mobjWidth = 560;
    int armWidth = 232;
    string[] helpDes;
    List<int> helpIds;
    #endregion
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = CloseBtnClick;
        //UIEventListener.Get(mbtn_gift).onClick = GiftBtnClick;
        mscrollBar.onChange.Add(new EventDelegate(OnChange));
        UIEventListener.Get(mbtn_help).onClick = HelpBtnClick;

        helpDes = SundryTableManager.Instance.GetSundryEffect(625).Split('#');
        helpIds = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(626));

        armItems = mPoolHandleManager.CreateGeneratePool<ArmRaceItem>();
        armItems.Count = armRank;
        mgrid_armGrid.MaxCount = armRank;
        for (int i = 0; i < armRank; i++)
        {
            armItems[i].Init(mgrid_armGrid.controlList[i], i, ArmItemClick);
        }
        mClientEvent.AddEvent(CEvent.ResEquipCompetitionMessage, GetRamDataChange);


        Refresh();
        mscr_taskScr.ScrollImmidate(0);
        mscr_taskScr.ResetPosition();
    }

    public override void Show()
    {
        base.Show();
        SpringPanel.Begin(mscr_armScr.gameObject, new Vector3(-384 - ((curGroup - 1) * armWidth), -30, 0), 20f);

    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < armItems.Count; i++)
        {
            armItems[i].UnInit();
        }
        for (int i = 0; i < tasksList.Count; i++)
        {
            tasksList[i].Recycle();
        }
        base.OnDestroy();
    }
    void GetRamDataChange(uint id, object data)
    {
        Refresh();
        if (mobj_con.activeSelf)
        {
            RefreshTask();
        }
    }
    bool isUpgrade = false;
    Dictionary<int, GoalDatas> t_data = new Dictionary<int, GoalDatas>();
    void Refresh()
    {
        //Ӧ������curramItem��index ��ȡ����
        if (curGroup != CSArmRaceInfo.Instance.ReturnCurrentGroup())
        {
            isUpgrade = true;
        }
        else
        {
            isUpgrade = false;
        }
        curGroup = CSArmRaceInfo.Instance.ReturnCurrentGroup();
        CSArmRaceInfo.Instance.GetCurrentGroupDatas(curGroup, curGroupData);
        CSArmRaceInfo.Instance.GetCurrentGroupDicDatas(curGroup, t_data);
        for (int i = 0; i < armRank; i++)
        {
            if (CSArmRaceInfo.Instance.ReturnCurrentGroup(i + 1))
            {
                armItems[i].Refresh(2);
            }
            else
            {
                if ((i + 1) == curGroup)
                {
                    armItems[i].Refresh(1, curGroupData);
                    if (isUpgrade)
                    {
                        ArmItemClick(armItems[i]);
                    }
                    else
                    {
                        RefreshTask();
                    }
                }
                else
                {
                    armItems[i].Refresh(3);
                }
            }
        }
    }
    ILBetterList<TABLE.ARMSRACETASK> Dlist = new ILBetterList<TABLE.ARMSRACETASK>(10);
    ILBetterList<TABLE.ARMSRACETASK> Tlist = new ILBetterList<TABLE.ARMSRACETASK>(10);
    ILBetterList<TABLE.ARMSRACETASK> Flist = new ILBetterList<TABLE.ARMSRACETASK>(10);
    ILBetterList<TABLE.ARMSRACETASK> Result = new ILBetterList<TABLE.ARMSRACETASK>(10);

    List<TABLE.ARMSRACETASK> Taskslist = new List<TABLE.ARMSRACETASK>();
    void RefreshTask()
    {
        Taskslist.Clear();
        ArmsRaceTaskTableManager.Instance.GetTasksByGroup(curArmitem.index + 1, Taskslist);
        mgrid_taskGrid.MaxCount = Taskslist.Count;
        if (tasksList.Count < Taskslist.Count)
        {
            int gap = Taskslist.Count - tasksList.Count;
            for (int i = 0; i < gap; i++)
            {
                tasksList.Add(new ArmTaskItem());
            }
        }
        Dlist.Clear();
        Tlist.Clear();
        Flist.Clear();
        Result.Clear();
        for (int i = 0; i < Taskslist.Count; i++)
        {
            if (!t_data.ContainsKey(Taskslist[i].id))//δ���
            {
                Tlist.Add(Taskslist[i]);
            }
            else
            {
                if (t_data[Taskslist[i].id].reward == 1)//����ȡ
                {
                    Flist.Add(Taskslist[i]);
                }
                else
                {
                    if (t_data[Taskslist[i].id].value >= ArmsRaceTaskTableManager.Instance.GetArmsRaceTaskCount(Taskslist[i].id))
                    {
                        Dlist.Add(Taskslist[i]);//����ȡ
                    }
                    else
                    {
                        Tlist.Add(Taskslist[i]);
                    }
                }
            }
        }
        curArmitem.ChangeRedPointState((Dlist.Count > 0 ? true : false));
        Result.AddRange(Dlist);
        Result.AddRange(Tlist);
        Result.AddRange(Flist);
        for (int i = 0; i < tasksList.Count; i++)
        {
            //Debug.Log($"{Result[i].desc2}   {Result[i].count}");
            if (i >= Result.Count)
            {
                tasksList[i].UnInit();
            }
            else
            {
                tasksList[i].Init(mgrid_taskGrid.controlList[i], Result[i], curGroupData);
            }
        }
        //ˢ��  ��ʲô����װ������ʾ   �����ť����ʾ 
        //�߻�˵ȥ�� �ⲿ��
        //mbtn_gift.SetActive((curGroup == curArmitem.index + 1) ? true : false);
        int tempId = GiftBagTableManager.Instance.GetArmIdByGroupId(curArmitem.index);
        dailypurchase.GiftBuyInfo info = CSArmRaceInfo.Instance.GetGiftInfoById(tempId);
        mobj_giftRedPoint.SetActive(false);
        if (info.buyTimes == 0)
        {
            if (CSDayChargeInfo.Instance.GetDayCharge() >= GiftBagTableManager.Instance.GetGiftBagPara(tempId))
            {
                mobj_giftRedPoint.SetActive(true);
            }
        }
        mlb_help.text = helpDes[curArmitem.index];
    }
    #region ClickEvent
    void CloseBtnClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIArmRacePanel>();
    }
    void GiftBtnClick(GameObject _go)
    {
        UIManager.Instance.CreatePanel<UIArmRacePromptPanel>(p =>
        {
            (p as UIArmRacePromptPanel).SetType(curArmitem.index);
        });
    }
    void HelpBtnClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel((HelpType)helpIds[curArmitem.index]);
    }
    void OnChange()
    {
        if (mscrollBar.value >= 0.95)
        {
            mobj_arrow.SetActive(false);
        }
        else
        {
            mobj_arrow.SetActive(true);
        }
    }
    void ArmItemClick(ArmRaceItem _item)
    {
        if (_item.state == 3)
        {
            UtilityTips.ShowRedTips(1304);
            return;
        }
        else if (_item.state == 2)
        {
            UtilityTips.ShowRedTips(1305);
            return;
        }
        if (_item == curArmitem)
        {
            mobj_con.SetActive(!mobj_con.activeSelf);
        }
        else
        {
            curArmitem = _item;
            mobj_con.SetActive(true);
        }
        for (int i = 0; i < armItems.Count; i++)
        {
            if (mobj_con.activeSelf)
            {
                if (armItems[i].index > curArmitem.index)
                {
                    armItems[i].ChangePosition(mobjWidth + (armItems[i].index * armWidth));
                }
                else
                {
                    if (armItems[i].index == curArmitem.index)
                    {
                        mobj_con.transform.localPosition = new Vector3(armWidth * curArmitem.index, 0, 0);
                    }
                    armItems[i].ChangePosition((armItems[i].index * armWidth));
                }
            }
            else
            {
                armItems[i].ChangePosition((armItems[i].index * armWidth));
            }
        }
        if (mobj_con.activeSelf)
        {
            RefreshTask();
        }
        else
        {
            mscr_armScr.ScrollImmidate(0f);
        }
    }
    #endregion

}
public class ArmRaceItem
{
    public GameObject go;
    public GameObject bg;
    public GameObject bg_lock;
    public UISprite sp_title;
    public Transform itemPar;
    public GameObject btn_Get;
    public GameObject obj_Had;
    public UILabel lb_pro;
    public GameObject redPoint;
    public GameObject effect;
    public int index;
    public int state = 0;
    public UIItemBase item;
    Action<ArmRaceItem> action;
    int finishedCount = 0;
    int totalCount = 0;
    List<TABLE.ARMSRACETASK> Taskslist = new List<TABLE.ARMSRACETASK>();
    public ArmRaceItem()
    {

    }
    public void Init(GameObject _go, int _ind, Action<ArmRaceItem> _action)
    {
        go = _go;
        index = _ind;
        action = _action;
        bg = go.transform.Find("bg").gameObject;
        bg_lock = go.transform.Find("lock").gameObject;
        sp_title = go.transform.Find("sp_title").GetComponent<UISprite>();
        itemPar = go.transform.Find("itemPar");
        btn_Get = go.transform.Find("btn_get").gameObject;
        obj_Had = go.transform.Find("sp_get").gameObject;
        lb_pro = go.transform.Find("lb_hint").GetComponent<UILabel>();
        redPoint = go.transform.Find("redpoint").gameObject;
        effect = go.transform.Find("effect").gameObject;
        UIEventListener.Get(btn_Get).onClick = GetBtnClick;
        UIEventListener.Get(bg).onClick = BgClick;
        sp_title.spriteName = $"level_0_{index}";
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_arm_add");
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar, itemSize.Size60);
        }
        List<int> rewards = UtilityMainMath.SplitStringToIntList(PackTableManager.Instance.GetPackRewards(index + 1));
        item.Refresh(rewards[0]);
        item.SetCount(rewards[1]);
    }
    public void Refresh(int _state, List<GoalDatas> goals = null)
    {
        state = _state;
        obj_Had.SetActive(state == 2);
        CSEffectPlayMgr.Instance.ShowUITexture(bg, $"arm{index}_bg");
        //  1��ǰ�� ��ʾ����   2����ȡ  3δ����
        if (state == 1)
        {
            finishedCount = 0;
            for (int i = 0; i < goals.Count; i++)
            {
                if (goals[i].reward == 1)
                {
                    finishedCount++;
                }
            }
            Taskslist.Clear();
            ArmsRaceTaskTableManager.Instance.GetTasksByGroup(index + 1, Taskslist);
            totalCount = Taskslist.Count;
            //Debug.Log($"{finishedCount }   {totalCount}   {goals.Count}");
            if (goals.Count != 0 && finishedCount == totalCount)
            {
                btn_Get.SetActive(true);
                lb_pro.gameObject.SetActive(false);
                ChangeRedPointState(true);
            }
            else
            {
                lb_pro.gameObject.SetActive(true);
                lb_pro.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1994), $"{finishedCount}/{totalCount}");
                btn_Get.SetActive(false);
                ChangeRedPointState(false);
            }
            bg_lock.SetActive(false);
            bg.GetComponent<UITexture>().color = CSColor.white;
            effect.SetActive(true);
        }
        else if (state == 2)
        {
            btn_Get.SetActive(false);
            lb_pro.gameObject.SetActive(false);
            bg_lock.SetActive(false);
            effect.SetActive(false);
            bg.GetComponent<UITexture>().color = CSColor.gray;
            ChangeRedPointState(false);
        }
        else if (state == 3)
        {
            btn_Get.SetActive(false);
            lb_pro.gameObject.SetActive(true);
            lb_pro.text = ClientTipsTableManager.Instance.GetClientTipsContext(1995);
            bg_lock.SetActive(true);
            effect.SetActive(false);
            bg.GetComponent<UITexture>().color = CSColor.gray;
            CSEffectPlayMgr.Instance.ShowUITexture(bg_lock, $"arm_lock");
            ChangeRedPointState(false);
        }
    }
    void GetBtnClick(GameObject _go)
    {
        //Debug.Log($" ��ȡ��{index + 1}��Ľ���");
        Net.CSEquipCompetitionRewardMessage(0, index + 1);
    }
    void BgClick(GameObject _go)
    {
        if (action != null) { action(this); }
    }
    public void ChangePosition(float _x)
    {
        go.transform.localPosition = new Vector3(_x, 0, 0);
    }
    public void ChangeRedPointState(bool _state)
    {
        redPoint.SetActive(_state);
    }
    public void UnInit()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
        CSEffectPlayMgr.Instance.Recycle(bg);
        CSEffectPlayMgr.Instance.Recycle(bg_lock);
        UIItemManager.Instance.RecycleSingleItem(item);
        go = null;
    }
}

public class ArmTaskItem
{
    public GameObject go;
    public GameObject select;
    public Transform itemPar;
    public UILabel name;
    public GameObject btnGet;
    public GameObject btnGo;
    public GameObject objGot;
    public UISlider slider;

    //
    public UIItemBase item;
    TABLE.ARMSRACETASK data;
    GoalDatas mes;

    public ArmTaskItem()
    {

    }
    public void Init(GameObject _go, TABLE.ARMSRACETASK _data, List<GoalDatas> _mes)
    {
        go = _go;
        go.SetActive(true);
        data = _data;
        select = go.transform.Find("select").gameObject;
        itemPar = go.transform.Find("itemPar");
        name = go.transform.Find("lb_name").GetComponent<UILabel>();
        btnGet = go.transform.Find("btn_get").gameObject;
        btnGo = go.transform.Find("btn_go").gameObject;
        objGot = go.transform.Find("sp_get").gameObject;
        slider = go.transform.Find("slider_exp").GetComponent<UISlider>();
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, itemPar, itemSize.Size60);
        }
        mes = null;
        for (int i = 0; i < _mes.Count; i++)
        {
            if (_mes[i].configId == data.id)
            {
                mes = _mes[i];
            }
        }
        Refersh();
        UIEventListener.Get(btnGet).onClick = GetBtnClick;
        UIEventListener.Get(btnGo).onClick = GoBtnClick;
    }
    string des;
    public void Refersh()
    {
        List<List<int>> reward = UtilityMainMath.SplitStringToIntLists(data.rewards);
        item.Refresh(reward[0][0]);
        item.SetCount(reward[0][1]);
        if (mes == null)
        {
            btnGet.SetActive(false);
            btnGo.SetActive(true);
            objGot.SetActive(false);
            slider.value = 0f;
            name.text = $"{data.desc2}[00ff00]({0}/{data.count})";
        }
        else
        {
            if (mes.reward == 1)//����ȡ
            {
                btnGet.SetActive(false);
                btnGo.SetActive(false);
                objGot.SetActive(true);
                name.text = $"{data.desc2}[00ff00]({mes.value}/{data.count})";
                slider.value = 1f;
                return;
            }
            if (data.count <= mes.value)//����ȡ
            {
                btnGet.SetActive(true);
                btnGo.SetActive(false);
                objGot.SetActive(false);
                name.text = data.desc2;
                name.text = $"{data.desc2}[00ff00]({mes.value}/{data.count})";
                slider.value = 1f;
            }
            else
            {
                btnGet.SetActive(false);
                btnGo.SetActive(true);
                objGot.SetActive(false);
                name.text = data.desc2;
                name.text = $"{data.desc2}[ff0000]({mes.value}/{data.count})";
                slider.value = mes.value * 1f / data.count;
            }
        }
    }
    void GetBtnClick(GameObject _go)
    {
        //Debug.Log("Ҫ�콱������ID��  " + data.id);
        Net.CSEquipCompetitionRewardMessage(data.id, 0);
        //UIManager.Instance.ClosePanel<UIArmRacePanel>();
    }
    void GoBtnClick(GameObject _go)
    {
        //Debug.Log("Ҫ��ȥ�������  " + data.uiModel);
        UIManager.Instance.ClosePanel<UIArmRacePanel>();
        if (data.uiModel != 0)
        {
            UtilityPanel.JumpToPanel(data.uiModel);
        }
    }
    public void UnInit()
    {
        go.SetActive(false);
    }
    public void Recycle()
    {

        UIItemManager.Instance.RecycleSingleItem(item);
    }
}
