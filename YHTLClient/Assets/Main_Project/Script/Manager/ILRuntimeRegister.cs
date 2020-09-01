//-------------------------------------------------------------------------
//Game Sate
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Generated;

public class ILRuntimeRegister : Singleton2<ILRuntimeRegister>
{
#if ILRuntime

	public ILRuntime.Runtime.Enviorment.AppDomain _appdomain;

	public ILRuntime.Runtime.Enviorment.AppDomain appdomain
	{
		get
		{
			if (_appdomain == null)
				_appdomain = CSGame.appdomain;
			return _appdomain;
		}
		set { _appdomain = value; }
	}


	public unsafe void SetupCLRRedirection()
	{
		var arr = typeof(GameObject).GetMethods();
		foreach (var item in arr)
		{
			if (item.Name == "AddComponent" && item.GetGenericArguments().Length == 1)
			{
				appdomain.RegisterCLRMethodRedirection(item, AddComponent);
			}
			else if (item.Name == "GetComponent" && item.GetGenericArguments().Length == 1)
			{
				appdomain.RegisterCLRMethodRedirection(item, GetComponent);
			}
			else if (item.Name == "GetComponentInChildren" && item.GetGenericArguments().Length == 1)
			{
				appdomain.RegisterCLRMethodRedirection(item, GetComponentInChildren);
			}
			else if (item.Name == "GetComponentsInChildren" && item.GetGenericArguments().Length == 0 &&
				item.ReturnType.Name == "Component[]")
			{
				appdomain.RegisterCLRMethodRedirection(item, GetComponentsInChildren);
			}
			else if (item.Name == "GetComponentsInChildren" && item.GetGenericArguments().Length == 1 &&
				item.GetParameters().Length == 0 && item.ReturnType.Name == "T[]")
			{
				appdomain.RegisterCLRMethodRedirection(item, GetComponentsInChildrenType);
			}
		}
	}

	unsafe StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
		CLRMethod __method, bool isNewObj)
	{
		//CLR重定向的说明请看相关文档和教程，这里不多做解释
		ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

		var ptr = __esp - 1;
		//成员方法的第一个参数为this
		GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
		if (instance == null)
			throw new System.NullReferenceException();
		__intp.Free(ptr);

		var genericArgument = __method.GenericArguments;
		//AddComponent应该有且只有1个泛型参数
		if (genericArgument != null && genericArgument.Length == 1)
		{
			var type = genericArgument[0];
			object res;
			if (type is CLRType)
			{
				//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
				res = instance.AddComponent(type.TypeForCLR);
			}
			else
			{
				//热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
				//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
				var ilInstance = new ILTypeInstance(type as ILType, false);
				//接下来创建Adapter实例
				var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
				//unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
				clrInstance.ILInstance = ilInstance;
				clrInstance.AppDomain = __domain;
				//这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
				ilInstance.CLRInstance = clrInstance;

				res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
				//只有当前激活的才会立马调用一次Awake
				if (instance.activeInHierarchy)
				{
					clrInstance.Awake(); //因为Unity调用这个方法时还没准备好所以这里补调一次
				}
			}

			return ILIntepreter.PushObject(ptr, __mStack, res);
		}

		return __esp;
	}

	unsafe StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
		CLRMethod __method, bool isNewObj)
	{
		//CLR重定向的说明请看相关文档和教程，这里不多做解释
		ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

		var ptr = __esp - 1;
		//成员方法的第一个参数为this
		GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
		if (instance == null)
			throw new System.NullReferenceException();
		__intp.Free(ptr);

		var genericArgument = __method.GenericArguments;
		//GetComponent应该有且只有1个泛型参数
		if (genericArgument != null && genericArgument.Length == 1)
		{
			var type = genericArgument[0];
			object res = null;
			if (type is CLRType)
			{
				//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
				res = instance.GetComponent(type.TypeForCLR);
			}
			else
			{
				//因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
				var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
				for (int i = 0; i < clrInstances.Length; i++)
				{
					var clrInstance = clrInstances[i];
					if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
					{
						if (clrInstance.ILInstance.Type == type)
						{
							res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
							break;
						}
					}
				}
			}

			return ILIntepreter.PushObject(ptr, __mStack, res);
		}

		return __esp;
	}

	unsafe StackObject* GetComponentInChildren(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
		CLRMethod __method, bool isNewObj)
	{
		//CLR重定向的说明请看相关文档和教程，这里不多做解释
		ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

		var ptr = __esp - 1;
		//成员方法的第一个参数为this
		GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
		if (instance == null)
			throw new System.NullReferenceException();
		__intp.Free(ptr);

		var genericArgument = __method.GenericArguments;
		//GetComponentInChildren应该有且只有1个泛型参数
		if (genericArgument != null && genericArgument.Length == 1)
		{
			var type = genericArgument[0];
			object res = null;
			if (type is CLRType)
			{
				//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
				res = instance.GetComponentInChildren(type.TypeForCLR);
			}
			else
			{
				//因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
				var clrInstances = instance.GetComponentsInChildren<MonoBehaviourAdapter.Adaptor>();
				for (int i = 0; i < clrInstances.Length; i++)
				{
					var clrInstance = clrInstances[i];
					if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
					{
						if (clrInstance.ILInstance.Type == type)
						{
							res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
							break;
						}
					}
				}
			}

			return ILIntepreter.PushObject(ptr, __mStack, res);
		}

		return __esp;
	}

	unsafe StackObject* GetComponentsInChildren(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
		CLRMethod __method, bool isNewObj)
	{
		//CLR重定向的说明请看相关文档和教程，这里不多做解释
		ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

		var ptr = __esp - 1;
		//成员方法的第一个参数为this
		GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
		if (instance == null)
		{
			throw new System.NullReferenceException();
		}

		__intp.Free(ptr);

		var genericArgument = __method.GenericArguments;
		//GetComponentsInChildren应该有且只有1个泛型参数
		if (genericArgument != null && genericArgument.Length == 1)
		{
			var type = genericArgument[0];
			object res = null;
			if (type is CLRType)
			{
				//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
				res = instance.GetComponentsInChildren(type.TypeForCLR);
			}
			else
			{
				//因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
				var clrInstances = instance.GetComponentsInChildren<MonoBehaviourAdapter.Adaptor>();
				for (int i = 0; i < clrInstances.Length; i++)
				{
					var clrInstance = clrInstances[i];
					if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
					{
						if (clrInstance.ILInstance.Type == type)
						{
							res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
							break;
						}
					}
				}
			}

			return ILIntepreter.PushObject(ptr, __mStack, res);
		}

		return __esp;
	}

	unsafe StackObject* GetComponentsInChildrenType(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
		CLRMethod __method, bool isNewObj)
	{
		//CLR重定向的说明请看相关文档和教程，这里不多做解释
		ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

		var ptr = __esp - 1;
		//成员方法的第一个参数为this
		GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
		if (instance == null)
		{
			throw new System.NullReferenceException();
		}

		__intp.Free(ptr);

		var genericArgument = __method.GenericArguments;
		//GetComponentsInChildren应该有且只有1个泛型参数
		if (genericArgument != null && genericArgument.Length == 1)
		{
			var type = genericArgument[0];
			object res = null;
			List<ILTypeInstance> compents = new List<ILTypeInstance>();

			if (type is CLRType)
			{
				//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
				res = instance.GetComponentsInChildren(type.TypeForCLR);
			}
			else
			{
				//因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
				var clrInstances = instance.GetComponentsInChildren<MonoBehaviourAdapter.Adaptor>();
				for (int i = 0; i < clrInstances.Length; i++)
				{
					var clrInstance = clrInstances[i];
					if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
					{
						if (clrInstance.ILInstance.Type == type)
						{
							//res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
							compents.Add(clrInstance.ILInstance);
							//break;
						}
					}
				}

				res = compents.ToArray();
			}

			return ILIntepreter.PushObject(ptr, __mStack, res);
		}

		return __esp;
	}
#endif

#if ILRuntime
	public void InitializeILRuntime()
	{
#if UNITY_EDITOR
		appdomain.DebugService.StartDebugService(56000);
#endif
		//这里做一些ILRuntime的注册
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
		appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
		appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
		appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
		appdomain.RegisterCrossBindingAdaptor(new IDisposableAdapter());
		appdomain.RegisterCrossBindingAdaptor(new IEnumerableAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UISliderAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UICenterOnChildAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UIScrollViewAdapter());
		appdomain.RegisterCrossBindingAdaptor(new SpringPanelAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UIDragDropItemAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UITweenerAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UITableAdapter());
		appdomain.RegisterCrossBindingAdaptor(new UIWrapContentAdapter());
		appdomain.RegisterCrossBindingAdaptor(new Adapt_IMessage());
        appdomain.RegisterCrossBindingAdaptor(new IComparableAdapter());
        appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
		appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
		appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
		appdomain.RegisterValueTypeBinder(typeof(CSMisc.Dot2), new CSMiscDot2Binding());
		//other
		appdomain.RegisterCrossBindingAdaptor(new SystemExceptionAdapter());
		appdomain.RegisterCrossBindingAdaptor(new BehaviourProviderAdapter());
		appdomain.RegisterCrossBindingAdaptor(new AvatarUnitAdapter());
        RegisterMethod();
		RegisterDelegate();
		//CLR绑定放上面注册之后，，否则重复情况下会导致上面不能覆盖
		CLRBindings.Initialize(appdomain);
	}


	void RegisterMethod()
    {
        appdomain.DelegateManager.RegisterMethodDelegate<global::CSMisc.Dot2>();        appdomain.DelegateManager.RegisterMethodDelegate<global::ModelLoadBase>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Action, System.Boolean>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
		appdomain.DelegateManager.RegisterMethodDelegate<global::CSResourceWWW>();
		appdomain.DelegateManager.RegisterMethodDelegate<global::CSResource>();
		appdomain.DelegateManager
			.RegisterMethodDelegate<global::ELogToggleType, System.String, global::ELogColorType>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.String>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.String[]>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.String>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
		appdomain.DelegateManager.RegisterMethodDelegate<global::BehaviorState>();
		appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32>();
		appdomain.DelegateManager
			.RegisterFunctionDelegate<UnityEngine.Object, global::EShareMatType, System.String, System.Boolean,
				UnityEngine.Material>();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform>();
		appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, System.Object, System.Boolean>();
		appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, System.Object, System.Int32>();
		appdomain.DelegateManager
			.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
		appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Boolean>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Vector2>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();
		appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, System.Object, System.Boolean>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Int32>();
		appdomain.DelegateManager.RegisterMethodDelegate<heart.Heartbeat>();
		appdomain.DelegateManager.RegisterMethodDelegate<global::NetInfo>();
		appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Boolean>();
		appdomain.DelegateManager
			.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance,
				ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector3>();
		appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Int32, System.Char, System.Char>();
		appdomain.DelegateManager.RegisterFunctionDelegate<global::Adapt_IMessage.Adaptor>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.GameObject>();
        appdomain.DelegateManager.RegisterFunctionDelegate<global::Adapt_IMessage.Adaptor, global::Adapt_IMessage.Adaptor, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::Adapt_IMessage.Adaptor>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Int32, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<Main_Project.SDKScript.SDK.LoginInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<Main_Project.Script.Update.UpdateCheckCode>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::TableHandle>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.UInt32, System.Object>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::Adapt_IMessage.Adaptor, System.Int32>();

    }

    void RegisterDelegate()
	{
		appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ThreadStart>((act) =>
		{
			return new System.Threading.ThreadStart(() => { ((Action) act)(); });
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
		{
			return new System.AsyncCallback((ar) => { ((Action<System.IAsyncResult>) act)(ar); });
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<TencentMobileGaming.QAVOnEventCallBack>((act) =>
		{
			return new TencentMobileGaming.QAVOnEventCallBack((type, subType, data) =>
			{
				((Action<System.Int32, System.Int32, System.String>) act)(type, subType, data);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<TencentMobileGaming.QAVEnterRoomComplete>((act) =>
		{
			return new TencentMobileGaming.QAVEnterRoomComplete((result, error_info) =>
			{
				((Action<System.Int32, System.String>) act)(result, error_info);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<TencentMobileGaming.QAVExitRoomComplete>((act) =>
		{
			return new TencentMobileGaming.QAVExitRoomComplete(() => { ((Action) act)(); });
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<TencentMobileGaming.QAVRoomDisconnect>((act) =>
		{
			return new TencentMobileGaming.QAVRoomDisconnect((result, error_info) =>
			{
				((Action<System.Int32, System.String>) act)(result, error_info);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<TencentMobileGaming.QAVEndpointsUpdateInfo>((act) =>
		{
			return new TencentMobileGaming.QAVEndpointsUpdateInfo((eventID, count, openIdList) =>
			{
				((Action<System.Int32, System.Int32, System.String[]>) act)(eventID, count, openIdList);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<global::BagWrapContent.OnInitializeItem>((act) =>
		{
			return new global::BagWrapContent.OnInitializeItem((go, realIndex) =>
			{
				((Action<UnityEngine.GameObject, System.Int32>) act)(go, realIndex);
			});
		});

		appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Object>>((act) =>
		{
			return new System.Comparison<System.Object>((x, y) =>
			{
				return ((Func<System.Object, System.Object, System.Int32>) act)(x, y);
			});
		});
		appdomain.DelegateManager
			.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
			{
				return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
				{
					return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>) act)(obj);
				});
			});
		appdomain.DelegateManager
			.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
			{
				return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
				{
					return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance,
						ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>) act)(x, y);
				});
			});
		appdomain.DelegateManager.RegisterDelegateConvertor<global::UIInput.OnValidate>((act) =>
		{
			return new global::UIInput.OnValidate((text, charIndex, addedChar) =>
			{
				return ((Func<System.String, System.Int32, System.Char, System.Char>)act)(text, charIndex, addedChar);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<global::CSBetterList<global::Adapt_IMessage.Adaptor>.CompareFunc>((act) =>
		{
			return new global::CSBetterList<global::Adapt_IMessage.Adaptor>.CompareFunc((left, right) =>
			{
				return ((Func<global::Adapt_IMessage.Adaptor, global::Adapt_IMessage.Adaptor, System.Int32>)act)(left, right);
			});
		});
        appdomain.DelegateManager.RegisterDelegateConvertor<global::UIWrapContent.OnInitializeItem>((act) =>

        {

            return new global::UIWrapContent.OnInitializeItem((go, wrapIndex, realIndex) =>

            {

                ((Action<UnityEngine.GameObject, System.Int32, System.Int32>)act)(go, wrapIndex, realIndex);

            });

        });
        appdomain.DelegateManager.RegisterDelegateConvertor<global::UICenterOnChild.OnCenterCallback>((act) =>

        {

            return new global::UICenterOnChild.OnCenterCallback((centeredObject) =>

            {

                ((Action<UnityEngine.GameObject>)act)(centeredObject);

            });

        });

        appdomain.DelegateManager.RegisterDelegateConvertor<global::CSBetterList<ILRuntime.Runtime.Intepreter.ILTypeInstance>.CompareFunc>((act) =>
        {
            return new global::CSBetterList<ILRuntime.Runtime.Intepreter.ILTypeInstance>.CompareFunc((left, right) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(left, right);

            });

        });

        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<global::Adapt_IMessage.Adaptor>>((act) =>
        {
            return new System.Comparison<global::Adapt_IMessage.Adaptor>((x, y) =>
            {
                return ((Func<global::Adapt_IMessage.Adaptor, global::Adapt_IMessage.Adaptor, System.Int32>)act)(x, y);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<global::QuDaoInterface.LoginMultiSucHandler>((act) =>
            {
                return new global::QuDaoInterface.LoginMultiSucHandler((data) =>
                {
                    ((Action<Main_Project.SDKScript.SDK.LoginInfo>)act)(data);
                });
            });
        appdomain.DelegateManager.RegisterDelegateConvertor<global::MainBaseEvent.Callback>((act) =>
        {
	        return new global::MainBaseEvent.Callback((uiEvtID, data) =>
	        {
		        ((Action<System.UInt32, System.Object>)act)(uiEvtID, data);
	        });
        });

    }
#endif
}