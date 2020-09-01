using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UISpringPaoDianInstacePanel : UIBasePanel
{
    List<int> fixedPos = new List<int>();
    List<int> randomPos = new List<int>();
    long randomTime = 0;
    Schedule randomTimer;
    Schedule countDownSchaedule;

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Resident; }
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    public override void Init()
    {
        base.Init();
        fixedPos = PaoDianTableManager.Instance.GetPaoDianInfoByType(1);
        UIEventListener.Get(mbtn_exit).onClick = ExitBtnClick;
        mClientEvent.Reg((uint)CEvent.SCRandomPaoDianMessage, SCRandomPaoDianMessage);
        mClientEvent.Reg((uint)CEvent.SCPaoDianExpMessage, SCPaoDianExpMessage);
        mClientEvent.Reg((uint)CEvent.ECM_SCInstanceFinishMessage, GetInstanceFinish);
        if (!Timer.Instance.IsInvoking(countDownSchaedule))
        {
            countDownSchaedule = Timer.Instance.InvokeRepeating(0, 1f, CountDown);
        }


        msg = CSPaoDianInfo.Instance.GetPaoDianRanfomInfo();
        RefreshPaoDian();
        if (!Timer.Instance.IsInvoking(randomTimer))
        {
            randomTimer = Timer.Instance.InvokeRepeating(0, 1f, RandomTimeDown);
        }
        mlb_des.text = ClientTipsTableManager.Instance.GetClientTipsContext(1650);
    }

    public override void Show()
    {
        base.Show();
    }
    void ExitBtnClick(GameObject _go)
    {
        Net.ReqLeaveInstanceMessage(true);
    }
    paodian.RandomPaoDian msg;
    void SCRandomPaoDianMessage(uint id, object data)
    {
        msg = CSPaoDianInfo.Instance.GetPaoDianRanfomInfo();
        RefreshPaoDian();
        if (!Timer.Instance.IsInvoking(randomTimer))
        {
            randomTimer = Timer.Instance.InvokeRepeating(0, 1f, RandomTimeDown);
        }
    }
    void SCPaoDianExpMessage(uint id, object data)
    {
        FNDebug.Log(" 获得泡点经验  ");
        long exp = (long)data;
        mlb_exp.text = exp.ToString();
    }
    
    void GetInstanceFinish(uint id, object data)
    {
        Timer.Instance.CancelInvoke(countDownSchaedule);
        mlb_exp.text = "";
        mlb_coin.text = "";
        mlb_springQua.text = "";
    }
    void CountDown(Schedule _schedule)
    {
        RefreshPaoDian();
    }
    void RandomTimeDown(Schedule _schedule)
    {
        
        if (msg!=null)
        {
            randomTime = (msg.nextRefreshTime - CSServerTime.DateTimeToStampForMilli(CSServerTime.Now)) / 1000;
            FNDebug.Log(msg.nextRefreshTime  +"   " + randomTime);
            mlb_randomtime.text = CSServerTime.Instance.FormatLongToTimeStr(randomTime, 3);
        }
        if (randomTime <= 0)
        {
            randomTime = 0;
            Timer.Instance.CancelInvoke(randomTimer);
            mlb_randomtime.text = "";
        }
    }
    int x = 0;
    int y = 0;
    void RefreshPaoDian()
    {
        x =CSAvatarManager.MainPlayer.NewCell.Coord.x;
        y =CSAvatarManager.MainPlayer.NewCell.Coord.y;
        for (int i = 0; i < fixedPos.Count; i++)
        {
            if (PaoDianTableManager.Instance.GetPaoDianX(fixedPos[i]) == x && PaoDianTableManager.Instance.GetPaoDianY(fixedPos[i]) == y)
            {
                mlb_springQua.text = UtilityColor.GetColorName(PaoDianTableManager.Instance.GetPaoDianMultiple(fixedPos[i]));
                //mlb_springQua.color = UtilityCsColor.Instance.GetColor(PaoDianTableManager.Instance.GetPaoDianMultiple(fixedPos[i]));
                return;
            }
        }
        if (msg != null)
        {
            for (int i = 0; i < msg.paoDianPoints.Count; i++)
            {
                if (msg.paoDianPoints[i].x == x && msg.paoDianPoints[i].y == y)
                {
                    mlb_springQua.text = UtilityColor.GetColorName(PaoDianTableManager.Instance.GetPaoDianMultiple(msg.paoDianPoints[i].configId));
                    //mlb_springQua.color = UtilityCsColor.Instance.GetColor(PaoDianTableManager.Instance.GetPaoDianMultiple(msg.paoDianPoints[i].configId));
                    return;
                }
            }
        }
        mlb_springQua.text = "";
    }
    protected override void OnDestroy()
    {
        Timer.Instance.CancelInvoke(countDownSchaedule);
        Timer.Instance.CancelInvoke(randomTimer);
        base.OnDestroy();
    }
}
