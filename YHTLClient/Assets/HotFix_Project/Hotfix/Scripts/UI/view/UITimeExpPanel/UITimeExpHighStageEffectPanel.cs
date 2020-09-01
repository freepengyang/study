using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Collections;
using TABLE;
using UnityEngine;

public class HighStageEffectItemData : CSPool.IPoolItem
{
    public PoolHandleManager pooledHandle;
    public List<CSAttributeInfo.KeyValue> keyValue;
    public TABLE.PAODIANSHENFU item;
    public bool reached;
    public void OnRecycle()
    {
        if (null != keyValue)
        {
            for (int i = 0, max = keyValue.Count; i < max; ++i)
            {
                keyValue[i].OnRecycle(pooledHandle);
            }
            keyValue.Clear();
            pooledHandle.Recycle(keyValue);
            keyValue = null;
        }
        pooledHandle = null;
    }
}

public static class StringHelper
{
    static Stack<StringBuilder> stringBuilders = new Stack<StringBuilder>(32);

    public static StringBuilder Get()
    {
        if (stringBuilders.Count > 0)
        {
            stringBuilders.Peek().Clear();
            return stringBuilders.Pop();
        }
        return new StringBuilder(1024);
    }

    public static void Put(StringBuilder stringBuilder)
    {
        if (null != stringBuilder)
        {
            stringBuilders.Push(stringBuilder);
        }
    }
}

public class HighStageEffectItem : UIBinder
{
    protected UILabel lb_key;
    protected UILabel lb_value;
    protected HighStageEffectItemData mData;

    public override void Init(UIEventListener handle)
    {
        lb_key = Get<UILabel>("lb_key");
        lb_value = Get<UILabel>("lb_value");
    }

    public override void Bind(object data)
    {
        mData = data as HighStageEffectItemData;
        if(null != mData)
        {
            if (null != lb_key && null != mData.item)
            {
                //达到1阶
                string colorStr = UtilityColor.GetColorString(mData.reached ? ColorType.MainText : ColorType.WeakText);
                lb_key.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(mData.reached ? 59 : 60), colorStr,mData.item.Rank);
            }
            if (null != lb_value && null != mData.keyValue)
            {
                string mainColor = UtilityColor.GetColorString(ColorType.MainText);
                string attrValueColor = UtilityColor.GetColorString(mData.reached ? ColorType.Green : ColorType.WeakText);

                var sb = StringHelper.Get();
                for (int i = 0, max = mData.keyValue.Count; i < max; ++i)
                {
                    if (i > 0)
                        sb.AppendLine();
                    if (mData.reached)
                    {
                        sb.AppendFormat(ClientTipsTableManager.Instance.GetClientTipsContext(61), mainColor, mData.keyValue[i].Key, attrValueColor, mData.keyValue[i].Value);
                    }
                    else
                    {
                        sb.AppendFormat(ClientTipsTableManager.Instance.GetClientTipsContext(62), attrValueColor, mData.keyValue[i].Key, attrValueColor, mData.keyValue[i].Value);
                    }
                }
                lb_value.text = sb.ToString();
                StringHelper.Put(sb);
            }
        }
    }

    public override void OnDestroy()
    {
        lb_key = null;
        lb_value = null;
        mData = null;
    }
}

public partial class UITimeExpHighStageEffectPanel : UIBasePanel
{
    protected CSPool.Pool<HighStageEffectItemData> starAttrDatas;

    public override void Init()
    {
        base.Init();
        mBtnClose.onClick = Close;
        starAttrDatas = GetListPool<HighStageEffectItemData>();
    }

    public void RefreshAttributes()
    {
        var datas = mPoolHandleManager.GetSystemClass<List<HighStageEffectItemData>>();
        starAttrDatas.RecycleAllItems();
        var attrIds = mPoolHandleManager.GetSystemClass<RepeatedField<int>>();
        var attrValues = mPoolHandleManager.GetSystemClass<RepeatedField<int>>();
        int rank = 2;
        int star = 0;
        int id = PaoDianShenFuTableManager.Instance.make_id(star,rank);
        TABLE.PAODIANSHENFU current = null;
        while (PaoDianShenFuTableManager.Instance.TryGetValue(id, out current) && null != current)
        {
            attrIds.Clear();
            attrValues.Clear();
            attrIds.Add(current.clientRankPara);
            attrValues.Add(current.rankaddNum);
            var attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrIds, attrValues);
            var attrItemData = starAttrDatas.Get();
            attrItemData.keyValue = mPoolHandleManager.GetSystemClass<List<CSAttributeInfo.KeyValue>>();
            for (int i = 0; i < attrItems.Count; ++i)
            {
                attrItemData.item = current;
                attrItemData.reached = current.Rank <= CSTimeExpManager.Instance.Rank;
                attrItemData.pooledHandle = mPoolHandleManager;
                attrItemData.keyValue.Add(attrItems[i]);
            }
            datas.Add(attrItemData);
            attrItems.Clear();
            mPoolHandleManager.Recycle(attrItems);
            id = PaoDianShenFuTableManager.Instance.make_id(star, ++rank);
        }
        attrIds.Clear();
        attrValues.Clear();
        mPoolHandleManager.Recycle(attrIds);
        mPoolHandleManager.Recycle(attrValues);
        mGridEffects.Bind<HighStageEffectItemData, HighStageEffectItem>(datas,mPoolHandleManager);
        datas.Clear();
        mPoolHandleManager.Recycle(datas);

        //adapt
        Bounds boundsDesc = NGUIMath.CalculateRelativeWidgetBounds(mGridEffects.transform);
        mBG.height = 62 + (int)boundsDesc.size.y;
    }

    public override void Show()
    {
        base.Show();

        RefreshAttributes();
    }

    protected override void OnDestroy()
    {
        mGridEffects.UnBind<HighStageEffectItem>();
        mGridEffects = null;
        starAttrDatas = null;
        base.OnDestroy();
    }
}