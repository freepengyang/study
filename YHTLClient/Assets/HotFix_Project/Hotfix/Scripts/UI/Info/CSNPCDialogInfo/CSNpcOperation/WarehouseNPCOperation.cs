using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WarehouseNPCOperation : SpecialNpcOperationBase
{
    public override bool DoSpecial(CSAvatar avatar)
    {
        UIManager.Instance.CreatePanel<UIBagPanel>(p =>
        {
            (p as UIBagPanel).SetWarehouseOpenMode(false);
            (p as UIBagPanel).SelectChildPanel(2);
        });
        return true;
    }
}
