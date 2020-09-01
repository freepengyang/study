
//Author jiabao
//date 2015.12.15

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CSSpriteAnimation : CSBaseAnimation
{
    public IBaseFrame FrameAni;
    private bool mIsHasShoadow = true;
    private bool mIsShowShoadow = true;
    public Transform SpriteUvGoTrans;
    public ShadowData shadowData = null;
    public BoxCollider Box;
    private CSAction mAction;
    private bool mIsDataSplit;
    private EShareMatType mMatType;
    private int mAvatarType;
    private CSOrganData organData;

    public void InItData(GameObject go, CSOrganData _organData, bool isHasShoadow)
    {
        // 部件
        Go = go;
        this.organData = _organData;
        Box = _organData.box;
        mAction = _organData.action;
        mIsDataSplit = _organData.isDataSplit;
        mMatType = _organData.matType;
        mAvatarType = _organData.avatarType;
        // 动画纹理
        if (SpriteUvGoTrans == null) SpriteUvGoTrans = new GameObject("UV").transform;
        SpriteUvGoTrans.parent = CahcheTrans;
        SpriteUvGoTrans.localPosition = Vector3.zero;
        SpriteUvGoTrans.localScale = Vector3.one;

        if (Sprite == null) Sprite = SpriteUvGoTrans.gameObject.AddComponent<CSSprite>();

        if (FrameAni == null)
        {
            if (mIsDataSplit)
            {
                FrameAni = SpriteUvGoTrans.gameObject.AddComponent<CSSpriteFrame2>();
            }
            else
            {
                FrameAni = SpriteUvGoTrans.gameObject.AddComponent<CSSpriteFrame>();
            }
        }

        // 动画
        mIsHasShoadow = isHasShoadow;
        if (mIsHasShoadow)
        {
            // 动画纹理
            if (shadowData == null) shadowData = new ShadowData();
            if (shadowData.ShadowUvGoTrans == null) shadowData.ShadowUvGoTrans = new GameObject("Shadow").transform;
            shadowData.ShadowUvGoTrans.parent = CahcheTrans;
            shadowData.ShadowUvGoTrans.localPosition = new Vector3(0, 0, 5);
            shadowData.ShadowUvGoTrans.localScale = Vector3.one;
            if (shadowData.ShadowSprite == null) shadowData.ShadowSprite = shadowData.ShadowUvGoTrans.gameObject.AddComponent<CSSprite>();
            shadowData.ShadowSprite.isSlant = true;
            Sprite.getShadowSprite = shadowData.ShadowSprite;
            ShowShadow(mIsShowShoadow);
        }

        if (FrameAni != null)
        {
            FrameAni.Initialization();
        }
    }

    public Vector3 getAniPosition()
    {
        return SpriteUvGoTrans == null ? Vector3.zero : SpriteUvGoTrans.position;
    }

    public void ShowShadow(bool isShow)
    {
        if (!mIsHasShoadow || shadowData == null) return;
        if (shadowData.ShadowUvGoTrans.gameObject.activeSelf != isShow)
        {
            mIsShowShoadow = isShow;
            shadowData.ShadowUvGoTrans.gameObject.SetActive(isShow);
        }
    }

    void Update()
    {
        if (organData == null) return;

        if (FrameAni != null) FrameAni.UpdateFrame();
  
        if (Box != null && Sprite != null && Sprite.getAtlas != null)
        {
            if (Sprite.IsUVChange)
            {
                Sprite.IsUVChange = false;
                Vector3 vec = Vector2.zero;
                vec.x = Sprite.getUV.x;
                vec.y = Sprite.getUV.y;
                vec.z = 2;
                Box.size = vec;

                if (mAction.Direction == CSDirection.Left
                    || mAction.Direction == CSDirection.Left_Down ||
                    mAction.Direction == CSDirection.Left_Up)
                {
                    vec.x = Sprite.getCentre.x;
                }
                else
                {
                    vec.x = -Sprite.getCentre.x;
                }
                vec.y = Sprite.getCentre.y;
                vec.z = mAvatarType == EAvatarType.NPC ? -10000f : 0f;
                Box.center = vec;
            }
        }
    }

    public bool Run
    {
        get
        {
            if (FrameAni != null)
            {
                return FrameAni.isPlaying;
            }

            return false;
        }
    }

    public bool EndOfAction(string motion,int m)
    {
        if (FrameAni == null) return false;
        CSBaseFrame bf = FrameAni as CSBaseFrame;
        if (bf != null)
        {
            SFAtlas atlas = FrameAni.atlas;
            if (atlas == null) return false;
            if (!FrameAni.Loop && FrameAni.IsPlayingOnLastOneFrame() && atlas.name.Contains(motion)) return true;
        }
        CSBaseFrame2 bf2 = FrameAni as CSBaseFrame2;
        if (bf2 != null)
        {
            if (!FrameAni.Loop && FrameAni.IsPlayingOnLastOneFrame() && mAction.Motion == m) return true;
        }
        return false;
    }

    public float getTime()
    {
        if (FrameAni != null)
        {
            return FrameAni.EstimateTakeTime;
        }
        return 0f;
    }

    public void setAniGoDepth(int depth)
    {
        if (SpriteUvGoTrans != null) 
        {
            SpriteUvGoTrans.localPosition = new Vector3(SpriteUvGoTrans.localPosition.x, SpriteUvGoTrans.localPosition.y, depth);
        }
    }

    public void SetShodowDepath()
    {
        if (shadowData != null)
        {
            shadowData.ShadowUvGoTrans.localPosition = new Vector3(shadowData.ShadowUvGoTrans.localPosition.x, shadowData.ShadowUvGoTrans.localPosition.y, -20);
        }
    }

    public void setFPS(int fps)
    {
        if (FrameAni != null)
        {
            if (FrameAni.isChange)
                FrameAni.FPS = fps;
        }
    }

    public void setLoop(bool bl)
    {
        if (FrameAni != null)
        {
            FrameAni.setLoop(bl);
        }
    }

    public void SetCurrentFrame(int frame)
    {
        if (FrameAni != null)
        {
            if (mIsDataSplit)
            {
                CSBaseFrame2 b = FrameAni as CSBaseFrame2;
                b.CurrentNameCount = frame;
            }
        }
    }

    public void SetStopFrameType(int motion, int stopType)
    {
        if (FrameAni != null)
        {
            BaseFrame bf = FrameAni as BaseFrame;
            if (stopType == EActionStopFrameType.None)
            {
                bf.StopFrame = motion == CSMotion.Dead ? EActionStopFrameType.End : EActionStopFrameType.First;
            }
            else
            {
                bf.StopFrame = stopType;
            }
        }
    }

    public bool getLoop()
    {
        if (FrameAni != null)
        {
            return FrameAni.getLoop();
        }
        return false;
    }

    public void setAtlas(SFAtlas atlas, int curDirection)
    {
        setAtlas(atlas as UIAtlas, curDirection);
    }

    public void setAtlas(UIAtlas atlas, int curDirection)
    {
        if (Sprite != null && FrameAni != null)
        {
            Sprite.getAtlas = atlas;
            if (mIsHasShoadow)
            {
                shadowData.ShadowSprite.getAtlas = atlas;
                SetShaodwShader(shadowData.ShadowSprite, mMatType);
            }
            UpdateAnimationsNames(curDirection, true);
            FrameAni.SetAtlasPostProc();
        }
    }

    public void Play(bool isReset = true)
    {
        if (FrameAni != null)
        {
            FrameAni.Play(isReset);
        }
    }

    public void SetSprite(string spriteName)
    {
        if (Sprite != null)
        {
            Sprite.InitSetSpriteName = spriteName;
        }
    }

    public void ClearAtlas()
    {
        if (Sprite != null && FrameAni != null)
        {
            FrameAni.Clear();
            Sprite.getAtlas = null;
            if (mIsHasShoadow) shadowData.ShadowSprite.getAtlas = null;
        }
    }

    private void OnDestroy()
    {
        if (Sprite != null)
        {
            Sprite.Destroy();
        }
        if (shadowData != null)
        {
            shadowData.ShadowSprite.Destroy();
        }
    }

    #region ISFSpriteAnimation
    public IBaseFrame getFrameAni
    {
        get { return FrameAni; }
        set { FrameAni = value; }
    }

    public CSSpriteBase getSprite
    {
        get { return Sprite; }
        set { Sprite = value; }
    }

    public ShadowData ShadowData
    {
        get { return shadowData; }
        set { shadowData = value; }
    }

    public void UpdateAnimationsNames(int curDirection, bool isReset = false)
    {
        if (FrameAni != null)
        {
            Vector3 vec = new Vector3(-1f, 1f, 1f);
            if (mIsHasShoadow) shadowData.ShadowSprite.isNegative = true;
            if (curDirection == (int)CSDirection.Left_Up)
            {
                curDirection = (int)CSDirection.Right_Up;
            }
            else if (curDirection == (int)CSDirection.Left)
            {
                curDirection = (int)CSDirection.Right;
            }
            else if (curDirection == (int)CSDirection.Left_Down)
            {
                curDirection = (int)CSDirection.Right_Down;
            }
            else
            {
                vec = new Vector3(1f, 1f, 1f);

                if (mIsHasShoadow) shadowData.ShadowSprite.isNegative = false;
            }
            //CahcheTrans.localScale = vec;
            Sprite.ApplyScaleTrans(CahcheTrans, vec);
            FrameAni.RefreshNames();
        }
    }

#endregion
}                                                                                                                                                                                                                             