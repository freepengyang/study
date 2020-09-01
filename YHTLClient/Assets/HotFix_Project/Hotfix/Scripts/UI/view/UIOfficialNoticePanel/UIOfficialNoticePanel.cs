using System;
using System.Collections;
using System.Collections.Generic;
using task;
using UnityEngine;
using MiniJSON;
using UnityEngine.Networking;

public partial class UIOfficialNoticePanel : UIBasePanel
{
    public List<Dictionary<string, object>> resMsg;
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_know).onClick = Close;
        UIEventListener.Get(mbtn_close).onClick = Close;
        resMsg = new List<Dictionary<string, object>>();
    }

    public override void Show()
    {
        base.Show();
        StartRequestNotice();
    }

    public void StartRequestNotice()
    {
        BindCoroutine(1,RequestNotice());
    }

    IEnumerator RequestNotice()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrl.noticeUrl);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            object obj = Json.Deserialize(www.downloadHandler.text);
            List<object> mlist = obj as List<object>;
            if (mlist != null)
            {
                
                for (int i = 0; i < mlist.Count; i++)
                {
                    resMsg.Add(mlist[i] as Dictionary<string, object>);
                }
                OnShowNotice();
            }
        }
        else
        {
            SDebug.Log(www.error);
        }
    }

    public void OnBtnKnow(GameObject obj)
    {
        UIManager.Instance.ClosePanel<UIOfficialNoticePanel>();
    }

    void OnShowNotice()
    {
        switch (Platform.mPlatformType)
        {
            case PlatformType.EDITOR:
            case PlatformType.ANDROID:
                mlb_content.text = OnNoticeContentCombine(resMsg);
                // mlb_content.transform.GetComponent<BoxCollider>().enabled =
                //     mlb_content.height < mlb_content.parent.GetComponent<UIPanel>().height ? false : true;
                break;
            case PlatformType.IOS:
                //if (CSVersionMgr.Instance != null && CSVersionMgr.Instance.IsAppStorePassed())
                //{
                mlb_content.text = OnNoticeContentCombine(resMsg);
                // mlb_content.transform.GetComponent<BoxCollider>().enabled =
                //     mlb_content.height < mlb_content.parent.GetComponent<UIPanel>().height ? false : true;
                //}
                break;
        }
    }

    string OnNoticeContentCombine(List<Dictionary<string, object>> msg)
    {
        string content;
        string call;
        CSStringBuilder.Clear();
        string record = "";
        int count = msg.Count > 5 ? 5 : msg.Count;
        for (int i = 0; i < count; i++)
        {
            call = "[ea9419]" + msg[i]["call"].ToString() + "[-]\n\n";
            msg[i]["content"] = msg[i]["content"].ToString().Trim().Replace("\n", "\n     ");
            content = "[e1d9a1]     " + msg[i]["content"] + "[-]";
            if (i != msg.Count - 1)
            {
                record = CSStringBuilder.Append(call + content).Append("\n\n\n").ToString();
            }
            else
            {
                record = CSStringBuilder.Append(call + content).ToString();
            }
        }

        return record;
    }

    protected override void OnDestroy()
    {
        resMsg = null;
        base.OnDestroy();
    }
}