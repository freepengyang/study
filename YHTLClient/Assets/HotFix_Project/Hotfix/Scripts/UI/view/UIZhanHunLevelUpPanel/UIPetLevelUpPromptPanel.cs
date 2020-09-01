using System;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;


public partial class UIPetLevelUpPromptPanel : UIBasePanel
{
    private Dictionary<int,int> mustItems;
    private Dictionary<int,int> luckItems;
    private FastArrayElementFromPool<UIItemBase> items;
    //private int AddExp = 0;
    private List<UILabel> lbList;
    private int PetMaxLv = 0;
    #region 跳字参数
    float mUpdateDeltaTime = 0.1f;
    float mFightTimer = 0;
    private List<float> mUpdateTimes;
    #endregion
    private int oldLevel = 0;
    private int curLevel = 0;
    bool[] isFinishs = new bool[]{false,false,false};
    private Coroutine coroutine;
    
    
    /// <summary>
    /// 计算出来的面板中模块的高度 0 为 基础高度 1 为 技能条 2 为幸运回收
    /// </summary>
    private List<int> mustTimers;
    
    private int[] Heighs = new[] {120,30,140}; 
    
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        mustItems = mPoolHandleManager.GetSystemClass<Dictionary<int,int>>();
        mustItems.Clear();
        luckItems = mPoolHandleManager.GetSystemClass<Dictionary<int,int>>();
        luckItems.Clear();
        lbList = mPoolHandleManager.GetSystemClass<List<UILabel>>();
        lbList.Clear();
        mUpdateTimes = mPoolHandleManager.GetSystemClass<List<float>>();
        mUpdateTimes.Clear();
        mustTimers = mPoolHandleManager.GetSystemClass<List<int>>();
        mustTimers.Clear();
    }
	
    public override void Show()
    {
        base.Show();
        RefreshUI();
    }

    public void RefreshUI()
    {
        int id = CSPetBasePropInfo.Instance.GetZhanHunSuitId();
        PetMaxLv = ZhanHunSuitTableManager.Instance.GetZhanHunSuitMaxLevel(id);
        
        var csinfo = CSPetLevelUpInfo.Instance;
        int High = Heighs[0];
        var iteminfo = csinfo.resItemCallBackInfo.itemInfo;
        for (int i = 0; i < iteminfo.Count; i++)
        {
            ITEMCALLBACKBASE itemcallbackbase;
            if (ItemCallBackBaseTableManager.Instance.TryGetValue(iteminfo[i].id,out itemcallbackbase))
            {
                if (itemcallbackbase.item == 7)
                    continue;

                if (itemcallbackbase.unlock == 0)
                {
                    if (!mustItems.ContainsKey(itemcallbackbase.item))
                        mustItems.Add(itemcallbackbase.item,iteminfo[i].num);
                    
                }
                else
                {
                    if (!luckItems.ContainsKey(itemcallbackbase.item))
                        luckItems.Add(itemcallbackbase.item,iteminfo[i].num);    
                }

                
            }
        }

        #region 必得收益
        mgrid_mustItems.MaxCount = mustItems.Count;
        var mustIter = mustItems.GetEnumerator();
        int mustindex = 0;
        mustTimers.Clear();
        mUpdateTimes.Clear();
        while (mustIter.MoveNext())
        {
            Transform trans = mgrid_mustItems.controlList[mustindex].transform;
            UISprite icon = trans.GetComponent<UISprite>();
            lbList.Add(UtilityObj.Get<UILabel>(trans,"lb_value"));
            mUpdateTimes.Add(100);
            for (var it = CSPetLevelUpInfo.Instance.DicTimes.GetEnumerator(); it.MoveNext();)
            {
                int num = mustIter.Current.Value;
                
                if (num >= it.Current.Key)
                {
                    mUpdateTimes[mustindex] = it.Current.Value;
                }
            }
            
            ITEM item;
            if (ItemTableManager.Instance.TryGetValue(mustIter.Current.Key,out item))
            {
                icon.spriteName = $"tubiao{item.icon}";
                //lb_value.text = mustIter.Current.Value.ToString();
            }
            mustindex++;
        }
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.InvokeRepeating(0, mUpdateDeltaTime , UpdateItemNum);
        #endregion

        #region 幸运收益
        int luckindex = 0;

        mluckItem.SetActive(luckItems.Count !=0);
        items = mPoolHandleManager.CreateItemPool(PropItemType.Normal, 
            mgrid_luckItems.transform,8,itemSize.Size70);
        if (luckItems.Count !=0)
        {
            var luckIter = luckItems.GetEnumerator();
            while (luckIter.MoveNext())
            {
                var item = items.Append();
                item.Refresh(luckIter.Current.Key);
                item.SetCount(luckIter.Current.Value);
                luckindex++;
            }

            mgrid_luckItems.enabled = true;
            High += Heighs[2];
        }
        
        #endregion

        int petid = CSPetBasePropInfo.Instance.GetZhanHunSuitId();
        var resinfo = csinfo.resPetInfo;
        int oldexp = csinfo.oldCurExp;
        int curexp = resinfo.exp;
        curLevel = resinfo.level;
        oldLevel = csinfo.oldCurLv;
        bool isOpenExp = petid != 0 && (oldexp != curexp || curLevel != oldLevel);
        mType2.gameObject.SetActive(isOpenExp);
        if (isOpenExp)
        {
            High += Heighs[1];
            mType2.height =60;
            
            int upLevelCount = resinfo.level - csinfo.oldCurLv;
            
            int curMax = ZhanHunLevelTableManager.Instance.GetZhanHunLevelNeedExp(resinfo.level);
            int oldMax = ZhanHunLevelTableManager.Instance.GetZhanHunLevelNeedExp(csinfo.oldCurLv);
            mlb_lv.text = CSString.Format(1703, oldLevel.ToString());
            
            if (csinfo.oldCurLv == PetMaxLv && resinfo.level == PetMaxLv)
            {
                mlb_exp.text = CSString.Format(1728);
                mslider_level.value = 1;
                
            }
            else
            {
                mlb_exp.text = $"{curexp}/{curMax}".BBCode(ColorType.MainText);
                if (upLevelCount == 0)
                {
                    TweenProgressBar.Begin(mslider_level, 1f * (curexp - oldexp)/(float)curMax, 
                        (float)oldexp/oldMax, (float)curexp/curMax);
                }
                else
                {
                    //CSGame.Sington.StopCoroutine(SetProgressBarValue(0,0,0));
                    BindCoroutine(1,SetProgressBarValue(upLevelCount,
                        (float)oldexp/oldMax, (float)curexp/curMax));
                    
                    //coroutine = ScriptBinder.StartCoroutine();
                }
            }
        }
        else
        {
            mType2.height = 0;
        }

        mTexbg.height = High;
    }

    
    void UpdateItemNum()
    {
        mFightTimer += mUpdateDeltaTime;

        if (lbList.Count != mustItems.Count)
        {
            //Debug.LogError("显示数据错误");
            return;
        }

        var iter = mustItems.GetEnumerator();
        
        int index = 0;
        while (iter.MoveNext())
        {
            int AddValue = iter.Current.Value;
            float time = (float) mUpdateTimes[index] / 1000;
            int curV = (int) Mathf.Lerp(0, AddValue, mFightTimer / time );
            
            if (curV >= AddValue)
            {
                isFinishs[index] = true;
            }
            lbList[index].text = curV < AddValue ? curV.ToString() : AddValue.ToString();
            index++;
        }
        
        if(isFinishs[0]&&isFinishs[1]&&isFinishs[2])
        {
            ScriptBinder.StopInvokeRepeating();
        }
    }

    IEnumerator SetProgressBarValue(int count, float start, float end)
    {
        for (int i = 0; i <= count; i++)
        {
            if (i == 0)
            {
                TweenProgressBar.Begin(mslider_level, 1f * (1 - start), start, 1);
                yield return new WaitForSeconds(1f * (1 - start));
                mlb_lv.text = CSString.Format(1703, (curLevel).ToString());  
                if (curLevel >= PetMaxLv)
                {
                    mlb_exp.text = CSString.Format(1728);
                }
                //ShowLevel(i);
            }
            else
            {
                if (curLevel>=PetMaxLv)
                {
                    mslider_level.value = 1;
                }
                else
                {
                    TweenProgressBar.Begin(mslider_level, 1f * end, 0, end);
                }

                break;
            }
            
            // if (i == 0)
            // {
            //     TweenProgressBar.Begin(mslider_level, 1f * (1 - start), start, 1);
            //     yield return new WaitForSeconds(1f * (1 - start));
            //     mlb_lv.text = CSString.Format(1703, (curLevel).ToString());  
            //     if (curLevel >= PetMaxLv)
            //     {
            //         mlb_exp.text = CSString.Format(1728);
            //     }
            //     ShowLevel(i);
            //     
            // }
            // else if (i == count)
            // {
            //     TweenProgressBar.Begin(mslider_level, 1f * end, 0, end);
            //     yield return new WaitForSeconds(1f * end);
            //     if (curLevel == PetMaxLv)
            //     {
            //         mslider_level.value = 1;
            //     }
            //     
            // }
            // else
            // {
            //     TweenProgressBar.Begin(mslider_level, 1f, 0, 1);
            //     yield return new WaitForSeconds(1);
            //     ShowLevel(i);
            // }

            
        }
    }
    
    //修改需求 现在直接跳到最后
    private void ShowLevel(int index)
    {
        int curlv = oldLevel + index + 1;
        mlb_lv.text = CSString.Format(1703, (curlv).ToString());  
        if (curlv >= PetMaxLv)
        {
            mlb_exp.text = CSString.Format(1728);
            mslider_level.value = 1;
        }
    }

    protected override void OnDestroy()
    {
        mPoolHandleManager.Recycle(mustItems);
        mPoolHandleManager.Recycle(luckItems);
        mPoolHandleManager.Recycle(mustTimers);
        if (coroutine!= null)
        {
            //关闭协程
            CSGame.Sington.StopCoroutine(coroutine);
        }
       
        mustItems = null;
        luckItems = null;
        base.OnDestroy();
    }
}
