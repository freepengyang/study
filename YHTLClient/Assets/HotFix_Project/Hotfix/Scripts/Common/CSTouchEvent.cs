
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using task;

public class CSTouchEvent : CSGameMgrBase<CSTouchEvent>
{
    private bool mTouchEvent;
    private bool mIsMainPlayerTargetPosChange = false;

    private float mLastStopTime = 0;
    public float LastStopTime
    {
        get { return mLastStopTime; }
        set { mLastStopTime = value; }
    }

    public bool TouchEvent
    {
        get
        {
            return mTouchEvent;
        }
        set
        {
            mTouchEvent = value;
        }
    }

    private CSDirection mTouchDirection;
    public CSDirection TouchDirection
    {
        get
        {
            return mTouchDirection;
        }
        set
        {
            if (mTouchDirection != value)
                mTouchDirection = value;
        }
    }

    private CSMisc.Dot2 mClickCoord;
    public CSMisc.Dot2 ClickCoord
    {
        get { return mClickCoord; }
        set { mClickCoord = value; }
    }

    private float mlastTime = 0.0f;
    private float lastClickTime = 0.0f;
    public static CSTouchEvent Sington;
    public override void Start()
    {
        base.Start();
        Sington = this;
    }

    bool IsClickNGUI
    {
        get
        {
            return UICamera.selectedObject == null || UICamera.selectedObject.transform.name != "UI Root";
        }
    }

    //点击地面次数//
    private int numberClicks = 0;

    /// <summary>
    /// 双击打断任务 true:调用后续逻辑
    /// </summary>
	private bool DoubleClickSuspend()
    {
        //TODO:ddn
        //UIMainSceneManager win = UIManager.Instance.GetPanel<UIMainSceneManager>();
        //if (win != null && win.IsHasCurSelectSkill()) return true;

        if (Utility.isDoMissionOrAutoFighting/* || CSMainPlayerInfo.Instance.FishID != 0*/)
        {
            if (Time.time - lastClickTime > 1f)
            {
                numberClicks = 1;
            }
            else
            {
                numberClicks++;
            }

            lastClickTime = Time.time;

            if (numberClicks <= 1)
            {
                UtilityTips.ShowCenterInfo(CSString.Format(645));
                //if (CSAvatarManager.MainPlayerInfo.FishID != 0) return false;
                return true;
            }
            else
            {
                CSPathFinderManager.Instance.ReSetPath();
                //CSAutoFightMgr.Instance.IsBegin = false;
                CSAutoFightManager.Instance.Stop();
                CSPathFinderManager.Instance.PathGuideState = PathGuideState.None;
               CSAvatarManager.MainPlayer.Stop();
                numberClicks = 0;
            }
        }
        else
        {
            numberClicks = 0;
        }
        return true;
    }

    Touch mTouchData;
    int mLastTouchId = -1;

    List<Touch> mLastTwoTouch = new List<Touch>();
    List<Touch> mCurTwoTouch = new List<Touch>();

    public void Update()
    {
        //if (CSGame.Sington == null || CSGame.Sington.CurrentState.State != GameState.MainScene) return;

        if (!CSScene.IsLanuchMainPlayer) return;

        //if (CSSceneManager.Instance.isHuangQuanZhanDongHua) return;

        //if (CSScene.Sington.MainPlayer.getMoveState() == EMoveState.Controlled) return;

        DetectSendMainPlayerTargetPosChange();

        if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickDown(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                ClickHoldOrDrag(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClickUp(Input.mousePosition);
            }

        }
        else if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                mTouchData = Input.GetTouch(i);
                UpdateCurTwoTouch(mTouchData);
                if (mTouchData.phase == TouchPhase.Began)
                {
                    ClickDown(mTouchData.position);

                    if (mLastTouchId == -1 && !IsClickNGUI)
                    {
                        mLastTouchId = mTouchData.fingerId;
                    }
                }

                if (mTouchData.phase == TouchPhase.Moved || mTouchData.phase == TouchPhase.Stationary)
                {
                    if (mLastTouchId != -1 && mLastTouchId == mTouchData.fingerId)
                        ClickHoldOrDrag(mTouchData.position);
                }

                if (mTouchData.phase == TouchPhase.Ended || mTouchData.phase == TouchPhase.Canceled)
                {
                    if (mLastTouchId != -1 && mLastTouchId == mTouchData.fingerId)
                    {
                        ClickUp(mTouchData.position);
                        mLastTouchId = -1;
                    }
                }
            }
            CheckTwoTouchSlide();
        }
    }

    void UpdateCurTwoTouch(Touch touch)
    {
        if (mCurTwoTouch == null)
            return;
        for (int j = 0; j < mCurTwoTouch.Count; j++)
        {
            if (mCurTwoTouch[j].fingerId == touch.fingerId)
            {
                mCurTwoTouch[j] = touch;
                return;
            }
        }
        if (touch.phase == TouchPhase.Began && !IsClickNGUI)
            mCurTwoTouch.Add(touch);
    }

    void CheckTwoTouchSlide()
    {
        if (mCurTwoTouch != null && mLastTwoTouch != null)
        {
            if (mCurTwoTouch.Count != 2)
            {
                if (mLastTwoTouch.Count > 0)
                    mLastTwoTouch.Clear();
            }

            for (int i = 0; i < mCurTwoTouch.Count; i++)
            {
                mTouchData = mCurTwoTouch[i];
                if (mTouchData.phase == TouchPhase.Began)
                {
                    if (mCurTwoTouch.Count == 2)
                    {
                        mLastTwoTouch.Clear();
                        mLastTwoTouch.AddRange(mCurTwoTouch);
                    }
                }
                else if (mTouchData.phase == TouchPhase.Ended || mTouchData.phase == TouchPhase.Canceled)
                {
                    if (mCurTwoTouch.Count == 2 && mLastTwoTouch.Count == 2)
                    {
                        //TODO:ddn
                        //if (UIMainSceneManager.Instance && mTouchData.fingerId == mLastTwoTouch[i].fingerId)
                        //{
                        //    UIMainSceneManager.Instance.ExecuteOnTwoTouchSlide(Vector2.Distance(mCurTwoTouch[0].position, mCurTwoTouch[1].position) - Vector2.Distance(mLastTwoTouch[0].position, mLastTwoTouch[1].position));
                        //}
                    }

                    if (mLastTwoTouch.Count > 0)
                        mLastTwoTouch.Clear();
                    mCurTwoTouch.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    void ClickDown(Vector3 clickPos)
    {
        if (mCurTwoTouch != null && mCurTwoTouch.Count == 2)
        {
            if (CSPathFinderManager.Instance.PathGuideState == PathGuideState.None && CSScene.IsLanuchMainPlayer &&CSAvatarManager.MainPlayer.TouchMove == EControlState.Mouse)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Stop);
            }
            return;
        }
        //暂时先这么写
        if (IsClickNGUI) return;

        //if (UIMainSkillPanel.Singleton)
        //    UIMainSkillPanel.Singleton.HideSkillSchemeShortcut();

        if (TouchEvent) return;

        if (CSScene.IsLanuchMainPlayer)
        {

            if (Time.time - mlastTime < 0.4f)
            {
                return;
            }

            mlastTime = Time.time;

            if (!DoubleClickSuspend()) return;
            if (!UtilityPath.IsAutoFind()) HotManager.Instance.EventHandler.SendEvent(CEvent.CancelPathFind);

            if (Camera.main == null) return;

            Vector3 vt3 = Camera.main.ScreenToWorldPoint(clickPos);
            vt3 = vt3 / CSConstant.PixelRatio;
            ClickCoord = dichotomyFind(vt3, CSCell.Size.x, CSCell.Size.y);
            Ray ray = Camera.main.ScreenPointToRay(clickPos);


            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    HitObject ho = hit.collider.GetComponent<HitObject>();
                    if (ho != null)
                    {
                        ho.OnHit(ho);
                        return;
                    }
                    CSAvatarGo avatarGo = hit.collider.GetComponentInParent<CSAvatarGo>();
                    if (avatarGo != null)
                    {
                        //Debug.Log("click: " + avatarGo.Owner.OldCell.Coord.x + " y = " + avatarGo.Owner.OldCell.Coord.y + " MianPlayer x = " + 
                        //   CSAvatarManager.MainPlayer.OldCell.Coord.x + "  y = " + avatarGo.Owner.OldCell.Coord.y  + " avatarId = " + avatarGo.Owner.ID);
                        CSCharacter character =CSAvatarManager.MainPlayer;
                        bool isOnHit = false;
                        if(character != null)
                        {
                            isOnHit = character.TouchEvent.SetTarget(avatarGo.Owner);
                        }
                        if(!isOnHit)
                        {
                            avatarGo.OnHit(character);
                        }
                        return;
                    }
                }
            }

            if (!Utility.isDoMissionOrAutoFighting)
            {
                if (CSAvatarManager.MainPlayer.TouchMove == EControlState.Idle)
                {
                   CSAvatarManager.MainPlayer.TouchMove = EControlState.Mouse;
                }
            }
        }
    }


    void ClickHoldOrDrag(Vector3 clickPos)
    {
        if (Utility.isDoMissionOrAutoFighting) return;

        if (CSScene.IsLanuchMainPlayer &&CSAvatarManager.MainPlayer.TouchMove == EControlState.Mouse)
        {
            Vector3 characterPos =CSAvatarManager.MainPlayer.GetRealPosition2();
            int dirction = UtilityFight.GetMouseDirection(Camera.main.ScreenToWorldPoint(clickPos), characterPos);
            HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Move, dirction);
        }
    }

    void ClickUp(Vector3 clickPos)
    {
        if (Utility.isDoMissionOrAutoFighting) return;

        if (CSScene.IsLanuchMainPlayer &&CSAvatarManager.MainPlayer.TouchMove == EControlState.Mouse)
        {
            HotManager.Instance.EventHandler.SendEvent(CEvent.UIJoystick_Stop);
        }
    }

    public static bool IsExist(int goal_x, int goal_y)
    {
        if (CSAvatarManager.MainPlayer == null) return false;

        if (Compare(goal_x,CSAvatarManager.MainPlayer.NewCell.Coord.x)
            && Compare(goal_y,CSAvatarManager.MainPlayer.NewCell.Coord.y))
        {
            return true;
        }

        return false;
    }

    static bool Compare(int a, int b)
    {
        return (Mathf.Abs(a - b) < 5);
    }
    
    
    public void OnHitTerrain(CSMisc.Dot2 coord, bool isPlayClickEffect = true, bool isMouseClick = false)
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        HotManager.Instance.EventHandler.RemoveEvent(CEvent.HitTerrainMoveOver);

        if (CSAvatarManager.MainPlayer.IsBeControl) return;
        if (isMouseClick)
        { 
            CSAutoFightManager.Instance.Stop();
        }

        ClickCoord = coord;
       CSAvatarManager.MainPlayer.ResetTouchData(ClickCoord);
       CSAvatarManager.MainPlayer.TowardsTarget(ClickCoord);
        //mainPlayer.onStopTrigger -= OnStopMainPlayer;
        //mainPlayer.onStopTrigger += OnStopMainPlayer;
        //MarkMainPlayerTargetPosChange = true;
    }

    public bool MarkMainPlayerTargetPosChange
    {
        set
        {
            mIsMainPlayerTargetPosChange = value;
            if (!mIsMainPlayerTargetPosChange)
            {
                //CSGame.Sington.EventHandler.SendEvent(CEvent.MainPlayerMapPointsClear, null);
                HotManager.Instance.EventHandler.SendEvent(CEvent.MainPlayerMapPointsClear, null);

            }
        }
    }

    void DetectSendMainPlayerTargetPosChange()
    {
        if (mIsMainPlayerTargetPosChange)
        {
            mIsMainPlayerTargetPosChange = false;
            HotManager.Instance.EventHandler.SendEvent(CEvent.MainPlayerTargetPosChange, null);
        }
    }

    void OnStopMainPlayer(CSCell cell)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.HitTerrainMoveOver);
    }

    private bool PlayClickEffect(CSMisc.Dot2 coord)
    {
        CSCell cell = CSMesh.Instance.getCell(coord.x, coord.y);

        if (cell == null) return false;

        //if (cell.isAttributes(MapEditor.CellType.Resistance))
        //{
        //    Utility.ShowTips(100022, 1.5f, ColorType.Red);
        //    return false;
        //}
        //else
        //{
        //    Utility.ShowCenterMoveUpInfo(3, 100250, ColorType.White);
        //}
        return true;
    }

    public static CSMisc.Dot2 dichotomyFind(Vector3 dot, int x, int y)
    {
        if (dot == null)
        {
            return new CSMisc.Dot2();
        }
        int cellX = Mathf.CeilToInt(dot.x / x) - 1;
        int cellY = Mathf.CeilToInt((CSTerrain.Instance.NewSize.y - dot.y) / y) - 1;
        CSCell cell = CSMesh.Instance.getCell(cellX, cellY);
        if (cell == null)
        {
            return CSAvatarManager.MainPlayer != null &&CSAvatarManager.MainPlayer.OldCell!=null?CSAvatarManager.MainPlayer.OldCell.Coord : new CSMisc.Dot2();
        }
        return cell.Coord;
    }
}
