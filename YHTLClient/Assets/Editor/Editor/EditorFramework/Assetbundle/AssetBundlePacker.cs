using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
namespace ExtendEditor
{
    public enum EPackType
    {
        特定路径差异打包,
        特定路径强制打包,
        自定义路径打包,
    }
    public class AssetBundlePacker : SelectionBase
    {
        [MenuItem("Tools/AssetBundle/Pack Select to AssetBundle %g")]
        public static void PackAssetBundle()
        {
            EditorWindow window = GetWindow(typeof(AssetBundlePacker));
            window.Show();
        }

        [MenuItem("Assets/AssetBundle/Pack Select to AssetBundle")]
        public static void RightClick()
        {
            EditorWindow window = GetWindow(typeof(AssetBundlePacker));
            window.Show();
        }

        protected BuildTarget mBuildTarget = BuildTarget.Android;
        protected EPackType mEPackType = EPackType.特定路径差异打包;
        protected bool mIsBeginDeal = false;
        protected int mDeleteIndex = -1;
        protected int mDeleteDealCount = 0;
        protected string targetDir = "";
        protected List<string> mDeleteDealPathList = new List<string>();
        protected List<List<string>> mBuildPathList = new List<List<string>>()
        {
            new List<string>(){"/Map/"},
            new List<string>(){"/MiniMap/"},
            //new List<string>(){"/UI/Prefabs/"},
            new List<string>(){"/Model/",".prefab"},
            new List<string>(){"/Audio/"},
			new List<string>(){"/ScaleMap/"},
            new List<string>(){"/Model/Effect/"},
			new List<string>(){"/UIPlayer/"},
			new List<string>(){"/UIWeapon/"},
			new List<string>(){"/UIWing/"},        };
        protected bool mIsBuildByBuildPathSet = true;
        protected float mLastBuildTime = 0;
        protected int mBuildTiemInternal = 1;
        protected List<string> mBuildTypeDescList = new List<string>()
        {
            "特定路径打包：对AssetBundleRes下以上目录的资源进行操作，在AssetBundle/文件夹下面存放相应的MD5以及依赖集的MD5码判断文件是否修改",
            "选中打包：对AssetBundleRes下以上目录的资源进行操作，无论文件有没有修改，都进行打包",
            "自定义路径打包：任何目录，会打开文件保存路径对话框",
        };
        public override void OnEnable()
        {
            base.OnEnable();
            //EditorApplication.update += UpdateBack;
            //Debug.LogError("BuildTarget = " + (int)mBuildTarget);
            mBuildTarget = (BuildTarget)EditorPrefsUtility.GetInt(Application.dataPath+"/mBuildTarget", (int)mBuildTarget);
            mEPackType = (EPackType)EditorPrefsUtility.GetInt(Application.dataPath + "/mEPackType", (int)mEPackType);
        }

        void OnDisable()
        {
            //EditorApplication.update -= UpdateBack;
        }

        void UpdateBack()
        {
            if (!base.CanHandle()) return;
            switch (mEPackType)
            {
                case EPackType.特定路径差异打包:
                    {
                        DetectPackSpecialPath(false,false);
                    }
                    break;
                case EPackType.特定路径强制打包:
                    {
                        DetectPackSpecialPath(true,false);
                    }
                    break;
                case EPackType.自定义路径打包:
                    {
                    }
                    break;
            }
        }

        public virtual void OnClickPack() { }
        protected double mNextPackTime = 0;
        public override void OnGUI()
        {
            if (base.CanHandle()) return;
            base.OnGUI();
            BuildTarget target = (BuildTarget)EditorGUILayout.EnumPopup("BuildTarget", mBuildTarget);

            EPackType type = (EPackType)EditorGUILayout.EnumPopup("PackType", mEPackType);
            mIsBuildByBuildPathSet = EditorGUILayout.Toggle("根据路径匹配打包", mIsBuildByBuildPathSet);
            if (target != mBuildTarget || type != mEPackType)
            {
                mBuildTarget = target;
                mEPackType = type;
                SavePrefs();
            }
            mBuildTiemInternal = EditorGUILayout.IntField("每次运行次数（越大越快）", mBuildTiemInternal);
            if (GUILayout.Button("Pack AssetBundle"))
            {
                if ((int)mBuildTarget == 0)
                {
                    EditorUtility.DisplayDialog("Error", "BuildTarget must be select", "Ok");
                    return;
                }

                if (mPathList.Count == 0)
                {
                    EditorUtility.DisplayDialog("Error", "请选择资源", "Ok");
                    return;
                }

                switch (mEPackType)
                {
                    case EPackType.特定路径差异打包:
                    case EPackType.特定路径强制打包:
                        {
                            OnClickPack();
                            base.BeginHandle();
                        }
                        break;
                    case EPackType.自定义路径打包:
                        {
                            if(Selection.activeObject != null)
                            {
                                PackCustomBundle(Selection.activeObject,Selection.activeObject.name);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Error", "请选中一个需要打包的资源", "Ok");
                            }
                        }
                        break;
                }
            }

            switch (mEPackType)
            {
                case EPackType.特定路径差异打包:
                    {
                        DetectPackSpecialPath();
                    }
                    break;
                case EPackType.特定路径强制打包:
                    {
                        DetectPackSpecialPath(true);
                    }
                    break;
                case EPackType.自定义路径打包:
                    {
                    }
                    break;
            }
            EditorGUILayout.LabelField("说明:" + mBuildTypeDescList[(int)mEPackType]);
            //if (GUILayout.Button("删除无用AssetBundle"))
            //{
            //    Clear(true);
            //}
            //Clear();
            //if (GUILayout.Button("上传MD5"))
            //{
            //    FileUtility.UseCommandUploadFileToSVN(Application.dataPath + "/../AssetBundle");
            //}
            //if (GUILayout.Button("上传AssetBundle文件"))
            //{
            //    targetDir = mBuildTarget == BuildTarget.StandaloneWindows64 ? "/../StandaloneWinows64/StreamingAssets" : "/StreamingAssets";
            //    FileUtility.UseCommandUploadFileToSVN(Application.dataPath + targetDir);
            //}

            if (GUILayout.Button("启动游戏 Deal OutLog Data.txt"))
            {
                //DealText();
                DealLargeText();
            }
            DealTextProc();
            GUILayout.Label("Cur Test Proc = " + dealIndex + "/" + contentLint.Count);
            GUILayout.Label("Cur Write Proc = " + writeIndex + "/" + dic.Count);
        }
        List<string> contentLint = new List<string>();
        int dealIndex = -1;
        int writeIndex = -1;
        string resultOne = "";
        Dictionary<string, int> dic = new Dictionary<string,int>();
        void DealLargeText()
        {
            contentLint.Clear();
            dic.Clear();
            if (CSGame.Sington != null)
                CSGame.Sington.StartCoroutine(SyncReadLargeText());
        }

        IEnumerator SyncReadLargeText()
        {
            string path = Application.dataPath + "/data.txt";
            if (!File.Exists(path)) yield break;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    //content = sr.ReadToEnd();

                    string line;
                    int num = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        contentLint.Add(line);
                        num++;
                        if (num >= 10000)
                        {
                            num = 0;
                            FNDebug.Log("Read line = "+contentLint.Count);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }
            dealIndex = 0;
            resultOne = "";
        }

        void DealTextProc()
        {
            if (dealIndex != -1 && dealIndex < contentLint.Count)
            {
                for (int i = 0; i < 1000; i++)
                {
                    string s = contentLint[dealIndex];
                    int length = s.Length;
                    int indexBegin = s.IndexOf("logString ");
                    //int timeLength =  @"{""time"":".Length;
                    if (indexBegin != -1)
                    {
                        resultOne = s.Substring(indexBegin);
                        if (!resultOne.Contains("-----------------host-------------") && !resultOne.Contains("包内资源列表路径"))
                        {
                            if (!dic.ContainsKey(resultOne))
                                dic.Add(resultOne, 1);
                            else dic[resultOne] = dic[resultOne] + 1;
                        }
                    }
                    dealIndex++;
                    if (dealIndex >= contentLint.Count)
                    {
                        dealIndex = -1;
                        writeIndex = 0;
                        FNDebug.LogError(dic.Count + "个异常");
                        break;
                    }
                }
            }
            
            if (writeIndex != -1)
            {
                string pathOut = Application.dataPath + "/data2.txt";
                string outTxt = "";
               
                for (int j = 0; j < 1000; j++)
                {
                    if (dic.Count > 0)
                    {
                        foreach (var cur in dic)
                        {
                            outTxt += cur.Value + "次 " + cur.Key.Replace(@"\n", "\r\n").Replace(@"""stackTrace """, "\r\n" + @"""stackTrace """) + "\r\n\r\n";
                            dic.Remove(cur.Key);
                            break;
                        }
                    }
                    else
                    {
                        writeIndex = -1;
                    }
                }
                FileUtility.Write(pathOut, outTxt, true);
                if (writeIndex == -1)
                {
                    FileUtility.Open(pathOut);
                }
            }
        }

        public override void OnInspectorUpdate()
        {
            //base.OnInspectorUpdate();
            if (base.CanHandle())
            {
                if (mNextPackTime != 0 && EditorApplication.timeSinceStartup < mNextPackTime) return;
                UpdateBack();
            }
            else
            {
                DealTextProc();
            }
            //Debug.Log("Cur Test Proc = " + mHandleIndex + "/" + mPathList.Count);

        }

        public void DetectPackSpecialPath(bool isForcePack = false,bool isDealUI = true)
        {
            if (!base.CanHandle()) return;
            for (int i = 0; i < mBuildPathList.Count; i++)
            {
                string info = "Special Path =";
                for (int j = 0; j < mBuildPathList[i].Count; j++)
                {
                    info += "AssetBundleRes" + mBuildPathList[i][j];
                }
                if(isDealUI)EditorGUILayout.LabelField(info);
            }
            if (base.CanHandle())
            {
                if (mNextPackTime != 0 && EditorApplication.timeSinceStartup < mNextPackTime)
                {
                    return;
                }
                mLastBuildTime = (float)EditorApplication.timeSinceStartup;
                mIsBeginDeal = true;
                int moveNum = mBuildTiemInternal;
                int moveIndex = 0;
                while (moveIndex < moveNum && base.CanHandle())
                {
                    string path = base.GetCurHandlePath();
                    bool canBundle = false;
                    switch (mEPackType)
                    {
                        case EPackType.特定路径差异打包:
                        case EPackType.特定路径强制打包:
                            {
                                for (int i = 0; i < mBuildPathList.Count; i++)
                                {
                                    bool isFind = true;
                                    for (int j = 0; j < mBuildPathList[i].Count; j++)
                                    {
                                        if (!path.Contains(mBuildPathList[i][j]))
                                        {
                                            isFind = false;
                                            break;
                                        }
                                    }
                                    if (isFind)
                                    {
                                        canBundle = true;
                                        break;
                                    }
                                }
                            }
                            break;
                    }

                    bool isRealCanBundle = false;
                    if (mIsBuildByBuildPathSet)
                    {
                        isRealCanBundle = canBundle && (IsFileChange(true) || isForcePack);
                    }
                    else
                    {
                        isRealCanBundle = (canBundle&&IsFileChange(true)) || isForcePack;
                    }
                    if (isRealCanBundle)
                    {
                        Object obj = base.GetCurHandleObj();
                        if (obj != null)
                        {
                            targetDir = mBuildTarget == BuildTarget.StandaloneWindows64 ? "/../StandaloneWinows64/StreamingAssets/" : "/StreamingAssets/";
                            path = Application.dataPath + targetDir + path.Replace("Assets/", "").Replace("AssetBundleRes/", "");
                            //path = path.Replace("SplitAtlas", "Atlas");
                            //path = path.Replace("AtlasAction", "Atlas"); 
                            //path = path.Replace("AtlasDirection", "Atlas");
                            //path = path.Replace("SplitAtlasDirection", "Atlas");

                            path = path.Replace("SplitAtlas", "");
                            path = path.Replace("AtlasAction", "");
                            path = path.Replace("AtlasDirection", "");
                            path = path.Replace("SplitAtlasDirection", "");

                            FileUtility.DetectCreateDirectory(path);
                            int lastIndex = path.LastIndexOf(".");
                            string extentsion = "";
                            extentsion = ".assetbundle";
                            path = path.Substring(0, lastIndex) + extentsion;
                            
                             #if (!UNITY_4_7&&!UNITY_4_6)
                            if (!PackExtend(path, obj))
                            {
                                BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, mBuildTarget);
                            }
#else
                            BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, mBuildTarget);
#endif
                            //BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, mBuildTarget);
                            if (isDealUI) EditorGUILayout.LabelField("Dealing =" + path);
                        }
                    }
                    else
                    {
                        if (moveNum < 10)
                            moveNum++;
                    }
                    moveIndex++;
                    base.MoveHandle();
                }
            }
            else
            {
                if (mIsBeginDeal)
                {
                    mIsBeginDeal = false;
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Finish", "Done", "Ok");
                }
            }
            targetDir = mBuildTarget == BuildTarget.StandaloneWindows64 ? "/../StandaloneWinows64/StreamingAssets/" : "/StreamingAssets/";
            if (isDealUI) EditorGUILayout.LabelField("输出目录：" + targetDir + "（对应AssetBundleRes文件夹）");
        }

        public virtual bool PackExtend(string path, Object Object) { return false; }

        public void PackCustomBundle(Object obj,string fileName)
        {
            string path = EditorUtility.SaveFilePanel("Save As",
                          fileName, fileName, "asestbundle");

            if (!string.IsNullOrEmpty(path))
            {
                  #if (!UNITY_4_7&&!UNITY_4_6)
                            if (!PackExtend(path, obj))
                            {
                                BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, mBuildTarget);
                            }
#else
                BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, mBuildTarget);
#endif
            }
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            SavePrefs();
        }

        void SavePrefs()
        {
            EditorPrefsUtility.SetInt(Application.dataPath + "/mBuildTarget", (int)mBuildTarget);
            EditorPrefsUtility.SetInt(Application.dataPath + "/mEPackType", (int)mEPackType);
        }

        bool IsFileChange(bool isSaveToMd5 = false)
        {
            return true;
            // string path = base.GetCurHandlePath().Replace("Assets/","");
            // string md5FilePath = Application.dataPath + "/../AssetBundle/MD5/"+path.Substring(0,path.LastIndexOf("."))+".txt";
            // string md5FileContent = FileUtility.ReadToEnd(md5FilePath);
            // string filePath = Application.dataPath + "/" + path;
            // //string Md5 = MD5Utility.GetMD5HashFromFile(filePath);
            // bool isChnage = IsObjectOrDependentChange(md5FileContent);
            // string assetBundlePath = Application.dataPath + "/" + path.Substring(0, path.LastIndexOf(".")).Replace("AssetBundleRes", "StreamingAssets") + ".assetBundle";
            // //bool isChnage = FileUtility.FileIsChange(filePath, md5FilePath);
            // if (!File.Exists(assetBundlePath)) isChnage = true;
            // if (isChnage && isSaveToMd5)
            // {
            //     SaveMD5ToFile(filePath, md5FilePath);
            // }
            // return isChnage;
        }

        bool IsObjectOrDependentChange(string md5FileContent)
        {
            Dictionary<string, string> selfAndDependentDic = new Dictionary<string, string>();//save in txt
            string[] strPair = md5FileContent.Split('\n', '\r');
            for (int i = 0; i < strPair.Length; i++)
            {
                string[] str = strPair[i].Split('#');
                if (str.Length != 2) continue;
                if (!selfAndDependentDic.ContainsKey(str[0]))
                    selfAndDependentDic.Add(str[0], str[1]);
            }
            if (!IsNotAllow()) return false;
            List<Object> dependObjs = GetDependent(base.GetCurHandleObj());
            if (selfAndDependentDic.Count == 0) return true;
            for (int i = 0; i < dependObjs.Count; i++)
            {
                Object obj = dependObjs[i];
                string path = AssetDatabase.GetAssetPath(obj);
                string guid = AssetDatabase.AssetPathToGUID(path);
                //Debug.LogError("guid = " + guid);
                if (!selfAndDependentDic.ContainsKey(guid)) return true;
                string md5 = selfAndDependentDic[guid];
                string curMd5 = MD5Utility.GetMD5HashFromFile(path);
                bool isChange = curMd5 != md5;
                if (isChange) return true;
            }

            return false;
        }

        bool IsNotAllow()
        {
            List<Object> dependObjs = GetDependent(base.GetCurHandleObj());
            for (int i = 0; i < dependObjs.Count; i++)
            {
                Object obj = dependObjs[i];
                string path = AssetDatabase.GetAssetPath(obj);
                                if (path.Contains("Library"))
                {
                    FNDebug.LogError("you need not use the asset in library = " + base.GetCurHandlePath()+" "+obj.name);
                    //base.End();
                    return false;
                }
            }
            return true;
        }

        List<Object> GetDependent(Object obj)
        {
            List<Object> list = new List<Object>();
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { obj });
            list.Add(base.GetCurHandleObj());
            foreach (Object ob in dependObjs)
            {
                if (list.Contains(ob)) continue;
                list.Add(ob);
            }
            return list;
        }

        void SaveMD5ToFile(string filePath, string md5FilePath)
        {
            List<Object> dependObjs = GetDependent(base.GetCurHandleObj());
            string content = "";
            List<string> list = new List<string>();
            for (int i = 0; i < dependObjs.Count; i++)
            {
                Object obj = dependObjs[i];
                string path = AssetDatabase.GetAssetPath(obj);
                //Debug.LogError("path = "+path);
                string guid = AssetDatabase.AssetPathToGUID(path);
                if (list.Contains(guid)) continue;
                list.Add(guid);
                string Md5 = MD5Utility.GetMD5HashFromFile(path);
                content += guid + "#" + Md5 + "\r\n";
            }
            FileUtility.Write(md5FilePath, content, false);
        }

        /// <summary>
        /// clear the delete asset but the assetbundle is still existed;
        /// </summary>
        void Clear(bool isCearBegin = false)
        {
            if (isCearBegin)
            {
                string assetBundlePath = Application.dataPath + "/StreamingAssets";
                
                FileUtility.GetDeepAssetPaths(assetBundlePath, mDeleteDealPathList, ".assetbundle");
                mDeleteDealCount = mDeleteDealPathList.Count;
                mDeleteIndex = 0;
            }
            while (mDeleteIndex != -1 && mDeleteIndex < mDeleteDealCount)
            {
                string streamPath = mDeleteDealPathList[mDeleteIndex];
                string resourcePath = streamPath.Replace("StreamingAssets/", "AssetBundleRes/");
                resourcePath = Application.dataPath + "/../" + resourcePath;
                //Debug.Log("resourcePath = " + resourcePath);
                if (!FileUtility.IsFileExist(resourcePath, true))
                {
                    //删除Assetbundle
                    string deleteAb = Application.dataPath + "/../" + streamPath;
                    //File.Delete(deleteAb);
                    FNDebug.Log("deleteAb = " + deleteAb);
                    mDeleteIndex++;
                    File.Delete(deleteAb);
                    break;
                }
                EditorUtility.DisplayProgressBar("Dealing", (mDeleteIndex + 1) + "@/" + mDeleteDealCount, (mDeleteIndex+1) * 1.0f / mDeleteDealCount);
                mDeleteIndex++;
            }
            if (mDeleteIndex >= mDeleteDealCount)
            {
                mDeleteIndex = -1;
                EditorUtility.ClearProgressBar();
            }
        }

        string path = "";
        List<string> prefabList = new List<string>();
        List<string> matList = new List<string>();
        List<string> texList = new List<string>();

        public void OnGuiTemp()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("路径 = " + path);
            if (GUILayout.Button("OpenDir"))
            {
                path = EditorUtility.OpenFolderPanel("操作文件夹", path, "");
                FileUtility.GetDeepAssetPaths(path, prefabList,".prefab");
                FileUtility.GetDeepAssetPaths(path, matList, ".mat");
                FileUtility.GetDeepAssetPaths(path, texList, ".png");
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button(@"绑定都是的引用（仅支持UIAtlas Mat Texture）"))
            {
                DealProc();
            }
        }

        void DealProc()
        {
            for (int i = 0; i < prefabList.Count; i++)
            {
                if (i < matList.Count && i < texList.Count)
                {
                    DealProc(prefabList[i], matList[i], texList[i]);
                }
                else
                {
                    FNDebug.LogError("mat或者texture没有找到 = " + prefabList[i]);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void DealProc(string prefabPath,string matPath,string texPath)
        {
            GameObject go = FileUtility.GetObject(prefabPath) as GameObject;
            UIAtlas atlas = go.GetComponent<UIAtlas>();
            Material mat = FileUtility.GetObject(matPath) as Material;
            Texture tex = FileUtility.GetObject(texPath) as Texture;
            if (atlas != null && mat != null && tex != null)
            {
                mat.mainTexture = tex;
                atlas.spriteMaterial = mat;
            }
            else
            {
                FNDebug.LogError("Atlas 或者 mat 或者 texture为空 = " + prefabPath);
            }
        }
    }
}