using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class UIFunctionPanel : UIBasePanel
{
    /// <summary>
    /// 此处值为 FuncOpen 的 id，， 和 FunctionType 对应
    /// </summary>
    

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }


    private UIGridContainerHot<BtnData> gridContainerleft;
    private UIGridContainerHot<BtnData> gridContainerright;


    public override void Init()
    {
        base.Init();

        UIEventListener.Get(mbtn_texleftbg).onClick = OnClosePanel;
        UIEventListener.Get(mbtn_texrightbg).onClick = OnClosePanel;
        UIEventListener.Get(mbtn_texleftbg).onDragEnd = OnClosePanel;
        UIEventListener.Get(mbtn_texrightbg).onDragEnd = OnClosePanel;
        
        CSScenePanelPosManager.Instance.AddPanel(name);
    }

    private void InitFuntion()
    {
        TABLE.SUNDRY sundry;
        if (SundryTableManager.Instance.TryGetValue(202, out sundry))
        {
            List<List<int>> funtionList = UtilityMainMath.SplitStringToIntLists(sundry.effect);
            if (funtionList.Count < 2) return;

            FunctionTweenRotation rotationleft = mPoolHandleManager.GetCustomClass<FunctionTweenRotation>();
            rotationleft.Init(mbtn_leftIcons, FunctionRotateDir.left, 15);

            gridContainerleft = new UIGridContainerHot<BtnData>();
            gridContainerleft.SetArrangement(UIGridContainerHot<BtnData>.Arrangement.Circle).SetCellAngle(-15)
                .SetGameObject(mbtn_leftIcons.gameObject, mbtn_leftIcons.GetChild(0).gameObject);
            gridContainerleft.MaxCount = funtionList[0].Count;

            BtnData go1;
            for (int i = 0; i < gridContainerleft.MaxCount; i++)
            {
                go1 = gridContainerleft.controlList[i];
                go1.SetIcon(funtionList[0][i], i, true);
                rotationleft.AddChild(go1.gameObject);
                if(go1.RedPoint != null) RegisterObjRed((FuntionMuneType) funtionList[0][i], go1.RedPoint);
                UIEventListener.Get(go1.gameObject, funtionList[0][i]).onClick = OnClickPanel;
            }

            FunctionTweenRotation rotationRight = mPoolHandleManager.GetCustomClass<FunctionTweenRotation>();
            rotationRight.Init(mbtn_rightIcons, FunctionRotateDir.right, 15);

            gridContainerright = new UIGridContainerHot<BtnData>();
            gridContainerright.SetArrangement(UIGridContainerHot<BtnData>.Arrangement.Circle).SetCellAngle(15)
                .SetGameObject(mbtn_rightIcons.gameObject, mbtn_rightIcons.GetChild(0).gameObject);
            gridContainerright.MaxCount = funtionList[1].Count;

            for (int i = 0; i < gridContainerright.MaxCount; i++)
            {
                go1 = gridContainerright.controlList[i];
                go1.SetIcon(funtionList[1][i], i, false);
                rotationRight.AddChild(go1.gameObject);
                if(go1.RedPoint != null) RegisterObjRed((FuntionMuneType) funtionList[1][i], go1.RedPoint);
                UIEventListener.Get(go1.gameObject, funtionList[1][i]).onClick = OnClickPanel;
            }
        }
    }

    public override void Show()
    {
        base.Show();
        InitFuntion();
        CSEffectPlayMgr.Instance.ShowEffectPlay(mshowEffect, 17100);
        mbgFour.Invoke(0.4f, ShowBg);
        //SendOpenEvent();
    }

    private void ShowBg()
    {
        mbgFour.gameObject.SetActive(true);
    }
    
    protected void SendOpenEvent()
    {
        ScriptBinder.Invoke(1f,  SendOpenEvent);
    }

    private void OnClickPanel(GameObject go)
    {
        FuntionMuneType muneType = (FuntionMuneType) UIEventListener.Get(go).parameter;
        if (!UICheckManager.Instance.DoCheckButtonClick((FunctionType) muneType)) return;
        switch (muneType)
        {
            case FuntionMuneType.RolePanel:
                UIManager.Instance.CreatePanel<UIRolePanel>(p =>
                {
                    (p as UIRolePanel).ShowRolePanel();
                });
                break;
            case FuntionMuneType.BagPanel:
                UIManager.Instance.CreatePanel<UIBagPanel>();
                break;
            case FuntionMuneType.SkillPanel:
                UIManager.Instance.CreatePanel<UISkillCombinedPanel>();
                break;
            case FuntionMuneType.ForgePanel:
                UIManager.Instance.CreatePanel<UIEquipCombinePanel>(p =>
                {
                    (p as UIEquipCombinePanel).SelectChildPanel(1);
                });
                break;
            case FuntionMuneType.TimeExpPanel:
                UIManager.Instance.CreatePanel<UITimeExpCombinedPanel>();
                break;
            case FuntionMuneType.WingPanel:
                UIManager.Instance.CreatePanel<UIWingCombinedPanel>(f=>
                    (f as UIWingCombinedPanel).OpenChildPanel((int) UIWingCombinedPanel.ChildPanelType.CPT_WING)
                    );
                break;
            case FuntionMuneType.SocialPanel:
                UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
                {
                    (f as UIRelationCombinedPanel)
                        .OpenChildPanel((int) UIRelationCombinedPanel.ChildPanelType.CPT_FRIEND)
                        ?.SelectChildPanel((int) FriendType.FT_FRIEND);
                });
                break;
            case FuntionMuneType.UnionPanel:
                UtilityPanel.JumpToPanel(Utility.HasGuild() ? 11800 : 11803);
                break;
            case FuntionMuneType.RankingPanel:
                UIManager.Instance.CreatePanel<UIRankingCombinedPanel>(p =>
                {
                    (p as UIRankingCombinedPanel).SelectChildPanel();
                });
                break;
            case FuntionMuneType.SettingPanel:
                UIManager.Instance.CreatePanel<UIConfigPanel>();
                break;
			case FuntionMuneType.UIWarPetCombinedPanel:
				UIManager.Instance.CreatePanel<UIWarPetCombinedPanel>();
				break;
			case FuntionMuneType.HandBookPanel:
                UIManager.Instance.CreatePanel<UIHandBookCombinedPanel>(f =>
                {
                    (f as UIHandBookCombinedPanel).OpenChildPanel((int) UIHandBookCombinedPanel.ChildPanelType
                        .CPT_SETUP);
                });
                break;
            case FuntionMuneType.RongLian:
                UIManager.Instance.CreatePanel<UIWoLongXiLianCombinePanel>();
                break;
        }

        UIManager.Instance.ClosePanel<UIFunctionPanel>();
    }

    private void RegisterObjRed(FuntionMuneType type, GameObject gameObject)
    {
        RedPointType[] redPointTypes = CSFunPanelRedPointInfo.Instance.GetFucResList(type);
        if (redPointTypes != null)
        {
            RegisterRedList(gameObject, redPointTypes);
        }
    }

    private void OnClosePanel(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIFunctionPanel>();
    }

    protected override void OnDestroy()
    {
		gridContainerleft?.Dispose();
        gridContainerleft = null;
        gridContainerright?.Dispose();
        gridContainerright = null;
        
        CSEffectPlayMgr.Instance.Recycle(mshowEffect);
        CSScenePanelPosManager.Instance.RemovePanel(name);
        base.OnDestroy();
    }
}


public class BtnData : GridContainerBase
{
    private UISprite icon;

    private UISprite Icon
    {
        get { return icon ? icon : (icon = gameObject.transform.Find("spr_icon").GetComponent<UISprite>()); }
    }

    private GameObject redPoint;

    public GameObject RedPoint
    {
        get { return redPoint ? redPoint : redPoint = gameObject.transform.Find("redpoint").gameObject; }
    }
    
    private UISprite redPointSpr;

    private UISprite RedPointSpr
    {
        get { return redPointSpr ? redPointSpr : redPointSpr = RedPoint.transform.GetComponent<UISprite>(); }
    }
    
    private TweenAlpha bgEffect;

    private TweenAlpha BgEffect
    {
        get { return bgEffect ? bgEffect : bgEffect = gameObject.transform.Find("spr_effect").GetComponent<TweenAlpha>(); }
    }

    private CSInvoke csInvoke;

    private CSInvoke CsInvoke
    {
        get { return csInvoke ? csInvoke : csInvoke = BgEffect.transform.GetComponent<CSInvoke>(); }
    }
    
    private CSInvoke csInvoke2;

    private CSInvoke CsInvoke2
    {
        get { return csInvoke2 ? csInvoke2 : csInvoke2 = gameObject.transform.GetComponent<CSInvoke>(); }
    }
    
    private UISprite btn_role;

    private UISprite Btn_role
    {
        get { return btn_role ? btn_role : (btn_role = gameObject.transform.GetComponent<UISprite>()); }
    }

    private const int MAX_INDEX = 5;

    public void SetIcon(int funtionId, int index, bool left)
    {
        Icon.spriteName = "funtion_" + funtionId;
        gameObject.name = Icon.spriteName;
        
        if(index > MAX_INDEX) return;
        if (!left) index = MAX_INDEX - index;

        BgEffect.delay = index * 0.07f;
        BgEffect.PlayForward();
        
        CsInvoke.Invoke(0.8f, HideEffect);
        CsInvoke2.Invoke(BgEffect.delay, ShowIcon);
    }

    private void HideEffect()
    {
        //BgEffect.gameObject.SetActive(false);
        BgEffect.duration = 0.5f;
        BgEffect.PlayReverse();
    }
    
    private void ShowIcon()
    {
        Btn_role.alpha = 1;
        Icon.alpha = 1;
        RedPointSpr.alpha = 1;
    }

    public override void Dispose()
    {
        icon = null;
        redPoint = null;
        bgEffect = null;
    }
}