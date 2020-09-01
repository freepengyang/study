using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
[ExecuteInEditMode]
public class AutoChangeColorLabel : MonoBehaviour {
    [HideInInspector]
    public Color StartColor = Color.red;
    [HideInInspector]
    public Color MiddleColor = Color.yellow;
    [HideInInspector]
    public Color EndColor = Color.blue;

    private Color tmpStart;
    private Color tmpMiddle;
    private Color tmpEnd;
    private Color tmptmp;

    private Coroutine coroutine;
    private UILabel text;
	// Use this for initialization
	void OnEnable() {
        Reset();
    }

    public void Reset()
    {
        tmpStart = StartColor;
        tmpMiddle = MiddleColor;
        tmpEnd = EndColor;
        text = GetComponent<UILabel>();
        coroutine = StartCoroutine(ChangeStartColor(1));
    }

    // Update is called once per frame
    void Update () {
        
	}

    IEnumerator ChangeStartColor(float duration)
    {
        float rate = 1f / duration;
        float t = 0;

        while (true)
        {
            t += Time.deltaTime * rate;

            StartColor = Color.Lerp(tmpStart, tmpEnd, t);
            text.HorizontalGradientLeft = StartColor;
            MiddleColor = Color.Lerp(tmpMiddle, tmpStart, t);
            text.HorizontalGradientMiddle = MiddleColor;
            EndColor = Color.Lerp(tmpEnd, tmpMiddle, t);
            text.HorizontalGradientRight = EndColor;
            if (t > 1)
            {
                tmptmp = tmpStart;
                tmpStart = tmpEnd ;
                tmpEnd = tmpMiddle ;
                tmpMiddle = tmptmp;

                StartCoroutine(ChangeStartColor(duration));
                break;
            }
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
