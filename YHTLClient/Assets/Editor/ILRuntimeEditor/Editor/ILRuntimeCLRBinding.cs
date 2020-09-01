#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    [MenuItem("Tools/ILRuntime/Generate CLR Binding Code")]
    static void GenerateCLRBinding()
    {
        List<Type> types = new List<Type>();
        types.Add(typeof(int));
        types.Add(typeof(float));
        types.Add(typeof(long));
        types.Add(typeof(object));
        types.Add(typeof(string));
        types.Add(typeof(Array));
        types.Add(typeof(Vector2));
        types.Add(typeof(Vector3));
        types.Add(typeof(Quaternion));
        types.Add(typeof(GameObject));
        types.Add(typeof(UnityEngine.Object));
        types.Add(typeof(Transform));
        types.Add(typeof(RectTransform));
        types.Add(typeof(Time));
        types.Add(typeof(FNDebug));
        types.Add((typeof(CSMisc.Dot2)));
        //所有DLL内的类型的真实C#类型都是ILTypeInstance
        types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types,
            "Assets/Main_Project/ILRuntime/Generated");
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/ILRuntime/Generate CLR Binding Code by Analysis")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        //string path = Application.dataPath + "/../Data/HotFix_Project.dll";
        //string pathDefault = "Assets/StreamingAssets/HotFix_Project.dll";
        string path = Application.dataPath + "/../Library/ScriptAssemblies/UIHotResPanel.dll";
        using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            domain.LoadAssembly(fs);

            //Crossbind Adapter is needed to generate the correct binding code
            InitILRuntime(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain,
                "Assets/Main_Project/ILRuntime/Generated");
        }

        AssetDatabase.Refresh();
    }
    
    [MenuItem("Tools/ILRuntime/生成跨域继承适配器")]
    static void GenerateCrossbindAdapter()
    {
        //由于跨域继承特殊性太多，自动生成无法实现完全无副作用生成，所以这里提供的代码自动生成主要是给大家生成个初始模版，简化大家的工作
        //大多数情况直接使用自动生成的模版即可，如果遇到问题可以手动去修改生成后的文件，因此这里需要大家自行处理是否覆盖的问题

        /*using(System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Samples/ILRuntime/1.6.3/Demo/Scripts/Examples/04_Inheritance/InheritanceAdapter.cs"))
        {
            sw.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(TestClassBase), "ILRuntimeDemo"));
        }*/

        AssetDatabase.Refresh();
    }

    static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
    {
        //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
        domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        domain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        domain.RegisterCrossBindingAdaptor(new IDisposableAdapter());
        domain.RegisterCrossBindingAdaptor(new IEnumerableAdapter());
        domain.RegisterCrossBindingAdaptor(new UISliderAdapter());
        domain.RegisterCrossBindingAdaptor(new UICenterOnChildAdapter());
        domain.RegisterCrossBindingAdaptor(new UIScrollViewAdapter());
        domain.RegisterCrossBindingAdaptor(new SpringPanelAdapter());
        domain.RegisterCrossBindingAdaptor(new UIDragDropItemAdapter());
        domain.RegisterCrossBindingAdaptor(new UITweenerAdapter());
        domain.RegisterCrossBindingAdaptor(new UITableAdapter());
        domain.RegisterCrossBindingAdaptor(new UIWrapContentAdapter());
        domain.RegisterCrossBindingAdaptor(new Adapt_IMessage());
        domain.RegisterCrossBindingAdaptor(new IComparableAdapter());
        domain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        domain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        domain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
        domain.DelegateManager.RegisterFunctionDelegate<global::Adapt_IMessage.Adaptor>();
        //other
        domain.RegisterCrossBindingAdaptor(new SystemExceptionAdapter());
        domain.RegisterCrossBindingAdaptor(new BehaviourProviderAdapter());
        domain.RegisterCrossBindingAdaptor(new AvatarUnitAdapter());
        domain.RegisterValueTypeBinder(typeof(CSMisc.Dot2), new CSMiscDot2Binding());
    }
}
#endif