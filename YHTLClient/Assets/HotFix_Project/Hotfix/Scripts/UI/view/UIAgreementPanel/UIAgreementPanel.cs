using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class UIAgreementPanel : UIBasePanel
{
	public override bool ShowGaussianBlur { get => false; }
	
	private bool isHook = true;
	
	public override void Init()
	{
		base.Init();
		// AddCollider();
		mClientEvent.Reg((uint) CEvent.UserAgreementRead, UserAgreementRead);
		// mbtn_close.onClick = Close;
		mbtn_know.onClick = OnClickKnow;
		mbtn_hook.onClick = OnClickHook;
		CSGame.Sington.StartCoroutine(Dowload());
		mobj_hook.SetActive(isHook);
	}

	void UserAgreementRead(uint id, object data)
	{
		isHook = true;
		mobj_hook.SetActive(isHook);
	}

	public override void Show()
	{
		base.Show();
	}
	
	void OnClickKnow(GameObject go)
	{
		if (go == null) return;
		if (!isHook)
		{
			UIManager.Instance.CreatePanel<UIAgreementTipsPanel>();
		}
		else
		{
			if (!PlayerPrefs.HasKey("allowUIAgreementPanel"))
			{
				UIManager.Instance.CreatePanel<UIOfficialNoticePanel>();
				PlayerPrefs.SetInt("allowUIAgreementPanel", 1);
			}
			Close();
		}
	}	
	
	void OnClickHook(GameObject go)
	{
		if (go == null) return;
		isHook = !isHook;
		mobj_hook.SetActive(isHook);
	}
	
	private IEnumerator Dowload()
	{
		UnityWebRequest www = UnityWebRequest.Get(AppUrl.agreementUrl);

		yield return www.SendWebRequest();

		if (string.IsNullOrEmpty(www.error))
		{
			if (string.IsNullOrEmpty(www.downloadHandler.text))
			{
				yield return DowloadNormal();
			}
			else
			{
				if (mlb_content != null)
				{
					mlb_content.text = www.downloadHandler.text;
				}
			}
		}
		else
		{
			yield return DowloadNormal();
		}
		www.Dispose();
	}

	private IEnumerator DowloadNormal()
	{
		UnityWebRequest www2 = UnityWebRequest.Get(AppUrl.agreementUrl);

		yield return www2.SendWebRequest();

		if (string.IsNullOrEmpty(www2.error))
		{
			if (mlb_content != null)
			{
				mlb_content.text = www2.downloadHandler.text;
			}
		}
		www2.Dispose();
	}

	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
