using System.Collections.Generic;
using UnityEngine;

public partial class UIWoLongPromptPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    ILBetterList<LongLiBase> baseItems = new ILBetterList<LongLiBase>(12);
    public override void Init()
    {
        base.Init();
        AddCollider();
    }

    public override void Show()
    {
        base.Show();
    }
    public void SetData(List<wolong.ActiveSkillGroupInfo> activeSkill)
    {
        mgrid.MaxCount = activeSkill.Count;
        for (int i = 0; i < activeSkill.Count; i++)
        {
            baseItems.Add(new LongLiBase(mgrid.controlList[i]));
            baseItems[i].Refresh(activeSkill[i]);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    class LongLiBase
    {
        GameObject go;
        UISprite icon;
        UILabel name;
        int groupid = 0;
        public LongLiBase(GameObject _go)
        {
            go = _go;
            icon = go.transform.Find("sp_buff").GetComponent<UISprite>();
            name = go.transform.Find("key").GetComponent<UILabel>();
            UIEventListener.Get(icon.gameObject).onClick = Click;
        }
        public void Refresh(wolong.ActiveSkillGroupInfo _info)
        {
            icon.spriteName = SkillTableManager.Instance.GetIconByGroupId(_info.skillGroup);
            name.text = SkillTableManager.Instance.GetNameByGroupId(_info.skillGroup);
            groupid = _info.skillGroup;
        }
        void Click(GameObject _go)
        {
            UIManager.Instance.CreatePanel<UIWoLongSkillTipsPanel>(p =>
            {
                (p as UIWoLongSkillTipsPanel).SetData(groupid, 0, 0, null);
            });
        }
    }
}
