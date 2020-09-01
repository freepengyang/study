using instance;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class UIGuildBossInstancePanel : UIBasePanel
{
    public override UILayerType PanelLayerType => UILayerType.Resident;

    public override bool ShowGaussianBlur => false;

    InstanceInfo _InstanceInfo;
    TABLE.INSTANCE instance;


    Dictionary<int, int> rewardDic = new Dictionary<int, int>();
    List<UIItemBase> items = new List<UIItemBase>();


    public override void Init()
	{
		base.Init();
        mClientEvent.AddEvent(CEvent.GetEnterInstanceInfo, GetEnterInstanceInfo);
        mClientEvent.AddEvent(CEvent.ResInstanceInfo, UpdateInstanceInfo);
    }
	
	public override void Show()
	{
		base.Show();

        GetEnterInstanceInfo(0, null);
    }
	
	protected override void OnDestroy()
	{
        _InstanceInfo = null;
        instance = null;
        rewardDic?.Clear();
        rewardDic = null;
        UIItemManager.Instance.RecycleItemsFormMediator(items);
        base.OnDestroy();
	}


    void GetEnterInstanceInfo(uint id, object param)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        if (_InstanceInfo == null) return;
        if (!InstanceTableManager.Instance.TryGetValue(_InstanceInfo.instanceId, out instance)) return;

        mlb_title.text = instance.mapName;

        rewardDic.Clear();
        var awardsStrAtt = instance.show.Split('&');
        for (int s = 0; s < awardsStrAtt.Length; s++)
        {
            var str = awardsStrAtt[s].Split('#');
            if (str.Length > 1)
            {
                int itemId = 0;
                int num = 0;
                int.TryParse(str[0], out itemId);
                int.TryParse(str[1], out num);
                rewardDic.Add(itemId, num);
            }
        }
        
        Utility.GetItemByBoxid(rewardDic, mGrid, ref items, itemSize.Size50);
        if (items != null && items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].obj.CustomActive(false);
            }
            BindCoroutine(1, ShowItems());
        }
        

        UpdateInstanceInfo(id, param);
    }

    IEnumerator ShowItems()
    {
        yield return null;
        if (items != null && items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].obj.CustomActive(true);
            }
        }
    }


    void UpdateInstanceInfo(uint id, object param)
    {
        _InstanceInfo = CSInstanceInfo.Instance.GetInstanceInfo();
        if (_InstanceInfo == null) return;
        if (!InstanceTableManager.Instance.TryGetValue(_InstanceInfo.instanceId, out instance)) return;

        if (MonsterInfoTableManager.Instance.TryGetValue(instance.param, out TABLE.MONSTERINFO monsterinfo))
        {
            string color = _InstanceInfo.killedBoss >= 1 ? UtilityColor.Green : UtilityColor.Red;//策划表示总数量写死
            mlb_count.text = $"{UtilityColor.MainText}{monsterinfo.name}{color}({_InstanceInfo.killedBoss}/1)";
        }
    }
}
