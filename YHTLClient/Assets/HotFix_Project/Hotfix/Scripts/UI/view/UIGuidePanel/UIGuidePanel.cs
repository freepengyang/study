using System;
using UnityEngine;

public partial class UIGuidePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Guide; }
    }

    public override void Init()
	{
		base.Init();

		mClientEvent.AddEvent(CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
		mClientEvent.AddEvent(CEvent.ChangeGuidePanelVisible, OnVisibleChanged);
		ScriptBinder.InvokeRepeating(0.0f, 0.01f, TracePosition);
	}

	protected void OnVisibleChanged(uint id ,object argv)
	{
		if(argv is bool visible)
		{
			SetVisible(visible);
		}
	}

	protected void MoveUIMainScenePanel(uint id,object argv)
	{
		if(argv is bool fadeOut && null != mActivedGuide)
		{
			var activedTween = mActivedGuide.GetComponent<TweenAlpha>();
			if(!fadeOut)
			{
				activedTween?.PlayForward();
			}
			else
			{
				activedTween?.PlayReverse();
			}
		}
	}

	protected Vector2 mFixedOffset = new Vector2(0.5f, 0.5f);
	protected Vector3 mLocalPosition = Vector3.zero;
	protected void TracePosition()
	{
		Transform transform;
		if (null != weakRef && weakRef.TryGetTarget(out transform) && null != transform)
		{
            if (null != mTips)
            {
				UISprite sprite = null;
				if(mSprite.TryGetTarget(out sprite))
				{
					Vector2 pivotOffset = mFixedOffset - sprite.pivotOffset + anchorOffset;
					Vector3 scale = sprite.root.transform.localScale;
					Vector3 pixelOffset = pivotOffset * sprite.localSize * sprite.root.transform.localScale;
					mLocalPosition.x = localPosition.x * scale.x;
					mLocalPosition.y = localPosition.y * scale.y;
					mTips.transform.position = transform.position + pixelOffset + mLocalPosition;
					this.Panel.alpha = sprite.finalAlpha;
					//Debug.LogFormat("<color=#00ff00>[引导界面]:[Alpha:{0}]</color>", this.Panel.alpha);
					return;
				}
				mTips.transform.position = transform.position + localPosition;
            }
		}
		this.Panel.alpha = 1.0f;
		//Debug.LogFormat("<color=#ff0000>[引导界面]:[Alpha:{0}]</color>", this.Panel.alpha);
	}
	
	public override void Show()
	{
		base.Show();
	}

	protected WeakReference<UISprite> mSprite = new WeakReference<UISprite>(null);
	protected WeakReference<Transform> weakRef = new WeakReference<Transform>(null);
	protected Vector3 localPosition = Vector3.zero;
	protected Vector2 anchorOffset = Vector2.zero;
	//TABLE.GUIDEGROUP guideItem;
	protected Transform mActivedGuide;
	public void Show(Transform transform,TABLE.GUIDEGROUP guideItem)
	{
		if(!CSGuideManager.Instance.IsGuiding)
		{
			FNDebug.LogFormat("<color=#00ff00>[新手引导]:激活当前引导项目失败，已经被终止</color>");
			return;
		}
		FNDebug.LogFormat("<color=#00ff00>[新手引导]:激活当前引导项目</color>");
		BindLayer(transform, guideItem.DepthPanel);
		localPosition = new Vector3(guideItem.AnchorAbsX, guideItem.AnchorAbsY, 0);
		anchorOffset = new Vector2(guideItem.AnchorX * 0.01f, guideItem.AnchorY * 0.01f);

		InitCtrlGroups(guideItem.factor);
		if (null != mTips)
		{
			mTips.transform.position = transform.position + localPosition;
		}
		weakRef.SetTarget(transform);
		var sprite = transform.GetComponent<UISprite>();

        if (null == sprite && (guideItem.AnchorAbsX > 0) || (guideItem.AnchorAbsY > 0) || (guideItem.AnchorX > 0) || (guideItem.AnchorY > 0))
        {
            //Debug.LogFormat("<color=#ffff00>[新手引导]:控件上面没有UISprite组件,位置偏移设置可能会有问题 百分比:[({0},{1})] 绝对值:[({2},{3})]</color>"
                //, guideItem.AnchorX, guideItem.AnchorY, guideItem.AnchorAbsX, guideItem.AnchorAbsY);
        }

        mSprite.SetTarget(sprite);
	}

	public enum CtrlType
	{
		CT_TEXT = 1,
		CT_SPRITE = 2,
		CT_TEXTURE = 3,
		CT_EFFECT_NODE = 4,
	}

	protected void BindLayer(Transform transform,string depthPanel)
	{
        //设置层级
        UIPanel panel = null;
        var root = transform;
        while (root && null == panel && root != root.parent)
        {
            panel = root.GetComponent<UIPanel>();
            root = root.parent;
        }

		if (!string.IsNullOrEmpty(depthPanel))
		{
			var tempPanel = Get<UIPanel>(depthPanel, panel.transform);
			if (null != tempPanel)
			{
				panel = tempPanel;
				FNDebug.Log($"[引导面板深度重定向成功]");
			}
			else
			{
				FNDebug.LogError($"[引导面板深度重定向失败]");
			}
		}

        if (null != panel && null != this.Panel)
        {
            this.Panel.depth = panel.depth + 1;
            //Debug.LogFormat("引导面板深度重置:{0}", panel.depth + 1);
        }
    }

	protected void InitCtrlGroups(string link)
    {
		var links = link.Split(new char[] { '\r','\n'});
		for(int i = 0; i < links.Length; ++i)
        {
			if(!string.IsNullOrEmpty(links[i]))
            {
				InitCtrlValues(links[i]);
			}
		}
    }

	protected void InitCtrlValues(string links)
	{
		var tokens = links.Split('#');
		if(tokens.Length <= 0)
		{
            //Debug.LogFormat("<color=#ff0000>结点不能为空</color>");
            return;
        }

        var nodeName = tokens[0];
		mActivedGuide = mTips.transform.Find(nodeName);
        if (null == mActivedGuide)
        {
            //Debug.LogFormat("<color=#ff0000>结点无法找到[{0}]</color>", nodeName);
            return;
        }
		var activedTween = mActivedGuide.GetComponent<TweenAlpha>();
		activedTween?.PlayForward();

		for (int i = 0; i < mTips.transform.childCount;++i)
		{
			mTips.transform.GetChild(i).CustomActive(false);
		}
		mActivedGuide.CustomActive(true);

        if (tokens.Length != 4)
		{
			//Debug.LogFormat("<color=#ff0000>参数错误:[结点名称]#[控件类型]#[控件路径]#[值]:当前值:[{0}]</color>", links);
			return;
		}

		int type = 0;
		if(!int.TryParse(tokens[1],out type) || type <= 0 || type > 4)
		{
			//Debug.LogFormat("<color=#ff0000>控件类型错误:当前值:[{0}]</color>", type);
			return;
		}

		CtrlType ctrlType = (CtrlType)type;
		if(ctrlType == CtrlType.CT_TEXT)
		{
			var text = Get<UILabel>(tokens[2], mActivedGuide);
            if (null != text)
            {
				text.text = tokens[3];
            }
        }
        else if (ctrlType == CtrlType.CT_SPRITE)
        {
            var sprite = Get<UISprite>(tokens[2], mActivedGuide);
            if (null != sprite)
            {
				sprite.spriteName = tokens[3];
            }
        }
        else if (ctrlType == CtrlType.CT_TEXTURE)
        {
            var tex = Get<UITexture>(tokens[2], mActivedGuide);
			if(null != tex)
			{
				CSEffectPlayMgr.Instance.ShowUITexture(tex.gameObject, tokens[3]);
			}
        }
		else if(ctrlType == CtrlType.CT_EFFECT_NODE)
        {
			var objNode = Get(tokens[2], mActivedGuide);
			int effectId = 0;
			if(null != objNode && int.TryParse(tokens[3],out effectId))
            {
				objNode.gameObject.PlayEffect(effectId);
			}
        }
    }
	
	protected override void OnDestroy()
	{
        mActivedGuide = null;
        mClientEvent.RemoveEvent(CEvent.MoveUIMainScenePanel, MoveUIMainScenePanel);
		mClientEvent.RemoveEvent(CEvent.ChangeGuidePanelVisible, OnVisibleChanged);
		//guideItem = null;
		weakRef.SetTarget(null);
		weakRef = null;
		mSprite.SetTarget(null);
		mSprite = null;
		base.OnDestroy();
		FNDebug.LogFormat("<color=#00ff00>[新手引导]:强制重置引导，界面被销毁</color>");
		CSGuideManager.Instance.ResetGuideStep();
	}
}
