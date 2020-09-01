using System;
using System.Collections;
using System.Collections.Generic;
using task;
using UnityEngine;

public partial class UINPCDialogPanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.NpcDialog;
    }

    public override bool ShowGaussianBlur => false;

    private CSNPCDialogData _dialogData;
    float m_desScrollViewHeight = 0;
    private List<UIItemBase> rewardItemList = null;

    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtnClose).onClick = OnClosePanel;
        mClientEvent.AddEvent(CEvent.Scene_UpdateRoleMove, CheckOutOfRange);
    }

    public override void Show()
    {
        base.Show();
        Reset();
    }

    public void Show(CSNPCDialogData data)
    {
        if (data == null)
        {
            OnClosePanel(null);
            return;
        }

        _dialogData = data;
        ShowTitle();
        ShowSay();
        ShowTaskDescription();
        ShowButtons();
        ShowTime();
    }


    private void ShowTitle()
    {
        mlb_title.text = _dialogData.NpcName;
    }

    private void ShowSay()
    {
        if (_dialogData.CurTaskTab == null || _dialogData.CurTaskState == TaskState.Accepted ||
            _dialogData.TaskProcess <= 3)
        {
            mlb_say.gameObject.SetActive(true);
            mdesTask.SetActive(false);
            mlb_say.text = _dialogData.TaskDescription;
            mlb_limited.text = _dialogData.LimitContext;

            float sayHeight = NGUIMath.CalculateRelativeWidgetBounds(mlb_say.transform).size.y;
            m_desScrollViewHeight = (sayHeight + 70);
            if (!string.IsNullOrEmpty(_dialogData.LimitContext))
                m_desScrollViewHeight += NGUIMath.CalculateRelativeWidgetBounds(mlb_limited.transform).size.y + 5;
            m_desScrollViewHeight = Mathf.Max(60, m_desScrollViewHeight);
            Vector4 rect = mdescriptionScrollView.panel.finalClipRegion;
            mdescriptionScrollView.panel.SetRect(rect.x, 0, rect.z, m_desScrollViewHeight);

            Vector3 vector3 = new Vector3();
            vector3.Set(mdescriptionScrollView.transform.localPosition.x, -(m_desScrollViewHeight * 0.5f), 0);
            mdescriptionScrollView.transform.localPosition = vector3;
            
            vector3.Set(mlb_say.transform.localPosition.x, (m_desScrollViewHeight * 0.5f) - 20, 0);
            mlb_say.transform.localPosition = vector3;
            if (!string.IsNullOrEmpty(_dialogData.LimitContext))
            {
                vector3.Set(mlb_limited.transform.localPosition.x,
                    mlb_say.transform.localPosition.y - sayHeight - 40, 0);
                mlb_limited.transform.localPosition = vector3;
            }
        }
    }

    private void ShowTaskDescription()
    {
        if (_dialogData.CurTaskTab != null && _dialogData.CurTaskState != TaskState.Accepted &&
            _dialogData.TaskProcess > 3)
        {
            mdesTask.SetActive(true);
            mlb_say.gameObject.SetActive(false);

            mlb_taskDesp.text = _dialogData.TaskDescription;

            mlb_taskName.text = _dialogData.TaskTitle;

            if (_dialogData.SilverCount > 0)
            {
                mlb_silvervalue.text = _dialogData.SilverCount.ToString();
                mlb_silvervalue.transform.parent.gameObject.SetActive(true);
            }

            List<NPCTaskReward> rewardList = _dialogData.TaskRewards;
            if (rewardList.Count > 0)
            {
                rewardItemList = UIItemManager.Instance.GetUIItems(rewardList.Count, PropItemType.Normal,
                    mgrid_itemGrid.transform);
                if(rewardItemList != null)
                    for (int i = 0; i < rewardItemList.Count; i++)
                    {
                        NPCTaskReward reward = rewardList[i];
                        if (reward != null)
                        {
                            rewardItemList[i].Refresh(reward.TableItem);
                            rewardItemList[i].SetCount(reward.Count, CSColor.white);
                        }
                    }

                mgrid_itemGrid.Reposition();
                mbuttonScrollView.ResetPosition();
            }
        }
    }

    private void ShowButtons()
    {
        int buttonCounts = _dialogData.NpcButtons.Init(_dialogData);

        if (buttonCounts > 1)
        {
            mbtntask.SetActive(false);
            mbuttonScrollView.gameObject.SetActive(true);
            mgrid_desp.MaxCount = buttonCounts;
            for (int i = 0; i < buttonCounts; i++)
            {
                _dialogData.NpcButtons.SetButton(mgrid_desp.controlList[i], SetButton, i);
            }
        }else if (buttonCounts == 1)
        {
            mbtntask.SetActive(true);
            mbuttonScrollView.gameObject.SetActive(false);
            _dialogData.NpcButtons.SetButton(mbtntask, SetButton, 0);
        }
    }

    private void ShowTime()
    {
        if (_dialogData.Time > 0)
        {
            ScriptBinder.InvokeRepeating(0, 1, TimerDown);
        }
    }

    private void TimerDown()
    {
        if (_dialogData.Time >= 0)
        {
            //CSStringBuilder.Clear();
            //CSStringBuilder.Append(btnName, "(", _dialogData.Time.ToString(), ")");
            //mlb_taskValue.text = CSStringBuilder.ToString();
            _dialogData.Time--;
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
            UIEventListener uiEvent = mbtntask.GetComponent<UIEventListener>();
            if (uiEvent && uiEvent.onClick != null)
            {
                uiEvent.onClick(mbtntask);
            }
        }
    }

    private void SetButton(GameObject btn, string btnName, System.Action action, bool close = true)
    {
        btn.transform.Find("lb_taskValue").GetComponent<UILabel>().text = btnName;
        UIEventListener.Get(btn).onClick = p =>
        {
            if (action != null) action();
            if (close)
                UIManager.Instance.ClosePanel<UINPCDialogPanel>();
        };
    }
    

    private void CheckOutOfRange(uint id, object data)
    {
        if (_dialogData != null && _dialogData.CurNPC != null)
        {
            if (!Utility.IsNearPlayerInMap(_dialogData.CurNPC.bornX, _dialogData.CurNPC.bornY))
            {
                OnClosePanel(null);
            }
        }
    }


    private void OnClosePanel(GameObject go)
    {
        CSPathFinderManager.Instance.PathGuideState = PathGuideState.None;

        UIManager.Instance.ClosePanel<UINPCDialogPanel>();
    }


    public void Reset()
    {
        _dialogData = null;
        m_desScrollViewHeight = 0;
        if (rewardItemList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(rewardItemList);
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        Reset();
    }
}