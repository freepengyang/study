using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using TABLE;

//public interface ILFastModeItem
//{
//    int id { get; }
//    int[] intValues { get; set; }
//    long[] longValues { get; set; }
//    string[] stringValues { get; set; }
//    void Encode(System.IO.Stream stream);
//}

//public interface ILFastMode
//{
//    int IntValueFixedLength { get; }
//    int StringValueFixedLength { get; }
//    int LongValueFixedLength { get; }
//    int[] VarIntValues { get; set; }
//    long[] VarLongValues { get; set; }
//    string[] VarStringValues { get; set; }
//    byte[] Rules { get; }
//    ILFastModeItem[] Rows { get; set; }
//    void Decode(byte[] contents);
//}
//public abstract class ILFastModeItem
//{
//    public virtual int id { get; set; }
//    public int[] intValues;// { get; set; }
//    public long[] longValues;// { get; set; }
//    public string[] stringValues;// { get; set; }
//    public abstract void Encode(System.IO.Stream stream);
//}

public abstract class ILFastModeItem
{
    //public int id { get; set; }
    public int[] intValues;// { get; set; }
    public long[] longValues;// { get; set; }
    public string[] stringValues;// { get; set; }
}

public abstract class ILFastMode
{
    public int IntValueFixedLength;
    public int StringValueFixedLength;
    public int LongValueFixedLength;
    public int[] VarIntValues;
    public long[] VarLongValues;// { get; set; }
    public string[] VarStringValues;// { get; set; }
    public byte[] Rules;// { get; }
    public TableData gItem;
    //public ILFastModeItem[] Rows;// { get; set; }
    public abstract void Decode(byte[] contents);
}

public enum ArrayType
{
    AT_INT = 0,
    AT_STRING = 1,
}

public interface IPooledArray
{
    ArrayType ArrayType { get; }
    void Bind(ILFastMode fastMode, int value);
    void Release();
}

public struct IntArray /*: IPooledArray*/
{
    public static IntArray Default = new IntArray();
    //public ArrayType ArrayType
    //{
    //    get
    //    {
    //        return ArrayType.AT_INT;
    //    }
    //}
    //public void Bind(ILFastMode fastMode, int value)
    //{
    //    this.fastMode = fastMode;
    //    start = value & 0xFFF;
    //    end = (value >> 12) & 0xFFF;
    //    length = end - start;
    //}
    public ILFastMode __fastMode;
    public int __start;
    public int __end;
    public int __length;
    public int Count
    {
        get
        {
            return __length;
        }
    }
    public int Length
    {
        get
        {
            return __length;
        }
    }
    public int this[int i]
    {
        get
        {
            //if (null == fastMode)
            //    return 0;
            //int v = start + i;
            //if (v < 0 || v >= fastMode.VarIntValues.Length)
            //    return 0;
            return __fastMode.VarIntValues[__start + i];
        }
    }
    //public void Release()
    //{
    //    ArrayHelper.Instance.Recycle(this);
    //}
}

public struct LongArray /*: IPooledArray*/
{
    //public ArrayType ArrayType
    //{
    //    get
    //    {
    //        return ArrayType.AT_INT;
    //    }
    //}
    //public void Bind(ILFastMode fastMode, int value)
    //{
    //    this.fastMode = fastMode;
    //    start = value & 0xFFF;
    //    end = (value >> 12) & 0xFFF;
    //    length = end - start;
    //}
    public ILFastMode __fastMode;
    public int __start;
    public int __end;
    public int __length;
    public int Count
    {
        get
        {
            return __length;
        }
    }
    public int Length
    {
        get
        {
            return __length;
        }
    }
    public long this[int i]
    {
        get
        {
            //if (null == fastMode)
            //    return 0;
            //int v = start + i;
            //if (v < 0 || v >= fastMode.VarIntValues.Length)
            //    return 0;
            return __fastMode.VarLongValues[__start + i];
        }
    }
    //public void Release()
    //{
    //    ArrayHelper.Instance.Recycle(this);
    //}
}

public struct StringArray/* : IPooledArray*/
{
    //public ArrayType ArrayType
    //{
    //    get
    //    {
    //        return ArrayType.AT_STRING;
    //    }
    //}

    //public void Bind(ILFastMode fastMode, int value)
    //{
    //    this.fastMode = fastMode;
    //    start = value & 0xFFF;
    //    end = (value >> 12) & 0xFFF;
    //    length = end - start;
    //}

    public ILFastMode __fastMode;
    public int __start;
    public int __end;
    public int __length;
    public int Count
    {
        get
        {
            return __length;
        }
    }
    public int Length
    {
        get
        {
            return __length;
        }
    }
    public string this[int i]
    {
        get
        {
            //if (null == fastMode)
            //    return string.Empty;
            //int v = start + i;
            //if (v < 0 || v >= fastMode.VarStringValues.Length)
            //    return string.Empty;
            return __fastMode.VarStringValues[__start + i];
        }
    }
    //public void Release()
    //{
    //    ArrayHelper.Instance.Recycle(this);
    //}
}

public static class ArrayExtend
{
    public static void Add(this RepeatedField<int> arr,IntArray intArr)
    {
        if(null != arr)
        {
            for (int i = 0, max = intArr.Count; i < max; ++i)
            {
                arr.Add(intArr[i]);
            }
        }
    }

    public static void AddRange(this RepeatedField<int> arr, IntArray intArr)
    {
        if (null != arr)
        {
            for (int i = 0, max = intArr.Count; i < max; ++i)
            {
                arr.Add(intArr[i]);
            }
        }
    }

    public static void AddRange(this RepeatedField<TABLE.KEYVALUE> arr, LongArray longArr,PoolHandleManager poolHandleManager)
    {
        if (null != arr)
        {
            for (int i = 0, max = longArr.Count; i < max; ++i)
            {
                var keyvalue = poolHandleManager.GetSystemClass<TABLE.KEYVALUE>();
                keyvalue.key = longArr[i].key();
                keyvalue.value = longArr[i].value();
                arr.Add(keyvalue);
            }
        }
    }

    public static void Add(this RepeatedField<TABLE.KEYVALUE> arr, long longValue,PoolHandleManager poolHandleManager)
    {
        if (null != arr)
        {
            var keyvalue = poolHandleManager.GetSystemClass<TABLE.KEYVALUE>();
            keyvalue.key = longValue.key();
            keyvalue.value = longValue.value();
            arr.Add(keyvalue);
        }
    }

    public static int key(this long v)
    {
        return (int)(v & 0xFFFFFFFF);
    }

    public static int value(this long v)
    {
        return (int)((v >> 32));
    }
}

//public class ArrayHelper : Singleton<ArrayHelper>
//{
//    Stack<object>[] mpools = new Stack<object>[2];
//    public IPooledArray Get(ILFastMode fastMode,int value,ArrayType arrayType)
//    {
//        IPooledArray pooledArray = null;
//        var pool = mpools[(int)arrayType];
//        if (null == pool || pool.Count <= 0)
//        {
//            if(arrayType == ArrayType.AT_INT)
//            {
//                pooledArray = new IntArray();
//            }
//            else
//            {
//                pooledArray = new StringArray();
//            }
//        }
//        else
//            pooledArray = pool.Pop() as IPooledArray;
//        pooledArray.Bind(fastMode, value);
//        return pooledArray;
//    }
//    public void Recycle(IPooledArray pooledArray)
//    {
//        if(null != pooledArray)
//        {
//            var pool = mpools[(int)pooledArray.ArrayType];
//            if (null == pool)
//                mpools[(int)pooledArray.ArrayType] = pool = new Stack<object>(32);
//            pool.Push(pooledArray);
//        }
//    }
//}