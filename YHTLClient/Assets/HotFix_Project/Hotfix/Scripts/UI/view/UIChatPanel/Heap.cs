public interface HeapEntry
{
    int heapIndex { get; set; }
}

public class Heap<T> where T : class, HeapEntry, new()
{
    FastArrayElementKeepHandle<T> heap;
    ElementCompare<T> comparison;

    public Heap(ElementCompare<T> comparison, int capacity = 32)
    {
        this.comparison = comparison;
        heap = new FastArrayElementKeepHandle<T>(capacity);
    }

    public int Count
    {
        get
        {
            return heap.Count;
        }
    }

    public bool Empty()
    {
        return heap.Count <= 0;
    }

    public void Clear()
    {
        heap.Clear();
    }

    /// <summary>
    /// 遍历元素
    /// </summary>
    /// <param name="forEach"></param>
    public void Travel(System.Action<T> forEach)
    {
        for (int i = 0, max = heap.Count; i < max; ++i)
        {
            forEach(heap[i]);
        }
    }

    /// <summary>
    /// 压入指定元素
    /// </summary>
    /// <param name="heapHandle"></param>
    public void Push(T entry)
    {
        if (null != entry)
        {
            entry.heapIndex = heap.Count;
            heap.Append(entry);
            up_heap(entry.heapIndex);
        }
    }

    /// <summary>
    /// 移除指定元素
    /// </summary>
    /// <param name="entry"></param>
    public bool Remove(T entry)
    {
        //头元素用数组未元素替换，然后下沉
        int index = entry.heapIndex;
        int n = heap.Count;
        if (index >= 0 && index < n)
        {
            if (index == n - 1)
            {
                heap.RemoveAt(n - 1);
            }
            else
            {
                swap_heap(index, n - 1);
                heap.RemoveAt(n - 1);

                int parent = (index - 1) >> 1;
                if (index > 0 && comparison(heap[index], heap[parent]) < 0)
                    up_heap(index);
                else
                    down_heap(index);
            }
            return true;
        }
        return false;
    }

    public T Top()
    {
        if (heap.Count > 0)
            return heap[0];
        return null;
    }

    /// <summary>
    /// 弹出堆顶元素
    /// </summary>
    /// <returns></returns>
    public T Pop()
    {
        if (heap.Count > 0)
        {
            T entry = heap[0];
            Remove(entry);
            return entry;
        }
        return null;
    }

    /// <summary>
    /// 下沉
    /// </summary>
    /// <param name="index"></param>
    void down_heap(int index)
    {
        //从上到下，算出左右子节点，和最小的交换
        int lchild = (index << 1) + 1;
        int n = heap.Count;
        while (lchild < n)
        {
            int minChild = (lchild + 1 == n || comparison(heap[lchild], heap[lchild + 1]) < 0) ? lchild : lchild + 1;
            if (comparison(heap[index], heap[minChild]) < 0)
                break;
            swap_heap(index, minChild);
            index = minChild;
            lchild = (index << 1) + 1;
        }
    }

    /// <summary>
    /// 上浮
    /// </summary>
    void up_heap(int index)
    {
        //下至上，和父节点比较。如果小于父节点上浮
        int parent = (index - 1) >> 1;
        while (index > 0 && comparison(heap[index], heap[parent]) < 0)
        {
            swap_heap(index, parent);
            index = parent;
            parent = (index - 1) / 2;
        }
    }

    /// <summary>
    /// //交换两个堆元素
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    void swap_heap(int l, int r)
    {
        heap.Swap(l, r);
        heap[l].heapIndex = l;
        heap[r].heapIndex = r;
    }
}