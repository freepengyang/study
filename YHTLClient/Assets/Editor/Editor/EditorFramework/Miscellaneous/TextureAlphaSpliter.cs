using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace ExtendEditor
{
    public class TextureAlphaSpliter:SelectionBase
    {
        public enum ESpliterType
        {
            Textrue,
            Atlas,
            UIPrefabAtlas,//生成ETC 4的透明通道图，并将原图改成RGB16，将透明通道图放到材质上面，并设置IsAlphaTex参数
            RevertUIPrefabAtlas,//重置上面的步骤
            UIAtlas,//直接对UI Atlas操作
            SetAlphaSpliterFlag,//Set
        }
        const string
            RGBEndName = "(rgb)",
           AlphaEndName = "(a)";

        public static bool WhetherSplit = true;
        public static bool AlphaHalfSize;

        static Texture s_rgba;
        static ESpliterType mSpliterType = ESpliterType.Textrue;
        static List<UIAtlas> prefabAtlas = new List<UIAtlas>();
        static int prefabDealIndex = -1;
        [MenuItem("Tools/Miscellaneous/TextureAlphaSpliter")]
        public static void TextureAlphaSpliterProc()
        {
            EditorWindow win = GetWindow(typeof(TextureAlphaSpliter));
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Split Texture"))
            {
                base.BeginHandle();
                mSpliterType = ESpliterType.Textrue;
            }

            if (GUILayout.Button("Split Atlas"))
            {
                base.BeginHandle();
                mSpliterType = ESpliterType.Atlas;
            }

            if (GUILayout.Button("Split UI Prefab Atlas"))
            {
                string txtPath = Application.dataPath.Replace("Assets", "") + "AssetsChangeDetect";
                System.Diagnostics.Process p = System.Diagnostics.Process.Start("TortoiseProc.exe", @"/command:update" + " /path:" + txtPath + " /closeonend:1");
                p.WaitForExit();

                base.BeginHandle();
                prefabDealIndex = 0;
                mSpliterType = ESpliterType.UIPrefabAtlas;
            }

            if (GUILayout.Button("Revert UI Prefab Atlas"))
            {
                base.BeginHandle();
                prefabDealIndex = 0;
                mSpliterType = ESpliterType.RevertUIPrefabAtlas;
            }

            if (GUILayout.Button("UI Atlas"))
            {
                base.BeginHandle();
                prefabDealIndex = 0;
                mSpliterType = ESpliterType.UIAtlas;
            }

            if (GUILayout.Button("SetAlphaSpliterFlag"))
            {
                base.BeginHandle();
                mSpliterType = ESpliterType.SetAlphaSpliterFlag;
            }

            if (base.CanHandle())
            {
                UnityEngine.Object obj = base.GetCurHandleObj();

                if (mSpliterType == ESpliterType.Textrue)
                {
                    DealTexture(obj);
                }
                else if (mSpliterType == ESpliterType.Atlas)
                {
                    try
                    {
                        DealAtlas(obj);
                        EditorUtility.UnloadUnusedAssetsIgnoreManagedReferences();
                        System.GC.Collect();
                    }
                    catch (System.Exception ex)
                    {
                        FNDebug.LogError(ex);
                    }
                }
                else if(mSpliterType == ESpliterType.UIPrefabAtlas)
                {
                    try
                    {
                        DealUIPrefabAtlas(obj);
                        EditorUtility.UnloadUnusedAssetsImmediate();
                        System.GC.Collect();
                    }
                    catch (System.Exception ex)
                    {
                        FNDebug.LogError(ex);
                    }
                }
                else if (mSpliterType == ESpliterType.RevertUIPrefabAtlas)
                {
                    try
                    {
                        RevertUIPrefabAtlas(obj);
                        EditorUtility.UnloadUnusedAssetsImmediate();
                        System.GC.Collect();
                    }
                    catch (System.Exception ex)
                    {
                        FNDebug.LogError(ex);
                    }
                }
                else if (mSpliterType == ESpliterType.UIAtlas)
                {
                    try
                    {
                        DealUIPrefabAtlas(obj);
                        EditorUtility.UnloadUnusedAssetsImmediate();
                        System.GC.Collect();
                    }
                    catch (System.Exception ex)
                    {
                        FNDebug.LogError(ex);
                    }
                }
                else if (mSpliterType == ESpliterType.SetAlphaSpliterFlag)
                {
                    SetAlphaSpliterFlag();
                }
                if (mSpliterType == ESpliterType.UIPrefabAtlas || mSpliterType == ESpliterType.RevertUIPrefabAtlas)
                {
                    if (prefabDealIndex != -1) return;
                    prefabDealIndex = 0;
                }
                base.MoveHandle();
                if (!base.CanHandle())
                {
                    if (mSpliterType == ESpliterType.UIPrefabAtlas)
                    {
                        string txtPath = Application.dataPath.Replace("Assets", "") + "AssetsChangeDetect";
                        System.Diagnostics.Process p = System.Diagnostics.Process.Start("TortoiseProc.exe", @"/command:commit" + " /path:" + txtPath + " /closeonend:0");
                    }
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }

        void SetAlphaSpliterFlag()
        {
            string path = base.GetCurHandlePath();
            if (!path.Contains(".mat")||!path.Contains("SplitAtlas"))
            {
                return;
            }
            Material mat = base.GetCurHandleObj() as Material;
            if (mat != null && mat.HasProperty("_IsAlphaSplit"))
            {
                mat.SetInt("_IsAlphaSplit", 1);
            }
        }

        void DealUIPrefabAtlas(UnityEngine.Object obj)
        {
            if (prefabDealIndex == -1) return;
            GameObject go = obj as GameObject;
            if(go == null)return;
            string path = AssetDatabase.GetAssetPath(obj);
            if (!path.Contains("Resources/UI/Prefabs/")) return;

            if (prefabDealIndex == 0)
            {
                GetAllAtlas();
            }
            if (prefabDealIndex < prefabAtlas.Count)
            {
                UIAtlas atlas = prefabAtlas[prefabDealIndex];
                if (atlas == null || atlas.spriteMaterial == null || atlas.spriteMaterial.mainTexture == null)
                {
                    prefabDealIndex++;
                    if (prefabDealIndex >= prefabAtlas.Count)
                    {
                        prefabDealIndex = -1;
                    }
                    return;
                }
                string srcPath = AssetDatabase.GetAssetPath(atlas.spriteMaterial.mainTexture);
                if (!FileUtility.IsFileChange(srcPath, true))
                {
                    prefabDealIndex++;
                    if (prefabDealIndex >= prefabAtlas.Count)
                    {
                        prefabDealIndex = -1;
                    }
                    return;
                }
                var importer = (TextureImporter)AssetImporter.GetAtPath(srcPath);
                importer.textureFormat = TextureImporterFormat.ARGB32;
                EditorUtil.SetTexReadable((Texture2D)atlas.spriteMaterial.mainTexture, true);
                string dirPath = FileUtility.GetDirectory(srcPath);
                string saveAssetPath = dirPath + "/" + atlas.spriteMaterial.mainTexture.name + AlphaEndName + ".png";
                FNDebug.LogError(saveAssetPath);
                Texture alpha = CreateAlphaTexture((Texture2D)atlas.spriteMaterial.mainTexture, false, dirPath);
                atlas.spriteMaterial.shader = Shader.Find("Unlit/Transparent Colored");
                atlas.spriteMaterial.SetTexture("_AlphaTex", alpha);
                atlas.spriteMaterial.SetInt("_IsAlphaSplit", 1);
                importer.textureFormat = TextureImporterFormat.RGB16;
                EditorUtil.SetTexReadable((Texture2D)atlas.spriteMaterial.mainTexture, false);

                FileUtility.WriteFileChange(srcPath);

                prefabDealIndex++;
                if (prefabDealIndex >= prefabAtlas.Count)
                {
                    prefabDealIndex = -1;
                }
            }
            else
            {
                prefabDealIndex = -1;
            }
        }

        void RevertUIPrefabAtlas(UnityEngine.Object obj)
        {
            if (prefabDealIndex == -1) return;
            GameObject go = obj as GameObject;
            if (go == null) return;
            string path = AssetDatabase.GetAssetPath(obj);
            if (!path.Contains("Resources/UI/Prefabs/")) return;

            if (prefabDealIndex == 0)
            {
                GetAllAtlas();
            }
            if (prefabDealIndex < prefabAtlas.Count)
            {
                UIAtlas atlas = prefabAtlas[prefabDealIndex];
                if (atlas == null || atlas.spriteMaterial == null || atlas.spriteMaterial.mainTexture == null)
                {
                    prefabDealIndex++;
                    if (prefabDealIndex >= prefabAtlas.Count)
                    {
                        prefabDealIndex = -1;
                    }
                    return;
                }
                string srcPath = AssetDatabase.GetAssetPath(atlas.spriteMaterial.mainTexture);
                var importer = (TextureImporter)AssetImporter.GetAtPath(srcPath);
                EditorUtil.IsDealingTexFormat = true;
                importer.isReadable = false;
                importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                AssetDatabase.ImportAsset(srcPath);
                EditorUtil.IsDealingTexFormat = false;

                Texture alpha = atlas.spriteMaterial.GetTexture("_AlphaTex");
                if (alpha != null)
                {
                    string alphaPath = AssetDatabase.GetAssetPath(alpha);
                    if (alpha != null)
                        AssetDatabase.DeleteAsset(alphaPath);
                }

                atlas.spriteMaterial.SetInt("_IsAlphaSplit", 0);
                prefabDealIndex++;
                if (prefabDealIndex >= prefabAtlas.Count)
                {
                    prefabDealIndex = -1;
                }
            }
            else
            {
                prefabDealIndex = -1;
            }
        }

        void GetAllAtlas()
        {
            prefabAtlas.Clear();
            GameObject go = base.GetCurHandleObj() as GameObject;
            if (go == null)
            {
                return;
            }
            if (mSpliterType == ESpliterType.UIAtlas)
            {
                UIAtlas atlas = go.GetComponent<UIAtlas>();
                if (atlas != null)
                    prefabAtlas.Add(atlas);
            }
            else
            {
                UISprite[] sps = go.GetComponentsInChildren<UISprite>(true);
                for (int i = 0; i < sps.Length; i++)
                {
                    UISprite sp = sps[i];
                    if (sp == null || sp.atlas == null || sp.atlas.spriteMaterial == null || sp.atlas.spriteMaterial.mainTexture == null) continue;
                    if (!prefabAtlas.Contains(sp.atlas))
                    {
                        prefabAtlas.Add(sp.atlas);
                    }
                }
            }
        }

        static void DealAtlas(UnityEngine.Object obj)
        {
            GameObject go = obj as GameObject;
            if (go == null) return;
            UIAtlas atlas = go.GetComponent<UIAtlas>();
            Texture tex = atlas.spriteMaterial != null ?atlas.spriteMaterial.mainTexture:null;
            if (atlas != null&&tex!=null)
            {
                string srcAtlasPath = AssetDatabase.GetAssetPath(atlas);
                string srcDir = FileUtility.GetDirectory(srcAtlasPath);
                string srcMatPath = srcDir + "/Res/" + atlas.name + ".mat";

                string destAtlasPath = srcAtlasPath.Replace("Atlas", "SplitAtlas");
                string destDir = FileUtility.GetDirectory(destAtlasPath);
                string destTexAndMatPath = destDir + "/Res";
                string destMatPath = destTexAndMatPath + "/" + atlas.name + ".mat";
                string destTexAlphaPath = destTexAndMatPath + "/" + atlas.name +AlphaEndName+".png";
                string destTexRgbPath = destTexAndMatPath + "/" + atlas.name +RGBEndName+".png";

                FileUtility.DetectCreateDirectory(destAtlasPath);
                FileUtility.DetectCreateDirectory(destTexAndMatPath);
                
                AssetDatabase.DeleteAsset(destAtlasPath);
                AssetDatabase.CopyAsset(srcAtlasPath, destAtlasPath);
                AssetDatabase.ImportAsset(destAtlasPath);

                AssetDatabase.DeleteAsset(destMatPath);
                AssetDatabase.CopyAsset(srcMatPath, destMatPath);
                AssetDatabase.ImportAsset(destMatPath);
                //Debug.Log("destTexAndMatPath = " + destTexAndMatPath);
                SplitAlpha(tex, false, destTexAndMatPath);
                //Debug.Log("destTexAndMatPath = " + destTexAndMatPath);
                GameObject g = LoadAssetAtPath(destAtlasPath, typeof(GameObject)) as GameObject;
                UIAtlas newAtlas = g != null ? g.GetComponent<UIAtlas>() : null;
                Material newMat = LoadAssetAtPath(destMatPath, typeof(Material)) as Material;
                Shader newShader = Shader.Find("Mobile/LZF/ColorSet");
                newMat.shader = newShader;
                Texture a = LoadAssetAtPath(destTexAlphaPath, typeof(Texture)) as Texture;
                Texture rgb = LoadAssetAtPath(destTexRgbPath, typeof(Texture)) as Texture;
                newMat.SetTexture("_MainTex", rgb);
                newMat.SetTexture("_AlphaTex", a);
                newMat.SetInt("_IsAlphaSplit", 1);
                AssetDatabase.ImportAsset(destMatPath);
                newAtlas.spriteMaterial = newMat;
                newAtlas.MarkAsChanged();
            }
        }

        static UnityEngine.Object LoadAssetAtPath(string path,Type type)
        {
            path = path.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, type);
            return obj;
        }

        static void DealTexture(UnityEngine.Object obj)
        {
            Texture tex = obj as Texture;
            if (tex != null)
            {
                SplitAlpha(tex, false);
            }
        }

        public static void SplitAlpha(Texture src, bool alphaHalfSize, string saveAssetPath = "")
        {
            if (src == null)
                throw new ArgumentNullException("src");

            // make it readable
            string srcAssetPath = AssetDatabase.GetAssetPath(src);
            EditorUtil.IsDealingTexFormat = true;
            var importer = (TextureImporter)AssetImporter.GetAtPath(srcAssetPath);
            {
                importer.isReadable = true;
                importer.textureFormat = TextureImporterFormat.ARGB32;
            }
            AssetDatabase.ImportAsset(srcAssetPath);
            EditorUtil.SetTexReadable((Texture2D)src, true);
            Texture alpha = CreateAlphaTexture((Texture2D)src, alphaHalfSize, saveAssetPath);
            Texture rgb = CreateRGBTexture(src, saveAssetPath);
            EditorUtil.IsDealingTexFormat = false;
            EditorUtil.SetTexReadable((Texture2D)src, false);
        }

        static Texture CreateRGBTexture(Texture src, string saveAssetPath = "")
        {
            if (src == null)
                throw new ArgumentNullException("src");

            string srcPath = AssetDatabase.GetAssetPath(src);
            if (string.IsNullOrEmpty(saveAssetPath)) saveAssetPath = GetPath(src, RGBEndName);
            else saveAssetPath = saveAssetPath +"/"+ src.name+RGBEndName+".png";
            int size = Mathf.Max(src.width, src.height, 32);

            AssetDatabase.DeleteAsset(saveAssetPath);
            AssetDatabase.CopyAsset(srcPath, saveAssetPath);
            AssetDatabase.ImportAsset(saveAssetPath);

            SetSettings(saveAssetPath, size, TextureImporterFormat.ETC_RGB4, TextureImporterFormat.PVRTC_RGB4);

            return (Texture)AssetDatabase.LoadAssetAtPath(saveAssetPath, typeof(Texture));
        }

        static Texture CreateAlphaTexture(Texture2D src, bool alphaHalfSize, string saveAssetPath = "")
        {
            if (src == null)
                throw new ArgumentNullException("src");

            // create texture
            var srcPixels = src.GetPixels();
            var tarPixels = new Color[srcPixels.Length];
            for (int i = 0; i < srcPixels.Length; i++)
            {
                float r = srcPixels[i].a;
                tarPixels[i] = new Color(r, r, r);
            }

            Texture2D alphaTex = new Texture2D(src.width, src.height, TextureFormat.ARGB32, false);
            alphaTex.SetPixels(tarPixels);
            alphaTex.Apply();

            // save
            if (string.IsNullOrEmpty(saveAssetPath)) saveAssetPath = GetPath(src, AlphaEndName);
            else saveAssetPath = saveAssetPath + "/"+src.name+AlphaEndName+".png";
            string fullPath = Application.dataPath + "/../" + saveAssetPath;
            //Debug.LogError("fullPath = " + fullPath);
            var bytes = alphaTex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // setting
            int size = alphaHalfSize ? Mathf.Max(src.width / 2, src.height / 2, 32) : Mathf.Max(src.width, src.height, 32);
            SetSettings(saveAssetPath, size, TextureImporterFormat.ETC_RGB4, TextureImporterFormat.PVRTC_RGB4);

            return (Texture)AssetDatabase.LoadAssetAtPath(saveAssetPath, typeof(Texture));
        }

        static void SetSettings(string assetPath, int maxSize, TextureImporterFormat androidFormat, TextureImporterFormat iosFormat)
        {
            var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
            {
                importer.npotScale = TextureImporterNPOTScale.ToNearest;
                importer.isReadable = false;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = false;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Point;
                importer.anisoLevel = 4;
                importer.textureFormat = androidFormat;
                importer.SetPlatformTextureSettings("Android", maxSize, androidFormat, true);
                importer.SetPlatformTextureSettings("iPhone", maxSize, iosFormat, true);
                //importer.SetPlatformTextureSettings("Standalone", maxSize, androidFormat, 100);
            }
            AssetDatabase.ImportAsset(assetPath);
        }

        static string GetPath(Texture src, string endName)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            string srcAssetPath = AssetDatabase.GetAssetPath(src);
            if (string.IsNullOrEmpty(srcAssetPath))
                return null;

            string dirPath = Path.GetDirectoryName(srcAssetPath);
            string ext = Path.GetExtension(srcAssetPath);
            string fileName = Path.GetFileNameWithoutExtension(srcAssetPath);

            if (fileName.EndsWith(RGBEndName))
                fileName = fileName.Substring(0, fileName.Length - RGBEndName.Length);

            if (fileName.EndsWith(AlphaEndName))
                fileName = fileName.Substring(0, fileName.Length - AlphaEndName.Length);

            return string.Format("{0}/{1}{2}{3}", dirPath, fileName, endName ?? "", ext);
        }

        public static Texture GetRGBA(Texture src)
        {
            if (src != null && (s_rgba == null || s_rgba.name != src.name))
            {
                string path = GetPath(src, "");
                if (!string.IsNullOrEmpty(path))
                    s_rgba = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
            }

            return s_rgba;
        }
    }
}