using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CSPaoDianInfo : CSInfo<CSPaoDianInfo>
{
    public override void Dispose()
    {
       
    }
    paodian.RandomPaoDian mes;
    public void SetPaoDianRandomInfo(paodian.RandomPaoDian _mes)
    {
        mes = _mes;
        //for (int i = 0; i < mes.paoDianPoints.Count; i++)
        //{
        //    Debug.Log($"{mes.paoDianPoints[i].configId}   {mes.paoDianPoints[i].x}   {mes.paoDianPoints[i].y}");
        //}
    }

    public paodian.RandomPaoDian GetPaoDianRanfomInfo()
    {
        return mes;
    }
}
