using UnityEngine;
using System.Collections;

/// <summary>
///挂载需要初始化shader的prefab上
/// </summary>
public class CSMatShaderSet : MonoBehaviour
{
	// Use this for initialization
    public Material mat;
	void Awake () 
    {
        if (mat != null&& mat.shader!=null)
        {
            mat.shader = Shader.Find(mat.shader.name);  
        }

        Destroy(this);
	}
}
