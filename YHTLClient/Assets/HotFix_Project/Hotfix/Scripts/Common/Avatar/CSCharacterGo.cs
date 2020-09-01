using UnityEngine;
using System.Collections;

public class CSCharacterGo : CSAvatarGo
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
                if (Owner is CSCharacter player)
                    player.InitShieldEffect(true);
            }
        }
    }
}
