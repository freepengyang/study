using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 基础事件
/// </summary>
public class MainBaseEvent
{
	public delegate void Callback(uint uiEvtID, object data);

	/// <summary>
	/// 事件的集合
	/// </summary>
	public Dictionary<uint, MainEventDelegate> mDicEvtDelegate = new Dictionary<uint, MainEventDelegate>();

	/// <summary>
	/// 事件委托类
	/// </summary>
	public class MainEventDelegate
	{
		/// <summary>
		/// 事件ID
		/// </summary>
		private uint uiEvtID;

		private List<Callback> arrCallBack = new List<Callback>();
		private List<Callback> arr2Process = new List<Callback>();

		public MainEventDelegate(uint _uiEvtId)
		{
			uiEvtID = _uiEvtId;
		}

		/// <summary>
		/// 添加事件
		/// </summary>
		/// <param name="cb">回调函数</param>
		public void AddCallBack(Callback cb)
		{
			if (!arrCallBack.Contains(cb))
			{
				arrCallBack.Add(cb);
			}
		}


		/// <summary>
		/// 移除事件
		/// </summary>
		/// <param name="cb">回调函数</param>
		public void RemoveCallBack(Callback cb)
		{
			arrCallBack.Remove(cb);
		}

		/// <summary>
		/// 是否存在事件
		/// </summary>
		/// <param name="cb"></param>
		/// <returns></returns>
		public bool ContainsCallBack(MainBaseEvent.Callback cb)
		{
			return arrCallBack.Contains(cb);
		}

		/// <summary>
		/// 发送事件
		/// </summary>
		/// <param name="objData">参数</param>
		public void ProcEvent(object objData)
		{
			arr2Process.AddRange(arrCallBack);

			for (int i = 0; i < arr2Process.Count; i++)
			{
				Callback cb = arr2Process[i] as Callback;
				cb(uiEvtID, objData);
			}

			arr2Process.Clear();
		}
	}

	/// <summary>
	/// 添加事件
	/// </summary>
	/// <param name="uiEvtID"></param>
	/// <param name="callBack"></param>
	public void Reg(uint uiEvtID, Callback callBack)
	{
		MainEventDelegate evtDelegate = null;

		if (!mDicEvtDelegate.ContainsKey(uiEvtID))
		{
			evtDelegate = new MainEventDelegate(uiEvtID);
			mDicEvtDelegate.Add(uiEvtID, evtDelegate);
		}
		else
		{
			evtDelegate = mDicEvtDelegate[uiEvtID];
		}

		if (null != evtDelegate)
		{
			evtDelegate.AddCallBack(callBack);
		}
	}

	/// <summary>
	/// 删除事件
	/// </summary>
	/// <param name="uiEvtID"></param>
	/// <param name="callBack"></param>
	public void UnReg(uint uiEvtID, Callback callBack)
	{
		if (mDicEvtDelegate.ContainsKey(uiEvtID))
		{
			MainEventDelegate evtDelegate = mDicEvtDelegate[uiEvtID];
			evtDelegate.RemoveCallBack(callBack);
		}
	}

	/// <summary>
	/// 删除事件
	/// </summary>
	/// <param name="uiEvtId"></param>
	public void UnReg(uint uiEvtId)
	{
		if (mDicEvtDelegate.ContainsKey(uiEvtId))
		{
			mDicEvtDelegate.Remove(uiEvtId);
		}
	}

	/// <summary>
	/// 分发事件
	/// </summary>
	/// <param name="uiEvtID"></param>
	/// <param name="objData"></param>
	/// <returns></returns>
	public void ProcEvent(uint uiEvtID, object objData)
	{
		if (mDicEvtDelegate.ContainsKey(uiEvtID))
		{
			MainEventDelegate evtDelegate = mDicEvtDelegate[uiEvtID];
			evtDelegate.ProcEvent(objData);
		}
	}

	/// <summary>
	/// 是否存在相同的key和Callback
	/// </summary>
	/// <param name="uiEvtID"></param>
	/// <param name="callBack"></param>
	/// <returns></returns>
	public bool IsExistKeyAndCallBack(uint uiEvtID, Callback callBack)
	{
		return mDicEvtDelegate.ContainsKey(uiEvtID) && mDicEvtDelegate[uiEvtID].ContainsCallBack(callBack);
	}
}

// 用于保证成对注册和注销
public class MainEventHanlderManager
{
	private static MainEventHanlderManager mInstance;

	public static MainEventHanlderManager Instance
	{
		get
		{
			if (mInstance == null)
				mInstance = new MainEventHanlderManager();

			return mInstance;
		}
	}

	public enum MainDispatchType
	{
		Socket = 0,
		Event,
		RedPoint,
		CombineRedPoint,
	}

	struct MsgCBPair
	{
		public uint ID;
		public MainBaseEvent.Callback CB;
	}

	private List<MsgCBPair> mCBPairs = new List<MsgCBPair>();


	static MainBaseEvent msSocketDispatcher = new MainBaseEvent();
	static MainBaseEvent msEventDispatcher = new MainBaseEvent();
	static MainBaseEvent msRedPointDispatcher = new MainBaseEvent();
	static MainBaseEvent msCombineRedPointDispatcher = new MainBaseEvent();

	public MainBaseEvent mDispatcher;

	public MainEventHanlderManager()
	{
		mDispatcher = msEventDispatcher;
	}

	public MainEventHanlderManager(MainDispatchType dt)
	{
		mDispatcher = msEventDispatcher;
		SetEventType(dt);
	}

	public void SetEventType(MainDispatchType dt)
	{
		switch (dt)
		{
			case MainDispatchType.Event:
				mDispatcher = msEventDispatcher;
				break;

			case MainDispatchType.Socket:
				mDispatcher = msSocketDispatcher;
				break;
		}
	}


	public void Reg(uint msgId, MainBaseEvent.Callback cb)
	{
		for (int i = 0; i < mCBPairs.Count; ++i)
		{
			if (mCBPairs[i].ID == msgId && mCBPairs[i].CB == cb)
			{
				if (mDispatcher.IsExistKeyAndCallBack(msgId, cb))
				{
					return;
				}
			}
		}

		MsgCBPair pair;
		pair.ID = msgId;
		pair.CB = cb;
		mCBPairs.Add(pair);
		mDispatcher.Reg(msgId, cb);
	}

	public void UnReg(uint msgId, MainBaseEvent.Callback cb)
	{
		for (int i = 0; i < mCBPairs.Count; ++i)
		{
			if (mCBPairs[i].ID == msgId && mCBPairs[i].CB == cb)
			{
				mCBPairs.RemoveAt(i);
				break;
			}
		}

		mDispatcher.UnReg(msgId, cb);
	}

	//针对UnReg会造成删去全局所有msgId事件回调的问题，优化成只删去和mCBPairs相关的回调
	public void UnRegMsg(uint msgId)
	{
		for (int i = 0; i < mCBPairs.Count; i++)
		{
			if (mCBPairs[i].ID == msgId)
			{
				mDispatcher.UnReg(msgId, mCBPairs[i].CB);
				mCBPairs.RemoveAt(i);
			}
		}
	}

	public void UnReg(uint msgId)
	{
		for (int i = 0; i < mCBPairs.Count; i++)
		{
			if (mCBPairs[i].ID == msgId)
			{
				mCBPairs.RemoveAt(i);
			}
		}

		mDispatcher.UnReg(msgId);
	}

	public void UnRegAll()
	{
		for (int i = 0; i < mCBPairs.Count; ++i)
		{
			mDispatcher.UnReg(mCBPairs[i].ID, mCBPairs[i].CB);
		}

		mCBPairs.Clear();
	}

	public void ProcEvent(uint uiEvtID, object objData)
	{
		mDispatcher.ProcEvent(uiEvtID, objData);
	}

	#region 针对客户端事件

	public void AddEvent(MainEvent uiEvtID, MainBaseEvent.Callback callback)
	{
		Reg((uint) uiEvtID, callback);
	}

	public void AddEvent(uint uiEvtID, MainBaseEvent.Callback callback)
	{
		Reg(uiEvtID, callback);
	}

	public void RemoveEvent(MainEvent uiEvitID)
	{
		UnReg((uint) uiEvitID);
	}

	public void RemoveEvent(MainEvent uiEvitID, MainBaseEvent.Callback cb)
	{
		UnReg((uint) uiEvitID, cb);
	}

	public void SendEvent(MainEvent uiEvtID, object objData = null)
	{
		ProcEvent((uint) uiEvtID, objData);
	}

	#endregion
}

