using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;
namespace ExtendEditor
{
    public abstract class OperationTypeBase : EditorWindowBase<OperationTypeBase>
    {
        public class OperationData
        {
            public bool isFolder = true;
            public Object prefab;
            public Object instPrefab;
            public List<Component> compList = new List<Component>();
        }
        public abstract Type DefaultSelectType { get; }
        public abstract string OperationPath{get;}
        public List<Type> typeList = new List<Type>();
        public string[] typeNames = null;
        public int mCurSelectIndex = 0;
        public List<string> allOperationPrefabList = null;
        public List<OperationData> dataList = new List<OperationData>();
        public int dealIndex = -1;
        public Vector2 scrollPos = Vector2.zero;
        public override void OnGUI()
        {
            base.OnGUI();
            GetCurType();
            EditorGUILayout.LabelField("Prefab路径:"+OperationPath);
            mCurSelectIndex = EditorGUILayout.Popup("选择类型", mCurSelectIndex, typeNames);
            if (allOperationPrefabList == null)
            {
                allOperationPrefabList = new List<string>();
                FileUtility.GetDeepAssetPaths(OperationPath, allOperationPrefabList, ".prefab");
            }
            DrawCustom();
            DealProc();
            DrawPrefab();
        }

        public virtual void DrawCustom()
        {
            
        }


        void GetCurType()
        {
            if (typeList.Count == 0)
            {
                typeList = EditorUtil.GetUnityEngineAssemblyAllTypes();
                typeNames = new string[typeList.Count];
                for (int i = 0; i < typeList.Count; i++)
                {
                    if (typeList[i] == DefaultSelectType)
                    {
                        mCurSelectIndex = i;
                    }
                    typeNames[i] = typeList[i].ToString().Replace("UnityEngine.", "");
                }
            }
        }

        public bool DrawDealButton()
        {
            if (GUILayout.Button("获得所有Prefab数据"))
            {
                dataList.Clear();
                dealIndex = 0;
                return true;
            }
            return false;
        }

        void DealProc()
        {
            if (dealIndex != -1 && dealIndex < allOperationPrefabList.Count)
            {
                DealProc(allOperationPrefabList[dealIndex]);
                dealIndex++;
                EditorUtility.DisplayProgressBar("Progress", "处理中(" + dealIndex + "/" + allOperationPrefabList.Count+")", dealIndex * 1.0f / allOperationPrefabList.Count);

                if (dealIndex >= allOperationPrefabList.Count)
                {
                    dealIndex = -1;
                    dataList.Sort(CompareSort_Data);
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        void DealProc(string path)
        {
            UnityEngine.Object obj = FileUtility.GetObject(path);
            if (obj == null) return;
            GameObject go = obj as GameObject;
            if (go == null) return;
            OperationData data = new OperationData();
            data.prefab = go;
            Component[] comps = go.GetComponentsInChildren(typeList[mCurSelectIndex], true);
            data.compList.AddRange(comps);
            data.compList.Sort(CompareSort_Comp);
            dataList.Add(data);
        }

        public virtual int CompareSort_Comp(Component f,Component s)
        {
            return string.Compare(f.gameObject.name, s.gameObject.name);
        }

        public virtual int CompareSort_Data(OperationData f, OperationData s)
        {
            return string.Compare(f.prefab.name, s.prefab.name);
        }

        void DrawPrefab()
        {
            if (dataList.Count == 0) return;
            if (dealIndex != -1) return;
            try
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                for (int i = 0; i < dataList.Count; i++)
                {
                    DrawPrefab(dataList[i], i);
                }
                EditorGUILayout.EndScrollView();
            }
            catch (System.Exception ex)
            {

            }
        }

        public void ClonePrefab(OperationData data)
        {
            data.instPrefab = PrefabUtility.InstantiatePrefab(data.prefab);
            GameObject go = data.instPrefab as GameObject;

            GameObject uiRoot = GameObject.Find("UI Root");
            if (uiRoot != null)
            {
                go.transform.parent = uiRoot.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
            }

            Component[] comps = go.GetComponentsInChildren(typeList[mCurSelectIndex], true);
            data.compList.Clear();
            data.compList.AddRange(comps);
            data.compList.Sort(CompareSort_Comp);
        }

        public void DeleteInstPrefab(OperationData data)
        {
            if (data.instPrefab == null) return;
            GameObject.DestroyImmediate(data.instPrefab);
            GameObject go = data.prefab as GameObject;
            Component[] comps = go.GetComponentsInChildren(typeList[mCurSelectIndex], true);
            data.compList.Clear();
            data.compList.AddRange(comps);
            data.compList.Sort(CompareSort_Comp);
        }

        public void RevertPrefab(OperationData data)
        {
            if (data.instPrefab == null) return;
            PrefabUtility.RevertPrefabInstance(data.instPrefab as GameObject);
        }

        public void ApplyPrefab(OperationData data)
        {
            if (data.instPrefab == null) return;
            PrefabUtility.ReplacePrefab(data.instPrefab as GameObject,data.prefab, ReplacePrefabOptions.ConnectToPrefab);
        }

        void DrawPrefab(OperationData data,int index)
        {
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prefab:"+index, GUILayout.Width(100));
            if (GUILayout.Button(data.prefab.name, GUILayout.Width(150)))
            {
                data.isFolder = !data.isFolder;
                Selection.activeObject = data.instPrefab == null?data.prefab:data.instPrefab;
            }
            CustomDrawPrefab(data);
            EditorGUILayout.EndHorizontal();
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            DrawComp(data);
        }

        public virtual void CustomDrawPrefab(OperationData data)
        {

        }

        void DrawComp(OperationData data)
        {
            if (data.isFolder) return;
            for (int i = 0; i < data.compList.Count; i++)
            {
                DrawComp(data.compList[i],i);
            }
        }

        void DrawComp(Component comp,int index)
        {
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("       Compent:" + index + "", GUILayout.Width(100));
            if (GUILayout.Button(comp.gameObject.name,GUILayout.Width(200)))
            {
                Selection.activeGameObject = comp.gameObject;
            }
            CustomDrawComp(comp);
            EditorGUILayout.EndHorizontal();
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        }

        public virtual void CustomDrawComp(Component comp)
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = 0; i < dataList.Count; i++)
            {
                OperationData data = dataList[i];
                if (data.instPrefab != null)
                {
                    GameObject.DestroyImmediate(data.instPrefab);
                }
            }
        }
    }

}