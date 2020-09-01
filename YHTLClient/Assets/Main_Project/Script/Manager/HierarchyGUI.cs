using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HierarchyGUI : MonoBehaviour
{
    public class Data
    {
        public Data parent;
        public Transform self;
        public bool isFolder = true;
        public int layerIndex = 0;
        public List<Data> childList = new List<Data>();//被折叠时，childList无效
    }
    List<Data> list = new List<Data>();
    Vector2 scrollPos;
    Data lastData = null;

    bool isEnableUICamera = true;
	void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Find All Game",GUILayout.Width(200)))
        {
            GameObject.Find("UI Root").transform.Find("Camera").GetComponent<UICamera>().eventReceiverMask = 0;
            Object[] gos = GameObject.FindObjectsOfType(typeof(GameObject));
            list.Clear();
            GetTopData(gos, list);
            GetChild(list);
        }

        bool b = GUILayout.Toggle(isEnableUICamera, "禁用UICamera碰撞", GUILayout.Width(100));
        if (isEnableUICamera != b)
        {
            isEnableUICamera = b;
            if (isEnableUICamera)
            {
                GameObject.Find("UI Root").transform.Find("Camera").GetComponent<UICamera>().eventReceiverMask = -1;
            }
            else
            {
                GameObject.Find("UI Root").transform.Find("Camera").GetComponent<UICamera>().eventReceiverMask = 0;
            }
        }

        GUILayout.EndHorizontal();

        if (list.Count != 0)
        {
            //GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width),GUILayout.Height(Screen.height - 20));
            ResetAllDataHeight(list);
            lastData = null;
            for (int i = 0; i < list.Count;i++)
            {
                Data data = list[i];
                DrawData(data);
            }
            GUILayout.EndScrollView();
        }
    } 

    void ResetAllDataHeight(List<Data> L)
    {
        for (int i = 0; i < L.Count; i++)
        {
            Data data = L[i];
            data.layerIndex = 0;
            ResetAllDataHeight(data.childList);
        }
    }

    void DrawData(Data data)
    {
        if (data.self == null) return;
        if (data.parent == null)
        {
            data.layerIndex = 0;
        }
        else
        {
            data.layerIndex = data.parent.layerIndex + 1;
        }
        //string s = GetEmpty(data.layerIndex);
        GUILayout.BeginHorizontal();
        //if(!string.IsNullOrEmpty(s))GUILayout.Label(s);
        GUILayout.Label(" ", GUILayout.Width(20 * data.layerIndex));
        if (data.childList.Count > 0)
        {
            data.isFolder = GUILayout.Toggle(data.isFolder, "",GUILayout.Width(20));
        }
        else
        {
            GUILayout.Label(" ", GUILayout.Width(20));
        }
        GUILayout.Box(data.self.name,GUILayout.Width(200));

        if (GUILayout.Button("Active:" + data.self.gameObject.activeSelf, GUILayout.Width(100)))
        {
            data.self.gameObject.SetActive(!data.self.gameObject.activeSelf);
        }
        int panelDepth = 0;
        if (data.self.gameObject.GetComponent<UIPanel>() != null)
            panelDepth = data.self.gameObject.GetComponent<UIPanel>().depth;

        if (panelDepth != 0)
            GUILayout.Label("depth:" +panelDepth.ToString(), GUILayout.Width(200));

        GUILayout.EndHorizontal();
        //lastData = data;

        if (!data.isFolder)
        {
            for (int i = 0; i < data.childList.Count; i++)
            {
                DrawData(data.childList[i]);
            }
        }
    }

    string GetEmpty(int num)
    {
        string s = "";
        for (int i = 0; i < num; i++)
        {
            s += " ";
        }
        return s;
    }

    void GetChild(List<Data> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GetChild(list[i]);
        }
    }

    void GetChild(Data data)
    {
        for (int i = 0; i < data.self.childCount; i++)
        {
            Transform t = data.self.GetChild(i);
            Data d = new Data();
            d.self = t;
            d.layerIndex = data.layerIndex + 1;
            d.parent = data;
            data.childList.Add(d);
            GetChild(d);
        }
    }

    void GetTopData(Object[] gos,List<Data> list)
    {
        for (int i = 0; i < gos.Length; i++)
        {
            GameObject go = gos[i] as GameObject;
            if (go == null) continue;
            if (go.name == "GameState") continue;
            Transform trans = GetTopTrans(go.transform);
            Data data = GetData(trans, list);
            if (data != null) continue;
            data = new Data();
            data.self = trans;
            list.Add(data);
        }
    }

    Data GetData(Transform trans, List<Data> list)
    {
        Data data = list.Find((t) => t.self == trans);
        return data;
    }

    Transform GetTopTrans(Transform trans)
    {
        while (trans != null)
        {
            if (trans.parent == null) return trans;
            trans = trans.parent;
        }
        return null;
    }
}
