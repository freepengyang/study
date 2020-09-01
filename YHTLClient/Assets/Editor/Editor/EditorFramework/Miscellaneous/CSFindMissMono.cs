using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
namespace ExtendEditor
{
    public class CSFindMissMono : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/CSFindMissMono")]
        public static void CSFindMissMonoProc()
        {
            EditorWindow win = GetWindow(typeof(CSFindMissMono));
            win.Show();
        }


        public override void OnGUI()
        {
            base.OnGUI();
            
            if(GUILayout.Button("Deal"))
            {
                base.BeginHandle();
            }
            if (base.CanHandle())
            {
                GameObject g = base.GetCurHandleObj() as GameObject;
                if (g != null)
                {
                    FindMiss(g);
                }
                base.MoveHandle();
            }
        }

        void FindMiss(GameObject go)
        {
            if (go == null) return;
            Transform[] scripts = go.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < scripts.Length;i++ )
            {
                if (scripts[i] != null)
                {
                    Component[] comps = scripts[i].GetComponents<Component>();
                    foreach (Component c in comps)
                    {
                        if (c == null)
                        {
                            PrintPaht(scripts[i].transform);
                        }
                    }
                }
            }
        }

        void PrintPaht(Transform trans)
        {
            Transform tr = trans;
            string s = "";
            while (tr != null)
            {
                s += tr.name+"->";
                tr = tr.parent;
            }
            FNDebug.Log("Miss = "+s);
        }
    }
}

