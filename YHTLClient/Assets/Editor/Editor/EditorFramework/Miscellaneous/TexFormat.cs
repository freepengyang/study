using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace ExtendEditor
{
    /// <summary>
    /// 注意UITexturePostProcessor可能有其他操作
    /// </summary>
    public class TexFormat :  SelectionBase
    {
        bool mIsReadable = false;
        bool mCanReadable = false;

        TextureImporterType mTextureType = TextureImporterType.Default;
        bool mCanTextureType = false;

        TextureImporterNPOTScale mNpotScale = TextureImporterNPOTScale.None;
        bool mCanTextureImporterNPOTScale = false;

        int mAnisoLevel = 0;
        bool mCanAisoLevel = false;

        bool mMipmapEnabled = false;
        bool mCanMipMapEnbled = false;

        bool mAlphaIsTransparency = true;
        bool mCanAlphaIsTransparency = false;

        TextureWrapMode mWrapMode = TextureWrapMode.Clamp;
        bool mCanWrapMode = false;

        FilterMode mFilterMode = FilterMode.Point;
        bool mCanFilterMode = false;

        TextureImporterFormat mTextureFormat = TextureImporterFormat.AutomaticCompressed;
        bool mCanTextureFormat = false;

        int mMaxTextureSize = 4096;
        bool mCanMaxTextureSize = false;

        bool mIsDealAtlasTexture = false;
        [MenuItem("Tools/Miscellaneous/TexFormat")]
        public static void RenameProc()
        {
            EditorWindow win = GetWindow(typeof(TexFormat));
            win.Show();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.BeginHorizontal();
            mCanReadable = EditorGUILayout.Toggle("Change", mCanReadable);
            mIsReadable = EditorGUILayout.Toggle("mIsReadable", mIsReadable);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanTextureType = EditorGUILayout.Toggle("Change", mCanTextureType);
            mTextureType = (TextureImporterType)EditorGUILayout.EnumPopup("mTextureType", mTextureType);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanTextureImporterNPOTScale = EditorGUILayout.Toggle("Change", mCanTextureImporterNPOTScale);
            mNpotScale = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup("mNpotScale", mNpotScale);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanAisoLevel = EditorGUILayout.Toggle("Change", mCanAisoLevel);
            mAnisoLevel = EditorGUILayout.IntField("mAnisoLevel", mAnisoLevel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanMipMapEnbled = EditorGUILayout.Toggle("Change", mCanMipMapEnbled);
            mMipmapEnabled = EditorGUILayout.Toggle("mMipmapEnabled", mMipmapEnabled);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanAlphaIsTransparency = EditorGUILayout.Toggle("Change", mCanAlphaIsTransparency);
            mAlphaIsTransparency = EditorGUILayout.Toggle("mAlphaIsTransparency", mAlphaIsTransparency);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanWrapMode = EditorGUILayout.Toggle("Change", mCanWrapMode);
            mWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("mWrapMode", mWrapMode);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanFilterMode = EditorGUILayout.Toggle("Change", mCanFilterMode);
            mFilterMode = (FilterMode)EditorGUILayout.EnumPopup("mFilterMode", mFilterMode);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanTextureFormat = EditorGUILayout.Toggle("Change", mCanTextureFormat);
            mTextureFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("mTextureFormat", mTextureFormat);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            mCanMaxTextureSize = EditorGUILayout.Toggle("Change", mCanMaxTextureSize);
            mMaxTextureSize = EditorGUILayout.IntField("mMaxTextureSize", mMaxTextureSize);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Deal"))
            {
                mIsDealAtlasTexture = false;
                base.BeginHandle();
            }

            if (GUILayout.Button("Deal Atlas Texture"))
            {
                mIsDealAtlasTexture = true;
                base.BeginHandle();
            }

            if (base.CanHandle())
            {
                EditorUtil.IsDealingTexFormat = true;
                int index = 0;
                while (index < 1&&base.CanHandle())
                {
                    string path = "";
                    path = base.GetCurHandlePath();
                    if (mIsDealAtlasTexture)
                    {
                        Object obj = base.GetCurHandleObj();
                        GameObject go = obj as GameObject;
                        if (go == null)
                        {
                            base.MoveHandle();
                            continue;
                        }
                        string temp = GetUIPrefabTexPath(path);
                        if (!string.IsNullOrEmpty(temp)) path = temp;
                    }
                    else
                    {
                        path = base.GetCurHandlePath();
                    }
                    
                    TextureImporter importer = TextureImporter.GetAtPath(path) as TextureImporter;
                    if (importer != null)
                    {
                        bool isChange = false;
                        if (mCanReadable && importer.isReadable != mIsReadable)
                        {
                            isChange = true;
                            importer.isReadable = mIsReadable;
                        }
                        if (mCanTextureType && importer.textureType != mTextureType)
                        {
                            isChange = true;
                            importer.textureType = mTextureType;
                        }
                        if (mCanTextureImporterNPOTScale && importer.npotScale != mNpotScale)
                        {
                            isChange = true;
                            importer.npotScale = mNpotScale;
                        }
                        if (mCanAisoLevel && importer.anisoLevel != mAnisoLevel) 
                        {
                            isChange = true;
                            importer.anisoLevel = mAnisoLevel;
                        }
                        if (mCanMipMapEnbled && importer.mipmapEnabled != mMipmapEnabled)
                        {
                            isChange = true;
                            importer.mipmapEnabled = mMipmapEnabled;
                        }
                        if (mCanAlphaIsTransparency && importer.alphaIsTransparency != mAlphaIsTransparency)
                        {
                            isChange = true;
                            importer.alphaIsTransparency = mAlphaIsTransparency;
                        }
                        if (mCanWrapMode && importer.wrapMode != mWrapMode)
                        {
                            isChange = true;
                            importer.wrapMode = mWrapMode;
                        }
                        if (mCanFilterMode && importer.filterMode != mFilterMode)
                        {
                            isChange = true;
                            importer.filterMode = mFilterMode;
                        }

                        if (mCanTextureFormat && importer.textureFormat != mTextureFormat)
                        {
                            isChange = true;
                            importer.textureFormat = mTextureFormat;
                        }
                        if (mCanMaxTextureSize && importer.maxTextureSize != mMaxTextureSize)
                        {
                            isChange = true;
                            importer.maxTextureSize = mMaxTextureSize;
                        }
                        if (isChange)
                        {
                            importer.spriteImportMode = SpriteImportMode.None;
                            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                        }  
                    }
                    
                    base.MoveHandle();
                    index++;
                }
            }
            else
            {
                EditorUtil.IsDealingTexFormat = false;
            }
            //AssetDatabase.Refresh();
        }

        string GetUIPrefabTexPath(string prefabPath)
        {
            GameObject go = FileUtility.GetObject(prefabPath) as GameObject;
            UIAtlas atlas = go.GetComponent<UIAtlas>();
            if (atlas == null) return "";
            if (atlas != null && atlas.spriteMaterial != null && atlas.spriteMaterial.mainTexture!= null)
            {
                string path = AssetDatabase.GetAssetPath(atlas.spriteMaterial.mainTexture);
                return path;
            }
            else
            {
                FNDebug.LogError("Atlas 或者 mat 或者 texture为空 = " + prefabPath);
                return "";
            }
        }
    }
}

