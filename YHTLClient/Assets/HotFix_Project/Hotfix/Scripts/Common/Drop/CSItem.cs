
//-------------------------------------------------------------------------
//道具
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;

public class CSItem
{
    public map.RoundItem BaseInfo;
    public TABLE.ITEM itemTbl;
    public bool isLoad;
    public int pickType;
    public GameObject goItem;
    public CSCell OldCell;
    private CSLabel lb_name;
    private CSSprite spr_icon;
    private CSSprite spr_effect;
    private GameObject mRoot;
    private CSResource mRes = null;
    private Transform mParent;

	/// <summary> 用于显示金钥匙道具特效 </summary>
	CSSceneEffect keyEffect;
	CSSceneEffect keyEffectRound;
	public long ItemId
    {
        get
        {
            if (BaseInfo != null)
            {
                return BaseInfo.itemId;
            }
            return 0;
        }
    }

    
    
    // public string Icon
    // {
    //     get
    //     {
    //        return GetIcon();
    //     }
    //     
    // }
    
    

    public static UIAtlas mItemIcon;
    public static UIAtlas ItemIcon
    {
        get { return mItemIcon; }
        set
        {
            if (value == null) return;
            mItemIcon = value;
            mItemIcon.IsUsePoolItem = false;
        }
    }

    /// <summary>
    /// 极限挑战中的治疗血球
    /// </summary>
    readonly int recoverBallId = 50000029;

    CSSceneEffect recoverBallEffect;


    private CSObjectPoolItem mPoolItem;
    public CSObjectPoolItem PoolItem
    {
        get { return mPoolItem; }
        set { mPoolItem = value; }
    }
    public void Init(map.RoundItem info, Transform tran)
    {
        isLoad = true;
        mParent = tran;
        BaseInfo = info;
        OldCell = CSMesh.Instance.getCell(info.x, info.y);
    }

    private bool InitModel()
    {
        if(goItem == null)
        {
            GameObject goClone = CSGameManager.Instance.GetStaticObj("sceneitem") as GameObject;
            if (goClone == null)
            {
                return false;
            }
            goItem = GameObject.Instantiate(goClone) as GameObject;
        }
        if (goItem == null)
        {
            return false;
        }
        NGUITools.SetParent(mParent, goItem);
        Vector3 vec = OldCell.LocalPosition2;
        vec.z = -10;
        Transform trans = goItem.transform;
        trans.localPosition = vec;

		mRoot = (mRoot != null) ? mRoot : UtilityObj.GetObject<GameObject>(trans, "root");
        if(mRoot != null)
        {
            //mRoot.SetActive(false);
            lb_name = (lb_name != null) ? lb_name : UtilityObj.GetOrAdd<CSLabel>(trans, "root/lb_head");
            spr_icon = (spr_icon != null) ? spr_icon : UtilityObj.GetOrAdd<CSSprite>(trans, "root/spr_icon");
            spr_effect = (spr_effect != null) ? spr_effect : UtilityObj.GetOrAdd<CSSprite>(trans, "root/spr_effect");
            return true;
        }
        return false;
    }

    
    
    void InitCombat()
    {
        if (itemTbl == null || goItem == null || lb_name == null) return;

        Color color = Color.white;
        CSMisc.ItemQulityColorDic.TryGetValue((uint)itemTbl.quality, out color);

        lb_name.fontSize = 15;
        lb_name.color = UtilityCsColor.Instance.GetColor(itemTbl.quality);
        lb_name.text = itemTbl.name;
        goItem.name = itemTbl.name;

        //float y = itemTbl.id == recoverBallId ? 45 : 30;
        lb_name.transform.localPosition = new Vector3(0, 30, -1);
    }

    private void SetModelSprite()
    {
        if (spr_icon == null || itemTbl == null)
        {
            return;
        }

        if (itemTbl.id == recoverBallId)
        {
            //spr_icon.SpriteName = "";
            //spr_icon.InitSetSpriteName = "";
            spr_icon.gameObject.SetActive(false);
            return;
        }

        if (ItemIcon == null)
        {
            GameObject go = CSGameManager.Instance.GetStaticObj("ItemIcon") as GameObject;
            if (go == null)
            {
                return;
            }
            ItemIcon = go.GetComponent<UIAtlas>();
        }
        if (ItemIcon == null)
        {
            return;
        }

    

        spr_icon.gameObject.SetActive(true);
        spr_icon.Atlas = ItemIcon;
        string icon = GetIcon();
        //Debug.Log("icon" + icon);
        spr_icon.InitSetSpriteName = icon;
        spr_icon.SpriteName = icon;
        if (OldCell != null)
        {
            bool isTrans = OldCell.isAttributes(MapEditor.CellType.Lucency);
            if (isTrans)
            {
                spr_icon.SetShader(CSShaderManager.GetShareMaterial(ItemIcon, EShareMatType.Transparent), new Vector4(1, 1, 1, 0.45f), Vector4.one);
            }
            else
            {
                spr_icon.SetShader(CSShaderManager.GetShareMaterial(ItemIcon, EShareMatType.Normal), Vector4.one, Vector4.one);
            }
        }
    }

    private void ShowEffects()
    {
        if (itemTbl == null || mRoot == null)
        {
            return;
        }
        if(spr_effect != null)
        {
            spr_effect.gameObject.SetActive(false);
        }

        if (itemTbl.id == recoverBallId)
        {
            float depth = CSMisc.GetDepth(OldCell, 0);
            Vector3 pos = new Vector3(0, -20, depth);
            if (recoverBallEffect == null)
            {
                recoverBallEffect = CSSceneEffectMgr.Instance.Create(mRoot.transform, 6016, pos);
            }
            else
            {
                recoverBallEffect.Play(pos);
            }

            return;
        }

        //金钥匙特效改成根据item img字段是否显示特效
        if (!string.IsNullOrEmpty(itemTbl.img)) {
			int effectId = 0;
			int.TryParse(itemTbl.img, out effectId);
			float tempY = mRoot.transform.localPosition.y + 185;
			float depth = mRoot.transform.localPosition.z + 1;
            Vector3 pos = new Vector3(0, tempY, depth);
            if (keyEffect == null)
            {
			    keyEffect = CSSceneEffectMgr.Instance.Create(mRoot.transform, effectId, pos);
            }
            else
            {
                keyEffect.Play(mRoot.transform, effectId, pos);
            }
        }
		//金钥匙特效
		if (itemTbl.id == 30030030)
		{
            if(keyEffectRound != null)
            {
                keyEffectRound   = CSSceneEffectMgr.Instance.Create(mRoot.transform, 6035, Vector3.zero);
            }
            else
            {
                keyEffectRound.Replay();
            }
		}
    }

    private void SetActive()
    {
        mRoot.SetActive(true);
        goItem.SetActive(true);
    }

    void OnLoadEffect(CSResource res)
    {
        if (this == null || spr_effect == null || res.MirrorObj == null) return;

        GameObject go = res.MirrorObj as GameObject;

        if (go == null) return;

        UIAtlas atlas = go.GetComponent<UIAtlas>();
        if (atlas == null) return;
        spr_effect.Atlas = atlas;

        spr_effect.transform.localScale = Vector3.one;
        int y = 0;
        //if (itemTbl != null && itemTbl.id == recoverBallId)
        //{
        //    y = -20;
        //    spr_effect.transform.localScale = new Vector3(0.7f, 0.7f);
        //}
        spr_effect.transform.localPosition = new Vector3(0, y, -2);
        spr_effect.gameObject.SetActive(true);
        CSEffectAnimation mEffectAni_3 = spr_effect.gameObject.GetComponent<CSEffectAnimation>();
        if (mEffectAni_3 == null)
        {
            mEffectAni_3 = spr_effect.gameObject.AddComponent<CSEffectAnimation>();
        }
        mEffectAni_3.RefreshNames();
        mEffectAni_3.setLoop(true);
    }

    public bool IsNeedRemove()
    {
        if (BaseInfo != null && (CSServerTime.Instance.TotalMillisecond > BaseInfo.endTime /*&& !CSScene.IsDaTaoSha*/))
        {
            return true;
        }
        return false;
    }

    public void Refresh()
    {
        if (BaseInfo == null)
        {
            return;
        }
        if (mParent == null || OldCell == null)
        {
            return;
        }

        if (ItemTableManager.Instance.TryGetValue(BaseInfo.itemConfigId, out itemTbl))
        {
            if(InitModel())
            {
                InitCombat();
                SetModelSprite();
                SetOldCell(true);
                SetActive();
                ShowEffects();
                UpdateShowInMiWu();
            }
        }
    }

    private void SetOldCell(bool b)
    {
        if (OldCell != null && OldCell.node != null)
        {
            if (b)
            {
                OldCell.node.AddItemID(ItemId);
            }
            else
            {
                OldCell.node.RemoveItemID(ItemId);
            }
        }
    }

    public void RemovePoolItem()
    {
        if (CSObjectPoolMgr.Instance != null)
        {
            CSObjectPoolMgr.Instance.RemovePoolItem(mPoolItem);
        }
        mPoolItem = null;
    }
    
    private void Hide()
    {
        if (goItem != null)
        {
            goItem.SetActive(false);
        }
    }

    private void ReleaseResCbk()
    {
        if (mRes != null)
        {
            mRes.ReleaseCallBack();
        }
    }

    private void ReleaseEffect()
    {
        if (recoverBallEffect != null)
        {
            CSSceneEffectMgr.Instance.Release(ref recoverBallEffect);
        }

        if (keyEffect != null)
        {
            CSSceneEffectMgr.Instance.Release(ref keyEffect);
        }
		if(keyEffectRound != null)
		{
			CSSceneEffectMgr.Instance.Release(ref keyEffectRound);
		}
	}

    private void DestroyEffect()
    {
        if (recoverBallEffect != null)
        {
            CSSceneEffectMgr.Instance.Destroy(recoverBallEffect);
            recoverBallEffect = null;
        }
        if (keyEffect != null)
        {
            CSSceneEffectMgr.Instance.Destroy(keyEffect);
            keyEffect = null;
        }
		if (keyEffectRound != null)
		{
			CSSceneEffectMgr.Instance.Destroy();
			keyEffectRound = null;
		}
	}

    public void Release()
    {
        pickType = 0;
        Hide();
        ReleaseEffect();
        SetOldCell(false);
        ReleaseResCbk();
        RemovePoolItem();
        //goItem = null;
    }

    public void Destroy()
    {
        if (spr_effect != null)
        {
            spr_effect.Atlas = null;
        }
        if (goItem != null)
        {
            GameObject.Destroy(goItem);
        }
		goItem = null;
        pickType = 0;
        DestroyEffect();
        SetOldCell(false);
        ReleaseResCbk();
        RemovePoolItem();
  
    }

    public void UpdateShowInMiWu()
    {
        if (!CSScene.IsLanuchMainPlayer) return;
        //if (!CSScene.IsMiWuScene) return;
        bool b = IsShowInMiWu();
        if (goItem != null)
        {
            goItem.transform.localScale = b ? Vector3.one : Vector3.zero;
        }
    }

    public bool IsShowInMiWu()
    {
        return true;
        //if (!CSScene.IsLanuchMainPlayer)
        //{
        //    return true;
        //}
        ////if (!CSScene.IsMiWuScene) return true;
        //if (CSCameraManager.Instance.MainCameraTrans == null)
        //{
        //    return true;
        //}
        //CSMisc.Dot2 dot;
        //if (CSAvatarManager.MainPlayerInfo.BuffInfo.IsHasBuff(80002) ||
        //   CSAvatarManager.MainPlayerInfo.BuffInfo.IsHasBuff(80004) ||
        //   CSAvatarManager.MainPlayerInfo.BuffInfo.IsHasBuff(80006))
        //{
        //    dot = TableLoader.Instance.MiWu2Range;
        //}
        //else
        //{
        //    dot = TableLoader.Instance.MiWu1Range;
        //}
        //CSMisc.Dot2 cameraPos = CSTouchEvent.dichotomyFind(CSScene.Sington.MainPlayer.CameraTrans.localPosition + CSScene.Sington.MainPlayer.CacheTransform.localPosition
        //    + new Vector3(0, -62, 0), CSCell.Size.x, CSCell.Size.y);
        //CSMisc.Dot2 d = OldCell.Coord - cameraPos;
        //d = d.Abs();
        //if (d.x <= dot.x && d.y <= dot.y)
        //{
        //    return true;
        //}
    }
    
    public string GetIcon()
    {
        //UnityEngine.Debug.Log("ffff" + itemTbl.type+ "||" + itemTbl.subType);
        int subType = itemTbl.subType;
        if (itemTbl.type == 1 && (subType == 1 || subType == 2))
        {
            var DicSundryInfos = Utility.GetGoldSundryInfo();
            if (DicSundryInfos == null)
                return itemTbl.icon;
            int count = BaseInfo.count;
            var itemInfos = DicSundryInfos[subType];
            //这里只循环3次
            if (itemInfos == null)
                return itemTbl.icon;
            int index = 0;
            for (int i = 0; i < itemInfos[1].Count; i++)
            {
                //i == 0 跳过 
                if (count > itemInfos[0][i] && i != 0)
                {
                    //UnityEngine.Debug.Log("num" + itemInfos[0][i]);
                    index++;
                }
            }

            //Debug.Log("iteminfo" +itemInfos[1][index] + "||" + index+"||" +count);
            
            return itemInfos[1][index].ToString();
        }
        else
        {
            return itemTbl.icon;
        }
    }
}
