using UnityEngine;
using System.Collections;

public class GMItem : UIBase, IDispose
{
	private UIInput _input1;

	private UIInput input1
	{
		get { return _input1 ?? (_input1 = Get<UIInput>("input1")); }
	}

	private UIInput _input2;

	private UIInput input2
	{
		get { return _input2 ?? (_input2 = Get<UIInput>("input2")); }
	}

	private UIInput _input3;

	private UIInput input3
	{
		get { return _input3 ?? (_input3 = Get<UIInput>("input3")); }
	}

	private UIInput _input4;

	private UIInput input4
	{
		get { return _input4 ?? (_input4 = Get<UIInput>("input4")); }
	}

	private UIInput _input5;

	private UIInput input5
	{
		get { return _input5 ?? (_input5 = Get<UIInput>("input5")); }
	}

	private UIInput _input6;

	private UIInput input6
	{
		get { return _input6 ?? (_input6 = Get<UIInput>("input6")); }
	}

	private UILabel _btnName;

	private UILabel btnName
	{
		get { return _btnName ?? (_btnName = Get<UILabel>("btn_send/btnName")); }
	}

	private GameObject _btn_send;

	private GameObject btn_send
	{
		get { return _btn_send ?? (_btn_send = Get<GameObject>("btn_send")); }
	}

	private UILabel _gmName;

	private UILabel gmName
	{
		get { return _gmName ?? (_gmName = Get<UILabel>("btn/btn_name")); }
	}

	private UILabel _label1;

	private UILabel label1
	{
		get { return _label1 ?? (_label1 = Get<UILabel>("input1/name")); }
	}

	private UILabel _label2;

	private UILabel label2
	{
		get { return _label2 ?? (_label2 = Get<UILabel>("input2/name")); }
	}

	private UILabel _label3;

	private UILabel label3
	{
		get { return _label3 ?? (_label3 = Get<UILabel>("input3/name")); }
	}

	private UILabel _label4;

	private UILabel label4
	{
		get { return _label4 ?? (_label4 = Get<UILabel>("input4/name")); }
	}

	private UILabel _label5;

	private UILabel label5
	{
		get { return _label5 ?? (_label5 = Get<UILabel>("input5/name")); }
	}

	private UILabel _label6;

	private UILabel label6
	{
		get { return _label6 ?? (_label6 = Get<UILabel>("input6/name")); }
	}


	[HideInInspector] public TABLE.GM tbl_gm;

	public string data;
	// Use this for initialization

	public void Show(TABLE.GM tbl)
	{
		tbl_gm = tbl;
		if (tbl == null) return;
		if (input1 == null || input2 == null || input3 == null || input4 == null || input5 == null ||
			input6 == null) return;

		if (!string.IsNullOrEmpty(tbl.parm))
		{
			string[] str = tbl.parm.Split(' '); //attr 血量 防御 攻击
			string[] str2 = tbl.message.Split(';')[0].Split(' ');

			input1.gameObject.SetActive(false);
			input2.gameObject.SetActive(false);
			input3.gameObject.SetActive(false);
			input4.gameObject.SetActive(false);
			input5.gameObject.SetActive(false);
			input6.gameObject.SetActive(false);
			if (str.Length > 1 && str2.Length > 1)
			{
				input1.gameObject.SetActive(true);
				input1.value = str2[1];
				if (label1 != null)
				{
					label1.gameObject.SetActive(true);
					label1.text = str[1];
				}
			}

			if (str.Length > 2 && str2.Length > 2)
			{
				input2.gameObject.SetActive(true);
				input2.value = str2[2];
				if (label2 != null)
				{
					label2.gameObject.SetActive(true);
					label2.text = str[2];
				}
			}

			if (str.Length > 3 && str2.Length > 3)
			{
				input3.gameObject.SetActive(true);
				input3.value = str2[3];
				if (label3 != null)
				{
					label3.gameObject.SetActive(true);
					label3.text = str[3];
				}
			}

			if (str.Length > 4 && str2.Length > 4)
			{
				input4.gameObject.SetActive(true);
				input4.value = str2[4];
				if (label4 != null)
				{
					label4.gameObject.SetActive(true);
					label4.text = str[4];
				}
			}

			if (str.Length > 5 && str2.Length > 5)
			{
				input5.gameObject.SetActive(true);
				input5.value = str2[5];
				if (label5 != null)
				{
					label5.gameObject.SetActive(true);
					label5.text = str[5];
				}
			}

			if (str.Length > 6 && str2.Length > 6)
			{
				input6.gameObject.SetActive(true);
				input6.value = str2[6];
				if (label6 != null)
				{
					label6.gameObject.SetActive(true);
					label6.text = str[6];
				}
			}
		}

		//设置时间根据当前显示
		if (tbl.id == 25)
		{
			input1.value = CSServerTime.Now.Year.ToString();
			input2.value = CSServerTime.Now.Month.ToString();
			input3.value = CSServerTime.Now.Day.ToString();
			input4.value = CSServerTime.Now.Hour.ToString();
			input5.value = CSServerTime.Now.Minute.ToString();
			input6.value = CSServerTime.Now.Second.ToString();
		}

		if (!string.IsNullOrEmpty(tbl.des))
		{
			btnName.text = tbl.des;
			UIEventListener.Get(btn_send).onClick = OnGmSubmitClick;
		}
	}

	private void OnGmSubmitClick(GameObject go)
	{
		if (go == null) return;
		if (tbl_gm == null) return;
		if (input1 == null || input2 == null || input3 == null || input4 == null || input5 == null ||
			input6 == null) return;

		string str = "@" + tbl_gm.gm;

		if (input1.gameObject.activeSelf)
		{
			str += " " + input1.value;
		}

		if (input2.gameObject.activeSelf)
		{
			str += " " + input2.value;
		}

		if (input3.gameObject.activeSelf)
		{
			str += " " + input3.value;
		}

		if (input4.gameObject.activeSelf)
		{
			str += " " + input4.value;
		}

		if (input5.gameObject.activeSelf)
		{
			str += " " + input5.value;
		}

		if (input6.gameObject.activeSelf)
		{
			str += " " + input6.value;
		}

		if (tbl_gm.gm == "time" || tbl_gm.gm == "setopentime")
		{
            //str = "@" + tbl_gm.gm + " " + input2.value + input3.value + input4.value + input5.value + input1.value +
            //	"." + input6.value;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int.TryParse(input1.value, out year);
            int.TryParse(input2.value, out month);
            int.TryParse(input3.value, out day);
            int.TryParse(input4.value, out hour);
            int.TryParse(input5.value, out minute);
            int.TryParse(input6.value, out second);
            System.DateTime dt = new System.DateTime(year, month, day, hour, minute, second);
            var timeStr = "";
            if (tbl_gm.gm == "time")
            {
                timeStr = dt.ToString("yyyyMMdd HH:mm:ss");
            }
            if (tbl_gm.gm == "setopentime")
            {
                timeStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            str = "@" + tbl_gm.gm + " " + timeStr;
        }

		Net.GMCommand(str);
		Transform defaultPanel = go.transform.parent.parent;
		if (defaultPanel != null && defaultPanel.name == "defaultPanel") defaultPanel.gameObject.SetActive(false);
	}

	public void showGMMessage(TABLE.GM tbl)
	{
		if (gmName == null || tbl == null) return;
		tbl_gm = tbl;
		gmName.text = tbl.des;
	}

	public void showGMMessage(string str)
	{
		if (gmName == null || str == null) return;
		data = str;
		if (str.Split('|').Length > 1)
		{
			gmName.text = str.Split('|')[1];
		}
	}

	public string getMessage()
	{
		if (data == null) return "";
		return data.Split('|')[0];
	}

	public override void Dispose()
	{
		base.Dispose();
		_input1 = null;
		_input2 = null;
		_input3 = null;
		_input4 = null;
		_input5 = null;
		_input6 = null;
		_btnName = null;
		_btn_send = null;
		_gmName = null;
		_label1 = null;
		_label2 = null;
		_label3 = null;
		_label4 = null;
		_label5 = null;
		_label6 = null;
	}
}