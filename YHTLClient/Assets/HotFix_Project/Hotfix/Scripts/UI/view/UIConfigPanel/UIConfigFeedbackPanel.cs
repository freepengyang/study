using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public partial class UIConfigFeedbackPanel : UIBasePanel
{
	protected UIInput minput_FeedbackContentInput;
	protected UIInput minput_PhoneTypeInput;
	protected UIInput minput_QQInput;
	protected GameObject mbtn_submit;

    string QQReg = "^[1-9]\\d{4,11}$";

    protected override void _InitScriptBinder()
	{
		minput_FeedbackContentInput = ScriptBinder.GetObject("input_FeedbackContentInput") as UIInput;
		minput_PhoneTypeInput = ScriptBinder.GetObject("input_PhoneTypeInput") as UIInput;
		minput_QQInput = ScriptBinder.GetObject("input_QQInput") as UIInput;
		mbtn_submit = ScriptBinder.GetObject("btn_submit") as UnityEngine.GameObject;
	}


    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();
        EventDelegate.Add(minput_FeedbackContentInput.onChange, () => { OnFeedbackContentInputValueChange(); });
        UIEventListener.Get(mbtn_submit).onClick = OnSubmitFeedback;
    }

    public void OnFeedbackContentInputValueChange()
    {
        if (string.IsNullOrEmpty(minput_FeedbackContentInput.value)) return;
        //if (minput_FeedbackContentInput.value.Length >= minput_FeedbackContentInput.characterLimit) UtilityTips.ShowRedTips(105262);
    }

    void OnSubmitFeedback(GameObject go)
    {
        if (string.IsNullOrEmpty(minput_FeedbackContentInput.value.Trim()))
        {
            //UtilityTips.ShowTips(100117);               //��ɫ�޸ĳ���ͳһ����ʾ��ɫ-------zc
            return;
        }
        //QQ�ſɲ�����,�������������,�ж��Ƿ���Ϲ淶
        if (!string.IsNullOrEmpty(minput_QQInput.value.Trim()))
        {
            if (!Regex.IsMatch(minput_QQInput.value.Trim(), QQReg))
            {
                //UtilityTips.ShowTips(100118);    //�����м�Ƚ��ڵ��ĳ��˿��µ���ʾ��ʾ��ʽ-------zc
                return;
            }
        }

        Net.ReqUserFeedbackMessage(minput_FeedbackContentInput.value, minput_PhoneTypeInput.value, minput_QQInput.value);
        
        //Utility.ShowTips("[FFEB04]�������ͳɹ�����л����֧�֣�лл��");
        minput_FeedbackContentInput.value = string.Empty;
        minput_PhoneTypeInput.value = string.Empty;
        minput_QQInput.value = string.Empty;
    }
}
