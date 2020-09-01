using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CSConfigInfo : CSInfo<CSConfigInfo>
{

    /// <summary>
    /// 道具拾取总开关
    /// </summary>
    bool allPropPickUpBool = true;
    Dictionary<int, int> propPickUpSwitch = new Dictionary<int, int>();

    ILBetterList<int> propPickUpIds = new ILBetterList<int>();
    ILBetterList<string> propPickUpStrs = new ILBetterList<string>();



    public int PropPickUpIdCount
    {
        get
        {
            if (propPickUpIds == null || propPickUpStrs == null) return 0;
            return propPickUpIds.Count;
        }
    }


    void InitPropPickUpSettings()
    {
        string sundry = SundryTableManager.Instance.GetSundryEffect(1135);
        if (!string.IsNullOrEmpty(sundry))
        {
            propPickUpIds?.Clear();
            propPickUpStrs?.Clear();
            var allStr = sundry.Split('&');
            for (int i = 0, len = allStr.Length; i < len; i++)
            {
                var pair = allStr[i].Split('#');
                if (pair.Length < 2) continue;
                int id = 0;
                if (!int.TryParse(pair[0], out id)) continue;
                propPickUpIds.Add(id);
                propPickUpStrs.Add(pair[1]);
                propPickUpSwitch.Add(id, 1);
            }
        }
    }


    public int GetPropPickUpTypeIdByIndex(int index)
    {
        if (propPickUpIds == null || propPickUpIds.Count <= index) return 0;
        return propPickUpIds[index];
    }

    public string GetPropPickUpNameByIndex(int index)
    {
        if (propPickUpStrs == null || propPickUpStrs.Count <= index) return "";
        return propPickUpStrs[index];
    }

    /// <summary>
    /// 道具拾取开关状态，参数传ITEM表PickUpType，已包含到道具拾取总开关状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool GetPropPickUpBool(int id)
    {
        bool allEnabled = GetBool(ConfigOption.AllItemPickUp);
        if (!allEnabled) return false;
        if (propPickUpSwitch == null || !propPickUpSwitch.ContainsKey(id)) return false;
        return propPickUpSwitch[id] == 1;
    }


    public void SetPropPickUpBool(int id, bool value)
    {
        if (propPickUpSwitch == null || !propPickUpSwitch.ContainsKey(id))
            return;

        var oldValue = propPickUpSwitch[id];
        var currentValue = value ? 1 : 0;
        if (oldValue != currentValue)
        {
            propPickUpSwitch[id] = currentValue;
            SavePropPickUpSettingToLocal(id, currentValue);
        }
    }


    /// <summary>
    /// 获取装备拾取三个小开关状态，该接口已包含装备拾取总开关状态
    /// </summary>
    /// <param name="configOption"></param>
    /// <returns></returns>
    public bool GetEquipPickUpBool(ConfigOption configOption)
    {
        switch (configOption)
        {
            case ConfigOption.BYEquipPickUpLvSwitch:
            case ConfigOption.BYEquipPickUpQualitySwitch:
            case ConfigOption.WLEquipPickUpLvSwitch:
                return GetInt(ConfigOption.AllEquipPickUp) == 1 && GetInt(configOption) == 1;
        }
        return GetInt(configOption) == 1;
    }


    public int GetAutoReturnTime()
    {
        int index = GetInt(ConfigOption.AutoGoBackTime);
        if (index < 0 || returnTimeList == null || index >= returnTimeList.Count) return 0;
        return returnTimeList[index];
    }

    public int GetAutoTransferTime()
    {
        int index = GetInt(ConfigOption.AutoRandomDeliveryTime);
        if (index < 0 || transferTimeList == null || index >= transferTimeList.Count) return 0;
        return transferTimeList[index];
    }


    void SavePropPickUpSettingToLocal(int pickUpId, int value)
    {
        var id = CSMainPlayerInfo.Instance.ID;
        string key = $"{id}PropPickUp_{pickUpId}";
        PlayerPrefs.SetInt(key, value);
    }


    void LoadPropPickUpSettingFromLocal()
    {
        if (propPickUpSwitch == null || propPickUpIds == null)
        {
            FNDebug.LogError("拾取设置规则流程有误");
            return;
        }

        var id = CSMainPlayerInfo.Instance.ID;
        for (int i = 0; i < propPickUpIds.Count; i++)
        {
            var pickUpId = propPickUpIds[i];
            string key = $"{id}PropPickUp_{pickUpId}";
            int value = PlayerPrefs.GetInt(key, 1);
            propPickUpSwitch[pickUpId] = value;
        }
    }

}
