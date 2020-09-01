using UnityEngine;
using System.Collections;

public class CSModelModule : MonoBehaviour 
{
    public GameObject Top;
    public GameObject Body;       // 身体
    public GameObject Weapon;     // 武器
    public GameObject Wing;       // 翅膀
    public GameObject Effect;     // 特效
    public GameObject Bottom;     // 选中
    public GameObject BottomNPC;  // 选中NPC
    public GameObject Mount;     //坐骑
    public GameObject MountHead;  //马头
    public bool isHasBottom;
    public void init(bool isHasBody, bool isHasWeapon, bool isHasWing, bool isHasEffect,bool isHasMount,bool isHasHead, bool isHasBottom, int avatarType)
    {
        if (Top == null) Top = newGameObject("Top");
        if (isHasBody && Body == null) Body = newGameObject("B");
        if (isHasWeapon && Weapon == null) Weapon = newGameObject("W");
        if (isHasWing && Wing == null) Wing = newGameObject("W");
        if (isHasEffect && Effect == null) Effect = newGameObject("E");
        if (isHasMount && Mount == null) Mount = newGameObject("M");
        if (isHasHead && MountHead == null) MountHead = newGameObject("MH");
        if (Mount != null) Mount.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        if (MountHead != null) MountHead.transform.localPosition = new Vector3(0.0f, 0.0f, -3.0f);
        if (avatarType == EAvatarType.MainPlayer)
        {
            if (Bottom == null) Bottom = newGameObject("B");
            if (BottomNPC == null) BottomNPC = newGameObject("BNPC");
        }
        this.isHasBottom = isHasBottom;
    }

    private GameObject newGameObject(string name) 
    {
        GameObject go = new GameObject(name);
        NGUITools.SetParent(this.transform, go);
        return go;
    }
}
