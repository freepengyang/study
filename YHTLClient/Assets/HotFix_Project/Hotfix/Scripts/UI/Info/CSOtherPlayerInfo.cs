using UnityEngine;
using user;

public class CSOtherPlayerInfo :  CSInfo<CSOtherPlayerInfo>
{
    public CSOtherPlayerInfo()
    {
    }

    public override void Dispose()
    {

    }
    
    private OtherPlayerInfo otherPlayerInfo = new OtherPlayerInfo();
    /// <summary>
    /// 当前查看的玩家信息
    /// </summary>
    public OtherPlayerInfo OtherPlayerInfo => otherPlayerInfo;


    #region 网络响应处理
    /// <summary>
    /// 覆盖当前查看玩家的信息
    /// </summary>
    public void HandleSetOtherPlayerInfo(OtherPlayerInfo msg)
    {
        if (msg == null) return;
        otherPlayerInfo = msg;
    }
    
    #endregion
}
