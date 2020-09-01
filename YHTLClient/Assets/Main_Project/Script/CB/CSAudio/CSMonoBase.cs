using UnityEngine;
using System.Collections;

public interface ICSMonoBase
{
    void CSUpdate();
    void CSOnDestroy();
}
/// <summary>
/// 
/// </summary>
/// <typeparam name="T_DataInfo">关联的数据类，如果没有，指定自己即可</typeparam>
public class CSMonoBase : MonoBehaviour, ICSMonoBase
{
    private CSDataBase mData;
    public CSDataBase Data { get { return mData; } set { mData = value; } }

    private GameObject mResRoot;
    public UnityEngine.GameObject ResRoot
    {
        get
        {
            if (mResRoot == null)
            {
                mResRoot = new GameObject(GetType()+"_Root");
                mResRoot.transform.parent = transform;
            }
            return mResRoot;
        }
    }

    public virtual void CSUpdate() { }

    public virtual void CSOnDestroy()
    {
        if (this == null) return;
        if (mData != null)
        {
            mData.RemoveAllEvent();
        }
        if (gameObject != null)
            Destroy(gameObject);
    }

    public virtual void CSOnDestroy(bool isDestroyGameObject)//切换场景时，不用调用删除了，由删除父节点进行删除
    {
        if (this == null) return;
        if (mData != null)
        {
            mData.RemoveAllEvent();
        }
        if (isDestroyGameObject&&gameObject != null)
            Destroy(gameObject);
    }

    public void DisconnectRoot(bool isDestroyRoot = true)
    {
        if (mResRoot == null) return;
        if (isDestroyRoot)
        {
            Destroy(mResRoot);
            mResRoot = null;
        }
        else
        {
            mResRoot.transform.parent = null;
            mResRoot = null;
        }
    }
}
