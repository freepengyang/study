using System;
using System.Collections;
using System.Collections.Generic;
using task;
using UnityEngine;
using MiniJSON;
using UnityEngine.Networking;



public partial class UIBillboardPanel : UIBasePanel
{
    public Dictionary<string, object> resMsg;

    readonly string sureStr = "确定";

    int timerCount = 3;

    bool centerIsClose;

    public override void Init()
	{
		base.Init();

        string timerStr = SundryTableManager.Instance.GetSundryEffect(1047);
        int.TryParse(timerStr, out timerCount);

        mbtn_know.onClick = OnBtnKnow;
        if (mtweenAlpha != null)
        {
            mtweenAlpha.SetOnFinished(Close);
        }

        mobj_center.CustomActive(true);
        StartRequestNotice();
    }
	
	public override void Show()
	{
		base.Show();

        mlb_btnKnow.text = $"{sureStr}({timerCount})";

        CSMainParameterManager.LoadingComplete = false;

        ScriptBinder.InvokeRepeating(1, 1, BtnTimer);
        ScriptBinder.InvokeRepeating2(0, 0.1f, WaitForEnterSceneTimer);
    }
	
	protected override void OnDestroy()
    {
        resMsg?.Clear();
        resMsg = null;
        base.OnDestroy();
	}
       

    public void OnBtnKnow(GameObject obj)
    {
        ScriptBinder.StopInvokeRepeating();
        mobj_center.CustomActive(false);
        centerIsClose = true;
        CSMainParameterManager.LoadingComplete = true;
        WaitForEnterSceneTimer();
    }

    void BtnTimer()
    {
        timerCount--;
        string btntext = timerCount > 0 ? $"{sureStr}({timerCount})" : $"{sureStr}";
        mlb_btnKnow.text = btntext;
        if (timerCount <= 0)
        {
            OnBtnKnow(null);
        }
    }


    void WaitForEnterSceneTimer()
    {
        if (!centerIsClose) return;
        if (CSScaleMapSystem.Instance.IsLoadedScaleMap)
        {
            ScriptBinder.StopInvokeRepeating2();
            Close();
        }
    }


    #region Context

    public void StartRequestNotice()
    {
        CSGame.Sington.StartCoroutine(RequestNotice());
    }

    IEnumerator RequestNotice()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrl.LoadingUrl);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            object obj = Json.Deserialize(www.downloadHandler.text);
            if (obj != null)
            {
                Dictionary<string, object> dic = obj as Dictionary<string, object>;
                if (dic != null)
                {
                    resMsg = dic;
                    OnShowNotice();
                }
            }
            
            //if (obj != null)
            //{
            //List<object> mlist = obj as List<object>;
            //if (mlist != null)
            //{

            //    for (int i = 0; i < mlist.Count; i++)
            //    {
            //        resMsg.Add(mlist[i] as Dictionary<string, object>);
            //    }
            //    OnShowNotice();
            //}
            //}            
        }
        else
        {
            SDebug.Log(www.error);
        }

        CSGame.Sington.StopCoroutine(RequestNotice());

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

    /// <summary>
    /// 旧方法
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    string OnNoticeContentCombine(List<Dictionary<string, object>> msg)
    {
        string content;
        string call;
        CSStringBuilder.Clear();
        //StringBuilder record = new StringBuilder();
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


    string OnNoticeContentCombine(Dictionary<string, object> msg)
    {
        string content = "";
        string call = "";
        CSStringBuilder.Clear();
        //StringBuilder record = new StringBuilder();
        string record = "";
        if (msg.ContainsKey("call"))
            call = "[ea9419]" + msg["call"].ToString() + "[-]\n\n";
        if (msg.ContainsKey("content"))
        {
            msg["content"] = msg["content"].ToString().Trim().Replace("\n", "\n     ");
            content = "[e1d9a1]     " + msg["content"] + "[-]";
        }            
        record = CSStringBuilder.Append(call + content).ToString();

        return record;
    }

    #endregion
}
