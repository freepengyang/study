using UnityEngine;

public static class EffectHelper
{
	public static void PlayEffect(this GameObject gameObject,string name,int speed = 10,bool loop = false,bool delete = true,System.Action cbDone = null)
	{
		if (null == gameObject)
			return;

		gameObject.EnableEffect(true, cbDone);
		CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, name, speed, loop, delete);
	}

    public static void PlayEffect(this GameObject gameObject, int effectId, int speed = 10,bool delete = true, System.Action cbDone = null)
    {
        if (null == gameObject)
            return;

        gameObject.EnableEffect(true, cbDone);
        CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, effectId,10,delete);
    }

    public static void StopEffect(this GameObject gameObject,System.Action cbDone = null)
    {
		gameObject.EnableEffect(false,cbDone);
    }

    public static void RecycleEffect(this GameObject gameObject)
    {
        if(null != gameObject)
            CSEffectPlayMgr.Instance.Recycle(gameObject);
    }

    static void EnableEffect(this GameObject gameObject,bool enable, System.Action cbDone = null)
    {
		if (null == gameObject)
			return;

        UISpriteAnimation spriteAnimation = gameObject.GetComponent<UISpriteAnimation>();
        if (null != spriteAnimation && null != cbDone)
        {
            spriteAnimation.OnFinish -= cbDone;
            if (enable)
            {
                spriteAnimation.OnFinish += cbDone;
            }
        }

		if(null != spriteAnimation && spriteAnimation.enabled != enable)
			spriteAnimation.enabled = enable;

        UISprite sprite = gameObject.GetComponent<UISprite>();
		if(null != sprite && sprite.enabled != enable)
			sprite.enabled = enable;
    }
}