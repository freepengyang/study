public delegate bool ActionClicked();
public partial class UICostPromptPanel : UIBasePanel
{
	public static void Open(int sundryId,int itemId,int itemCount, ActionClicked left = null, ActionClicked right = null, System.Action close = null)
	{
		UIManager.Instance.CreatePanel<UICostPromptPanel>(f =>
		{
			(f as UICostPromptPanel).Show(sundryId, itemId, itemCount, left, right, close);
		});
	}

    public enum FlagValue
	{
		FV_LEFT = (1 << 0),
		FV_RIGHT = (1 << 1),
		FV_CLOSE = (1 << 2),
	}

	int flag;
	int itemCfgId;
	int itemCount;

	public override void Init()
	{
		base.Init();
		Panel.alpha = 0.0f;
	}

	public void Show(int sundryId,int id,int count, ActionClicked left = null, ActionClicked right = null, System.Action close = null)
	{
		Panel.alpha = 1.0f;
		TABLE.SUNDRY sundryItem = null;
		if(!SundryTableManager.Instance.TryGetValue(sundryId,out sundryItem))
		{
			return;
		}
		var tokens = sundryItem.effect.Split('#');
		if (tokens.Length != 10)
			return;

		//标题
		if (null != mlb_Title)
			mlb_Title.text = tokens[0];
		//内容
		if (null != mlb_Content)
			mlb_Content.text = tokens[1];
		//物品ID
		itemCfgId = 0;
		if(id == 0)
		{
            if (!int.TryParse(tokens[2], out itemCfgId))
            {
                return;
            }
            //物品数量
            itemCount = 0;
            if (!int.TryParse(tokens[3], out itemCount))
            {
                return;
            }
        }
		else
		{
			itemCfgId = id;
			itemCount = count;
		}
		//显示L
		flag = 0;
		if(!int.TryParse(tokens[4],out flag))
		{
			return;
		}
		mbtn_left.gameObject.SetActive(flag != 0);
		if(flag != 0)
		{
			mlb_leftLabel.text = tokens[7];
			mbtn_left.onClick = f => 
			{
				if(null != left)
				{
					if(left.Invoke())
					{
						ClosePanel();
					}
				}
				else
				{
					ClosePanel();
				}
			};
		}

        //显示R
        flag = 0;
        if (!int.TryParse(tokens[5], out flag))
        {
            return;
        }
        mbtn_right.gameObject.SetActive(flag != 0);
        if (flag != 0)
        {
            mlb_rightLabel.text = tokens[8];
			mbtn_right.onClick = f => 
			{
                if (null != right)
                {
                    if (right.Invoke())
                    {
                        ClosePanel();
                    }
                }
                else
                {
                    ClosePanel();
                }
            };
		}

        //显示C
        flag = 0;
        if (!int.TryParse(tokens[6], out flag))
        {
            return;
        }

        mbtn_close.gameObject.SetActive(flag != 0);
        mbtn_close.onClick = f =>
        {
            close?.Invoke();
            ClosePanel();
        };

        mbtn_bg.onClick = f =>
        {
            close?.Invoke();
            ClosePanel();
        };

        mgrid.Reposition();
		mItemBar.Setup(UIEventListener.Get(mItemBarHandle));
		//内容
		if (null != mlb_Content)
			mlb_Content.text = string.Format(tokens[1], itemCfgId.QualityName(), itemCount).BBCode(ColorType.SubTitleColor);

        var itemData = UIItemBarManager.Instance.Get();
		itemData.cfgId = itemCfgId;
        itemData.needed = itemCount;
		itemData.owned = itemCfgId.GetItemCount();
        itemData.flag = (int)(ItemBarData.ItemBarType.IBT_GENERAL_COMPARE_SMALL| ItemBarData.ItemBarType.IBT_RED_GREEN/*| ItemBarData.ItemBarType.IBT_ONLY_COST*/);
        itemData.eventHandle = mClientEvent;

		mItemBar.Bind(itemData);
	}

	void ClosePanel()
	{
		UIManager.Instance.ClosePanel(GetType());
	}

	UIItemBar mItemBar = new UIItemBar();
	
	protected override void OnDestroy()
	{
		mItemBar?.OnDestroy();
		mItemBar = null;
        mbtn_left.onClick = null;
        mbtn_right.onClick = null;
        mbtn_close.onClick = null;
        mbtn_add.onClick = null;
		mbtn_bg.onClick = null;
		base.OnDestroy();
	}
}
