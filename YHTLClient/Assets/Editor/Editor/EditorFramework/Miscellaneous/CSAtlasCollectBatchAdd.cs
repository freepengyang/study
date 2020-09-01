using UnityEngine;
using System.Collections;
using UnityEditor;
namespace ExtendEditor
{
    public class CSAtlasCollectBatchAdd : SelectionBase
    {

        [MenuItem("Tools/Miscellaneous/为每一个UI窗口添加一个Atlas收集器")]
        public static void CSAtlasCollectBatchAddProc()
        {
            EditorWindow win = GetWindow(typeof(CSAtlasCollectBatchAdd));
            win.Show();
        }

        int mWaitFrameCount = 1;//不停顿 在RefreshData中获得Atlas为null，可能需要时间导入吧
        int mCount = 0;
        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Add CSAtlasCollect To UIBase"))
            {
                base.BeginHandle();
            }

            if (GUILayout.Button("UnloadUnusedAssets"))
            {
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
            if (mCount < mWaitFrameCount)
            {
                mCount++;
                return;
            }
        }

        public override void OnInspectorUpdate()
        {
            base.OnInspectorUpdate();
            if (base.CanHandle())
            {
                mCount = 0;
                Object obj = base.GetCurHandleObj();
                GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                bool isTexChange = AddCAAtlasCollectToUIBase(go);

                PrefabUtility.ReplacePrefab(go,obj);
                DestroyImmediate(go);
                //AssetDatabase.SaveAssets();
                base.MoveHandle();
                if (!base.CanHandle())
                {
                    FNDebug.Log("done");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
        }

        public static bool AddCAAtlasCollectToUIBase(object obj, bool isRefreshData = true)
        {
            //GameObject go = obj as GameObject;
            //if (go == null) return false;
            //if (go.GetComponent<UIAtlas>() != null) return false;
            //CSAtlasCollect a = go.GetComponent<CSAtlasCollect>();
            //if (a == null)
            //    a = go.AddComponent<CSAtlasCollect>();
            //if (isRefreshData)
            //    return RefreshData(a);
            return false;
        }

        //static bool RefreshData(CSAtlasCollect tar)
        //{
        //    //Selection.activeObject = tar.gameObject;
        //    tar.ClearObjList();
        //    UISprite[] sps = tar.gameObject.GetComponentsInChildren<UISprite>(true);
        //    for (int i = 0; i < sps.Length; i++)
        //    {
        //        if (sps[i] == null)
        //        {
        //            Debug.Log("uisprite null " + tar.gameObject.name);
        //            continue;
        //        }
        //        if (sps[i].atlas == null || sps[i].atlas.spriteMaterial == null || sps[i].atlas.spriteMaterial.mainTexture == null)
        //        {
        //            //Debug.Log("uisprite atlas " + tar.gameObject.name + " " + sps[i].gameObject.name);
        //            continue;
        //        }
        //        Material mat = sps[i].atlas.spriteMaterial;
        //        tar.AddObjData(mat.mainTexture, mat.GetTexture("_AlphaTex"), sps[i].gameObject);
        //    }

        //    UITexture[] texs = tar.gameObject.GetComponentsInChildren<UITexture>(true);
        //    bool isTexChange = false;
        //    for (int i = 0; i < texs.Length; i++)
        //    {
        //        if (!isTexChange)
        //        {
        //            isTexChange = SaveUITexturePath(tar.gameObject, texs[i]);
        //        }
        //        else
        //        {
        //            SaveUITexturePath(tar.gameObject, texs[i]);
        //        }
                
        //        tar.AddObjData(texs[i].mainTexture, null, texs[i].gameObject);
        //    }
        //    //if (tar.objList.Count == 0)
        //    //    Debug.Log(tar.gameObject.name + " " + tar.objList.Count);
        //    return isTexChange;
        //}

        static bool SaveUITexturePath(GameObject go,UITexture tex)
        {
            if (tex == null || tex.mainTexture == null)
            {
                if (tex != null && go!=null)
                {
                    Transform trans = tex.transform;
                    string p = "";
                    while (trans != null)
                    {
                        p += trans.name + "->";
                        trans = trans.parent;
                    }
                    UnityEngine.Debug.LogError("tex maintex is null = " + go.name + " " + p);
                }
                
                return false;
            }
            string path = AssetDatabase.GetAssetPath(tex.mainTexture);
            if (!path.Contains("Assets/Resources/"))
            {
                UnityEngine.Debug.LogError("UITexture 的图片必须放到Resources下面 "+path);
                return false;
            }
            path = path.Replace("Assets/Resources/", "");
            path = path.Substring(0, path.LastIndexOf("."));
            //if (tex.TexturePath != path)
            //{
            //    tex.TexturePath = path;
            //    UnityEngine.Debug.Log(go.name+" "+path);
            //    return true;
            //}
            return false;
        }

    }
}

