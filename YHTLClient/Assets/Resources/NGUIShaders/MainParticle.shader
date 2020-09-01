Shader "ZSY/Effect/MainParticle"
{
    Properties
    {
		[Enum(Alpha Blend,10,Addtive,1)] _DestBlend("混合模式", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("剔除", Float) = 0
		[Enum(Off,0,On,1)] _Zwrite("深度写入", Float) = 0
		_Zoffset("深度偏移",Range(-0.5,0.5)) = 0
		_ColorOffset ("亮度偏移", Range(-0.5, 0.5)) = 0.5
		[Space]
		[Space]
		//[Toggle(_MAIN_SWITCH)] _MAIN_SWITCH("基础效果开关", Float) = 1
		_MainContrast("幂", Float) = 1
		[Space]
		[Space]
		[Toggle(_FRESNEL_ON)] _FRESNEL_ON("Fresnel开关", Float) = 0
		[HDR]_FresnelColor("Fresnel Color", Color) = (1, 1, 1, 1)
		_Exp("Exp", Float) = 1
		[MaterialToggle] _CVS_W("自定义W = Fresnel范围", Float) = 0
		[MaterialToggle] _Inv_Fresnel("反向Fresnel", Float) = 0
		[Toggle(_FRESNELCHANEL_ON)] _FRESNELCHANEL_ON("Fresnel作为alpha", Float ) = 0
		[Space]
		[Space]
		[Toggle(_DISSOVLE_ON)] _DISSOVLE_ON("溶解开关", Float) = 0
		[Toggle(_DISSOLVE)] _DISSOLVE("Main/Noise 作为溶解纹理", Float) = 0
		[MaterialToggle] _CVS_Z("自定义Z = 溶解强度", Float) = 0
		_DissolveInt("溶解强度", Range(0, 1)) = 0
		_DissolveStep("边缘羽化", Range(-1, 1)) = 0
		_DissolveEdge("边缘范围", Range(-1, 1)) = 0
		[HDR]_EdgeColor("边缘 Color", Color) = (1, 1, 1, 1)
		[Space]
		[Space]
		_Intensity("Main/Mask/边缘/Fresnel 强度", Vector) = (1, 1, 1, 1)
		[Space]
		[Space]
        _MainTex ("Main Tex", 2D) = "white" {}
		[HDR]_TintColor("Tint Color", Color) = (1, 1, 1, 1)
		[MaterialToggle] _MainAlpha("Main alpha开关", Float) = 1
		[MaterialToggle] _CVS_UV("自定义XY = MainUV", Float) = 0
		[Toggle(_MAIN_POLAR)] _MAIN_POLAR("Main极坐标", Float) = 0
		_MainAlphaCut("Main Alpha Cut", Float) = 0
		[Toggle(_MAIN_OFFSET)] _MAIN_OFFSET ("色调偏移开关", Float) = 0
		_MainOffset("Main色调偏离", Range(-0.1,0.1)) = 0
		_MainSpeed("Main UV流动/旋转", Vector) = (0, 0, 0, 0)
		[Space]
		[Space]
		[Toggle(_MASK_ON)] _MASK_ON("Mask贴图开关", Float) = 0
		_MaskTex("Mask Tex", 2D) = "white" {}
		[MaterialToggle] _CVS_UV2("自定义XY = MaskUV", Float) = 0
		[Toggle(_MASK_POLAR)] _MASK_POLAR("Mask极坐标", Float) = 0
		[Toggle(_ALPHA_MASK)] _ALPHA_MASK("Mask作为Main遮罩", Float) = 0
		[Toggle(_NOISE_MASK)] _NOISE_MASK("Mask作为Noise遮罩", Float) = 0
		[Toggle(_ADD_TEX)] _ADD_TEX("Mask作为叠加纹理", Float) = 0
		[Toggle(_DISS_MASK)] _DISS_MASK("Mask作用于溶解纹理", Float) = 0
			
		[MaterialToggle] _MainMask("Main * Mask", Float) = 0
		_MaskAlphaCut("Mask Alpha Cut", Float) = 0
		[HDR]_AddTexColor("叠加纹理 Color", Color) = (1, 1, 1, 1)
		_MaskSpeed("Mask UV流动/旋转", Vector) = (0, 0, 0, 0)
		[Space]
		[Space]
		[Toggle(_NOISE_ON)] _NOISE_ON("Noise贴图开关", Float) = 0
		_NoiseTex("Noise Tex", 2D) = "white" {}
		[MaterialToggle] _CVS_UV3("自定义XY = NoiseUV", Float) = 0
		[Toggle(_NOISE_POLAR)] _NOISE_POLAR("Noise极坐标", Float) = 0
		[Toggle(_NOISE_GMASK)] _NOISE_GMASK("开启Noise作为全局遮罩", Float) = 0
		_NoiseInt("紊乱强度", Range(-1, 1)) = 0
		_NoiseSpeed("Noise UV流动/旋转/紊乱强度U/紊乱强度V", Vector) = (0, 0, 1, 1)
		[Space]
		[Space]
		[Toggle(_VERT_OFFSET)] _VERT_OFFSET("顶点偏移开关", Float) = 0
		_VertexOffsetInt("顶点偏移", Range(-1, 1)) = 0
		[MaterialToggle] _CVS_V("自定义W = 顶点偏移", Float) = 0
		
		/*_StencilComp("Stencil Comparison", Float) = 8.0
		_Stencil("Stencil ID", Float) = 0.0
		_StencilOp("Stencil Operation", Float) = 0.0
		_StencilWriteMask("Stencil Write Mask", Float) = 255.0
		_StencilReadMask("Stencil Read Mask", Float) = 255.0*/
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha [_DestBlend]
		Zwrite [_Zwrite]
		Cull [_Cull]
		Offset [_Zoffset], 0
		//LOD 99

		/*Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp]
		}*/

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature _ _MAIN_POLAR
			#pragma shader_feature _ _MAIN_OFFSET
			#pragma shader_feature _ _MASK_ON
			#pragma shader_feature _ _MASK_POLAR
			#pragma shader_feature _ _ALPHA_MASK
			#pragma shader_feature _ _NOISE_MASK
			#pragma shader_feature _ _ADD_TEX
			#pragma shader_feature _ _DISS_MASK
			#pragma shader_feature _ _NOISE_ON
			#pragma shader_feature _ _NOISE_POLAR
			#pragma shader_feature _ _NOISE_GMASK
			#pragma shader_feature _ _DISSOLVE
			#pragma shader_feature _ _FRESNEL_ON
			#pragma shader_feature _ _VERT_OFFSET
			#pragma shader_feature _ _DISSOVLE_ON
			#pragma shader_feature _ _FRESNELCHANEL_ON

            #include "../Include/ParticleIncludes.cginc"


            struct v2f
            {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR0;
				half4 uv0 : TEXCOORD0;
				half4 uv1 : TEXCOORD1;

				#if _FRESNEL_ON
				float4 posWorld : TEXCOORD2;
				half4 fresnel_color : COLOR1;
				#endif

				#if _NOISE_ON
				half2 noise_uv : TEXCOORD3;
				#endif

				#if _MASK_ON
				half2 mask_uv : TEXCOORD4;
				#endif

				half2 main_uv : TEXCOORD5;

				#if _DISSOVLE_ON
				half3 diss_var : TEXCOORD6;
				half4 edge_color : COLOR2;
				#endif
	
				float4 mask_color : COLOR3;
				float4 main_color : COLOR4;
            };

            v2f vert (appdata_particle v)
            {
                v2f o;
				o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
				o.color = float4(LinearToGammaSpace(v.color.rgb),v.color.a);
				//o.color = v.color;
				o.mask_color = _AddTexColor * v.color.a * _Intensity.y;
				o.main_color = _Intensity.x * _TintColor * o.color;

				half2 vaspeed = lerp(_Time.y * _MainSpeed.xy, o.uv1.xy, _CVS_UV);
				#if _MAIN_POLAR
				o.main_uv = vaspeed;
				#else
				o.main_uv = o.uv0 + vaspeed;
				#endif

				#if _MASK_ON
				half2 vmspeed = lerp(_Time.y * _MaskSpeed.xy, o.uv1.xy, _CVS_UV2);

				#if _MASK_POLAR
				o.mask_uv = vmspeed;
				#else
				o.mask_uv = TRANSFORM_TEX(vmspeed + o.uv0, _MaskTex);
				#endif

				#endif

				#if _NOISE_ON
				half2 vospeed = lerp(_Time.y * _NoiseSpeed.xy, o.uv1.xy, _CVS_UV3);
				
				#if _NOISE_POLAR
				o.noise_uv = vospeed;
				#else
				o.noise_uv = TRANSFORM_TEX(vospeed + o.uv0, _NoiseTex);
				#endif


				#if _VERT_OFFSET
				half2 noise_offset = o.noise_uv;

				#if _NOISE_POLAR
				half2 voramp = o.uv0 * 2 - 1;
				half2 vopolarcoord = half2(atan2(voramp.x, voramp.y) / UNITY_TWO_PI + 0.5, length(voramp));
				noise_offset = TRANSFORM_TEX(vospeed + vopolarcoord, _NoiseTex);
				#endif

				half4 noise = tex2Dlod(_NoiseTex, half4(noise_offset, 0, 0));
				v.vertex.xyz += noise.rgb * v.normal * lerp(_VertexOffsetInt, o.uv1.w, _CVS_V);
				#endif
				#endif


				#if _FRESNEL_ON
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.posWorld.w = lerp(_Exp, o.uv1.w, _CVS_W) * v.color.a;
				o.fresnel_color = v.color * _FresnelColor * _Intensity.w;
				#endif

				#if _DISSOVLE_ON
				half stepmax = _DissolveStep * 0.5 + 0.5;
				half stepmin = 1 - stepmax;
				half disInt = lerp(_DissolveInt, o.uv1.z, _CVS_Z) * -1.1 + 0.55;
				o.diss_var = half3(stepmax, stepmin, disInt);
				o.edge_color.a = _Intensity.z * v.color.a;
				o.edge_color.rgb = o.edge_color.a * _EdgeColor.rgb;
				#endif

				o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

			half4 frag (v2f i, float facing : VFACE) : SV_Target
			{
				half2 mainuv = 0;

				#if _MAIN_POLAR || _MASK_POLAR || _NOISE_POLAR
				half2 voramp = i.uv0 * 2 - 1;
				half2 vopolarcoord = half2(atan2(voramp.x, voramp.y) / UNITY_TWO_PI + 0.5, length(voramp));
				#endif

				#if _MAIN_POLAR 
				mainuv += vopolarcoord;
				#endif

				#if _MASK_POLAR && _MASK_ON
				i.mask_uv = TRANSFORM_TEX(i.mask_uv + vopolarcoord, _MaskTex);
				#endif

				#if _NOISE_POLAR && _NOISE_ON
				i.noise_uv = TRANSFORM_TEX(i.noise_uv + vopolarcoord, _NoiseTex);
				#endif

				#if _FRESNEL_ON
				half face = sign(facing) * 2 - 1;
				float3 normal = i.normal * face;
				float3 viewdir = normalize(UnityWorldSpaceViewDir(i.posWorld.xyz));
				float nv = saturate(dot(viewdir, normal));
				#endif

				#if _NOISE_ON
				half4 noise = tex2D(_NoiseTex, i.noise_uv);
				#else
				half4 noise = 0;
				#endif

				#if _MASK_ON
				half4 mask = tex2D(_MaskTex, i.mask_uv);
				#else
				half4 mask = 1;
				#endif

				half4 maskcolor = mask * i.mask_color;
				half maskcut = saturate((maskcolor.a - _MaskAlphaCut) * i.color.a);

				half2 noisemask = half2(noise.r * _NoiseSpeed.z , noise.g * _NoiseSpeed.w) * _NoiseInt;
				#if _NOISE_MASK
				noisemask *= maskcut;
				#endif

				#if _MAIN_POLAR
				mainuv += noisemask + lerp(_Time.yy * _MainSpeed.xy, i.uv1.xy, _CVS_UV);
				#else
				mainuv = i.uv0.xy + lerp(_Time.yy * _MainSpeed.xy, i.uv1.xy, _CVS_UV) + noisemask;
				#endif
				
				#if _MAIN_OFFSET
				half2 offuvR = TRANSFORM_TEX((mainuv + _MainOffset.xx), _MainTex);
				half2 offuvB = TRANSFORM_TEX((mainuv - _MainOffset.xx), _MainTex);
				half4 mainoffR = tex2D(_MainTex, offuvR);
				half4 mainoffB = tex2D(_MainTex, offuvB);
				half4 maincolor = tex2D(_MainTex, TRANSFORM_TEX(mainuv, _MainTex));
				half4 main = half4(mainoffR.r, maincolor.g, mainoffB.b, saturate(maincolor.a - _MainAlphaCut));
				#else
				half4 maincolor = tex2D(_MainTex, TRANSFORM_TEX(mainuv, _MainTex));
				half4 main = half4(maincolor.rgb, saturate(maincolor.a - _MainAlphaCut));
				#endif
				
				half4 mainpow = pow(main * i.main_color, _MainContrast);
				
				#if _DISSOVLE_ON
				half disint = i.diss_var.z;

				#if _DISSOLVE
				disint += noise.a;
				#else
				disint += maincolor.a;
				#endif

				#if _DISS_MASK
				disint = disint + (mask.r);
				#endif

				half dissolve = smoothstep(i.diss_var.y, i.diss_var.x, saturate(disint));
				half dissolverange = smoothstep(i.diss_var.y, i.diss_var.x, saturate(disint + _DissolveEdge * 0.2));
				half dissolveedge = dissolverange - dissolve;
				half3 edgecolor = i.edge_color.rgb * dissolveedge;
				half edgealpha = dissolveedge * i.edge_color.a + dissolve;
				#else
				half3 edgecolor = 0;
				half edgealpha = 1;
				#endif

				#if _FRESNEL_ON
				half fresnelalpha = pow(1 - nv, i.posWorld.w);
				fresnelalpha = lerp(fresnelalpha, 1 - fresnelalpha, _Inv_Fresnel);
				half4 fresnelcolor = i.fresnel_color * fresnelalpha;
				#else
				half4 fresnelcolor = 0;
				#endif

				#if _NOISE_GMASK
				half mask02 = noise.a;
				#else
				half mask02 = 1;
				#endif

				half mainalpha = lerp(1, mainpow.a, _MainAlpha);

				#if _ADD_TEX
				half3 finalcolor = mainpow.rgb + edgecolor + maskcolor.rgb * maskcut * _AddTexColor + fresnelcolor.rgb;
				half finalalpha = mainalpha + maskcut * (1 - _MainMask);
				#else
				half3 finalcolor = mainpow.rgb + edgecolor + fresnelcolor.rgb;
				half finalalpha = mainalpha;
				#endif

				#if _FRESNEL_ON & _FRESNELCHANEL_ON
				finalalpha = saturate((finalalpha * fresnelcolor.a) * edgealpha) ;
				#else
				finalalpha = saturate((finalalpha + fresnelcolor.a) * edgealpha) ;
				#endif

				#if _ALPHA_MASK
				finalalpha = saturate(finalalpha * maskcut * mask02);
				#else
				finalalpha = saturate(finalalpha * mask02);
				#endif

				finalcolor += _ColorOffset;
				//finalcolor = finalcolor * finalcolor * (finalcolor * 0.3 + 0.7);

				return half4(finalcolor, finalalpha);
			}
            ENDCG
        }
    }

	FallBack Off
}
