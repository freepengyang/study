using UnityEngine;
using System.Collections;


[RequireComponent(typeof(UIPanel))]
public class UIChatSpringPanel : SpringPanel
{

    public UISprite topMessage;
    public UISprite bottomMesssage;

    public float alphaThreshold=5;

    /// <summary>
    /// Advance toward the target position.
    /// </summary>
    protected override void AdvanceTowardsPosition()
    {
        base.AdvanceTowardsPosition();
    }

    public void UpdateSprite(float delta)
    {
        delta = Mathf.Abs(delta);
        if (bottomMesssage.gameObject.activeSelf)
        {
            bottomMesssage.alpha = delta / alphaThreshold;
        }
        else if(topMessage.gameObject.activeSelf)
        {
            topMessage.alpha = delta / alphaThreshold;
        }
    }

}
