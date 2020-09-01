public partial class UINostalgiaPromptPanel
{
	public override void Init()
	{
		base.Init();
		AddCollider();
        
	}
	
	public override void Show()
	{
		base.Show();
	}

	public void OpenPanel(NostalgiaSuit suit)
	{
		int Height = 195;
		
		int type = suit.Huaijiusuit.type;
		var huaijiusuit = suit.Huaijiusuit;
		msp_suit.spriteName = $"nostalgia_btn{type}";
		msp_name.spriteName = $"nostalgia_suit_font{type}";
		msp_num.spriteName = suit.Huaijiusuit.pic;
		mlb_hint.text = CSString.Format(1984,huaijiusuit.name);
		mlb_effect.text = $"{CSString.Format(1985)}: {huaijiusuit.desc.BBCode(ColorType.SecondaryText)}";
		if (!string.IsNullOrEmpty(huaijiusuit.descSmall))
		{
			mlb_time.text = CSString.Format(huaijiusuit.descSmall,UtilityColor.SecondaryText,UtilityColor.Green);
		}
		else
		{
			mlb_time.gameObject.SetActive(false);
			Height -= 30;
		}

		Height += mlb_hint.height;

		mTexbg.height = Height;
	}


	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
	
	
	
}
