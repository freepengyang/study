using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CSShaderManager : Singleton2<CSShaderManager>
{
    private CSShareMaterial mShareMaterial = null;
    public CSShaderManager()
    {
        mShareMaterial = new CSShareMaterial();
    }

    public static Material GetShareMaterial(UnityEngine.Object obj, EShareMatType type = EShareMatType.Normal, string shaderName = "", bool isTest = true)
    {
        if (Instance == null)
        {
            Shader shader = Shader.Find("Mobile/LZF/Alpha Blended");

            if (shader != null)
            {
                Material material = new Material(shader);

                return material;
            }
            return null;
        }
        return Instance.mShareMaterial.GetShareMat(obj, type, shaderName);
    }

    public static Material GetMaterial(string shaderName)
    {
        return Instance.mShareMaterial.GetMaterialByShaderName(shaderName);
    }


}
