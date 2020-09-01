using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EquipRecycleNPCOperation : SpecialNpcOperationBase
{
    public override bool DoSpecial(CSAvatar avatar)
    {
        UIManager.Instance.CreatePanel<UIBagPanel>(p =>
        {
            (p as UIBagPanel).SetRecycleOpenMode(true);
            (p as UIBagPanel).SelectChildPanel(4);
        });
        return true;
    }
}
