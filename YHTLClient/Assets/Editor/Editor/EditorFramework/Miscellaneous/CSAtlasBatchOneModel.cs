using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
namespace ExtendEditor
{
    public class MotionSelection
    {
        public CSMotion type;
        public bool isSelect = true;
    }
    public class CSAtlasBatchOneModel : SelectionBase
    {
        /// <summary>
        /// UITexture/Monster/401001/xxx.png 仅支持这样的目录格式（UITexture下面有3层目录，第三层是图片所在地）
        /// UITexture/DirParent/Dir/Attack/xxx.png 仅支持这样的目录格式（UITexture下面有3层目录，第三层是图片所在地）
        /// 对应打出来的UIAtlas放在Resources/Model/DirParent/AtlasModel/xxx_Dir
        /// 对应打出来的图片和Mat放在Resources/Model/DirParent/AtlasModel/Res/xxx_Dir
        /// 上面的Dir就是下面的dirInfo
        /// </summary>
        static string mFailAtlasPath;
        static string mNotPackAtlasTxtPath;
        static List<string> mNotPackAtlasPathRelationAssetsList = new List<string>();
        public class AtlasBatchData
        {
            /// <summary>
            /// Monster Player 等等
            /// </summary>
            public DirectoryInfo typeInfo;
            public DirectoryInfo idInfo;

            public string atlasParentDir;
            public string atlasDir;
            public string texAndMatDir;

            public string atlasPath;
            public string texPath;
            public string matPath;

            public string atlasPathRelationAssets;
            public string texPathRelationAssets;
            public string matPathRelationAssets;


            public string atlasName;
            public List<FileInfo> texFileInfoList = new List<FileInfo>();
        }

        protected override bool IsGetAllSelectFile
        {
            get
            {
                return false;
            }
        }

        private List<AtlasBatchData> mDataList = new List<AtlasBatchData>();
        private static string mLogTxtPath = "";
        private int mDealIndex = -1;
        float mLastWaitTime = 0;
        float mWaitTime = 1;
        private List<MotionSelection> motionList = new List<MotionSelection>();
        [MenuItem("Tools/Miscellaneous/3001_打Atlas:一个模型一个图集", false, 3001)]
        public static void CSAtlasBatchProc()
        {
            EditorWindow win = GetWindow(typeof(CSAtlasBatchOneModel));
            win.title = "一个模型一个图集";
            win.minSize = new Vector2(300, 400);
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.LabelField("Select Directory Num = " + mDataList.Count + " 正在处理:" + mDealIndex + " " + (base.CanHandle() && NGUISettings.atlas != null ? NGUISettings.atlas.name : ""));
            DrawAllMotionSelection();
            mLogTxtPath = Application.dataPath + "/../一个模型一个图集Log.txt";
            NGUIEditorTools.DrawSeparator();
            if (GUILayout.Button("Deal"))
            {
                FileUtility.Write(mLogTxtPath, "-----------" + System.DateTime.Now + "---------------" + "\r\n");
                EditorUtility.DisplayProgressBar("获得数据中", "获得数据中, please wait...", 0);
                GetDataList();
                mDealIndex = 0;
                EditorUtility.ClearProgressBar();
            }
            if (mDealIndex >= 0)
            {
                PackAtlas();
            }

            if (GUILayout.Button("Open Log"))
            {
                OpenTxt();
            }
        }

        void DrawAllMotionSelection()
        {
            if (motionList.Count == 0)
            {
                foreach (CSMotion type in Enum.GetValues(typeof(CSMotion)))
                {
                    MotionSelection data = new MotionSelection();
                    data.type = type;
                    data.isSelect = true;
                    motionList.Add(data);
                }
            }
            for (int i = 0; i < motionList.Count; i++)
            {
                MotionSelection data = motionList[i];
                bool isSelect = EditorGUILayout.Toggle(i==0?"全选":data.type.ToString(), data.isSelect);
                if (data.isSelect != isSelect)
                {
                    data.isSelect = isSelect;
                    if (i == 0)
                    {
                        for (int j = 1; j < motionList.Count; j++)
                        {
                            MotionSelection d = motionList[j];
                            d.isSelect = !d.isSelect;
                        }
                    }
                }
            }
        }

        bool IsSection(string dir)
        {
            for (int i = 0; i < motionList.Count; i++)
            {
                MotionSelection data = motionList[i];
                if (dir.Contains(data.type.ToString())) return data.isSelect;
            }
            return false;
        }

        void OpenTxt()
        {
            string path = mLogTxtPath;
            if (!File.Exists(path))
            {
                FNDebug.LogError("path = " + path + " 不存在");
                EditorUtility.DisplayDialog("Error", @"文件不存在，请先执行""Find All""操作", "确定");
                return;
            }

            System.Diagnostics.Process pp = System.Diagnostics.Process.Start(path);
        }


        void PackAtlas()
        {
            if (mDealIndex != -1 && mDealIndex < mDataList.Count)
            {
                try
                {
                    if ((float)EditorApplication.timeSinceStartup - mLastWaitTime > mWaitTime)
                    {
                        bool isPack = PackAtlas(mDataList[mDealIndex]);
                        if (!isPack)
                        {
                            mLastWaitTime = (float)EditorApplication.timeSinceStartup;
                            return;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayProgressBar("等待", "等待中", ((float)EditorApplication.timeSinceStartup - mLastWaitTime) / mLastWaitTime);
                        return;
                    }
                    
                    if (mFailAtlasPath != mDataList[mDealIndex].atlasPath)
                    {
                        FileUtility.Write(mLogTxtPath, "Sucess =" + mDataList[mDealIndex].atlasPath + " " + System.DateTime.Now + "\r\n");
                        FNDebug.Log("Sucess =" + mDataList[mDealIndex].atlasPath);
                    }
                    EditorUtility.UnloadUnusedAssetsImmediate();
                    GC.Collect();
                    mDealIndex++;
                }
                catch (System.Exception ex)
                {
                    
                    mDealIndex++;
                }
                if (mDealIndex >= mDataList.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.UnloadUnusedAssetsImmediate();
                    GC.Collect();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
            else
            {
                mDealIndex = -1;
                mDataList.Clear();
                OpenTxt();
            }
        }

        bool PackAtlas(AtlasBatchData data)
        {
            CheckCreateOutputDir(data);
            return CheckCreateAndUpdateAtlas(data);
        }
        #region GetData
        void GetDataList()
        {
            mNotPackAtlasTxtPath = Application.dataPath + "/..//NotPackAtlasPath.txt";
            string notPath = FileUtility.ReadToEnd(mNotPackAtlasTxtPath);
            string[] strs = notPath.Replace("\r\n","\n").Split('\n');
            mNotPackAtlasPathRelationAssetsList.Clear();
            mNotPackAtlasPathRelationAssetsList.AddRange(strs);
            

            mDataList.Clear();
            List<DirectoryInfo> list = new List<DirectoryInfo>();
            for (int i = 0; i < mSelectObjs.Length; i++)
            {
                EditorUtility.DisplayProgressBar("获得数据中", "获得数据中, please wait...", i * 1.0f / mSelectObjs.Length);
                string path = AssetDatabase.GetAssetPath(mSelectObjs[i]);
                path = Application.dataPath + path.Replace("Assets/", "/");
                if (!path.Contains("/UItexture/"))
                {
                    continue;
                }
                DirectoryInfo dir = new DirectoryInfo(path);
                GetDeepTexFileInfo(dir, list);
            }
            for (int j = 0; j < list.Count; j++)
            {
                DirectoryInfo file = list[j];
                if (file == null) continue;
                string s = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                if (mNotPackAtlasPathRelationAssetsList.Contains(s))
                    continue;
                AddData(file);
            }
        }

        void AddData(DirectoryInfo dir)
        {
            if (dir == null) return;
            
            AtlasBatchData data = new AtlasBatchData();
            data.idInfo = dir;
            data.typeInfo = dir.Parent;
            List<string> pngList = new List<string>();
            FileUtility.GetDeepAssetPaths(dir.FullName, pngList, ".png");

            for (int i = 0; i < pngList.Count; i++)
            {
                FileInfo f = new FileInfo(pngList[i]);
                if (f.Extension == ".png")
                {
                    if (!IsSection(f.FullName)) continue;
                    data.texFileInfoList.Add(f);
                }
            }

            string atlasParentPath = dir.Parent.FullName.Replace("UItexture", "AssetBundleRes/Model").Replace("\\", "/");
            data.atlasName = data.idInfo.Name;
            data.atlasParentDir = atlasParentPath;
            data.atlasDir = atlasParentPath + "/Atlas";
            data.texAndMatDir = data.atlasDir + "/Res";

            data.atlasPath = data.atlasDir + "/" + data.atlasName + ".prefab";
            data.texPath = data.texAndMatDir + "/" + data.atlasName + ".png";
            data.matPath = data.texAndMatDir + "/" + data.atlasName + ".mat";

            data.atlasPathRelationAssets = data.atlasPath.Replace(Application.dataPath, "Assets");
            data.texPathRelationAssets = data.texPath.Replace(Application.dataPath, "Assets");
            data.matPathRelationAssets = data.matPath.Replace(Application.dataPath, "Assets");

            mDataList.Add(data);
        }

        void GetDeepTexFileInfo(DirectoryInfo dir, List<DirectoryInfo> list)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();
            if (dirs.Length == 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i];
                    if (file.Extension != ".png") continue;
                    //模型文件加 是数字
                    int modelID = -1;
                    if(!int.TryParse(dir.Parent.Name,out modelID))break;
                    DirectoryInfo topDirInfo = GetTopDirInfo(dir);
                    if (topDirInfo == null) continue;
                    if (topDirInfo.Name != "UItexture") break;
                    if (list.Find(x => x.FullName == dir.Parent.FullName) != null) break;
                    list.Add(dir.Parent);
                    return;
                }
            }
            else
            {
                for (int i = 0; i < dirs.Length; i++)
                {
                    GetDeepTexFileInfo(dirs[i], list);
                }
            }
            return;
        }

        DirectoryInfo GetTopDirInfo(DirectoryInfo info)
        {
            DirectoryInfo dir = info;
            while (dir.Parent != null)
            {
                string path = dir.Parent.FullName.Replace("\\", "/").Replace(Application.dataPath, "");
                if (string.IsNullOrEmpty(path))
                {
                    return dir;
                }
                dir = dir.Parent;
            }
            return dir;
        }

        bool IsValidInEnum(string str)
        {
            foreach (string s in Enum.GetNames(typeof(CSMotion)))
            {
                if (s == str) return true;
            }
            return false;
        }

        #endregion

        #region
        void CheckCreateOutputDir(AtlasBatchData data)
        {
            bool isNeedRefresh = false;
            if (!Directory.Exists(data.atlasParentDir))
            {
                FileUtility.DetectCreateDirectory(data.atlasParentDir);
                isNeedRefresh = true;
            }
            if (!Directory.Exists(data.atlasDir))
            {
                FileUtility.DetectCreateDirectory(data.atlasDir);
                isNeedRefresh = true;
            }
            if (!Directory.Exists(data.texAndMatDir))
            {
                FileUtility.DetectCreateDirectory(data.texAndMatDir);
                isNeedRefresh = true;
            }

            if (isNeedRefresh)
            {
                EditorUtility.DisplayProgressBar("创建目录", "创建目录:" + data.atlasDir + ", please wait...", GetProgressValue());
                //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }

        bool CheckCreateAndUpdateAtlas(AtlasBatchData data)
        {
           // bool replace = false;
            if (File.Exists(data.atlasPath))
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(data.atlasPathRelationAssets, typeof(GameObject)) as GameObject;
                NGUISettings.atlas = go.GetComponent<UIAtlas>();
                Selection.activeObject = NGUISettings.atlas;
            }
            else
            {
                EditorUtility.DisplayProgressBar("创建图集", "创建图集:" + data.atlasName + ", please wait...", GetProgressValue());
                CreateAtlas(data);
                GameObject go = AssetDatabase.LoadAssetAtPath(data.atlasPathRelationAssets, typeof(GameObject)) as GameObject;
                NGUISettings.atlas = go.GetComponent<UIAtlas>();
                //replace = true;
                Selection.activeObject = NGUISettings.atlas;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                return false;
            }

            if (data.texFileInfoList.Count > 0)
            {
                List<Texture> list = new List<Texture>();
                foreach (FileInfo info in data.texFileInfoList)
                {
                    string path = info.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                    Texture tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
                    if (tex != null)
                        list.Add(tex);
                }
                if (list.Count > 0)
                {
                    EditorUtility.DisplayProgressBar("更新图片", "更新Atlas:" + data.atlasName + "(" + mDealIndex + "/" + mDataList.Count + ")", GetProgressValue());
                    UIAtlasMaker.RemoveExtractSprite(list);
                    UIAtlasMaker.SetAtlasReadable(true);
                    UIAtlasMaker.UpdateAtlas(list, true, false, false, OnPackFail);
                    UIAtlasMaker.SetAtlasReadable(false);
                    MoveFromTo(data.atlasPathRelationAssets.Replace(".prefab", ".png"), data.texPathRelationAssets);
                }
            }
            return true;
        }

        float GetProgressValue()
        {
            if (mDataList.Count == 0) return 0;
            if (mDealIndex == -1) return 0;
            return mDealIndex * 1.0f / mDataList.Count;
        }

        void CreateAtlas(AtlasBatchData data)
        {
            NGUISettings.currentPath = System.IO.Path.GetDirectoryName(data.atlasPathRelationAssets);
            GameObject go = AssetDatabase.LoadAssetAtPath(data.atlasPathRelationAssets, typeof(GameObject)) as GameObject;
            string matPath = data.matPathRelationAssets;

            // Try to load the material
            Material mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;

            // If the material doesn't exist, create it
            if (mat == null)
            {
                Shader shader = Shader.Find(NGUISettings.atlasPMA ? "Unlit/Premultiplied Colored" : "Unlit/Transparent Colored");
                mat = new Material(shader);

                // Save the material
                AssetDatabase.CreateAsset(mat, matPath);
                //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

                // Load the material so it's usable
                mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
            }

            // Create a new prefab for the atlas
            UnityEngine.Object prefab = (go != null) ? go : PrefabUtility.CreateEmptyPrefab(data.atlasPathRelationAssets);

            // Create a new game object for the atlas
            string atlasName = data.atlasName;
            go = new GameObject(atlasName);
            go.AddComponent<UIAtlas>().spriteMaterial = mat;

            // Update the prefab
            PrefabUtility.ReplacePrefab(go, prefab);
            DestroyImmediate(go);
            AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            // Select the atlas
            go = AssetDatabase.LoadAssetAtPath(data.atlasPathRelationAssets, typeof(GameObject)) as GameObject;
            NGUISettings.atlas = go.GetComponent<UIAtlas>();
            //Selection.activeGameObject = go;
        }

        void OnPackFail(UIAtlas atlas)
        {
            if (atlas == null) return;
            mFailAtlasPath = AssetDatabase.GetAssetPath(atlas);
            FileUtility.Write(mLogTxtPath, "Error 超过4096上限，请使用分动作打图集=" + mFailAtlasPath + " " + System.DateTime.Now + "\r\n");
            FNDebug.LogError("Error 超过4096上限，请使用分动作打图集=" + mFailAtlasPath);
            mFailAtlasPath = Application.dataPath.Replace("Assets", "")+mFailAtlasPath;
        }

        void MoveFromTo(string fromPath, string toPath)
        {
            string pre = Application.dataPath.Replace("Assets", "");
            if (!File.Exists(pre + fromPath)) return;
            if (File.Exists(pre + toPath)) return;
            AssetDatabase.MoveAsset(fromPath, toPath);
        }

        #endregion
    }

}
