using System.Collections.Generic;

public class CSForceIncreaseInfo : CSInfo<CSForceIncreaseInfo>
{
    public Map<int, ForceIncreaseRedData> ForceIncreaseDic = new Map<int, ForceIncreaseRedData>();

    public CSBetterList<int> OpenFuncList
    {
        get;
        set;
    }

    public void Init()
    {

        RegisterData(RedPointType.PetSkillUpgrade, 1855, 26000);
        RegisterData(RedPointType.SkillUpgrade, 979, 11400);
        RegisterData(RedPointType.WolongUpGrade, 1167, 10210);
		RegisterData(RedPointType.LianTi, 1090, 12910);
        RegisterData(RedPointType.Wing, 1089, 11900);
        RegisterData(RedPointType.EnhanceIgnorePanel, 1091, 11603);
        RegisterData(RedPointType.Gem, 1092, 11604);
        RegisterData(RedPointType.TimeExp, 1088, 11500);
        RegisterData(RedPointType.ZhuFu, 1094, 11500);
        RegisterData(RedPointType.WearableWoLongEquip, 1838, 10100);
        RegisterData(RedPointType.PetRefine, 1854, 26002);
        RegisterData(RedPointType.Combine, 1884, 11605);
        
        RegisterData(RedPointType.FashionActive, 1093, 23001);
        RegisterData(RedPointType.FashionUpStar, 1883, 23001);
        
        //RegisterData(RedPointType.GemLevelUp,1885,11641);
        RegisterData(RedPointType.HandBookSetuped, 1890, 11300,(f)=>
        {
            mClientEvent.SendEvent(CEvent.OnAutoSetupHandBook);
        });
        RegisterData(RedPointType.HandBookSlotUnlock, 1889, 11300, (f) =>
        {
            mClientEvent.SendEvent(CEvent.OnPopUnlockHandBookMessageBox);
        });
        RegisterData(RedPointType.HandBookSetupedUpgradeLevel, 1888, 11302);
        RegisterData(RedPointType.HandBookSetupedUpgradeQuality, 1887, 11303);
    }

    private void RegisterData(RedPointType _RedPoint, int _ClientTipsIs, int _GameModel, System.Action<UIBase> actionCreate = null)
    {
        if (!ForceIncreaseDic.ContainsKey((int)_RedPoint))
        {
            ForceIncreaseDic.Add((int)_RedPoint,
                new ForceIncreaseRedData((int)_RedPoint, _ClientTipsIs, _GameModel, actionCreate));
        }
    }


    public override void Dispose()
    {
        ForceIncreaseDic?.Clear();
        ForceIncreaseDic = null;

    }
}

public class ForceIncreaseRedData
{
    public int RedPoint { get; set; }

    public int ClientTipsId { get; set; }

    public int GameModel { get; set; }

    public System.Action<UIBase> actionCreate;

    public ForceIncreaseRedData(int _RedPoint, int _ClientTipsId, int _GameModel, System.Action<UIBase> action)
    {
        RedPoint = _RedPoint;
        ClientTipsId = _ClientTipsId;
        GameModel = _GameModel;
        actionCreate = action;
    }
}