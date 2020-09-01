using map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSReliveInfo : CSInfo<CSReliveInfo>
{
    public CSReliveInfo()
    {
        
    }

    public override void Dispose()
    {
      
    }

    /// <summary>
    /// 死亡复活信息
    /// </summary>
    ReliveData myReliveData = null;
    public ReliveData MyReliveData {
        get { return myReliveData; }
    }

    private int mapIdDeath = -1;
    /// <summary>
    /// 上次最近死亡地图
    /// </summary>
    public int MapIdDeath
    {
        get { return mapIdDeath; }
    }


    #region 接收网络消息处理数据函数
    /// <summary>
    /// 获取玩家死亡信息响应处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetPlayerDieMessage(player.PlayerDie msg)
    {
        if (msg == null) return;
        if (myReliveData == null)
            myReliveData = new ReliveData();

        myReliveData.playerDie.killlerType = msg.killlerType;
        myReliveData.playerDie.killerName = msg.killerName;
        myReliveData.playerDie.reliveCount = msg.reliveCount;
        myReliveData.playerDie.dieTime = msg.dieTime;
        mapIdDeath = CSMainPlayerInfo.Instance.MapID;
        UIManager.Instance.CreatePanel<UIRelivePanel>();
        
    }

    /// <summary>
    /// 复活响应处理
    /// </summary>
    /// <param name="msg"></param>
    public void GetReliveMessage(ReliveResponse msg)
    {
        if (msg == null) return;
    }
    #endregion


}

/// <summary>
/// 复活数据类
/// </summary>
public class ReliveData
{
    public player.PlayerDie playerDie = new player.PlayerDie();
    public map.ReliveResponse reliveResponse = new ReliveResponse();
}

