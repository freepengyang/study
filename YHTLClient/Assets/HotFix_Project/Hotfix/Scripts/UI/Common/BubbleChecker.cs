public abstract class BubbleChecker : IBubbleChecker
{
    public abstract int ID { get; }

    public virtual int IconID { get { return ID; } }

    protected EventHanlderManager mClientEvent;

    public void Create(EventHanlderManager eventHandle)
    {
        mClientEvent = eventHandle;
        OnCreate();
    }

    public void Destroy()
    {
        OnDestroy();
    }

    public abstract bool OnCheck();

    protected abstract void OnCreate();
    protected abstract void OnDestroy();
    public abstract void OnClick();
}