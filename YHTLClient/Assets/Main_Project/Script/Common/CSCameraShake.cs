using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCameraShake : MonoBehaviour
{
    // 抖动目标的transform(若未添加引用，怎默认为当前物体的transform)
    public Transform camTransform;

    //开始抖动倒计时
    private float shakeDelay;

    //持续抖动的时长
    public float shakeDuration = 0f;

    // 抖动幅度（振幅）振幅越大抖动越厉害
    public float shakeRange = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDelay > 0)
        {
            shakeDelay -= Time.deltaTime;
            return;
        }

        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeRange;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    //public void ResetShake()
    //{
    //    shakeDuration = 0f;
    //}

    public void SetShakeInfo(float shakeDelay,float shakeDuration,float shakeRange)
    {
        if (shakeDelay == 0)//如果为0，立即抖动，不用等一帧
            shakeDelay = -1;
        this.shakeDelay = shakeDelay;
        this.shakeRange = shakeRange;
        this.shakeDuration = shakeDuration;
    }
}
