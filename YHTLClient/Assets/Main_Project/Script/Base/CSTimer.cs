using UnityEngine;

namespace FlyBirds.Model
{
    public static class TimerType
    {
        public const int ONCE = 1;
        public const int LOOP = 2;
    }

    /// <summary>
    /// 定时器
    /// </summary>
    public class TimerEventHandle
    {
        static int ms_guid = 0x3b6679;
        public TimerEventHandle()
        {
            guid = ++ms_guid;
        }
        public int guid;
        public int status;
        public int eTimer;
        public int loopTimes;
        public float interval;
        public float expires;
        public System.Action action;
        public int heapIndex;
    }

    public class CSTimer : Singleton<CSTimer>
    {
        FastArrayPoolLocal<TimerEventHandle> timerHandlePool;
        public CSTimer()
        {
            timerHandlePool = new FastArrayPoolLocal<TimerEventHandle>(64);
            var heapLogicPool = new FastArrayPoolLocal<HeapEntry>(64);
            heap = new FastArrayElementFromPool<HeapEntry>(64, heapLogicPool.Get,f=>
            {
                timerHandlePool.Put(f.timer);
                f.timer = null;
                heapLogicPool.Put(f);
            }, heapLogicPool.Destroy);
        }

        public TimerEventHandle Invoke(float delay, System.Action action)
        {
            var timer = timerHandlePool.Get();
            timer.loopTimes = 0;
            timer.interval = 0;
            timer.action = action;
            timer.expires = delay + Time.realtimeSinceStartup;
            timer.eTimer = TimerType.ONCE;
            add_timer(timer);
            return timer;
        }

        public TimerEventHandle InvokeRepeating(float delay, float interval, System.Action action,int loopTimes = -1,int guid = 0)
        {
            var timer = timerHandlePool.Get();
            timer.loopTimes = loopTimes;
            timer.interval = interval;
            timer.action = action;
            timer.expires = delay + Time.realtimeSinceStartup;
            timer.eTimer = TimerType.LOOP;
            if (guid != 0)
                timer.guid = guid;
            add_timer(timer);
            return timer;
        }

        public void Update()
        {
            float now = Time.realtimeSinceStartup;
            while (heap.Count > 0 && heap[0].time <= now)
            {
                TimerEventHandle timer = heap[0].timer;
                if(timer.eTimer == TimerType.LOOP && (timer.loopTimes < 0 || --timer.loopTimes > 0))
                {
                    remove_timer(timer);
                    timer = timerHandlePool.Get();
                    timer.expires = timer.interval + now;
                    add_timer(timer);
                    timer.action();
                }
                else
                {
                    var action = timer.action;
                    timer.action = null;
                    remove_timer(timer);
                    action();
                }
            }
        }

        public override void OnDispose()
        {
            heap.Destroy();
            heap = null;
            timerHandlePool.Destroy();
            timerHandlePool = null;
        }

        private class HeapEntry
        {
            public float time;
            public TimerEventHandle timer;
        }
        FastArrayElementFromPool<HeapEntry> heap;
        /// <summary>
        /// 添加一个定时器
        /// </summary>
        /// <param name="handle"></param>
        void add_timer(TimerEventHandle timer)
        {
            //插到数组最后一个位置上，上浮
            timer.heapIndex = heap.Count;
            HeapEntry entry = heap.Append();
            entry.time = timer.expires;
            entry.timer = timer;
            up_heap(timer.heapIndex);
        }
        /// <summary>
        /// 移除一个定时器O(lgN)
        /// </summary>
        /// <param name="handle"></param>
        public void remove_timer(TimerEventHandle timer)
        {
            if (null == timer)
                return;
            //头元素用数组未元素替换，然后下沉
            int index = timer.heapIndex;
            int n = heap.Count;
            if (index >= 0 && index < n)
            {
                if (!object.ReferenceEquals(heap[index].timer, timer))
                {
                    return;
                }

                if (index == n - 1)
                    heap.Count = heap.Count - 1;
                else
                {
                    swap_heap(index, n - 1);
                    heap.Count = heap.Count - 1;

                    int parent = (index - 1) >> 1;
                    if (index > 0 && heap[index].time < heap[parent].time)
                        up_heap(index);
                    else
                        down_heap(index);
                }
            }
        }
        /// <summary>
        /// 定时上浮
        /// </summary>
        void up_heap(int index)
        {
            //下至上，和父节点比较。如果小于父节点上浮
            int parent = (index - 1) >> 1;
            while (index > 0 && heap[index].time < heap[parent].time)
            {
                swap_heap(index, parent);
                index = parent;
                parent = (index - 1) / 2;
            }
        }
        /// <summary>
        /// 定时下沉
        /// </summary>
        /// <param name="index"></param>
        void down_heap(int index)
        {
            //从上到下，算出左右子节点，和最小的交换
            int lchild = (index << 1) + 1;
            int n = heap.Count;
            while (lchild < n)
            {
                int minChild = (lchild + 1 == n || heap[lchild].time < heap[lchild + 1].time) ? lchild : lchild + 1;
                if (heap[index].time < heap[minChild].time)
                    break;
                swap_heap(index, minChild);
                index = minChild;
                lchild = (index << 1) + 1;
            }
        }
        /// <summary>
        /// //交换两个timer索引
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        void swap_heap(int l, int r)
        {
            heap.Swap(l, r);
            heap[l].timer.heapIndex = l;
            heap[r].timer.heapIndex = r;
        }
    }
}