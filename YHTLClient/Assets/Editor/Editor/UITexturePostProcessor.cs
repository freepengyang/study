    
//-------------------------------------------------------------------------
//UITexturePostProcessor
//Author LiZongFu
//Time 2015.1.21
//-------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using ExtendEditor;
using System.Collections.Generic;

public class UITexturePostProcessor : AssetPostprocessor
{
    TextureImporterPlatformSettings texSetting_Android = new TextureImporterPlatformSettings();

    TextureImporterPlatformSettings texSetting_IOS = new TextureImporterPlatformSettings();


    void OnPreprocessTexture()
    {
        if (EditorUtil.IsDealingTexFormat) return;

        TextureImporter importer = assetImporter as TextureImporter;

        if (assetPath.Contains("UITexture"))
        {
            importer.isReadable = true;
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.spriteImportMode = SpriteImportMode.None;

            texSetting_Android.name = "Android";
            texSetting_Android.maxTextureSize = 2048;
            texSetting_Android.format = TextureImporterFormat.RGBA32;
            texSetting_Android.allowsAlphaSplitting = true;
            texSetting_Android.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_Android);

            texSetting_IOS.name = "iPhone";
            texSetting_IOS.maxTextureSize = 2048;
            texSetting_IOS.format = TextureImporterFormat.RGBA32;
            texSetting_IOS.allowsAlphaSplitting = true;
            texSetting_IOS.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_IOS);
        }
        else if (assetPath.Contains("texture"))
        {
            importer.isReadable = true;
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.spriteImportMode = SpriteImportMode.None;

            texSetting_Android.name = "Android";
            texSetting_Android.maxTextureSize = 2048;
            texSetting_Android.format = TextureImporterFormat.RGBA32;
            texSetting_Android.allowsAlphaSplitting = true;
            texSetting_Android.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_Android);

            texSetting_IOS.name = "iPhone";
            texSetting_IOS.maxTextureSize = 2048;
            texSetting_IOS.format = TextureImporterFormat.RGBA32;
            texSetting_IOS.allowsAlphaSplitting = true;
            texSetting_IOS.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_IOS);
        }
        else if (assetPath.Contains("UIAsset/chart"))
        {
            importer.isReadable = true;
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.spriteImportMode = SpriteImportMode.None;

            texSetting_Android.name = "Android";
            texSetting_Android.maxTextureSize = 2048;
            texSetting_Android.format = TextureImporterFormat.RGBA32;
            texSetting_Android.allowsAlphaSplitting = true;
            texSetting_Android.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_Android);

            texSetting_IOS.name = "iPhone";
            texSetting_IOS.maxTextureSize = 2048;
            texSetting_IOS.format = TextureImporterFormat.RGBA32;
            texSetting_IOS.allowsAlphaSplitting = true;
            texSetting_IOS.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_IOS);
        }
        else if (assetPath.Contains("UIAsset/Font"))
        {
            importer.isReadable = true;
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.spriteImportMode = SpriteImportMode.None;
            importer.filterMode = FilterMode.Point;

            texSetting_Android.name = "Android";
            texSetting_Android.maxTextureSize = 2048;
            texSetting_Android.format = TextureImporterFormat.RGBA32;
            texSetting_Android.allowsAlphaSplitting = true;
            texSetting_Android.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_Android);

            texSetting_IOS.name = "iPhone";
            texSetting_IOS.maxTextureSize = 2048;
            texSetting_IOS.format = TextureImporterFormat.RGBA32;
            texSetting_IOS.allowsAlphaSplitting = true;
            texSetting_IOS.overridden = true;
            importer.SetPlatformTextureSettings(texSetting_IOS);
        }
        else if (assetPath.Contains("AssetBundleRes"))
		{
			importer.textureType = TextureImporterType.Default;
			importer.npotScale = TextureImporterNPOTScale.None;
			importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
			importer.alphaIsTransparency = false;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.spriteImportMode = SpriteImportMode.None;

            if (assetPath.Contains("Model") && !assetPath.Contains("NotDealFormat"))
			{
				importer.filterMode = FilterMode.Point;
                importer.isReadable = false;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.overridden = true;
                texSetting_Android.format = TextureImporterFormat.RGBA16;
                texSetting_Android.allowsAlphaSplitting = true;

                importer.SetPlatformTextureSettings(texSetting_Android);

                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);
            }
			else if (assetPath.Contains("AssetBundleRes/MiniMap"))
			{
				importer.filterMode = FilterMode.Point;
				importer.isReadable = false;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.format = TextureImporterFormat.RGB16;
                texSetting_Android.allowsAlphaSplitting = true;
                texSetting_Android.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_Android);

                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);
            }
            else if (assetPath.Contains("AssetBundleRes/Skill"))
            {
                importer.filterMode = FilterMode.Point;
                importer.isReadable = false;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.format = TextureImporterFormat.RGB16;
                texSetting_Android.allowsAlphaSplitting = true;
                texSetting_Android.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_Android);

                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);
            }
            else if (assetPath.Contains("AssetBundleRes/ScaleMap"))
            {
                importer.filterMode = FilterMode.Point;
                importer.isReadable = false;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.format = TextureImporterFormat.ETC2_RGB4;
                texSetting_Android.allowsAlphaSplitting = true;
                texSetting_Android.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_Android);
              
                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);

            }
            else if (assetPath.Contains("AssetBundleRes/Map"))
            {
                importer.filterMode = FilterMode.Point;
                importer.isReadable = false;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.format = TextureImporterFormat.ETC_RGB4;
                texSetting_Android.allowsAlphaSplitting = true;
                texSetting_Android.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_Android);

                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);

            }
            else if(assetPath.Contains("AssetBundleRes/UIEffect"))
            {

            }
            else
            {
                importer.filterMode = FilterMode.Point;
                importer.isReadable = true;

                texSetting_Android.name = "Android";
                texSetting_Android.maxTextureSize = 4096;
                texSetting_Android.format = TextureImporterFormat.ETC_RGB4;
                texSetting_Android.allowsAlphaSplitting = true;
                texSetting_Android.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_Android);

                texSetting_IOS.name = "iPhone";
                texSetting_IOS.maxTextureSize = 4096;
                texSetting_IOS.format = TextureImporterFormat.RGB16;
                texSetting_IOS.allowsAlphaSplitting = true;
                texSetting_IOS.overridden = true;
                importer.SetPlatformTextureSettings(texSetting_IOS);
            }
		}
    }
}