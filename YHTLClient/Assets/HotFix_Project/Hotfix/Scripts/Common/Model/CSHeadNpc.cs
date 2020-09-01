using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHeadNpc : CSHead
{
    CSSprite _sp_title;
    CSSprite sp_title { get { return _sp_title ?? (_sp_title = UtilityObj.GetOrAdd<CSSprite>(transform, "root/sp_title")); } }

    public override void Init(CSAvatar avatar,bool isVisible = true, bool isHideModel = false, bool isHideAllName = false)
    {
        InitCompoment();
        mAvatar = avatar;
        transform.parent = avatar.ModelModule.Top.transform;
        transform.localScale = Vector3.one;
        if (lb_actorName != null)
        {
            lb_actorName.fontSize = 16;
            lb_actorName.color = CSColor.green;
            lb_actorName.text = avatar.GetName();
        }

        if (sp_title != null)
        {
            CSNpc npc = mAvatar as CSNpc;
            if (npc!= null && npc.tblNpc != null && npc.tblNpc.title != 0)
            {
                sp_title.Atlas = CSGameManager.Instance.GetStaticAtlas("actor_title");
                sp_title.SpriteName = npc.tblNpc.title.ToString();
            }            
        }

        SetHeadPosition();
        Show();
    }

    public override void SetHeadPosition()
    {
        CSNpc npc = mAvatar as CSNpc;
        if (npc != null && npc.tblNpc != null)
        {
            transform.localPosition = new Vector3(0, npc.tblNpc.headHeight, -100000);
        }
        else
        {
            transform.localPosition = new Vector3(0, 120, -100000);
        }
    }
    
    /// <summary>
    /// npc名字始终显示    
    /// </summary>
    /// <param name="active"></param>
    public override void SetHeadActive(bool active)
    {
        SetActorNameActive(true);
    }

}
