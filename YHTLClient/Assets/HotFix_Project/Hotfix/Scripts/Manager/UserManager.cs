using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 用户数据管理
/// </summary>
public class UserManager : Singleton<UserManager>
{
    public static long mUserId = 0;

    public int RoleCount
    {
        get
        {
            return RoleBriefList.Count;
        }
    }

    public List<user.RoleBrief> RoleBriefList = new List<user.RoleBrief>();   //角色列表

    //删除角色
    public void UpdateRemoveRoleList(long roleId)
    {
        for (int i = 0; i < RoleBriefList.Count; i++)
        {
            if (RoleBriefList[i].roleId == roleId)
            {
                RoleBriefList.RemoveAt(i);
            }
        }

        CSConstant.RoleCount = RoleCount;
    }

    //更新角色列表
    public void UpdateRoleList(user.LoginResponse s_login)
    {
        if (s_login == null) return;

        RoleBriefList.Clear();

        for (int i = 0; i < s_login.roleList.Count; i++)
        {
            user.RoleBrief s_role = s_login.roleList[i];
            RoleBriefList.Add(s_role);
        }
        CSConstant.RoleCount = RoleCount;
        //TODO:ddn
        //if (s_login.recommendedNationSpecified)
        //    CSChooseCountryController.Instance.CurRecommendCountry = s_login.recommendedNation;
        //CSChooseCountryController.Instance.Country = s_login.openNations;
    }

    public void UpdateRoleList(user.RoleBrief rolebrief)
    {
        UpdateRemoveRoleList(rolebrief.roleId);
        RoleBriefList.Add(rolebrief);
        CSConstant.RoleCount = RoleCount;
    }

    public user.RoleBrief GetRoleBrief(long roleId)
    {
        for (int i = 0; i < RoleBriefList.Count; i++)
        {
            if (RoleBriefList[i].roleId == roleId)
            {
                return RoleBriefList[i];
            }
        }
        return null;
    }

}