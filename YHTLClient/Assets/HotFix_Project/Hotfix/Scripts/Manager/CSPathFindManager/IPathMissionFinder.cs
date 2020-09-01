public interface IPathMissionFinder
{
    void Reset();
    bool SetTarget(CSMissionBase _mission);
    uint TargetSceneID { get; set; }
}