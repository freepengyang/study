using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRankingItemBinder : UIBinder
{
    public bool isSelect;
    public RankingType type;
    public RankingSubType subType;
    public Action<RankingSubType> actionItem;

    private UIEventListener subTempListener;
    private UIToggle subTempToggle;
    private UILabel name;
    private UILabel checkmarkName;
    private Map<RankingType, List<RankingSubType>> mapRankingSubTypes;

    public override void Init(UIEventListener handle)
    {
        subTempListener = handle.GetComponent<UIEventListener>();
        subTempToggle = handle.GetComponent<UIToggle>();
        name = Get<UILabel>("name");
        checkmarkName = Get<UILabel>("name", subTempToggle.activeSprite.transform);
        subTempListener.onClick = OnClickSubTab;
    }

    public override void Bind(object data)
    {
        mapRankingSubTypes = CSRankingInfo.Instance.MapRankingSubTypes;
        RefreshUI();
    }

    void RefreshUI()
    {
        subTempToggle.Set(isSelect);
        string[] stringssubTabName = UtilityMainMath.StrToStrArr(CSString.Format(1157));
        if (stringssubTabName.Length<4)
        {
            // Debug.Log("------------------排行榜子页签名字数量配置错误@刘轶");
        }
        for (int i = 0; i < mapRankingSubTypes[type].Count; i++)
        {
            if (subType == mapRankingSubTypes[type][i])
            {
                name.text = stringssubTabName[i];
                checkmarkName.text = stringssubTabName[i];
                break;
            }
        }
    }

    void OnClickSubTab(GameObject go)
    {
        actionItem?.Invoke(subType);
    }

    public override void OnDestroy()
    {
        actionItem=null;
        subTempListener=null;
        subTempToggle=null;
        name=null;
        checkmarkName=null;
        mapRankingSubTypes = null;
    }
}

