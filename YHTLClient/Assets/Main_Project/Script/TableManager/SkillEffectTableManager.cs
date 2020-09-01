//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// <auto-generated>
//------------------------------------------------------------------------------

// Generated from: protos/c_table_skilleffect.proto
using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class SkillEffectTableManager : TableManager_Main<TABLE.SKILLEFFECTARRAY, TABLE.SKILLEFFECT,int,SkillEffectTableManager>
{
	protected override void OnResourceLoaded(CSResourceWWW res)
	{
		base.OnResourceLoaded(res);
		Dic = new Dictionary<int, TABLE.SKILLEFFECT>(array.rows.Count);
		if (array != null)
		{
			for (int i = 0; i < array.rows.Count; i++)
			{
				int id = array.rows[i].id;
				AddTables(id, array.rows[i]);
			}
		}
		base.OnDealOver();
	}
	public int GetSkillEffectId(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.id;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectName(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.name;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModel(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectDelaytime(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.delaytime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectScale(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.scale;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectLayer(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectZhenlv(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zhenlv;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectCengji(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cengji;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectXuanzhuan(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.xuanzhuan;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModelframelist(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.modelframelist;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectFrame(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.frame;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModel2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectDelaytime2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.delaytime2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectScale2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.scale2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectLayer2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectZhenlv2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zhenlv2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectCengji2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cengji2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectXuanzhuan2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.xuanzhuan2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModelframelist2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.modelframelist2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectFrame2(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.frame2;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModel3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectDelaytime3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.delaytime3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectScale3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.scale3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectLayer3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectZhenlv3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zhenlv3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectCengji3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cengji3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectXuanzhuan3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.xuanzhuan3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModelframelist3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.modelframelist3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectFrame3(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.frame3;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModel4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.model4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectDelaytime4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.delaytime4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectScale4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.scale4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectLayer4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.layer4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectZhenlv4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.zhenlv4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectCengji4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.cengji4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectXuanzhuan4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.xuanzhuan4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectModelframelist4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.modelframelist4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectFrame4(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.frame4;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectBeginDelay(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.beginDelay;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectBeAttackDelayTime(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.beAttackDelayTime;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectRemovedelay(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.removedelay;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectStartPlayHurt(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.startPlayHurt;
		}
		else
		{
			return defaultValue;
		}
	}
	public int GetSkillEffectNum(int id,int defaultValue = default(int))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.num;
		}
		else
		{
			return defaultValue;
		}
	}
	public string GetSkillEffectPoint(int id,string defaultValue = default(string))
	{
		TABLE.SKILLEFFECT cfg = null;
		if(TryGetValue(id,out cfg))
		{
			return cfg.point;
		}
		else
		{
			return defaultValue;
		}
	}
}