using UnityEngine;

/// <summary>
/// Extend for NGUI UIPlaySound
/// </summary>
public class UIPlaySoundExtend : MonoBehaviour
{
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnEnable,
        OnDisable,
    }

    [SerializeField]
    public int ButtonType = 2022;
    
    [SerializeField]
    public Trigger trigger = Trigger.OnClick;

    bool mIsOver = false;

    private UIButton btn;
    
    bool CanPlay
    {
        get
        {
            if (!enabled) { return false; }
            return (btn == null || btn.isEnabled);
        }
    }

    private void Awake()
    {
        btn = GetComponent<UIButton>();
    }

    void OnEnable()
    {
        if (trigger == Trigger.OnEnable)
        {
            PlayAudio();
        }

    }

    void OnDisable()
    {
        if (trigger == Trigger.OnDisable)
        {
            PlayAudio();
        }
    }

    void OnHover(bool isOver)
    {
        if (trigger == Trigger.OnMouseOver)
        {
            if (mIsOver == isOver)
            {
                return;
            }

            mIsOver = isOver;
        }

        if (CanPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
        {
            PlayAudio();
        }
    }

    void OnPress(bool isPressed)
    {
        if (trigger == Trigger.OnPress)
        {
            if (mIsOver == isPressed)
            {
                return;
            }
            mIsOver = isPressed;
        }

        if (CanPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
        {
            PlayAudio();
        }
    }

    void OnClick()
    {
        if (CanPlay && trigger == Trigger.OnClick)
        {
            PlayAudio();
        }
    }
    
    private void PlayAudio()
    {
        HotFix_InvokeThird.UIPlaySoundInCSAudio(ButtonType);
    }
}