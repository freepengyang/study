using System.Collections;
public class MoneyData
{
    public FastArrayMeta<int> moneyIds = new FastArrayMeta<int>(4);
    public MoneyData Clear()
    {
        moneyIds.Clear();
        return this;
    }
}

public class CSMoneyInfo : CSInfo<CSMoneyInfo>
{
    Stack mActived = new Stack(4);
    Stack mPool = new Stack(4);

    protected MoneyData Get()
    {
        return mPool.Count > 0 ? (mPool.Pop() as MoneyData).Clear() : new MoneyData();
    }

    public void Push(int a)
    {
        var data = Get();
        mActived.Push(data);
        data.moneyIds.Add(a);
        TryCreateMoneyPanel(data);
    }

    public void Push(int a, int b)
    {
        var data = Get();
        mActived.Push(data);
        data.moneyIds.Add(a);
        data.moneyIds.Add(b);
        TryCreateMoneyPanel(data);
    }

    public void Push(int a, int b, int c)
    {
        var data = Get();
        mActived.Push(data);
        data.moneyIds.Add(a);
        data.moneyIds.Add(b);
        data.moneyIds.Add(c);
        TryCreateMoneyPanel(data);
    }

    public void Push(int a, int b, int c,int d)
    {
        var data = Get();
        mActived.Push(data);
        data.moneyIds.Add(a);
        data.moneyIds.Add(b);
        data.moneyIds.Add(c);
        data.moneyIds.Add(d);
        TryCreateMoneyPanel(data);
    }

    public void Pop()
    {
        if(mActived.Count == 0)
        {
            FNDebug.LogError("stack could not keep balance ....");
            return;
        }

        if(mActived.Count > 0)
        {
            mPool.Push(mActived.Pop());
        }

        if(mActived.Count > 0)
        {
            TryCreateMoneyPanel(mActived.Peek() as MoneyData);
        }
        else
        {
            UIManager.Instance.ClosePanel<UIMoneyPanel>();
        }
    }

    protected void TryCreateMoneyPanel(MoneyData data)
    {
        var moneyPanelHandle = UIManager.Instance.GetPanel<UIMoneyPanel>();
        if(null == moneyPanelHandle)
        {
            UIManager.Instance.CreatePanel<UIMoneyPanel>(f =>
            {
                moneyPanelHandle = f as UIMoneyPanel;
            });
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.OnMoneyStackChanged, data);
    }

    public override void Dispose()
    {
        mActived.Clear();
        mPool.Clear();
    }
}