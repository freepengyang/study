using UnityEngine;
using System.Collections.Generic;


public class UIGridMainScene : UIGrid
{
	protected override void ResetPosition (List<Transform> list)
	{
		mReposition = false;

		// Epic hack: Unparent all children so that we get to control the stage in which they are re-added back in
		// EDIT: Turns out this does nothing.
		//for (int i = 0, imax = list.Count; i < imax; ++i)
		//	list[i].parent = null;

		int x = 0;
		int y = 0;
		int maxX = 0;
		int maxY = 0;
		Transform myTrans = transform;

        int direction = pivot == UIWidget.Pivot.Right ? -1 : 1; 

        // Re-add the children in the same stage we have them in and position them accordingly
        for (int i = 0, imax = list.Count; i < imax; ++i)
		{
			Transform t = list[i];
			// See above
			//t.parent = myTrans;

			Vector3 pos = t.localPosition;
			float depth = pos.z;

			if (arrangement == Arrangement.CellSnap)
			{
				if (cellWidth > 0) pos.x = Mathf.Round(pos.x / cellWidth) * cellWidth;
				if (cellHeight > 0) pos.y = Mathf.Round(pos.y / cellHeight) * cellHeight;
			}
			else pos = (arrangement == Arrangement.Horizontal) ?
				new Vector3(cellWidth * x, -cellHeight * y, depth) :
				new Vector3(direction * cellWidth * y, -cellHeight * x, depth);

			if (animateSmoothly && Application.isPlaying)
			{
				SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
				sp.updateScrollView = true;
				sp.ignoreTimeScale = true;
			}
			else t.localPosition = pos;

			maxX = Mathf.Max(maxX, x);
			maxY = Mathf.Max(maxY, y);

			if (++x >= maxPerLine && maxPerLine > 0)
			{
				x = 0;
				++y;
			}
		}

		//// Apply the origin offset
		//if (pivot != UIWidget.Pivot.TopLeft)
		//{
		//	Vector2 po = NGUIMath.GetPivotOffset(pivot);

		//	float fx, fy;

		//	if (arrangement == Arrangement.Horizontal)
		//	{
		//		fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
		//		fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);
		//	}
		//	else
		//	{
		//		fx = Mathf.Lerp(0f, maxY * cellWidth, po.x);
		//		fy = Mathf.Lerp(-maxX * cellHeight, 0f, po.y);
		//	}

		//	for (int i = 0; i < myTrans.childCount; ++i)
		//	{
		//		Transform t = myTrans.GetChild(i);
		//		SpringPosition sp = t.GetComponent<SpringPosition>();

		//		if (sp != null)
		//		{
		//			sp.target.x -= fx;
		//			sp.target.y -= fy;
		//		}
		//		else
		//		{
		//			Vector3 pos = t.localPosition;
		//			pos.x -= fx;
		//			pos.y -= fy;
		//			t.localPosition = pos;
		//		}
		//	}
		//}
	}
}
