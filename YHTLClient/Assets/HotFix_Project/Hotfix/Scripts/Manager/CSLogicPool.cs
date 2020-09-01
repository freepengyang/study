using System.Collections.Generic;
using UnityEngine;

namespace CSPool
{
    public interface IPoolItem
    {
        void OnRecycle();
    }

    public class ListPoolHandle
    {
        public static Pool<T> CreateList<T>(PoolHandleManager pooledHandle) where T : class, IPoolItem, new()
        {
            Pool<T> handle = pooledHandle.GetSystemClass<Pool<T>>();
            handle.Initialize(pooledHandle);
            return handle;
        }

        public static void DestroyList<T>(T handle) where T : IRelease
        {
            if (null != handle)
            {
                handle.Release();
            }
        }
    }

    public interface IRelease
    {
        void Release();
    }

    public class Pool<T> : IRelease where T : class, IPoolItem,new()
    {
        protected List<T> pooledItems;
        protected List<T> activedItems;
        protected PoolHandleManager pooledHandle;

        public void Initialize(PoolHandleManager pooledHandle)
        {
            this.pooledHandle = pooledHandle;
            pooledItems = pooledHandle.GetSystemClass<List<T>>();
            activedItems = pooledHandle.GetSystemClass<List<T>>();
        }

        public T Get()
        {
            T handle;
            if(pooledItems.Count > 0)
            {
                handle = pooledItems[0];
                activedItems.Add(handle);
                pooledItems.RemoveAt(0);
            }
            else
            {
                handle = pooledHandle.GetSystemClass<T>();
                activedItems.Add(handle);
            }
            return handle;
        }

        public void RecycleAllItems()
        {
            pooledItems.AddRange(activedItems);
            for(int i = 0; i < activedItems.Count;++i)
            {
                activedItems[i].OnRecycle();
            }
            activedItems.Clear();
        }

        public void Release()
        {
            for(int i = 0; i < activedItems.Count; ++i)
            {
                activedItems[i].OnRecycle();
                pooledHandle.Recycle(activedItems[i]);
            }
            activedItems.Clear();
            pooledHandle.Recycle(activedItems);
            activedItems = null;

            for (int i = 0; i < pooledItems.Count; ++i)
            {
                pooledHandle.Recycle(pooledItems[i]);
            }
            pooledItems.Clear();
            pooledHandle.Recycle(pooledItems);
            pooledItems = null;
        }

        public void Dispose()
        {
            pooledItems?.Clear();
            pooledItems = null;
            activedItems?.Clear();
            activedItems = null;
            pooledHandle = null;
        }
    }
}