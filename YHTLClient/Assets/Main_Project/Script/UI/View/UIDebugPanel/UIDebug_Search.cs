﻿using UnityEngine;
using System.Collections;

public class UIDebug_Search : MonoBehaviour {
    //public UIDebugPanel debugPanel;
    public GameObject butSearch;
    public UIInput input;
    public UILabel labDesc;
	public void Show(/*UIDebugPanel panel*/)
    {
        //debugPanel = panel;
        UIEventListener.Get(butSearch).onClick = OnClickSearch;
    }

    void OnClickSearch(GameObject go)
    {
        //int num = debugPanel.Search(input.value);
        //labDesc.text = "搜到" + num + "条";
    }
}
