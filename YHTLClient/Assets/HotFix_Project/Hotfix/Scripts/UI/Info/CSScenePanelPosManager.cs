public class CSScenePanelPosManager : CSInfo<CSScenePanelPosManager>
{
    private CSBetterList<string> needMoveList;

    public CSScenePanelPosManager()
    {
        needMoveList = new CSBetterList<string>();
    }
    
    public void AddPanel(string panel)
    {
        if(needMoveList == null) return;
        if (!needMoveList.Contains(panel))
            needMoveList.Add(panel);

        mClientEvent.SendEvent(CEvent.MoveUIMainScenePanel, needMoveList.Count > 0);
    }

    public void RemovePanel(string panel)
    {
        if(needMoveList == null) return;
        if (needMoveList.Contains(panel))
            needMoveList.Remove(panel);
        mClientEvent.SendEvent(CEvent.MoveUIMainScenePanel, needMoveList.Count > 0);
    }

    public void RemoveAll()
    {
        if(needMoveList == null) return;
        needMoveList.Clear();
        mClientEvent.SendEvent(CEvent.MoveUIMainScenePanel, false);
    }

    public bool IsHaveMove()
    {
        return needMoveList != null && needMoveList.Count > 0;
    }

    public override void Dispose()
    {
        if (needMoveList != null) needMoveList.Clear();
        needMoveList = null;

    }
}