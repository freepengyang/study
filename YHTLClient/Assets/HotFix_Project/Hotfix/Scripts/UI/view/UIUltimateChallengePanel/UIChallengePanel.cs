using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TABLE;
using ultimate;
using UnityEngine;
using wolong;

public partial class UIChallengePanel : UIBasePanel
{
    public RoleUltimateData _UltimateData;
    private List<TABLE.LEVELMAOXIANNODE> _MaoXianNodeDianList = new List<LEVELMAOXIANNODE>();
    private List<TABLE.LEVELMAOXIANNODE> _MaoXianNodeFragList = new List<LEVELMAOXIANNODE>();

    private long endTime;

    int maxResetCount;

    /// <summary>
    /// 总层数
    /// </summary>
    int totalLevel;

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.UltimateData, UpdateUltimateData);
        mbtn_challenge.onClick = OnChallengeClick;
        mbtn_buffAdd.onClick = OnBuffAddLookClick;
        mbtn_help.onClick = OnHelpClick;
        mbtn_reset.onClick = OnResetClick;
        mbtn_ranking.onClick = OnOpenRankClick;
        UIEventListener.Get(mbloodIcon).onClick = OnHintClick;

        Net.CSUltimateInfoMessage();

        mScrollView.panel.depth -= 1;

        string count = SundryTableManager.Instance.GetSundryEffect(683);
        if (!int.TryParse(count, out maxResetCount)) maxResetCount = 2;

        totalLevel = CSUltimateInfo.Instance.GetTotalLevel();

        ShowDefault();
    }

    public override void Show()
    {
        base.Show();
        ShowBg();
    }

    private void ShowDefault()
    {
        mlbLevelValue.text = "";
        mlbBloodValue.text = "";
        mspBloodValue.fillAmount = 1;
        mresurgenceCount.text = "";
    }

    private void RefreshUI()
    {
        if (_UltimateData == null) return;

        mtimebg.SetActive(true);

        if (_UltimateData.maxHp > 0)
        {
            mlbLevelValue.text = CSString.Format(570, _UltimateData.challengeLv);
            if (LevelMaoXianTableManager.Instance.TryGetValue(_UltimateData.challengeLv, out TABLE.LEVELMAOXIAN maoxianTab))
            {
                mspLevelValue.fillAmount = _UltimateData.challengeExp * 1.0f / maoxianTab.upgrade;
            }
            
            CSStringBuilder.Clear();
            mlbBloodValue.text = CSStringBuilder.Append(_UltimateData.hp, "/", _UltimateData.maxHp).ToString();
            mspBloodValue.fillAmount = _UltimateData.hp * 1.0f / _UltimateData.maxHp;
            mbloodIcon.SetActive(_UltimateData.hp <= _UltimateData.maxHp * 0.1f);
        }
        else
        {
            mlbLevelValue.text = CSString.Format(570, 1);
            mspLevelValue.fillAmount = 0;

            //float hp = CSMainPlayerInfo.Instance.HP * 1.0f;
            float maxHp = CSMainPlayerInfo.Instance.MaxHP * 1.0f;
            mspBloodValue.fillAmount = 1;
            mlbBloodValue.text = $"{maxHp}/{maxHp}";
            mbloodIcon.SetActive(false);
        }

        //重置按钮
        msp_resetBtn.spriteName = _UltimateData.resetCount < maxResetCount ? "btn_nbig1" : "btn_nbig4";
        mlb_resetBtn.color = UtilityColor.HexToColor(_UltimateData.resetCount < maxResetCount ? "#b0bbcf" : "#c0c0c0");
        //挑战按钮
        msp_challengeBtn.spriteName = _UltimateData.maxInstLevel < totalLevel ? "btn_nbig2" : "btn_nbig4";
        mlb_challengeBtn.color = UtilityColor.HexToColor(_UltimateData.maxInstLevel < totalLevel ? "#cfbfb0" : "#c0c0c0");

        if (_UltimateData.maxReliveTimes - _UltimateData.reliveTimes == 0)
            mresurgenceCount.text = CSString.Format(740).BBCode(ColorType.Red);
        else
            mresurgenceCount.text = (_UltimateData.maxReliveTimes - _UltimateData.reliveTimes).ToString()
                .BBCode(ColorType.Green);
        endTime = (long) (_UltimateData.endTime * 0.001f);

        ScriptBinder.StopInvokeRepeating();
        leftTime = endTime - CSServerTime.Instance.TotalSeconds;
        if (leftTime > 0)
        {
            ScriptBinder.InvokeRepeating(0, 1, RefreshTime);
        }
    }

    private void RefreshNode()
    {
        if (_MaoXianNodeDianList == null) _MaoXianNodeDianList = new List<LEVELMAOXIANNODE>();
        else _MaoXianNodeDianList.Clear();
        if (_MaoXianNodeFragList == null) _MaoXianNodeFragList = new List<LEVELMAOXIANNODE>();
        else _MaoXianNodeFragList.Clear();

        var arr = LevelMaoXianNodeTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var levelItem = arr[k].Value as TABLE.LEVELMAOXIANNODE;
            //if (levelItem.id > _UltimateData.maxInstLevel)
            //{
            //    if (levelItem.bigNode == 0)
            //        _MaoXianNodeDianList.Add(levelItem);
            //    else
            //        _MaoXianNodeFragList.Add(levelItem);
            //}
            if (levelItem.bigNode == 0)
                _MaoXianNodeDianList.Add(levelItem);
            else
                _MaoXianNodeFragList.Add(levelItem);
        }

        mgrid_dian.Bind<LEVELMAOXIANNODE, UltimateNode>(_MaoXianNodeDianList, mPoolHandleManager);
        mgrid_frag.Bind<LEVELMAOXIANNODE, UltimateNode>(_MaoXianNodeFragList, mPoolHandleManager);
    }

    private void RefreshBgPos()
    {
        if (!LevelMaoXianNodeTableManager.Instance.TryGetValue(_UltimateData.maxInstLevel,
            out TABLE.LEVELMAOXIANNODE table)) return;

        float minX = mScrollView.panel.width * 5f;
        float maxX = msp_bgMap.width * 10 - minX;
        float minY = mScrollView.panel.height * 5f;
        float maxY = msp_bgMap.height * 10 - minY;
        float x = Mathf.Abs(table.x) > maxX ? 0 : 1;
        float y = Mathf.Abs(table.y) > maxY ? 0 : 1;
        if (x > 0.1f && Mathf.Abs(table.x) > minX)
            x = 1 - (Mathf.Abs(table.x) - minX) / (maxX - minX);
        if (y > 0.1f && Mathf.Abs(table.y) > minY)
            y = 1 - (Mathf.Abs(table.y) - minY) / (maxY - minY);
        mScrollView.SetDragAmount(x, y);
    }

    private void ShowBg()
    {
        CSEffectPlayMgr.Instance.ShowUITexture(msp_bgMap.gameObject, "extremity_challenge_map");
    }

    private long leftTime;

    private void RefreshTime()
    {
        leftTime = endTime - CSServerTime.Instance.TotalSeconds;
        if (leftTime >= 0)
        {
            mlb_time.text = CSServerTime.Instance.FormatLongToTimeStr(endTime - CSServerTime.Instance.TotalSeconds);
        }
        else
        {
            UIManager.Instance.ClosePanel<UIUltimateChallengePanel>();
        }
    }


    #region Event

    private void OnChallengeClick(GameObject go)
    {
        if (_UltimateData == null) return;
        if (_UltimateData.maxInstLevel >= totalLevel)
        {
            UtilityTips.ShowRedTips(1916);
            return;
        }

        if (_UltimateData.maxHp > 0 && _UltimateData.hp == 0)
        {
            if (_UltimateData.maxReliveTimes - _UltimateData.reliveTimes > 0)
            {
                UtilityTips.ShowPromptWordTips(14, () => { EnterInstance(); });
                return;
            }
            else
            {
                UtilityTips.ShowRedTips(CSString.Format(739));
                return;
            }
        }

        EnterInstance();
    }

    private void EnterInstance()
    {
        int instanceid = _UltimateData.curInstId;

        if (instanceid == 0)
            instanceid = InstanceTableManager.Instance.GetInstanceIdByType(5, 1);
        else
        {
            instanceid = _UltimateData.curInstId + 1;
            if (!InstanceTableManager.Instance.array.gItem.id2offset.ContainsKey(instanceid))
            {
                UtilityTips.ShowRedTips(CSString.Format(738));
            }
        }

        Net.ReqEnterInstanceMessage(instanceid);

        UIManager.Instance.ClosePanel<UIUltimateChallengePanel>();
    }

    private void OnBuffAddLookClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIUltimateChallengeGainEffectPanel>();
    }

    private void OnHelpClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Ultimate);
    }

    private void OnResetClick(GameObject go)
    {
        if (_UltimateData != null && _UltimateData.resetCount >= maxResetCount)
        {
            UtilityTips.ShowRedTips(1652);
            return;
        }
        UtilityTips.ShowPromptWordTips(79, ResetComfirm);
    }


    void ResetComfirm()
    {
        Net.CSResetUltimateMessage();
    }


    private void OnOpenRankClick(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIUltimateChallengeRankPanel>();
    }

    void OnHintClick(GameObject go)
    {
        UtilityTips.ShowPromptWordTips(78, null);
    }

    #endregion

    private void UpdateUltimateData(uint id, object data)
    {
        _UltimateData = CSUltimateInfo.Instance.UltimateData;
        RefreshUI();
        RefreshBgPos();
        RefreshNode();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(msp_bgMap.gameObject);
        base.OnDestroy();
    }
}

public class UltimateNode : UIBinder
{
    private LEVELMAOXIANNODE nodeTab;
    private UILabel _level;
    private GameObject effect;
    UISprite sp_icon;

    RoleUltimateData _UltimateData;

    public override void Init(UIEventListener handle)
    {
        _level = Get<UILabel>("lb_level");
        effect = Get<GameObject>("effect");
        sp_icon = handle.GetComponent<UISprite>();
    }

    public override void Bind(object data)
    {
        nodeTab = data as LEVELMAOXIANNODE;
        if (nodeTab == null) return;

        Handle.gameObject.transform.localPosition = new Vector3(nodeTab.x * 0.1f, nodeTab.y * 0.1f);

        _UltimateData = CSUltimateInfo.Instance.UltimateData;
        if (_UltimateData == null) return;

        bool isClosed = nodeTab.id <= _UltimateData.maxInstLevel;

        if (_level != null /*&& nodeTab.bigNode == 1*/)
        {
            _level.text = CSString.Format(CSUltimateInfo.Instance.NodeTabStr, nodeTab.id);
            _level.gameObject.SetActive(nodeTab.id == _UltimateData.maxInstLevel + 1);
        }
            

        if (effect != null)
        {
            if(nodeTab.id == _UltimateData.maxInstLevel + 1)
                CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_battle_fire");
            else
                CSEffectPlayMgr.Instance.Recycle(effect);
        }

        if (sp_icon != null)
        {
            if (nodeTab.bigNode == 1)
            {
                sp_icon.spriteName = isClosed ? "flag2" : "flag1";
            }
            else sp_icon.spriteName = isClosed ? "point2" : "point1";
        }

        if (nodeTab.id == _UltimateData.maxInstLevel + 1)
        {
            Handle.onClick = OnChallengeClick;
        }
        else if(nodeTab.id < _UltimateData.maxInstLevel + 1)
        {
            Handle.onClick = IsClosedClick;
        }
        else
        {
            Handle.onClick = IsLockedClick;
        }
    }

    public override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(effect);
        nodeTab = null;
        _UltimateData = null;
        _level = null;
        effect = null;
        sp_icon = null;
    }


    private void OnChallengeClick(GameObject go)
    {
        if (_UltimateData == null) return;
        if (_UltimateData.maxInstLevel >= CSUltimateInfo.Instance.GetTotalLevel())
        {
            UtilityTips.ShowRedTips(1916);
            return;
        }
        if (_UltimateData.maxHp > 0 && _UltimateData.hp == 0)
        {
            if (_UltimateData.maxReliveTimes - _UltimateData.reliveTimes > 0)
            {
                UtilityTips.ShowPromptWordTips(14, () => { EnterInstance(); });
                return;
            }
            else
            {
                UtilityTips.ShowRedTips(CSString.Format(739));
                return;
            }
        }

        EnterInstance();
    }

    private void EnterInstance()
    {
        int instanceid = _UltimateData.curInstId;

        if (instanceid == 0)
            instanceid = InstanceTableManager.Instance.GetInstanceIdByType(5, 1);
        else
        {
            instanceid = _UltimateData.curInstId + 1;
            if (!InstanceTableManager.Instance.array.gItem.id2offset.ContainsKey(instanceid))
            {
                UtilityTips.ShowRedTips(CSString.Format(738));
            }
        }

        Net.ReqEnterInstanceMessage(instanceid);

        UIManager.Instance.ClosePanel<UIUltimateChallengePanel>();
    }


    void IsLockedClick(GameObject go)
    {
        UtilityTips.ShowRedTips(1646);
    }


    void IsClosedClick(GameObject go)
    {
        UtilityTips.ShowRedTips(1647);
    }

}