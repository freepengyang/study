using UnityEngine;

public class UIDailyHideBasePanel : UIBasePanel
{
    private float deathTime = -1.0f;
    protected float DeathTime
    {
        get
        {
            return deathTime;
        }
        set
        {
            deathTime = value;
        }
    }

    private float fadeTime = -1.0f;
    protected float FadeTime
    {
        get
        {
            return fadeTime;
        }
        set
        {
            fadeTime = value;
        }
    }

    public override void Show()
    {
        base.Show();
        if (DeathTime > 0.0f)
        {
            if(FadeTime > 0.0f)
            {
                ScriptBinder.StopInvoke();
                ScriptBinder.Invoke(DeathTime, FadeOut);
            }
            else
            {
                ScriptBinder.StopInvoke2();
                ScriptBinder.Invoke2(DeathTime, Close);
            }
        }
    }

    protected void AutoClose(float delay = 3.0f,float fade = 0.3f)
    {
        DeathTime = delay;
        FadeTime = fade;
    }
    
    void FadeOut()
    {
        var now = Time.time;
        var end = Time.time + FadeTime;
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.InvokeRepeating(0.0f, 0.002f,()=>
        {
            float t = (Time.time - now) * 1.0f / FadeTime;
            //Debug.LogFormat("t = {0}", t);
            Panel.alpha = Mathf.Lerp(1, 0, t);
        });
        ScriptBinder.StopInvoke2();
        ScriptBinder.Invoke2(FadeTime, Close);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}