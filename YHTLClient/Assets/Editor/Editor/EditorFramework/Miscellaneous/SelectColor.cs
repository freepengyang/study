using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
namespace ExtendEditor
{
    public class SelectColor : EditorWindowBase<SelectColor>
    {
        Vector2 mScrollPos = Vector2.zero;
        string enterHex = "";
        string changeHex = "";

        string enterDig = "";
        string changeDig = "";

        List<string> mList = new List<string>()
        {
            "ffd400",
            "00ccff",
            "cb55ff",
            "ff0000",
            "ffeebb",
            "81796a",
            "ffffff",
            "ff9900",
            "22e600",
            "c4b88e",
            "dddda9",
            "ffff99",
            "928078",
            "785035",
        };
        
        [MenuItem("Tools/Miscellaneous/SelectColor")]
        public static void SelectColorProc()
        {
            EditorWindow win = GetWindow(typeof(SelectColor));
            win.Show();
        }
        
        public override void OnGUI()
        {
            base.OnGUI();
            mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos);
            enterHex = EditorGUILayout.TextField("输入16进制颜色值", enterHex);
            if (GUILayout.Button("转化"))
            {
                int r = System.Convert.ToInt32(enterHex.Substring(0, 2), 16);
                int g = System.Convert.ToInt32(enterHex.Substring(2, 2), 16);
                int b = System.Convert.ToInt32(enterHex.Substring(4, 2), 16);
                Color color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1);
                changeHex = color.r + "," + color.g + "," + color.b;
            }
            EditorGUILayout.TextField(changeHex);

            enterDig = EditorGUILayout.TextField("输入10进制颜色值(x,x,x)", enterDig);
            if (GUILayout.Button("转化"))
            {
                string[] strs = enterDig.Split(',');
                if (strs.Length == 3)
                {
                    string r =  System.Convert.ToInt32(strs[0]).ToString("x2");
                    string g = System.Convert.ToInt32(strs[1]).ToString("x2");
                    string b = System.Convert.ToInt32(strs[2]).ToString("x2");

                    changeDig = r + g + b;
                }
            }
            EditorGUILayout.TextField(changeDig);

            for (int i = 0; i < mList.Count; i++)
            {
                int r = System.Convert.ToInt32(mList[i].Substring(0,2), 16);
                int g = System.Convert.ToInt32(mList[i].Substring(2,2), 16);
                int b = System.Convert.ToInt32(mList[i].Substring(4,2), 16);
                Color color = new Color(r / 255.0f, g / 255.0f, b / 255.0f,1);
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("编号" + i + " ", GUILayout.Width(40));
                GUILayout.Label(mList[i],GUILayout.Width(40));
                if (GUILayout.Button("Copy", GUILayout.Width(40))) FileUtility.CopyText(mList[i]);
                GUILayout.Label(string.Format("({0},{1},{2})", r, g, b), GUILayout.Width(40));
                if (GUILayout.Button("Copy", GUILayout.Width(40))) FileUtility.CopyText(string.Format("({0},{1},{2})", r, g, b));
                GUILayout.Label(string.Format("({0}f,{1}f,{2}f)", color.r.ToString("F2"), color.g.ToString("F2"), color.b.ToString("F2")), GUILayout.Width(40));
                if (GUILayout.Button("Copy", GUILayout.Width(40))) FileUtility.CopyText(string.Format("({0}f,{1}f,{2}f)", color.r.ToString("F2"), color.g.ToString("F2"), color.b.ToString("F2")));

                EditorGUILayout.ColorField(color, GUILayout.Width(200));

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

}