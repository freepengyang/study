using System.Collections.Generic;
using UnityEngine;
public partial class UIMoneyTipsPanel : UIBasePanel
{
    #region variable
    int id = 0;
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    MoneyShowItem item1;
    MoneyShowItem item2;
    #endregion
    public override void Init()
    {
        base.Init();
        AddCollider();
    }
    int NameId = 0;
    int GetWayId = 0;
    int DesId = 0;

    int UpDis = 14;
    int nameY = -27;
    int distance = 10;
    public void SetId(int _id, Vector3 _v3)
    {
        id = _id;
        //Debug.Log("show  " + id);
        TABLE.MONEYTYPE cfg = MoneyTypeTableManager.Instance.GetCfg(id);
        ItemTableManager ins = ItemTableManager.Instance;
        List<List<int>> content = UtilityMainMath.SplitStringToIntLists(cfg.content);
        List<int> nameList = UtilityMainMath.SplitStringToIntList(cfg.name);
        List<int> getwayList = UtilityMainMath.SplitStringToIntList(cfg.getway);
        List<int> desList = UtilityMainMath.SplitStringToIntList(cfg.desc);
        if (cfg != null)
        {
            float titleH = 0;
            float item1H = 0;
            float lineH = 0;
            float item2H = 0;

            item1 = new MoneyShowItem(mtable_item.gameObject, 0);
            if (cfg.type == 1)
            {
                mlb_title.gameObject.SetActive(true);
                mlb_title.text = ins.GetItemName(cfg.itemid);
                mlb_title.transform.localPosition = new Vector3(16, nameY, 0);
                item1.Down(nameY - mlb_title.height - distance);
                titleH = mlb_title.height + 10;
            }
            else
            {
                mlb_title.gameObject.SetActive(false);
                item1.Down(nameY);
            }
            //item1.Down(nameY - ((mlb_title.gameObject.activeSelf) ? mlb_title.height : 0) - distance);
            NameId = (nameList.Count > 0) ? nameList[0] : 0;
            GetWayId = (getwayList.Count > 0) ? getwayList[0] : 0;
            DesId = (desList.Count > 0) ? desList[0] : 0;
            item1.Refresh(content[0], NameId, GetWayId, DesId);
            if (content.Count == 1)
            {
                mobj_line.gameObject.SetActive(false);
            }
            else
            {
                mobj_line.gameObject.SetActive(true);
                mobj_line.transform.localPosition = new Vector3(164, mitem1Down.transform.localPosition.y - mitem1Down.height, 0);
                GameObject go = GameObject.Instantiate(mtable_item.gameObject, mtrans_par);
                item2 = new MoneyShowItem(go, 0);
                item2.Down((int)mobj_line.transform.localPosition.y - distance);
            }
            item1H = NGUIMath.CalculateRelativeWidgetBounds(item1.go.transform, false).size.y;
            lineH = (item2 == null) ? 0 : (distance + 2);
            if (item2 != null)
            {
                NameId = (nameList.Count > 1) ? nameList[1] : 0;
                GetWayId = (getwayList.Count > 1) ? getwayList[1] : 0;
                DesId = (desList.Count > 1) ? desList[1] : 0;
                item2.Refresh(content[1], NameId, GetWayId, DesId);
                item2H = NGUIMath.CalculateRelativeWidgetBounds(item2.go.transform, false).size.y;
            }
            msp_bg.height = (int)(UpDis + titleH + item1H + lineH + item2H + UpDis - 9);
            mtrans_view.position = _v3;
        }
    }
    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    class MoneyShowItem
    {
        public GameObject go;
        UILabel label1;
        UILabel label2;
        UILabel label3;
        //int distance = 3;
        int startPos = 0;
        public MoneyShowItem(GameObject _go, int y)
        {
            go = _go;
            label1 = go.transform.Find("lb_money").GetComponent<UILabel>();
            label2 = go.transform.Find("lb_way").GetComponent<UILabel>();
            label3 = go.transform.Find("lb_use").GetComponent<UILabel>();
        }
        public void Refresh(string _str1, string _str2, string _str3)
        {
            label1.text = _str1;
            label2.text = _str2;
            label3.text = _str3;
            label1.gameObject.SetActive(_str1 == "");
        }
        public void Down(int y)
        {
            startPos = y;
        }
        public void Refresh(List<int> _ids, int _name, int _getway, int _des)
        {
            long count = 0;
            for (int i = 0; i < _ids.Count; i++)
            {
                count = count + CSBagInfo.Instance.GetMoneyCount(_ids[i]);
            }
            if (_name == 0)
            {
                label1.gameObject.SetActive(false);
            }
            else
            {
                label1.gameObject.SetActive(true);
                label1.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(_name), count);
            }
            if (_getway == 0)
            {
                label2.gameObject.SetActive(false);
            }
            else
            {
                label2.gameObject.SetActive(true);
                label2.text = ClientTipsTableManager.Instance.GetClientTipsContext(_getway);
            }
            if (_des == 0)
            {
                label3.gameObject.SetActive(false);
            }
            else
            {
                label3.gameObject.SetActive(true);
                label3.text = ClientTipsTableManager.Instance.GetClientTipsContext(_des);
            }
            label1.transform.localPosition = new Vector3(0, startPos, 0);
            int lb1 = (int)NGUIMath.CalculateRelativeWidgetBounds(label1.transform, false).size.y;
            int lb2 = (int)NGUIMath.CalculateRelativeWidgetBounds(label2.transform, false).size.y;
            label2.transform.localPosition = new Vector3(0, label1.transform.localPosition.y - lb1, 0);
            label3.transform.localPosition = new Vector3(0, label2.transform.localPosition.y - lb2, 0);
        }
    }
}
