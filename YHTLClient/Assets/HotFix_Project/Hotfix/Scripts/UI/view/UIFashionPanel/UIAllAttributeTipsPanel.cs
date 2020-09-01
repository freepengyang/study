using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Google.Protobuf.Collections;
using UnityEngine;

/// <summary>
/// 通用总属性加成界面
/// </summary>
public partial class UIAllAttributeTipsPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get
        {
            return false;
        }
    }

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = OnClickClose;
        mbtn_bg.onClick = OnClickClose;
    }

    void OnClickClose(GameObject go)
    {
        Close();
    }

    public override void Show()
    {
        base.Show();
    }
    
    /// <summary>
    /// 显示所有属性加成
    /// </summary>
    /// <param name="titleId">标题Id(配在clienttips表)</param>
    /// <param name="listInfo">所有属性总特征值之和列表</param>
    public void ShowAllAttributeData(int titleId, List<List<int>> listInfo)
    {
        if (listInfo == null) return;
        //显示标题
        CSStringBuilder.Clear();
        mlb_title.text = CSString.Format(titleId);
        //无加成
        if (listInfo.Count == 0)
        {
            mobj_nontips.SetActive(true);
            mfix.SetActive(false);
            return;
        }
        mobj_nontips.SetActive(false);
        mfix.SetActive(true);
        //计算总属性
        RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
        attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, listInfo);
        mgrid_attributes.MaxCount = attrItems.Count;
        GameObject gp = null;
        UILabel lb_content = null;
        for (int i = 0; i < mgrid_attributes.MaxCount; i++)
        {
            gp = mgrid_attributes.controlList[i];
            lb_content = gp.transform.Find("lb_name").gameObject.GetComponent<UILabel>();
            CSStringBuilder.Clear();
            lb_content.text = CSStringBuilder.Append("[cbb694]",attrItems[i].Key, CSString.Format(999), 
                "[-][DCD5B8]",attrItems[i].Value,"[-]").ToString();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
