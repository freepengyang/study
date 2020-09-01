using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using Object = UnityEngine.Object;


namespace AssetBundles
{
	public class LoadedAssetBundle
	{
		public AssetBundle m_AssetBundle;
		public int m_ReferencedCount;
		public Object asset = null;

		public LoadedAssetBundle(AssetBundle assetBundle, Object obj)
		{
			m_AssetBundle = assetBundle;
			m_ReferencedCount = 1;
			asset = obj;
		}

		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			m_AssetBundle = assetBundle;
			m_ReferencedCount = 1;
		}
	}


	public class AssetBundleManager : MonoBehaviour
	{
		static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
		static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
		static string mPersistentNormalAssetsPath = "";
		static string mPersistentDataPath = "";
		public static AssetBundleManifest m_AssetBundleManifest = null;
		private static string curLoadServerType;
		private static AssetBundle ab;

		// AssetBundleManifest object which can be used to load the dependecies and check suitable assetBundle variants.
		public static AssetBundleManifest AssetBundleManifestObject
		{
			set { m_AssetBundleManifest = value; }
		}

		// Get loaded AssetBundle, only return vaild object when all the dependencies are downloaded successfully.
		static public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			LoadedAssetBundle bundle = null;
			error = "";
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
			//if (bundle == null)
			//    return null;

			//// No dependencies are recorded, only the bundle itself is required.
			//string[] dependencies = null;
			//if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
			//    return bundle;

			//// Make sure all dependencies are loaded
			//foreach (var dependency in dependencies)
			//{
			//    if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
			//        return bundle;

			//    // Wait all the dependent assetBundles being loaded.
			//    LoadedAssetBundle dependentBundle;
			//    m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
			//    if (dependentBundle == null)
			//        return null;
			//}

			return bundle;
		}

		public static void InitializeMaifest()
		{
			mPersistentNormalAssetsPath = $"{Application.persistentDataPath}/0/{CSMisc.GetPlatformName()}/";
			mPersistentDataPath = $"{Application.persistentDataPath}/{CSConstant.ServerType}/{CSMisc.GetPlatformName()}/";

			string path = GetAssetBundlePath(CSMisc.GetPlatformName());

			ab = AssetBundle.LoadFromFile(path);

			if (ab != null)
			{
				AssetBundleManifestObject = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			}
			else
			{
				if (FNDebug.developerConsoleVisible) FNDebug.LogError("--------------AssetBundleManifest -----------");
			}
		}
		
		public static void LoadMaifest(string serverType)
		{
			if(curLoadServerType == serverType) return;
			if(ab != null)
				ab.Unload(false);
			string path = GetAssetBundlePath(CSMisc.GetPlatformName());
			ab = AssetBundle.LoadFromFile(path);
			if (ab != null)
			{
				AssetBundleManifestObject = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			}
			else
			{
				if (FNDebug.developerConsoleVisible) FNDebug.LogError("--------------AssetBundleManifest -----------");
			}
		}

		#region 卸载AssetBundle

		// Unload assetbundle and its dependencies.
		static public void UnloadAssetBundle(string assetBundleName)
		{
			string uiName = "ui/" + (assetBundleName.Replace("(Clone)", "")).ToLower();
			UnloadAssetBundleInternal(uiName);
			UnloadDependencies(uiName);
		}

		static protected void UnloadDependencies(string assetBundleName)
		{
			string[] dependencies = null;

			if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
				return;

			// Loop dependencies.
			for (int i = 0; i < dependencies.Length; i++)
			{
				UnloadAssetBundleInternal(dependencies[i]);
			}

			m_Dependencies.Remove(assetBundleName);
		}

		static protected void UnloadAssetBundleInternal(string assetBundleName)
		{
			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);

			if (bundle == null) return;

			if (--bundle.m_ReferencedCount == 0)
			{
				bundle.m_AssetBundle.Unload(true);
				m_LoadedAssetBundles.Remove(assetBundleName);
			}
		}

		#endregion

		#region 同步加载AssetBundle

		static public LoadedAssetBundle LoadUIAssetAsync(string assetBundleName)
		{
			if (m_AssetBundleManifest == null)
			{
				if (FNDebug.developerConsoleVisible)
					FNDebug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return null;
			}

			// Get dependecies from the AssetBundleManifest object..
			string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);


			LoadUIAssetBundleInternal(assetBundleName);

			if (!m_Dependencies.ContainsKey(assetBundleName))
				m_Dependencies.Add(assetBundleName, dependencies);

			for (int i = 0; i < dependencies.Length; i++)
			{
				LoadUIAssetBundleInternal(dependencies[i]);
			}

			LoadedAssetBundle bundle = null;

			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);

			return bundle;
		}


		/// <summary>
		/// 检查本地有没有资源，没有拿默认里面资源
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public static string GetAssetBundlePath(string assetBundleName)
		{
			string path = mPersistentDataPath + assetBundleName;

			if (!File.Exists(path))
			{
				path = mPersistentNormalAssetsPath + assetBundleName;
				curLoadServerType = "0";
			}else
			{
				curLoadServerType = CSConstant.ServerType;
			}

			return path;
		}

		static public LoadedAssetBundle LoadUIAssetBundleInternal(string assetBundleName)
		{
			LoadedAssetBundle bundle = null;

			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);

			if (bundle != null)
			{
				bundle.m_ReferencedCount++;
				return bundle;
			}

			string url = GetAssetBundlePath(assetBundleName);

			AssetBundle ab = AssetBundle.LoadFromFile(url);

			if (ab != null)
			{
				bundle = new LoadedAssetBundle(ab);
				m_LoadedAssetBundles.Add(assetBundleName, bundle);
			}
			else
			{
				if (FNDebug.developerConsoleVisible) FNDebug.LogError(assetBundleName + " == null");
			}

			return bundle;
		}

		#endregion


		#region 异步加载AssetBundle

		public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
		{
			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
			return bundle;
		}

		public static IEnumerator LoadUIAssetToAsync(string assetBundleName)
		{
			assetBundleName = "ui/" + assetBundleName.ToLower();
			if (m_AssetBundleManifest == null)
			{
				FNDebug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				yield break;
			}

			List<string> loadABList = new List<string>();
			string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);


			foreach (string dependence in dependencies)
			{
				LoadUIAssetBundleInternalAsync(dependence, loadABList);
			}

			LoadUIAssetBundleInternalAsync(assetBundleName);

			while (!m_LoadedAssetBundles.ContainsKey(assetBundleName) || loadABList.Count != dependencies.Length)
			{
				yield return null;
			}
		}

		public static void LoadUIAssetBundleInternalAsync(string assetBundleName, List<string> loadABList = null, int index = 0)
		{
			if (index >= 3)
			{
				FNDebug.LogError($"{assetBundleName}  is Load Fail -----------------");
				if(loadABList != null) loadABList.Add(assetBundleName);
				return;
			}

			LoadedAssetBundle bundle = null;

			if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
			{
				bundle.m_ReferencedCount++;
				if(loadABList != null) loadABList.Add(assetBundleName);
				return;
			}

			string thisPath = mPersistentDataPath + assetBundleName;


			if (!File.Exists(thisPath))
			{
				string webPath = GetWebAssetBundlePath(assetBundleName);
				CSGame.Sington.StartCoroutine(CSPreDownLoadManger.Instance.LoadSingeRes(assetBundleName, webPath, (string a,bool b)=>{
				    LoadUIAssetBundleInternalAsync(assetBundleName,loadABList, index++);
				}));
				//LoadUIAssetBundleInternalAsync(assetBundleName,loadABList, index++);
			}
			else
			{
				AssetBundle ab = AssetBundle.LoadFromFile(thisPath);
				if (ab != null)
				{
					bundle = new LoadedAssetBundle(ab);
					m_LoadedAssetBundles.Add(assetBundleName, bundle);

					if(loadABList != null) loadABList.Add(assetBundleName);
				}else
				{
				    FNDebug.LogError(assetBundleName + " == null");
				}
			}
		}

		public static string GetWebAssetBundlePath(string assetBundleName)
		{
			return SFOut.URL_mServerResURL + "Android/" + assetBundleName;
		}

		#endregion
	}
}