using System.Collections.Generic;
using UnityEngine;

public class CSDamageInfo : CSInfo<CSDamageInfo>
{
	Dictionary<long, long> mPlayer2MeDamage = new Dictionary<long, long>(32);

	public void AddDamage(long guid,long value)
	{
		if (mPlayer2MeDamage.ContainsKey(guid))
			mPlayer2MeDamage[guid] += value;
		else
			mPlayer2MeDamage.Add(guid, value);
	}

	public long GetDamage(long guid)
    {
		long v = 0;
		mPlayer2MeDamage.TryGetValue(guid, out v);
		return v;
	}

	public void ClearAllDamages()
	{
		mPlayer2MeDamage.Clear();
	}

	public override void Dispose()
	{
		mPlayer2MeDamage?.Clear();
		mPlayer2MeDamage = null;
	}
}