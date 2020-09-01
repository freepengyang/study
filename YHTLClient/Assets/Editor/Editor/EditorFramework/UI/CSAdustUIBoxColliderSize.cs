using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
namespace ExtendEditor
{
    public class CSAdustUIBoxColliderSize : OperationTypeBase
    {

        [MenuItem("Tools/UI/修改Prefab的BoxCollider的大小")]
        public static void CSAdustUIBoxColliderSizeProc()
        {
            EditorWindow win = GetWindow(typeof(CSAdustUIBoxColliderSize));
            win.Show();
        }

        public override Type DefaultSelectType
        {
            get { return typeof(BoxCollider); }
        }

        public override string OperationPath
        {
            get { return Application.dataPath + "/Resources/UI/Prefabs"; }
        }

        public override void OnGUI()
        {
            base.OnGUI();
           
        }

        public override void DrawCustom()
        {
            base.DrawCustom();

            EditorGUILayout.LabelField("批处理所有UI Prefab的最小BoxCollider大小");

            if (GUILayout.Button("处理所有不小于60，并将UISprite的Collider选项去掉"))
            {
                DealAllPrefab();
            }

            bool isClick = base.DrawDealButton();
        }

        void DealAllPrefab()
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                OperationData data = dataList[i];
                
                for (int j = 0; j < data.compList.Count; j++)
                {
                    BoxCollider box = data.compList[j] as BoxCollider;
                    if (box == null) continue;
                    bool isChange = false;
                    Vector3 size = box.size;
                    if (size.x < 60)
                    {
                        size.x = 60;
                        isChange = true;
                    }
                    if (size.y < 60)
                    {
                        size.y = 60;
                        isChange = true;
                    }
                    if (isChange)
                    {
                        box.size = size;
                        UISprite sprite = box.GetComponent<UISprite>();
                        if (sprite != null) sprite.autoResizeBoxCollider = false;
                    }
                }
            }
            AssetDatabase.SaveAssets();
        }

        public override int CompareSort_Comp(Component f, Component s)
        {
            BoxCollider fb = f as BoxCollider;
            BoxCollider sb = s as BoxCollider;
            float minF = Mathf.Min(fb.size.x, fb.size.y);
            float minS = Mathf.Min(sb.size.x, sb.size.y);
            if (minF < minS) return -1;
            if (minF > minS) return 1;
            return 0;
        }

        public override void CustomDrawPrefab(OperationTypeBase.OperationData data)
        {
            base.CustomDrawPrefab(data);
            if (data.instPrefab == null && GUILayout.Button("Clone", GUILayout.Width(100)))
            {
                if (data.instPrefab != null)
                {
                    EditorUtility.DisplayDialog("Error", "已经克隆了Prefab", "确定");
                    return;
                }
                base.ClonePrefab(data);
            }

            if (data.instPrefab != null && GUILayout.Button("Delete", GUILayout.Width(100)))
            {
                base.DeleteInstPrefab(data);
            }

            if (data.instPrefab != null && GUILayout.Button("Revert", GUILayout.Width(100)))
            {
                base.RevertPrefab(data);
            }

            if (data.instPrefab != null && GUILayout.Button("Apply", GUILayout.Width(100)))
            {
                base.ApplyPrefab(data);
            }
        }

        public override void CustomDrawComp(Component comp)
        {
            base.CustomDrawComp(comp);
            BoxCollider box = comp as BoxCollider;
            GUILayout.Label("BoxSize="+box.size, GUILayout.Width(200));
        }
    }
}
