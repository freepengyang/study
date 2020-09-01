using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode; 
#endif
using System.Collections;
using System.IO;

public class XcodeProjectMod : MonoBehaviour
{
#if UNITY_IOS

    internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath))
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		print (path);
		if (buildTarget == BuildTarget.iOS)
		{
			string projPath = PBXProject.GetPBXProjectPath(path);
			PBXProject proj = new PBXProject();

			proj.ReadFromString(File.ReadAllText(projPath));
			string target = proj.TargetGuidByName("Unity-iPhone");

//			// 追加自己的框架
//			CopyAndReplaceDirectory ("Assets/Lib/UnityAds.framework", Path.Combine (path, "Frameworks/UnityAds.framework"));
//			proj.AddFileToBuild(target, proj.AddFile("Frameworks/UnityAds.framework", "Frameworks/UnityAds.framework", PBXSourceTree.Source));
//
//			CopyAndReplaceDirectory ("Assets/Lib/GoogleMobileAds.framework", Path.Combine (path, "Frameworks/GoogleMobileAds.framework"));
//			proj.AddFileToBuild(target, proj.AddFile("Frameworks/GoogleMobileAds.framework", "Frameworks/GoogleMobileAds.framework", PBXSourceTree.Source));
//
//			CopyAndReplaceDirectory ("Assets/Lib/Chartboost.framework", Path.Combine (path, "Frameworks/Chartboost.framework"));
//			proj.AddFileToBuild(target, proj.AddFile("Frameworks/Chartboost.framework", "Frameworks/Chartboost.framework", PBXSourceTree.Source));

			// 添加文件
//			var fileName = "libFlurry_6.4.0.a";
//			File.Copy(Path.Combine("Assets/Lib", fileName), Path.Combine(path, fileName));
//			proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));
//
//			fileName="Flurry.h";
//			File.Copy(Path.Combine("Assets/Lib", fileName), Path.Combine(path, fileName));
//			proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));

//			string fileName="UnityIOSSDK.m";
//			File.Copy(Path.Combine("Assets/KYScript/lib/", fileName), Path.Combine(path+"/Classes", fileName));
//			proj.AddFileToBuild(target,proj.AddFile(Path.Combine(path+"/Classes", fileName),Path.Combine("/Classes", fileName),PBXSourceTree.Source));

			// 追加系统框架
			proj.AddFrameworkToProject (target, "QuartzCore.framework", true);//false是可选择添加
			proj.AddFrameworkToProject (target, "SystemConfiguration.framework", true);
			proj.AddFrameworkToProject (target, "Security.framework", true);
			proj.AddFrameworkToProject (target, "MobileCoreServices.framework", true);
			proj.AddFrameworkToProject (target, "libsqlite3.tbd", true);
			proj.AddFrameworkToProject (target, "libz.1.dylib", true);
			proj.AddFrameworkToProject (target, "JavaScriptCore.framework", true);
			// Yosemiteでipaが書き出せないエラーに対応するための設定
			//proj.SetBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");

			// 设定框架的检索路径设定／追加
			//proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
			//proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

			// 写入
			File.WriteAllText(projPath, proj.WriteToString());
		}
	}
#endif
}
