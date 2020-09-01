using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ExtendEditor
{
    public class AssetBundlePreview : SelectionBase
    {
        public class Data
        {
            public string path;
            public UIAtlas atlas;
            public AssetBundle assetbundle;
        }
        [MenuItem("Tools/AssetBundle/AssetBundlePreview")]
        public static void AssetBundlePreviewProc()
        {
            EditorWindow win = GetWindow(typeof(AssetBundlePreview));
            win.Show();
        }

        string curShowPath = string.Empty;
        UIAtlas curShowAtlas = null;
        Texture curShowPicture = null;
        UnityWebRequest curWWW = null;
        Dictionary<string, Data> hasLoadedDic = new Dictionary<string, Data>();
        int frame = 20;
        float showTime
        {
            get
            {
                return 1.0f / frame;
            }
        }
        double nextPlayTime = 0;
        int curPlayIndex = 0;
        public override void OnGUI()
        {
            base.OnGUI();
            string path = mPathList.Count > 0 ? mPathList[0] : curShowPath;
            if (curShowPath != path)
            {
                curPlayIndex = 0;
                curShowAtlas = null;
                curShowPicture = null;
                curShowPath = path;
                FNDebug.Log(curShowPath);
                if (hasLoadedDic.ContainsKey(curShowPath))
                {
                    curShowAtlas = hasLoadedDic[curShowPath].atlas;
                    nextPlayTime = EditorApplication.timeSinceStartup + showTime;
                }
                else
                {
                    string loadingPath = "file://"+Application.dataPath.Replace("Assets", "") + curShowPath;
                    FNDebug.Log("loadingPath = " + loadingPath);
                    curWWW = UnityWebRequestAssetBundle.GetAssetBundle(loadingPath);
                    curWWW.Send();
                }
            }
            if (curShowAtlas != null)
            {
                DrawAtlas(curShowAtlas);
            }

            if (curShowPicture != null)
            {
                DrawPic(curShowPicture);
            }
        }

        void DrawPic(Texture tex)
        {
            EditorGUILayout.ObjectField(tex, typeof(Texture));

            GUI.DrawTexture(new Rect(100, 150, tex.width, tex.height),tex);
        }

        void DrawAtlas(UIAtlas atlas)
        {
            if (atlas == null) return;

            EditorGUILayout.ObjectField(atlas, typeof(UIAtlas));

            if(curPlayIndex<0||curPlayIndex>=atlas.spriteList.Count)return;
            UISpriteData data = atlas.spriteList[curPlayIndex];
            Vector2 center = new Vector2(300,300);
            float originalWidth = data.paddingLeft + data.paddingRight + data.width;
            float originalHeight = data.paddingTop + data.paddingBottom + data.height;
            float xRatio = data.x * 1.0f / atlas.spriteMaterial.mainTexture.width;
            float yRatio = 1 - (data.y+data.height) * 1.0f / atlas.spriteMaterial.mainTexture.height;
            float wRatio = data.width * 1.0f / atlas.spriteMaterial.mainTexture.width;
            float hRatio = data.height * 1.0f / atlas.spriteMaterial.mainTexture.height;
            float left = 100 - (originalWidth * 0.5f - data.paddingLeft);
            float top = 150 - (originalHeight * 0.5f - data.paddingTop);
            //GUI.DrawTextureWithTexCoords(new Rect(100, 100, data.width, data.height), atlas.spriteMaterial.mainTexture, new Rect(xRatio, yRatio, wRatio, hRatio));
            GUI.DrawTextureWithTexCoords(new Rect(left, top, data.width, data.height),
                atlas.spriteMaterial.mainTexture, new Rect(xRatio, yRatio, wRatio, hRatio));


            if (EditorApplication.timeSinceStartup >= nextPlayTime)
            {
                nextPlayTime = EditorApplication.timeSinceStartup + showTime;
                curPlayIndex++;
                curPlayIndex = curPlayIndex % atlas.spriteList.Count;
            }
        }

        public override void OnInspectorUpdate()
        {
            base.OnInspectorUpdate();
            if (curWWW != null && curWWW.isDone)
            {
                FNDebug.Log("www load");
                if (curWWW.isNetworkError)
                {
                    UnityEngine.Debug.LogError(curWWW.error);
                    return;
                }
                try
                {
                    AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(curWWW);
                    string[] strs = assetBundle.GetAllAssetNames();
                    if (strs.Length > 0)
                    {
                        UnityEngine.Object obj = assetBundle.LoadAsset(strs[0]);
                        GameObject go = obj as GameObject;
                        if (go != null)
                        {
                            UIAtlas atlas = go.GetComponent<UIAtlas>();
                            if (atlas != null)
                            {
                                curShowAtlas = atlas;
                                FNDebug.Log("curShowAtlas  = " + curShowAtlas.name);
                                nextPlayTime = EditorApplication.timeSinceStartup + showTime;
                                if (!hasLoadedDic.ContainsKey(curShowPath))
                                {
                                    Data data = new Data();
                                    data.path = curShowPath;
                                    data.atlas = atlas;
                                    data.assetbundle = assetBundle;
                                    hasLoadedDic.Add(curShowPath, data);
                                }
                            }
                        }
                        Texture tex = obj as Texture;
                        if (tex != null)
                        {
                            curShowPicture = tex;
                        }
                    }
                    assetBundle.Unload(false);
                }
                catch (System.Exception ex)
                {
                	
                }
                
                curWWW.Dispose();
                curWWW = null;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            var cur = hasLoadedDic.GetEnumerator();
            while (cur.MoveNext())
            {
                Data data = cur.Current.Value;
                if (data.assetbundle != null)
                {
                    data.assetbundle.Unload(true);
                }
            }
            hasLoadedDic.Clear();
        }
    }

}