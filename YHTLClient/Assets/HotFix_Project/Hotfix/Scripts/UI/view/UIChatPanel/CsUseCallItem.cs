using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsUseCallItem : CSInfo<CsUseCallItem>
{
    Map<long, UISummonPanel> MapSummonPanel = new Map<long, UISummonPanel>();
    List<UISummonPanel> ListSummonPanel = new List<UISummonPanel>();
    public void AddPanel(long id, UISummonPanel panel)
    {
        if (MapSummonPanel.ContainsKey(id))
        {
            MapSummonPanel[id] = panel;
        }
        else
        {
            MapSummonPanel.Add(id, panel);
        }
        SetDep(id);
    }
    public void RemovePanel(long id)
    {
        if (MapSummonPanel.ContainsKey(id))
        {           
            GameObject.Destroy(MapSummonPanel[id].UIPrefab);
            MapSummonPanel.Remove(id);
        }    
    }
    public void RemoveAllSummonPanel()
    {
        List<long> SummonPanelKeys = new List<long>(MapSummonPanel.Keys);
        for (int i = 0; i < SummonPanelKeys.Count; i++)
        {
            RemovePanel(SummonPanelKeys[i]);
        }
    }
    public void SetDep(long id)
    {     
        UILayerMgr.Instance.SetLayer(MapSummonPanel[id].UIPrefab, MapSummonPanel[id].PanelLayerType);       
    }

    public override void Dispose()
    {
        RemoveAllSummonPanel();
    }
}
