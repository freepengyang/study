using UnityEditor;
using UnityEditor.Build.Reporting;

public class AndroidApkProcess
{
    [MenuItem("Apk/Start Apk")]
    static void StartApk()
    {
        BuildResult result = BuildAndroidProject.BuildUnityAndroidProject();
        if(result != BuildResult.Succeeded)
        {
            UnityEngine.Debug.LogError("Build Unity APK Failed  :   error : " + result.ToString());
            return;
        }
        DoAssetbundle.CreateAllAssetBundles();
        DoAssetbundle.EncryptAB();
        CreateResListBytes.CreateAllToTxt();
        CreateResListBytes.CrateAllResBytes();
        FBPkgGen.BuildPackage.BuildSPKG();
    }
    
    [MenuItem("Apk/Start Resource")]
    static void StartResource()
    {
        DoAssetbundle.CreateAllAssetBundles();
        DoAssetbundle.EncryptAB();
        CreateResListBytes.CreateAllToTxt();
        CreateResListBytes.CrateAllResBytes();
        FBPkgGen.BuildPackage.BuildSPKG();
    }
    
    [MenuItem("Apk/Start Apk No ResMd5")]
    static void StartApkNoMd5()
    {
        BuildResult result = BuildAndroidProject.BuildUnityAndroidProject();
        if(result != BuildResult.Succeeded)
        {
            UnityEngine.Debug.LogError("Build Unity APK Failed  :   error : " + result.ToString());
            return;
        }
        DoAssetbundle.CreateAllAssetBundles();
        DoAssetbundle.EncryptAB();
        FBPkgGen.BuildPackage.BuildSPKG();
    }
    
    [MenuItem("Apk/Start SPKG")]
    static void StartSPKG()
    {
        FBPkgGen.BuildPackage.BuildSPKG();
    }
}