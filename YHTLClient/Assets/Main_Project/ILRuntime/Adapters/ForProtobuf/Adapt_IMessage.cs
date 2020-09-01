using System;
using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System.Collections.Generic;
using ILRuntime.CLR.TypeSystem;

public class Adapt_IMessage : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(IMessage); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MyAdaptor, IMessage
    {
        static Dictionary<ILType, AdaptHelper.AdaptMethod[]> ms_iltype_to_adaptmethods = new Dictionary<ILType, AdaptHelper.AdaptMethod[]>(512);

        public Adaptor(AppDomain appdomain, ILTypeInstance instance) : base(appdomain,instance)
        {

        }

        protected override AdaptHelper.AdaptMethod[] GetAdaptMethods()
        {
            var type = _instance.Type;
            AdaptHelper.AdaptMethod[] adaptMethods = null;
            if (!ms_iltype_to_adaptmethods.TryGetValue(type,out adaptMethods))
            {
                adaptMethods = new AdaptHelper.AdaptMethod[]
                {
                    new AdaptHelper.AdaptMethod { Name = "MergeFrom", ParamCount = 1 },
                    new AdaptHelper.AdaptMethod { Name = "WriteTo", ParamCount = 1 },
                    new AdaptHelper.AdaptMethod { Name = "CalculateSize", ParamCount = 0 },
                };
                ms_iltype_to_adaptmethods.Add(type, adaptMethods);
                //Debug.LogFormat("<color=#00ff00>[ILType]:[{0}]:[{1}]</color>", ms_iltype_to_adaptmethods.Count - 1,type.FullName);
            }
            return adaptMethods;
        }

        public void MergeFrom(CodedInputStream input)
        {
            InvokeNonRet(0,input);
        }

        public void WriteTo(CodedOutputStream output)
        {
            InvokeNonRet(1, output);
        }

        public int CalculateSize()
        {
            return InvokeIntValueRet(2);
        }
    }
}
