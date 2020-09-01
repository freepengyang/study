using UnityEngine;
using System.Collections;
using System;

public class UIActorHurtEffect : MonoBehaviour
{
    private CSAtlasLabel mHurtLb;
    private bool mIsAvtive = false;
    private int mCount = 0;
    private int mDelay;
    private float mBeginTime = 0.0f;
    private float mAlpha = 1f;
    private Color mColor = new Color(1f, 1f, 1f, 1f);
    private float mScale = 0.4f;
    private bool mIsNumber;

    private bool mIsDelayPlay;

    public static Vector3 Vector3Right = new Vector3(1.2f, 1, 0);
    public static Vector3 Vector3Left = new Vector3(-1.2f, 1, 0);
    public static Vector3 Vector3OffestY = new Vector3(0, 20, 0);
    public static Vector3 Vector3OffestXY = new Vector3(-60, 20, 0);


    private Vector3 mVector3Move = Vector3.one;
    public bool IsNumber
    {
        get
        {
            return mIsNumber;
        }
        set
        {
            mIsNumber = value;
        }
    }
    public bool IsAvtive()
    {
        return (mIsAvtive || mIsDelayPlay);
    }

    public CSAtlasLabel hurtLb
    {
        get
        {
            if (mHurtLb == null)
            {
                Transform trans = transform.Find("label");
                if (trans != null)
                {
                    mHurtLb = trans.GetComponent<CSAtlasLabel>();
                }
            }
            return mHurtLb;
        }
    }

    public bool IsDelayPlay()
    {
        return mIsDelayPlay;
    }

    public void Init(int dirction,float delayTime, int value)
    {
        IsNumber = UtilityMain.IsNumber(mHurtLb.TextType);
        mBeginTime = Time.time + delayTime;
        mAlpha = 1f;
        mIsDelayPlay = true;
        gameObject.SetActive(true);
        SetColor(0);
        //mIsAvtive = true;
        mDelay = 0;
        mCount = 0;
        mVector3Move = (dirction == EFlyDirection.Right) ? Vector3Right : Vector3Left;
        SetScale(0.4f);
        SetPosition(value);
    }

    public void SetPosition(int value)
    {
        //transform.localPosition = (IsNumber) ? Vector3OffestY : Vector3OffestXY;
        if(IsNumber)
        {
            transform.localPosition = Vector3OffestY;
        }
        else
        {
            float x = (value == 0) ? (-40) : (int)(Math.Log10(Math.Abs(value)) + 1) * (-12) - 36;
            transform.localPosition = new Vector3(x,20,0);
        }
        //transform.localPosition = (IsNumber) ? Vector3OffestY : Vector3OffestXY;
    }


    void Update()
    {
        if (mIsDelayPlay)
        {
            if (mBeginTime <= Time.time)
            {
                mIsAvtive = true;
                SetColor(1);
                mIsDelayPlay = false;
            }
            return;
        }
        if (!mIsAvtive) return;

        if (Time.time - mBeginTime > 1.5f)
        {
            gameObject.SetActive(false);
            mIsAvtive = false;
            mIsDelayPlay = false;
        }
        //transform.Translate(Vector3.up * Time.deltaTime * 0.15f);
        mDelay++;

        if(mDelay <= 6)
        {
            mScale += 0.27f;
        }
        else if(mDelay <= 10)
        {
            mScale -= 0.2f;
            mScale = (mScale < 1) ? 1.0f : mScale;
        }
        SetScale(mScale);

        if (mDelay >= 25)//0.5s后开始淡入淡出
        {
            //transform.Translate(Vector3.up * Time.deltaTime * 0.15f);
            transform.Translate(mVector3Move * Time.deltaTime * 0.25f);
            mCount++;
            if (mCount >= 2)
            {
                mAlpha -= 0.05f;
                mCount = 0;
                if (mAlpha >= 0)
                {
                    SetColor(mAlpha);
                }
            }
        }
    }

    private void SetColor(float a)
    {
        mColor.r = 1f;
        mColor.g = 1f;
        mColor.b = 1f;
        mColor.a = a;
        hurtLb.SetColor(mColor);
    }

    private void SetScale(float scale)
    {
        mScale = scale;
        transform.localScale = Vector3.one * mScale;
    }

    void OnDestroy()
    {
        mIsAvtive = false;
        CSActorHurtEffect.hurtCloneList.Remove(this);
    }
}
