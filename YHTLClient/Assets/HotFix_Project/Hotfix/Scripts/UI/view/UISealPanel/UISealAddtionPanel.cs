using System;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UISealAddtionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private int level = CSSealGradeInfo.Instance.MySealLevel;

    public override void Init()
    {
        base.Init();
        mbtn_close.onClick = Close;
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        List<TABLE.LEVELSEAL> listLevelSeal = new List<LEVELSEAL>();
        listLevelSeal.Clear();
        var arr = LevelSealTableManager.Instance.array.gItem.handles;
        for(int i = 0,max = arr.Length;i<max;++i)
        {
            var item = arr[i].Value as TABLE.LEVELSEAL;
            if (BufferTableManager.Instance.GetBufferExpBuff(item.expPer) != 0)
            {
                listLevelSeal.Add(item);
            }
        }

        mgrid_seal_addtion.MaxCount = listLevelSeal.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        UILabel lb_content1;
        UILabel lb_content2;
        for (int i = 0; i < mgrid_seal_addtion.MaxCount; i++)
        {
            gp = mgrid_seal_addtion.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            lb_content1 = gpBinder.GetObject("lb_content1") as UILabel;
            lb_content2 = gpBinder.GetObject("lb_content2") as UILabel;
            //[00ff0c]//绿色
            //[dcd5b8]//米色
            if (level > listLevelSeal[i].levelSeal)
            {
                CSStringBuilder.Clear();
                lb_content1.text = CSStringBuilder
                    .Append("[00ff0c]", CSString.Format(749, listLevelSeal[i].levelSeal), "[-]").ToString();
                float expPer = (float) BufferTableManager.Instance.GetBufferExpBuff(listLevelSeal[i].expPer) / 100;
                CSStringBuilder.Clear();
                lb_content2.text = CSStringBuilder
                    .Append("[00ff0c]", CSString.Format(750, listLevelSeal[i].levelSeal, expPer), "[-]").ToString();
            }
            else
            {
                CSStringBuilder.Clear();
                lb_content1.text = CSStringBuilder
                    .Append("[dcd5b8]", CSString.Format(749, listLevelSeal[i].levelSeal), "[-]").ToString();
                float expPer = (float) BufferTableManager.Instance.GetBufferExpBuff(listLevelSeal[i].expPer) / 100;
                CSStringBuilder.Clear();
                lb_content2.text = CSStringBuilder
                    .Append("[dcd5b8]", CSString.Format(750, listLevelSeal[i].levelSeal, expPer), "[-]").ToString();
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}