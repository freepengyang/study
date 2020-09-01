using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using Object = UnityEngine.Object;
namespace ExtendEditor
{
    public class CSAtlasFind : SelectionBase
    {
        public class PanelData
        {
            public Object panel;
            public float totalMemeroyNum = 0;
            public List<AtlasData> list = new List<AtlasData>();
        }

        public class AtlasData
        {
            public bool isFolder = true;
            public Type resType;
            public string propertyName;
            public UnityEngine.Object resObj;
            public float totalMemeroyNum = 0;
            public List<GameObject> refGOList = new List<GameObject>();
        }

        private GameObject go;
        private Vector2 mScrollPos = Vector2.zero;
        private List<PanelData> panelDatalist = new List<PanelData>();
        private List<PanelData> panelDataAll = new List<PanelData>();
        List<string> listAll = new List<string>();
        private int mFindAllIndex = -1;
        [MenuItem("Tools/UI/查找Prefab中引用的图集")]
        public static void AtlasFindProc()
        {
            EditorWindow win = GetWindow(typeof(CSAtlasFind));
            win.Show();
        }

        public override void OnGUI()
        {
            //base.OnGUI();
            go = EditorGUILayout.ObjectField(go, typeof(GameObject)) as GameObject;
            if (GUILayout.Button("Deal"))
            {
                panelDatalist.Clear();
                FindAllObject(go,typeof(UISprite),"atlas",panelDatalist);
                FindAllObject(go,typeof(UILabel), "ambigiousFont",panelDatalist);
                if(panelDatalist.Count>0)
                panelDatalist[0].list.Sort(CompareSort);
            }
            
            if (GUILayout.Button("Find All"))
            {
                mFindAllIndex = 0;
                FindAll();
            }

            DealFindAll();


            if (GUILayout.Button("Open All"))
            {
                OpenTxt();
            }

            DrawDealResult();
        }

        void DrawDealResult()
        {
            try
            {
                if (panelDatalist.Count > 0)
                {
                    mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos);
                    int index = 0;
                    PanelData panelData = panelDatalist[0];
                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Label("内存总量:" + panelData.totalMemeroyNum + "MB");
                    foreach (AtlasData data in panelData.list)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField((index + 1) + " ", GUILayout.Width(50));
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button((data.resObj is UIAtlas ? "Atlas:" : "Font") + data.resObj.name, GUILayout.Width(200)))
                        {
                            data.isFolder = !data.isFolder;
                            Selection.activeObject = data.resObj;
                        }
                        GUILayout.Label("内存大小:" + data.totalMemeroyNum + "MB" + " 引用数量=" + data.refGOList.Count);
                        EditorGUILayout.EndHorizontal();
                        if (!data.isFolder)
                        {

                            for (int i = 0; i < data.refGOList.Count; i++)
                            {
                                if (GUILayout.Button(" Commonent:" + data.refGOList[i].name, GUILayout.Width(300)))
                                {
                                    Selection.activeGameObject = data.refGOList[i] as GameObject;
                                }
                            }

                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        index++;
                    }
                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.EndScrollView();
                }
            }
            catch (System.Exception ex)
            {
                FNDebug.LogError(ex);
                panelDatalist.Clear();
            }
            
        }

        void FindAll()
        {
            string uiPath = Application.dataPath + "/UIAsset/Prefabs";

            listAll.Clear();

            FileUtility.GetDeepAssetPaths(uiPath, listAll, ".prefab");

            panelDataAll.Clear();
        }

        void DealFindAll()
        {
            
            if (mFindAllIndex != -1&&mFindAllIndex<listAll.Count)
            {
                EditorUtility.DisplayProgressBar("Progress", "处理中(" + mFindAllIndex + "/" + listAll.Count,mFindAllIndex*1.0f/listAll.Count);
                UnityEngine.Object obj = FileUtility.GetObject(listAll[mFindAllIndex]);
                GameObject go = obj as GameObject;
                if (go == null) return;
                FindAllObject(go, typeof(UISprite), "atlas", panelDataAll);
                FindAllObject(go, typeof(UILabel), "ambigiousFont", panelDataAll);
                PanelData data = panelDataAll.Find(delegate(PanelData d)
                {
                    return d.panel = go;
                });
                if (data != null)
                {
                    data.list.Sort(CompareSort);
                }
                mFindAllIndex++;
                if (mFindAllIndex >= listAll.Count)
                {
                    mFindAllIndex = -1;
                    EditorUtility.ClearProgressBar();
                    panelDataAll.Sort(CompareSort);
                    WriteToTxt(panelDataAll);
                    listAll.Clear();
                    panelDataAll.Clear();
                }
            }          
        }

        int CompareSort(PanelData f, PanelData s)
        {
            if (f.totalMemeroyNum > s.totalMemeroyNum) return -1;
            if (f.totalMemeroyNum < s.totalMemeroyNum) return 1;
            return 0;
        }

        int CompareSort(AtlasData f, AtlasData s)
        {
            if (f.totalMemeroyNum > s.totalMemeroyNum) return -1;
            if (f.totalMemeroyNum < s.totalMemeroyNum) return 1;
            return 0;
        }

        void WriteToTxt(List<PanelData> dic)
        {
            string content = "";
            foreach (PanelData panelData in dic)
            {
                GameObject go = panelData.panel as GameObject;
                if(go == null)continue;
                content += "UI窗口名称:" + go.name + " 图集数量:" + panelData.list.Count + " 内存大小" + panelData.totalMemeroyNum + "MB" + "\r\n";
                int index = 0;
                foreach (AtlasData atlasData in panelData.list)
                {
                    Object obj = atlasData.resObj;
                    string path = AssetDatabase.GetAssetPath(obj);
                    //int num = 0;
                    UIAtlas atlas = atlasData.resObj as UIAtlas;
                    content += "    (" + index + ")图集:" + atlasData.resObj.name + "   引用数量:" + atlasData.refGOList.Count + " 内存大小" + atlasData.totalMemeroyNum + "MB" + "\r\n";
                    for (int i = 0; i < atlasData.refGOList.Count; i++)
                    {
                        GameObject g = atlasData.refGOList[i];
                        string str = EditorUtil.GetParentPath(g);
                        content += "        "+ str + "\r\n";
                    }
                    index++;
                }
            }
            FileUtility.Write(Application.dataPath + "/../UIPrefabRef.txt", content, false);
            OpenTxt();
        }

        void OpenTxt()
        {
            string path = Application.dataPath + "/../UIPrefabRef.txt";
            if (!File.Exists(path))
            {
                UnityEngine.Debug.LogError("path = " + path + " 不存在");
                EditorUtility.DisplayDialog("Error",@"文件不存在，请先执行""Find All""操作","确定");
                return;
            }

            Process pp = Process.Start(path);
        }

        void FindAllObject(GameObject go, Type componentType, string propertyName, List<PanelData> dic)
        {
            if (go == null) return;
            PanelData panelData = dic.Find(delegate(PanelData d)
            {
                return d.panel == go;
            }
            );
            if (panelData == null)
            {
                panelData = new PanelData();
                panelData.panel = go;
                dic.Add(panelData);
            }
            Component[] cs = go.GetComponentsInChildren(componentType, true);
            
            foreach (Component comp in cs)
            {
                if (comp == null) continue;
                System.Reflection.PropertyInfo propertyInfo = componentType.GetProperty(propertyName);
                UnityEngine.Object obj = propertyInfo.GetValue(comp, null) as UnityEngine.Object; 
                if(obj == null)
                {
                    EditorUtil.PrintLogPath(comp.gameObject,"Atlas 或者 Font为空");
                    continue;
                }
                AtlasData data = panelData.list.Find(delegate(AtlasData a)
                {
                    return a.resObj == obj;
                });
                if(data == null)
                {
                    data = new AtlasData();
                    data.resObj = obj;
                    data.resType = componentType;
                    UIAtlas atlas = obj as UIAtlas;
                    if (atlas != null && atlas.spriteMaterial != null && atlas.spriteMaterial.mainTexture != null)
                    {
                        int num = UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(atlas.spriteMaterial.mainTexture);
                        Texture tex = atlas.spriteMaterial.GetTexture("_AlphaTex");
                        if (tex != null) num += UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(tex);
                        data.totalMemeroyNum = num*1.0f/1024f/1024f;
                    }
                    else
                    {
                        data.totalMemeroyNum = EditorUtil.GetTotalMemoryNum(obj);
                    }
                    
                    panelData.totalMemeroyNum += data.totalMemeroyNum;
                    panelData.list.Add(data);
                }
                data.refGOList.Add(comp.gameObject);
            }
        }
    }

}
