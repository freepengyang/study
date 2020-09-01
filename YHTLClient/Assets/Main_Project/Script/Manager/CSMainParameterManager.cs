public class CSMainParameterManager
{
    //地图数据信息
    public static TableMapInfo tableMapInfo = new TableMapInfo();
    
    public static CSCell mainPlayerOldCell;

    //CSScene
    public static bool StartEnterSceneComplete;

    //UILoading
    public static bool LoadingComplete;
}

public class TableMapInfo
{
    public int id;
    public string mapSize;
    public int img;
    public void SetMapInfo(int id, string mapSize, int img)
    {
        this.id = id;
        this.mapSize = mapSize;
        this.img = img;
    }
}