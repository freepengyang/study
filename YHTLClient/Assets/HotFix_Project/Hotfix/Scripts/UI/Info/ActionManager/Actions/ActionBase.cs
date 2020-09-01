public abstract class ActionBase : IAction
{
    public abstract int ID { get; }

    public virtual bool Succeed
    {
        get;set;
    }

    public virtual IActionParam ActionParam
    {
        get
        {
            return actionParam;
        }
    }

    protected IActionParam actionParam;
    public virtual void Init(IActionParam argv)
    {
        actionParam = argv;
    }

    public abstract bool IsDone();

    public IAction Next { get; set; }

    public virtual void OnRecycle()
    {
        ActionPoolManager.Instance.Recycle(actionParam);
        actionParam = null;
    }
}