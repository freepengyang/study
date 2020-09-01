/********************************************************************
	created:	2016/06/03
	created:	3:6:2016   9:17
	filename: 	E:\GameProject\Client\Branch\Client\Assets\Script\Common\CSMiscGizmos.cs
	file path:	E:\GameProject\Client\Branch\Client\Assets\Script\Common
	file base:	CSMiscGizmos
	file ext:	cs
	author:		Jzx
	
	purpose:	
*********************************************************************/
using UnityEngine;
using System.Collections;

public class CSMiscGizmos 
{
    /// <summary>
    /// 顺时针或者逆时针传入4个点
    /// </summary>
    /// <param name="point"></param>
    public static void DrawRect(Vector2[] point)
    {
        Gizmos.DrawLine(point[0], point[1]);
        Gizmos.DrawLine(point[1], point[2]);
        Gizmos.DrawLine(point[2], point[3]);
        Gizmos.DrawLine(point[3], point[0]);
    }


    public static void DrawCell(int x, int y)
    {
        Vector2[] point = new Vector2[4];
        CSCell cell = CSMesh.Instance.getCell(x, y);

       // Debug.Log(x+"|"+y+" = "+cell.MinX+","+cell.MinY+","+cell.MaxX+","+cell.MaxY);

        if (cell == null) return;
        point[0] = new Vector2(cell.WorldMinX, cell.WorldMinY);
        point[1] = new Vector2(cell.WorldMinX, cell.WorldMaxY);
        point[2] = new Vector2(cell.WorldMaxX, cell.WorldMaxY);
        point[3] = new Vector2(cell.WorldMaxX, cell.WorldMinY);
        
        CSMiscGizmos.DrawRect(point);
    }
}
