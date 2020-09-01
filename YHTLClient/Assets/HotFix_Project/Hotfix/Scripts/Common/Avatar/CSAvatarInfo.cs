using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单位的基础信息
/// 初始化的时候,注意 ID,Name,HP,MaxHP,MP,MaxMP,Coord,
/// 玩家需要MapID,Line,Exp,Career,Sex,CreateRoleTime
/// 除开玩家,所有单位存在ConfigId--在对应表中的配置ID
/// </summary>
/// 
public class CSAvatarInfo
{
    protected CSMisc.Dot2 mCoord;
    public float mSpeed = 0.0f;
    protected string name;
    protected int hp = 0;
    public long ID = 0;
    public CSAvatarInfo()
    {
        mCoord = new CSMisc.Dot2();
    }

    //局部事件，只监听用当前对象注册的消息
    public ClientHanlderManager EventHandler = new ClientHanlderManager();
   
    public virtual string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    public virtual int Level { get; set; }
    public virtual int Career { get; set; }
    public virtual int Sex { get; set; }
    public virtual int MapID { get; set; }
    public virtual int Line { get; set; }
    public virtual int HP { get; set; }
    public virtual int MaxHP { get; set; }
    public virtual CSMisc.Dot2 Coord
    {
        get { return mCoord; }
        set { mCoord = value; }
    }
    public virtual int DeltaHP { get; set; }
    public virtual int RealHP { get; set; }
    public virtual int Weapon { get; set; }
    public virtual int BodyModel { get; set; }
    public virtual CSBuffInfo BuffInfo { get; set; }
    public virtual int MP { get; set; }
    public virtual int MaxMP { get; set; }
    public virtual long Exp { get; set; }
    public virtual long ConfigId { get; set; }
    public virtual int AvatarType { get; set; }
    public virtual int Quality { get; set; }
    public float Speed
    {
        get { return mSpeed; }
        set { mSpeed = (value); }
    }

    public virtual float PublicCDTime { get; set; }

    public virtual void Release()
    {
        EventHandler.RemoveAll();
    }

}
