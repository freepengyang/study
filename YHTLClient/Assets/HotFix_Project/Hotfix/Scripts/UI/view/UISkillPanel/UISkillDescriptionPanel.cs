using System.Collections;
using UnityEngine;

public partial class UISkillDescriptionPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		mBtnClose.onClick = Close;
	}

    public void Show(string description)
    {
		//var tokens = description.Split('#');
		//if (tokens.Length != 2)
		//	return;
		Panel.alpha = 0;
		//if (null != mskillName)
		//	mskillName.text = tokens[0];
		if (null != mDescription)
			mDescription.text = description;

		mview.height = mDescription.height + mTail.height/* + mTitle.height*/;
        mview.transform.localPosition = new Vector3(mview.transform.localPosition.x, mview.height * 0.5f, mview.transform.localPosition.z);

		co = CSGame.Sington.StartCoroutine(InitDescription());
	}

	Coroutine co;
	IEnumerator InitDescription()
	{
		yield return null;
		yield return null;
		var screenPos = UICamera.currentCamera.WorldToScreenPoint(UICamera.lastWorldPosition);
		screenPos += new Vector3(-mview.width,mview.height + 12, 0);
		mview.transform.position = UICamera.currentCamera.ScreenToWorldPoint(screenPos);
		yield return null;

		Panel.alpha = 1;
	}

    protected override void OnDestroy()
	{
		if(null != co)
		{
			CSGame.Sington.StopCoroutine(co);
			co = null;
		}
		base.OnDestroy();
	}
}
