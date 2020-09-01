//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Similar to UIButtonColor, but adds a 'disabled' state based on whether the collider is enabled or not.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	/// <summary>
	/// Current button that sent out the onClick event.
	/// </summary>

	static public UIButton current;

	/// <summary>
	/// Whether the button will highlight when you drag something over it.
	/// </summary>

	public bool dragHighlight = false;

	/// <summary>
	/// Name of the hover state sprite.
	/// </summary>

	public string hoverSprite;

	/// <summary>
	/// Name of the pressed sprite.
	/// </summary>

	public string pressedSprite;

	/// <summary>
	/// Name of the disabled sprite.
	/// </summary>

	public string disabledSprite;

	/// <summary>
	/// Name of the hover state sprite.
	/// </summary>

	public UnityEngine.Sprite hoverSprite2D;

	/// <summary>
	/// Name of the pressed sprite.
	/// </summary>

	public UnityEngine.Sprite pressedSprite2D;

	/// <summary>
	/// Name of the disabled sprite.
	/// </summary>

	public UnityEngine.Sprite disabledSprite2D;

	/// <summary>
	/// Whether the sprite changes will elicit a call to MakePixelPerfect() or not.
	/// </summary>

	public bool pixelSnap = false;

	/// <summary>
	/// Click event listener.
	/// </summary>

	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Cached value
	[System.NonSerialized] UISprite mSprite;
	[System.NonSerialized] UI2DSprite mSprite2D;
	[System.NonSerialized] string mNormalSprite;
	[System.NonSerialized] UnityEngine.Sprite mNormalSprite2D;

	/// <summary>
	/// Whether the button should be enabled.
	/// </summary>

	public override bool isEnabled
	{
		get
		{
			if (!enabled) return false;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			Collider col = collider;
#else
			Collider col = gameObject.GetComponent<Collider>();
#endif
			if (col && col.enabled) return true;
			Collider2D c2d = GetComponent<Collider2D>();
			return (c2d && c2d.enabled);
		}
		set
		{
			if (isEnabled != value)
			{
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				Collider col = collider;
#else
				Collider col = gameObject.GetComponent<Collider>();
#endif
				if (col != null)
				{
					col.enabled = value;
					UIButton[] buttons = GetComponents<UIButton>();
					foreach (UIButton btn in buttons) btn.SetState(value ? State.Normal : State.Disabled, false);
				}
				else
				{
					Collider2D c2d = GetComponent<Collider2D>();

					if (c2d != null)
					{
						c2d.enabled = value;
						UIButton[] buttons = GetComponents<UIButton>();
						foreach (UIButton btn in buttons) btn.SetState(value ? State.Normal : State.Disabled, false);
					}
					else enabled = value;
				}
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the normal sprite.
	/// </summary>

	public string normalSprite
	{
		get
		{
			if (!mInitDone) OnInit();
			return mNormalSprite;
		}
		set
		{
			if (!mInitDone) OnInit();
			if (mSprite != null && !string.IsNullOrEmpty(mNormalSprite) && mNormalSprite == mSprite.spriteName)
			{
				mNormalSprite = value;
				SetSprite(value);
				NGUITools.SetDirty(mSprite);
			}
			else
			{
				mNormalSprite = value;
				if (mState == State.Normal) SetSprite(value);
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the normal sprite.
	/// </summary>

	public UnityEngine.Sprite normalSprite2D
	{
		get
		{
			if (!mInitDone) OnInit();
			return mNormalSprite2D;
		}
		set
		{
			if (!mInitDone) OnInit();
			if (mSprite2D != null && mNormalSprite2D == mSprite2D.sprite2D)
			{
				mNormalSprite2D = value;
				SetSprite(value);
				NGUITools.SetDirty(mSprite);
			}
			else
			{
				mNormalSprite2D = value;
				if (mState == State.Normal) SetSprite(value);
			}
		}
	}
    void Awake() 
    { 
        ButtonName = GetButtonName() == null ? "" : GetButtonName().text; 
    }
	/// <summary>
	/// Cache the sprite we'll be working with.
	/// </summary>

	protected override void OnInit ()
	{
		base.OnInit();
        UIButtonScale scale = gameObject.GetComponent<UIButtonScale>();
        if(scale == null)scale = gameObject.AddComponent<UIButtonScale>();
		mSprite = (mWidget as UISprite);
		mSprite2D = (mWidget as UI2DSprite);
		if (mSprite != null) mNormalSprite = mSprite.spriteName;
		if (mSprite2D != null) mNormalSprite2D = mSprite2D.sprite2D;
	}

	/// <summary>
	/// Set the initial state.
	/// </summary>

	protected override void OnEnable ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			mInitDone = false;
			return;
		}
#endif
		if (isEnabled)
		{
			if (mInitDone) OnHover(UICamera.hoveredObject == gameObject);
		}
		else SetState(State.Disabled, true);
        
        SetNormalState();
	}

	/// <summary>
	/// Drag over state logic is a bit different for the button.
	/// </summary>

	protected override void OnDragOver ()
	{
		if (isEnabled && (dragHighlight || UICamera.currentTouch.pressed == gameObject))
			base.OnDragOver();
	}

	/// <summary>
	/// Drag out state logic is a bit different for the button.
	/// </summary>

	protected override void OnDragOut ()
	{
		if (isEnabled && (dragHighlight || UICamera.currentTouch.pressed == gameObject))
			base.OnDragOut();
	}

	/// <summary>
	/// Call the listener function.
	/// </summary>

	protected virtual void OnClick ()
	{
		if (current == null && isEnabled)
		{
			current = this;
			EventDelegate.Execute(onClick);
			current = null;

            SetDisabled();
		}
	}

	/// <summary>
	/// Change the visual state.
	/// </summary>

	public override void SetState (State state, bool immediate)
	{
		base.SetState(state, immediate);

		if (mSprite != null)
		{
			switch (state)
			{
				case State.Normal: SetSprite(mNormalSprite); break;
				case State.Hover: SetSprite(string.IsNullOrEmpty(hoverSprite) ? mNormalSprite : hoverSprite); break;
				case State.Pressed: SetSprite(pressedSprite); break;
				case State.Disabled: SetSprite(disabledSprite); break;
			}
		}
		else if (mSprite2D != null)
		{
			switch (state)
			{
				case State.Normal: SetSprite(mNormalSprite2D); break;
				case State.Hover: SetSprite(hoverSprite2D == null ? mNormalSprite2D : hoverSprite2D); break;
				case State.Pressed: SetSprite(pressedSprite2D); break;
				case State.Disabled: SetSprite(disabledSprite2D); break;
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the sprite.
	/// </summary>

	protected void SetSprite (string sp)
	{
		if (mSprite != null && !string.IsNullOrEmpty(sp) && mSprite.spriteName != sp)
		{
			mSprite.spriteName = sp;
			if (pixelSnap) mSprite.MakePixelPerfect();
		}
	}

	/// <summary>
	/// Convenience function that changes the sprite.
	/// </summary>

	protected void SetSprite (UnityEngine.Sprite sp)
	{
		if (sp != null && mSprite2D != null && mSprite2D.sprite2D != sp)
		{
			mSprite2D.sprite2D = sp;
			if (pixelSnap) mSprite2D.MakePixelPerfect();
        }
    }

    #region  禁用

    public int DisabledTime = 0;
    private int mDisabledTime;
    private string ButtonName;

    private void SetDisabled()
    {
        if (DisabledTime > 0)
        {
            mDisabledTime = DisabledTime;
            SetSprite(disabledSprite);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(UpdateDisabledTime());
        }
    }

    IEnumerator UpdateDisabledTime()
    {
        UILabel lb = GetButtonName();
        lb.text = mDisabledTime.ToString();
        mDisabledTime--;
        yield return new WaitForSeconds(1.0f);
        if (mDisabledTime > 0)
        {
            StartCoroutine(UpdateDisabledTime());
        }
        else
        {
            SetNormalState();
        }
    }

    private void SetNormalState()
    {
        if (DisabledTime > 0)
        {
            GetButtonName().text = ButtonName;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            SetSprite(normalSprite);
        }
    }

    private UILabel GetButtonName()
    {
        UILabel lb = gameObject.GetComponentInChildren<UILabel>();
        return lb;
    }

    public void SetDisabled(bool isDisabled,string txt)
    {
        if (isDisabled)
        {
            SetSprite(disabledSprite);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GetButtonName().text = txt;
        }
        else
        {
            SetSprite(normalSprite);
            gameObject.GetComponent<BoxCollider>().enabled = true;
            GetButtonName().text = ButtonName;
        }
    }

    #endregion
}
