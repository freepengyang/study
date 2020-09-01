using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TABLE;
using UnityEngine;

public enum GuildObjectType
{
    GOT_EVENTLISTENER = 1,
    GOT_BUTTON = 2,
    GOT_TOGGLE = 3,
}

public class GuideItemData
{
    public TABLE.GUIDEGROUP item;
    public int triggerLevel;
    public int triggerTaskType;
    public int triggerTaskId;
    public int triggerStatus;
    public string panelName;
}

public interface IConditionTrigger
{
    int Index { get; set; }
    void Create(GuideItemData guideItemData);
    bool Condition(object argv);
    void Trigger();
    string Description();
    void Destroy();
    bool Auto();
}

public class CSGuideManager : CSInfo<CSGuideManager>
{
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    Dictionary<string, UIBase> mActivedPanels = new Dictionary<string, UIBase>();
    string GuidActionFinger = @"Finger";
    public int CurrentGuideId = -1;
    System.Action CurrentGuideAction = null;
    /// <summary>
    /// 注意这里的引导是按组保存的 groupid = id / 1000 step = id%1000
    /// 向服务器发送的第一个字段是 TABLE.GUIDEGROUP 表id
    /// </summary>
    HashSet<int> mTriggeredGuides = new HashSet<int>();
    public bool NeedRecovery
    {
        get;private set;
    }

    public bool IsOtherGroupGuiding(int guideId)
    {
        if (CurrentGuideId == -1)
            return false;
        if (CurrentGuideId/1000 == guideId/1000)
            return false;
        return true;
    }

    public bool IsGuiding
    {
        get
        {
            return CurrentGuideId != -1;
        }
    }

    Dictionary<int, List<IConditionTrigger>> mConditionTriggers;
    public void InitTriggers()
    {
        mConditionTriggers = new Dictionary<int, List<IConditionTrigger>>(32);
        var arr = GuideGroupTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            IConditionTrigger trigger = null;
            var item = arr[i].Value as TABLE.GUIDEGROUP;
            //KeyValuePair<int, TABLE.GUIDEGROUP> kv = (KeyValuePair<int, TABLE.GUIDEGROUP>)it.Current;
            //等级达成
            if (item.TriggerType == 1)
            {
                int triggerLv = 0;
                if (!int.TryParse(item.beginParam, out triggerLv))
                    continue;
                trigger = new LevelConditionTrigger();
                var guideItemData = new GuideItemData();
                guideItemData.item = item;
                guideItemData.triggerLevel = triggerLv;
                trigger.Create(guideItemData);
            }//界面打开触发器
            else if (item.TriggerType == 4)
            {
                trigger = new PanelConditionTrigger();
                var guideItemData = new GuideItemData();
                guideItemData.item = item;
                guideItemData.panelName = item.beginParam;
                trigger.Create(guideItemData);
            }//任务触发器
            else if(item.TriggerType == 2)
            {
                var tokens = item.beginParam.Split('#');
                if (tokens.Length != 2)
                    continue;
                int triggerTaskId = 0;
                if (!int.TryParse(tokens[0], out triggerTaskId))
                    continue;
                int triggerStatus = 0;
                if (!int.TryParse(tokens[1], out triggerStatus))
                    continue;
                trigger = new TaskConditionTrigger();
                var guideItemData = new GuideItemData();
                guideItemData.item = item;
                guideItemData.triggerTaskId = triggerTaskId;
                guideItemData.triggerStatus = triggerStatus;
                trigger.Create(guideItemData);
            }
            else if(item.TriggerType == -2)
            {
                var tokens = item.beginParam.Split('#');
                if (tokens.Length != 2)
                    continue;
                int triggerTaskType = 0;
                if (!int.TryParse(tokens[0], out triggerTaskType))
                    continue;
                int triggerStatus = 0;
                if (!int.TryParse(tokens[1], out triggerStatus))
                    continue;
                trigger = new ResidentTaskConditionTrigger();
                var guideItemData = new GuideItemData();
                guideItemData.item = item;
                guideItemData.triggerTaskType = triggerTaskType;
                guideItemData.triggerStatus = triggerStatus;
                trigger.Create(guideItemData);
            }
            else if(item.TriggerType == 3)
            {
                //气泡触发器(气泡出现的时候触发)
                int bubbleId = 0;
                if (!int.TryParse(item.beginParam, out bubbleId))
                    continue;
                trigger = new BubbleConditionTrigger();
                var guideItemData = new GuideItemData();
                guideItemData.item = item;
                guideItemData.triggerTaskId = bubbleId;
                trigger.Create(guideItemData);
            }
            else if(item.TriggerType == -100)
            {
                var pms = item.beginParam.Split('#');
                if (pms.Length < 2)
                    continue;
                int logicId = -1;
                if (!int.TryParse(pms[0], out logicId))
                    continue;

                if(logicId == 1)
                {
                    if (pms.Length != 4)
                        continue;
                    int taskId = -1;
                    if (!int.TryParse(pms[1], out taskId))
                        continue;
                    int status = -1;
                    if (!int.TryParse(pms[2], out status))
                        continue;
                    int count = -1;
                    if (!int.TryParse(pms[3], out count))
                        continue;
                    trigger = new ResidentLogicTrigger();
                    (trigger as ResidentLogicTrigger).LogicId = logicId;
                    var guideItemData = new GuideItemData();
                    guideItemData.item = item;
                    guideItemData.triggerTaskId = taskId;
                    guideItemData.triggerStatus = status;
                    guideItemData.triggerLevel = count;
                    trigger.Create(guideItemData);
                }
            }

            if(!mConditionTriggers.ContainsKey(item.TriggerType))
            {
                mConditionTriggers.Add(item.TriggerType, new List<IConditionTrigger>(16));
            }
            List<IConditionTrigger> triggers = mConditionTriggers[item.TriggerType];

            triggers.Add(trigger);
        }
    }

    public void InitGuideSet(RepeatedField<int> newbiewGuides)
    {
#if UNITY_EDITOR
        if(newbiewGuides.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            stringBuilder.Append($"<color=#00ff00>[新手引导]:服务器已经保存的数据 >>>");
            for(int i = 0,max = newbiewGuides.Count;i<max;++i)
            {
                int id = newbiewGuides[i] >> 16;
                stringBuilder.Append($"|[{id}]");
            }
            stringBuilder.Append("</color>");
            FNDebug.Log(stringBuilder.ToString());
        }
        else
        {
            FNDebug.Log($"<color=#00ff00>[新手引导]:服务器还没有任何新手引导数据</color>");
        }
#endif
        mTriggeredGuides.Clear();
        for(int i = 0,max = newbiewGuides.Count;i < max;++i)
        {
            mTriggeredGuides.Add((newbiewGuides[i] >> 16)/1000);
        }
    }

    public bool IsGuideTriggered(int id)
    {
        return mTriggeredGuides.Contains(id/1000);
    }

    public void Initialize(RepeatedField<int> newbiewGuides)
    {
        InitGuideSet(newbiewGuides);
        InitTriggers();
        HotManager.Instance.EventHandler.AddEvent(CEvent.OnTaskGuideNameChanged, OnTaskChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.Task_FinshGuide, OnTaskChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.Task_GoalUpdate, OnTaskChanged);
        HotManager.Instance.EventHandler.AddEvent(CEvent.ItemListChange, OnItemListChange);
        HotManager.Instance.EventHandler.AddEvent(CEvent.OpenPanel, OnPanelOpen);
        HotManager.Instance.EventHandler.AddEvent(CEvent.MainPlayer_LevelChange, OnMainPlayer_LevelChange);
        HotManager.Instance.EventHandler.AddEvent(CEvent.BubbleGuide, OnBubbleGuideChanged);

        //初始化主动触发一下
        var mission = CSMissionManager.Instance.GetMissionByType(TaskType.MainLine);
        if (null != mission)
        {
            if(!Trigger(2, mission))
            {
                Trigger(-100, mission);
            }
        }
    }

    protected void OnBubbleGuideChanged(uint id, object argv)
    {
        if (argv is int bubbleId)
        {
            Trigger(3, argv);
        }
    }

    protected void OnPanelOpen(uint id, object argv)
    {
        if (argv is UIBase basePanel)
        {
            Trigger(4, argv,false);
        }
    }

    protected void OnMainPlayer_LevelChange(uint id,object argv)
    {
        TryStopUnLevelFixGuide();
        Trigger(1,argv);
    }

    protected void TryStopUnLevelFixGuide()
    {
        if(CurrentGuideId != -1)
        {
            TABLE.GUIDEGROUP guideItem = null;
            if(!GuideGroupTableManager.Instance.TryGetValue(CurrentGuideId,out guideItem))
            {
                CurrentGuideId = -1;
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束[id:{0}] 无法在引导表中被找到</color>", CurrentGuideId);
                return;
            }

            int playerLv = CSMainPlayerInfo.Instance.Level;
            if(playerLv < guideItem.beginLv || playerLv > guideItem.endLv)
            {
                ResetGuideStep(false);
                CurrentGuideId = -1;
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:[id:{0}]:超过等级范围[{1},{2}]</color>",
                    guideItem.id, guideItem.beginLv, guideItem.endLv);
            }
        }
    }

    protected void OnTaskChanged(uint id, object argv)
    {
        if(argv is int taskId)
        {
            var mission = CSMissionManager.Instance.GetMissionByTaskId(taskId);
            if (null != mission)
            {
                if (!Trigger(2, mission))
                {
                    Trigger(-100, mission);
                }
            }
        }
    }

    protected void OnItemListChange(uint id, object argv)
    {
        var mission = CSMissionManager.Instance.GetMissionByTaskId(0);
        Trigger(-100, mission);
    }

    public bool Trigger(int triggerType,object argv,bool byHand = false)
    {
        if (!mConditionTriggers.ContainsKey(triggerType))
        {
            return false;
        }
        var list = mConditionTriggers[triggerType];
        for (int i = 0; i < list.Count; ++i)
        {
            var trigger = list[i];
            if (null == trigger)
                continue;

            if(!byHand && !trigger.Auto())
            {
                continue;
            }

            if (!trigger.Condition(argv))
            {
                continue;
            }
            NeedRecovery = false;
            //Debug.LogFormat("<color=#00ff00>[(关闭)引导重启埋点]:{0}</color>", argv.GetType());
            trigger.Trigger();
            return true;
        }
        return false;
    }

    public void Register(UIBase panel)
    {
        if (null != panel)
        {
            var type = panel.GetType();
            if (mActivedPanels.ContainsKey(type.Name))
            {
                mActivedPanels[type.Name] = panel;
            }
            else
            {
                mActivedPanels.Add(panel.GetType().Name, panel);
            }
            //UnityEngine.Debug.LogFormat("<color=#00ff00>[界面]->[{0}]</color>", panel.GetType().Name);
        }
    }

    public void Remove(UIBase panel)
    {
        if(null != panel)
            mActivedPanels.Remove(panel.GetType().Name);
    }

    //获得Transform
    Transform GetTransform(string name,string path)
    {
        if(!mActivedPanels.ContainsKey(name))
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[获取控件] ==> 界面[{0}]不存在</color>", name);
            return null;
        }
        var panel = mActivedPanels[name];
        var transform = panel.Get(path);
        if(null == transform)
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[获取控件] ==> 控件无法找到 路径:[{0}]</color>", path);
            return null;
        }
        return transform;
    }

    public void ResetGuideStep(bool onDestroy = false)
    {
        var action = CurrentGuideAction;
        CurrentGuideAction = null;
        action?.Invoke();
        HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeGuidePanelVisible, false);
    }

    protected void TriggerNext()
    {
        CurrentGuideAction?.Invoke();
        CurrentGuideAction = null;
        HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeGuidePanelVisible, false);

        ExecutePrevConditionTrigger();
    }

    protected void TriggerNext(GameObject go)
    {
        CurrentGuideAction?.Invoke();
        CurrentGuideAction = null;
        HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeGuidePanelVisible, false);

        ExecutePrevConditionTrigger();
    }

    protected void ExecutePrevConditionTrigger()
    {
        if (-1 != CurrentGuideId)
        {
            TABLE.GUIDEGROUP guideItem = null;
            if (!GuideGroupTableManager.Instance.TryGetValue(CurrentGuideId, out guideItem))
            {
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束[id:{0}] 无法在引导表中被找到</color>", CurrentGuideId);
                CurrentGuideId = -1;
                return;
            }

            TABLE.GUIDEGROUP nextGuideItem = null;
            if (!GuideGroupTableManager.Instance.TryGetValue(CurrentGuideId + 1, out nextGuideItem))
            {
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束[id:{0}]</color>", CurrentGuideId);
                CurrentGuideId = -1;
                //注意这里保存是已经完成的所以是 guideItem 而不是 nextGuideItem
                SendGuideEndMsg(guideItem);
                return;
            }
            //由前置完成后触发
            if (nextGuideItem.TriggerType == 0)
            {
                FNDebug.LogFormat("<color=#ff00ff>[新手引导]:执行前置触发[id:{0}]</color>", CurrentGuideId);
                Trigger(CurrentGuideId + 1);
            }
        }
    }

    Schedule mScheduleDelayCall;
    protected void DelayNext(Schedule schedule)
    {
        Trigger(CurrentGuideId + 1);
    }

    Dictionary<string, int> mString2CtrlType = new Dictionary<string, int>
    {
        { "UIButton",2 },
        { "UIToggle",3 },
        { "UIEventListener",1 },
    };

    void SetupEventListener(Transform transform, int type, TABLE.GUIDEGROUP guideItem, int gameModelId)
    {
        CurrentGuideAction = null;
        var handle = transform.GetComponent<UIEventListener>();
        if (null == handle)
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[获取控件] ==> 控件类型错误 类型:[{0}]</color>", type);
            return;
        }

        int lockedGuideId = CurrentGuideId;
        int currentGroupId = CurrentGuideId / 1000;
        var guideLife = transform.gameObject.AddComponent<GuideLife>();
        int guideId = guideItem.id;

        //注册销毁流程
        guideLife.onDeath = (bool byHand) =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[{0}]:[控件销毁]:UIEventListener</color>", guideId);
            if(byHand)
            {
                if (currentGroupId == CurrentGuideId / 1000)
                {
                    TABLE.GUIDEGROUP nextGuideItem = null;
                    if (!GuideGroupTableManager.Instance.TryGetValue(lockedGuideId + 1, out nextGuideItem))
                    {
                        FNDebug.LogFormat("<color=#00ff00>[新手引导]:引导结束[id:{0}]</color>", lockedGuideId);
                        CurrentGuideId = -1;
                        //注意这里保存是已经完成的所以是 guideItem 而不是 nextGuideItem
                        SendGuideEndMsg(guideItem);
                        return;
                    }

                    int playerLv = CSMainPlayerInfo.Instance.Level;
                    if(playerLv < nextGuideItem.beginLv || playerLv > nextGuideItem.endLv)
                    {
                        CurrentGuideId = -1;
                        FNDebug.LogFormat("<color=#ff0000>[新手引导]:同组引导被中断</color>", type);
                        return;
                    }
                }
            }
            else
            {
                if (currentGroupId == CurrentGuideId / 1000)
                {
                    CurrentGuideId = -1;
                    FNDebug.LogFormat("<color=#ff0000>[新手引导]:同组引导被中断</color>", type);
                }
            }
            ResetGuideStep(true);
        };

        //注册下一步触发流程
        TABLE.GAMEMODELS gameModel = null;
        if (!GameModelsTableManager.Instance.TryGetValue(gameModelId, out gameModel))
        {
            var onClick = handle.onClick;

            handle.onClick = (f) =>
             {
                 TriggerNext(f);
                 onClick?.Invoke(f);
             };

            CurrentGuideAction = () =>
            {
                handle.onClick = onClick;
                guideLife.Remove();
            };
        }
        else
        {
            var onClick = handle.onClick;
            System.Action<GameObject> modelLink = (f) =>
            {
                UtilityPanel.JumpToPanel(gameModelId);
                TriggerNext(f);
            };
            handle.onClick = modelLink;

            CurrentGuideAction = () =>
            {
                handle.onClick = onClick;
                guideLife.Remove();
            };
        }

        //注册启动流程
        guideLife.onStart = () =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[{0}][正式启动]...</color>", guideId);
            Start(handle.transform, guideItem);
        };
    }

    void SetupButton(Transform transform, int type, TABLE.GUIDEGROUP guideItem, int gameModelId)
    {
        CurrentGuideAction = null;
        var handle = transform.GetComponent<UIButton>();
        if (null == handle)
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[获取控件] ==> 控件类型错误 类型:[{0}]</color>", type);
            return;
        }

        int currentGroupId = CurrentGuideId / 1000;
        var guideLife = transform.gameObject.AddComponent<GuideLife>();

        //注册销毁流程
        guideLife.onDeath = (bool byHand) =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[控件销毁] UIBUTTON</color>", type);
            if (!byHand && currentGroupId == CurrentGuideId / 1000)
            {
                CurrentGuideId = -1;
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:同组引导被中断</color>", type);
            }
            ResetGuideStep(true);
        };

        //注册下一步触发流程
        TABLE.GAMEMODELS gameModel = null;
        if (!GameModelsTableManager.Instance.TryGetValue(gameModelId, out gameModel))
        {
            EventDelegate.Remove(handle.onClick, TriggerNext);
            EventDelegate.Add(handle.onClick, TriggerNext);

            CurrentGuideAction = () =>
            {
                EventDelegate.Remove(handle.onClick, TriggerNext);
                guideLife.Remove();
            };
        }
        else
        {
            var eventDelegates = handle.onClick;
            var triggerDelegates = mPoolHandle.GetSystemClass<List<EventDelegate>>();
            EventDelegate.Add(triggerDelegates, () =>
            {
                UtilityPanel.JumpToPanel(gameModelId);
            });
            EventDelegate.Add(triggerDelegates, TriggerNext);
            handle.onClick = triggerDelegates;

            CurrentGuideAction = () =>
            {
                triggerDelegates.Clear();
                mPoolHandle.Recycle(triggerDelegates);
                handle.onClick = eventDelegates;
                guideLife.Remove();
            };
        }

        //注册启动流程
        guideLife.onStart = () =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[{0}][正式启动]...</color>", guideItem.id);
            Start(handle.transform, guideItem);
        };
    }

    void SetupToggle(Transform transform, int type, TABLE.GUIDEGROUP guideItem, int gameModelId)
    {
        CurrentGuideAction = null;
        var handle = transform.GetComponent<UIToggle>();
        if (null == handle)
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[获取控件] ==> 控件类型错误 类型:[{0}]</color>", type);
            return;
        }

        int currentGroupId = CurrentGuideId / 1000;
        var guideLife = transform.gameObject.AddComponent<GuideLife>();

        //注册销毁流程
        guideLife.onDeath = (bool byHand) =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[控件销毁] UIToggle</color>", type);
            if (!byHand && currentGroupId == CurrentGuideId / 1000)
            {
                CurrentGuideId = -1;
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:同组引导被中断</color>", type);
            }
            ResetGuideStep(true);
        };

        //注册下一步触发流程
        EventDelegate.Remove(handle.onChange, TriggerNext);
        EventDelegate.Add(handle.onChange, TriggerNext);
        CurrentGuideAction = () =>
        {
            EventDelegate.Remove(handle.onChange, TriggerNext);
            guideLife.Remove();
        };

        //注册启动流程
        guideLife.onStart = () =>
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[{0}][正式启动]...</color>", guideItem.id);
            Start(handle.transform, guideItem);
        };
    }

    void Setup(Transform transform,int type, TABLE.GUIDEGROUP guideItem,int gameModelId)
    {
        GuildObjectType guildObjectType = (GuildObjectType)type;
        if (guildObjectType == GuildObjectType.GOT_EVENTLISTENER)
        {
            SetupEventListener(transform, type, guideItem, gameModelId);
            return;
        }

        if (guildObjectType == GuildObjectType.GOT_BUTTON)
        {
            SetupButton(transform, type, guideItem, gameModelId);
            return;
        }

        if(guildObjectType == GuildObjectType.GOT_TOGGLE)
        {
            SetupToggle(transform, type, guideItem, gameModelId);
        }
    }

    public void SendGuideEndMsg(TABLE.GUIDEGROUP guideItem)
    {
        if(null != guideItem)
        {
            int startId = guideItem.id / 1000 * 1000 + 1;
            if(GuideGroupTableManager.Instance.TryGetValue(startId,out TABLE.GUIDEGROUP ggitem) && ggitem.TriggerType < 0)
            {
                FNDebug.LogFormat("<color=#00ff00>[新手引导]:[触发完成]:[GUIDEGROUPID:{0}]:[触发类型值:{1}]</color>", guideItem.id,ggitem.TriggerType);
                return;
            }
            mTriggeredGuides.Add(guideItem.id/1000);
            FNDebug.LogFormat("<color=#00ff00>[新手引导]:[向服务器发送数据保存]:[GUIDEGROUPID:{0}]</color>", guideItem.id);
            Net.ReqSaveNewbieGuideMessage(guideItem.id,0);
        }
    }

    public int LastGuideDailyTaskId
    {
        get;set;
    }
    public void ResidentDailyTrigger(int guidId,int taskId)
    {
        if (!mConditionTriggers.ContainsKey(-2))
        {
            return;
        }
        var triggers = mConditionTriggers[-2];
        if (triggers.Count <= 0)
            return;

        if(!(triggers[0] is ResidentTaskConditionTrigger residentTaskConditionTrigger))
        {
            return;
        }
        residentTaskConditionTrigger.Value.triggerTaskId = taskId;
        LastGuideDailyTaskId = taskId;
        //residentTaskConditionTrigger.Value.triggerStatus = status;

        var mission = CSMissionManager.Instance.GetMissionByTaskId(taskId);
        if (null != mission)
        {
            Trigger(-2, mission);
        }
    }

    public void Trigger(int guidId,params int[] argvs)
    {
        if(guidId / 1000 == CurrentGuideId / 1000 && guidId <= CurrentGuideId)
        {
            return;
        }

        //如果不是同一组
        if (guidId/1000 != CurrentGuideId/1000 && CurrentGuideId != -1)
        {
            FNDebug.LogFormat("<color=#ff00ff>[新手引导]:[其他引导[id:{0}]触发中无法触发新的引导][id:{1}]</color>", CurrentGuideId, guidId);
            return;
        }
        FNDebug.LogFormat("<color=#ff00ff>[新手引导]:[触发引导][id:{0}]</color>",guidId);
        ResetGuideStep();

        TABLE.GUIDEGROUP guideItem = null;
        if(!GuideGroupTableManager.Instance.TryGetValue(guidId,out guideItem))
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束[id:{0}]</color>", guidId);
            return;
        }

        if (string.IsNullOrEmpty(guideItem.Links))
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束[没有引导内容][id:{0}]</color>", guidId);
            return;
        }

        CurrentGuideId = guidId;
        ExecuteCmdLink(guideItem,argvs);
    }

    protected void ExecuteCmdLink(TABLE.GUIDEGROUP guideItem,params int[] argvs)
    {
        var link = guideItem.Links;
        var tokens = link.Split('#');
        if (tokens.Length < 2)
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束 参数个数不能小于 2 content = {0} Id = {1}</color>", link,guideItem.id);
            ResetGuideStep();
            return;
        }

        int cmd;
        if (!int.TryParse(tokens[0], out cmd))
        {
            CurrentGuideId = -1;
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:引导结束 第一个参数必须为整数 content = {0} Id = {1}</color>", link, guideItem.id);
            ResetGuideStep();
            return;
        }

        if(cmd == 1)
        {
            if (!(tokens.Length >= 4 && tokens.Length <= 5))
            {
                CurrentGuideId = -1;
                FNDebug.LogFormat("<color=#ff0000>[新手引导]:参数异常 [1#界面名称#控件路径#控件类型] 当前值=[{0}] Id = {1}</color>", link, guideItem.id);
                ResetGuideStep();
                return;
            }

            string action = string.Empty;
            if (guideItem.GuildType == 1)
                action = GuidActionFinger;
            int gameModelId = 0;
            if (tokens.Length == 5)
                int.TryParse(tokens[4], out gameModelId);

            if(guideItem.TriggerType == -2)
            {
                Create(tokens[1], string.Format(tokens[2], argvs[0]) , action, guideItem, tokens[3], gameModelId);
            }
            else
            {
                Create(tokens[1], tokens[2], action, guideItem, tokens[3], gameModelId);
            }
        }
    }

    public void Create(string name,string path,string action, TABLE.GUIDEGROUP guideItem,string ctrl, int gameModelId)
    {
        if(null == guideItem)
        {
            CurrentGuideId = -1;
            ResetGuideStep();
            return;
        }

        if (null == guideItem || !mString2CtrlType.ContainsKey(ctrl))
        {
            CurrentGuideId = -1;
            ResetGuideStep();
            return;
        }

        var transform = GetTransform(name, path);
        if (null == transform)
        {
            CurrentGuideId = -1;
            ResetGuideStep();
            return;
        }

        int type = mString2CtrlType[ctrl];
        Setup(transform,type, guideItem, gameModelId);
    }

    void Start(Transform transform, TABLE.GUIDEGROUP guideItem)
    {
        if(IsOtherGroupGuiding(guideItem.id))
        {
            FNDebug.LogFormat("<color=#ff0000>[新手引导]:[{0}]:[延时启动失败] [其他引导:{1}] 正在触发...</color>",guideItem.id,CurrentGuideId);
            return;
        }
        CurrentGuideId = guideItem.id;
        try
        {
            UIGuidePanel panel = UIManager.Instance.GetPanel<UIGuidePanel>();
            if (null != panel)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.ChangeGuidePanelVisible, true);
                panel.Show(transform, guideItem);
            }
            else
            {
                UIManager.Instance.CreatePanel<UIGuidePanel>(f =>
                {
                    (f as UIGuidePanel).Show(transform, guideItem);
                });
            }
        }
        catch (Exception e)
        {
            FNDebug.Log(e); 
            CurrentGuideId = -1;
        }
    }

    public override void Dispose()
    {
        mTriggeredGuides?.Clear();
        mTriggeredGuides = null;
        CurrentGuideId = -1;

        if (null != mScheduleDelayCall)
        {
            Timer.Instance.RemoveSchedule(mScheduleDelayCall);
            mScheduleDelayCall = null;
        }

        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OnTaskGuideNameChanged, OnTaskChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.Task_FinshGuide, OnTaskChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.Task_GoalUpdate, OnTaskChanged);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.ItemListChange, OnItemListChange);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.MainPlayer_LevelChange, OnMainPlayer_LevelChange);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OpenPanel, OnPanelOpen);
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.BubbleGuide, OnBubbleGuideChanged);

        CurrentGuideAction = null;
        mPoolHandle = null;
        mActivedPanels.Clear();
        mActivedPanels = null;
    }
}