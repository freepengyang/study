using UnityEngine;
using System.Collections.Generic;

public class UICenterOnChildBag : UICenterOnChild
{
    private BagWrapContent bagWrapContent { get { return this.GetComponent<BagWrapContent>(); } }

	/// <summary>
	/// Recenter the draggable list on the center-most child.
	/// </summary>

	public override void Recenter ()
	{
		if (mScrollView == null)
		{
			mScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);

			if (mScrollView == null)
			{
				FNDebug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in stage to work", this);
				enabled = false;
				return;
			}
			else
			{
				if (mScrollView)
				{
					mScrollView.centerOnChild = this;
					mScrollView.onDragFinished += OnDragFinished;
				}

				if (mScrollView.horizontalScrollBar != null)
					mScrollView.horizontalScrollBar.onDragFinished += OnDragFinished;

				if (mScrollView.verticalScrollBar != null)
					mScrollView.verticalScrollBar.onDragFinished += OnDragFinished;
			}
		}
		if (mScrollView.panel == null) return;

		Transform trans = transform;
		if (trans.childCount == 0) return;

		// Calculate the panel's center in world coordinates
		Vector3[] corners = mScrollView.panel.worldCorners;
		Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

		// Offset this value by the momentum
		Vector3 momentum = mScrollView.currentMomentum * mScrollView.momentumAmount;
		Vector3 moveDelta = NGUIMath.SpringDampen(ref momentum, 9f, 2f);
		Vector3 pickingPoint = panelCenter - moveDelta * 0.01f; // Magic number based on what "feels right"

		float min = float.MaxValue;
		Transform closest = null;
		int index = 0;
		int ignoredIndex = 0;

		UIGrid grid = GetComponent<UIGrid>();
		List<Transform> list = null;

		// Determine the closest child
		if (grid != null)
		{
			list = grid.GetChildList();

			for (int i = 0, imax = list.Count, ii = 0; i < imax; ++i)
			{
				Transform t = list[i];
				if (!t.gameObject.activeInHierarchy) continue;
				float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

				if (sqrDist < min)
				{
					min = sqrDist;
					closest = t;
					index = i;
					ignoredIndex = ii;
				}
				++ii;
			}
		}
		else
		{
			for (int i = 0, imax = trans.childCount, ii = 0; i < imax; ++i)
			{
				Transform t = trans.GetChild(i);
				if (!t.gameObject.activeInHierarchy) continue;
				float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

				if (sqrDist < min)
				{
					min = sqrDist;
					closest = t;
					index = i;
					ignoredIndex = ii;
				}
				++ii;
			}
		}

        // If we have a touch in progress and the next page threshold set
        if (nextPageThreshold > 0f && UICamera.currentTouch != null)
		{
            if (bagWrapContent != null)
            {
                ignoredIndex = Mathf.RoundToInt(closest.localPosition.x / bagWrapContent.itemSize);
                
            }

            // If we're still on the same object
            if (mCenteredObject != null && mCenteredObject.transform == (list != null ? list[index] : trans.GetChild(index)))
			{
				Vector3 totalDelta = UICamera.currentTouch.totalDelta;
				totalDelta = transform.rotation * totalDelta;

				float delta = 0f;

				switch (mScrollView.movement)
				{
					case UIScrollView.Movement.Horizontal:
					{
						delta = totalDelta.x;
						break;
					}
					case UIScrollView.Movement.Vertical:
					{
						delta = totalDelta.y;
						break;
					}
					default:
					{
						delta = totalDelta.magnitude;
						break;
					}
				}

				if (Mathf.Abs(delta) > nextPageThreshold)
				{
					if (delta > nextPageThreshold)
					{
                        if ( bagWrapContent != null)
                        {
                            ignoredIndex = ignoredIndex > trans.childCount ? (ignoredIndex % 2 == 0 ? 2 : 1) : ignoredIndex;
                        }
                        // Next page
                        if (list != null)
						{
							if (ignoredIndex > 0)
							{
								closest = list[ignoredIndex - 1];
							}
							else closest = (GetComponent<UIWrapContent>() == null) ? list[0] : list[list.Count - 1];
						}
						else if (ignoredIndex > 0 && ignoredIndex <= trans.childCount)
						{
							closest = trans.GetChild(ignoredIndex - 1);
						}
                        else
                        {
                            closest = (bagWrapContent == null) ? trans.GetChild(0) : ignoredIndex > bagWrapContent.minIndex ? ignoredIndex % 2 - 1 > 0 ? trans.GetChild(ignoredIndex % 2 - 1) : trans.GetChild(0) : ignoredIndex % 2 == 0 ? trans.GetChild(0) : trans.GetChild(trans.childCount - 1);
                        }
                    }
                    else if (delta < -nextPageThreshold)
                    {
						// Previous page
						if (list != null)
						{
							if (ignoredIndex < list.Count - 1)
							{
								closest = list[ignoredIndex + 1];
							}
							else closest = (GetComponent<UIWrapContent>() == null) ? list[list.Count - 1] : list[0];
						}
						else if (ignoredIndex < trans.childCount - 1)
						{
							closest = trans.GetChild(ignoredIndex + 1);
						}
                        else
                        {
                            closest = (bagWrapContent == null) ? trans.GetChild(trans.childCount - 1) : ignoredIndex < bagWrapContent.maxIndex ? ignoredIndex % 2 + 1<trans.childCount? trans.GetChild(ignoredIndex % 2 + 1): trans.GetChild(0): ignoredIndex % 2 == 0 ? trans.GetChild(0) : trans.GetChild(trans.childCount - 1);
                        }
					}
                    
				}
			}

		}
		CenterOn(closest, panelCenter);
	}

}
