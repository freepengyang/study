using System;  
using System.Collections.Generic;  
using System.IO;  
using UnityEditor;  
using UnityEngine;  
using Object = UnityEngine.Object;  

public class BuildProject  
{   
	[MenuItem("Automated/Automated Android Build")]  
	public static void iOSBuild ()
	{ 
		//鎵撳寘涔嬪墠鍏堣?缃?竴涓娺棰勫畾涔夋爣绛撅紝 鎴戝缓璁?ぇ瀹舵渶濂歼鍋氫竴浜氝 91 鍚屾?鎺н蹇?敤 PP鍔╂墜涓绫荤殑鏍囩?銆佭杩欐牱鍦ㄤ唬鐮佷腑鍙?互鐏垫椿鐨勫紑鍚?鎴栬呭叧闂?涓浜涗唬鐮併佁
		//鍥犱负 杩欓噷鎴戞槸鎵挎帴 涓婁竴绡囨枃绔狅紝 鎴戝氨浠?haresdk鍋氫緥瀛忂锛岃繖鏍锋柟渚垮ぇ瀹跺?涔熯锛嬏
		//PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, "USE_SHARE");
		//杩欓噷灏辨槸鏋勫缓xcode宸ョ▼鐨勬牳蹇冩柟娉曚簡锛嬤
		//鍙傛暟1 闇瑕佹墦鍖呯殑鎵鏈夊満鏅?
		//鍙傛暟2 闇瑕佹墦鍖呯殑鍚嶏紝 杩欓噷鍙栧埌鐨勫氨鏄?shell浼犺繘鏉ョ殑瀛楃?涓边91
		//鍙傛暟3 鎵撳寘骞冲彴
		FNDebug.Log("Unity command line build starts from here\n---------------------------------------");
		BuildPipeline.BuildPlayer(GetBuildScenes(), "/Volumes/JENKINS/JENKINS/Output.iOS/", BuildTarget.iOS, BuildOptions.None);
		FNDebug.Log("Unity command line build ends here\n----------------------------------------------");
	}

    [MenuItem("Automated/Automated Ios Build")]  
	public static void AndroidBuild ()
	{
		FNDebug.Log("Unity command line build starts from here\n--------------------------------------");
//		BuildPipeline.BuildPlayer(GetBuildScenes(), "/Users/eyouplay/Desktop/Output.Android/Output.apk", BuildTarget.Android, BuildOptions.None);
		BuildPipeline.BuildPlayer(GetBuildScenes(), "/Volumes/JENKINS/JENKINS/Output/Step.I/", BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
		FNDebug.Log("Unity command line build ends here\n----------------------------------------------");
	}

	private static string[] GetBuildScenes()  
	{  
		List<string> names = new List<string>();  
		foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)  
		{  
			if (e == null) { continue; }  
			if (e.enabled) { names.Add(e.path); }  
		}  
		return names.ToArray();  
	}
}
