using UnityEngine;
using System.Collections;

public class bossItem : UIBase, IDispose
{
	private UILabel _name;

	private UILabel mName
	{
		get { return _name ?? (_name = Get<UILabel>("name")); }
	}

	private UILabel _id;

	private UILabel mId
	{
		get { return _id ?? (_id = Get<UILabel>("id")); }
	}

	private UILabel _btnName;

	private UILabel mBtnName
	{
		get { return _btnName ?? (_btnName = Get<UILabel>("btn/btn_name")); }
	}

	private GameObject _btn;

	private GameObject mBtn
	{
		get { return _btn ?? (_btn = Get<GameObject>("btn")); }
	}

	//private TABLE.MONSTERINFO idata;
	//private TABLE.TASKS idata2;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void RefreshUI(TABLE.MONSTERINFO itemData)
	{
		//idata = itemData;
		if (itemData == null) return;
		if (mBtnName != null) mBtnName.text = "召唤";
		if (mName != null) mName.text = itemData.name;
		if (mId != null) mId.text = itemData.id.ToString();
		UIEventListener.Get(mBtn).onClick = OnBtnClick;
	}

	public void RefreshUI(TABLE.TASKS itemData)
	{
		//idata2 = itemData;
		if (itemData == null) return;
		if (mBtnName != null) mBtnName.text = "完成";
		if (mName != null) mName.text = itemData.name;
		if (mId != null) mId.text = itemData.id.ToString();
		UIEventListener.Get(mBtn).onClick = OnBtnClick;
	}

	public void RefreshUI(TABLE.TITLE itemData)
	{
		//idata2 = itemData;
		if (itemData == null) return;
		if (mBtnName != null) mBtnName.text = "获得";
		if (mName != null) mName.text = itemData.titleName;
		//UnityEngine.Debug.LogError(itemData.name);
		if (mId != null) mId.text = itemData.id.ToString();
		//UnityEngine.Debug.LogError(itemData.id.ToString());
		UIEventListener.Get(mBtn).onClick = OnBtnClick;
	}

	private void OnBtnClick(GameObject go)
	{
		string str = null;
		if (mBtnName.text == "召唤")
		{
			str = "@m" + " " + mId.text;
		}

		if (mBtnName.text == "完成")
		{
			str = "@task" + " " + mId.text;
		}

		if (mBtnName.text == "获得")
		{
			str = "@title" + " " + mId.text;
		}

		Net.GMCommand(str);
	}

	public override void Dispose()
	{
		base.Dispose();
		_name = null;
		_id = null;
		_btnName = null;
		_btn = null;
	}
}