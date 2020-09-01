using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCharacterPath
{
    private CSAvatar mAvatar = null;
    public bool IsCanCrossScene { get; set; }

    public CSCharacterPath(CSAvatar avatar)
    {
        mAvatar = avatar;
    }

    public Node GetNextTarget(CSBetterList<Node> paths, int touchTypeType, int TouchMove)
    {
        Node node = null;

        if (mAvatar == null) return null;

        if (paths != null && paths.Count > 0)
        {
            if (!GetAttackPath(paths)) return null;
            AdjustCrossPath(paths);
            node = GetFirstNode(paths);
        }
        else  
        {
            if (touchTypeType == ETouchType.Touch && TouchMove == EControlState.JoyStick)
            {
                AdjustObstructPath(paths, mAvatar.TouchEvent.Direction);

                node = GetFirstNode(paths);
            }
        }
       
        return node;
    }

    public bool IsSeparate(CSMisc.Dot2 tarDot)
    {
        Node tarNode = CSMesh.Instance.getNode(tarDot);
        if(tarNode == null)
        {
            return true;
        }
        if(!tarNode.IsCanCrossWall())
        {
            return true;
        }
        return false;
    }

    bool GetAttackPath(CSBetterList<Node> paths)
    {
        if (mAvatar.TouchEvent.Type == ETouchType.Attack)
        {
            CSMisc.Dot2 targetCoord = paths[paths.Count - 1].coord;

            if (!mAvatar.OldCell.Coord.Equal(targetCoord))
            {
                TABLE.SKILL tblSkill = mAvatar.TouchEvent.GetTableSkill();
                bool isCanReleaseSkill = false;
                if (mAvatar.TouchEvent.Target != null)
                {
                    CSCell targetCell = mAvatar.TouchEvent.Target.OldCell;
                    if (IsSeparate(targetCell.Coord))
                    {
                        targetCoord = targetCell.Coord;
                    }
                }
                if (tblSkill != null)
                {
                    isCanReleaseSkill = CSSkillTools.GetBestLaunchCoord(tblSkill.effectArea, tblSkill.clientRange, tblSkill.effectRange, mAvatar.OldCell.Coord, targetCoord);
                }
                if (isCanReleaseSkill)
                {
                    paths.Clear();
                    return false;
                }
            }
        }
        return true;
    }

    void AdjustCrossPath(CSBetterList<Node> paths)
    {
        Node first = paths[0];

        if (first.cell == null)
        {
            first.cell = CSMesh.Instance.getCell(first.coord.x, first.coord.y);
        }
        if (!IsCanCross(first))
        {
            paths.RemoveAt(0);
            FindCrossPath(paths,mAvatar.OldCell.node);
        }
        else if(!first.IsCanCrossWall())
        {
            CSAvatar target = mAvatar.TouchEvent.Target;
            if (target != null)
            {
                int dir = CSDirectPath.GetDirection(mAvatar.OldCell, 
                    target.OldCell.LocalPosition2, mAvatar.TouchEvent.Direction);
                AdjustObstructPath(paths, dir);
            }
        }
    }

    bool IsCanCross(Node node)
    {
        if (!node.isCanCrossNpc) return false;
        if (!IsCanCrossScene && !node.isProtect && node.avatarNum > 0) return false;
        return true;
    }

    private Node FindCrossPath(CSBetterList<Node> paths,Node start, int beginIndex = 0)
    {
        if (start == null) return null;

        CSBetterList<Node> assistList = null;

        int needRemoveIndex = -1;

        for (int i = beginIndex,iMax = paths.Count; i < iMax; i++)
        {
            Node goal = paths[i];

            if (goal.cell == null) goal.cell = CSMesh.Instance.getCell(goal.coord);

            if (goal.cell != null && i != (iMax - 1))
            {
                if (goal.bObstacle) continue;

                if (!IsCanCross(goal)) continue;
            }

            if (goal.cell != null)
            {
                needRemoveIndex = i;//往前寻找，找到一个可走的格子，在后面需要将这个格子从mpaths中删除，因为该格子已经在assistList的最后一个了
                assistList = CSAStar.FindPath(start, goal, true, true);
                break;
            }
        }
        if (needRemoveIndex != -1)
        {
            for (int i = needRemoveIndex; i >= beginIndex; i--)
            {
                paths.RemoveAt(i);
            }
        }
        else
        {
            paths.Clear();
        }

        if (assistList != null && assistList.Count > 0)
        {
            assistList.RemoveAt(0);//删除start 节点（不可走点）
            paths.Insert(0, assistList);
        }
        return null;
    }

    private void AdjustObstructPath(CSBetterList<Node> paths,int direction)
    {
        if(mAvatar.OldCell != null)
        {
            Node node = CSDirectPath.FindObstructPath(mAvatar.OldCell.node, direction);

            if(node != null)
            {
                if (paths == null)
                {
                    paths = new CSBetterList<Node>();
                }
                paths.Insert(0, node);
            }
        }
    }

    private Node GetFirstNode(CSBetterList<Node> paths)
    {
        Node node = null;

        if (paths != null && paths.Count > 0)
        {
            node = paths[0];

            if (node.cell == null)
            {
                node.cell = CSMesh.Instance.getCell(node.coord.x, node.coord.y);
            }
            paths.RemoveAt(0);

            if (!node.IsCanCrossWall())
            {
                return null;
            }
        }
        return node;
    }

    public void AdjustAttackPath(CSCell oldCell)
    {
        if (mAvatar == null)
        {
            return;
        }
        TouchData touchData = mAvatar.TouchEvent;
        if (touchData == null)
        {
            return;
        }
        if (touchData.Type == ETouchType.Attack)
        {
            CSBetterList<Node> list = mAvatar.PathData.PathArray;
            if (list.Count == 0)
            {
                if (touchData.Target != null && 
                    touchData.Target.AvatarType != EAvatarType.MainPlayer)
                {
                    //找当前位置四周一个可走的区域，如果没有找到，不处理,如果对象是主角，不处理
                    if (!touchData.Target.IsMoving)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            CSMisc.Dot2 dot = CSMisc.dirList[i];
                            dot = dot + oldCell.Coord;
                            Node node = CSMesh.Instance.getNode(dot);
                            if (node != null && !node.bObstacle)
                            {
                                list.Add(oldCell.node);
                                list.Add(node);
                                list.Add(oldCell.node);//攻击点
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
