using TABLE;
using UnityEngine;

public class GuildBufferItemData
{
    public long attr;
    public TABLE.UNIONBUFF buffer;
	public int position;
}

public class UIGuildBufferItemBinder : UIBinder
{
    UILabel lb_name;
    UILabel lb_level;
    UISprite sp_check;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lab_name");
        lb_level = Get<UILabel>("lab_lv");
        sp_check = Get<UISprite>("spr_Choose");
    }

    GuildBufferItemData bufferData;
    public override void Bind(object data)
    {
        bufferData = data as GuildBufferItemData;
        if (null == bufferData)
        {
            return;
        }

        if(null != lb_name)
        {
            lb_name.text = string.Empty;
            TABLE.CLIENTATTRIBUTE clientAttribute = null;
            if (/*null != bufferData.attr && */ClientAttributeTableManager.Instance.TryGetValue(bufferData.attr.key(),out clientAttribute))
            {
                lb_name.text = CSString.Format(clientAttribute.tipID);
            }
        }

        if(null != lb_level && null != bufferData.buffer)
        {
            lb_level.text = bufferData.buffer.Level.ToString();
        }

        int nextPosition = CSGuildInfo.Instance.NextPosition(CSGuildInfo.Instance.CurImproveId);
        sp_check.CustomActive(nextPosition == bufferData.position && !CSGuildInfo.Instance.ImproveFull);
    }

    public override void OnDestroy()
    {
        lb_name = null;
        lb_level = null;
        sp_check = null;
        bufferData = null;
    }
}