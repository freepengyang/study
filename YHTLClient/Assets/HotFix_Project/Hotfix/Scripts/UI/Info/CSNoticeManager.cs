using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class CSNoticeManager : CSInfo<CSNoticeManager> {

    public CSNoticeManager()
    {
        NoticeMap = new Map<NoticeType, Queue<tip.BulletinResponse>>();
    }

    public Map<NoticeType, Queue<tip.BulletinResponse>> NoticeMap;

    public void ResBulletinMessage(tip.BulletinResponse info)
    {
        NetInfo obj;
        obj.obj = null;
        obj.msgId = 0;
        ResBulletinMessage(info, obj);
    }
    
    public void ResBulletinMessage(tip.BulletinResponse info, NetInfo obj)
    {
        if(info.display== (int)NoticeType.CenterTopAndChat)
        {
            info.display = (int)NoticeType.CenterTop;
        }else if(info.display == (int)NoticeType.BottomAndChat)
        {
            info.display = (int)NoticeType.Bottom;
        }else if(info.display == (int)NoticeType.CenterTopAndBotton)
        {
            info.display = (int)NoticeType.Bottom;
            if (obj.obj != null)
            {
                tip.BulletinResponse info2 = Network.Deserialize<tip.BulletinResponse>(obj);
                info2.display = (int)NoticeType.CenterTop;
                CSNoticeManager.Instance.NoticeEnqueue((NoticeType)info2.display, info);
            }
        }else if (info.display == (int) NoticeType.TopAndChat)
        {
            info.display = (int) NoticeType.Top;
        }
        CSNoticeManager.Instance.NoticeEnqueue((NoticeType)info.display, info);
    }
    
    
    public void NoticeEnqueue(NoticeType type, tip.BulletinResponse MessageInfo)
    {
        if (!NoticeMap.ContainsKey(type))
            NoticeMap.Add(type, new Queue<tip.BulletinResponse>());
        if(type != NoticeType.CenterTopAndChat )
        {
            if (MessageInfo.count == 0 || type == NoticeType.CenterTop) MessageInfo.count = 1;
            for (int i = 0; i < MessageInfo.count; i++)
            {
                NoticeMap[type].Enqueue(MessageInfo);
            }
        }else
        {
            NoticeMap[type].Enqueue(MessageInfo);//type为4时，代表公告时间
        }

        if (CSScene.IsLanuchMainPlayer)
            ShowNoticePanel(type);
    }

    public void NoticeDequeue(NoticeType type)
    {
        if (!NoticeMap.ContainsKey(type))
            return;
        if (NoticeMap[type].Count > 0)
            NoticeMap[type].Dequeue();
    }

    public tip.BulletinResponse NoticePeek(NoticeType type)
    {
        if (!NoticeMap.ContainsKey(type))
            return null;
        if (NoticeMap[type].Count > 0)
            return NoticeMap[type].Peek();
        return null;
    }

    public int NoticeCount(NoticeType type)
    {
        if (!NoticeMap.ContainsKey(type))
            return 0;
        return NoticeMap[type].Count;
    }

    public void ShowAllNotice()
    {
        int nums = Enum.GetNames(typeof(NoticeType)).Length;
        for (int i = 1; i < nums; i++)
        {
            NoticeType type = (NoticeType)i;
            if(NoticeCount(type) > 0)
            {
                ShowNoticePanel(type);
            }
        }
    }

    private UINoticePanel mNoticeTopPanel;
    private UINoticeSecondPanel mNoticeCenterTopPanel;
    private UINoticeBottomPanel mNoticeBottomPanel;
    private UINoticeColoursPanel mNoticeColoursPanel;
    private UINoticeBelowPanel mNoticeBelowPanel;

    private void ShowNoticePanel(NoticeType type)
    {
        switch (type)
        {
            case NoticeType.Top:
                if (mNoticeTopPanel == null || mNoticeTopPanel.UIPrefab == null)
                {
                    UIManager.Instance.CreatePanel<UINoticePanel>(action: (f) =>
                    {
                        mNoticeTopPanel = f as UINoticePanel;
                    });
                }
                else
                {
                    mNoticeTopPanel.Show();
                }
                break;
            case NoticeType.CenterTop:
            case NoticeType.CenterTopAndChat:
                if (mNoticeCenterTopPanel == null || mNoticeCenterTopPanel.UIPrefab == null)
                {
                    UIManager.Instance.CreatePanel<UINoticeSecondPanel>(action: (f) =>
                    {
                        mNoticeCenterTopPanel = f as UINoticeSecondPanel;
                    });
                }
                else
                {
                    mNoticeCenterTopPanel.Show();
                }
                break;
            case NoticeType.Bottom:
                if (mNoticeBottomPanel == null || mNoticeBottomPanel.UIPrefab == null)
                {
                    UIManager.Instance.CreatePanel<UINoticeBottomPanel>(action: (f) =>
                    {
                        mNoticeBottomPanel = f as UINoticeBottomPanel;
                    });
                }
                else
                {
                    mNoticeBottomPanel.Show();
                }
                break;
            case NoticeType.Below:
                if(mNoticeBelowPanel == null || mNoticeBelowPanel.UIPrefab == null)
                {
                    UIManager.Instance.CreatePanel<UINoticeBelowPanel>(action: (f) =>
                    {
                        mNoticeBelowPanel = f as UINoticeBelowPanel;
                    });
                }else
                {
                    mNoticeBelowPanel.Show();
                }

                break;
            case NoticeType.ColoursWorld:
                if(mNoticeColoursPanel == null || mNoticeColoursPanel.UIPrefab == null)
                {
                    UIManager.Instance.CreatePanel<UINoticeColoursPanel>(action: (f) =>
                    {
                        mNoticeColoursPanel = f as UINoticeColoursPanel;
                    });
                }else
                {
                    mNoticeColoursPanel.Show();
                }
                break;
        }
    }

    public tip.BulletinResponse NewNotice(string msg,int count,int display)
    {
        tip.BulletinResponse message = new tip.BulletinResponse();
        message.count = count;
        message.msg = msg;
        message.display = display;
        return message;
    }

    public override void Dispose()
    {
        NoticeMap.Clear();
        if (mNoticeTopPanel != null) UIManager.Instance.ClosePanel<UINoticePanel>();
        if (mNoticeCenterTopPanel != null) UIManager.Instance.ClosePanel<UINoticeSecondPanel>();
        if (mNoticeBottomPanel != null) UIManager.Instance.ClosePanel<UINoticeBottomPanel>();
        if (mNoticeColoursPanel != null) UIManager.Instance.ClosePanel<UINoticeColoursPanel>();
        if (mNoticeBelowPanel != null) UIManager.Instance.ClosePanel<UINoticeBelowPanel>();

    }
}
