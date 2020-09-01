using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIStarSuitTipPanel : UIBasePanel
{
    CSBetterList<string> attrKeys = new CSBetterList<string>();
    CSBetterList<int> attrValues = new CSBetterList<int>();

    public void SetGo(GameObject _go)
    {
        UIPrefab = _go;
    }

    public override void Init()
    {
        base.Init();

        mBtn_close.onClick = CloseClick;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    public void RefreshActived(int mainLv, int mainCount, int nextCount, string timeCountDown = "")
    {
        mSp_bg.transform.localPosition = new Vector2(0, mainLv < 15 ? 235 : 150);
        mobj_curInfo.transform.localPosition = new Vector2(0, mainLv < 15 ? 0 : -85);

        string mainColor = UtilityColor.GetColorString(ColorType.SecondaryText);
        string secondColor = UtilityColor.GetColorString(ColorType.MainText);
        string greenColor = UtilityColor.GetColorString(ColorType.Green);
        string redColor = UtilityColor.GetColorString(ColorType.Red);

        bool isProtect = string.IsNullOrEmpty(timeCountDown) ? false : true;

        mlb_curMaintitle.text = string.Format("{0}{1}星星级套装效果{2}({3})", secondColor, mainLv, isProtect ? redColor : greenColor, isProtect ? "即将失效" : "已激活");
        mlb_curSubtitle.text = string.Format("{0}全部部位达到{1}星且穿戴装备{2}({3}/12)", mainColor, mainLv, greenColor, mainCount);

        var attrs = QianghuaTableManager.Instance.GetQianghuaTaozhuangAttr(mainLv + 1);
        RepeatedField<CSAttributeInfo.KeyValue> kvs = null;
        kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
        
        mgrid_cur.MaxCount = kvs.Count;
        for (int i = 0; i < mgrid_cur.MaxCount; i++)
        {
            UILabel lbKey = mgrid_cur.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
            UILabel lbValue = mgrid_cur.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
            lbKey.text = kvs[i].Key.BBCode(ColorType.SecondaryText);
            lbValue.text = $"+{kvs[i].Value}".BBCode(ColorType.MainText);
        }

        //mlb_hint1.text = $"威慑：对{mainLv}星星级套以下的玩家增伤10%".BBCode(ColorType.Green);   策划说后端没做，去掉
        //mlb_hint1.transform.localPosition = new Vector2(-148, 20 + (5 - mgrid_cur.MaxCount) * 26);
        mlb_hint1.CustomActive(false);

        mlb_timeCountDown.gameObject.SetActive(isProtect);
        if (isProtect)
        {
            mlb_timeCountDown.text = $"{mainLv}星星级套装效果保留剩余{timeCountDown}".BBCode(ColorType.Red);
            mlb_timeCountDown.transform.localPosition = new Vector2(-148, -5 + (mgrid_cur.MaxCount > 5 ? 0 : (5 - mgrid_cur.MaxCount) * 26));
        }

        mobj_line.SetActive(mainLv < 15);
        mobj_nextInfo.SetActive(mainLv < 15);
        if (mainLv < 15)
        {
            mlb_nextMaintitle.text = string.Format("{0}星星级套装效果(未激活)", mainLv + 1).BBCode(ColorType.WeakText);
            mlb_nextSubtitle.text = string.Format("[7e7971]全部部位达到{0}星且穿戴装备[ff0000]({1}/12)", mainLv + 1, nextCount);

            var nextAttrs = QianghuaTableManager.Instance.GetQianghuaTaozhuangAttr(mainLv + 2);
            RepeatedField<CSAttributeInfo.KeyValue> nextKvs = null;
            nextKvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, nextAttrs);
            
            mgrid_next.MaxCount = nextKvs.Count;
            for (int i = 0; i < mgrid_next.MaxCount; i++)
            {
                UILabel lbKey = mgrid_next.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
                UILabel lbValue = mgrid_next.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
                lbKey.text = nextKvs[i].Key.BBCode(ColorType.WeakText);
                lbValue.text = $"+{nextKvs[i].Value}".BBCode(ColorType.WeakText);
            }

            mobj_nextInfo.transform.localPosition = new Vector2(0, -300 + (mgrid_cur.MaxCount > 5 ? 0 : (5 - mgrid_cur.MaxCount) * 26) + (isProtect ? 0 : 16));
            mobj_line.transform.localPosition = new Vector2(0, -26 + (mgrid_cur.MaxCount > 5 ? 0 : (5 - mgrid_cur.MaxCount) * 26) + (isProtect ? 0 : 16));

            //mlb_hint2.text = $"威慑：对{mainLv + 1}星星级套以下的玩家增伤10%".BBCode(ColorType.WeakText);
            //mlb_hint2.transform.localPosition = new Vector2(-148, 64 + (mgrid_next.MaxCount > 5 ? 0 : (5 - mgrid_next.MaxCount) * 26));
            mlb_hint2.CustomActive(false);

            mPoolHandleManager.Recycle(nextKvs);

            mSp_bg.height = 497 - (mgrid_cur.MaxCount > 5 ? 0 : (5 - mgrid_cur.MaxCount) * 26) - (mgrid_next.MaxCount > 5 ? 0 : (5 - mgrid_next.MaxCount) * 26) - (isProtect ? 0 : 16);
            mSp_bg.height = mSp_bg.height - 16;//32来自威慑的两条文字
        }
        else mSp_bg.height = 105 + mgrid_cur.MaxCount * 26 - 16;

        mPoolHandleManager.Recycle(kvs);
    }


    public void RefreshUnactived(int lv, int count)
    {
        mSp_bg.transform.localPosition = new Vector2(0, 150);
        mobj_curInfo.transform.localPosition = new Vector2(0, -85);

        mlb_curMaintitle.text = string.Format("{0}星星级套装效果(未激活)", lv).BBCode(ColorType.WeakText);
        mlb_curSubtitle.text = string.Format("[7e7971]全部部位达到{0}星且穿戴装备[ff0000]({1}/12)", lv, count);

        mobj_nextInfo.SetActive(false);
        mlb_timeCountDown.gameObject.SetActive(false);
        mobj_line.SetActive(false);

        var attrs = QianghuaTableManager.Instance.GetQianghuaTaozhuangAttr(lv + 1);
        RepeatedField<CSAttributeInfo.KeyValue> kvs = null;
        kvs = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);

        mgrid_cur.MaxCount = kvs.Count;
        for (int i = 0; i < mgrid_cur.MaxCount; i++)
        {
            UILabel lbKey = mgrid_cur.controlList[i].transform.GetChild(0).GetComponent<UILabel>();
            UILabel lbValue = mgrid_cur.controlList[i].transform.GetChild(1).GetComponent<UILabel>();
            lbKey.text = kvs[i].Key.BBCode(ColorType.WeakText);
            lbValue.text = $"+{kvs[i].Value}".BBCode(ColorType.WeakText);
        }

        //mlb_hint1.transform.localPosition = new Vector2(-148, 20 + (5 - mgrid_cur.MaxCount) * 26);
        //mlb_hint1.text = $"威慑：对{lv}星星级套以下的玩家增伤10%".BBCode(ColorType.WeakText);
        mlb_hint1.CustomActive(false);

        mSp_bg.height = 105 + mgrid_cur.MaxCount * 26 - 16;//16来自mlb_hint1

        mPoolHandleManager.Recycle(kvs);
    }



    void CloseClick(GameObject go)
    {
        if (UIPrefab != null) UIPrefab.SetActive(false);
    }


    void RecycleList<T>(RepeatedField<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            mPoolHandleManager.Recycle(list[i]);
        }
        list.Clear();
        mPoolHandleManager.Recycle(list);
    }
}
