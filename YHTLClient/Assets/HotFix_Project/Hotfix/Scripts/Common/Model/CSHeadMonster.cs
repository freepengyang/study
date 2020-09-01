using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHeadMonster : CSHead
{
    private int mShowName;
    public override void Init(CSAvatar avatar,bool isVisible = false, bool isHideModel = false, bool isHideAllName = false)
    {
        CSMonster monster = avatar as CSMonster;
        if (monster != null && monster.tblMonsterInfo != null)
        {
            mShowName = monster.tblMonsterInfo.showName;
            if (mShowName > 0)
            {
                isVisible = true;
                isHideAllName = false;
            }
        }
        base.Init(avatar, isVisible, isHideModel,isHideAllName);
    }

    public override void Show()
    {
        if(mShowName > 0)
        {
            isHideAllName = false;
        }
        base.Show();
    }

    public override void SetHeadPosition()
    {
        CSMonster monster = mAvatar as CSMonster;
        if (monster != null && monster.tblMonsterInfo != null)
        {
            transform.localPosition = new Vector3(0, monster.tblMonsterInfo.headHeight, -100000);
        }
    }

    public override void SetNameColor()
    {
        if(mAvatar == null || lb_actorName == null)
        {
            return;
        }
        Color color = Color.white;
        CSMonster monster = mAvatar as CSMonster;
        if (monster != null && monster.tblMonsterInfo != null)
        {
            if (!UtilityColor.MonsterNameColorDic.TryGetValue(monster.tblMonsterInfo.quality, out color))
            {
                color = Color.white;
            }
        }
        lb_actorName.color = color;
    }

    protected override void SetName()
    {
        if (CSStartLoadAssembly.Sington.IsMonsterHeadShowConfigID)
        {
            CSMonster monster = mAvatar as CSMonster;
            if (monster != null && monster.tblMonsterInfo != null)
            {
                if(lb_actorName != null)
                {
                    //lb_actorName.text = string.Format("{0}__{1}__{2}", mAvatar.GetName(), monster.tblMonsterInfo.id,monster.ID);
                    lb_actorName.text = string.Format("{0}__{1}__{2}_{3}", mAvatar.GetName(), monster.tblMonsterInfo.id, monster?.Info.Level, monster?.BaseInfo.MaxHP);
                }
            }
        }
        else
        {
            base.SetName();
        }
    }
    public override void SetActive(bool value)
    {
        if(gameObject.activeSelf != value)
        {
            gameObject.SetActive(value);
        }
    }
}
       

   


