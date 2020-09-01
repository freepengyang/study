using System;
using Google.Protobuf.Collections;
public partial class Net
{
	public static void CSWoLongPetChangeStateMessage(Int32 id,Int32 state)
	{
		pet.WoLongPetState req = CSProtoManager.Get<pet.WoLongPetState>();
		req.id = id;
		req.state = state;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWoLongPetChangeStateMessage,req);
	}
	public static void CSWoLongPetInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSWoLongPetInfoMessage,null);
	}
	public static void CSPetInfoMessage()
	{
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPetInfoMessage,null);
	}
	public static void CSPetUpgradeMessage(Int32 type,RepeatedField<Int32> bagIndices)
	{
		pet.ReqPetUpgrade req = CSProtoManager.Get<pet.ReqPetUpgrade>();
		req.type = type;
		req.bagIndices.Clear();
		req.bagIndices.AddRange(bagIndices);
		bagIndices.Clear();
		CSNetRepeatedFieldPool.Put(bagIndices);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPetUpgradeMessage,req);
	}
	public static void CSPetSkillUpgradeMessage(Int32 skillGroup)
	{
		pet.ReqPetSkillUpgrade req = CSProtoManager.Get<pet.ReqPetSkillUpgrade>();
		req.skillGroup = skillGroup;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPetSkillUpgradeMessage,req);
	}
	public static void CSUnlockPetTianFuMessage(Int32 paging,Int32 starrating)
	{
		pet.ReqUnlockPetTianFu req = CSProtoManager.Get<pet.ReqUnlockPetTianFu>();
		req.paging = paging;
		req.starrating = starrating;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSUnlockPetTianFuMessage,req);
	}
	public static void CSPetTianFuRandomPassiveSkillMessage(Int32 pos,Int32 special)
	{
		pet.PetTianFuRandomPassiveSkill req = CSProtoManager.Get<pet.PetTianFuRandomPassiveSkill>();
		req.pos = pos;
		req.special = special;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPetTianFuRandomPassiveSkillMessage,req);
	}
	public static void CSPetTianFuChosePassiveSkillMessage(Int32 pos,Int32 special)
	{
		pet.PetTianFuRandomPassiveSkill req = CSProtoManager.Get<pet.PetTianFuRandomPassiveSkill>();
		req.pos = pos;
		req.special = special;
		CSHotNetWork.Instance.SendMsg((int)ECM.CSPetTianFuChosePassiveSkillMessage,req);
	}
	public static void CSSetCallBackSettingMessage(RepeatedField<Int32> callBackSettingList)
	{
		pet.CallBackSetting req = CSProtoManager.Get<pet.CallBackSetting>();
		req.callBackSettingList.Clear();
		req.callBackSettingList.AddRange(callBackSettingList);
		callBackSettingList.Clear();
		CSNetRepeatedFieldPool.Put(callBackSettingList);
		CSHotNetWork.Instance.SendMsg((int)ECM.CSSetCallBackSettingMessage,req);
	}
}
