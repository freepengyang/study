using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGuildActivityPanel : UIBasePanel
{
    private enum GuildActivity { Combat = 1, Boss}





    public override void Init()
    {
        base.Init();

        RegChildPanel<UIGuildCombatPanel>((int)GuildActivity.Combat, mUIGuildCombatPanel, mtg_fight);
        RegChildPanel<UIGuildBossPanel>((int)GuildActivity.Boss, mUIGuildBossPanel, mtg_boss);

        //mtg_fight.Set(true);
        //UIEventListener.Get(mbtn_fight, (int)GuildActivity.Combat).onClick = BtnClick;
        //UIEventListener.Get(mbtn_boss, (int)GuildActivity.Boss).onClick = BtnClick;
    }

    public override void Show()
    {
        base.Show();

        //OpenChildPanel(1, false);
    }

    protected override void OnDestroy()
    {

        base.OnDestroy();
    }

    //public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    //{
    //    if (type == 1)
    //    {
    //        return base.OpenChildPanel(type, false);
    //    }

    //    return base.OpenChildPanel(type, fromToggle);
    //}


    //void BtnClick(GameObject go)
    //{

    //}

}
