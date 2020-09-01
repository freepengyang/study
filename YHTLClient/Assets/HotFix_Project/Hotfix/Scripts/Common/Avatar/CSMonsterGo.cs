using UnityEngine;

public class CSMonsterGo : CSAvatarGo
{
    public override void Init(CSAvatar avatar)
    {
        base.Init(avatar);

        Transform root = avatar.CacheRootTransform;

        if (root != null)
        {
            CSModelModule module = avatar.ModelModule;

            if (module != null)
            {
                Owner.Model.InitPart(module);
            }

            CSMonster monster = Owner as CSMonster;

            if (monster != null)
            {
                monster.CacheTransform.localPosition = monster.GetPosition();
                monster.InitHead();
                monster.InitBottom();
                monster.ResetNpcMonster(false,false);
            }
        }
    }

    public override void OnHit(CSAvatar clicker)
    {
        if(clicker == null)
        {
            return;
        }
        if(Owner == null || Owner.IsDead)
        {
            //Debug.Log("MonsterGo: owner is null or isDead");
            return;
        }
        CSModel clickerModel = clicker.Model;
        if(clickerModel != null && clickerModel.Bottom.Go != null)
        {
            Owner.Model.AttachBottom(clickerModel.Bottom);
            clickerModel.ShowSelectAndHideOtherBottom(ModelStructure.Bottom);
            if(Owner.head != null)
            {
                Owner.head.Show();
            }
        }

        if(Owner.BaseInfo == null)
        {
            return;
        }

        CSAvatarInfo info = Owner.BaseInfo;
        
        if (UIManager.Instance.IsPanel<UIRoleSelectionInfoPanel>())
            UIManager.Instance.ClosePanel<UIRoleSelectionInfoPanel>();

        TABLE.MONSTERINFO monsterinfo = null;
        if (MonsterInfoTableManager.Instance.TryGetValue((int)info.ConfigId, out monsterinfo))
        {
            if (monsterinfo.type==2|| monsterinfo.type==5||
                (monsterinfo.type==1&&monsterinfo.quality>=3)||
                monsterinfo.type==8||monsterinfo.type==7)
            {
                UIManager.Instance.CreatePanel<UIMonsterSelectionInfoPanel>((f) =>
                {
                    (f as UIMonsterSelectionInfoPanel).ShowSelectData(info);
                });
            }
            else
            {
                if (UIManager.Instance.IsPanel<UIMonsterSelectionInfoPanel>())
                    UIManager.Instance.ClosePanel<UIMonsterSelectionInfoPanel>();
            }
        }
    }
}