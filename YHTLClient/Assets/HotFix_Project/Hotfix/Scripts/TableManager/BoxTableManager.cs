using System;
using System.Collections.Generic;
using System.Linq;
using TABLE;

public partial class BoxTableManager
{
    private List<List<int>> itemList;
    private List<List<int>> numList;
    private List<List<int>> carrerList;
    private List<List<int>> sexList;

    public void GetBoxAwardById(int boxId, Dictionary<int, int> rewardDic)
    {
        if (rewardDic == null) return;
        rewardDic.Clear();
        if (!Instance.TryGetValue(boxId, out BOX boxTab)) return;
        itemList = UtilityMainMath.SplitStringToIntLists(boxTab.item);
        numList = UtilityMainMath.SplitStringToIntLists(boxTab.num);
        carrerList = UtilityMainMath.SplitStringToIntLists(boxTab.career);
        sexList = UtilityMainMath.SplitStringToIntLists(boxTab.sex);
        if (itemList.Count != numList.Count)
        {
            UtilityTips.ShowRedTips($"检查 Box 表数据   BoxId ： {boxId}");
            return;
        }
        int sex = CSMainPlayerInfo.Instance.Sex;
        int carrer = CSMainPlayerInfo.Instance.Career;
        for (var i = 0; i < itemList.Count; i++)
        {
            #if UNITY_EDITOR
            if (itemList[i].Count != numList[i].Count)
            {
                UtilityTips.ShowRedTips($"检查 Box 表数据   BoxId ： {boxId}");
                return;
            }
            #endif
            for (var i1 = 0; i1 < itemList[i].Count; i1++)
            {
                if(!string.IsNullOrEmpty(boxTab.sex))
                {
                    if (i >= sexList.Count || i1 >= sexList[i].Count)
                        continue;

                    if (sexList[i][i1] != 2 && sexList[i][i1] != sex)
                        continue;
                }

                if (!string.IsNullOrEmpty(boxTab.career))
                {
                    if (i >= carrerList.Count || i1 >= carrerList[i].Count)
                        continue;

                    if (carrerList[i][i1] != 0 && carrerList[i][i1] != carrer)
                        continue;
                }

                if (!rewardDic.ContainsKey(itemList[i][i1]))
                    rewardDic.Add(itemList[i][i1], numList[i][i1]);
            }
        }

    }

    /// <summary>
    /// 活动该礼包的特效
    /// </summary>
    /// <param name="boxid"></param>
    /// <returns></returns>
    public int GetEffect(int boxid,int sex,int career)
    {
        string effectStr = GetBoxEffect(boxid);
        
        //string[] effectTemp = effectStr.Split('&');
        List<List<int>> EffectList = UtilityMainMath.SplitStringToIntLists(effectStr);
        if (EffectList == null)
        {
            return 0;
        }
        int curSex = EffectList.Count > 1 ? sex : 0;
        if (career == 0)
            return int.Parse(effectStr);
        
        int curCareer = EffectList[curSex].Count > 1 ? career - 1 : 0;

        return EffectList[curSex][curCareer];
    }


}