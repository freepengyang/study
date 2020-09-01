using System;
using rankalonetable;
using UnityEngine;

public class UIRankingInfoBinder : UIBinder
{
    public bool isSelect;
    // public int index;
    public RankingType type;
    public RankingSubType subType;
    public Action<int> actionInfo;

    private RankingData rankingData;
    private UIEventListener infoListener;
    private UIToggle infoToggle;
    private UISprite sp_rank;
    private UILabel lb_1;
    private UILabel lb_2;
    private UILabel lb_3;
    private UILabel lb_4;
    private UISprite bg;

    private string first = "[ff9000]";
    private string second = "[ff00f0]";
    private string third = "[00ff0c]";
    private string word = "[dcd5b8]";
    

    // private Map<RankingType, List<RankingSubType>> mapRankingSubTypes;

    public override void Init(UIEventListener handle)
    {
        bg = Get<UISprite>("bg");
        infoListener = Get<UIEventListener>("bg");
        infoToggle = Get<UIToggle>("bg");
        sp_rank = Get<UISprite>("sp_rank");
        lb_1 = Get<UILabel>("lb_1");
        lb_2 = Get<UILabel>("lb_2");
        lb_3 = Get<UILabel>("lb_3");
        lb_4 = Get<UILabel>("lb_4");
        infoListener.onClick = OnClickInfo;
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        RankingData info = (RankingData) data;
        rankingData = info;
        // mapRankingSubTypes = CSRankingInfo.Instance.MapRankingSubTypes;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (rankingData == null) return;
        infoToggle.Set(isSelect);
        bg.spriteName = index % 2 == 0 ? "list_subbg1" : "list_subbg2";
        switch (index)
        {
            case 0:
            case 1:
            case 2:
                sp_rank.spriteName = $"rank{index + 1}";
                sp_rank.gameObject.SetActive(true);
                lb_1.text = "";
                break;
            default:
                sp_rank.gameObject.SetActive(false);
                lb_1.text = $"{word}{index + 1}";
                break;
        }

        switch (type)
        {
            case RankingType.Grade:
            case RankingType.FightingPower:
                if (rankingData.RoleRankInfo.roleRankInfo.Count - 1 < index)
                {
                    lb_2.text = $"{word}{CSString.Format(1162)}";
                    lb_3.text = $"{word}{CSString.Format(1163)}";
                    lb_4.text = $"{word}{CSString.Format(1163)}";
                }
                else
                {
                    RoleRankInfo roleRankInfo = rankingData.RoleRankInfo.roleRankInfo[index];
                    if (subType == RankingSubType.All)
                    {
                        switch (index)
                        {
                            case 0:
                                lb_2.text = $"{first}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                                break;
                            case 1:
                                lb_2.text = $"{second}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                                break;
                            case 2:
                                lb_2.text = $"{third}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                                break;
                            default:
                                lb_2.text = $"{word}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                                break;
                        }
                    }
                    else
                    {
                        switch (index)
                        {
                            case 0:
                                lb_2.text = $"{first}{roleRankInfo.name}";
                                break;
                            case 1:
                                lb_2.text = $"{second}{roleRankInfo.name}";
                                break;
                            case 2:
                                lb_2.text = $"{third}{roleRankInfo.name}";
                                break;
                            default:
                                lb_2.text = $"{word}{roleRankInfo.name}";
                                break;
                        }
                    }


                    string lb = "";
                    switch (index)
                    {
                           
                        case 0:
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{first}{lb}";

                            lb = type == RankingType.Grade ? roleRankInfo.level.ToString() : roleRankInfo.value;
                            lb_4.text = $"{first}{lb}";
                            break;
                        case 1:
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{second}{lb}";

                            lb = type == RankingType.Grade ? roleRankInfo.level.ToString() : roleRankInfo.value;
                            lb_4.text = $"{second}{lb}";
                            break;
                        case 2:
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{third}{lb}";

                            lb = type == RankingType.Grade ? roleRankInfo.level.ToString() : roleRankInfo.value;
                            lb_4.text = $"{third}{lb}";
                            break;
                        default:
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{word}{lb}";
                            
                            lb = type == RankingType.Grade ? roleRankInfo.level.ToString() : roleRankInfo.value;
                            lb_4.text = $"{word}{lb}";
                            break;
                    }
                    
                   
                    
                }

                break;
            case RankingType.Wing:
                if (rankingData.RoleRankInfo.roleRankInfo.Count - 1 < index)
                {
                    lb_2.text = $"{word}{CSString.Format(1162)}";
                    lb_3.text = $"{word}{CSString.Format(1163)}";
                    lb_4.text = $"{word}{CSString.Format(1163)}";
                }
                else
                {
                    RoleRankInfo roleRankInfo = rankingData.RoleRankInfo.roleRankInfo[index];
                    string lb = "";
                    switch (index)
                    {
                        case 0:
                            lb_2.text = $"{first}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{first}{lb}";
                            break;
                        case 1:
                            lb_2.text = $"{second}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{second}{lb}";
                            break;
                        case 2:
                            lb_2.text = $"{third}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{third}{lb}";
                            break;
                        default:
                            lb_2.text = $"{word}{roleRankInfo.name}({Utility.GetCareerName(roleRankInfo.career,true)})";
                            lb = String.IsNullOrEmpty(roleRankInfo.unionName)
                                ? CSString.Format(1163)
                                : roleRankInfo.unionName;
                            lb_3.text = $"{word}{lb}";
                            break;
                    }
                    
                    //X阶X星单独处理
                    int advance = (int) WingTableManager.Instance.GetWingRank(int.Parse(roleRankInfo.value));
                    int star = (int) WingTableManager.Instance.GetWingStarID(int.Parse(roleRankInfo.value));
                    lb_4.text = CSString.Format(1165, advance, star);
                }

                break;
            case RankingType.Guild:
                if (rankingData.UnionRankInfo.unionRankInfo.Count - 1 < index)
                {
                    lb_2.text = $"{word}{CSString.Format(1162)}";
                    lb_3.text = $"{word}{CSString.Format(1163)}";
                    lb_4.text = $"{word}{CSString.Format(1163)}";
                }
                else
                {
                    UnionRankInfo unionRankInfo = rankingData.UnionRankInfo.unionRankInfo[index];
                    switch (index)
                    {
                        case 0:
                            lb_2.text = $"{first}{unionRankInfo.unionName}";
                            lb_3.text = $"{first}{unionRankInfo.name}";
                            lb_4.text = $"{first}{unionRankInfo.level}";
                            break;
                        case 1:
                            lb_2.text = $"{second}{unionRankInfo.unionName}";
                            lb_3.text = $"{second}{unionRankInfo.name}";
                            lb_4.text = $"{second}{unionRankInfo.level}";
                            break;
                        case 2:
                            lb_2.text = $"{third}{unionRankInfo.unionName}";
                            lb_3.text = $"{third}{unionRankInfo.name}";
                            lb_4.text = $"{third}{unionRankInfo.level}";
                            break;
                        default:
                            lb_2.text = $"{word}{unionRankInfo.unionName}";
                            lb_3.text = $"{word}{unionRankInfo.name}";
                            lb_4.text = $"{word}{unionRankInfo.level}";
                            break;
                    }
                    
                    
                }

                break;
        }
    }

    void OnClickInfo(GameObject go)
    {
        actionInfo?.Invoke(index);
    }

    public override void OnDestroy()
    {
        actionInfo = null;
        infoListener = null;
        infoToggle = null;
        sp_rank = null;
        lb_1 = null;
        lb_2 = null;
        lb_3 = null;
        lb_4 = null;
        // mapRankingSubTypes = null;
    }
}