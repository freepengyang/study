using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

/// <summary>
/// 所有的UI数据类 都必须继承这个类，在返回登录时，会统一调用DisPose
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CSInfo<T> : CSBase where T :CSInfo<T>,new()
{
	private EventHanlderManager _mClientEvent;

	protected EventHanlderManager mClientEvent
	{
		get
		{
			if (_mClientEvent == null)
				_mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
			return _mClientEvent;
		}
	}


	private static T m_instance;

	public static T Instance
	{
		get
		{
			if (m_instance == null) m_instance = new T();

            return m_instance;
		}
    }

	/// <summary>
	/// 继承的类不要重写此方法，此方法用于CSInfo<T>中销毁事件
	/// </summary>
	public override void OnDispose()
	{
		Dispose();
		if (_mClientEvent != null) _mClientEvent.UnRegAll();
		_mClientEvent = null;
		m_instance = null;
	}
	
	public abstract void Dispose();
}

public abstract class CSBase
{
	protected CSBase()
	{
		CSBaseManager.Instance.AddBase(this.GetType().Name, this);
	}

	/// <summary>
	/// 继承的类不要重写此方法，此方法用于CSInfo<T>中销毁事件
	/// </summary>
	public abstract void OnDispose();
}