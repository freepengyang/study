using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using System;
using System.Reflection;

public class HotfixManager
{
    private IType IT_hotfix = null;

    private static HotfixManager mInstance;
#if ILRuntime
    private ILRuntime.Runtime.Enviorment.AppDomain appdomain = null;
#endif
    public static HotfixManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new HotfixManager();
            }
            return mInstance;
        }
        set { mInstance = value; }
    }

    public Type TYPE_TIMESPAN
    {
        get;
        private set;
    }

    public Type TYPE_GUID
    {
        get;
        private set;
    }

    public Type TYPE_URI
    {
        get;
        private set;
    }

    public Type TYPE_BYTEARRAY
    {
        get;
        private set;
    }

    public Type TYPE_SYSTYPE
    {
        get;
        private set;
    }

    public IType GetItypeByName(string str)
    {
#if ILRuntime
        return appdomain.GetType(str);
#else
        return null;
#endif
    }


    public void InitInfo()
    {
        #if ILRuntime
        appdomain = CSGame.appdomain;
        #endif
        //IT_hotfix= appdomain.LoadedTypes ["HotFix_Project.HotFix_UIlogin"];
        //Type t = IT_hotfix.ReflectionType;
        TYPE_TIMESPAN = GetCLRType(typeof(System.TimeSpan));
        TYPE_GUID = GetCLRType(typeof(System.Guid));
        TYPE_URI = typeof(Uri);
        TYPE_BYTEARRAY = GetCLRType(typeof(System.Byte[]));
        TYPE_SYSTYPE = GetCLRType(typeof(System.Type));
    }

    public Type GetCLRType(Type type)
    {
        return type is ILRuntime.Reflection.ILRuntimeWrapperType ? ((ILRuntime.Reflection.ILRuntimeWrapperType)type).CLRType.TypeForCLR : type;
    }

    public Type GetListItemType(Type type)
    {
        var wt = type as ILRuntime.Reflection.ILRuntimeWrapperType;
        if (wt != null)
        {
            var clrType = wt.CLRType;
            if (clrType != null && clrType.GenericArguments != null && clrType.GenericArguments.Length > 0)
            {
                type = clrType.GenericArguments[0].Value.ReflectionType;
                return type;
            }
        }
        else
        {
            
            for (int i = 0; i < type.GetMethods().Length; i++)
            {
                MethodInfo method = type.GetMethods()[i];

                if (method.IsStatic || method.Name != "Add") continue;
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1)
                {
                    return parameters[0].ParameterType;
                }
            }
        }

        return null;
    }

    public object ConvertCLRInstance(object value)
    {
        ILRuntime.Runtime.Intepreter.ILTypeInstance instance = value as ILRuntime.Runtime.Intepreter.ILTypeInstance;
        if (instance != null)
        {
            return instance.CLRInstance;
        }
        return value;
    }

    public object CreateInstance(Type type)
    {
#if ILRuntime
        var _type = appdomain.GetType(type);
        return appdomain.Instantiate(_type.FullName);
#else
        var _type = CSGame.Sington.assembly.GetType(type.Name);
        if (_type != null)
            return CSGame.Sington.assembly.CreateInstance(_type.FullName);
        return null;
#endif
    }
    public HotfixManager()
    {

    }

    public object Check_Hotfix(String FunctionName, int ParamNum, bool IsNeedReturn, params object[] date)
    {
        object obj = null;
#if ILRuntime
        if (IT_hotfix == null || appdomain == null) return null;

        IMethod method = IT_hotfix.GetMethod(FunctionName, ParamNum);
        if (method == null) return null;
        obj = appdomain.Invoke(method, null, date);
        if (IsNeedReturn)
            return obj;
        else
            return method;
#else
        return obj;
#endif
    }


    public object Checkhotfix(String ClassName, String FunctionName, int ParamNum, bool IsNeedReturn, params object[] date)
    {
        UnityEngine.Debug.Log(ClassName);
#if ILRuntime
        string Itypekey = "HotFix_Project.HF_" + ClassName;
        IType Itype = appdomain.GetType(Itypekey);
        if (Itype == null) return null;

        IMethod method = Itype.GetMethod(FunctionName, ParamNum);
        if (method == null) return null;
        if (IsNeedReturn)
            return appdomain.Invoke(method, null, date);
        else
        {
            appdomain.Invoke(method, null, date);
            return method;
        }
#else
        return null;
#endif
    }

}