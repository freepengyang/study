using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionItem : IDispose
{
	private GameObject mPrefab;
	private FunctionTweenRotation mParent;
	private Vector3 mdelVec;
	private int curAngle;

	private Transform _spr_icon;

	private Transform spr_icon
	{
		get { return _spr_icon ?? (_spr_icon = mPrefab.transform.Find("spr_icon")); }
	}

	public void Init(GameObject go, FunctionTweenRotation parent)
	{
		mPrefab = go;
		mParent = parent;
		curAngle = (int) mPrefab.transform.localEulerAngles.z;
		if (curAngle > 180)
			curAngle = curAngle - 360;
		UIEventListener.Get(go).onDragStart = OnDragStart;
		UIEventListener.Get(go).onDrag = OnDrag;
		UIEventListener.Get(go).onDragEnd = OnDragEnd;
	}

	private void OnDragStart(GameObject go)
	{
		if (mParent != null) mParent.OnDragStart();
	}

	private void OnDrag(GameObject go, Vector2 vector2)
	{
		if (mParent != null) mParent.OnDrag(vector2);
	}

	private void OnDragEnd(GameObject go)
	{
		if (mParent != null) mParent.OnDragEnd();
	}

	public void OnRotate(float angle)
	{
		if (angle > 180)
			angle = angle - 360;
		mdelVec.Set(0, 0, (angle + curAngle) * -1);
		if (spr_icon != null) spr_icon.localEulerAngles = mdelVec;
	}

	public int GetPosY()
	{
		if (mPrefab != null)
			return (int) mPrefab.transform.localPosition.y;
		return 0;
	}

	public void Dispose()
	{
		mPrefab = null;
		mParent = null;
		_spr_icon = null;
	}
}