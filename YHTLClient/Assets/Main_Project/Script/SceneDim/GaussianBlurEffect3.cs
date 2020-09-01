using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class GaussianBlurEffect3 : MonoBehaviour
{
	#region  variables

	public Shader GaussianBlurShader;
	public float blurSpread = 1; //模糊系数，传递给shader
	public int iterations = 4; //纹理循环次数
	public int downSample = 4; //纹理分割系数

	private bool isOpenGaussian;//是否打开场景模糊
	private Material curMaterial;
	private RenderTexture rtTempA;
	private RenderTexture rtTempB;
	private float downSamplerate;
	private int rtW;
	private int rtH;

	Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(GaussianBlurShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}

			return curMaterial;
		}
	}

	#endregion

	// Start is called before the first frame update
	void Awake()
	{
		downSamplerate = 1.0f / downSample;
		if (GaussianBlurShader == null)
		{
			GaussianBlurShader = Shader.Find("Effect/GaussianBlur3");
		}
		CSGame.MainEventHandler.AddEvent(MainEvent.ShowOrCloseGaussian, ShowOrCloseGaussian);
	}

	private void ShowOrCloseGaussian(uint id, object data)
	{
		isOpenGaussian = (bool)data;
	}

	/// <summary>
	/// OnRenderImage is called after all rendering is complete to render image.
	/// </summary>
	/// <param name="src">The source RenderTexture.</param>
	/// <param name="dest">The destination RenderTexture.</param>
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (GaussianBlurShader != null && isOpenGaussian)
		{
			material.SetFloat("_BlurSize", blurSpread);
			//定义临时渲染纹理宽高
			downSamplerate = 1.0f / downSample;
			rtW = (int)(src.width * downSamplerate);
			rtH = (int)(src.height * downSamplerate);
			//获取临时渲染图片A,及修改图格式
			rtTempA = RenderTexture.GetTemporary(rtW, rtH, 0, src.format);
			rtTempA.filterMode = FilterMode.Bilinear;
			//获取临时渲染图片B,及修改图格式
			rtTempB = RenderTexture.GetTemporary(rtW, rtH, 0, src.format);
			rtTempB.filterMode = FilterMode.Bilinear;
			//循环渲染数次
			for (int i = 0; i < iterations; i++)
			{
				if (i == 0)
				{
					//通过通道1渲染
					Graphics.Blit(src, rtTempA, material, 0);
					//通过通道2渲染
					Graphics.Blit(rtTempA, rtTempB, material, 1);
				}
				else
				{
					Graphics.Blit(rtTempB, rtTempA, material, 0);
					Graphics.Blit(rtTempA, rtTempB, material, 1);
				}
			}

			Graphics.Blit(rtTempB, dest);
			//释放临时渲染图片
			RenderTexture.ReleaseTemporary(rtTempA);
			RenderTexture.ReleaseTemporary(rtTempB);
		}
		else
		{
			Graphics.Blit(src, dest);
		}
	}

	// Update is called once per frame
	void Update()
	{
	}
}