using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace ExtendEditor
{
    public class TexturePalette : SelectionBase
    {
        public int paletteNum = 18;
        public string paletteTexPath = "Assets/Resources/Test.png";
        [MenuItem("Tools/Miscellaneous/TexturePalette")]
        public static void TexturePaletteProc()
        {
            EditorWindow win = GetWindow(typeof(TexturePalette));
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (GUILayout.Button("CreatePalette"))
            {
                //CreatePalette();
                CreatePalette2();
            }

            if (GUILayout.Button("CreatePaletteIndex"))
            {
                base.BeginHandle();
            }

            if (base.CanHandle())
            {
                try
                {
                    CreateImgPalette();
                }
                catch (System.Exception ex)
                {
                	FNDebug.LogError(ex);
                }
                
                base.MoveHandle();
            }
        }

        void CreatePalette()
        {
            int paletteColorNum = 1 << paletteNum;
            int perColorNum_a = 1<<(paletteNum / 3);
            int perColorNum_g = 1<<(paletteNum / 3);
            int perColorNum_b = 1<<(paletteNum / 3);
            int left = paletteNum % 3;

            if (left == 1)
            {
                perColorNum_a = perColorNum_a << 1;
            }
            else if (left == 2)
            {
                perColorNum_a = perColorNum_a << 1;
                perColorNum_g = perColorNum_g << 1;
            }

            int step_a = 256 / perColorNum_a;
            int step_g = 256 / perColorNum_g;
            int step_b = 256 / perColorNum_b;

            var tarPixels = new Color[paletteColorNum];
            int index = 0;
            for (int i = 0; i < perColorNum_a; i++)
            {
                int r = i * step_a;
                for (int j = 0; j < perColorNum_g; j++)
                {
                    int g = j * step_g;
                    for (int t = 0; t < perColorNum_b; t++)
                    {
                        int b = t * step_b;
                        tarPixels[index++] = new Color(r/255.0f, g/255.0f, b/255.0f);
                    }
                }
            }
            int width = 1 << (paletteNum / 2);
            int height = 1 << (paletteNum / 2+paletteNum%2);
            Texture2D alphaTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            alphaTex.SetPixels(tarPixels);
            alphaTex.Apply();

            AssetDatabase.DeleteAsset(paletteTexPath);

            string fullPath = paletteTexPath;
            var bytes = alphaTex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
            int size = Mathf.Max(width, height, 32);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetSettings(paletteTexPath, size, TextureImporterFormat.AutomaticTruecolor, TextureImporterFormat.AutomaticTruecolor);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void CreatePalette2()
        {
            base.BeginHandle();
            int paletteColorNum = 1 << paletteNum;

            var tarPixels = new Color[256*256];

            Texture2D tex = base.GetCurHandleObj() as Texture2D;
            var srcPixels = tex.GetPixels();
            int index = 0;
            for (int i = 0; i < srcPixels.Length; i++)
            {
                bool isFind = false;
                for (int j = 0; j < index; j++)
                {
                    if (tarPixels[j].r == srcPixels[i].r && tarPixels[j].g == srcPixels[i].g && tarPixels[j].b == srcPixels[i].b)
                    {
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    if (index >= tarPixels.Length)
                    {
                        FNDebug.LogError("颜色值太多了，调色板存不下");
                    }
                    else
                    {
                        tarPixels[index] = new Color(srcPixels[i].r, srcPixels[i].g, srcPixels[i].b);
                        index++;
                    }
                }
            }
            FNDebug.LogError("index = " + index);

            Texture2D alphaTex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            alphaTex.SetPixels(tarPixels);
            alphaTex.Apply();

            AssetDatabase.DeleteAsset(paletteTexPath);

            string fullPath = paletteTexPath;
            var bytes = alphaTex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
            int size = Mathf.Max(256, 256, 32);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetSettings(paletteTexPath, size, TextureImporterFormat.AutomaticTruecolor, TextureImporterFormat.AutomaticTruecolor);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            base.End();
        }

        void CreateImgPalette()
        {
            EditorUtil.IsDealingTexFormat = true;
            var importer = (TextureImporter)AssetImporter.GetAtPath(paletteTexPath);
            importer.isReadable = true;
            AssetDatabase.ImportAsset(paletteTexPath);

            importer = (TextureImporter)AssetImporter.GetAtPath(base.GetCurHandlePath());
            importer.isReadable = true;
            AssetDatabase.ImportAsset(base.GetCurHandlePath());
           
            Texture2D paletteTex = AssetDatabase.LoadAssetAtPath(paletteTexPath, typeof(Texture2D)) as Texture2D;
            Texture2D tex = base.GetCurHandleObj() as Texture2D;
            var srcPixels = tex.GetPixels();
            var palettePixels = paletteTex.GetPixels();
            var tarPixels = new Color[srcPixels.Length];
            int width = tex.width;
            int height = tex.height;

            int paletteColorNum = 1 << paletteNum;
            int perColorNum_a = 1 << (paletteNum / 3);
            int perColorNum_g = 1 << (paletteNum / 3);
            int perColorNum_b = 1 << (paletteNum / 3);
            int left = paletteNum % 3;

            if (left == 1)
            {
                perColorNum_a = perColorNum_a << 1;
            }
            else if (left == 2)
            {
                perColorNum_a = perColorNum_a << 1;
                perColorNum_g = perColorNum_g << 1;
            }

            int step_a = 256 / perColorNum_a;
            int step_g = 256 / perColorNum_g;
            int step_b = 256 / perColorNum_b;
             List<int> testList = new List<int>();
            for (int i = 0; i < srcPixels.Length; i++)
            {
                //int r = (int)(srcPixels[i].r * 255);
                //int g = (int)(srcPixels[i].g * 255);
                //int b = (int)(srcPixels[i].b * 255);

                //int index_r = Mathf.RoundToInt(r * 1.0f / step_a);
                //int index_g = Mathf.RoundToInt(g * 1.0f / step_g);
                //int index_b = Mathf.RoundToInt(b * 1.0f / step_b);
                //int index = index_r * perColorNum_g * perColorNum_b + index_g * perColorNum_b + index_b;
                //int widthIndex = index % paletteTex.width;
                //int heightIndex = index / paletteTex.height;
                //Color paletteColor = paletteTex.GetPixel(widthIndex, heightIndex);

                //if (r != 0 || g != 0 || b != 0)
                //{
                //    string str = "";
                //}

                //float min = float.MaxValue;
                int widthIndex = -1;
                int heightIndex = -1;
                bool isFind = false;
                for (int j = 0; j < palettePixels.Length; j++)
                {
                    int r = (int)(srcPixels[i].r * 255);
                    int g = (int)(srcPixels[i].g * 255);
                    int b = (int)(srcPixels[i].b * 255);
                    int pr = (int)(palettePixels[j].r * 255);
                    int pg = (int)(palettePixels[j].g * 255);
                    int pb = (int)(palettePixels[j].b * 255);
                    if (r == pr && g == pg && b == pb)
                    {
                        widthIndex = j % paletteTex.width;
                        heightIndex = j / paletteTex.width;
                        if (!testList.Contains(j))
                            testList.Add(j);
                        //Debug.Log("widthIndex = " + widthIndex + " heightIndex" + heightIndex);
                        isFind = true;
                        break;
                    }
                }
                
                if (!isFind)
                {
                    FNDebug.LogError("Not Find = " + srcPixels[i]);
                }
                float u = widthIndex * 1.0f / paletteTex.width + 0.5f / paletteTex.width;
                float v = heightIndex * 1.0f / paletteTex.height + 0.5f / paletteTex.height;
                tarPixels[i] = new Color(u, v, 0);
            }
            //for (int i = 0; i < testList.Count; i++)
            //{
            //    int widthIndex = testList[i] % paletteTex.width;
            //    int heightIndex = testList[i] / paletteTex.width;
            //    Debug.LogError("widthIndex = " + widthIndex + " heightIndex=" + heightIndex);
            //}
            //return;
            Texture2D alphaTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            alphaTex.SetPixels(tarPixels);
            alphaTex.Apply();
            string fullPath = FileUtility.GetAssetFullPath(base.GetCurHandlePath());
            fullPath = FileUtility.GetDirectory(fullPath)+"/" + tex.name + "(rgb).png";

            string assetPath = FileUtility.GetAssetPath(fullPath);
            AssetDatabase.DeleteAsset(assetPath);

            var bytes = alphaTex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
            int size = Mathf.Max(width, height, 32);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetSettings(assetPath, size, TextureImporterFormat.AutomaticTruecolor, TextureImporterFormat.AutomaticTruecolor);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtil.IsDealingTexFormat = false;
        }

        static void SetSettings(string assetPath, int maxSize, TextureImporterFormat androidFormat, TextureImporterFormat iosFormat)
        {
            var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
            {
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.isReadable = false;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = false;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Bilinear;
                importer.anisoLevel = 4;
                importer.textureFormat = androidFormat;
                importer.SetPlatformTextureSettings("Android", maxSize, androidFormat, true);
                importer.SetPlatformTextureSettings("iPhone", maxSize, iosFormat, true);
                //importer.SetPlatformTextureSettings("Standalone", maxSize, androidFormat, 100);
            }
            AssetDatabase.ImportAsset(assetPath);
        }
    }
}
