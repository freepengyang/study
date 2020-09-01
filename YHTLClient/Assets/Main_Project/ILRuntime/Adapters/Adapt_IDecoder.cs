using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using ILRuntime.CLR.TypeSystem;
using System.Collections.Generic;

public class Adapt_IDecoder : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(IDecoder); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MyAdaptor, IDecoder
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
                    new AdaptHelper.AdaptMethod { Name = "DeEncode", ParamCount = 2 },
                };
                ms_iltype_to_adaptmethods.Add(type, adaptMethods);
                //Debug.LogFormat("<color=#00ff00>[ILType]:[{0}]:[{1}]</color>", ms_iltype_to_adaptmethods.Count - 1,type.FullName);
            }
            return adaptMethods;
        }

        public ITable DeEncode(System.IO.Stream stream, byte[] buffer)
        {
            return (ITable)Invoke(0,stream,buffer);
        }
    }
}
