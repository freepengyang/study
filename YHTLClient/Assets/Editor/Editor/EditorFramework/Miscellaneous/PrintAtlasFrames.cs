using UnityEngine;
using System.Collections;
using ExtendEditor;
using UnityEditor;
using System.Collections.Generic;
namespace ExtendEditor
{
    public class PrintAtlasFrames : SelectionBase
    {
      [MenuItem("Tools/Miscellaneous/PrintAtlasFrames")]
        public static void PrintAtlasFramesProc()
        {
            EditorWindow window = GetWindow(typeof(PrintAtlasFrames));
            window.Show();
        }

        Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Deal"))
            {
                base.BeginHandle();
            }

            for (int j = 0; j < 1000; j++)
            {
                if (base.CanHandle())
                {
                    GameObject go = base.GetCurHandleObj() as GameObject;
                    if (go != null)
                    {
                        UIAtlas atlas = go.GetComponent<UIAtlas>();
                        if (atlas != null)
                        {
                            int index = atlas.name.IndexOf("_");
                            string name = "";
                            if (index != -1)
                            {
                                name = atlas.name.Substring(0, index);
                            }
                            else
                            {
                                name = atlas.name;
                            }
                            if (!dic.ContainsKey(atlas.spriteList.Count))
                            {
                                dic.Add(atlas.spriteList.Count, new List<string>());
                            }
                            if (!dic[atlas.spriteList.Count].Contains(name))
                            {
                                dic[atlas.spriteList.Count].Add(name);
                            }
                        }
                    }
                    base.MoveHandle();
                    if (!base.CanHandle())
                    {
                        var cur = dic.GetEnumerator();
                        while (cur.MoveNext())
                        {
                            int key = cur.Current.Key;
                            List<string> list = cur.Current.Value;
                            string s = key + "帧 ";
                            for (int i = 0; i < list.Count; i++)
                            {
                                s += list[i] + " ";
                            }
                            FNDebug.Log(s);
                        }
                        break;
                    }
                }
            }
        }

    }

}
