using static CSAttributeInfo;

public class SingleAttrData : CSPool.IPoolItem
{
    public PoolHandleManager pooledHandle;
    public int number;
    public KeyValue kv;
    public void OnRecycle()
    {
        kv?.OnRecycle(pooledHandle);
        kv = null;
    }
}