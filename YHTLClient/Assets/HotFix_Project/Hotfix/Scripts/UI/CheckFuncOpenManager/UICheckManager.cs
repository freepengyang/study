using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using systemcontroller;
using TABLE;
using UnityEngine;

public class UICheckManager : CSInfo<UICheckManager>
{
    //注册的按钮
    private Dictionary<int, List<GameObject>> mBtnObjectMap = new Dictionary<int, List<GameObject>>(64);

    //检测类
    private UICheckBase mCheckBase;

    //存储已经检测过的type对应的功能id
    private Map<Type, int> mAlreadyCheckPanel = new Map<Type, int>(32);

    //存储已经检测过的gameModel对应的功能id
    private Map<int, int> mAlreadyCheckModelPanel = new Map<int, int>(32);

    //存储需要显示在主页面的功能
    private ILBetterList<TABLE.FUNCOPEN> mMainOpenFuncList = new ILBetterList<TABLE.FUNCOPEN>(32);

    //后台服务器控制功能是否开启
    private Map<int, FunctionInfo> ServerBackGroundControl = new Map<int, FunctionInfo>(16);

    //Funtion 表中的功能开放状态
    private Map<int, FunctionInfo> FunctionOpenStateMap = new Map<int, FunctionInfo>(64);

    public void Init()
    {
        SetCheckContext(ServerType.GameServer);
        mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, MainPlayerLevelChange);

        Check();
        CheckAllBtnState();

        mClientEvent.SendEvent(CEvent.UICheckManagerInitCheckComplete);
    }

    /// <summary> 
    /// 登录需要检测的功能
    /// </summary>
    private void Check()
    {
        DoCheckMainFuncNotice();

    }

    /// <summary> 检测面板是否可以被开启 </summary>
    public bool DoCheckPanel(Type type)
    {
        int funcOpenId = -1;
        if (mAlreadyCheckPanel != null && mAlreadyCheckPanel.ContainsKey(type))
        {
            funcOpenId = mAlreadyCheckPanel[type];
        }
        else
        {
            int funId = GameModelsTableManager.Instance.GetFuncIdByModleName(type.ToString());
            if (funId > 0)
            {
                funcOpenId = FuncOpenTableManager.Instance.GetIdByModleName(funId);
            }

            if (funcOpenId > 0)
            {
                mAlreadyCheckPanel?.Add(type, funcOpenId);
            }
        }

        if (funcOpenId > 0)
        {
            return DoCheckButtonClick(funcOpenId);
        }

        return true;
    }

    /// <summary> 检测功能点击状态， 有文字提示 </summary>
    public bool DoCheckButtonClick(FunctionType id)
    {
        return DoCheckButtonClick((int)id);
    }

    /// <summary> 检测功能点击状态， 有文字提示 </summary>
    public bool DoCheckButtonClick(int id)
    {
        if (mCheckBase == null) return true;
        if(!GetServerControlOpen(id, true))
        {
            return false;
        }
        if (!DoCheckFunction(id))
        {
            mCheckBase.ShowTips(id);
            return false;
        }

        return true;
    }

    /// <summary> 检测功能是否开启 ,无文字提示</summary>
    public bool DoCheckFunction(FunctionType type)
    {
        return DoCheckFunction((int)type);
    }

    public bool DoCheckFunctionExtend(int type)
    {
        if (type == 0)
        {
            return true;
        }

        return DoCheckFunction(type);
    }

    /// <summary> 检测功能是否开启 ,无文字提示</summary>
    public bool DoCheckFunction(int type)
    {
        bool isFuncOpen = true;
        if(!GetServerControlOpen(type, false))
        {
            return false;
        }

        if (FunctionOpenStateMap.ContainsKey(type))
        {
            if (FunctionOpenStateMap[(type)].state == 0)
                return false;
        }

        if (mCheckBase != null)
            isFuncOpen = mCheckBase.CheckFunction(type);
        return isFuncOpen;
    }

    /// <summary> 检测单个按钮状态并设置 </summary>
    public bool CheckSingleBtnState(int id)
    {
        return SetBtnState(id);
    }

    /// <summary> 检测所有按钮状态 </summary>
    public void CheckAllBtnState()
    {
        for (var it = mBtnObjectMap.GetEnumerator(); it.MoveNext();)
        {
            SetBtnState(it.Current.Key);
        }
    }

    /// <summary> 获取type绑定的对象 </summary>
    public List<GameObject> GetBtnByType(int type)
    {
        if (mBtnObjectMap.ContainsKey(type))
        {
            return mBtnObjectMap[type];
        }

        return null;
    }

    /// <summary> 注册按钮 </summary>
    public void RegBtn(int funcId, GameObject obj)
    {
        if (obj == null) return;
        if (!mBtnObjectMap.ContainsKey(funcId))
        {
            var list = new List<GameObject>();
            if (!list.Contains(obj))
                list.Add(obj);
            mBtnObjectMap.Add(funcId, list);
        }
        else
        {
            var list = mBtnObjectMap[funcId];
            if (!list.Contains(obj))
                list.Add(obj);
        }

        
    }

    /// <summary> 注册按钮并且检测功能状态,如果按钮显示，取消注册 </summary>
    public bool RegBtnAndCheck(FunctionType funcId, GameObject obj)
    {
         return RegBtnAndCheck((int)funcId, obj);
    }

    /// <summary> 注册按钮并且检测功能状态,如果按钮显示，取消注册 </summary>
    public bool RegBtnAndCheck(int funcId, GameObject obj)
    {
        RegBtn(funcId, obj);
        if (CheckSingleBtnState(funcId))
        {
            UnRegBtn(funcId);
            return true;
        }

        return false;
    }

    /// <summary> 取消注册 </summary>
    public void UnRegBtn(int id)
    {
        if (mBtnObjectMap.ContainsKey(id))
        {
            mBtnObjectMap.Remove(id);
        }
    }

    /// <summary> 获取主页面当前显示的功能预告 </summary>
    public FUNCOPEN GetMainOpenRecommendTab()
    {
        if (mMainOpenFuncList == null || mMainOpenFuncList.Count == 0)
            return null;
        return mMainOpenFuncList[0];
    }

    /// <summary> 检测主页面任务旁边得功能预告 </summary>
    private void DoCheckMainFuncNotice(bool isUpLevel = false)
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        int roleLevel = CSMainPlayerInfo.Instance.Level;
        if (mMainOpenFuncList == null || mMainOpenFuncList.Count == 0) InitShowMainFunc();
        TABLE.FUNCOPEN funcopen = null;
        if (mMainOpenFuncList != null && mMainOpenFuncList.Count > 0)
        {
            mMainOpenFuncList.Sort((f1, f2) => { return f1.needLevel - f2.needLevel; });
            if (isUpLevel)
            {
                for (var i = 0; i < mMainOpenFuncList.Count; i++)
                {
                    if (mMainOpenFuncList[i].needLevel <= CSMainPlayerInfo.Instance.Level)
                    {
                        mMainOpenFuncList.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (mMainOpenFuncList.Count > 0)
                funcopen = mMainOpenFuncList[0];
        }

        EventData data = CSEventObjectManager.Instance.SetValue(isUpLevel, funcopen);
        mClientEvent.SendEvent(CEvent.OpenFuncRecommond, data);
        CSEventObjectManager.Instance.Recycle(data);
    }

    /// <summary>
    /// 初始化后台控制的功能
    /// </summary>
    /// <param name="list"></param>
    public void InitServerControl(RepeatedField<FunctionInfo> list)
    {
        if (list == null || list.Count == 0) return;
        for (var i = 0; i < list.Count; i++)
        {
            if (ServerBackGroundControl.ContainsKey(list[i].functionId))
                ServerBackGroundControl[list[i].functionId] = list[i];
            else
                ServerBackGroundControl.Add(list[i].functionId, list[i]);
        }
    }

    /// <summary>
    /// 判断后台功能是否开放
    /// </summary>
    public bool GetServerControlOpen(int id, bool isShowTips = true)
    {
        bool isOpen = true;
        if (ServerBackGroundControl.ContainsKey(id))
        {
            isOpen = ServerBackGroundControl[id].state == 1;
        }

        if (!isOpen && isShowTips) UtilityTips.ShowRedTips(582);
        return isOpen;
    }

    /// <summary>
    /// 更改功能开放状态
    /// </summary>
    /// <param name="list"></param>
    public void FunctionOpenState(RepeatedField<FunctionInfo> list)
    {
        if (list == null || list.Count == 0) return;
        for (var i = 0; i < list.Count; i++)
        {
            FunctionInfo info = list[i];
            //Debug.Log(info.functionId + " 收到的功能开放id   " + info.state);

            if (FunctionOpenStateMap.ContainsKey(info.functionId))
            {
                if (FunctionOpenStateMap[info.functionId].state != info.state)
                {
                    FunctionOpenStateMap[info.functionId] = info;
                    CheckSingleBtnState(info.functionId);
                    mClientEvent.SendEvent(CEvent.FunctionOpenStateChange, info.functionId);
                }                    
            }
            else
                FunctionOpenStateMap.Add(info.functionId, info);
        }

        DoCheckMainFuncNotice();
    }

    /// <summary>
    /// 更改功能开放状态
    /// </summary>
    public void FunctionOpenState(FunctionType funcId, bool state)
    {
        int id = (int)funcId;
        int stateId = state ? 1 : 0;
        if (FunctionOpenStateMap.ContainsKey(id))
        {
            if (FunctionOpenStateMap[id].state != stateId)
                mClientEvent.SendEvent(CEvent.FunctionOpenStateChange, id);
            FunctionOpenStateMap[id].state = stateId;
            CheckSingleBtnState(id);
        }
    }

    #region Private

    /// <summary> 设置按钮状态 </summary>
    private bool SetBtnState(int functionId)
    {
        if (mCheckBase == null) return false;
        bool isShow = false;
        List<GameObject> go = GetBtnByType(functionId);
        if (go != null && mCheckBase != null)
        {
            bool isConfigHide = mCheckBase.IsConfigHideBtn(functionId);
            isShow = !isConfigHide || DoCheckFunction(functionId);
            if (IOSPlatformCheckIcon((FunctionType)functionId))
            {
                isShow = isShow && QuDaoConstant.OpenRecharge;
            }

            for (var it = go.GetEnumerator(); it.MoveNext();)
            {
                mCheckBase.SetBtnIsShow(functionId, it.Current, isShow); 
            }
            
            
        }

        return isShow;
    }

    /// <summary> ios平台在过审时，需要屏蔽部分功能 </summary>
    private bool IOSPlatformCheckIcon(FunctionType id)
    {
        if (Platform.IsIOS)
        {
            if (true /*id == FunctionType*/)
                return true;
        }

        return false;
    }

    private void InitShowMainFunc()
    {
        if (mMainOpenFuncList == null) mMainOpenFuncList = new ILBetterList<FUNCOPEN>();
        mMainOpenFuncList.Clear();
        TABLE.FUNCOPEN fcunItem = null;
        var arr = FuncOpenTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            fcunItem = arr[k].Value as TABLE.FUNCOPEN;
            if (fcunItem != null && fcunItem.herald > 0 && fcunItem.needLevel > CSMainPlayerInfo.Instance.Level)
            {
                mMainOpenFuncList.Add(fcunItem);
            }
        }
    }

    #endregion


    #region 获取检测类型

    private void SetCheckContext(ServerType type)
    {
        if (CSScene.IsLanuchMainPlayer)
        {
            switch (type)
            {
                case ServerType.GameServer:
                    mCheckBase = new UICheckInGame();
                    break;
            }
        }
    }

    #endregion

    private void MainPlayerLevelChange(uint id, object data)
    {
        DoCheckMainFuncNotice(true);
    }

    public override void Dispose()
    {
        mClientEvent?.UnRegAll();
        mBtnObjectMap.Clear();
        mAlreadyCheckPanel.Clear();
        mMainOpenFuncList.Clear();
        mAlreadyCheckModelPanel.Clear();
        ServerBackGroundControl.Clear();
        FunctionOpenStateMap.Clear();
        mBtnObjectMap = null;
        mAlreadyCheckPanel= null;
        mMainOpenFuncList = null;
        mAlreadyCheckModelPanel = null;
        ServerBackGroundControl = null;
        FunctionOpenStateMap = null;
    }
}