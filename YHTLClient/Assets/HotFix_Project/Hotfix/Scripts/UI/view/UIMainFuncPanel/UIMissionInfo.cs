using System;
using task;
using UnityEngine;

public class UIMissionInfo : GridContainerBase
{
    private UILabel _lbTile;
    private UILabel _lbContent;

    private UILabel _lbSpecial;

    //private TweenAlpha _ani;
    private UISprite _sprBg;
    private UILabel _lbState;
    private GameObject _choose;
    private GameObject _finish;
    private UISlider _taskSlider;
    private UILabel _taskSliderLab;

    private UISprite _spIcon;
    //private UISprite _aniSpr;

    private GameObject _go_effect;
    private UISpriteAnimation _effectAnimation;
    private UISprite _effectSprite;

    private GameObject _go_effect_mission_can_accept_or_submit;//有任务可交或可接时

    private bool isInit;
    private Vector3 titleTrans;

    private CSMissionBase _missioninfo;

    /// <summary>
    /// 点击事件，用于处理选中效果消失和隐藏
    /// </summary>
    public Action<UIMissionInfo> OnClick { get; set; }

    EventHanlderManager mClient = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    public int height { get; set; }

    private void InitComponent()
    {
        isInit = true;
        _lbTile = Get<UILabel>("lb_title");
        _lbContent = Get<UILabel>("lb_content");
        _lbSpecial = Get<UILabel>("lb_special");
        _sprBg = Get<UISprite>("bg");
        _lbState = Get<UILabel>("lb_state");
        _choose = Get("choose").gameObject;
        _finish = Get("finsh").gameObject;
        _taskSlider = Get<UISlider>("task_slider");
        _taskSliderLab = Get<UILabel>("task_slider/Label");
        _spIcon = Get<UISprite>("sp_icon");
        _go_effect = Get("bg/go_effect").gameObject;
        _effectAnimation = _go_effect.GetComponent<UISpriteAnimation>();
        _effectSprite = _go_effect.GetComponent<UISprite>();
        _effectAnimation.enabled = false;
        _effectSprite.enabled = false;
        _go_effect_mission_can_accept_or_submit = Get("bg/go_effect_accept_or_submit").gameObject;

        UIEventListener.Get(_sprBg.gameObject).onClick = OnMissionClick;

        HotManager.Instance.EventHandler.AddEvent(CEvent.OpenMissionGuidePanel,OnMissionGuilde);
    }

    protected void OnMissionGuilde(uint id,object argv)
    {
        if(argv is int taskId && null != _missioninfo && _missioninfo.TaskId == taskId)
        {
            UIManager.Instance.CreatePanel<UIMissionGuildPanel>(f=>
            {
                var panel = f as UIMissionGuildPanel;
                panel.Show(_sprBg);
            });
        }
    }

    public void Show(CSMissionBase info)
    {
        if (info == null) return;
        if (!isInit) InitComponent();
        if (info.TasksTab.taskType == 1)
        {
            bool needResidentEffect = CSMainPlayerInfo.Instance.Level < 50;
            if (needResidentEffect)
            {
                if(!_effectAnimation.enabled)
                {
                    _effectAnimation.enabled = true;
                    _effectSprite.enabled = true;
                    CSEffectPlayMgr.Instance.ShowUIEffect(_go_effect, 17905);
                }
            }
            else
            {
                if(_effectAnimation.enabled && !needResidentEffect)
                {
                    _effectAnimation.enabled = false;
                    _effectSprite.enabled = false;
                }
            }
            gameObject.name = "main";
        }
        else
        {
            if (_effectAnimation.enabled)
            {
                _effectAnimation.enabled = false;
                _effectSprite.enabled = false;
            }
            gameObject.name = info.TaskId.ToString();
        }

        Reset();
        _missioninfo = info;
        ShowAcceptOrSubmitEffect();
        ShowTaskType();
        ShowTitle();
        ShowState();
        ShowContent();
        ShowSpecial();
        CalculateHeight();
        ResetChoose();
    }

    void ShowAcceptOrSubmitEffect()
    {
        if (null != _missioninfo && (/*_missioninfo.TaskState == TaskState.Acceptable || */_missioninfo.TaskState == TaskState.Completed))
        {
            _go_effect_mission_can_accept_or_submit.PlayEffect(17906);
        }
        else
        {
            _go_effect_mission_can_accept_or_submit.StopEffect();
        }
    }

    //状态变更刷新
    public void Refresh(CSMissionBase info)
    {
        _missioninfo = info;

        ShowAcceptOrSubmitEffect();
        ShowState();
        ShowContent();
        ShowSpecial();
        CalculateHeight();
        ResetChoose();
    }
    
    public bool IsTask(int taskId)
    {
        if (_missioninfo != null && _missioninfo.TaskId == taskId) return true;
        return false;
    }

    static int dailyTriggerInterval = -1;
    static int DailyTriggerInterval
    {
        get
        {
            if(-1 == dailyTriggerInterval)
            {
                int v = 5;
                int.TryParse(SundryTableManager.Instance.GetSundryEffect(710, string.Empty), out v);
                dailyTriggerInterval = v;
            }
            return dailyTriggerInterval;
        }
    }
    public bool IsDailyTaskAndFinished()
    {
        if (null == _missioninfo)
            return false;

        if ((TaskType)_missioninfo.TasksTab.taskType != TaskType.Daily)
            return false;

        //Debug.LogFormat("[日常任务引导]:[{0}]", _missioninfo.TaskId);
        if(_missioninfo.TaskState != TaskState.Completed)
        {
            return false;
        }

        if (_missioninfo.lastGuideTime < 0.0f)
        {
            _missioninfo.lastGuideTime = Time.realtimeSinceStartup;
            return false;
        }

        var delta = Time.realtimeSinceStartup - _missioninfo.lastGuideTime;
        if (delta < DailyTriggerInterval)
            return false;

        int guideId = 100001000;
        if (CSGuideManager.Instance.IsOtherGroupGuiding(guideId))
        {
            return false;
        }

        _missioninfo.lastGuideTime = Time.realtimeSinceStartup;
        CSGuideManager.Instance.ResidentDailyTrigger(guideId,_missioninfo.TaskId);
        return true;
    }

    private void ShowTaskType()
    {
        switch ((TaskType) _missioninfo.TasksTab.taskType)
        {
            case TaskType.MainLine:
                _spIcon.spriteName = "mission1";
                break;
            case TaskType.Daily:
                _spIcon.spriteName = "mission2";
                break;
            case TaskType.BranchLine:
                _spIcon.spriteName = "mission3";
                break;
			case TaskType.TodayCanDo:
				_spIcon.spriteName = "mission3";
				break;
			case TaskType.GetEquip:
				_spIcon.spriteName = "mission3";
				break;
			case TaskType.WantStronger:
				_spIcon.spriteName = "mission3";
				break;
			case TaskType.WantIngot:
				_spIcon.spriteName = "mission3";
				break;
		}
	}

    private void ShowTitle()
    {
        _lbTile.text = _missioninfo.ShowTitle();
    }

    private void ShowState()
    {
        if (_missioninfo.TaskState == TaskState.Completed)
        {
            _lbState.text = "";
            _finish.SetActive(true);
        }
        else
        {
            _lbState.text = _missioninfo.ShowState();
            _finish.SetActive(false);
        }
    }

    private void ShowContent()
    {
        _lbContent.text = _missioninfo.ShowContent();
    }

    private void ShowSpecial()
    {
        _lbSpecial.text = _missioninfo.ShowSpecial();
    }

    private void CalculateHeight()
    {
        height = 0;
        if (!string.IsNullOrEmpty(_lbTile.text))
        {
            height += _lbTile.height + 10;
        }

        titleTrans = _lbTile.transform.localPosition;
        _lbContent.transform.localPosition = titleTrans - Vector3.up * height;
        height += _lbContent.height + 10;

        if (!string.IsNullOrEmpty(_lbSpecial.text))
        {
            _lbSpecial.transform.localPosition = titleTrans - Vector3.up * height;
            height += _lbSpecial.height + 10;
        }

        _sprBg.height = height;
        _choose.GetComponent<UISprite>().height = height;
        _sprBg.ResizeCollider();
        _go_effect_mission_can_accept_or_submit.GetComponent<UISprite>().UpdateAnchors();
        _go_effect.GetComponent<UISprite>().UpdateAnchors();
        _spIcon.UpdateAnchors();
    }

    public void ChooseObj()
    {
        _choose.SetActive(true);
    }

    public void ResetChoose()
    {
        _choose.SetActive(false);
    }

    private void OnMissionClick(GameObject go)
    {
        if (_missioninfo == null) return;
        _missioninfo.OnClick(true);
        CSMissionManager.Instance.CurSelectMission = _missioninfo;
        if (OnClick != null)
            OnClick(this);
    }

    private void Reset()
    {
        _missioninfo = null;
        _lbTile.text = "";
        _lbContent.text = "";
        _lbState.text = "";
        _taskSliderLab.text = "";
        _finish.SetActive(false);
        _choose.SetActive(false);
    }

    public override void Dispose()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OpenMissionGuidePanel, OnMissionGuilde);
        _go_effect_mission_can_accept_or_submit.RecycleEffect();
        _go_effect_mission_can_accept_or_submit = null;
        _go_effect.RecycleEffect();
        _go_effect = null;
        isInit = true;
        _go_effect = null;
        _effectSprite = null;
        _effectAnimation = null;
        _lbTile = null;
        _lbContent = null;
        _lbSpecial = null;
        _sprBg = null;
        _lbState = null;
        _choose = null;
        _finish = null;
        _taskSlider = null;
        _taskSliderLab = null;
        _spIcon = null;
        isInit = false;
        _missioninfo = null;
        OnClick = null;
    }
}