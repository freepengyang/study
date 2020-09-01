using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

public class UIDragDropItemAdapter : CrossBindingAdaptor
{
	public override Type BaseCLRType
	{
		get
		{
			return typeof(UIDragDropItem);
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

	internal class Adaptor : UIDragDropItem, CrossBindingAdaptorType
	{
		ILTypeInstance instance;
		ILRuntime.Runtime.Enviorment.AppDomain appdomain;

		public Adaptor()
		{

		}

		public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
		{
			this.appdomain = appdomain;
			this.instance = instance;
		}

		public ILTypeInstance ILInstance { get { return instance; } }
    }
}