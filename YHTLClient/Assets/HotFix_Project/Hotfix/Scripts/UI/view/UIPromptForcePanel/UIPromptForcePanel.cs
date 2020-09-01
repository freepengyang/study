using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIPromptForcePanel : UIBasePanel
{
    private System.Action mCallBack;
    private System.Object mData;

    public override UILayerType PanelLayerType { get { return UILayerType.Hint; } }

    public int mPromptID = 0;
    TABLE.PROMPTWORD tablePrompt;
    private bool isOpen = true;//是否二次打开面板

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_right).onPress = OnClickRightBtn;
    }

    public override void Show()
    {
        base.Show();
        isOpen = true;
    }

    public void Show(int promptId, System.Action callback, params System.Object[] contents)
    {
        if (Show(promptId, contents))
        {
            mCallBack = callback;
        }
        else
        {
            if (promptId == 1) mCallBack = callback;//特殊处理表格数据没加载时的回调
        }
    }
    
    /// <summary>
    /// 表格数据加载前，需要弹框的，调用此接口，，其他切勿调用
    /// </summary>
    public void Show(string content, string leftBtn, string rightBtn, System.Action callback, params System.Object[] contents)
    {
        mlb_Title.text = CSStringTip.HINT;
        if (mlb_Content != null)
        {
            if (contents != null && contents.Length > 0)
                mlb_Content.text = string.Format(content, contents);
            else
                mlb_Content.text = content;
        }
        if (!string.IsNullOrEmpty(rightBtn))
        {
            if (mlb_rightLabel != null)
            {
                mlb_rightLabel.text = rightBtn;
                mlb_rightLabel.transform.parent.gameObject.SetActive(true);
            }
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
        }

        if (!string.IsNullOrEmpty(tablePrompt.rightBtn))
        {
            if (mlb_rightLabel != null)
            {
                mlb_rightLabel.text = tablePrompt.rightBtn;
                mlb_rightLabel.transform.parent.gameObject.SetActive(true);
            }
        }
        return true;
    }


    public override void OnRecycle()
    {
        base.OnRecycle();
    }

    #region Button

    private void OnClickRightBtn(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            isOpen = false;
            if (mCallBack != null)
            {
                mCallBack();
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
        mbtn_right = null;
        mCallBack = null;
        mlb_rightLabel = null;
        base.OnDestroy();
    }

}
