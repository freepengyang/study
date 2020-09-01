﻿using UnityEngine;
using System.Collections;
using GroupData = UIDebugInfo.GroupData;
using Data = UIDebugInfo.Data;
public class UIDebugScrollBase : MonoBehaviour 
{
    public UIScrollView scroll;
    public UIGridContainer gc;
    protected CSBetterList<GroupData> dataList = null;
    private bool mIsNeedUpdateData = false;
    public GameObject mark;
    public ELogToggleType togType = ELogToggleType.NormalMSG;
    public bool IsNeedUpdateData
    {
        get { return mIsNeedUpdateData; }
        set {
            //if (mIsNeedUpdateData == value) return;
            mIsNeedUpdateData = value;
            if (mark.gameObject.activeSelf != mIsNeedUpdateData)
                mark.gameObject.SetActive(mIsNeedUpdateData);
        }
    }
    public void RefreshScroll(bool isUpdateData = false)
    {
        if (isUpdateData)
        {
            dataList = GetGroupList();
        }
        DrawGC();
    }

    public void UpdateData()
    {
        dataList = GetGroupList();
        mIsNeedUpdateData = false;
    }

    public void Clear()
    {
        dataList = null;
        IsNeedUpdateData = false;
        DrawGC();
    }

    public void ClearData()
    {
        dataList = null;
        IsNeedUpdateData = false;
    }

    protected virtual CSBetterList<GroupData> GetGroupList()
    {
        return null;
    }

    protected virtual void DrawGC()
    {
        if (dataList == null)
        {
            gc.MaxCount = 0;
            return;
        }
        gc.MaxCount = GetMaxLine();
        int groupIndex = 0;
        for(int i = 0;i<gc.MaxCount;i++)
        {
            UIDebugGCItem item = gc.controlList[i].GetComponent<UIDebugGCItem>();
            item.gcIndex = i;
            GroupData groupData;
            Data data;
            GetData(i + 1, out groupData, out data);
            if (groupData != null)//group标头位置
            {
                item.folder.SetActive(groupData.list.Count > 1);
                item.spFolder.spriteName = groupData.isFolder ? "btn_add" : "btn_sub";
                UIEventListener.Get(item.folder).onClick = OnClickFolder;
                groupData.gcIndex = i;
                groupData.groupIndex = groupIndex++;
                item.labNum.text = groupData.groupIndex.ToString();
                item.labContent.text = groupData.groupName+" "+groupData.groupNum;
                item.labContent.color = groupData.color;
                item.spSelect.gameObject.SetActive(groupData.isSelect);
            }
            else if(data != null)//具体日志位置
            {
                data.gcIndex = i;
                item.folder.SetActive(false);
                item.labNum.text = "";
                item.labContent.text = data.log;
                item.labContent.color = data.color;
                item.spSelect.gameObject.SetActive(data.isSelect);
            }
            //UIEventListener.Get(item.gameObject).onClick = OnClickGCItem;
        }
    }

    void OnClickGCItem(GameObject go)
    {
        UIDebugGCItem item = go.GetComponent<UIDebugGCItem>();
        GroupData groupData;
        Data data;
        GetData(item.gcIndex + 1, out groupData, out data);
        if (groupData != null)
        {
            groupData.isSelect = !groupData.isSelect;
            item.spSelect.gameObject.SetActive(groupData.isSelect);
        }
        else if (data != null)
        {
            data.isSelect = !data.isSelect;
            item.spSelect.gameObject.SetActive(data.isSelect);
        }
    }

    void OnClickFolder(GameObject go)
    {
        UIDebugGCItem item = go.transform.parent.GetComponent<UIDebugGCItem>();
        GroupData groupData;
        Data data;
        GetData(item.gcIndex + 1, out groupData, out data);
        if (groupData != null)
        {
            groupData.isFolder = !groupData.isFolder;
            DrawGC();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line">从1开始</param>
    /// <param name="outGroupData">只有1个元素或者是group title则返回groupData</param>
    /// <param name="outData"></param>
    protected void GetData(int line,out GroupData outGroupData,out Data outData)
    {
        outGroupData = null;
        outData = null;
        if (dataList == null) return;
        int temp = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            GroupData groupData = dataList[i];
            if (groupData.list.Count == 1)
            {
                temp++;
                if (temp == line)
                {
                    outGroupData = groupData;
                    break;
                }
            }
            else
            {
                temp++;//group title
                if (temp == line)
                {
                    outGroupData = groupData;
                    break;
                }
                if (!groupData.isFolder&&line<=temp + groupData.list.Count)
                {
                    outData = groupData.list[line - temp -1];
                    break;
                }
                if (!groupData.isFolder) temp += groupData.list.Count;
            }
        }
        return;
    }

    protected int GetMaxLine()
    {
        if (dataList == null) return 0;
        int line = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].list.Count <= 1)
            {
                line += 1;
            }
            else if (dataList[i].isFolder)
            {
                line += 1;
            }
            else
            {
                line += dataList[i].list.Count + 1;//如果group里面只有一条那么只算1（即只有groupName），如果有多条那么算上groupName
            }
        }
        return line;
    }

    public void SetBoxColliderEnable(bool isEnable)
    {
        BoxCollider[] boxs = scroll.transform.GetComponentsInChildren<BoxCollider>(true);
        for (int i = 0; i < boxs.Length; i++)
        {
            if (boxs[i].name == "folder") continue;
            boxs[i].enabled = isEnable;
        }
    }

    public int Search(string str)
    {
        int num = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].isSelect = false;
            if (!string.IsNullOrEmpty(str) && dataList[i].groupName.ToLower().Contains(str.ToLower()))
            {
                dataList[i].isSelect = true;
                num++;
            }
            for (int j = 0; j < dataList[i].list.Count; j++)
            {
                Data data = dataList[i].list[j];
                data.isSelect = false;
                if (!string.IsNullOrEmpty(str) && data.log.ToLower().Contains(str.ToLower()))
                {
                    data.isSelect = true;
                    num++;
                }
            }
        }
        DrawGC();
        return num;
    }
}
