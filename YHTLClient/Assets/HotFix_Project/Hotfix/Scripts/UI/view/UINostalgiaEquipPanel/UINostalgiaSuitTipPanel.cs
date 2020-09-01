using gem;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UINostalgiaSuitTipPanel : UIBasePanel
{
    public void OpenPanel(NostalgiaSuit nostalgiaSuit)
    {
        var suit = nostalgiaSuit.Huaijiusuit;
        mobj_line.SetActive(false);
        mobj_nextInfo.SetActive(false);
        int bgH = 0;
        
        if (nostalgiaSuit.Huaijiusuit.nextID == 0)
        {
            //最高级
            bgH += ShowInfo(suit,mobj_curInfo,true);
            
        }
        else
        {
            if (!nostalgiaSuit.isActive)
            {
                //最低级未激活
                bgH += ShowInfo(suit,mobj_curInfo,false);
            }
            else
            {
                bgH += ShowInfo(suit,mobj_curInfo,true);
                HUAIJIUSUIT nextsuit;
                if (HuaiJiuSuitTableManager.Instance.TryGetValue(suit.nextID,out nextsuit))
                {
                    mobj_line.SetActive(true);
                    mobj_nextInfo.SetActive(true);
                    bgH += ShowInfo(nextsuit,mobj_nextInfo,false);
                }
                
            }
        }
        mspr_outframe.height  = bgH;
        
        ScriptBinder.gameObject.SetActive(false);
        ScriptBinder.gameObject.SetActive(true);
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
        
    }


    //显示信息
    public int ShowInfo(HUAIJIUSUIT huajiusuit,GameObject go,bool isNoGray)
    {
        int high = 166;
        Transform lb_attr1 =  UtilityObj.Get<Transform>(go.transform, "lb_attr1");
        Transform lb_attr2 =  UtilityObj.Get<Transform>(go.transform, "lb_attr2");
        Transform lb_attr3 =  UtilityObj.Get<Transform>(go.transform, "lb_attr3");
        UILabel lb_maintitle = UtilityObj.Get<UILabel>(go.transform, "lb_maintitle");
        
        UILabel lb_hint1 = UtilityObj.Get<UILabel>(lb_attr1, "lb_hint");
        UILabel lb_desc1 = UtilityObj.Get<UILabel>(lb_attr1, "lb_desc");
        
        UILabel lb_hint2 = UtilityObj.Get<UILabel>(lb_attr2, "lb_hint");
        UILabel lb_desc2 = UtilityObj.Get<UILabel>(lb_attr2, "lb_desc");
        
        UILabel lb_desc3 = UtilityObj.Get<UILabel>(lb_attr3, "lb_desc");
        var Info = CSNostalgiaEquipInfo.Instance;
        var equips = Info.GetSuitNum(huajiusuit);
        
        string maintitlestr = CSString.Format(1042, equips.Count, Info.suitSubTypes.Count);

        //title
        string numstr = equips.Count >= Info.suitSubTypes.Count
            ? maintitlestr.BBCode(ColorType.Green)
            : maintitlestr.BBCode(ColorType.Red);
        lb_maintitle.text = $"{huajiusuit.name.BBCode(ColorType.Yellow)}{numstr}";

        //左边标题
        lb_hint1.color = isNoGray ? UtilityColor.GetColor(ColorType.MainText) : UtilityColor.GetColor(ColorType.WeakText);
        lb_hint2.color = isNoGray ? UtilityColor.GetColor(ColorType.MainText) : UtilityColor.GetColor(ColorType.WeakText);

        //穿戴一阶记忆套装,
        var para = Getdesc1Param(huajiusuit,equips,isNoGray);
        if (para.Length == 5)
            lb_desc1.text = CSString.Format(1973,para);
        
        //套装效果
        lb_desc2.text = isNoGray ? huajiusuit.desc.BBCode(ColorType.SecondaryText) 
            : huajiusuit.desc.BBCode(ColorType.WeakText);

        high += lb_desc2.height;
        
        bool isShow = !string.IsNullOrEmpty(huajiusuit.descSmall);
        lb_attr3.gameObject.SetActive(isShow);
        if (isShow)
        {
            var desc3color1 = isNoGray ? UtilityColor.MainText : UtilityColor.WeakText;
            var desc3color2 = isNoGray ? UtilityColor.SecondaryText : UtilityColor.WeakText;
            lb_desc3.text = string.Format(huajiusuit.descSmall, desc3color1, desc3color2);
        }
        else
        {
            high -= 30;
        }

        return high;
    }

    private string[] Getdesc1Param(HUAIJIUSUIT suit,Dictionary<int,NostalgiaBagClass> equips,bool isNoGray)
    {
        List<string> list = mPoolHandleManager.GetSystemClass<List<string>>();
        list.Clear();
        var desc1color = isNoGray ? UtilityColor.SecondaryText : UtilityColor.WeakText;
        list.Add(desc1color);
        var equipNames = suit.descEquipName.Split('&');
        for (int i = 0; i < equipNames.Length; i++)
        {
            var temp = equipNames[i].Split('#');
            if (temp!= null)
            {
                int key;
                int.TryParse(temp[0], out key);
                var name = temp[1];
                if (equips.ContainsKey(key))
                    list.Add(name.BBCode(ColorType.Green));
            
                else
                    list.Add(name.BBCode(ColorType.WeakText));
            }
        }

        return list.ToArray();
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
