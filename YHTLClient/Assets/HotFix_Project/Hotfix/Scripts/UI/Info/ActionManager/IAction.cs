public interface IAction
{
    int ID { get; }
    void Init(IActionParam argv);
    IActionParam ActionParam { get; }
    bool IsDone();
    bool Succeed { get; }
    IAction Next { get; set; }
    void OnRecycle();
}

public interface IActionParam
{

}

public interface IActionParam<A> : IActionParam
{
    void Run(A a);
}

public interface IActionParam<A,B> : IActionParam
{
    void Run(A a, B b);
}

public interface IActionParam<A,B,C> : IActionParam
{
    void Run(A a, B b, C c);
}

public interface IActionParam<A,B,C,D> : IActionParam
{
    void Run(A a, B b, C c, D d);
}

public class ActionNodeParam : IActionParam<IAction,IActionParam>
{
    public void Run(IAction a, IActionParam b)
    {
        action = a;
        param = b;
    }
    public IAction action;
    public IActionParam param;
}

public class ActionFindNpcParam : IActionParam<int>
{
    public int npcId;
    public void Run(int a)
    {
        npcId = a;
    }
}

public class ActionFindMapParam : IActionParam<int>
{
    public int mapId;
    public void Run(int a)
    {
        mapId = a;
    }
}

public class ActionOpenFrameParam : IActionParam<int>
{
    public int gameModelId;
    public void Run(int a)
    {
        gameModelId = a;
    }
}

public class ActionFindPosParam : IActionParam<int,int,int>
{
    public int mapId;
    public int x;
    public int y;
    public void Run(int mapId,int x,int y)
    {
        this.mapId = mapId;
        this.x = x;
        this.y = y;
    }
}

public class ActionQueueParam : IActionParam<IAction[]>
{
    public IAction[] mActionLists;
    public void Run(IAction[] actionLists)
    {
        mActionLists = actionLists;
    }
}