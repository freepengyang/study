using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcModelLoad : ModelLoadBase, IModelLoad
{
    public override void Analyze()
    {
        base.BeginLoad();
        curMotion = this.Action.Motion;
        curDirection = this.Action.Direction;
        LoadBody();
        base.EndLoad(OnFinishAllCallBack);
    }

    private void LoadBody()
    {
        base.Load(this.BodyId, curMotion, curDirection,ModelStructure.Body, ResourceType.NpcAtlas, ResourceAssistType.NPC);
    }

    public override void OnFinishAllCallBack(ModelLoadBase b)
    {
        base.OnFinishAllCallBack(b);
    }
}
