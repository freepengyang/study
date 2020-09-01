using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSkillFlagManager : Singleton<CSSkillFlagManager>
{
    private CSSkillFlagWarrior skillFlagWarrior = null;
    private CSSkillFlagMaster skillFlagMasetr = null;
    public bool isYeManFlag;
    public int attackRange;
    public bool isCancle;
    public bool isHaveTarget;
    public bool isRelease;    

    public void Show(int skillId,int career ,int direction)
    {
        TABLE.SKILL tblSkill = null;
        if (SkillTableManager.Instance.TryGetValue(skillId, out tblSkill))
        {
            isRelease = false;
            if (career == ECareer.Warrior)
            {
                if (skillFlagWarrior == null)
                {
                    skillFlagWarrior = new CSSkillFlagWarrior();
                }
                skillFlagWarrior.Show(skillId, direction);
                isYeManFlag = true;
                attackRange = tblSkill.clientRange;
            }
            else if (career == ECareer.Master)
            {
                if (skillFlagMasetr == null)
                {
                    skillFlagMasetr = new CSSkillFlagMaster();
                }
                skillFlagMasetr.Show(skillId, direction);
            }
        }
    }

    public void Hide()
    {
        if (skillFlagWarrior != null)
        {
            skillFlagWarrior.Hide();
        }
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.Hide();
        }
        isYeManFlag = false;
        isRelease = true;
    }

    public void OnDrag(Vector3 localPosition, int direction)
    {
        localPosition = localPosition * 3.0f;
        if (skillFlagWarrior != null)
        {
            skillFlagWarrior.OnDrag(localPosition, direction);
        }
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.OnDrag(localPosition, direction);
        }
    }


    public void SetTarget(bool value)
    {
        if (skillFlagWarrior != null)
        {
            skillFlagWarrior.SetTarget(value);
        }
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.SetTarget(value);
        }
    }

    public bool IsCancle()
    {
        if(skillFlagWarrior != null)
        {
            return skillFlagWarrior.IsCancle();
        }
        if (skillFlagMasetr != null)
        {
            return skillFlagMasetr.IsCancle();
        }
        return false;
    }

    public void SetCancle(bool value)
    {
        if (skillFlagWarrior != null)
        {
            skillFlagWarrior.SetCancle(value);
        }
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.SetCancle(value);
        }
    }

    public int GetCoordX(int targetCoordX, int attackCoordX)
    {
        if (skillFlagMasetr != null && isRelease)
        {
            return skillFlagMasetr.GetCoordX(targetCoordX, attackCoordX);
        }
        return targetCoordX;
    }

    public int GetCoordY(int targetCoordY, int attackCoordY)
    {
        if (skillFlagMasetr != null && isRelease)
        {
            return skillFlagMasetr.GetCoordY(targetCoordY, attackCoordY);
        }
        return targetCoordY;
    }

    public CSCell GetTargetCell(CSCell mainPlayerCell)
    {
        if(mainPlayerCell == null)
        {
            return mainPlayerCell;
        }
        if (skillFlagMasetr != null && isRelease)
        {
            if(skillFlagMasetr.IsDrag())
            {
                int coordX = mainPlayerCell.Coord.x + skillFlagMasetr.offsetCoordX;
                int coordY = mainPlayerCell.Coord.y - skillFlagMasetr.offsetCoordY;
                CSCell cell = CSMesh.Instance.getCell(coordX, coordY);
                if(cell != null)
                {
                    return cell;
                }
            }
        }
        return mainPlayerCell;
    }

    public bool IsToTerrain()
    {
        if(skillFlagMasetr != null)
        {
            return skillFlagMasetr.IsDrag();
        }
        return false;
    }

    public bool IsTargetInRange(CSAvatar atarget)
    {
        if (skillFlagMasetr != null)
        {
            return skillFlagMasetr.IsInRange(atarget);
        }
        return true;
    }

    public void Reset()
    {
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.Reset();
        }
    }

    public void Destroy()
    {
        if (skillFlagWarrior != null)
        {
            skillFlagWarrior.Destroy();
            skillFlagWarrior = null;
        }
        if (skillFlagMasetr != null)
        {
            skillFlagMasetr.Destroy();
            skillFlagMasetr = null;
        }
        isYeManFlag = false;
        isHaveTarget = false;
        isRelease = false;
    }


}
