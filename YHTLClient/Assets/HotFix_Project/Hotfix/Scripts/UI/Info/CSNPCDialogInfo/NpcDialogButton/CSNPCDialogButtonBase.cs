using System;
using TABLE;
using UnityEngine;

public class CSNPCDialogButtonBase
{
    protected CSNPCDialogData _Data;
    protected int _TaskCount = 0;
    protected int _NpcDefaultButtonCount = 0;

    public int Init(CSNPCDialogData data)
    {
        _Data = data;
        return GetButtonCount();
    }
    
    /// <summary>
    /// 获取按钮数量
    /// </summary>
    /// <returns></returns>
    public virtual int GetButtonCount()
    {
        return InitDefault();
    }

    protected int InitDefault()
    {
        if (_Data.CurNPC != null)
        {
            if (!string.IsNullOrEmpty(_Data.CurNPC.openPanel))
            {
                _NpcDefaultButtonCount = _Data.CurNPC.openPanel.Split('#').Length;
            }
        }
        
        return _TaskCount + _NpcDefaultButtonCount;
    }

    public virtual int InitTasks(CSNPCDialogData data)
    {
        _Data = data;
        if (data.Mission != null)
            _TaskCount = data.MissionList.Count;
        return _TaskCount;
    }
    
    /// <summary>
    /// 设置按钮功能
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback">GameObject: 按钮   string：按钮显示文字    Action：点击回调    bool： 点击后是否关闭面板</param>
    /// <param name="index">多个任务时，按钮的顺序, 默认只有一个按钮</param>
    public virtual void SetButton(GameObject go, Action<GameObject, string, System.Action, bool> callback, int index = 0)
    {

    }
}