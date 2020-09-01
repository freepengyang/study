using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIPkModePanel : UIBasePanel
{
    #region variable
    string[] _ModeList;
    public string[] ModeList
    {
        get
        {
            if (_ModeList == null)
            {
                _ModeList = SundryTableManager.Instance.GetSundryEffect(393).Split('#');
            }
            return _ModeList;
        }
    }
    Vector3 unlockPos = Vector3.zero;
    Vector3 lockPos = new Vector3(3, 0, 0);
    #endregion
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.PkModeChangedNtf, GetPKModeChange);
        mClientEvent.Reg((uint)CEvent.PkModeTips, GetPKModeTips);
        UIEventListener.Get(mbtn_showModeList.gameObject).onClick = ShowModeList;
        UIEventListener.Get(mobj_shield).onClick = HideModeList;
        for (int i = 0; i < mgrid_modes.transform.childCount; i++)
        {
            UIEventListener.Get(mgrid_modes.transform.GetChild(i).gameObject, i + 1).onClick = ModeItemClick;
        }
        RefreshModeLabel();
    }
    void GetPKModeChange(uint id, object data)
    {
        RefreshModeLabel();
    }
    void GetPKModeTips(uint id, object data)
    {
        int mode = (CSMainPlayerInfo.Instance.PkModeMap == 0) ? CSMainPlayerInfo.Instance.PkMode : CSMainPlayerInfo.Instance.PkModeMap;
        if (mode != 0)
        {
            UtilityTips.ShowRedTips(687, ModeList[mode - 1]);
        }
    }
    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {

        base.OnDestroy();
    }
    void ShowModeList(GameObject _go)
    {
        int mode = (CSMainPlayerInfo.Instance.PkModeMap == 0) ? CSMainPlayerInfo.Instance.PkMode : CSMainPlayerInfo.Instance.PkModeMap;
        int mapMode = CSMainPlayerInfo.Instance.PkModeMap;
        if (mapMode == mode)
        {
            UtilityTips.ShowRedTips(686);
            return;
        }
        mobj_ModeList.SetActive(true);
        //RefreshModeLabel();
        RefreshModeList();
    }
    void HideModeList(GameObject _go)
    {
        mobj_ModeList.SetActive(false);
    }
    void ModeItemClick(GameObject _go)
    {
        int mode = (int)UIEventListener.Get(_go).parameter;
        Net.ReqSetPkModeMessage(mode);
        mobj_ModeList.SetActive(false);
    }
    void RefreshModeLabel()
    {
        int mode = (CSMainPlayerInfo.Instance.PkModeMap == 0) ? CSMainPlayerInfo.Instance.PkMode : CSMainPlayerInfo.Instance.PkModeMap;
        int mapMode = CSMainPlayerInfo.Instance.PkModeMap;
        mlb_mode.text = ModeList[mode - 1];
        mbtn_showModeList.spriteName = (12100 + mode).ToString();
        if (mapMode == mode)
        {
            mobj_lock.SetActive(true);
            mlb_mode.transform.localPosition = lockPos;
        }
        else
        {
            mobj_lock.SetActive(false);
            mlb_mode.transform.localPosition = unlockPos;
        }
    }
    void RefreshModeList()
    {
        int mode = (CSMainPlayerInfo.Instance.PkModeMap == 0) ? CSMainPlayerInfo.Instance.PkMode : CSMainPlayerInfo.Instance.PkModeMap;
        //地图配0  读服务器模式
        for (int i = 0; i < mgrid_modes.transform.childCount; i++)
        {
            mgrid_modes.transform.GetChild(i).gameObject.SetActive(i == (mode - 1) ? false : true);
        }
        mgrid_modes.Reposition();
    }
}
