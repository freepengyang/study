using UnityEngine;
using System.Collections;

public class CSPaths
{
    static CSBetterList<Node> pathArray = new CSBetterList<Node>();


    /// <summary>
    /// first count Direct path,second the astar path
    /// </summary>
    /// <param name="avatar"></param>
    /// <returns></returns>
    public static CSBetterList<Node> getPath(AvatarUnit avatar) 
    {
        pathArray.Clear();

        if (avatar.IsBeControl) return pathArray;

        if (avatar.OldCell == null || avatar.NewCell == null) return pathArray;

        if (avatar.OldCell.Coord.Equal(avatar.touchhCoord))
        {
            return pathArray;
        }

        Node startNode = CSMesh.Instance.getNode(avatar.IsMoving ? avatar.NewCell.Coord: avatar.OldCell.Coord);

        Node goalNode = CSMesh.Instance.getNode(avatar.touchhCoord);

        if (avatar.AvatarType != EAvatarType.MainPlayer)
        {
            pathArray = CSDirectPath.FindPath(startNode, goalNode);

            if (pathArray.Count != 0)
            {
                return pathArray;
            }

            pathArray = CSAStar.FindPath(startNode, goalNode, false, false);

            return pathArray;
        }
        else
        {
            pathArray = CSAStar.FindPath(startNode, goalNode, false, true);

            return pathArray;
        }
       
    }

    /// <summary>
    /// Keyboard
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static CSBetterList<Node> getPath(AvatarUnit avatar, int direction)
    {
        pathArray.Clear();

        if (avatar.IsBeControl) return pathArray;

        if (avatar == null) return pathArray;

        CSCell s_cell = avatar.IsMoving ? avatar.NewCell : avatar.OldCell;

        CSDirectPath.GetDirectionPath(direction, s_cell, ref pathArray);

        return pathArray;
    }
}
