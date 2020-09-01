﻿Shader "Mobile/LZF/SimpleAlpha" {  
    Properties {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
        _TransVal ("Transparency Value", Range(0,1)) = 0.5  
    }  
    SubShader { 
	
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}  
        LOD 200  
          
        CGPROGRAM  
        #pragma surface surf Lambert alpha  
  
        sampler2D _MainTex;  
        float _TransVal;  
  
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 col;
			if (i.color.r < 0.001)
			{
				col = tex2D(_MainTex, i.texcoord);
				float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
				col.rgb = float3(grey, grey, grey);
			}
			else
			{
				col = tex2D(_MainTex, i.texcoord) * i.color;
			}
			return col;
		}

        struct Input {  
            float2 uv_MainTex;  
        };  
  
        void surf (Input IN, inout SurfaceOutput o) {  
            half4 c = tex2D (_MainTex, IN.uv_MainTex);  
            o.Albedo = c.rgb;  
            //o.Alpha = c.b * _TransVal;  
			o.Alpha = _TransVal; 
        }  
        ENDCG  
    }   
    FallBack "Diffuse"  
} 