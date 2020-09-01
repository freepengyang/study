public interface IBubbleChecker
{
    int ID { get; }
    void Create(EventHanlderManager eventHandle);
    void Destroy();
    void OnClick();
    bool OnCheck();
}