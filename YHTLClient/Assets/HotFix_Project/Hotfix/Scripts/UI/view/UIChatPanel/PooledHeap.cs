public delegate int ElementCompare<T>(T l, T r);
public class PooledHeap<T> where T : class, HeapEntry, new()
{
    Heap<T> heap;
    ElementPool<T> elementPool;

    public PooledHeap(int capacity,
        ElementCompare<T> comparison,
        System.Action<T> onCreate = null,
        System.Action<T> onGet = null,
        System.Action<T> onPut = null,
        System.Action<T> onDestroy = null) : base()
    {
        heap = new Heap<T>(comparison, capacity);
        elementPool = new ElementPool<T>(capacity, onCreate, onGet, onPut, onDestroy);
    }

    /// <summary>
    /// 从池子中取出一个元素
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        return elementPool.Get();
    }

    public void Push(T element)
    {
        heap.Push(element);
    }

    /// <summary>
    /// 移除元素
    /// </summary>
    /// <param name="element"></param>
    public void Remove(T element)
    {
        heap.Remove(element);
        elementPool.Put(element);
    }

    /// <summary>
    /// 返回堆顶元素
    /// </summary>
    /// <returns></returns>
    public T Top()
    {
        return heap.Top();
    }

    /// <summary>
    /// 弹出堆顶元素
    /// </summary>
    public void Pop()
    {
        T element = heap.Top();
        Remove(element);
    }

    public bool Empty()
    {
        return heap.Empty();
    }
    public int Count
    {
        get
        {
            return heap.Count;
        }
    }
    /// <summary>
    /// 清除元素，不销毁内存，回收池
    /// </summary>
    public void Clear()
    {
        heap.Travel(elementPool.Put);
        heap.Clear();
    }

    /// <summary>
    /// 销毁此对象，不可以再次访问
    /// </summary>
    public void Destroy()
    {
        if (null == heap)
            return;

        heap.Travel(elementPool.Put);
        heap.Clear();
        heap = null;
        elementPool.Destroy();
        elementPool = null;
    }
}