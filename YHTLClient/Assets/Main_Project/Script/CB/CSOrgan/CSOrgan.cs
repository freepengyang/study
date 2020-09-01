

//-------------------------------------------------------------------------
//CSPart
//Author jiabao
//Time 2015.1.15
//-------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class CSOrgan
{
    public GameObject Go;
    public Transform GoTrans;
    public int Type;
    public CSSpriteAnimation Animation;
    public bool IsHasShoadow = true;
    public int Structure;
    protected CSOrganData organData;

    public CSOrgan(GameObject gb, CSOrganData _organDatar)
    {
        Go = gb;
        if (Go != null) GoTrans = Go.transform;
        organData = _organDatar;
    }

    public virtual void Initialization()
    {
        if (Animation == null)
        {
            Animation = Go.AddComponent<CSSpriteAnimation>();
            Animation.InItData(Go, organData, IsHasShoadow);
        }
    }

    public int GetModelHeight()
    {
        if (Animation != null)
        {
            CSSpriteBase sprite = Animation.getSprite;
            if (sprite != null)
            {
                return (int)sprite.UV.y;
            }
        }
        return 0;
    }

    public Object GetShareMatObj()
    {
        if (Animation != null)
        {
            if (Animation.getSprite.getAtlas != null) return Animation.getSprite.getAtlas;
            return Animation.getSprite.mPicture;
        }
        return null;
    }

    public void SetShareMat(EShareMatType type, Material mat, Vector4 color, Vector4 greyColor)
    {
        if (Animation == null) return;
        if (type == EShareMatType.Balck || type == EShareMatType.Balck_Transparent)
        {
            if (Animation.ShadowData != null)
            {
                Animation.ShadowData.ShadowSprite.SetShader(mat, color, greyColor);
            }
        }
        else
        {
            Animation.getSprite.SetShader(mat, color, greyColor);
        }
    }

    public virtual void InitOrgan()
    {
        Initialization();
        SetLoop(true);
    }

    public virtual void SwitchAction(int curDirection, bool isReset = false, int depth = 0)
    {
        SwitchActionDirection(curDirection, isReset, depth);
    }

    public virtual void SwitchActionDirection(int curDirection, bool isReset = false, int depth = 0)
    {
        SetPartDepth(depth);

        if (Animation != null) Animation.UpdateAnimationsNames(curDirection, isReset);
    }

    public virtual void SetPartDepth(int depth)
    {
        if (Animation != null) Animation.setAniGoDepth(depth);
    }

    public virtual void ClearAtlas()
    {
        if (Animation != null) Animation.ClearAtlas();
    }

    public virtual void SetFPS(int fps)
    {
        if (Animation != null) Animation.setFPS(fps);
    }

    public virtual void SetLoop(bool bl)
    {
        if (Animation != null) Animation.setLoop(bl);
    }

    public virtual void SetAtlas(UIAtlas atlas, int curDirection)
    {
        if (Animation == null) return;
        Animation.setAtlas(atlas, curDirection);
    }

    public virtual void SetCurrentFrame(int frame)
    {
        if (Animation != null) Animation.SetCurrentFrame(frame);
    }

    public virtual void SetStopFrameType(int motion, int stopType)
    {
        if (Animation != null) Animation.SetStopFrameType(motion, stopType);
    }

    public void Play(bool isReset)
    {
        if (Animation != null) Animation.Play(isReset);
    }

    public bool getLoop()
    {
        if (Animation != null) return Animation.getLoop();
        return false;
    }

    public bool getMediaStop()
    {
        if (Animation != null)
        {
            return Animation.Run;
        }

        return false;
    }

    public bool EndOfAction(string motion, int m)
    {
        if (Animation != null)
        {
            return Animation.EndOfAction(motion, m);
        }
        return false;
    }

    public float getMediaTime()
    {
        if (Animation != null) return Animation.getTime();
        return 0;
    }
}
