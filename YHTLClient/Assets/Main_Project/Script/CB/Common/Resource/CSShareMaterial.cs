using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EShareMatType
{
    Normal,
    Transparent,
    Balck,
    Balck_Transparent,
    ColorBright,
    ColorBright_Transparent,
    ColorSet_Grey,
    ColorSet_Grey_Transparent,
    ColorSet_Green,
    ColorSet_Green_Transparent,
    ColorSet_Red,
    ColorSet_Red_Transparent,
    ColorSet_Blue,
    ColorSet_Blue_Transparent,
    ColorAdd,
    ColorAdd_Transparent,
    DeadTransparent,
    ColorScreen,
}

 [System.Serializable]
public class CSShareMaterial 
{
    public Dictionary<int, List<Material>> InstanceIDToShareMat = new Dictionary<int, List<Material>>();
    public Dictionary<int, Material> InstanceIDToYeManDic = new Dictionary<int, Material>();
    public Dictionary<int, string> TypeToShaderName = new Dictionary<int, string>()
    {
        {(int)EShareMatType.Normal,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.Balck,"Mobile/LZF/Black"},
        {(int)EShareMatType.Balck_Transparent,"Mobile/LZF/Black"},
        {(int)EShareMatType.ColorBright,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorBright_Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Grey,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Grey_Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Green,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Green_Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Red,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Red_Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Blue,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorSet_Blue_Transparent,"Mobile/LZF/ColorSet"},
        {(int)EShareMatType.ColorAdd,"Mobile/LZF/ColorAdd"},
        {(int)EShareMatType.ColorAdd_Transparent,"Mobile/LZF/ColorAdd"},
        {(int)EShareMatType.ColorScreen,"Mobile/LZF/ColorScreen"},
    };

    public Dictionary<string, string> ShaderToHotShaderDic = new Dictionary<string, string>()
    {
        {"Mobile/LZF/ColorAdd","ColorAdd"},
        {"Mobile/LZF/ColorScreen","ColorScreen"},
        {"Mobile/LZF/ColorSet","ColorSet"},
        {"Mobile/LZF/Black","Black"},
        {"Unlit/Transparent Colored","TransparentColored"},
    };

     public void Clear()
    {
        InstanceIDToShareMat.Clear();
        InstanceIDToYeManDic.Clear();
    }

     public string GetShader(EShareMatType type)
    {
        if (TypeToShaderName.ContainsKey((int)type))
            return TypeToShaderName[(int)type];
        return "";
    }
     public Material GetShaderMatByYeMan(UnityEngine.Object obj,string shaderName)
     {
         if (obj == null) return null;
         int id = obj.GetInstanceID();
         if (!InstanceIDToYeManDic.ContainsKey(id))
         {
             Shader shader = Shader.Find(shaderName);
             Material mat = new Material(shader);
             InstanceIDToYeManDic.Add(id, mat);
         }
         return InstanceIDToYeManDic[id];
     }

    public Material GetShareMat(UnityEngine.Object obj, EShareMatType type = EShareMatType.Normal, string shaderName = "")
    {
        Shader hotShader = null;
        if (type == EShareMatType.Normal)
        {
            SFAtlas atlas = obj as SFAtlas;
            if (atlas != null)
            {
                if (atlas.spriteMaterial != null && hotShader != null && atlas.spriteMaterial.shader != hotShader)
                {
                    Texture _MainTex = atlas.spriteMaterial.mainTexture;
                    Texture _AlphaTex = atlas.spriteMaterial.HasProperty("_AlphaTex") ? atlas.spriteMaterial.GetTexture("_AlphaTex") : null;

                    atlas.spriteMaterial.shader = hotShader;
                    atlas.spriteMaterial.SetTexture("_MainTex", _MainTex);
                    atlas.spriteMaterial.SetTexture("_AlphaTex", _AlphaTex);
                    atlas.spriteMaterial.SetInt("_IsAlphaSplit", _AlphaTex != null ? 1 : 0);
                }
                return atlas.spriteMaterial;
            }
        }
        if (obj == null)return null;
        int id = obj.GetInstanceID();
        if (!InstanceIDToShareMat.ContainsKey(id))
        {
            InstanceIDToShareMat.Add(id, new List<Material>());
        }
        if ((int)type >= InstanceIDToShareMat[id].Count)
        {
            int count = InstanceIDToShareMat[id].Count;
            for (int i = 0; i <= (int)type - count; i++)
            {
                InstanceIDToShareMat[id].Add(null);
            }
        }
        if (InstanceIDToShareMat[id][(int)type] == null)
        {
            Shader shader = null;
            if (hotShader != null)
            {
                shader = hotShader;
            }
            else
            {
                shaderName = !string.IsNullOrEmpty(shaderName) ? shaderName : TypeToShaderName[(int)type];
                //Debug.LogError(shaderName);
                shader = Shader.Find(shaderName);
            }
            
            Material mat = new Material(shader);
            mat.name = TypeToShaderName[(int)type] /*+ "_" + id*/;
            InstanceIDToShareMat[id][(int)type] = mat;

        }
        return InstanceIDToShareMat[id][(int)type];
    }

    public Material GetNewMaterial(EShareMatType type = EShareMatType.Normal, string shaderName = "")
    {
        shaderName = string.IsNullOrEmpty(shaderName) ? TypeToShaderName[(int)type] : shaderName;
        Shader shader = Shader.Find(shaderName);
        return new Material(shader);
    }

    public Material GetMaterialByShaderName(string shaderName)
    {
        Shader shader = Shader.Find(shaderName);
        return new Material(shader);
    }
}
