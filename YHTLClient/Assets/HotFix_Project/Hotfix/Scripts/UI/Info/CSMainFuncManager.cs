using System;
using System.Collections.Generic;

/// <summary>
/// 右侧任务面板，在活动或者副本时，需要显示的面板
/// </summary>
public enum MainFuncActType
{
    None,
    MissionPanel,//任务面板
}

/// <summary>
/// 按钮显示的文本
/// </summary>
public enum MainFucShowName
{
    Mission, //任务
    Activity, //活动
    Instance, //副本
    PlayerInfo,//玩家信息
    MonsterInfo,//怪物信息
    Team,//组队信息 
}

public class CSMainFuncManager : CSInfo<CSMainFuncManager>
{
    private UIBase _mCurShowActivity;
    private UIMainFuncPanel _mainFuncPanel;

    public const int MT_MODE = 1;//mission & team
    public const int PM_MODE = 2;//player & monster
    private int _mode = 1;//初始化状态
    public int Mode
    {
        get
        {
            return _mode;
        }
    }

    public void Init(int mode = -1,bool changeTabByMap = false)
    {
        if (mode > 0)
            _mode = mode;

        if (_mainFuncPanel == null)
        {
            UIManager.Instance.CreatePanel<UIMainFuncPanel>(f =>
            {
                _mainFuncPanel = f as UIMainFuncPanel;
                HotManager.Instance.EventHandler.SendEvent(CEvent.MainFuncModeChanged, changeTabByMap);
            });
        }
        else
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.MainFuncModeChanged, changeTabByMap);
        }
    }
    
    public void ShowFuncPanel<T>(MainFucShowName showName) where T : UIBase
    {
        ShowFuncPanel(typeof(T), showName);
    }

    public void ShowFuncPanel(Type type, MainFucShowName showName)
    {
        if (_mCurShowActivity != null)
        {
            if(_mCurShowActivity.GetType() == type) return;
            UIManager.Instance.ClosePanel(_mCurShowActivity.GetType());
        }
        UIManager.Instance.CreatePanel(type, f =>
        {
            _mainFuncPanel?.InitPanel(f.UIPrefabTrans, showName);
            _mCurShowActivity = f;
        });
    }
    
    public void ShowFuncPanel(Type type)
    {
        ShowFuncPanel(type, MainFucShowName.Instance);
    }

    public void CloseFucPanel<T>() where T : UIBase
    {
        if (_mCurShowActivity.GetType() == typeof(T))
        {
            UIManager.Instance.ClosePanel<T>();
            _mCurShowActivity = null;
        }
    }

    int sundryId = 1083;
    HashSet<int> showPlayerDic;
    public bool ShowPlayerInfo(int mapId)
    {
        if(null == showPlayerDic)
        {
            TABLE.SUNDRY sundryItem = null;
            if(!SundryTableManager.Instance.TryGetValue(sundryId,out sundryItem))
            {
                return false;
            }
            var tokens = sundryItem.effect.Split('#');
            showPlayerDic = new HashSet<int>();
            for(int i = 0,max = tokens.Length;i<max;++i)
            {
                int id = 0;
                if (int.TryParse(tokens[i], out id))
                    showPlayerDic.Add(id);
            }
        }
        return showPlayerDic.Contains(mapId);
    }
    public void InitDownPanel()
    {
        int mapId = CSMainPlayerInfo.Instance.MapID;
        if (Mode == MT_MODE)
        {
            int type = InstanceTableManager.Instance.GetInstanceType(mapId);
            if(type == (int)ECopyType.WorldBoss)
            {
                CloseFucPanel<UIMainTeamPanel>();
                CopyShowPanel(mapId);
                mClientEvent.SendEvent(CEvent.MainFuncShowRanking, true);
            }
            else
            {
                ShowFuncPanel<UIMainTeamPanel>(MainFucShowName.Team);
            }
        }
        else
        {
            ShowFuncPanel<UIMonsterInfoPanel>(MainFucShowName.MonsterInfo);
        }
    }

    public void InitUpPanel()
    {
        if(Mode == MT_MODE)
        {
            int mapId = CSMainPlayerInfo.Instance.MapID;
            //特殊条件判断，优先于地图
            if (CSGuildFightManager.Instance.IsActivityOpened && CSGuildFightManager.Instance.IsInActivityMap)
            {
                ShowFuncPanel<UIGuildFightStatusPanel>(MainFucShowName.Activity);
            }
            //勇者之地，王者之地
            else if (mapId == 321101 || mapId == 321111)
                ShowFuncPanel(typeof(UIMapInstancePanel));
            else
            {
                if (MapInfoTableManager.Instance.GetMapInfoType(mapId) == 0)
                {
                    ShowFuncPanel<UIMissionPanel>(MainFucShowName.Mission);
                }
                else
                {
                    CopyShowPanel(mapId);
                    mClientEvent.SendEvent(CEvent.MainFuncShowRanking, false);
                    OnceEventTrigger.Instance.Register(OnceEvent.AutoFightTrigger, () =>
                     {
                         CSInstanceInfo.Instance.DetectInstanceState(mapId, (int)EInstanceState.Started);
                     });
                }
            }
        }
        else
        {
            ShowFuncPanel<UIPlayerInfoPanel>(MainFucShowName.PlayerInfo);
        }
    }

    /// <summary>
    /// 根据副本地图显示副本面板
    /// </summary>
    /// <param name="mapId"></param>
    private void CopyShowPanel(int mapId)
    {
        //Debug.Log("mapid" + mapId);
        int type = InstanceTableManager.Instance.GetInstanceType(mapId);
        switch ((ECopyType)type)
        {
            case ECopyType.SelfBoss:
                ShowFuncPanel(typeof(UIDungeonInstacePanel));
                break;
            case ECopyType.SpringPaoDian:
                ShowFuncPanel(typeof(UISpringPaoDianInstacePanel));
                break;
            case ECopyType.WorldBoss:
                ShowFuncPanel(typeof(UIWorldBossInstancePanel));
                break;
            case ECopyType.UltimateChallenge:
                ShowFuncPanel(typeof(UIUltimateInstacePanel));
                break;
            case ECopyType.Dreamland:
                ShowFuncPanel(typeof(UIDreamInstancePanel));
                break;
            case ECopyType.Dungeon:
                ShowFuncPanel(typeof(UIDungeonSiegeInstancePanel));
                break;
            case ECopyType.UndergroundTreasure:
                ShowFuncPanel(typeof(UIUndergroundTreasureInstancePanel));
                break;
			case ECopyType.RandomThing:
				ShowFuncPanel(typeof(UIRandomThingInstancePanel));
				break;
			case ECopyType.DailyMap:
				ShowFuncPanel(typeof(UIMapInstancePanel));
				break;
			case ECopyType.HonorBossChanllenge:
                ShowFuncPanel(typeof(UIDungeonInstacePanel));
                break;
            case ECopyType.GuildCombat:
                ShowFuncPanel(typeof(UIGuildRankInstancePanel));
                break;
            case ECopyType.GuildBoss:
                ShowFuncPanel(typeof(UIGuildBossInstancePanel));
                break;
            //如果没有特殊的 ，仍然打开任务面板
            default:
                ShowFuncPanel<UIMissionPanel>(MainFucShowName.Mission);
                break;
        }
    }

    public override void Dispose()
    {
        _mCurShowActivity = null;

        _mainFuncPanel = null;
    }
}