using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UDebug = UnityEngine.Debug;

public class TexturePacker : EditorWindow
{
    [MenuItem("Tools/Open TexturePacker Window")]
    public static void OpenWindown()
    {
        GetWindow<TexturePacker>("TexturePacker Window", true);
    }

    string[] toolbarTitles = new string[] { "更新现有Atlas", "创建新Atlas", "批量创建新Atlas", "将Atlas里的sprite导出成原图" };
    int toolbarIndex = 0;

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarTitles, GUILayout.Height(30));
        }
        GUILayout.EndHorizontal();
        NGUIEditorTools.DrawSeparator();
        if (toolbarIndex == 0)
        {
            UpdateAtlasPanel();
        }
        else if (toolbarIndex == 1)
        {
            SinglePackTexture();
        }
        else if (toolbarIndex == 2)
        {
            BatchPackTexture();
        }
        else if (toolbarIndex == 3)
        {
            ExportSpritePanel();
        }
    }

    string resPath = "";
    string atlasPath = "";
    string newCheckPath = "";
    string oldCheckPath = "";
    void SinglePackTexture()
    {
        GUILayout.BeginVertical();
        {
            resPath = PathField(resPath, "图片资源路径", "设置为当前选择的路径", 100, 150);
            atlasPath = PathField(atlasPath, "保存图片路径", "设置为当前选择的路径", 100, 150);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space((Screen.width - 150) / 2);
                if (GUILayout.Button("自动生成Atlas", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    string defaultPath = Directory.GetCurrentDirectory();
                    string curProjPath = defaultPath.Replace("\\", "/");

                    DirectoryInfo dirInfo = new DirectoryInfo(curProjPath + "/" + resPath);
                    if (dirInfo.Exists)
                    {
                        PackTexture(dirInfo.Name, curProjPath, atlasPath, resPath.Replace("/" + dirInfo.Name, ""));
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.Space(6);
        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(6);

        GUILayout.BeginVertical();
        {
            newCheckPath = PathField(newCheckPath, "新Atlas文件", "设置为当前选择的Atlas", 100, 150);
            oldCheckPath = PathField(oldCheckPath, "旧Atlas文件", "设置为当前选择的Atlas", 100, 150);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("检查Atlas", GUILayout.Height(50)))
                {
                    CheckAtlas(newCheckPath, oldCheckPath);
                }

                if (GUILayout.Button("copy旧Atlas属性到新Atlas", GUILayout.Height(50)))
                {
                    CopyAtlas(newCheckPath, oldCheckPath);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    string PathField(string path, string labelName, string buttonName, int labelWidth, int buttonWidth)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(labelName, GUILayout.Width(labelWidth));
            path = GUILayout.TextField(path);
            if (GUILayout.Button(buttonName, GUILayout.Width(buttonWidth)))
            {
                if (Selection.activeObject != null)
                {
                    path = AssetDatabase.GetAssetPath(Selection.activeObject);
                }
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(6);
        return path;
    }

    string batchResPath = "";
    string batchAtlasPath = "";
    string batchNewCheckPath = "";
    string batchOldCheckPath = "";
    private void BatchPackTexture()
    {
        GUILayout.BeginVertical();
        {
            batchResPath = PathField(batchResPath, "图片资源路径", "设置为当前选择的路径", 100, 150);
            batchAtlasPath = PathField(batchAtlasPath, "保存图片路径", "设置为当前选择的路径", 100, 150);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space((Screen.width - 150) / 2);
                if (GUILayout.Button("批量生成Atlas", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    string defaultPath = Directory.GetCurrentDirectory();
                    string curProjPath = defaultPath.Replace("\\", "/")/* + "/" + resPath*/;
                    string[] subDirs = Directory.GetDirectories(curProjPath + "/" + batchResPath);

                    for (int i = 0; i < subDirs.Length; i++)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(subDirs[i]);
                        if (dirInfo.Exists && dirInfo.Name != ".vs")
                        {
                            PackTexture(dirInfo.Name, curProjPath, batchAtlasPath, batchResPath);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.Space(6);
        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(6);

        GUILayout.BeginVertical();
        {
            batchNewCheckPath = PathField(batchNewCheckPath, "新Atlas路径", "设置为当前选择的路径", 100, 150);
            batchOldCheckPath = PathField(batchOldCheckPath, "旧Atlas路径", "设置为当前选择的路径", 100, 150);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("批量检查Atlas", GUILayout.Height(50)))
                {
                    string folderPath = Directory.GetCurrentDirectory().Replace("\\", "/");
                    string[] newSubFile = Directory.GetFiles(folderPath + "/" + batchNewCheckPath, "*.prefab");

                    for (int i = 0; i < newSubFile.Length; i++)
                    {
                        FileInfo newFileInfo = new FileInfo(newSubFile[i]);
                        CheckAtlas(batchNewCheckPath + "/" + newFileInfo.Name, batchOldCheckPath + "/" + newFileInfo.Name);
                    }
                }
                if (GUILayout.Button("批量copy旧Atlas属性到新Atlas", GUILayout.Height(50)))
                {
                    string folderPath = Directory.GetCurrentDirectory().Replace("\\", "/");
                    string[] newSubFile = Directory.GetFiles(folderPath + "/" + batchNewCheckPath, "*.prefab");

                    for (int i = 0; i < newSubFile.Length; i++)
                    {
                        FileInfo newFileInfo = new FileInfo(newSubFile[i]);
                        CopyAtlas(batchNewCheckPath + "/" + newFileInfo.Name, batchOldCheckPath + "/" + newFileInfo.Name);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void PackTexture(string atlasname, string curProjPath, string _atlasPath, string _resPath)
    {
        try
        {

            if (!Directory.Exists(curProjPath + "/" + _resPath + "/" + atlasname))
            {
                return;
            }

            string procArguments = string.Format("--size-constraints POT --algorithm Basic --data {1}/{2}/{0}.txt --format unity --sheet {1}/{2}/{0}.png {1}/{3}/{0}", atlasname, curProjPath, _atlasPath, _resPath);
            CallTexturePackerProcess(procArguments);

            AssetDatabase.Refresh();

            string matPath = _atlasPath + "/" + atlasname + ".mat";
            string prefabPath = _atlasPath + "/" + atlasname + ".prefab";
            string texturePath = _atlasPath + "/" + atlasname + ".png";
            string jsonPath = _atlasPath + "/" + atlasname + ".txt";

            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
            TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (textureImporter != null && texture != null)
            {
                SetTextureImporter(texturePath, texture, textureImporter);
            }
            else
            {
                AssetDatabase.Refresh();
                return;
            }

            GameObject go = CreateNewEmptyPrefab(atlasname, prefabPath);

            Material mat = CreateNewMaterial(matPath);
            mat.mainTexture = texture;

            UIAtlas atlas = go.AddComponent<UIAtlas>();
            atlas.spriteMaterial = mat;

            ImportTexturePackerJson(jsonPath, atlas);

            atlas.MarkAsChanged();
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

    private static void CallTexturePackerProcess(string procArguments)
    {
        Process proc = new Process();
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.FileName = "TexturePacker.exe";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.Arguments = procArguments;
        proc.Start();
        proc.WaitForExit();

        string errorStr = proc.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(errorStr))
        {
            UDebug.LogError(errorStr);
        }
        else
        {
            UDebug.Log(proc.StandardOutput.ReadToEnd());
        }

        proc.Close();
    }

    private static void ImportTexturePackerJson(string jsonPath, UIAtlas atlas)
    {
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
        NGUIJson.LoadSpriteData(atlas, ta);
        AssetDatabase.DeleteAsset(jsonPath);
    }

    private static Material CreateNewMaterial(string matPath)
    {
        Material mat = new Material(Shader.Find("Unlit/Transparent Colored"));
        AssetDatabase.CreateAsset(mat, matPath);
        mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        return mat;
    }

    private static GameObject CreateNewEmptyPrefab(string atlasname, string prefabPath)
    {
        GameObject go = new GameObject(atlasname);
        PrefabUtility.CreatePrefab(prefabPath, go);
        DestroyImmediate(go);
        go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return go;
    }

    void CheckAtlas(string newAtlasPath, string oldAtlasPath)
    {
        UDebug.LogFormat("检查图集{0}和{1}", newAtlasPath, oldAtlasPath);
        GameObject newAtlasGo = AssetDatabase.LoadAssetAtPath<GameObject>(newAtlasPath);
        if (newAtlasGo == null)
        {
            UDebug.LogErrorFormat("没有新Atlas：{0}", newAtlasPath);
            return;
        }

        UIAtlas newAtlas = newAtlasGo.GetComponent<UIAtlas>();
        if (newAtlas == null)
        {
            UDebug.LogErrorFormat("没有新Atlas：{0}", newAtlasPath);
            return;
        }

        GameObject oldAtlasGo = AssetDatabase.LoadAssetAtPath<GameObject>(oldAtlasPath);
        if (oldAtlasGo == null)
        {
            UDebug.LogErrorFormat("没有旧Atlas：{0}", oldAtlasPath);
            return;
        }

        UIAtlas oldAtlas = oldAtlasGo.GetComponent<UIAtlas>();
        if (oldAtlas == null)
        {
            UDebug.LogErrorFormat("没有旧Atlas：{0}", oldAtlasPath);
            return;
        }

        List<UISpriteData> newSpriteList = newAtlas.spriteList;
        List<UISpriteData> oldSpriteList = oldAtlas.spriteList;
        if (newSpriteList.Count != oldSpriteList.Count)
        {
            UDebug.LogErrorFormat("新图集数量为：{0}, 旧图集数量为：{1}", newSpriteList.Count, oldSpriteList.Count);
            return;
        }

        TextureImporter newTex = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(newAtlas.texture)) as TextureImporter;
        if (newTex == null)
        {
            UDebug.LogErrorFormat("图集：{0}没有texture", newAtlasPath);
            return;
        }

        TextureImporter oldTex = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(oldAtlas.texture)) as TextureImporter;
        if (oldTex == null)
        {
            UDebug.LogErrorFormat("图集：{0}没有texture", oldAtlasPath);
            return;
        }

        if (newTex.alphaIsTransparency != oldTex.alphaIsTransparency)
        {
            UDebug.LogErrorFormat("图片的 alphaIsTransparency 不一样");
        }

        if (newTex.wrapMode != oldTex.wrapMode)
        {
            UDebug.LogErrorFormat("图片的 wrapMode 不一样");
        }

        if (newTex.filterMode != oldTex.filterMode)
        {
            UDebug.LogErrorFormat("图片的 filterMode 不一样");
        }

        if (newTex.anisoLevel != oldTex.anisoLevel)
        {
            UDebug.LogErrorFormat("图片的 anisoLevel 不一样");
        }

        if (newTex.textureFormat != oldTex.textureFormat)
        {
            UDebug.LogErrorFormat("图片的 textureFormat 不一样");
        }

        UISpriteData newSprite;
        UISpriteData oldSprite;
        bool isEqual = true;
        for (int i = 0; i < newSpriteList.Count; i++)
        {
            newSprite = newSpriteList[i];

            if (newSprite == null)
            {
                UDebug.LogErrorFormat("新图集有空sprite");
                isEqual = false;
                continue;
            }

            oldSprite = oldAtlas.GetSprite(newSprite.name);
            if (oldSprite == null)
            {
                UDebug.LogErrorFormat("旧图集中没找到：{0}", newSprite.name);
                isEqual = false;
                continue;
            }

            if (newSprite.width != oldSprite.width)
            {
                UDebug.LogErrorFormat("图片 {0}和{1}宽度不一样", newSprite.name, oldSprite.name);
                isEqual = false;
            }

            if (newSprite.height != oldSprite.height)
            {
                UDebug.LogErrorFormat("图片 {0}和{1}高度不一样", newSprite.name, oldSprite.name);
                isEqual = false;
            }
        }

        if (isEqual)
            UDebug.LogFormat("两图集里sprite的名字宽高都一样");
    }

    void CopyAtlas(string newAtlasPath, string oldAtlasPath)
    {
        UDebug.LogFormat("copy图集{0}和{1}", newAtlasPath, oldAtlasPath);
        GameObject newAtlasGo = AssetDatabase.LoadAssetAtPath<GameObject>(newAtlasPath);
        if (newAtlasGo == null)
        {
            UDebug.LogErrorFormat("没有新Atlas：{0}", newAtlasPath);
            return;
        }

        UIAtlas newAtlas = newAtlasGo.GetComponent<UIAtlas>();
        if (newAtlas == null)
        {
            UDebug.LogErrorFormat("没有新Atlas：{0}", newAtlasPath);
            return;
        }

        GameObject oldAtlasGo = AssetDatabase.LoadAssetAtPath<GameObject>(oldAtlasPath);
        if (oldAtlasGo == null)
        {
            UDebug.LogErrorFormat("没有旧Atlas：{0}", oldAtlasPath);
            return;
        }

        UIAtlas oldAtlas = oldAtlasGo.GetComponent<UIAtlas>();
        if (oldAtlas == null)
        {
            UDebug.LogErrorFormat("没有旧Atlas：{0}", oldAtlasPath);
            return;
        }

        TextureImporter newTex = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(newAtlas.texture)) as TextureImporter;
        if (newTex == null)
        {
            UDebug.LogErrorFormat("图集：{0}没有texture", newAtlasPath);
            return;
        }

        TextureImporter oldTex = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(oldAtlas.texture)) as TextureImporter;
        if (oldTex == null)
        {
            UDebug.LogErrorFormat("图集：{0}没有texture", oldAtlasPath);
            return;
        }

        newTex.alphaIsTransparency = oldTex.alphaIsTransparency;
        newTex.wrapMode = oldTex.wrapMode;
        newTex.filterMode = oldTex.filterMode;
        newTex.anisoLevel = oldTex.anisoLevel;
        newTex.textureFormat = oldTex.textureFormat;
        AssetDatabase.ImportAsset(newAtlasPath);
        AssetDatabase.ImportAsset(oldAtlasPath);

        List<UISpriteData> newSpriteList = newAtlas.spriteList;
        UISpriteData newSprite;
        UISpriteData oldSprite;
        for (int i = 0; i < newSpriteList.Count; i++)
        {
            newSprite = newSpriteList[i];

            if (newSprite == null)
            {
                UDebug.LogErrorFormat("新图集数量有空sprite");
                continue;
            }

            oldSprite = oldAtlas.GetSprite(newSprite.name);
            if (oldSprite == null)
            {
                UDebug.LogErrorFormat("旧图集数量有空sprite");
                continue;
            }

            newSprite.width = oldSprite.width;
            newSprite.height = oldSprite.height;
            newSprite.borderBottom = oldSprite.borderBottom;
            newSprite.borderLeft = oldSprite.borderLeft;
            newSprite.borderRight = oldSprite.borderRight;
            newSprite.borderTop = oldSprite.borderTop;
            newSprite.paddingBottom = oldSprite.paddingBottom;
            newSprite.paddingLeft = oldSprite.paddingLeft;
            newSprite.paddingRight = oldSprite.paddingRight;
            newSprite.paddingTop = oldSprite.paddingTop;
        }

        newAtlas.MarkAsChanged();
        //和File>Save Project一样，保存所有被标记修改的Asset
        AssetDatabase.SaveAssets();
    }

    string spriteFolderPath = "";
    void ExportSpritePanel()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label("先设置分割atlas图片保存路径，再选择要分割的atlas（可以多选），最后点分割atlas图片按钮");
            spriteFolderPath = PathField(spriteFolderPath, "分割atlas图片保存路径", "设置为当前选择的路径", 150, 150);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space((Screen.width - 150) / 2);
                if (GUILayout.Button("分割atlas图片", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    if (Selection.objects != null && !string.IsNullOrEmpty(spriteFolderPath))
                    {
                        for (int j = 0; j < Selection.objects.Length; j++)
                        {
                            UIAtlas atlas = AssetDatabase.LoadAssetAtPath<UIAtlas>(AssetDatabase.GetAssetPath(Selection.objects[j]));

                            SpliteAtlas(atlas);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void SpliteAtlas(UIAtlas atlas)
    {
        if (atlas != null && atlas.texture != null)
        {
            Texture2D tex = atlas.texture as Texture2D;
            if (tex != null)
            {
                string texPath = AssetDatabase.GetAssetPath(tex);
                TextureImporter ti = AssetImporter.GetAtPath(texPath) as TextureImporter;

                ti.isReadable = true;
                AssetDatabase.ImportAsset(texPath);

                List<UISpriteData> spriteDataList = atlas.spriteList;
                UISpriteData spriteData;
                int width, height;
                Color[] colorArray;
                Color emptyColor = new Color(0, 0, 0, 0);
                for (int i = 0; i < spriteDataList.Count; i++)
                {
                    spriteData = spriteDataList[i];
                    width = spriteData.width + spriteData.paddingLeft + spriteData.paddingRight;
                    height = spriteData.height + spriteData.paddingTop + spriteData.paddingBottom;
                    colorArray = new Color[width * height];
                    int index;
                    for (int h = 0; h < height; h++)
                    {
                        for (int w = 0; w < width; w++)
                        {
                            index = w + h * width;
                            if (w >= spriteData.paddingLeft && w < spriteData.width + spriteData.paddingLeft && h >= spriteData.paddingBottom && h < spriteData.height + spriteData.paddingBottom)
                            {
                                //Texture的像素的原点在左下角，而atlas里的原点左上角，所以要对h轴坐标进行转换
                                colorArray[index] = tex.GetPixel(spriteData.x + (w - spriteData.paddingLeft), tex.height - (spriteData.y + spriteData.height) + (h - spriteData.paddingBottom));
                            }
                            else
                            {
                                colorArray[index] = emptyColor;
                            }
                        }
                    }
                    Texture2D spriteTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
                    spriteTex.SetPixels(colorArray);
                    spriteTex.Apply();
                    byte[] bytes = spriteTex.EncodeToPNG();

                    string savePath = spriteFolderPath + "/" + atlas.name;
                    if (!Directory.Exists(Directory.GetCurrentDirectory().Replace("\\", "/") + "/" + savePath))
                    {
                        AssetDatabase.CreateFolder(spriteFolderPath, atlas.name);
                    }
                    if (!System.IO.Directory.Exists(savePath))
                        System.IO.Directory.CreateDirectory(savePath);
                    File.WriteAllBytes(string.Format("{0}/{1}.png", savePath, spriteData.name), bytes);
                }

                ti.isReadable = false;
                AssetDatabase.ImportAsset(texPath);
                AssetDatabase.Refresh();
            }
        }
    }


    string updateResPath = "";
    string updateAtlasPath = "";
    void UpdateAtlasPanel()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label("先把新加的图片拷贝到需要更新的Atlas的原图文件夹里或删除不需要的文件，再设置原图文件夹路径和Atlas文件路径，然后点击更新atlas按钮");
            updateResPath = PathField(updateResPath, "图片资源路径", "设置为当前选择的路径", 100, 150);
            updateAtlasPath = PathField(updateAtlasPath, "更新Atlas文件", "设置为当前选择Atlas文件", 100, 150);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space((Screen.width - 150) / 2);
                if (GUILayout.Button("更新atlas", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    UpdateAtlas(updateAtlasPath, updateResPath);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void UpdateAtlas(string _atlasPath, string _resPath)
    {
        string curProjPath = Directory.GetCurrentDirectory().Replace("\\", "/");
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath<UIAtlas>(_atlasPath);
        if (atlas == null)
            return;

        DirectoryInfo resDirInfo = new DirectoryInfo(curProjPath + "/" + updateResPath);
        if (!resDirInfo.Exists)
            return;

        _atlasPath = _atlasPath.Replace("/" + atlas.name + ".prefab", "");
        DirectoryInfo atlasDirInfo = new DirectoryInfo(curProjPath + "/" + _atlasPath);
        if (!atlasDirInfo.Exists)
            return;

        if (atlas.spriteMaterial == null)
        {
            atlas.spriteMaterial = CreateNewMaterial(_atlasPath + "/" + atlas.name + ".mat");
        }

        string saveTexPath;
        if (atlas.spriteMaterial.mainTexture != null)
        {
            saveTexPath = curProjPath + "/" + AssetDatabase.GetAssetPath(atlas.spriteMaterial.mainTexture);
        }
        else
        {
            saveTexPath = atlasDirInfo.ToString() + "/" + atlas.name + ".png";
        }

        string texturePath = saveTexPath.Replace(curProjPath + "/", "");
        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        bool hasOldTexture = false;
        FilterMode filterMode = FilterMode.Point;
        int anisoLevel = 1;
        if (textureImporter != null)
        {
            hasOldTexture = true;
            filterMode = textureImporter.filterMode;
            anisoLevel = textureImporter.anisoLevel;
            AssetDatabase.DeleteAsset(texturePath);
            AssetDatabase.Refresh();
        }

        string processParameter = string.Format("--size-constraints POT --algorithm Basic --data {1}/{0}.txt --format unity --sheet {3} {2}",
            atlas.name, atlasDirInfo.ToString(), resDirInfo.ToString(), saveTexPath);
        CallTexturePackerProcess(processParameter);

        AssetDatabase.Refresh();

        string atlasname = atlas.name;

        string jsonPath = _atlasPath + "/" + atlasname + ".txt";

        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
        textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (textureImporter != null && texture != null)
        {
            SetTextureImporter(texturePath, texture, textureImporter);
            if (hasOldTexture)
            {
                textureImporter.filterMode = filterMode;
                textureImporter.anisoLevel = anisoLevel;
            }
        }
        else
        {
            AssetDatabase.Refresh();
            return;
        }

        atlas.spriteMaterial.mainTexture = texture;

        AssetDatabase.ImportAsset(jsonPath);
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
        NGUIJson.LoadSpriteData(atlas, ta);
        atlas.MarkAsChanged();

        AssetDatabase.DeleteAsset(jsonPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SetTextureImporter(string texturePath, Texture texture, TextureImporter textureImporter)
    {
        textureImporter.textureType = TextureImporterType.Default;
        textureImporter.mipmapEnabled = false;
        textureImporter.alphaIsTransparency = false;
        textureImporter.maxTextureSize = Mathf.Max(texture.width, texture.height);
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.wrapMode = TextureWrapMode.Clamp;
        textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
        AssetDatabase.ImportAsset(texturePath);
    }
}
