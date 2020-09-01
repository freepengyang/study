using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyLoopScrollView : MonoBehaviour {

    /// <summary>
    /// 视图中最多显示item的个数
    /// </summary>
    [HideInInspector]
    public int MaxShowCount;
    private int RealShowCount;
    /// <summary>
    /// 视图中每个方向缓存的个数
    /// </summary>
    [HideInInspector]
    public int InvisibleCache = 4;
    /// <summary>
    /// 排列方式
    /// </summary>
    public enum ArrangeDirection
    {
        Left_to_Right,
        Right_to_Left,
        Up_to_Down,
        Down_to_Up,
    }
    /// <summary>  
    /// items的排列方式默认从上到下
    /// </summary>  
    [HideInInspector]
    public ArrangeDirection arrangeDirection = ArrangeDirection.Up_to_Down;

    /// <summary>  
    /// 列表单项模板，注意不要放在itemParent目录下  
    /// </summary>  
    public GameObject itemPrefab;

    /// <summary>  
    /// The items list.  
    /// </summary>  
    [HideInInspector]
    public List<MyLoopItemObject> itemsList = new List<MyLoopItemObject>();
    /// <summary>  
    /// The datas list.  
    /// </summary>  
    public List<MyLoopItemData> datasList;

    /// <summary>  
    /// 列表脚本  
    /// </summary>  
    public UIScrollView scrollView;

    public GameObject itemParent;

    /// <summary>  
    /// itemsList的第一个元素  
    /// </summary>  
    MyLoopItemObject firstItem;
    /// <summary>  
    /// itemsList的最后一个元素  
    /// </summary>  
    MyLoopItemObject lastItem;
    /// <summary>
    /// 当前第一个Item的Index
    /// </summary>
    int mnCurFirstItemIndex = 0;


    public delegate void DelegateHandler(MyLoopItemObject item, MyLoopItemData data);
    /// <summary>  
    /// 响应  
    /// </summary>  
    public DelegateHandler OnItemInit;

    /// <summary>  
    /// 第一item的起始位置  
    /// </summary>  
    [HideInInspector]
    public Vector3 itemStartPos = Vector3.zero;
    /// <summary>  
    /// 菜单项间隙  
    /// </summary>  
    [HideInInspector]
    public float gapDis = 0f;
    /// <summary>  
    /// 开始拖动的位置 
    /// </summary>  
    private float startDragPos;
    /// <summary>  
    /// 结束拖动的位置 
    /// </summary>  
    private float endDragPos;

    //缓存transform
    private Transform scrollTrans;

    //脚本挂上UICenterOnChild时，不会调用OnFinished方法，此处填false
    public bool isCheckFinish = true;

    // 对象池  
    // 再次优化，频繁的创建与销毁  
    Queue<MyLoopItemObject> itemLoop = new Queue<MyLoopItemObject>();

    //方向，上下为1，左右为-1
    int direction = 1;

    void Awake()
    {
        if (itemPrefab == null || scrollView == null || itemParent == null)
        {
            if (Debug.developerConsoleVisible) Debug.LogError("LoopScrollView.Awake() 有属性没有在inspector中赋值");
            return;
        }

        scrollTrans = scrollView.transform;
        // 设置scrollview的movement  
        if (arrangeDirection == ArrangeDirection.Up_to_Down ||
           arrangeDirection == ArrangeDirection.Down_to_Up)
        {
            scrollView.movement = UIScrollView.Movement.Vertical;
        }
        else
        {
            scrollView.movement = UIScrollView.Movement.Horizontal;
        }
        scrollView.onDragStarted += () => {
            if (scrollView.movement == UIScrollView.Movement.Horizontal)
            {
                startDragPos = scrollTrans.localPosition.x;
            }
            else
                startDragPos = scrollTrans.localPosition.y;
        };
        scrollView.onDragFinished += () =>
        {
            if (scrollView.movement == UIScrollView.Movement.Horizontal)
            {
                endDragPos = scrollTrans.localPosition.x; ;
            }
            else
            {
                endDragPos = scrollTrans.localPosition.y; ;
            }
        };
        scrollView.onStoppedMoving += () =>
        {
            scrollView.UpdatePosition();
        };
        direction = scrollView.movement == UIScrollView.Movement.Horizontal ? -1 : 1;
    }

    // Update is called once per frame  
    void Update()
    {
        //if(scrollView.isDragging)  
        {
            Validate();
        }
    }

    /// <summary>  
    /// 检验items的两端是否要补上或删除  
    /// </summary>  
    void Validate()
    {
        if (datasList == null || datasList.Count == 0)
        {
            return;
        }

        // 如果itemsList还不存在  
        if (itemsList.Count == 0)
        {
            MyLoopItemObject item = GetItemFromLoop();
            if (tempFirstItem == -1)
            {
                InitItem(item, 0, datasList[0]);
            }
            else
            {
                InitItemDontReset(item, tempFirstItem, datasList[tempFirstItem]);
            }
            lastItem = item;
            SetFirstItem(item);
            itemsList.Add(item);
        }

        //   
        bool all_invisible = true;
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].widget.isVisible == true)
            {
                all_invisible = false;
                tempFirstItem = -1;
            }
        }

        //在所有item都看不到的时候
        if (all_invisible == true)
        {
            OnDragNoItemReset(60);
            return;
        }

        // 先判断前端是否要增减  
        if (firstItem.widget.isVisible)
        {
            // 判断要不要在它的前面补充一个item  
            if (firstItem.dataIndex > 0)
            {
                MyLoopItemObject item = GetItemFromLoop();

                // 初化：数据索引、大小、位置、显示  
                int index = firstItem.dataIndex - 1;
                AddToFront(firstItem, item, index, datasList[index]);
                SetFirstItem(item);
                itemsList.Insert(0, item);
            }
        }
        else
        {
            // 判断要不要将它移除  
            // 条件：自身是不可见的；且它后一个item也是不可见的（或被被裁剪过半的）.  
            //      这有个隐含条件是itemsList.Count>=2.  
            if (itemsList.Count >= InvisibleCache)
            {
                bool isRemove = true;
                for (int i = 0; i < InvisibleCache; i++)
                {
                    if (itemsList[i].widget.isVisible)
                    {
                        isRemove = false;
                    }
                }
                if (isRemove)
                {
                    itemsList.Remove(firstItem);
                    PutItemToLoop(firstItem);
                    SetFirstItem(itemsList[0]);
                }
            }
        }

        // 再判断后端是否要增减  
        if (lastItem.widget.isVisible)
        {
            // 判断要不要在它的后面补充一个item  
            if (lastItem.dataIndex < datasList.Count - 1)
            {
                MyLoopItemObject item = GetItemFromLoop();

                // 初化：数据索引、大小、位置、显示  
                int index = lastItem.dataIndex + 1;
                AddToBack(lastItem, item, index, datasList[index]);
                lastItem = item;
                itemsList.Add(item);
            }
        }
        else
        {
            // 判断要不要将它移除  
            // 条件：自身是不可见的；且它前一个item也是不可见的（或被被裁剪过半的）.  
            //      这有个隐含条件是itemsList.Count>=2.  
            if (itemsList.Count >= InvisibleCache)
            {
                bool isRemove = true;
                for (int i = 1; i <= InvisibleCache; i++)
                {
                    if (itemsList[itemsList.Count - i].widget.isVisible)
                    {
                        isRemove = false;
                    }
                }
                if (isRemove)
                {
                    itemsList.Remove(lastItem);
                    PutItemToLoop(lastItem);
                    lastItem = itemsList[itemsList.Count - 1];
                }
            }
        }
    }

    /// <summary>  
    /// Init the specified datas.  
    /// </summary>  
    /// <param name="datas">Datas.</param>  
    public void Init(List<MyLoopItemData> datas, DelegateHandler onItemInitCallback)
    {
        RealShowCount = MaxShowCount < datas.Count ? MaxShowCount : datas.Count;
        datasList = datas;
        itemParent.GetComponent<UIWidget>().height = datas.Count * itemPrefab.GetComponent<UIWidget>().height;
        this.OnItemInit = onItemInitCallback;

        Validate();
    }

    public void ResetItemDatas(List<MyLoopItemData> datas, DelegateHandler onItemInitCallback)
    {
        RealShowCount = MaxShowCount < datas.Count ? MaxShowCount : datas.Count;
        datasList = datas;
        itemParent.GetComponent<UIWidget>().height = datas.Count * itemPrefab.GetComponent<UIWidget>().height;
        this.OnItemInit = onItemInitCallback;
    }

    /// <summary>  
    /// 构造一个 item 对象  
    /// </summary>  
    /// <returns>The item.</returns>  
    MyLoopItemObject CreateItem()
    {
        GameObject go = NGUITools.AddChild(itemParent, itemPrefab);
        UIWidget widget = go.GetComponent<UIWidget>();
        MyLoopItemObject item = new MyLoopItemObject();
        item.widget = widget;
        go.SetActive(true);
        return item;
    }

    /// <summary>  
    /// 用数据列表来初始化scrollview  
    /// </summary>  
    /// <param name="item">Item.</param>  
    /// <param name="indexData">Index data.</param>  
    /// <param name="data">Data.</param>  
    void InitItem(MyLoopItemObject item, int dataIndex, MyLoopItemData data)
    {
        if (item == null) return;

        item.dataIndex = dataIndex;
        if (OnItemInit != null)
        {
            OnItemInit(item, data);
        }
        if (item.widget != null)
            item.widget.transform.localPosition = itemStartPos;
    }

    void InitItemDontReset(MyLoopItemObject item, int dataIndex, MyLoopItemData data)
    {
        item.dataIndex = dataIndex;
        if (OnItemInit != null)
        {
            OnItemInit(item, data);
        }
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            float offsetY = (item.widget.height + gapDis) * dataIndex;
            if (arrangeDirection == ArrangeDirection.Down_to_Up) offsetY *= -1f;
            item.widget.transform.localPosition = itemStartPos - new Vector3(0, offsetY, 0);
        }
        else
        {
            float offsetX = (item.widget.width + gapDis) * dataIndex;
            if (arrangeDirection == ArrangeDirection.Right_to_Left) offsetX *= -1f;
            item.widget.transform.localPosition = itemStartPos + new Vector3(offsetX, 0, 0);
        }
    }

    /// <summary>  
    /// 在itemsList前面补上一个item  
    /// </summary>  
    void AddToFront(MyLoopItemObject priorItem, MyLoopItemObject newItem, int newIndex, MyLoopItemData newData)
    {
        InitItem(newItem, newIndex, newData);
        // 计算新item的位置  
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            float offsetY = priorItem.widget.height * 0.5f + gapDis + newItem.widget.height * 0.5f;
            if (arrangeDirection == ArrangeDirection.Down_to_Up) offsetY *= -1f;
            newItem.widget.transform.localPosition = priorItem.widget.cachedTransform.localPosition + new Vector3(0f, offsetY, 0f);
        }
        else
        {
            float offsetX = priorItem.widget.width * 0.5f + gapDis + newItem.widget.width * 0.5f;
            if (arrangeDirection == ArrangeDirection.Right_to_Left) offsetX *= -1f;
            newItem.widget.transform.localPosition = priorItem.widget.cachedTransform.localPosition - new Vector3(offsetX, 0f, 0f);
        }
    }

    /// <summary>  
    /// 在itemsList后面补上一个item  
    /// </summary>  
    void AddToBack(MyLoopItemObject backItem, MyLoopItemObject newItem, int newIndex, MyLoopItemData newData)
    {
        InitItem(newItem, newIndex, newData);
        // 计算新item的位置  
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            float offsetY = backItem.widget.height * 0.5f + gapDis + newItem.widget.height * 0.5f;
            if (arrangeDirection == ArrangeDirection.Down_to_Up) offsetY *= -1f;
            newItem.widget.transform.localPosition = backItem.widget.cachedTransform.localPosition - new Vector3(0f, offsetY, 0f);
        }
        else
        {
            float offsetX = backItem.widget.width * 0.5f + gapDis + newItem.widget.width * 0.5f;
            if (arrangeDirection == ArrangeDirection.Right_to_Left) offsetX *= -1f;
            newItem.widget.transform.localPosition = backItem.widget.cachedTransform.localPosition + new Vector3(offsetX, 0f, 0f);
        }
    }

    int tempFirstItem = -1;
    /// <summary>
    /// 设回初始状态
    /// </summary>
    /// <param name="isResetPos">是否重置位置</param>
    public void ResetToBegining(bool isResetPos = true)
    {
        //itemLoop = new Queue<MyLoopItemObject>();
        if (itemsList != null)
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                itemsList[i].widget.gameObject.name = "0";
                itemsList[i].widget.gameObject.SetActive(false);

                if (!itemLoop.Contains(itemsList[i]))
                {
                    PutItemToLoop(itemsList[i]);
                }
            }
        }
        if (firstItem != null)
        {
            tempFirstItem = isResetPos ? -1 : mnCurFirstItemIndex;
        }
        lastItem = null;
        firstItem = null;
        itemsList.Clear();
        if (isResetPos)
        {
            scrollView.ResetPosition();
        }
    }

    public void OnDragNoItemReset(int itemHeight)
    {
        float index = scrollView.panel.clipOffset.y / itemHeight;
        int nIndex = (int)Mathf.Abs(index);

        MyLoopItemObject item = GetItemFromLoop();
        ResetToBegining(false);
        InitItemDontReset(item, nIndex, datasList[nIndex]);
        lastItem = item;
        SetFirstItem(item);
        itemsList.Add(item);
    }

    #region 对象池性能相关  
    /// <summary>  
    /// 从对象池中取行一个item  
    /// </summary>  
    /// <returns>The item from loop.</returns>  
    MyLoopItemObject GetItemFromLoop()
    {
        MyLoopItemObject item;
        if (itemLoop == null || itemLoop.Count <= 0)
        {
            item = CreateItem();
        }
        else
        {
            item = itemLoop.Dequeue();
            item.widget.gameObject.SetActive(true);
        }
        if (item != null && item.widget != null)
        {
            item.widget.gameObject.SetActive(true);
        }
        return item;
    }
    /// <summary>  
    /// 将要移除的item放入对象池中  
    /// --这个里我保证这个对象池中存在的对象不超过RealShowCount个  
    /// </summary>  
    /// <param name="item">Item</param>  
    void PutItemToLoop(MyLoopItemObject item)
    {
        if (itemLoop.Count >= RealShowCount)
        {
            Destroy(item.widget.gameObject);
            return;
        }
        item.dataIndex = -1;
        item.widget.gameObject.SetActive(false);
        itemLoop.Enqueue(item);
    }
    #endregion

    private void SetFirstItem(MyLoopItemObject itemObj)
    {
        firstItem = itemObj;
        mnCurFirstItemIndex = itemObj.dataIndex;
    }
}

[System.Serializable]
public class MyLoopItemObject
{
    /// <summary>  
    /// The widget.  
    /// </summary>  
    public UIWidget widget;

    /// <summary>  
    /// 本item，在实际整个scrollview中的索引位置，  
    /// 即对就数据，在数据列表中的索引  
    /// </summary>  
    public int dataIndex = -1;

}

public class MyLoopItemData
{


}
