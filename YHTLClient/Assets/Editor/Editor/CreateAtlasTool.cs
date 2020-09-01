using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 打包图集工具
/// </summary>
public class CreateAtlasTool : EditorWindow
{
    static List<object> PitchObjs;
    static Dictionary<Texture2D,bool> SelectionObjs;
    string index = "索引";
    string texture = "贴图";
    string UIprefabpath = "Assets/UIAsset/chart/";
    Vector2 mScroll = Vector2.zero;

    [MenuItem("Tools/打包图集 %m")]
    static void Excute()
    {
        CreateAtlasTool data = EditorWindow.GetWindow<CreateAtlasTool>("打包图集");
        data.Show();
    }

    void Awake()
    {
        RefreshList();
    }

    void OnGUI()
    {
        GUILayout.Label("需要打包图集的贴图列表如下：");

        GUILayout.BeginHorizontal("TextArea",GUILayout.MaxHeight(20));
        GUILayout.Label(index,GUILayout.Width(30));
        GUILayout.Label(texture,GUILayout.Width(250));
        GUILayout.Label("是否打包",GUILayout.Width(50));
        GUILayout.EndHorizontal();


        mScroll = GUILayout.BeginScrollView(mScroll);

        for (int i = 0; i < PitchObjs.Count; i++)
        {
            Texture2D data = (Texture2D)PitchObjs[i];
            DrawRow(i, data);
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        GUI.contentColor = Color.white;
        if (GUILayout.Button("生成图集",GUILayout.MaxWidth(100)))
        {
            CreateTexture();
        }
        if (GUILayout.Button("更新图集", GUILayout.Width(100)))
        {
            FNDebug.Log("更新图集");
            UpSpriteData();
        }
        if (GUILayout.Button("-", GUILayout.Width(30)))
        {
            FNDebug.Log("删除");
        }
    }

    void RefreshList()
    {
        object[] obj = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        PitchObjs = new List<object>();
        PitchObjs.AddRange(obj);
        object[] materials =Selection.GetFiltered(typeof(Material),SelectionMode.DeepAssets);

        SelectionObjs = new Dictionary<Texture2D, bool>();
        for (int i = 0; i < PitchObjs.Count;i++)
        {
            Texture2D tex = (Texture2D)PitchObjs[i];
            for (int j = 0; j < materials.Length; j++)
            {
                Material mate =(Material)materials[j];
                if (tex.name == mate.name)
                {
                    if (!SelectionObjs.ContainsKey(tex))
                    {
                        SelectionObjs.Add(tex, false);
                    }
                    else
                    {
                        SelectionObjs[tex] = false;
                    }
                }
                else
                {
                    if (!SelectionObjs.ContainsKey(tex))
                    {
                        SelectionObjs.Add(tex, false);//默认选中
                    }
                }
            }
        }
        
    }

    bool DrawRow(int index,Texture2D data)
    {
        GUI.contentColor = SelectionObjs[data] == true ? Color.white : Color.yellow;
        GUILayout.BeginHorizontal();
        GUILayout.Label(index.ToString(), GUILayout.Width(30));

        if (GUILayout.Button(data.ToString(), EditorStyles.label, GUILayout.Width(250)))
        {
            SelectionObjs[data] = !SelectionObjs[data];
        }
        SelectionObjs[data] = GUILayout.Toggle(SelectionObjs[data], "", GUILayout.Width(50));
        GUILayout.Space(1);
        GUILayout.EndHorizontal();

        return SelectionObjs[data];
    }

    void UpSpriteData()
    {
        foreach (Texture2D ob in SelectionObjs.Keys)
        {
            if (SelectionObjs[ob])
            {
                string path = AssetDatabase.GetAssetPath(ob);
                string pfabth = UIprefabpath + ob.name + ".prefab";
                if (File.Exists(pfabth))
                {                    
                    
                    if (Path.GetExtension(path) == ".png")
                    {
                        if (AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)))
                        {
                            GameObject prb = AssetDatabase.LoadAssetAtPath(pfabth, typeof(GameObject)) as GameObject;
                            UIAtlas uiAtlas = prb.GetComponent<UIAtlas>();
                            //Material mat = AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".mat"), typeof(Material)) as Material;
                            //uiAtlas.spriteMaterial = mat;
                            TextAsset ta = AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)) as TextAsset;
                            NGUIJson.LoadSpriteData(uiAtlas, ta);
                            uiAtlas.MarkAsChanged();
                        }
                    }

                    AssetDatabase.SaveAssets();


                }
            }
            else
            {
                FNDebug.Log("no need pack");
            }
        }
        AssetDatabase.Refresh();
    }

    void CreateTexture()
    {
       // PitchObjs = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        foreach (Texture2D ob in SelectionObjs.Keys)
        {
            if (SelectionObjs[ob])
            {
                string path = AssetDatabase.GetAssetPath(ob);
                if (Path.GetExtension(path) == ".png")
                {
                    if (AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)))
                    {
                        #region 第一步：根据图片创建游戏对象、材质对象
                        GameObject atlase = new GameObject();
                        atlase.name = ob.name;
                        Material mat = new Material(Shader.Find("Unlit/Transparent Colored"));
                        mat.name = ob.name;
                        AssetDatabase.CreateAsset(mat, path.Replace(".png", ".mat"));
                        #endregion

                        #region 第二步：给对象添加组件、给材质球关联着色器及纹理同时关联tp产生的坐标信息文件
                        atlase.AddComponent<UIAtlas>();
                        mat.mainTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                        UIAtlas uiAtlas = atlase.GetComponent<UIAtlas>();
                        uiAtlas.spriteMaterial = mat;
                        TextAsset ta = AssetDatabase.LoadAssetAtPath(path.Replace(".png", ".txt"), typeof(TextAsset)) as TextAsset;
                        NGUIJson.LoadSpriteData(uiAtlas, ta);

                        uiAtlas.MarkAsChanged();
                        #endregion

                        #region 第三步：创建预设
                        CreatePrefab(atlase, ob.name, UIprefabpath);
                        #endregion
                    }
                }
            }
            else
            {
                FNDebug.Log("no need pack");
            }
        }
        AssetDatabase.Refresh();
    }

    static Object CreatePrefab(GameObject go, string name, string prepath)
    {
        Object tmpPrefab = PrefabUtility.CreateEmptyPrefab(prepath + name + ".prefab");
        tmpPrefab = PrefabUtility.ReplacePrefab(go, tmpPrefab, ReplacePrefabOptions.ConnectToPrefab);
        Object.DestroyImmediate(go);
        return tmpPrefab;
    }
}