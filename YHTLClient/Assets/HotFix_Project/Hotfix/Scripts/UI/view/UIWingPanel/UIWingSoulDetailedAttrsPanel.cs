using System;
using Google.Protobuf.Collections;
using UnityEngine;

public partial class UIWingSoulDetailedAttrsPanel : UIBasePanel
{
    //槽位位置
    private int position;
    private WingSpiritData wingSpiritData;
    private WingSoulData wingSoulData;
    private int addition = 0;

    public override void Init()
    {
        base.Init();
        AddCollider();
        wingSpiritData = CSWingInfo.Instance.WingSpiritData;
        addition = wingSpiritData.Addition;
    }

    public override void Show()
    {
        base.Show();
    }

    private LongArray attrInfo;
    private RepeatedField<int> ids = new RepeatedField<int>();
    private RepeatedField<int> values = new RepeatedField<int>();

    public void OpenWingSoulDetailedAttrsPanel(int position, Vector3 v3)
    {
        switch (position)
        {
            case 1:
                mlb_title.text = CSString.Format(1961);
                break;
            case 2:
                mlb_title.text = CSString.Format(1962);
                break;
            case 3:
                mlb_title.text = CSString.Format(1963);
                break;
        }

        if (wingSpiritData == null) return;
        for (int i = 0, max = wingSpiritData.WingSoulDatas.Count; i < max; i++)
        {
            WingSoulData wingSoul = wingSpiritData.WingSoulDatas[i];
            if (wingSoul.Position == position)
                wingSoulData = wingSoul;
        }

        if (wingSoulData == null) return;
        attrInfo = CSWingInfo.Instance.GetSpecialAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
            wingSoulData.YulingsoulCfg);

        ids.Clear();
        values.Clear();
        for (int j = 0, max1 = attrInfo.Count; j < max1; j++)
        {
            ids.Add(attrInfo[j].key());
            values.Add((int) (attrInfo[j].value() * ((addition + 10000) * 1f / 10000)));
        }

        //当前属性加成
        RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
        if (ids.Count > 0 && values.Count > 0)
            attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, ids, values);

        GameObject gp;
        mgrid_effects.MaxCount = attrItems.Count;
        for (int i = 0; i < mgrid_effects.MaxCount; i++)
        {
            gp = mgrid_effects.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIWingSoulDetailedAttrsBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWingSoulDetailedAttrsBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWingSoulDetailedAttrsBinder;
            }

            Binder.Bind(attrItems[i]);
        }

        mbg.height = mgrid_effects.MaxCount * (int) mgrid_effects.CellHeight + 60;
        //位置自适应
        UIPrefabTrans.position = v3;
        UIPrefabTrans.localPosition += new Vector3(0f, mbg.height / 2 + 15, 0f);
    }

    protected override void OnDestroy()
    {
        mgrid_effects.UnBind<UIWingSoulDetailedAttrsBinder>();
        base.OnDestroy();
    }
}

public class UIWingSoulDetailedAttrsBinder : UIBinder
{
    private UILabel lb_key;
    private UILabel lb_value;
    private CSAttributeInfo.KeyValue keyValue;

    public override void Init(UIEventListener handle)
    {
        lb_key = Get<UILabel>("lb_key");
        lb_value = Get<UILabel>("lb_value");
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        keyValue = (CSAttributeInfo.KeyValue) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (keyValue != null)
        {
            lb_key.text = $"[cbb694]{keyValue.Key}{CSString.Format(999)}[-]";
            lb_value.text = $"[00FF0C]{keyValue.Value}[-]";
        }
    }

    public override void OnDestroy()
    {
        lb_key = null;
        lb_value = null;
        keyValue = null;
    }
}