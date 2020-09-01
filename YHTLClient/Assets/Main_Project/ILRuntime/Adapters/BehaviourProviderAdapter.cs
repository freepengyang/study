using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;

public class BehaviourProviderAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(BehaviourProvider);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    class Adaptor : BehaviourProvider, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public Adaptor() { }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }


        IMethod mInitialize;
        bool mInitializeGot;
        //缓存这个数组来避免调用时的GC Alloc
        object[] param1 = new object[1];
        public override bool InitializeFSM(FSMState fsm)
        {
            if (!mInitializeGot)
            {
                mInitialize = instance.Type.GetMethod("InitializeFSM", 1);
                mInitializeGot = true;
            }
            if (mInitialize != null)
            {
                param1[0] = fsm;
                return (bool)appdomain.Invoke(mInitialize, instance, param1);
            }
            else
            {
                return false;
            }
        }

        IMethod mReset;
        bool mResetGot;
        public override void Reset()
        {
            if (!mResetGot)
            {
                mInitialize = instance.Type.GetMethod("Reset", 1);
                mResetGot = true;
            }
            if (mReset != null)
                appdomain.Invoke(mReset, instance, null);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
        }
    }
}