#ifndef PARTICLE_INCLUDES_H
#define PARTICLE_INCLUDES_H

#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
half _MainContrast;
half4 _TintColor;
half _MainAlpha;
half _CVS_UV;
half _MainAlphaCut;
half _MainOffset;
half4 _MainSpeed;
half4 _Intensity;

sampler2D _MaskTex;
float4 _MaskTex_ST;
fixed _CVS_UV2;
fixed _MaskPolarCoord;
fixed _MainMask;
half _MaskAlphaCut;
half4 _AddTexColor;
half4 _MaskSpeed;

sampler2D _NoiseTex;
float4 _NoiseTex_ST;
fixed _CVS_UV3;
fixed _NoisePolarCoord;
half _NoiseInt;
half4 _NoiseSpeed;
fixed _CVS_Z;
half _DissolveInt;
half _DissolveStep;
half _DissolveEdge;
half4 _EdgeColor;

half4 _FresnelColor;
half _Exp;
half _CVS_W;
half _Inv_Fresnel;

half _VertexOffsetInt;
fixed _CVS_V;
half _ColorOffset;

struct appdata_particle
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	half4 texcoord0 : TEXCOORD0;
	half4 texcoord1 : TEXCOORD1;
	float4 color : COLOR;
};

#endif //PARTICLE_INCLUDES_H