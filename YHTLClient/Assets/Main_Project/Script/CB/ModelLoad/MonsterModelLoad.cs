using System;
using System.Collections;
using Random = UnityEngine.Random;

public class MonsterModelLoad : ModelLoadBase, IModelLoad
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
        base.Load(this.BodyId, curMotion, curDirection, ModelStructure.Body, ResourceType.MonsterAtlas, ResourceAssistType.Monster);
    }

    public override void OnFinishAllCallBack(ModelLoadBase b)
    {
        base.OnFinishAllCallBack(b);
    }
}