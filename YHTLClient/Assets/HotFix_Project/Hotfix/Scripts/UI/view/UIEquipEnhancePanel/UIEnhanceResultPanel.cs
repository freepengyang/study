using Google.Protobuf.Collections;

public partial class UIEnhanceResultPanel : UIBasePanel
{
	protected UITexture mSp_bg;
	protected UILabel mlb_hint;
	protected UILabel mlb_title;
	protected UIGridContainer mgrid_items;
    protected UnityEngine.GameObject mobj_effect;



    protected override void _InitScriptBinder()
	{
		mSp_bg = ScriptBinder.GetObject("Sp_bg") as UITexture;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mgrid_items = ScriptBinder.GetObject("grid_items") as UIGridContainer;
        mobj_effect = ScriptBinder.GetObject("obj_effect") as UnityEngine.GameObject;
    }

    public override void Init()
    {
        base.Init();
        AddCollider();


        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, 17750);
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        base.OnDestroy();
    }


    public void RefreshUI(int suitLv)
    {
        //mlb_hint.text = $"威慑：对{suitLv}星星级套以下的玩家增伤10%".BBCode(ColorType.Green);
        mlb_hint.CustomActive(false);
        mlb_title.text = $"[ffcc00]激活{suitLv}星套装效果";

        var attrs = QianghuaTableManager.Instance.GetQianghuaTaozhuangAttr(suitLv + 1);
        RepeatedField<CSAttributeInfo.KeyValue> kvs = null;
        kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
        
        mgrid_items.MaxCount = kvs.Count;
        for (int i = 0; i < mgrid_items.MaxCount; i++)
        {
            UILabel lbKey = mgrid_items.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
            UILabel lbValue = mgrid_items.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
            lbKey.text = kvs[i].Key;
            lbValue.text = $"+{kvs[i].Value}";
        }

        mPoolHandleManager.Recycle(kvs);
    }


    void RecycleList<T>(RepeatedField<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            mPoolHandleManager.Recycle(list[i]);
        }
        list.Clear();
        mPoolHandleManager.Recycle(list);
    }

}
