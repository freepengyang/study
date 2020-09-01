using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//public class TableLoader_Main
//{
//    /*private static TableLoader mInstance;

//    public static TableLoader PrivateInstance
//    {
//        get { return mInstance; }
//        set { mInstance = value; }
//    }

//    //调用 ExtendTableLoader.Instance
//    private static TableLoader Instance
//    {
//        get
//        {
//            if (mInstance == null) mInstance = new TableLoader();
//            return mInstance;
//        }
//        set { mInstance = value; }
//    }*/

//    //public delegate void OnLoadAllTable();
//    public event System.Action onLoadAllTable;

//    public event System.Action OnPreLoadTable;

//    /****************  C&D  ****************/
//    public TableLoader()
//    {
//        mPreLoadedNum = 0;
//        preLoadTables = new Dictionary<string, string>();
//    }

//    public void OnLoadedOneTable(CSResource res)
//    {
//        needLoadList.Remove(res.Path);
//        if (preLoadTables.ContainsKey(res.FileName))
//        {
//            mPreLoadedNum++;
//        }
//        else
//        {
//            mHasLoadedNum++;
//        }

//        if (preLoadTables.Count == mPreLoadedNum)
//        {
//            preLoadTables.Clear();
//            mPreLoadedNum = 0;
//            if (OnPreLoadTable != null)
//            {
//                OnPreLoadTable();
//                OnPreLoadTable = null;
//                preLoadTables.Clear();
//            }
//        }

//        mIsLoadedAll = (mHasLoadedNum + CSPreLoadResourceRes.CurNum) == (mMaxLoadNum + CSPreLoadResourceRes.MaxNum);
//        if (mIsLoadedAll && IsStartCalculate)
//        {
//            mHasLoadedNum = 0;
//            mMaxLoadNum = 0;
//            IsStartCalculate = false;
//            if (onLoadAllTable != null)
//            {
//                onLoadAllTable();
//            }

//            onLoadAllTable = null;
//        }
//    }

//    public void ChecLoadedkProgress()
//    {
//        mIsLoadedAll = (mHasLoadedNum + CSPreLoadResourceRes.CurNum) == (mMaxLoadNum + CSPreLoadResourceRes.MaxNum);
//    }

//    #region 变量

//    public void Clear()
//    {
//    }

//    private bool mIsLoadedAll = false;

//    public bool IsLoadedAll
//    {
//        get { return mIsLoadedAll; }
//        set { mIsLoadedAll = value; }
//    }

//    private int mHasLoadedNum = 0;

//    public int HasLoadedNum
//    {
//        get { return mHasLoadedNum; }
//        set { mHasLoadedNum = value; }
//    }

//    //是否开始计算剩余量，，抛出加载采用分帧，需要此处设置完成
//    public bool IsStartCalculate;

//    public int mMaxLoadNum = 0;

//    public int MaxLoadNum
//    {
//        get { return mMaxLoadNum; }
//        set { mMaxLoadNum = value; }
//    }

//    public List<string> needLoadList = new List<string>();

//    public float CurLoadProgress
//    {
//        get
//        {
//            if ( MaxLoadNum == 0) return 1;
//            return (mHasLoadedNum + CSPreLoadResourceRes.CurNum) * 1.0f /
//                   (MaxLoadNum + CSPreLoadResourceRes.MaxNum);
//        }
//    }

//    #endregion


//    private int mPreLoadedNum = 0;
//    public Dictionary<string, string> preLoadTables = null;


//    public void PreLoadTables(System.Action onPreloadComplete)
//    {
//        OnPreLoadTable = onPreloadComplete;
//    }

//    ///****************  Pubic Funcs  ****************/
//    public virtual void LoadTables()
//    {
//        mIsLoadedAll = false;
//        IsStartCalculate = false;
//    }

//    /****************  Members  ****************/
//    public class TableDesc
//    {
//        /// <summary>
//        /// 表格字典
//        /// </summary>
//        /// <param name="tableName">表格名字</param>
//        /// <param name="page_name">分页名字</param>
//        /// <param name="proto_message_name">proto结构名字</param>
//        public TableDesc(string tableName, string page_name, string proto_message_name)
//        {
//            this.tableName = tableName;
//            this.param_0 = page_name;
//            this.param_1 = proto_message_name;
//        }

//        public string tableName;
//        public string param_0;
//        public string param_1;
//    }

//    /// <summary>
//    /// 打包队列
//    /// </summary>
//    public virtual List<TableDesc> PackTableList
//    {
//        get { return null; }
//    }
//}

public abstract class TableManager_Main<TableArrayT, T, K, T_1> : Singleton2<T_1>, IEnumerable where TableArrayT : class,new() where T : class,new() where T_1 : new()
{
    /****************  Overides  ****************/
    public IEnumerator GetEnumerator()
    {
        return dic.GetEnumerator();
    }

    public virtual bool needPreload
    {
        get { return false; }
    }

    /****************  Public Funs  ****************/
    public void AddTables(K key, T table)
    {
        if (dic.ContainsKey(key))
        {
            if (Application.isEditor)
                FNDebug.LogErrorFormat("表格 <  {0}  > s key:  {1}: exist!   赶紧检查表格数据", table?.ToString(),key);

            if (FNDebug.developerConsoleVisible)
                FNDebug.LogError(array.ToString() + "表格 <" + table.ToString() + "> s key: " + key + " exist!");
        }
        else
        {
            dic.Add(key, table);
        }

        PostProcess(table);
    }

    public virtual bool TryGetValue(K key, out T tbl)
    {
        tbl = null;

        if (null == dic)
            return false;

        if (dic.ContainsKey(key))
        {
            tbl = dic[key];
            return true;
        }

        if (dic.Count == 0)
        {
            if (FNDebug.developerConsoleVisible) FNDebug.LogError(typeof(T) + " 表格中没有数据 , 需加载表格数据!!!!");
        }

        tbl = default(T);
        return false;
    }

    protected virtual void PostProcess(T table)
    {

    }


    protected virtual void OnResourceLoaded(CSResourceWWW res)
    {
        res.onLoadedTable -= OnResourceLoaded;

        try
        {
            array = Network.ReadOneDataConfig<TableArrayT>(typeof(T), res);
        }
        catch (System.Exception ex)
        {
            if (FNDebug.developerConsoleVisible)
                FNDebug.LogError("---------------------load table error: " + ex.Message + "FileName==" + res.FileName);
            res.DealNeedWaitHotUpdate();
            res.onLoadedTable += OnResourceLoaded;
            return;
        }
        res.IsCanBeDelete = true;
        CSResourceManager.Singleton.DestroyResource(res.Path);
    }

    public virtual void OnDealOver()
    {
        array = default(TableArrayT);
    }

    public void OnLoad(string name)
    {
        string nameStr = name.ToLower() + ".bytes";
        CSResourceWWW res = CSResourceManager.Singleton.AddQueue(nameStr, ResourceType.TableBytes, null, ResourceAssistType.QueueDeal) as CSResourceWWW;
        res.IsCanBeDelete = false;
        res.onLoadedTable += OnResourceLoaded;
    }

    /****************  Members  ****************/
    public TableArrayT array;

    //public K key;
    Dictionary<K, T> dic = new Dictionary<K, T>();
    public Dictionary<K, T> Dic
    {
        get
        {
            return dic;
        }
        protected set
        {
            dic = value;
        }
    }

    //==========Indexer(LuXiangHua)============//
    public T this[K KEY]
    {
        get
        {
            T table;
            TryGetValue(KEY, out table);
            return table;
        }
    }
}