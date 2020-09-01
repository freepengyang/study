using UnityEngine;
using System.Collections;
using System.Reflection.Emit;
public class UIJoystickPanel : UIBase
{
    private Transform mJoystick;
    UIWidget mJoystickWidget;
    private Transform mTouch;
    private UISprite mArea;
    private UIWidget mZone;
    private Vector3 mMouseLocalPos;
    private Vector3 mCurObjPos;
    private GameObject mount;
    private float mAreaRadius;
    private bool mFixJoystick = false;
	private GameObject obj_lock;
    
	public bool FixJoystick
    { 
        get
        { 
            return mFixJoystick;
        } 
        set 
        { 
            mFixJoystick = value;
        } 
    }
    private Vector3 mJoystickDefaultLocalPosition;

    int mLastDirection = CSDirection.None;
    public int LastDirection
    {
        get { return mLastDirection; }
        set
        {
            mLastDirection = value;
        }
    }

    private float mPixelSizeAdjustment = 0;
    private float PixelSizeAdjustment 
    {
        get
        {
            if (mPixelSizeAdjustment == 0)
            {
                mPixelSizeAdjustment = NGUITools.FindInParents<UIRoot>(UIPrefab).pixelSizeAdjustment;
            }
            return mPixelSizeAdjustment;
        }
    }

    public void Reset()
    {
        LastDirection = CSDirection.None;
    }

    public override void Init()
    {
        mJoystick = UIPrefabTrans.Find("joystick");
        mJoystickWidget = mJoystick.GetComponent<UIWidget>();
        mTouch = UIPrefabTrans.Find("joystick/touch");
        mArea = Get<UISprite>("joystick/area");
        mZone = Get<UIWidget>("zone");
        mount = mTouch.transform.Find("Sprite").gameObject;
		obj_lock = UIPrefabTrans.Find("locked").gameObject;
        mClientEvent.AddEvent(CEvent.UIJoystick_Reset,OnReset);
        mClientEvent.AddEvent(CEvent.UIJoystick_Move, OnMove);
        mClientEvent.AddEvent(CEvent.UIJoystick_Stop, OnStop);
        mClientEvent.AddEvent(CEvent.Scene_EnterSceneAfter, Scene_EnterSceneAfter);
		mClientEvent.AddEvent(CEvent.GetRandomThingInfo, ShowLockImg);
        mClientEvent.AddEvent(CEvent.Setting_FixJoystickChange, FixJoystickSettingChange);
        UtilityFight.InitDirThresholds();
    }

    public override void Show()
    {
        mJoystick.gameObject.SetActive(true);
        mJoystickWidget.alpha = 0.4f;
        CoroutineManager.DoCoroutine(SaveDefaultPos());
        FixJoystick =   CSConfigInfo.Instance.GetBool(ConfigOption.FixJoystick);
        mAreaRadius = mArea.width / 2;
        UIEventListener.Get(mZone.gameObject).onPress = OnPressTouch;
        UIEventListener.Get(mZone.gameObject).onDrag = OnDragTouch;
        HotMain.UpdateCallBack += Update;
    }

    bool mHasSaveDefaultPos = false;
    IEnumerator SaveDefaultPos()
    {
        yield return null;
        mJoystickDefaultLocalPosition = mJoystick.localPosition;
        mCurObjPos = UIPrefabTrans.localPosition;
        mHasSaveDefaultPos = true;
    }

    Vector3 mKeyOffset = Vector3.zero;
    Vector3 mLastKeyOffest = Vector3.zero;
    void Update()
    {
        if (!CSScene.IsLanuchMainPlayer)
            return;
        /*if (CSInput.JoystickKeyDownA)
        {
            PressSkillShortcut(0, true);
        }

        if (CSInput.JoystickKeyUpA)
        {
            PressSkillShortcut(0, false);
        }

        if (CSInput.JoystickKeyDownB)
        {
            PressSkillShortcut(1, true);
        }

        if (CSInput.JoystickKeyUpB)
        {
            PressSkillShortcut(1, false);
        }

        if (CSInput.JoystickKeyDownX)
        {
            PressSkillShortcut(3, true);
        }

        if (CSInput.JoystickKeyUpX)
        {
            PressSkillShortcut(3, false);
        }

        if (CSInput.JoystickKeyDownY)
        {
            PressSkillShortcut(2, true);
        }

        if (CSInput.JoystickKeyUpY)
        {
            PressSkillShortcut(2, false);
        }

        if (CSInput.JoystickKeyDownRB)
        {
            PressSkillShortcut(4, true);
        }

        if (CSInput.JoystickKeyUpRB)
        {
            PressSkillShortcut(4, false);
        }

        if (CSInput.JoystickKeyDownLB)
        {
            PressSkillShortcut(5, true);
        }

        if (CSInput.JoystickKeyUpLB)
        {
            PressSkillShortcut(5, false);
        }

        if (CSInput.JoystickKeyDownRT)
        {
            PressSkillShortcut(6, true);
        }

        if (CSInput.JoystickKeyUpRT)
        {
            PressSkillShortcut(6, false);
        }

        if (CSInput.JoystickKeyDownLT)
        {
            PressSkillShortcut(7, true);
        }

        if (CSInput.JoystickKeyUpLT)
        {
            PressSkillShortcut(7, false);
        }

        if(CSInput.JoystickKeyUpBack)
        {
            //UIManager.Instance.CloseTopPanel();
        }*/

        mKeyOffset.y = CSInput.LeftJoystickY;
        mKeyOffset.x = CSInput.LeftJoystickX;

        if (mKeyOffset != Vector3.zero)
        {
            if (mLastKeyOffest == Vector3.zero)
            {
                if(!Raycast(mJoystick.position, mZone.gameObject))
                    return;
                OnPressTouch(true, (mJoystickDefaultLocalPosition) / PixelSizeAdjustment);
            }

            if (mKeyOffset != mLastKeyOffest)
            {
                OnDragTouch(null, (mKeyOffset - mLastKeyOffest) * mAreaRadius / PixelSizeAdjustment);
                mLastKeyOffest = mKeyOffset;
            }
        }
        else if (mLastKeyOffest != Vector3.zero)
        {
            StopMove();
        }

        if (CSScene.IsLanuchMainPlayer &&CSAvatarManager.MainPlayer.TouchMove == EControlState.JoyStick)
        {
            Move(UtilityFight.GetDirection(mTouch.localPosition));
        }
    }

    private UICamera mUICam;
    public UICamera UICam
    {
        get
        {
            if (mUICam == null)
                mUICam = UICamera.FindCameraForLayer(UIPrefab.layer);
            return mUICam;
        }
    }

    public bool Raycast(Vector3 startWorldPos, GameObject go)
    {
        if (go == null || UICam == null)
            return false;

        if(UICamera.Raycast(UICam.cachedCamera.WorldToScreenPoint(startWorldPos)) && UICamera.lastHit.collider && UICamera.lastHit .collider.gameObject == go)
        {
            return true;
        }
        return false;
    }

    private void OnPressTouch(GameObject go, bool state)
    {
        if (state)
        {
            OnPressTouch(state, ((Vector3)UICamera.currentTouch.pos - mCurObjPos));
        }
        else
        {
            OnPressTouch(state);
        }
    }

    void OnPressTouch(bool state, Vector3 pos = default(Vector3))
    {
        if (!mHasSaveDefaultPos) return;
        if (state)
        {
            if (mFixJoystick)
            {
                mJoystick.localPosition = mJoystickDefaultLocalPosition;
                mMouseLocalPos = pos * PixelSizeAdjustment - mJoystick.localPosition;
                if (mMouseLocalPos.sqrMagnitude > 50000)
                {
                    return;
                }
                else if (mMouseLocalPos.sqrMagnitude > 625) //25*25
                {
                    SetTouchPos(mMouseLocalPos);
                }
            }
            else
            {
                mJoystick.localPosition = pos * PixelSizeAdjustment;
                mMouseLocalPos = Vector3.zero;
            }
            CSAutoFightManager.Instance.Stop();
            UtilityPath.ReSetPath();

        }
        else
        {
            if (mFixJoystick == false) mJoystick.localPosition = mJoystickDefaultLocalPosition;
            mTouch.localPosition = Vector3.zero;
            Move(CSDirection.None);
            mLastKeyOffest = Vector3.zero;

        }

        ShowJoystic(state);

        if (CSScene.IsLanuchMainPlayer)
        {
           CSAvatarManager.MainPlayer.TouchMove = state ? EControlState.JoyStick : EControlState.Idle;
        }
    }

    void ShowJoystic(bool value)
    {
        mJoystickWidget.alpha = value ? 1f : 0.4f;
    }

    private void OnDragTouch(GameObject go, Vector2 delta)
    {
        if (CSScene.Sington == null ||CSAvatarManager.MainPlayer == null)
        {
            return;
        }
        if(CSAvatarManager.MainPlayer.TouchMove == EControlState.Idle)
        {
            return;
        }
        mMouseLocalPos += (Vector3)delta * PixelSizeAdjustment;
        SetTouchPos(mMouseLocalPos);
        //Debug.LogFormat("<color=#00ff00> OnDragTouch: {0}</color>",CSAvatarManager.MainPlayer.TouchMove.ToString());
    }

    private void SetTouchPos(Vector3 mouseLocalPos)
    {
        if (mouseLocalPos.magnitude > mAreaRadius) mTouch.localPosition = mouseLocalPos.normalized * mAreaRadius;
        else mTouch.localPosition = mouseLocalPos;
    }

    public void Move(int direction)
    {;
        if (!CSScene.IsLanuchMainPlayer ||CSAvatarManager.MainPlayer.IsBeControl) return;

        if (CSAvatarManager.MainPlayer.TouchMove == EControlState.Idle || direction == CSDirection.None)
        {
           CSAvatarManager.MainPlayer.Stop();
            LastDirection = CSDirection.None;
            CSAutoFightManager.Instance.Stop();
            return;
        }
        if (direction == LastDirection)
        {
            return;
        }
        LastDirection = direction;

        if (CSAvatarManager.MainPlayer.IsCanMove())
        {
            if (direction != CSDirection.None)
            {
               CSAvatarManager.MainPlayer.TowardsTarget(direction);
            }
            else
            {
               CSAvatarManager.MainPlayer.Stop();
            }
        }
        else
        {
            LastDirection = CSDirection.None;
        }
    }

    public void StopMove()
    {
        OnPressTouch(false);
    }


    private void Scene_EnterSceneAfter(uint uiEvtID, object data)
    {
        Reset();
    }

    private void OnReset(uint uiEvtID, object data)
    {
        Reset();
    }

    private void OnMove(uint uiEvtID, object data)
    {
        int dirction = (int)data;
        Move(dirction);
    }
    private void OnStop(uint uiEvtID, object data)
    {
        StopMove();
    }

	private void ShowLockImg(uint uiEvtID, object data)
	{
		obj_lock.SetActive((bool)data);
	}

    void FixJoystickSettingChange(uint id, object data)
    {
        FixJoystick = (bool)data;
    }


    protected override void OnDestroy()
    {
        HotMain.UpdateCallBack -= Update;
        if(mClientEvent != null)
        {
            mClientEvent.UnRegAll();
        }
        base.OnDestroy();
    }
}
