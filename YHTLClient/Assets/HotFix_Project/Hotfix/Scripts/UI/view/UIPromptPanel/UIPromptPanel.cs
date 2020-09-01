using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TABLE;

/// <summary>
/// 提示面板
/// </summary>
public class UIPromptPanel : UIBasePanel
{
    private Action<PromptState, bool> mCallBack;
    private System.Object mData;

    UILabel mlb_Title;
    UILabel mlb_Content;
    UIToggle mtog_value;
    UILabel mlb_togLabel;
    UnityEngine.GameObject mbtn_close;
    UnityEngine.GameObject mbtn_left;
    UnityEngine.GameObject mbtn_right;
    UILabel mlb_leftLabel;
    UILabel mlb_rightLabel;
    UILabel mlb_CDContent;

    protected override void _InitScriptBinder()
    {
        mlb_Title = ScriptBinder.GetObject("lb_Title") as UILabel;
        mlb_Content = ScriptBinder.GetObject("lb_Content") as UILabel;
        mtog_value = ScriptBinder.GetObject("tog_value") as UIToggle;
        mlb_togLabel = ScriptBinder.GetObject("lb_Label") as UILabel;
        mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
        mbtn_left = ScriptBinder.GetObject("btn_left") as UnityEngine.GameObject;
        mbtn_right = ScriptBinder.GetObject("btn_right") as UnityEngine.GameObject;
        mlb_leftLabel = ScriptBinder.GetObject("lb_leftLabel") as UILabel;
        mlb_rightLabel = ScriptBinder.GetObject("lb_rightLabel") as UILabel;
        mlb_CDContent = ScriptBinder.GetObject("lb_CDContent") as UILabel;
    }


    public override UILayerType PanelLayerType { get { return UILayerType.Hint; } }

    public int mPromptID = 0;
    TABLE.PROMPTWORD tablePrompt;
    private int invokeTime = 0;
    private bool isOpen = true;//是否二次打开面板
    Dictionary<int, string> ChooseTipsDic = new Dictionary<int, string>()
    {
        {1,CSString.Format(415)},
        {2, CSString.Format(416)},
    };


    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_left).onPress = OnClickLeftBtn;
        UIEventListener.Get(mbtn_right).onPress = OnClickRightBtn;
        UIEventListener.Get(mbtn_close).onPress = OnCloseClick;
        mlb_Content.SetupLink(null,() =>
        {
            mClientEvent.SendEvent(CEvent.PrompClose);
            Close();
        });
    }

    public override void Show()
    {
        base.Show();
        //if(!isOpen)PlayCommonOpenEffect();
        isOpen = true;
    }

    public void Show(int promptId, Action<PromptState, bool> callback, int CloseTime, params System.Object[] contents)
    {
        if (Show(promptId, contents))
        {
            mCallBack = callback;
        }
        else
        {
            if (promptId == 1) mCallBack = callback;//特殊处理表格数据没加载时的回调
        }

        if (CloseTime > 0)
        {
            InvokeClosePanel(CloseTime);
        }
    }
    
    /// <summary>
    /// 表格数据加载前，需要弹框的，调用此接口，，其他切勿调用
    /// </summary>
    public void Show(string content, string leftBtn, string rightBtn, Action<PromptState, bool> callback, params System.Object[] contents)
    {
        mlb_Title.text = CSStringTip.HINT;
        if (mlb_Content != null)
        {
            if (contents != null && contents.Length > 0)
                mlb_Content.text = string.Format(content, contents);
            else
                mlb_Content.text = content;
            mlb_Content.ResizeCollider();
        }
        if (mtog_value != null)
            mtog_value.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(rightBtn))
        {
            if (mlb_rightLabel != null)
            {
                mlb_rightLabel.text = rightBtn;
                mlb_rightLabel.transform.parent.gameObject.SetActive(true);
            }
        }

        if (!string.IsNullOrEmpty(leftBtn))
        {
            if (mlb_leftLabel != null)
            {
                mlb_leftLabel.text = leftBtn;
                mlb_leftLabel.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            if (mlb_rightLabel != null)
                mlb_rightLabel.transform.parent.localPosition = Vector3.up * mlb_rightLabel.transform.parent.localPosition.y;

            mlb_leftLabel.transform.parent.gameObject.SetActive(false);
        }
        mCallBack = callback;
    }

    private bool Show(int promptId, params System.Object[] contents)
    {
        if (!PromptWordTableManager.Instance.TryGetValue(promptId, out tablePrompt))
        {
            return false;
        }

        mlb_Title.text = tablePrompt.title;
        if (mlb_Content != null)
        {
            if (contents != null && contents.Length > 0)
                mlb_Content.text = string.Format(tablePrompt.dec, contents);
            else
                mlb_Content.text = tablePrompt.dec;
            mlb_Content.ResizeCollider();
        }

        if (tablePrompt.toggle == 2)
        {
            mPromptID = promptId;
            if (mtog_value)
            {
                Vector3 pos = mtog_value.transform.localPosition;
                mtog_value.transform.localPosition = new Vector3(pos.x - 30, pos.y, pos.z);
            }
        }

        if (mtog_value != null)
            mtog_value.gameObject.SetActive(tablePrompt.toggle == 1 || tablePrompt.toggle == 2);

        if (ChooseTipsDic.ContainsKey((int)tablePrompt.toggle))
        {
            if (mlb_togLabel) mlb_togLabel.text = ChooseTipsDic[(int)tablePrompt.toggle];
        }

        if (!string.IsNullOrEmpty(tablePrompt.rightBtn))
        {
            if (mlb_rightLabel != null)
            {
                mlb_rightLabel.text = tablePrompt.rightBtn;
                mlb_rightLabel.transform.parent.gameObject.SetActive(true);
            }
        }

        if (!string.IsNullOrEmpty(tablePrompt.leftBtn))
        {
            if (mlb_leftLabel != null)
            {
                mlb_leftLabel.text = tablePrompt.leftBtn;
                mlb_leftLabel.transform.parent.gameObject.SetActive(true);
            }
            // if (mlb_rightLabel != null)
            //     mlb_rightLabel.transform.parent.localPosition = Vector3.one * mlb_rightLabel.transform.parent.localPosition.y;

        }
        else
        {
            if (mlb_rightLabel != null)
                mlb_rightLabel.transform.parent.localPosition = Vector3.up * mlb_rightLabel.transform.parent.localPosition.y;

            mlb_leftLabel.transform.parent.gameObject.SetActive(false);
        }

        return true;
    }


    public override void OnRecycle()
    {
        if (mtog_value.value == true)
        {
            mtog_value.value = false;
            mtog_value.instantTween = true;
        }
        base.OnRecycle();
    }

    private void InvokeClosePanel(int seconds)
    {
        mlb_CDContent.gameObject.SetActive(true);
        invokeTime = seconds;
        ScriptBinder.InvokeRepeating(0f, 1f, CountDown);
    }

    private void CountDown()
    {
        if (invokeTime <= 0)
        {
            ScriptBinder.StopInvokeRepeating();
            UIManager.Instance.ClosePanel<UIPromptPanel>();
            mlb_CDContent.text = "";
            mlb_CDContent.gameObject.SetActive(false);
        }
        mlb_CDContent.text = CSString.Format(417, invokeTime);
        invokeTime--;
    }

    private void RecordToggleValue()
    {
        if (mtog_value != null && mtog_value.value && mPromptID > 0)
        {
            if (!Constant.ShowTipsOnceList.Contains(mPromptID))
            {
                Constant.ShowTipsOnceList.Add(mPromptID);
            }
        }
    }

    #region Button
    private void OnCloseClick(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            isOpen = false;
            if (mCallBack != null)
            {
                mCallBack(PromptState.close, mtog_value.value);
            }
            ClosePanel();
        }
    }

    private void OnClickLeftBtn(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            isOpen = false;
            if (mCallBack != null)
            {
                mCallBack(PromptState.leftBtn, mtog_value.value);
            }
            ClosePanel();
            
        }
    }


    private void OnClickRightBtn(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            isOpen = false;
            RecordToggleValue();
            if (mCallBack != null)
            {
                mCallBack(PromptState.rightBtn, mtog_value.value);
            }
            ClosePanel();
        }
    }


    private void ClosePanel()
    {
        if (!isOpen)
            UIManager.Instance.ClosePanel<UIPromptPanel>();
    }

    #endregion


    protected override void OnDestroy()
    {
        
        mlb_Title = null;
        mlb_Content = null;
        mtog_value = null;
        mlb_togLabel = null;
        mbtn_close = null;
        mbtn_left = null;
        mbtn_right = null;
        mlb_leftLabel = null;
        mCallBack = null;
        mlb_rightLabel = null;
        invokeTime = 0;
        mlb_CDContent.text = "";
        mlb_CDContent.gameObject.SetActive(false);
        base.OnDestroy();
    }

}
