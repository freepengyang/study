using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class SkillTableManager : TableManager<TABLE.SKILLARRAY, TABLE.SKILL, int, SkillTableManager>
{
    //protected override void OnResourceLoaded(CSResourceWWW res)
    //{
    //    base.OnResourceLoaded(res);

    //    if (array != null)
    //    {
    //        for (int i = 0; i < array.rows.Count; i++)
    //        {
    //            AddTables(array.rows[i].sid, array.rows[i]);
    //        }
    //    }
    //    base.OnDealOver();
    //}

    public string GetDescription(int itemId)
    {
        TABLE.SKILL itemTb;
        if (TryGetValue(itemId, out itemTb))
        {
            if (itemTb != null)
            {
                return itemTb.description;
            }
        }
        return "";
    }

    public string GetName(int itemId)
    {
        TABLE.SKILL itemTb;
        if (TryGetValue(itemId, out itemTb))
        {
            if (itemTb != null)
            {
                return itemTb.name;
            }
        }
        return "";
    }
    public string GetNameByGroupId(int _groupId)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SKILL;
            if (item.skillGroup == _groupId)
            {
                return item.name;
            }
        }
        return "";
    }
    public string GetDesByGroupId(int _groupId,int _lv = 1)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SKILL;
            if (item.skillGroup == _groupId && item.level == _lv)
            {
                return item.description;
            }
        }
        return "";
    }
    public string GetTipsDesByGroupId(int _groupId, int _lv = 1)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SKILL;
            if (item.skillGroup == _groupId && item.level == _lv)
            {
                return item.clientDescription;
            }
        }
        return "";
    }
    public string GetIconByGroupId(int _groupId)
    {
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var item = arr[k].Value as TABLE.SKILL;
            if (item.skillGroup == _groupId)
            {
                return item.icon;
            }
        }
        return "";
    }
}
