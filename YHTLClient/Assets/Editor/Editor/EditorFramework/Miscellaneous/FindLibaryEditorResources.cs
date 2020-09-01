using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
public class FindLibaryEditorResources : SelectionBase
{
    [MenuItem("Tools/Miscellaneous/FindLibaryEditorResources")]
    public static void RenameProc()
    {
        EditorWindow win = GetWindow(typeof(FindLibaryEditorResources));
        win.Show();
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (GUILayout.Button("Find"))
        {
            foreach (Object obj in mSelectObjs)
            {
                if (obj == null) continue;
                if (obj.GetType() != typeof(GameObject)) continue;
                GameObject go = (GameObject)obj;
                if (go == null) continue;
                UILabel[] labs = go.GetComponentsInChildren<UILabel>(true);
                foreach(UILabel lab in labs)
                {
                    if (lab == null) continue;
                    if (lab.trueTypeFont != null && lab.trueTypeFont.name.Contains("Lucida"))
                    {
                        FNDebug.LogError(go.name+" "+lab.name);
                    }
                }
            }
        }
    }
}
