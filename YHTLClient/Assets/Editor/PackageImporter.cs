using UnityEngine;
using System.Collections;
using UnityEditor;

public class PackageImporter : EditorWindow
{
    private string packagePath = string.Empty;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("PackagePath"))
            packagePath = PlayerPrefs.GetString("PackagePathEditor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Choose UnityPackage"))
        {
            packagePath = EditorUtility.OpenFilePanel("选取UnityPackage", packagePath, "unitypackage");
            if (!string.IsNullOrEmpty(packagePath))
            {
                PlayerPrefs.SetString("PackagePathEditor", packagePath);
            }
        }
        EditorGUILayout.LabelField(packagePath);

        if (GUILayout.Button("Import UnityPackage"))
        {
            ImportUnityPackage();
            this.Close();
        }
    }

    private void ImportUnityPackage()
    {
        AssetDatabase.ImportPackage(packagePath, false);
    }
}
