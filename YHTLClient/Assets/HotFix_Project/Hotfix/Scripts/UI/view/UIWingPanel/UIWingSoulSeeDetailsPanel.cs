using UnityEngine;

public partial class UIWingSoulSeeDetailsPanel : UIBasePanel
{
    private ILBetterList<ILBetterList<int>> listAttrAddition;

    public override void Init()
    {
        base.Init();
        AddCollider();
        listAttrAddition = CSWingInfo.Instance.ListAttrAddition;
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        if (listAttrAddition == null) return;
        //已加成不置灰, 未加成置灰
        mgrid_effects.MaxCount = listAttrAddition.Count;
        GameObject gp;
        for (int i = 0, max = mgrid_effects.MaxCount; i < max; i++)
        {
            gp = mgrid_effects.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIWingSoulSeeDetailsBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWingSoulSeeDetailsBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWingSoulSeeDetailsBinder;
            }

            Binder.index = i;
            Binder.curLevel = CSWingInfo.Instance.WingSpiritData.YuLingLevelId;
            Binder.Bind(listAttrAddition[i]);
        }

        mbg.height = mgrid_effects.MaxCount * (int) mgrid_effects.CellHeight + 60;
    }

    protected override void OnDestroy()
    {
        mgrid_effects.UnBind<UIWingSoulSeeDetailsBinder>();
        base.OnDestroy();
    }
}

public class UIWingSoulSeeDetailsBinder : UIBinder
{
    private UILabel lb_key;
    private UILabel lb_value;
    private ILBetterList<int> listKeyValue;
    public int curLevel;
    private GameObject bg;

    public override void Init(UIEventListener handle)
    {
        bg = Get<GameObject>("bg");
        lb_key = Get<UILabel>("lb_key");
        lb_value = Get<UILabel>("lb_value");
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        listKeyValue = (ILBetterList<int>) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        bg.SetActive(index%2 == 0);
        if (listKeyValue[0] <= curLevel) //不置灰
        {
            lb_key.text =
                $"{CSString.Format(1957, listKeyValue[0])}{CSString.Format(999)}".BBCode(ColorType.MainText);
            // lb_value.text = CSString.Format(1958, $"+{listKeyValue[1] * 1f / 100}%".BBCode(ColorType.Green));
            lb_value.text =
                $"{CSString.Format(1958).BBCode(ColorType.MainText)}{$"{listKeyValue[1] * 1f / 100}%".BBCode(ColorType.Green)}";
        }
        else //置灰
        {
            lb_key.text = $"{CSString.Format(1957, listKeyValue[0])}{CSString.Format(999)}".BBCode(ColorType.WeakText);
            // lb_value.text = CSString.Format(1958, $"+{listKeyValue[1] * 1f / 100}%").BBCode(ColorType.WeakText);
            lb_value.text =
                $"{CSString.Format(1958)}{listKeyValue[1] * 1f / 100}%".BBCode(ColorType.WeakText);
        }
    }

    public override void OnDestroy()
    {
        lb_key = null;
        lb_value = null;
        listKeyValue = null;
        bg = null;
    }
}