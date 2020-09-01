using gem;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIGemStarSuitTipPanel : UIBasePanel
{
    public void OpenPanel(PosGemSuit posGemSuit)
    {
        int level = GemSuitTableManager.Instance.GetGemSuitGamLevel(posGemSuit.configId);
        int curNum = CSGemInfo.Instance.GetGemNum(posGemSuit.pos, level);
        TABLE.GEMSUIT gemsuit = CSGemInfo.Instance.GetGemSuitLow(posGemSuit.pos);
        TABLE.GEMSUIT nextGemsuit = null;
        if (posGemSuit.configId !=0)
            GemSuitTableManager.Instance.TryGetValue(posGemSuit.configId, out gemsuit);
        var arr = GemSuitTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i < max;++i)
        {
            var item = arr[i].Value as TABLE.GEMSUIT;
            if (item.gamPosition == gemsuit.gamPosition && item.level > gemsuit.level)
            {
                nextGemsuit = item;
                break;
            }
        }

        int bgH = 20;
        int objcurinfoH = ShowInfo(gemsuit, mobj_curInfo,curNum);
        bgH += objcurinfoH;
        
        if (nextGemsuit != null && posGemSuit.configId != 0)
        {
            int nextNum = CSGemInfo.Instance.GetGemNum(nextGemsuit.gamPosition,nextGemsuit.gamLevel);
            bgH += ShowInfo(nextGemsuit, mobj_nextInfo, nextNum);
            bgH += 20; //添加线的高度
            //自适应组件的宽高
            //float liney = mobj_line.transform.transform.localPosition.y;
            //mobj_line.transform.localPosition = new Vector2(0,liney - objcurinfoH);
            //mobj_line.transform.localPosition = new Vector2(0,bgH/2 - objcurinfoH - 20);
            //float nexty = mobj_nextInfo.transform.localPosition.y;
            //mobj_nextInfo.transform.localPosition = new Vector2(0,nexty -objcurinfoH);

        }
        else
        {
            
            mobj_line.SetActive(false);
            mobj_nextInfo.SetActive(false);
        }
        
        mspr_outframe.height  = bgH;
        
        
        
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
    }


    //显示信息
    public int ShowInfo(TABLE.GEMSUIT gemsuit,GameObject go,int gemNum) {
        bool isReach = gemNum >= gemsuit.gamNum;
        UILabel mlb_maintitle =  UtilityObj.Get<UILabel>(go.transform, "lb_maintitle");
        UILabel lb_subtitle = UtilityObj.Get<UILabel>(go.transform, "lb_subtitle");
        UIGridContainer mgird = UtilityObj.Get<UIGridContainer>(go.transform, "scroll/grid");
        CSStringBuilder.Clear();
        //tempstr 临时用来存储str的变量
        string tempstr = isReach ? gemsuit.name.BBCode(ColorType.MainText) : gemsuit.name.BBCode(ColorType.WeakText);

        mlb_maintitle.text = CSStringBuilder.Append(tempstr, 
            (gemNum >= gemsuit.gamNum ? CSString.Format(977) : CSString.Format(978))).ToString();
        tempstr = isReach ? gemsuit.des.BBCode(ColorType.MainText) : gemsuit.des.BBCode(ColorType.WeakText);
        CSStringBuilder.Clear();

        //lb_subtitle.text = CSString.Format(tempstr, gemNum, gemsuit.gamNum);
        string gemNumstr = CSString.Format(1042, gemNum, gemsuit.gamNum);
        lb_subtitle.text = CSStringBuilder.Append(tempstr, isReach ?
            gemNumstr.BBCode(ColorType.Green):gemNumstr.BBCode(ColorType.Red)).ToString();
        RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
       
        if (gemsuit.attr.Count > 0)
        {
            attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, gemsuit.attr);
        }
        //Debug.Log(attrItems.Count + "||" + attrItems[1].Key);
        mgird.MaxCount = attrItems.Count;
        for (int i = 0; i < attrItems.Count; i++)
        {
            UILabel mlb_attr =  UtilityObj.Get<UILabel>(mgird.controlList[i].transform, "key");
            CSStringBuilder.Clear();
            tempstr = CSStringBuilder.Append(attrItems[i].Key, CSString.Format(999), attrItems[i].Value).ToString();
            mlb_attr.text = isReach ? tempstr.BBCode(ColorType.MainText) : tempstr.BBCode(ColorType.WeakText);
        }

        int OutValue = 70 + 26 * attrItems.Count; //70为计算出来的文本 前段的长度 26 为 grid 的间隔;

        mPoolHandleManager.Recycle(attrItems);
        return OutValue;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
}
