using System;
using System.Collections.Generic;
using TABLE;

public abstract class SpecialNpcOperationBase
{
    public abstract bool DoSpecial(CSAvatar avatar);

    public bool DoSpecial(int npcId)
    {
        if (NpcTableManager.Instance.TryGetValue(npcId, out NPC NpcInfo))
        {
            CSAvatar npc;
            if (NpcInfo.monsterId == 0)
            {
                npc = CSAvatarManager.Instance.GetAvatar(npcId, EAvatarType.NPC);
            }
            else
            {
                npc = CSAvatarManager.Instance.GetAvatar(NpcInfo.monsterId, EAvatarType.Monster);
            }

            return DoSpecial(npc);
        }

        return false;
    }
}

public class CSSpeNpcOperaMgr
{
    /// <summary>
    /// 通过NPC 打开特殊面板   --------  根据 NPC表的 func 创建脚本
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public static bool DoSpecialNpc(int npcId)
    {
        string func = NpcTableManager.Instance.GetNpcFunc(npcId);
        return DoSpecialNpc(npcId, func);
    }

    /// <summary>
    /// 通过NPC 打开特殊面板   --------  根据 NPC表的 func 创建脚本
    /// </summary>
    /// <param name="npcId"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static bool DoSpecialNpc(int npcId, string func)
    {
        if (string.IsNullOrEmpty(func)) return false;
        if (int.TryParse(func, out int panelId))
        {
            UtilityPanel.JumpToPanel(panelId);
            return true;
        }

        if (!string.IsNullOrEmpty(func) && func != "null")
        {
            try
            {
                string name = string.Format("{0}NPCOperation", func);
                Type type = Type.GetType(name);
                if (type != null)
                {
                    SpecialNpcOperationBase special = Activator.CreateInstance(type) as SpecialNpcOperationBase;

                    if (special != null)
                    {
                        return special.DoSpecial(npcId);
                    }
                }
            }
            catch (Exception e)
            {
                FNDebug.LogError($"SpecialNpcOperation DoSpecial Fial :  npcId is  {npcId}  :  {e}");
            }
        }

        return false;
    }
}

public class CSNpcOperaButtonMgr
{
    /// <summary>
    /// 通过任务类型 获取按钮   --------  根据 任务枚举值 创建脚本
    /// </summary>
    public static Type GetDialogButtonForTask(TaskType type)
    {
        string name = string.Format("CSNPCDialog{0}Button", type.ToString().ToString());
        Type buttonType = Type.GetType(name);
        return buttonType;
    }

    /// <summary>
    /// 通过Npc Id 获取按钮   --------  根据 NPC表的 func 创建脚本
    /// </summary>
    public static Type GetDialogButtonForNpc(int npcId)
    {
        string func = NpcTableManager.Instance.GetNpcFunc(npcId);
        if (string.IsNullOrEmpty(func)) return null;
        string name = string.Format("CSNPCDialog{0}Button", func);
        Type buttonType = Type.GetType(name);
        return buttonType;
    }
}