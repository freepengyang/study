using UnityEngine;

public class UITipsViewBase : UIBase, IRecycle
{
	private TweenAlpha mTweenAlpha;

	protected TweenAlpha TA
	{
		get
		{
			if (mTweenAlpha == null) mTweenAlpha = UIPrefab.GetComponent<TweenAlpha>();
			return mTweenAlpha;
		}
	}

	private TweenPosition mTweenPosition;
	public TweenPosition TP
	{
		get
		{
			if (mTweenPosition == null)
			{
				mTweenPosition = UIPrefab.GetComponent<TweenPosition>();
				if (mTweenPosition == null) mTweenPosition = UIPrefab.AddComponent<TweenPosition>();
			}

			return mTweenPosition;
		}
	}

	public override UILayerType PanelLayerType
	{
		get { return UILayerType.TopWindow; }
	}

	private UITipsViewPool.TipsViewType mTipPos = UITipsViewPool.TipsViewType.Down;

	public virtual UITipsViewPool.TipsViewType TipPos
	{
		get { return mTipPos; }
	}

	protected UILabel mlb_tips;
	protected UISprite msp_bg;
	protected UnityEngine.GameObject ChildGo;
	protected Transform ChildGoTrans;
	protected Vector3 _vector3 = Vector3.zero; //缓存坐标
	protected Vector3 gameObjectPos; //对象坐标

	protected override void _InitScriptBinder()
	{
		mlb_tips = ScriptBinder.GetObject("lb_tips") as UILabel;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		ChildGo = ScriptBinder.GetObject("GameObject") as UnityEngine.GameObject;
		ChildGoTrans = ChildGo.transform;
	}

	public override void Init()
	{
		_InitScriptBinder();
	}

	public virtual void ShowTips(string content, float timer/*, Color color*/)
	{

	}
    public virtual void SetExpType(int _type, string content, float timer/*, Color color*/)
    {

    }

    public virtual void Move(int index)
	{

	}

	public override void OnRecycle()
	{
		mlb_tips.text = "";
		Panel.alpha = 1;
		UIPrefabTrans.parent = UIManager.Instance.GetRoot();
		UIPrefabTrans.localPosition = Vector3.zero;
		UIPrefabTrans.localScale = Vector3.one;
		UIPrefab.SetActive(false);
		ChildGoTrans.localScale = Vector3.one;
		_vector3 = Vector3.one;
		gameObjectPos = Vector3.zero;
		UIPanel uipanel = null;
		uipanel = UIPrefab.GetComponent<UIPanel>();
		if (uipanel != null) uipanel.sortingOrder = 0;
		TA.ResetToBeginning();
		TA.enabled = false;
		TP.ResetToBeginning();
		TP.enabled = false;
	}
}