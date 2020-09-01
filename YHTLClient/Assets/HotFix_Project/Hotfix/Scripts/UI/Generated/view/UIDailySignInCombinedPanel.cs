public partial class UIDailySignInCombinedPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_empty;
	protected UnityEngine.GameObject mobj_emptyTexture;
	protected UnityEngine.GameObject mobj_notEmpty;
	protected UIEventListener mbtn_showPreview;
	protected UnityEngine.GameObject mobj_cardPool;
	protected UnityEngine.GameObject mobj_bag;
	protected UIGridContainer mGrid_commonCard;
	protected UIGridContainer mGrid_universalCard;
	protected UILabel mlb_pieces;
	protected UIEventListener mbtn_exchangePieces;
	protected UnityEngine.GameObject mobj_fullHint;
	protected UILabel mlb_cards;
	protected UIGridContainer mGrid_cardPool;
	protected UISprite msp_exchangePieces;
	protected UnityEngine.GameObject mbtn_scrollBag;
	protected UnityEngine.GameObject mbtn_scrollLeft;
	protected UIScrollView mscroll_left;
	protected UIScrollView mscroll_bag;
	protected TweenAlpha mtweenAlphaPool;
	protected TweenAlpha mtweenAlphaBag;
	protected UIWrapContent mwrap;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mobj_hint;
	protected UnityEngine.Transform mtrans_hintView;
	protected UILabel mlb_hint;
	protected UIEventListener mbtn_hint;
	protected TweenPosition mfx_fly;
	protected override void _InitScriptBinder()
	{
		mobj_empty = ScriptBinder.GetObject("obj_empty") as UnityEngine.GameObject;
		mobj_emptyTexture = ScriptBinder.GetObject("obj_emptyTexture") as UnityEngine.GameObject;
		mobj_notEmpty = ScriptBinder.GetObject("obj_notEmpty") as UnityEngine.GameObject;
		mbtn_showPreview = ScriptBinder.GetObject("btn_showPreview") as UIEventListener;
		mobj_cardPool = ScriptBinder.GetObject("obj_cardPool") as UnityEngine.GameObject;
		mobj_bag = ScriptBinder.GetObject("obj_bag") as UnityEngine.GameObject;
		mGrid_commonCard = ScriptBinder.GetObject("Grid_commonCard") as UIGridContainer;
		mGrid_universalCard = ScriptBinder.GetObject("Grid_universalCard") as UIGridContainer;
		mlb_pieces = ScriptBinder.GetObject("lb_pieces") as UILabel;
		mbtn_exchangePieces = ScriptBinder.GetObject("btn_exchangePieces") as UIEventListener;
		mobj_fullHint = ScriptBinder.GetObject("obj_fullHint") as UnityEngine.GameObject;
		mlb_cards = ScriptBinder.GetObject("lb_cards") as UILabel;
		mGrid_cardPool = ScriptBinder.GetObject("Grid_cardPool") as UIGridContainer;
		msp_exchangePieces = ScriptBinder.GetObject("sp_exchangePieces") as UISprite;
		mbtn_scrollBag = ScriptBinder.GetObject("btn_scrollBag") as UnityEngine.GameObject;
		mbtn_scrollLeft = ScriptBinder.GetObject("btn_scrollLeft") as UnityEngine.GameObject;
		mscroll_left = ScriptBinder.GetObject("scroll_left") as UIScrollView;
		mscroll_bag = ScriptBinder.GetObject("scroll_bag") as UIScrollView;
		mtweenAlphaPool = ScriptBinder.GetObject("tweenAlphaPool") as TweenAlpha;
		mtweenAlphaBag = ScriptBinder.GetObject("tweenAlphaBag") as TweenAlpha;
		mwrap = ScriptBinder.GetObject("wrap") as UIWrapContent;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mobj_hint = ScriptBinder.GetObject("obj_hint") as UnityEngine.GameObject;
		mtrans_hintView = ScriptBinder.GetObject("trans_hintView") as UnityEngine.Transform;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mbtn_hint = ScriptBinder.GetObject("btn_hint") as UIEventListener;
		mfx_fly = ScriptBinder.GetObject("fx_fly") as TweenPosition;
	}
}
