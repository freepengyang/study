using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;
using user;

public enum AvatarModelType
{
    AMT_Weapon = 1,
    AMT_Cloth = 2,
}

public static class AvatarModelHelper
{
    static void LoadAvatarWeapon(this GameObject gameObject, int itemId, int fashionId)
    {
        if (null == gameObject)
            return;

        //有时装先设置时装
        TABLE.FASHION fashionItem = null;
        if (FashionTableManager.Instance.TryGetValue(fashionId, out fashionItem) && fashionItem.weaponryModel != 0)
        {
            // CSEffectPlayMgr.Instance.Recycle(gameObject);
            CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, fashionItem.weaponryModel.ToString(),
                ResourceType.UIWeapon);
            gameObject.CustomActive(true);
            return;
        }

        //再设置装备
        TABLE.ITEM weaponItem = null;
        if (ItemTableManager.Instance.TryGetValue(itemId, out weaponItem))
        {
            // CSEffectPlayMgr.Instance.Recycle(gameObject);
            CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, weaponItem.model.ToString(), ResourceType.UIWeapon);
            gameObject.CustomActive(true);
            return;
        }

        //回收
        CSEffectPlayMgr.Instance.Recycle(gameObject);
        gameObject.CustomActive(true);
    }

    static void LoadAvatarCloth(this GameObject gameObject, int itemId, int fashionId, int sex)
    {
        if (null == gameObject)
            return;

        //显示时装
        TABLE.FASHION fashionItem = null;
        if (FashionTableManager.Instance.TryGetValue(fashionId, out fashionItem) && fashionItem.clothesModel != 0)
        {
            // CSEffectPlayMgr.Instance.Recycle(gameObject);
            CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, fashionItem.clothesModel.ToString(),
                ResourceType.UIPlayer);
            gameObject.CustomActive(true);
            return;
        }

        //显示装备
        TABLE.ITEM clothItem = null;
        if (ItemTableManager.Instance.TryGetValue(itemId, out clothItem))
        {
            // CSEffectPlayMgr.Instance.Recycle(gameObject);
            CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, clothItem.model.ToString(), ResourceType.UIPlayer);
            gameObject.CustomActive(true);
            return;
        }

        //显示裸模
        int defaultClothId = sex == 1 ? 615000 : 625000;
        // CSEffectPlayMgr.Instance.Recycle(gameObject);
        CSEffectPlayMgr.Instance.ShowUIEffect(gameObject, defaultClothId.ToString(), ResourceType.UIPlayer);
        gameObject.CustomActive(true);
    }

    public static void LoadAvatarModel(this GameObject gameObject, RoleBrief roleBrief,
        AvatarModelType eAvatarModelType)
    {
        if(null != roleBrief)
        {
            if (eAvatarModelType == AvatarModelType.AMT_Weapon)
            {
                gameObject.LoadAvatarModel(roleBrief.weapon, roleBrief.fashionId, roleBrief.sex, eAvatarModelType);
            }
            else if (eAvatarModelType == AvatarModelType.AMT_Cloth)
            {
                gameObject.LoadAvatarModel(roleBrief.armor, roleBrief.fashionId, roleBrief.sex, eAvatarModelType);
            }
        }
        else
        {
            gameObject.LoadAvatarModel(0, 0, 0, eAvatarModelType);
        }
    }

    public static void LoadAvatarModel(this GameObject gameObject, int itemId, int fashionId, int sex,
        AvatarModelType eAvatarModelType)
    {
        if (eAvatarModelType == AvatarModelType.AMT_Weapon)
        {
            gameObject.LoadAvatarWeapon(itemId, fashionId);
        }
        else if (eAvatarModelType == AvatarModelType.AMT_Cloth)
        {
            gameObject.LoadAvatarCloth(itemId, fashionId, sex);
        }
    }


    private static void LoadSceneAvatarBody(GameObject anchor, int modelId, int fashionBodyId,
        int motion, int direction, int resType = ResourceType.PlayerAtlas)
    {
        int armor = Utility.GetBodyModel(modelId, fashionBodyId, 0);
        if (armor > 0)
        {
            CSEffectPlayMgr.Instance.Recycle(anchor);
            string model = CSMisc.GetCombineModel(armor,  motion,  direction);
            CSEffectPlayMgr.Instance.ShowUIEffect(anchor, model, resType);
        }
    }

    private static void LoadSceneAvatarWeapon(GameObject anchor, int weaponId, int fashionWeaponId,
        int motion, int direction, int resType = ResourceType.PlayerAtlas)
    {
        int weaponModeID = Utility.GetWeaponModel(weaponId, fashionWeaponId);
        if (weaponModeID > 0)
        {
            CSEffectPlayMgr.Instance.Recycle(anchor);
            string model = CSMisc.GetCombineModel(weaponModeID,  motion,  direction);
            CSEffectPlayMgr.Instance.ShowUIEffect(anchor, model, resType);
        }
    }

    public static void LoadSceneAvatarModel(GameObject anchor, int modelId, int fashionModelId, int motion,
        int direction, int type, int resType = ResourceType.PlayerAtlas)
    {
        if (type == ModelStructure.Body)
        {
            LoadSceneAvatarBody(anchor, modelId, fashionModelId, motion, direction);
        }
        else if (type == ModelStructure.Weapon)
        {
            LoadSceneAvatarWeapon(anchor, modelId, fashionModelId, motion, direction, resType);
        }
    }

    public static void LoadAvatarModel(GameObject objClothes, GameObject objWeapon, int clothesId, int weaponId,
        int fashionClothesId, int fashionWeaponId, int suitId, int sex)
    {
        //套装
        if (suitId != 0)
        {
            TABLE.FASHION fashionItem;
            if (FashionTableManager.Instance.TryGetValue(suitId, out fashionItem))
            {
                if (fashionItem.clothesModel != 0)
                {
                    // CSEffectPlayMgr.Instance.Recycle(objClothes);
                    CSEffectPlayMgr.Instance.ShowUIEffect(objClothes, fashionItem.clothesModel.ToString(),
                        ResourceType.UIPlayer);
                    objClothes.CustomActive(true);
                }
                else
                {
                    //显示裸模
                    int defaultClothId = sex == 1 ? 615000 : 625000;
                    // CSEffectPlayMgr.Instance.Recycle(objClothes);
                    CSEffectPlayMgr.Instance.ShowUIEffect(objClothes, defaultClothId.ToString(), ResourceType.UIPlayer);
                    objClothes.CustomActive(true);
                }

                if (fashionItem.weaponryModel != 0)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(objWeapon, fashionItem.weaponryModel.ToString(),
                        ResourceType.UIWeapon);
                    objWeapon.CustomActive(true);
                }
                else
                {
                    CSEffectPlayMgr.Instance.Recycle(objWeapon);
                }

                return;
            }
        }

        objClothes.LoadAvatarCloth(clothesId, fashionClothesId, sex);
        objWeapon.LoadAvatarWeapon(weaponId, fashionWeaponId);
    }

    /// <summary>
    /// 加载时装武器称号
    /// </summary>
    /// <param name="objClothes">身体节点</param>
    /// <param name="objWeapon">武器节点</param>
    /// <param name="objTitle">称号节点</param>
    /// <param name="clothesId">衣服装备Id</param>
    /// <param name="weaponId">武器装备Id</param>
    /// <param name="fashionClothesId">时装衣服Id</param>
    /// <param name="fashionWeaponId">时装武器Id</param>
    /// <param name="fashionTitleId">时装称号Id</param>
    /// <param name="suitId">套装Id</param>
    /// <param name="sex">性别</param>
    public static void LoadAvatarModel(GameObject objClothes, GameObject objWeapon, GameObject objTitle, int clothesId,
        int weaponId, int fashionClothesId, int fashionWeaponId, int fashionTitleId, int suitId, int sex)
    {
        LoadAvatarModel(objClothes, objWeapon, clothesId, weaponId, fashionClothesId, fashionWeaponId, suitId, sex);
        if (fashionTitleId > 0 && objTitle != null)
        {
            TABLE.FASHION fashionItem;
            if (FashionTableManager.Instance.TryGetValue(fashionTitleId, out fashionItem))
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(objTitle, fashionItem.titleModel.ToString(),
                    ResourceType.UIEffect);
            }
        }
        else
        {
            CSEffectPlayMgr.Instance.Recycle(objTitle);
        }
    }
}