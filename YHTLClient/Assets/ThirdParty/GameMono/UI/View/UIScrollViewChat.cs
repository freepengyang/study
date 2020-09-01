using System;
using UnityEngine;

public class UIScrollViewChat : UIScrollView
{
    public Action CallBackShouldMove;
    
    /// <summary>
    /// Update the values of the associated scroll bars.
    /// </summary>

	public override void UpdateScrollbars (bool recalculateBounds)
	{
		if (mPanel == null) return;

		if (horizontalScrollBar != null || verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
                if(mShouldMove && CallBackShouldMove != null)
                {
                    CallBackShouldMove();
                }
			}

			Bounds b = bounds;
			Vector2 bmin = b.min;
			Vector2 bmax = b.max;

			if (horizontalScrollBar != null && bmax.x > bmin.x)
			{
				Vector4 clip = mPanel.finalClipRegion;
				int intViewSize = Mathf.RoundToInt(clip.z);
				if ((intViewSize & 1) != 0) intViewSize -= 1;
				float halfViewSize = intViewSize * 0.5f;
				halfViewSize = Mathf.Round(halfViewSize);

				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					halfViewSize -= mPanel.clipSoftness.x;

				float contentSize = bmax.x - bmin.x;
				float viewSize = halfViewSize * 2f;
				float contentMin = bmin.x;
				float contentMax = bmax.x;
				float viewMin = clip.x - halfViewSize;
				float viewMax = clip.x + halfViewSize;

				contentMin = viewMin - contentMin;
				contentMax = contentMax - viewMax;

				UpdateScrollbars(horizontalScrollBar, contentMin, contentMax, contentSize, viewSize, false);
			}

			if (verticalScrollBar != null && bmax.y > bmin.y)
			{
				Vector4 clip = mPanel.finalClipRegion;
				int intViewSize = Mathf.RoundToInt(clip.w);
				if ((intViewSize & 1) != 0) intViewSize -= 1;
				float halfViewSize = intViewSize * 0.5f;
				halfViewSize = Mathf.Round(halfViewSize);

				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					halfViewSize -= mPanel.clipSoftness.y;

				float contentSize = bmax.y - bmin.y;
				float viewSize = halfViewSize * 2f;
				float contentMin = bmin.y;
				float contentMax = bmax.y;
				float viewMin = clip.y - halfViewSize;
				float viewMax = clip.y + halfViewSize;

				contentMin = viewMin - contentMin;
				contentMax = contentMax - viewMax;

				UpdateScrollbars(verticalScrollBar, contentMin, contentMax, contentSize, viewSize, true);
			}
		}
		else if (recalculateBounds)
		{
			mCalculatedBounds = false;
		}
	}
}
