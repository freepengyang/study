using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillFlagItem 
{
    public const float Z_ORDER = 10001;
    public CSSceneEffect rangeEffect;
    private CSSprite sprRange;
    private int direction;
    private int lastDirection;
    private float scale;
    private float lastScale;
    public float attackWidth;
    public float attackHeight;
    public bool isCancle;
    public bool isHaveTarget;
    public Vector3 initPosition;
    private bool isInitialized;
    private bool isActive;
    private float zOrder;

    public void Hide()
    {
        if (rangeEffect != null)
        {
            rangeEffect.SetAvtive(false);
        }
        isActive = false;
    }

    public void Show(int effectId,float scale,Vector3 vec3, float zOrder)
    {
        initPosition = (Mathf.Abs(vec3.x) <= attackWidth && Mathf.Abs(vec3.y) <= attackHeight) ? vec3 : Vector3.zero;

        //if((Mathf.Abs(vec3.x) <= attackWidth) && (Mathf.Abs(vec3.y) <= attackHeight))
        //{
        //    initPosition = new Vector3(vec3.x,vec3.y, Z_ORDER);
        //}
        //else
        //{
        //    initPosition = new Vector3(0,0, Z_ORDER);
        //}
        this.scale = scale;
        this.zOrder = zOrder;
        isInitialized = false;
        isCancle = false;
        isActive = true;
        if (rangeEffect != null)
        {
            rangeEffect.SetAvtive(true);
            SetColor();
        }
        else
        {
            Transform anchor =CSAvatarManager.MainPlayer.Model.Effect.GoTrans;
            rangeEffect = CSSceneEffectMgr.Instance.Create(anchor, effectId, Vector3.zero, OnLoadedCallBack);
            if(!isInitialized)
            {
                OnLoadedCallBack();
            }
        }
        initPosition.z = zOrder;
        SetLocalPosition();
        SetLocalScale();
        SetRotation();
    }

    public void InitPosition(Vector3 pos)
    {
        if (rangeEffect != null && rangeEffect.root != null)
        {
            rangeEffect.root.localPosition = pos;
        }
    }

    public void SetMoveRange(float radious)
    {
        attackWidth = radious * CSCell.Size.x;
        attackHeight = radious * CSCell.Size.y;
    }

    public void SetDirection(int dir)
    {
        if(direction != dir)
        {
            direction = dir;
        }
    }

    public void SetLastDirection(int dir)
    {
        if(lastDirection != dir)
        {
            lastDirection = dir;
        }
    }

    public void SetLocalPosition()
    {
        if (rangeEffect != null && rangeEffect.root != null)
        {
            rangeEffect.root.localPosition = initPosition;
        }
    }

    public void SetLocalScale()
    {
        if (lastScale != this.scale)
        {
            if (rangeEffect != null && rangeEffect.root != null)
            {
                rangeEffect.root.localScale = new Vector3(scale, scale, scale);
                lastScale = scale;
            }
        }
    }

    public void SetRotation(int dir)
    {
        if(dir != this.direction)
        {
            direction = dir;
            SetRotation();
        }
    }

    private void SetRotation()
    {
        if(lastDirection != direction)
        {
            if (rangeEffect != null && rangeEffect.root != null)
            {
                //Vector3 v = targetPos - animation.CahcheTrans.position;
                //float zAngle = Mathf.Atan2(60, 40) * Mathf.Rad2Deg;
                float zAngle = getArrowAngle(direction);
                //cacheTrans.rotation = Quaternion.Euler(0, 0, Info.RotationDirction * 45 + zAngle);
                rangeEffect.root.transform.rotation = Quaternion.Euler(0, 0, zAngle);
                lastDirection = direction;
            }
        }
        //float angle = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg;

    }

    public void SetTouchPos(Vector3 mouseLocalPos)
    {
        if (rangeEffect != null && rangeEffect.root != null)
        {
            mouseLocalPos.z = 0;
            if (mouseLocalPos.magnitude > attackWidth)
            {
                Vector3 w_v3 = mouseLocalPos.normalized * attackWidth;
                Vector3 h_v3 = mouseLocalPos.normalized * attackHeight;
                rangeEffect.root.localPosition = new Vector3(w_v3.x, h_v3.y, zOrder);
            }
            else if (mouseLocalPos.magnitude > attackHeight)
            {
                Vector3 h_v3 = mouseLocalPos.normalized * attackHeight;
                rangeEffect.root.localPosition = new Vector3(mouseLocalPos.x, h_v3.y, zOrder);
            }
            else
            {
                rangeEffect.root.localPosition = new Vector3(mouseLocalPos.x, mouseLocalPos.y, zOrder);
            }
            //Debug.LogFormat("<color=#00ff00> localPosition = {0}   mouseLocalPos.magnitude  = {1} </color>", rangeEffect.root.localPosition, mouseLocalPos.magnitude);

            bool isCancle = (mouseLocalPos.magnitude >= attackWidth + 10);
            SetCancle(isCancle);

           // Debug.LogFormat("<color=#00ff00>mouseLocalPos.magnitude  = {0}   attackWidth = {1}  isCancle = {2} isHaveTarget = {3}</color>", mouseLocalPos.magnitude, attackWidth, isCancle, isHaveTarget);

            //Debug.LogError("SetTouchPos = " + rangeEffect.root.localPosition + " initPosition = " + initPosition);

        }
    }

    public void SetCancle(bool value)
    {
        if (isCancle != value)
        {
            isCancle = value;
            SetColor();
            //Color color = isCancle ? Color.red : Color.green;
            //SetColor(color);
        }
    }

    public void SetTarget(bool value)
    {
        if(isHaveTarget != value)
        {
            isHaveTarget = value;
            SetColor();
            //Debug.LogErrorFormat("<color=#ff0000> isHaveTarget = {0}</color>", isHaveTarget);
            //Color color = isHaveTarget ? Color.green : Color.red;
            //SetColor(color);
        }
    }

    public void SetColor()
    {
        Color color = isCancle ? Color.red : (isHaveTarget ? Color.green : Color.red);
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        if (rangeEffect != null && sprRange != null)
        {
            sprRange.SetShader(CSShaderManager.GetShareMaterial(sprRange.Atlas, EShareMatType.Transparent), color, Vector4.one);
        }
    }

    public bool GetCancle(Vector3 localPosition)
    {
        return (localPosition.magnitude >= attackWidth + 10);
    }

    private void OnLoadedCallBack()
    {
        if (rangeEffect != null)
        {
            if (rangeEffect.root != null)
            {
                isInitialized = true;
                sprRange = rangeEffect.Sprite;
                //SetColor(Color.green);
                SetColor();
                SetRotation();
                SetLocalScale();
                SetLocalPosition();
                rangeEffect.SetAvtive(isActive);
            }
        }
    }

    public static float getArrowAngle(int direction)
    {
        float angle = -90f;
        switch (direction)
        {
            case CSDirection.Up:
                angle = -90f;
                break;
            case CSDirection.Right_Up:
                angle = -146.3f;
                break;
            case CSDirection.Right:
                angle = 180f;
                break;
            case CSDirection.Right_Down:
                angle = 146.3f;
                break;
            case CSDirection.Down:
                angle = 90.0f;
                break;
            case CSDirection.Left_Down:
                angle = 33.7f;
                break;
            case CSDirection.Left:
                angle = 0f;
                break;
            case CSDirection.Left_Up:
                angle = -33.7f;
                break;
        }
        return angle;
    }

    public void Destroy()
    {
        if (rangeEffect != null)
        {
            rangeEffect.Destroy();
            rangeEffect = null;
        }
        sprRange = null;
        isInitialized = false;
    }

}
