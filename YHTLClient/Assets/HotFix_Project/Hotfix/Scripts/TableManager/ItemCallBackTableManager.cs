using System;
using System.Collections.Generic;
using TABLE;

public partial class ItemCallBackTableManager
{
    private List<ITEMCALLBACK> _itemcallbacks = new List<ITEMCALLBACK>();
    /// <summary>
    /// 根据四个参数找到唯一id 如果不填则找得是经验得信息 , 经验得剔除默认为 7 策划要求写死
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="levClass"></param>
    /// <param name="mode"></param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public ITEMCALLBACK GetItemInfos(int quality , int levClass , int mode , int itemId = 7 , int subType = 0)
    {
        var arr = array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var value = arr[j].Value as ITEMCALLBACK;

            if (subType == 0)
            {
                if (value.quality == quality && value.levClass == levClass && value.type == mode && itemId == value.para1)
                {
                    return value;
                }
            }
            else
            {
                if (value.subType.Count != 0)
                {
                    bool isSubType = false;
                    for (int i = 0; i < value.subType.Count; i++)
                    {
                        if (value.subType[i] == subType)
                        {
                            isSubType = true;
                        }
                    }

                    if (isSubType)
                    {
                        if (value.quality == quality && value.levClass == levClass && value.type == mode && itemId == value.para1)
                        {
                            return value;
                        }
                    }
                }
            }
        }
        
        return null;
    }


    /// <summary>
    /// item是否有itemcallback值
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="levClass"></param>
    /// <param name="mode"></param>
    /// <param name="subType"></param>
    /// <returns></returns>
    public bool IsHaveByItems(int quality, int levClass, int mode)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var value = arr[i].Value as TABLE.ITEMCALLBACK;
            if (value.quality == quality && value.levClass == levClass && value.type == mode)
            {
                return true;
            }
        }

        return false;
    }

    public void GetItemCallBack(int quality , int levClass , int mode ,int subType , ref Dictionary<int,ITEMCALLBACK> itemcallbacks)
    {
        itemcallbacks.Clear();
        var arr = array.gItem.handles;
        for(int k = 0,max = arr.Length;k < max;++k)
        {
            var value = arr[k].Value as ITEMCALLBACK;
            if (value.quality == quality && value.levClass == levClass && value.type == mode)
            {
                
                if (value.type == 2)
                {
                    for (int i = 0; i < value.subType.Count; i++)
                    {
                        if (value.subType[i] == subType)
                        {
                            itemcallbacks.Add(value.para1,value);
                            break;
                        }
                    }
                }
                else
                {
                    itemcallbacks.Add(value.para1, value);
                }
            }
        }
    }


}