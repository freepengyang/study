using bag;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;

public enum AttrType
{
    Absolute = 0,
    Percent = 1,
    SkillLvAdd = 2,
    SkillDesc = 3,
}

public class CSAttributeInfo : CSInfo<CSAttributeInfo>
{
    //属性用空值补全,如果左边的比右边少
    public void AlignAttributes(PoolHandleManager poolHandle, RepeatedField<KeyValue> kvs,
        RepeatedField<KeyValue> nextKvs)
    {
        for (int i = kvs.Count; i < nextKvs.Count; ++i)
        {
            kvs.Add(nextKvs[i].Clone(poolHandle, true));
        }
    }

    //注意这里会分割属性与技能，传进来的是技能与属性混合的，特别注意会修改源数据
    public RepeatedField<KeyValue> SplitAttributesToAttrAndSkill(PoolHandleManager poolHandle,
        RepeatedField<KeyValue> kvs)
    {
        RepeatedField<KeyValue> skills = poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        skills.Clear();
        for (int i = 0; i < kvs.Count; ++i)
        {
            if (kvs[i].AttrType == AttrType.SkillDesc)
            {
                skills.Add(kvs[i]);
                kvs.RemoveAt(i--);
            }
        }

        return skills;
    }

    //注意这里会分割属性与技能，传进来的是技能与属性混合的，特别注意会修改源数据
    public RepeatedField<IdValue> SplitAttributesToAttrAndSkill(PoolHandleManager poolHandle,
        RepeatedField<IdValue> kvs)
    {
        RepeatedField<IdValue> skills = poolHandle.GetSystemClass<RepeatedField<IdValue>>();
        skills.Clear();
        for (int i = 0; i < kvs.Count; ++i)
        {
            if (kvs[i].AttrType == AttrType.SkillDesc)
            {
                skills.Add(kvs[i]);
                kvs.RemoveAt(i--);
            }
        }

        return skills;
    }

    //获取未合并的属性
    public RepeatedField<IdValue> GetUnMergedAttributes(PoolHandleManager poolHandle, RepeatedField<int> ids,
        RepeatedField<int> values)
    {
        if (null == ids || null == values || ids.Count != values.Count)
        {
            FNDebug.LogFormat("属性ID列数 与 属性Value列数 不一致 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<IdValue>>();
        }

        Dictionary<int, IdValue> cachedDic = poolHandle.GetSystemClass<Dictionary<int, IdValue>>();
        cachedDic.Clear();
        var kvs = poolHandle.GetSystemClass<RepeatedField<IdValue>>();
        kvs.Clear();
        for (int i = 0; i < ids.Count; ++i)
        {
            var id = ids[i];
            TABLE.CLIENTATTRIBUTE clientAttribute = null;
            if (!ClientAttributeTableManager.Instance.TryGetValue(id, out clientAttribute))
            {
                continue;
            }

            TABLE.ATTRIBUTE linkedAttribute = null;
            if (!AttributeTableManager.Instance.TryGetValue(id, out linkedAttribute))
            {
                continue;
            }

            var v = values[i];
            if (!cachedDic.ContainsKey(id))
            {
                var idValue = poolHandle.GetSystemClass<IdValue>();
                cachedDic.Add(id, idValue);
                idValue.attrItem = linkedAttribute;
                idValue.clientAttrItem = clientAttribute;
                idValue.attrValue = values[i];
                idValue.percentValue = clientAttribute.per;
            }
            else
            {
                var idValue = cachedDic[id];
                if (null != idValue)
                {
                    idValue.attrValue += v;
                }
            }
        }

        var it = cachedDic.GetEnumerator();
        while (it.MoveNext())
        {
            kvs.Add(it.Current.Value);
        }

        cachedDic.Clear();
        poolHandle.Recycle(cachedDic);
        return kvs;
    }

    public void SortByOrder(RepeatedField<KeyValue> datas)
    {
        var datasList = SortHelper.GetComparersList(32);
        datasList.AddCompare(SortHelper.IntGreat, 0);

        var handles = SortHelper.GetSortHandle(datas.Count);
        SortHelper.SortHandle handle = null;
        CSAttributeInfo.KeyValue data = null;
        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            handle = handles[i];
            data = datas[i];
            handle.handle = data;
            handle.intValue[0] = data.sortId;
        }
        SortHelper.Sort(handles, datasList);

        for (int i = 0, max = handles.Count; i < max; ++i)
        {
            datas[i] = handles[i].handle as CSAttributeInfo.KeyValue;
        }

        handles.OnRecycle();
        datasList.OnRecycle();
    }

    public RepeatedField<KeyValue> GetAttributes(PoolHandleManager poolHandle, IntArray ids,
    IntArray values, bool needSkillAttr = true, bool needMergeAttr = true)
    {
        if (/*null == ids || null == values || */ids.Count != values.Count)
        {
            FNDebug.LogFormat("属性ID列数 与 属性Value列数 不一致 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        }

        Dictionary<int, KeyValue> skillList = poolHandle.GetSystemClass<Dictionary<int, KeyValue>>();
        skillList.Clear();

        Dictionary<int, KeyValue> cachedDic = poolHandle.GetSystemClass<Dictionary<int, KeyValue>>();
        cachedDic.Clear();
        var kvs = poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        kvs.Clear();
        for (int i = 0; i < ids.Count; ++i)
        {
            var id = ids[i];
            TABLE.CLIENTATTRIBUTE clientAttribute = null;
            if (!ClientAttributeTableManager.Instance.TryGetValue(id, out clientAttribute))
            {
                continue;
            }

            TABLE.ATTRIBUTE linkedAttribute = null;
            if (!AttributeTableManager.Instance.TryGetValue(clientAttribute.id, out linkedAttribute))
            {
                continue;
            }

            if ((AttrType)clientAttribute.AttrType != AttrType.SkillDesc)
            {
                //属性走这里
                var v = values[i];
                int key = needMergeAttr ? clientAttribute.attached : id;
                if (!cachedDic.ContainsKey(key))
                {
                    var keyValue = poolHandle.GetSystemClass<KeyValue>();
                    cachedDic.Add(key, keyValue);
                    keyValue.clientAttribute = null;
                    ClientAttributeTableManager.Instance.TryGetValue(key, out keyValue.clientAttribute);
                }

                var handle = cachedDic[key];
                if (null == handle.keyValues)
                {
                    handle.keyValues = poolHandle.GetSystemClass<CSBetterLisHot<IdValue>>();
                }

                if (!handle.ContainsKey(linkedAttribute.id))
                {
                    var idValue = poolHandle.GetSystemClass<IdValue>();
                    idValue.attrItem = linkedAttribute;
                    idValue.clientAttrItem = clientAttribute;
                    idValue.attrValue = v;
                    idValue.percentValue = linkedAttribute.per;
                    handle.Add(idValue);
                }
                else
                {
                    var idValue = handle.GetValue(id);
                    if (null != idValue)
                    {
                        idValue.attrValue += v;
                    }
                }
            }
            else
            {
                if (!needSkillAttr)
                    continue;

                var skillId = values[i];
                TABLE.SKILL skillItem = null;
                if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
                    continue;
                //技能走这里
                if (!skillList.ContainsKey(skillId))
                {
                    var handle = poolHandle.GetSystemClass<KeyValue>();
                    handle.clientAttribute = null;
                    int key = needMergeAttr ? clientAttribute.attached : id;
                    ClientAttributeTableManager.Instance.TryGetValue(key, out handle.clientAttribute);
                    if (null == handle.keyValues)
                    {
                        handle.keyValues = poolHandle.GetSystemClass<CSBetterLisHot<IdValue>>();
                    }

                    handle.keyValues.Clear();

                    var idValue = poolHandle.GetSystemClass<IdValue>();
                    idValue.attrItem = linkedAttribute;
                    idValue.clientAttrItem = clientAttribute;
                    idValue.attrValue = skillId;
                    idValue.percentValue = linkedAttribute.per;
                    skillList.Add(skillId, handle);
                    handle.Add(idValue);
                }
            }
        }

        var it = cachedDic.GetEnumerator();
        while (it.MoveNext())
        {
            kvs.Add(it.Current.Value);
        }

        cachedDic.Clear();
        poolHandle.Recycle(cachedDic);
        var skillIt = skillList.GetEnumerator();
        while (skillIt.MoveNext())
        {
            kvs.Add(skillIt.Current.Value);
        }

        skillList.Clear();
        poolHandle.Recycle(skillList);
        return kvs;
    }

    public RepeatedField<KeyValue> GetAttributes(PoolHandleManager poolHandle, RepeatedField<int> ids,
        RepeatedField<int> values, bool needSkillAttr = true, bool needMergeAttr = true)
    {
        if (null == ids || null == values || ids.Count != values.Count)
        {
            FNDebug.LogFormat("属性ID列数 与 属性Value列数 不一致 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        }

        Dictionary<int, KeyValue> skillList = poolHandle.GetSystemClass<Dictionary<int, KeyValue>>();
        skillList.Clear();

        Dictionary<int, KeyValue> cachedDic = poolHandle.GetSystemClass<Dictionary<int, KeyValue>>();
        cachedDic.Clear();
        var kvs = poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        kvs.Clear();
        for (int i = 0; i < ids.Count; ++i)
        {
            var id = ids[i];
            TABLE.CLIENTATTRIBUTE clientAttribute = null;
            if (!ClientAttributeTableManager.Instance.TryGetValue(id, out clientAttribute))
            {
                continue;
            }

            TABLE.ATTRIBUTE linkedAttribute = null;
            if (!AttributeTableManager.Instance.TryGetValue(clientAttribute.id, out linkedAttribute))
            {
                continue;
            }

            if ((AttrType)clientAttribute.AttrType != AttrType.SkillDesc)
            {
                //属性走这里
                var v = values[i];
                int key = needMergeAttr ? clientAttribute.attached : id;
                if (!cachedDic.ContainsKey(key))
                {
                    var keyValue = poolHandle.GetSystemClass<KeyValue>();
                    cachedDic.Add(key, keyValue);
                    keyValue.clientAttribute = null;
                    ClientAttributeTableManager.Instance.TryGetValue(key, out keyValue.clientAttribute);
                }

                var handle = cachedDic[key];
                if (null == handle.keyValues)
                {
                    handle.keyValues = poolHandle.GetSystemClass<CSBetterLisHot<IdValue>>();
                }

                if (!handle.ContainsKey(linkedAttribute.id))
                {
                    var idValue = poolHandle.GetSystemClass<IdValue>();
                    idValue.attrItem = linkedAttribute;
                    idValue.clientAttrItem = clientAttribute;
                    idValue.attrValue = v;
                    idValue.percentValue = linkedAttribute.per;
                    handle.Add(idValue);
                }
                else
                {
                    var idValue = handle.GetValue(id);
                    if (null != idValue)
                    {
                        idValue.attrValue += v;
                    }
                }
            }
            else
            {
                if (!needSkillAttr)
                    continue;

                var skillId = values[i];
                TABLE.SKILL skillItem = null;
                if (!SkillTableManager.Instance.TryGetValue(skillId, out skillItem))
                    continue;
                //技能走这里
                if (!skillList.ContainsKey(skillId))
                {
                    var handle = poolHandle.GetSystemClass<KeyValue>();
                    handle.clientAttribute = null;
                    int key = needMergeAttr ? clientAttribute.attached : id;
                    ClientAttributeTableManager.Instance.TryGetValue(key, out handle.clientAttribute);
                    if (null == handle.keyValues)
                    {
                        handle.keyValues = poolHandle.GetSystemClass<CSBetterLisHot<IdValue>>();
                    }

                    handle.keyValues.Clear();

                    var idValue = poolHandle.GetSystemClass<IdValue>();
                    idValue.attrItem = linkedAttribute;
                    idValue.clientAttrItem = clientAttribute;
                    idValue.attrValue = skillId;
                    idValue.percentValue = linkedAttribute.per;
                    skillList.Add(skillId, handle);
                    handle.Add(idValue);
                }
            }
        }

        var it = cachedDic.GetEnumerator();
        while (it.MoveNext())
        {
            kvs.Add(it.Current.Value);
        }

        cachedDic.Clear();
        poolHandle.Recycle(cachedDic);
        var skillIt = skillList.GetEnumerator();
        while (skillIt.MoveNext())
        {
            kvs.Add(skillIt.Current.Value);
        }

        skillList.Clear();
        poolHandle.Recycle(skillList);
        return kvs;
    }

    public RepeatedField<KeyValue> GetAttributes(PoolHandleManager poolHandle, List<List<int>> info,
        bool needSkillAttr = true, bool needMergeAttr = true)
    {
        if (null == info || info.Count <= 0)
        {
            FNDebug.LogFormat("属性配空 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        }

        RepeatedField<int> ids = poolHandle.GetSystemClass<RepeatedField<int>>();
        RepeatedField<int> values = poolHandle.GetSystemClass<RepeatedField<int>>();
        ids.Clear();
        values.Clear();
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].Count != 2)
            {
                FNDebug.LogFormat("属性列表未成键值对，长度无效");
                continue;
            }

            ids.Add(info[i][0]);
            values.Add(info[i][1]);
        }

        var kvs = GetAttributes(poolHandle, ids, values);
        ids.Clear();
        values.Clear();
        poolHandle.Recycle(ids);
        poolHandle.Recycle(values);
        return kvs;
    }

    public RepeatedField<KeyValue> GetAttributes(PoolHandleManager poolHandle, LongArray info,
    bool needSkillAttr = true, bool needMergeAttr = true)
    {
        if (info.Count <= 0)
        {
            FNDebug.LogFormat("属性配空 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        }

        RepeatedField<int> ids = poolHandle.GetSystemClass<RepeatedField<int>>();
        RepeatedField<int> values = poolHandle.GetSystemClass<RepeatedField<int>>();
        ids.Clear();
        values.Clear();
        for (int i = 0; i < info.Count; i++)
        {
            ids.Add(info[i].key());
            values.Add((int)info[i].value());
        }

        var kvs = GetAttributes(poolHandle, ids, values, needSkillAttr, needMergeAttr);
        ids.Clear();
        values.Clear();
        poolHandle.Recycle(ids);
        poolHandle.Recycle(values);
        return kvs;
    }

    public RepeatedField<KeyValue> GetAttributes(PoolHandleManager poolHandle, RepeatedField<KEYVALUE> info,
        bool needSkillAttr = true, bool needMergeAttr = true)
    {
        if (null == info || info.Count <= 0)
        {
            FNDebug.LogFormat("属性配空 @高飞");
            return poolHandle.GetSystemClass<RepeatedField<KeyValue>>();
        }

        RepeatedField<int> ids = poolHandle.GetSystemClass<RepeatedField<int>>();
        RepeatedField<int> values = poolHandle.GetSystemClass<RepeatedField<int>>();
        ids.Clear();
        values.Clear();
        for (int i = 0; i < info.Count; i++)
        {
            ids.Add(info[i].key);
            values.Add((int)info[i].value);
        }

        var kvs = GetAttributes(poolHandle, ids, values, needSkillAttr, needMergeAttr);
        ids.Clear();
        values.Clear();
        poolHandle.Recycle(ids);
        poolHandle.Recycle(values);
        return kvs;
    }

    public string GetStageUpDesc(PoolHandleManager poolHandle, TABLE.PAODIANSHENFU current, int clientTipId)
    {
        var stageUpEffect = string.Empty;

        var attrIds = poolHandle.GetSystemClass<RepeatedField<int>>();
        var attrNums = poolHandle.GetSystemClass<RepeatedField<int>>();
        attrIds.Add(current.clientRankPara);
        attrNums.Add(current.rankaddNum);
        var kvs = CSAttributeInfo.Instance.GetAttributes(poolHandle, attrIds, attrNums);
        attrIds.Clear();
        attrNums.Clear();
        poolHandle.Recycle(attrIds);
        poolHandle.Recycle(attrNums);

        if (kvs.Count > 0)
        {
            var specialKeyValue = kvs[0];
            stageUpEffect = CSString.Format(clientTipId, specialKeyValue.Key, specialKeyValue.Value);

            for (int j = 0; j < kvs.Count; ++j)
            {
                kvs[j].OnRecycle(poolHandle);
                poolHandle.Recycle(kvs[j]);
            }

            kvs.Clear();
        }

        poolHandle.Recycle(kvs);

        return stageUpEffect;
    }

    public class IdValue
    {
        const string CONST_FMT_PERCENT_VALUE = @"{0:F2}%";
        public TABLE.ATTRIBUTE attrItem;
        public TABLE.CLIENTATTRIBUTE clientAttrItem;
        public int attrValue;
        public int percentValue;

        public AttrType AttrType
        {
            get
            {
                if (null != clientAttrItem)
                    return (AttrType)clientAttrItem.AttrType;
                return AttrType.Absolute;
            }
        }

        public bool HasDiff(IdValue other)
        {
            if (other.attrItem != attrItem)
                return true;
            if (other.clientAttrItem != clientAttrItem)
                return true;
            if (attrValue != other.attrValue)
                return true;
            return false;
        }

        public IdValue Clone(PoolHandleManager poolHandle, bool clearValue)
        {
            var clonedObject = poolHandle.GetSystemClass<IdValue>();
            clonedObject.attrItem = attrItem;
            clonedObject.clientAttrItem = clientAttrItem;
            clonedObject.attrValue = attrValue;
            clonedObject.percentValue = percentValue;
            if (clearValue)
            {
                clonedObject.attrValue = 0;
            }

            return clonedObject;
        }

        public string Key
        {
            get
            {
                if (null == clientAttrItem)
                    return string.Empty;

                TABLE.CLIENTTIPS clientTip = null;
                if (!ClientTipsTableManager.Instance.TryGetValue((int) clientAttrItem.tipID, out clientTip))
                {
                    return string.Empty;
                }

                return clientTip.context;
            }
        }

        public string Value
        {
            get
            {
                if ((AttrType)clientAttrItem.AttrType == AttrType.Percent)
                {
                    return string.Format(CONST_FMT_PERCENT_VALUE, attrValue * 100.0f / percentValue);
                }

                if ((AttrType)clientAttrItem.AttrType == AttrType.SkillLvAdd)
                {
                    return $"{attrValue}";
                }

                if ((AttrType)clientAttrItem.AttrType == AttrType.SkillDesc)
                {
                    TABLE.SKILL skill = null;
                    if (SkillTableManager.Instance.TryGetValue(attrValue, out skill))
                        return skill.description;
                    return string.Empty;
                }

                return $"{attrValue}";
            }
        }

        public void OnRecycle(PoolHandleManager pool)
        {
            attrItem = null;
            clientAttrItem = null;
            attrValue = 0;
            percentValue = 0;
        }
    }

    public class KeyValue
    {
        public AttrType AttrType
        {
            get
            {
                if (null != clientAttribute)
                    return (AttrType)clientAttribute.AttrType;
                return AttrType.Absolute;
            }
        }

        public TABLE.CLIENTATTRIBUTE clientAttribute;
        public CSBetterLisHot<IdValue> keyValues;

        public bool HasDiff(KeyValue keyValue)
        {
            if (keyValue.clientAttribute != this.clientAttribute)
                return true;
            if (this.keyValues.Count != keyValue.keyValues.Count)
                return true;
            var targets = keyValue.keyValues;
            for (int i = 0; i < keyValues.Count; ++i)
            {
                if (targets[i].HasDiff(keyValues[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public int sortId
        {
            get
            {
                if (null != clientAttribute)
                    return clientAttribute.order;
                return 0;
            }
        }

        public KeyValue Clone(PoolHandleManager poolHandle, bool clearValue)
        {
            var clonedObject = poolHandle.GetSystemClass<KeyValue>();
            clonedObject.clientAttribute = clientAttribute;
            clonedObject.keyValues = poolHandle.GetSystemClass<CSBetterLisHot<IdValue>>();
            clonedObject.keyValues.Clear();
            for (int i = 0; i < keyValues.Count; ++i)
            {
                clonedObject.keyValues.Add(keyValues[i].Clone(poolHandle, clearValue));
            }

            return clonedObject;
        }

        public string Key
        {
            get
            {
                if (null == clientAttribute)
                    return string.Empty;

                TABLE.CLIENTTIPS clientTip = null;
                if (!ClientTipsTableManager.Instance.TryGetValue((int) clientAttribute.tipID, out clientTip))
                {
                    return string.Empty;
                }

                return clientTip.context;
            }
        }

        public bool IsZeroValue
        {
            get
            {
                for (int i = 0; i < keyValues.Count; ++i)
                {
                    if (keyValues[i].attrValue != 0)
                        return false;
                }

                return true;
            }
        }

        //CBB694 | DCD5B8 |CBB694|00FF0C
        public string Value
        {
            get
            {
                if (null == clientAttribute)
                    return string.Empty;

                if ((AttrType)clientAttribute.AttrType == AttrType.SkillDesc)
                {
                    if (null == keyValues || keyValues.Count != 1)
                    {
                        return string.Empty;
                    }

                    if (keyValues.Count == 1)
                        return keyValues[0].Value;
                }

                if (null == keyValues || keyValues.Count <= 0)
                {
                    return string.Empty;
                }

                if (keyValues.Count == 1)
                {
                    return keyValues[0].Value;
                }

                var values = GetFmtValue();
                try
                {
                    return string.Format(clientAttribute.attributeFmt, values);
                }
                catch (Exception e)
                {
                    FNDebug.LogErrorFormat("fmt err {0} argvLength = ", clientAttribute.attributeFmt, values.Length);
                    FNDebug.LogError(e); 
                    return string.Empty;
                }
            } 
        }

        protected object[] GetFmtValue()
        {
            var objs = new object[keyValues.Count];
            for (int i = 0; i < objs.Length; ++i)
            {
                objs[i] = keyValues[i].Value;
            }

            return objs;
        }

        public void OnRecycle(PoolHandleManager pool)
        {
            if (null != keyValues)
            {
                for (int i = 0; i < keyValues.Count; ++i)
                {
                    keyValues[i].OnRecycle(pool);
                    pool.Recycle(keyValues[i]);
                }

                keyValues.Clear();
                pool.Recycle(keyValues);
                keyValues = null;
            }
        }

        public bool ContainsKey(int id)
        {
            for (int i = 0; i < keyValues.Count; ++i)
            {
                if (keyValues[i].attrItem.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public IdValue GetValue(int id)
        {
            for (int i = 0; i < keyValues.Count; ++i)
            {
                if (keyValues[i].attrItem.id == id)
                {
                    return keyValues[i];
                }
            }

            return null;
        }

        protected int compare(IdValue l, IdValue r)
        {
            if (l.attrItem == null || r.attrItem == null) return 0;
            return r.attrItem.id - l.attrItem.id;
        }

        public void Add(IdValue idValue)
        {
            keyValues.Add(idValue);

            if (keyValues.Count > 1)
                keyValues.Sort(compare);
        }

        public void Sub(KeyValue other)
        {
            if (null != other)
            {
                for (int i = 0; i < other.keyValues.Count; ++i)
                {
                    var keyValue = GetValue(other.keyValues[i].attrItem.id);
                    if (null != keyValue)
                    {
                        keyValue.attrValue -= other.keyValues[i].attrValue;
                    }
                }
            }
        }

        public void RemoveKeyValueIfLessThanOrEqualZero(PoolHandleManager pool)
        {
            for (int i = 0; i < keyValues.Count; ++i)
            {
                var keyValue = keyValues[i];
                if (keyValue.attrValue <= 0)
                {
                    keyValue.OnRecycle(pool);
                    pool.Recycle(keyValue);
                    keyValues.RemoveAt(i--);
                }
            }
        }
    }

    public override void Dispose()
    {
    }
}

public static class CSAttributeInfoExtend
{
    public static void RecycleAttributes(this PoolHandleManager poolHandleManager,
        RepeatedField<CSAttributeInfo.KeyValue> kvs)
    {
        if (null != poolHandleManager && null != kvs)
        {
            for (int i = 0; i < kvs.Count; ++i)
                kvs[i].OnRecycle(poolHandleManager);
            kvs.Clear();
            poolHandleManager.Recycle(kvs);
        }
    }
}