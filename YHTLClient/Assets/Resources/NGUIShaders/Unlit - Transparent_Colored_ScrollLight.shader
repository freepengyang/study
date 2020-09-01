// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent_Colored_ScrollLight"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}

		//---add---------------------------------
		_MaskTex ("Mask Alpha (A)", 2D) = "white" {}
		_IfMask("Open mask if larger than 0.5", Range(0,1)) = 0
		_WidthRate ("Sprite.width/Atlas.width", float) = 1
		_HeightRate ("Sprite.height/Atlas.height", float) = 1
		_XOffset("offsetX/Atlas.width", float) = 0
		_YOffset("offsetY/Atlas.height", float) = 0
		_FlowTex("flow tex",2D) = ""{}
		_LightSpeed("Light Speed",float) = 2
		//--------------------------------------
	}
	
	SubShader
	{
		LOD 200

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
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
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
			//---add-------
			sampler2D _MaskTex;
			float _IfMask; 
			float _WidthRate;
			float _HeightRate;
			float _XOffset; 
			float _YOffset; 
			sampler2D _FlowTex;
			float _LightSpeed;
			//--------------

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col;
				col = tex2D(_MainTex, i.texcoord);
 
				//---------add---------------------------------
				//flow light
				if(i.color.g<=0.1)
				{
					float2 flow_uv = float2((i.texcoord.x-_XOffset)/_WidthRate, (i.texcoord.y-_YOffset)/_HeightRate);
					flow_uv.x/=2;
					flow_uv.x-= _Time.y *_LightSpeed;
					half flow = tex2D(_FlowTex,flow_uv).a; 
					col.rgb+= half3(flow,flow,flow);
				}
				//-----------------------------------------------
				return col;
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
