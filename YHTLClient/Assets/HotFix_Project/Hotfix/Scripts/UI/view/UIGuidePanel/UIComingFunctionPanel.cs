public partial class UIComingFunctionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
	{
		base.Init();
        mbtn_close.onClick = this.Close;
	}
	
	public override void Show()
	{
		base.Show();
	}

    TABLE.FUNCOPEN mFuncItem;
    public void Show(TABLE.FUNCOPEN funcItem)
    {
        mFuncItem = funcItem;
        if (null != funcItem)
        {
            if (null != mFuncIcon)
            {
                mFuncIcon.spriteName = funcItem.iconunlock;
            }
            if (null != mFuncTitle)
            {
                mFuncTitle.text = funcItem.title;
                //mFuncTitle.MakePixelPerfect();
            }
            if (null != mFuncOpenLevel)
            {
                mFuncOpenLevel.text = string.Format(mFuncOpenLevel.FormatStr, funcItem.needLevel);
            }
            if (null != mFuncDesc)
            {
                mFuncDesc.text = funcItem.description;
            }
        }
    }

    protected override void OnDestroy()
	{
        mFuncItem = null;

        base.OnDestroy();
	}
}
