using UnityEngine;
using System.Collections;
using UnityEditor;
namespace ExtendEditor
{
    public class RemoveTextureAlpha : EditorWindow
    {
        Texture2D tex;
        [MenuItem("Tools/Miscellaneous/移除图片的Alhpa通道")]
        public static void RenameProc()
        {
            EditorWindow win = GetWindow(typeof(RemoveTextureAlpha));
            win.Show();
        }


        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            tex = EditorGUILayout.ObjectField(tex, typeof(Texture2D)) as Texture2D;
            if (GUILayout.Button("Deal"))
            {
                Deal(tex);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("DealSelect"))
            {
                DealAll();
            }
        }

        void DealAll()
        {
            object[] objs = Selection.objects;

            for (int i = 0; i < objs.Length; i++)
            {
                Texture2D tex = objs[i] as Texture2D;
                if (tex == null) continue;
                Deal(tex);
            }
        }

        void Deal(Texture2D tex)
        {
            if (tex != null)
            {
                string path = AssetDatabase.GetAssetPath(tex);
                TextureImporter importer = TextureImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    FNDebug.LogError("not find path = " + path);
                    return;
                }
                if (importer.isReadable == false)
                {
                    importer.isReadable = true;
                    AssetDatabase.ImportAsset(path);
                }

                Color32[] pixels = tex.GetPixels32();

                int xmin = tex.width;
                int xmax = 0;
                int ymin = tex.height;
                int ymax = 0;
                int oldWidth = tex.width;
                int oldHeight = tex.height;
                for (int y = 0, yw = oldHeight; y < yw; ++y)
                {
                    for (int x = 0, xw = oldWidth; x < xw; ++x)
                    {
                        Color32 c = pixels[y * xw + x];

                        if (c.a != 0)
                        {
                            if (y < ymin) ymin = y;
                            if (y > ymax) ymax = y;
                            if (x < xmin) xmin = x;
                            if (x > xmax) xmax = x;
                        }
                    }
                }

                int newWidth = (xmax - xmin) + 1;
                int newHeight = (ymax - ymin) + 1;

                Color32[] newPixels = new Color32[newWidth * newHeight];

                for (int y = 0; y < newHeight; ++y)
                {
                    for (int x = 0; x < newWidth; ++x)
                    {
                        int newIndex = y * newWidth + x;
                        int oldIndex = (ymin + y) * oldWidth + (xmin + x);
                        newPixels[newIndex] = pixels[oldIndex];
                    }
                }

                Texture2D newTex = new Texture2D(newWidth, newHeight);
                newTex.name = tex.name;
                newTex.SetPixels32(newPixels);
                newTex.Apply();
                //AssetDatabase.DeleteAsset(path);
                byte[] bytes = newTex.EncodeToPNG();
                System.IO.File.WriteAllBytes(path, bytes);
                AssetDatabase.ImportAsset(path);
                DestroyImmediate(newTex);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
