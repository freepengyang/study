using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIFunctionPrompt : UIBase
{
    Dictionary<int, BubbleChecker> checkerDic = new Dictionary<int, BubbleChecker>();
    CSBetterLisHot<BubbleChecker> checkerList = new CSBetterLisHot<BubbleChecker>();
    UIGrid grid;
    GameObject arrow;
    bool isShowAll = false;
    float originalY = 201;
    List<UISprite> itemList = new List<UISprite>();
    /// <summary>
    /// 最大气泡数量
    /// </summary>
    const int MAX_COUNT = 6;
    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.OnFunctionPromptChange, GetRegister);
        grid = UIPrefab.transform.Find("grid").GetComponent<UIGrid>();
        arrow = UIPrefab.transform.Find("arrow").gameObject;
        UIEventListener.Get(arrow).onClick = ArrowClick;
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            itemList.Add(grid.transform.GetChild(i).GetComponent<UISprite>());
        }
    }

    public override void Show()
    {
        base.Show();
        RefrshItems();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    void GetRegister(uint id, object data)
    {
        RefrshItems();
    }

    int activeCount = 0;
    void RefrshItems()
    {
        activeCount = 0;
        checkerList.Clear();
        checkerDic = FunctionPromptManager.Instance.GetFunctionPromptInfo();
        var iter = checkerDic.GetEnumerator();
        while (iter.MoveNext())
        {
            //Debug.Log(iter.Current.Value.OnCheck()+"  气泡检测  "+ checkerList.Count);
            if (checkerList.Count == MAX_COUNT) { break; }
            //if (iter.Current.Value.OnCheck() && checkerList.Count < (MAX_COUNT - 1))
            if (iter.Current.Value.OnCheck() && checkerList.Count < MAX_COUNT)
            {
                activeCount++;
                checkerList.Add(iter.Current.Value);
                mClientEvent.SendEvent(CEvent.BubbleGuide, iter.Current.Value.ID);
            }
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            if (isShowAll)
            {
                if (i < checkerList.Count)
                {
                    itemList[i].gameObject.SetActive(true);
                    UIEventListener.Get(itemList[i].gameObject, checkerList[i]).onClick = OnClick;
                    itemList[i].spriteName = (checkerList[i].IconID).ToString();
                    itemList[i].gameObject.name = checkerList[i].ID.ToString();
                }
                else
                {
                    itemList[i].gameObject.SetActive(false);
                    itemList[i].gameObject.name = i.ToString();
                }
            }
            else
            {
                if (i < checkerList.Count && i < 2)
                {
                    itemList[i].gameObject.SetActive(true);
                    UIEventListener.Get(itemList[i].gameObject, checkerList[i]).onClick = OnClick;
                    itemList[i].spriteName = (checkerList[i].IconID).ToString();
                    itemList[i].gameObject.name = checkerList[i].ID.ToString();
                }
                else
                {
                    itemList[i].gameObject.SetActive(false);
                    itemList[i].gameObject.name = i.ToString();
                }
            }
        }
        grid.Reposition();
        if (isShowAll)
        {
            if (activeCount > 2)
            {
                arrow.SetActive(true);
                arrow.transform.localPosition = new Vector3(-208, 201 + (46 * activeCount), 0);
                arrow.transform.localEulerAngles = new Vector3(180, 0, 0);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
        else
        {
            if (activeCount > 2)
            {
                arrow.SetActive(true);
                arrow.transform.localPosition = new Vector3(-208, 201 + (46 * 2), 0);
                arrow.transform.localEulerAngles = Vector3.zero; ;
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }
    void OnClick(GameObject _go)
    {
        BubbleChecker obj = (BubbleChecker)UIEventListener.Get(_go).parameter;
        if (obj != null)
        {
            obj.OnClick();
        }
    }

    void ArrowClick(GameObject _go)
    {
        isShowAll = !isShowAll;
        RefrshItems();
    }
}



