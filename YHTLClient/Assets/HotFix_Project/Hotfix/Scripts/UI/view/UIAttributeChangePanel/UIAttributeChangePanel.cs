using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;
using user;

public partial class UIAttributeChangePanel : UIBasePanel
{
    public override UILayerType PanelLayerType => UILayerType.Tips;

    public override bool ShowGaussianBlur => false;

    private readonly int LIMIT_DATA_COUT = 3;

    readonly WaitForSeconds wait1 = new WaitForSeconds(0.1f);
    readonly WaitForSeconds wait2 = new WaitForSeconds(0.2f);
    readonly WaitForSeconds wait5 = new WaitForSeconds(0.5f);

    Queue<RepeatedField<CSAttributeInfo.KeyValue>> mQueue = new Queue<RepeatedField<CSAttributeInfo.KeyValue>>();
    bool IsHoldOn = false;
    bool IsPlaying = false;

    private RepeatedField<CSAttributeInfo.KeyValue> mHoldOnValue;

    private Coroutine _ShowCoroutine;
    private Coroutine _ShowCoroutine2;

    public override void Init()
    {
        base.Init();

        HotManager.Instance.EventHandler.AddEvent(CEvent.OnPlayFightPowerChanged, OnPlayFightPowerChanged);
    }

    void OnPlayFightPowerChanged(uint id,object argv)
    {
        if(argv is int[] arr)
        {
            PlayFight(arr[0], arr[1]);
        }
    }

    void PlayFight(int from, int to)
    {
        ScriptBinder.StopInvokeRepeating();
        mrolefight_down.gameObject.SetActive(false);
        ScriptBinder.StopInvoke();
        mOffsetFightValue = 0;
        mTargetFightValue = 0;
        mOldFightValue = 0;
        mCurFightValue = 0;

        mLastFightValue = from;// CSMainPlayerInfo.Instance.OldFightPower;
        mTargetFightValue = to;// CSMainPlayerInfo.Instance.fightPower;
        RefreshFight();
    }

    public void Show(int from,int to)
    {
        PlayFight(from, to);

        if (CSMainPlayerInfo.Instance.GetChangeAttrKey().Count == 0) return;

        if (mQueue.Count >= LIMIT_DATA_COUT)
        {
            IsHoldOn = true;
        }
        else
        {
            IsHoldOn = false;
        }

        mHoldOnValue = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, CSMainPlayerInfo.Instance.GetChangeAttrKey(), CSMainPlayerInfo.Instance.GetChangeAttrValue());

        if (!IsHoldOn)
        {
            Refresh();
        }
    }

    int mTargetFightValue = 0;
    int mOldFightValue = 0;
    int mCurFightValue = 0;
    int mLastFightValue = 0;
    int mOffsetFightValue = 0;
    float mFightTimer = 0f;
    float mUpdateTime = 1.5f;
    float mUpdateDeltaTime = 0.06f;
    float mDelayHideTime = 0.3f;

    public override bool Cached
    {
        get { return true; }
    }

    private void Refresh()
    {
        if (mHoldOnValue != null && mHoldOnValue.Count != 0)
            mQueue.Enqueue(mHoldOnValue);

        if (!IsPlaying)
        {
            CheckPlayQueue();
        }
    }

    private void RefreshFight()
    {
        //mTargetFightValue = CSMainPlayerInfo.Instance.fightPower;
        PlayFightAnim();
    }

    #region FightPower

    void PlayFightAnim()
    {
        if (mOldFightValue == mTargetFightValue) return;

        mOldFightValue = mTargetFightValue;
        mOffsetFightValue += mTargetFightValue - mLastFightValue;
        mFightTimer = 0;
        if (mOffsetFightValue > 0)
        {
            if (!mrolefight_down.gameObject.activeSelf)
            {
                mrolefight_down.gameObject.SetActive(true);
            }

            CSEffectPlayMgr.Instance.ShowUIEffect(mobj_fightPowerEff, 17142);
            ScriptBinder.StopInvokeRepeating();
            ScriptBinder.InvokeRepeating(0, mUpdateDeltaTime, UpdateFightValue);
        }
        else
        {
            mLastFightValue = mCurFightValue = mTargetFightValue;
            mOffsetFightValue = 0;

            ScriptBinder.StopInvokeRepeating();
            mrolefight_down.gameObject.SetActive(false);
        }
    }

    void UpdateFightValue()
    {
        mfight_part.SetActive(true);
        mnum.gameObject.SetActive(false);
        msubnum.gameObject.SetActive(false);
        mFightTimer += mUpdateDeltaTime;
        if (Mathf.Abs(mOffsetFightValue) > 9)
        {
            mCurFightValue = mLastFightValue + (int) ((mOffsetFightValue) * mFightTimer / mUpdateTime);
        }
        else
        {
            if (mLastFightValue / 10 == mTargetFightValue / 10)
            {
                mCurFightValue = (mTargetFightValue / 10) * 10 + UnityEngine.Random.Range(0, 10);
            }
            else
            {
                mCurFightValue = (mTargetFightValue / 100) * 100 + UnityEngine.Random.Range(10, 100);
            }
        }

        mrolefight_down.text = ((int) Mathf.Lerp(mTargetFightValue - mOffsetFightValue,
            mTargetFightValue, mFightTimer / mUpdateTime)).ToString();

        if (mOffsetFightValue > 0)
        {
            mnum.CustomActive(true);
            mnum.text = "+" + mOffsetFightValue.ToString();
        }
        else
        {
            msubnum.CustomActive(true);
            msubnum.text = mOffsetFightValue.ToString();
        }
        if(mFightTimer >= mUpdateTime)
        {
            mLastFightValue = mCurFightValue = mTargetFightValue;
            mFightTimer = 0;
            mOffsetFightValue = 0;
            ScriptBinder.StopInvokeRepeating();
            ScriptBinder.Invoke(mDelayHideTime, DelayHide);
        }
    }
    void DelayHide()
    {
        mfight_part.SetActive(false);
        ScriptBinder.StopInvoke();
    }
    #endregion

    void CheckPlayQueue()
    {
        IsPlaying = false;
        if (mQueue == null) mQueue = new Queue<RepeatedField<CSAttributeInfo.KeyValue>>();
        if (mQueue.Count > 0)
        {
            _ShowCoroutine = CoroutineManager.DoCoroutine(RefreshAttributeList(mQueue.Dequeue()));
            CheckHoldOn();
        }
        else
            _ShowCoroutine2 = CoroutineManager.DoCoroutine(DelayDestroy());
    }

    private IEnumerator RefreshAttributeList(RepeatedField<CSAttributeInfo.KeyValue> values)
    {
        IsPlaying = true;

        if (values.Count <= 0)
        {
            CheckPlayQueue();
            yield break;
        }

        mchangeValues.MaxCount = 0;
        mchangeValues.Bind<CSAttributeInfo.KeyValue, AtttrbuteChangeItem>(values, mPoolHandleManager);

        for (int i = 0; i < mchangeValues.MaxCount; i++)
        {
            mchangeValues.controlList[i].gameObject.SetActive(true);
            yield return wait1;
        }

        for (int j = 0; j < mchangeValues.controlList.Count; j++)
        {
            UIPlayTween go = mchangeValues.controlList[j].GetComponent<UIPlayTween>();
            yield return wait5;
            go.resetOnPlay = true;
            go.Play(true);
        }

        yield return wait2;
        CheckPlayQueue();
    }

    void CheckHoldOn()
    {
        if (IsHoldOn && mQueue.Count < LIMIT_DATA_COUT)
        {
            Refresh();
            IsHoldOn = !IsHoldOn;
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1.2f);
        if (!IsPlaying)
            UIManager.Instance.ClosePanel<UIAttributeChangePanel>();
    }

    protected override void OnDestroy()
    {
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.OnPlayFightPowerChanged, OnPlayFightPowerChanged);
        CSEffectPlayMgr.Instance.Recycle(mobj_fightPowerEff);
        mQueue = null;
        mHoldOnValue = null;
        mchangeValues.UnBind<AtttrbuteChangeItem>();
        if (_ShowCoroutine != null)
            CoroutineManager.StopCoroutine(_ShowCoroutine);
        if (_ShowCoroutine2 != null)
            CoroutineManager.StopCoroutine(_ShowCoroutine2);
        base.OnDestroy();
    }
}

public class AtttrbuteChangeItem : UIBinder
{
    UILabel lb_name;
    UILabel lb_value;
    private TweenPosition _tweenPosition;
    private Vector3 toPos;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_value = Get<UILabel>("lb_value");
        _tweenPosition = handle.transform.GetComponent<TweenPosition>();
        toPos = new Vector3(150, 0, 0);
    }

    public override void Bind(object data)
    {
        Handle.gameObject.SetActive(false);

        CSAttributeInfo.KeyValue attrData = data as CSAttributeInfo.KeyValue;
        if (attrData == null) return;
        if (lb_name != null)
        {
            lb_name.alpha = 1f;
            lb_name.text = attrData.Key;
        }

        if (lb_value != null)
        {
            lb_value.text = $"+{attrData.Value}";
            lb_value.alpha = 1f;
        }


        _tweenPosition.from = _tweenPosition.value;
        toPos.y = Handle.transform.localPosition.y + 50;
        _tweenPosition.to = toPos;
    }

    public override void OnDestroy()
    {
        lb_name = null;
        lb_value = null;
    }
}