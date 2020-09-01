using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWingSpiritAttrBinder : UIBinder
{
    private UILabel lb_name;
    private UILabel lb_nextName;
    private UISpriteAnimation spriteAnimation;
    public CSAttributeInfo.KeyValue keyValue;
    public CSAttributeInfo.KeyValue keyValueNext;
    private bool needStarEffect = true;
    public bool NeedStarEffect => needStarEffect;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_nextName = Get<UILabel>("lb_nextName");
        spriteAnimation = Get<UISpriteAnimation>("lb_upgrade_effect");
    }

    public override void Bind(object data)
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        //当前属性
        if (keyValue != null)
            lb_name.text = $"[cbb694]{keyValue.Key}{CSString.Format(999)}[-][DCD5B8]{keyValue.Value}[-]";

        //下一星属性(分满级和非满级)
        if (keyValueNext != null)
            lb_nextName.text = $"[cbb694]{keyValueNext.Key}{CSString.Format(999)}[-][00FF0C]{keyValueNext.Value}[-]";
        else
            lb_nextName.text = $"[cbb694]{keyValue.Key}{CSString.Format(999)}[-][00FF0C]{CSString.Format(971)}[-]";
    }

    public void PlayStarEffect()
    {
        if (spriteAnimation != null && needStarEffect)
        {
            needStarEffect = false;
            spriteAnimation.gameObject.SetActive(true);
            CSEffectPlayMgr.Instance.ShowUIEffect(spriteAnimation.gameObject, "effect_dragon_levelup_add", 10, false, false);
            spriteAnimation.OnFinish = OnStarPlayFinish;
        }
    }

    void OnStarPlayFinish()
    {
        spriteAnimation.gameObject.SetActive(false);
        needStarEffect = true;
    }
    
    public override void OnDestroy()
    {
        lb_name = null;
        lb_nextName = null;
        spriteAnimation = null;
        keyValue = null;
        keyValueNext = null;
    }
}
