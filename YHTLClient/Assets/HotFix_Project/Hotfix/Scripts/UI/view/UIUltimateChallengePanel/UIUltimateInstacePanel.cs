using System.Collections.Generic;
using instance;
using ultimate;
using UnityEngine;

public partial class UIUltimateInstacePanel : UIBasePanel
{
    public override UILayerType PanelLayerType => UILayerType.Resident;

    public override bool ShowGaussianBlur => false;

    private InstanceInfo _InstanceInfo;
    TABLE.INSTANCE instance;
    private List<int> conditionList;
    private bool isShowEffect;
    private RoleUltimateData _UltimateData;

    public override void Init()
    {
        base.Init();
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        //if (_InstanceInfo == null)
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, GetEnterInstanceInfo);

        mClientEvent.AddEvent(CEvent.ResInstanceInfo, ResInstanceInfo);
        mClientEvent.AddEvent(CEvent.UltimateData, UpdateUltimateData);

        mbtn_addattr.onClick = OnOpenAddAttrPanel;

        Net.CSUltimateInfoMessage();
    }

    public override void Show()
    {
        base.Show();
        isShowEffect = false;
        if (_InstanceInfo != null)
            Refresh();
    }

    private void Refresh()
    {
        if (!InstanceTableManager.Instance.TryGetValue(_InstanceInfo.instanceId, out instance)) return;
        RefreshMissionTarget();
    }

    private void UpdateUltimateData(uint id, object data)
    {
        _UltimateData = CSUltimateInfo.Instance.UltimateData;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _UltimateData = CSUltimateInfo.Instance.UltimateData;
        mlb_level.text = CSString.Format(570, _UltimateData.challengeLv);
        if (LevelMaoXianTableManager.Instance.TryGetValue(_UltimateData.challengeLv, out TABLE.LEVELMAOXIAN maoxianTab))
        {
            mslider.value = _UltimateData.challengeExp * 1.0f / maoxianTab.upgrade;
            mlabel.text = $"{_UltimateData.challengeExp}/{maoxianTab.upgrade}";
        }
    }

    private void RefreshMissionTarget()
    {
        if (_InstanceInfo == null || instance == null) return;
        if (instance != null && !string.IsNullOrEmpty(instance.mapName))
        {
            mlb_title.text = instance.mapName;
        }
        if (string.IsNullOrEmpty(instance.conditionSuccess) || instance.conditionSuccess == "0") return;
        conditionList = UtilityMainMath.SplitStringToIntList(instance.conditionSuccess);
        if (conditionList.Count < 2) return;

        switch (conditionList[0])
        {
            case 1:

                if (MonsterInfoTableManager.Instance.TryGetValue(conditionList[1], out TABLE.MONSTERINFO monsterinfo))
                {
                    if (_InstanceInfo.success)
                    {
                        mlb_killMonster.text =
                            $"{UtilityColor.GetColorStr(monsterinfo.quality)}{monsterinfo.name}  {UtilityColor.Green}1/1";
                    }
                    else
                    {
                        mlb_killMonster.text =
                            $"{UtilityColor.GetColorStr(monsterinfo.quality)}{monsterinfo.name}  {UtilityColor.Red}0/1";
                    }
                }

                break;
            case 2:
                mlb_killMonster.text =  ClientTipsTableManager.Instance.GetClientTipsContext(1620) ;
                break;
            case 6:
                CSStringBuilder.Clear();
                CSStringBuilder.Append(_InstanceInfo.killedMonsters + _InstanceInfo.killedBoss, "/", conditionList[1]);
                mlb_killMonster.text = CSStringBuilder.ToString();
                mlb_killMonster.text = _InstanceInfo.killedMonsters + _InstanceInfo.killedBoss < conditionList[1]
                    ? mlb_killMonster.text.BBCode(ColorType.Red)
                    : mlb_killMonster.text.BBCode(ColorType.Green);
                break;
        }
    }

    private void GetEnterInstanceInfo(uint id, object data)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        isShowEffect = false;
        Refresh();
    }

    private void ResInstanceInfo(uint id, object data)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        RefreshMissionTarget();
        ShowSuccessOrFail();
    }

    private void ShowSuccessOrFail()
    {
        if (_InstanceInfo.state != 3 || isShowEffect) return;

        isShowEffect = true;
        mChallengeResult.SetActive(true);
        if (_InstanceInfo.success)
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mbgtitle, 17058);
        }
        else
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(mbgtitle, 17059);
        }

        ScriptBinder.Invoke(2, () => { mChallengeResult?.SetActive(false); });
    }

    private void OnOpenAddAttrPanel(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIUltimateChallengeGainEffectPanel>();
    }

    protected override void OnDestroy()
    {
        _InstanceInfo = null;
        instance = null;
        conditionList?.Clear();
        conditionList = null;
        isShowEffect = false;
        CSEffectPlayMgr.Instance.Recycle(mbgtitle);
        base.OnDestroy();
    }
}