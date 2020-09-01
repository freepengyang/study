
using System;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public static class AdaptHelper
{
    public class AdaptMethod
    {
        public string Name;
        public int ParamCount;
        public IMethod Method;
    }

    public static IMethod GetMethod(this ILType type, AdaptMethod m)
    {
        if (m.Method != null)
            return m.Method;

        m.Method = type.GetMethod(m.Name, m.ParamCount);
        if (m.Method == null)
        {
            string baseClass = "";
            if (type.FirstCLRBaseType != null)
            {
                baseClass = type.FirstCLRBaseType.FullName;
            }
            else if (type.FirstCLRInterface != null)
            {
                baseClass = type.FirstCLRInterface.FullName;
            }

            throw new Exception(string.Format("can't find the method: {0}.{1}:{2}, paramCount={3}", type.FullName, m.Name, baseClass, m.ParamCount));
        }

        return m.Method;
    }
}

public abstract class MyAdaptor : CrossBindingAdaptorType
{
    protected AppDomain AppDomain { get; set; }
    protected ILTypeInstance _instance;
    private AdaptHelper.AdaptMethod[] _methods;

    protected abstract AdaptHelper.AdaptMethod[] GetAdaptMethods();

    public ILTypeInstance ILInstance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    protected object Invoke(int index, params object[] p)
    {
        if (_methods == null)
            _methods = GetAdaptMethods();

        var m = _instance.Type.GetMethod(_methods[index]);
        return AppDomain.Invoke(m, _instance, p);
    }

    protected void InvokeNonRet<T>(int index,T p)
    {
        if (_methods == null)
            _methods = GetAdaptMethods();
        var m = _instance.Type.GetMethod(_methods[index]);
        using (var ctx = AppDomain.BeginInvoke(m))
        {
            ctx.PushObject(_instance,false);
            ctx.PushObject(p,false);
            ctx.Invoke();
        }
    }

    protected int InvokeIntValueRet(int index)
    {
        if (_methods == null)
            _methods = GetAdaptMethods();

        var m = _instance.Type.GetMethod(_methods[index]);
        int result = 0;
        using (var ctx = AppDomain.BeginInvoke(m))
        {
            //依次将参数压入栈，如果为成员方法，第一个参数固定为对象实例
            ctx.PushObject(_instance,false);
            //开始调用
            ctx.Invoke();
            //调用完毕后使用对应的Read方法获取返回值
            result = ctx.ReadInteger();
        }
        return result;
    }

    protected MyAdaptor(AppDomain appdomain, ILTypeInstance instance)
    {
        AppDomain = appdomain;
        _instance = instance;
    }

}

