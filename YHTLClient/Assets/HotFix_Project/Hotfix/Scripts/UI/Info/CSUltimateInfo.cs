using System;
using shop;
using TABLE;
using ultimate;

public class CSUltimateInfo : CSInfo<CSUltimateInfo>
{
    public RoleUltimateData UltimateData { get; set; }

    public ResponseRankInfo RankInfo { get; set; }

    //3选1属性
    public SelectAdditionEffect SelectAdditionData { get; set; }

    //翻牌属性
    public SelectAdditionEffect _SelectAdditionEffect { get; set; }

    //商店信息
    public ShopInfoResponse _ShopInfoResponse { get; set; }

    //通关副本信息
    public UltimatePassInfo _UltimatePassInfo { get; set; }


    string nodeTabStr;
    public string NodeTabStr {
        get
        {
            if (string.IsNullOrEmpty(nodeTabStr))
            {
                nodeTabStr = ClientTipsTableManager.Instance.GetClientTipsContext(1678);
            }
            return nodeTabStr;
        }
    }


    ILBetterList<string> listBuffEffect;


    string petBufferStr;
    public string PetBufferStr
    {
        get
        {
            if (string.IsNullOrEmpty(petBufferStr))
            {
                petBufferStr = ClientTipsTableManager.Instance.GetClientTipsContext(1907);
            }
            return petBufferStr;
        }
    }


    int totalLevel;



    public void InitUltimate(RoleUltimateData msg)
    {
        UltimateData = msg;
        mClientEvent.SendEvent(CEvent.UltimateData);
    }

    public void InitRankInfo(ResponseRankInfo msg)
    {
        RankInfo = msg;
        mClientEvent.SendEvent(CEvent.UltimateRankInfo);
    }

    public void RefreshSelectAddition(ultimate.SelectAdditionEffect msg)
    {
        SelectAdditionData = msg;
        UIManager.Instance.CreatePanel<UIUltimateChallengeAttributePanel>();
    }

    /// <summary>
    /// 获取极限挑战剩余免费复活次数
    /// </summary>
    /// <returns></returns>
    public int GetUltimateReliveCount()
    {
        if(UltimateData == null) return 0;
        return UltimateData.maxReliveTimes - UltimateData.reliveTimes;
    }

    public void ShowUltimateCard(ultimate.SelectAdditionEffect msg)
    {
        if (msg == null || msg.additionAttrs == null || msg.additionAttrs.Count == 0)
        {
            UtilityTips.ShowRedTips(CSString.Format(734));
            return;
        }

        _SelectAdditionEffect = msg;
        UIManager.Instance.CreatePanel<UIUltimateChallengCardPanel>();
    }

    public void SelectCardFinish(int state)
    {
        if (_SelectAdditionEffect != null)
            _SelectAdditionEffect.additionAttrs.Clear();
    }

    public void RefreshPassInfo(ultimate.UltimatePassInfo msg)
    {
        _UltimatePassInfo = msg;
        UtilityTips.ShowExitInstanceCountDown(180);
    }


    public string GetAttrbuteName(ThreeTuple _ThreeTuple)
    {
        //增加人物属性固定值
        if (_ThreeTuple.a == 1)
        {
            return ClientAttributeTableManager.Instance.GetAttributeName(_ThreeTuple.b);
        }

        //增加人物属性百分比 （万分比）
        if (_ThreeTuple.a == 2)
        {
            return ClientAttributeTableManager.Instance.GetAttributeName(_ThreeTuple.b);
        }

        //增加特殊机制，写死
        if (_ThreeTuple.a == 4)
        {
            if (_ThreeTuple.b == 1)
            {
                return CSString.Format(722);
            }

            if (_ThreeTuple.b == 2)
            {
                return CSString.Format(723);
            }
        }

        //增加buffer..6为战魂buffer
        if (_ThreeTuple.a == 5 || _ThreeTuple.a == 6)
        {
            string buffer = _ThreeTuple.a == 5 ? "" : PetBufferStr;
            if (listBuffEffect == null) listBuffEffect = new ILBetterList<string>();
            else listBuffEffect.Clear();
            Utility.SetAttributeBuff(listBuffEffect, _ThreeTuple.b);
            if (listBuffEffect.Count > 0)
            {
                var arr = listBuffEffect[0].Split('：');
                if (arr.Length > 0)
                {
                    var nameStr = arr[0].Replace("[dcd5b8]", "");
                    return $"{buffer}{nameStr}";
                }                
            }
                
            //else
            //    return $"{buffer}{BufferTableManager.Instance.GetBufferName(_ThreeTuple.b)}";
        }

        return "";
    }



    public string GetAttrbuteValue(ThreeTuple _ThreeTuple)
    {
        //增加人物属性固定值
        if (_ThreeTuple.a == 1)
        {
            if (ClientAttributeTableManager.Instance.TryGetValue(_ThreeTuple.b,
                out TABLE.CLIENTATTRIBUTE clientattribute))
            {
                if ((AttrType)clientattribute.AttrType == AttrType.Percent)
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(Math.Round(_ThreeTuple.c * 100.0f / clientattribute.per, 1), "%");
                    return CSStringBuilder.ToString();
                }
            }
            
            return _ThreeTuple.c.ToString();
        }

        //增加人物属性百分比 （万分比）
        if (_ThreeTuple.a == 2)
        {
            CSStringBuilder.Clear();
            CSStringBuilder.Append(Math.Round(_ThreeTuple.c * 0.01f, 1), "%");
            return CSStringBuilder.ToString();
        }

        //增加特殊机制，写死
        if (_ThreeTuple.a == 4)
        {
            if (_ThreeTuple.b == 1)
            {
                CSStringBuilder.Clear();
                CSStringBuilder.Append(Math.Round(_ThreeTuple.c * 0.01f, 1), "%");
                return CSStringBuilder.ToString();
            }

            if (_ThreeTuple.b == 2)
            {
                return CSString.Format(656, _ThreeTuple.c);
            }
        }
        
        //增加buffer
        if (_ThreeTuple.a == 5 || _ThreeTuple.a == 6)
        {
            if (listBuffEffect == null) listBuffEffect = new ILBetterList<string>();
            else listBuffEffect.Clear();
            Utility.SetAttributeBuff(listBuffEffect, _ThreeTuple.b);
            var arr = listBuffEffect[0].Split('：');
            if (arr.Length > 1)
            {
                var nameStr = arr[1].Replace("[00ff0c]", "");
                return $"{nameStr}";
            }
        }

        return "";
    }


    public int GetTotalLevel()
    {
        if (totalLevel < 1)
        {
            totalLevel = 0;
            var arr = InstanceTableManager.Instance.array.gItem.handles;
            if (arr == null) return totalLevel;
            for (int k = 0, max = arr.Length; k < max; ++k)
            {
                TABLE.INSTANCE cfg = arr[k].Value as TABLE.INSTANCE;
                if (cfg.type == 5) totalLevel++;
            }
        }

        return totalLevel;
    }


    public override void Dispose()
    {
        UltimateData = null;
        RankInfo = null;
        SelectAdditionData = null;
        _SelectAdditionEffect = null;
        listBuffEffect?.Clear();
        listBuffEffect = null;
    }
}