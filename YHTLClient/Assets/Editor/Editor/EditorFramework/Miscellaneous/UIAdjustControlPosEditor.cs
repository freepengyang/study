using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
public class UIAdjustControlPosEditor :  SelectionBase
{
    [MenuItem("Tools/UI/UIAdjustControlPosEditor")]
    public static void UIAdjustControlPosEditorProc()
    {
        EditorWindow win = GetWindow(typeof(UIAdjustControlPosEditor));
        win.Show();
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (GUILayout.Button("Deal"))
        {
            base.BeginHandle();
        }

        if (base.CanHandle())
        {
            Deal();
            base.MoveHandle();
            if (!base.CanHandle())
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }

    void Deal()
    {
        GameObject go = base.GetCurHandleObj() as GameObject;
        if (go == null) return;
        Adjust<UILabel>(go);
        Adjust<UISprite>(go);
        Adjust<UITexture>(go);
    }

    void Adjust<T>(GameObject go)where T:UIWidget
    {
        T[] ts = go.GetComponentsInChildren<T>(true);
        for (int i = 0; i < ts.Length; i++)
        {
            T t = ts[i];
            if (t == null) continue;
            UILabel lab = t as UILabel;
            if (lab != null)
            {
                if (lab.overflowMethod == UILabel.Overflow.ShrinkContent)
                {
                    lab.overflowMethod = UILabel.Overflow.ResizeFreely;
                }
            }
            else
            {
                UIWidget widget = t as UIWidget;
                if (widget != null)
                {
                    widget.width = widget.width % 2 == 0 ? widget.width : widget.width + 1;
                    widget.height = widget.height % 2 == 0 ? widget.height : widget.height + 1;
                }
            }
                
            SetGoLocalPostion(t.transform);
        }
    }

    void SetGoLocalPostion(Transform trans)
    {
        while (trans != null)
        {
            Vector3 relationToGO = trans.localPosition;
            trans.localPosition = new Vector3(Mathf.RoundToInt(relationToGO.x), Mathf.RoundToInt(relationToGO.y), Mathf.RoundToInt(relationToGO.z));
            trans = trans.parent;
        }
    }
}
