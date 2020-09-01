using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDrawCellGizmos : CSGameMgrBase2<CSDrawCellGizmos>
{

    public override bool IsDonotDestroy
    {
        get
        {
            return true;
        }
    }

#if ILRuntime
#else

#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        DrawMeshSceneUI();
    }

    void DrawMeshSceneUI()
    {
        //if(!CSConstant.IsLanuchMainPlayer)
        //{
        //    return;
        //}
        if(CSMainParameterManager.mainPlayerOldCell == null)
        {
            return;
        }
        List<CSCell> list = CSMesh.Instance.GetViewCell(CSMainParameterManager.mainPlayerOldCell);
        int minX = int.MaxValue;
        int maxX = -int.MaxValue;
        int minY = int.MaxValue;
        int maxY = -int.MaxValue;
        for (int i = 0; i < list.Count; i++)
        {
            CSCell cell = list[i];
            if (cell == null) continue;
            if (cell.Coord.x < minX) minX = cell.Coord.x;
            if (cell.Coord.x > maxX) maxX = cell.Coord.x;
            if (cell.Coord.y < minY) minY = cell.Coord.y;
            if (cell.Coord.y > maxY) maxY = cell.Coord.y;
        }
        CSMiscGizmos.DrawRect(new Vector2[]{
            new Vector2(minX,minY),
            new Vector2(minX,maxY),
            new Vector2(maxX,maxY),
            new Vector2(maxX,minY),
        });
        for (int i = 0; i < list.Count; i++)
        {
            DrawMeshCell(list[i]);
            DrawCellLabel(list[i]);
        }
    }

    void DrawMeshCell(CSCell cell)
    {
        if (cell == null) return;
        CSMiscGizmos.DrawCell(cell.Coord.x, cell.Coord.y);
    }
    static GUIStyle mGUIStyle = new GUIStyle();
    void DrawCellLabel(CSCell cell)
    {
        UnityEditor.Handles.BeginGUI();
        mGUIStyle.alignment = TextAnchor.MiddleCenter;
        mGUIStyle.fontSize = 20;
        GUI.color = Color.black;
        int x = (int)(cell.getKey / 100000);
        int y = (int)(cell.getKey % 100000);
        string str = x + "|" + y;
        UnityEditor.Handles.Label(new Vector3(cell.WorldMinX, cell.WorldMaxY, -cell.WorldPosition.y), str);
        UnityEditor.Handles.EndGUI();

        //CSDirectPath.DrawLastPaths();
        //CSDirectPath.DrawPaths();
    }
#endif
#endif




}
