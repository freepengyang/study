// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mobile/LZF/ColorAdd"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "black" {}
		_AlphaTex ("AlphaTex", 2D) = "white" {}
		_MainTexRGBA ("MainTexRGB", Vector) = (1,1,1,1)
		_IsAlphaSplit ("IsAlphaSplit", Int) = 0
	}
	
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
	        fixed4 _MainTexRGBA;
			fixed4 _MainTexGrey;
			int _IsAlphaSplit;
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
			if(_IsAlphaSplit ==0)
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);  
				col.rgba = col.rgba*_MainTexRGBA;
				col.rgb = col.rgb*col.a;
				return col;
			}
			else
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);  
				col.rgb = col.rgb*_MainTexRGBA*fixed3(1.1,1.1,1.1);
				fixed4 alphaCol = tex2D(_AlphaTex,IN.texcoord);
				col.a = alphaCol.r*_MainTexRGBA.a;
				col.rgb = col.rgb*col.a;
				return col;
			}

			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
