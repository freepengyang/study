// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33563,y:32760,varname:node_4795,prsc:2|emission-1090-OUT;n:type:ShaderForge.SFN_Tex2d,id:4448,x:32134,y:32442,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8799,x:32378,y:32590,varname:node_8799,prsc:2|A-4448-A,B-8730-OUT;n:type:ShaderForge.SFN_VertexColor,id:5255,x:32920,y:32904,varname:node_5255,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:9091,x:32148,y:32670,ptovrint:False,ptlb:wenli,ptin:_wenli,varname:node_7599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3948,x:32511,y:32738,varname:node_3948,prsc:2|A-8799-OUT,B-8163-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8163,x:32307,y:32866,ptovrint:False,ptlb:light,ptin:_light,varname:node_1984,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:9849,x:31588,y:32602,varname:node_9849,prsc:2,uv:0;n:type:ShaderForge.SFN_Vector4Property,id:3199,x:31224,y:32721,ptovrint:False,ptlb:XY,ptin:_XY,varname:node_6897,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_ComponentMask,id:8965,x:31392,y:32740,varname:node_8965,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3199-XYZ;n:type:ShaderForge.SFN_ValueProperty,id:9486,x:31244,y:32983,ptovrint:False,ptlb:speed,ptin:_speed,varname:node_5968,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:6800,x:31268,y:33061,varname:node_6800,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6331,x:31481,y:32983,varname:node_6331,prsc:2|A-9486-OUT,B-6800-T;n:type:ShaderForge.SFN_Color,id:3295,x:32739,y:32512,ptovrint:False,ptlb:color,ptin:_color,varname:node_9679,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1204,x:33039,y:32635,varname:node_1204,prsc:2|A-3295-RGB,B-203-OUT;n:type:ShaderForge.SFN_Add,id:2738,x:31868,y:32695,varname:node_2738,prsc:2|A-9849-UVOUT,B-181-OUT;n:type:ShaderForge.SFN_Multiply,id:181,x:31588,y:32788,varname:node_181,prsc:2|A-8965-OUT,B-6331-OUT;n:type:ShaderForge.SFN_Multiply,id:1090,x:33318,y:32712,varname:node_1090,prsc:2|A-1204-OUT,B-5255-RGB,C-5255-A;n:type:ShaderForge.SFN_Power,id:203,x:32821,y:32700,varname:node_203,prsc:2|VAL-3948-OUT,EXP-3533-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3533,x:32696,y:32849,ptovrint:False,ptlb:power,ptin:_power,varname:node_3533,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Vector4Property,id:8832,x:31623,y:33100,ptovrint:False,ptlb:XY1,ptin:_XY1,varname:_XY_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_ComponentMask,id:6073,x:31791,y:33119,varname:node_6073,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8832-XYZ;n:type:ShaderForge.SFN_ValueProperty,id:1481,x:31643,y:33362,ptovrint:False,ptlb:speed1,ptin:_speed1,varname:_speed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:8978,x:31667,y:33440,varname:node_8978,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6726,x:31880,y:33362,varname:node_6726,prsc:2|A-1481-OUT,B-8978-T;n:type:ShaderForge.SFN_Multiply,id:3020,x:31987,y:33167,varname:node_3020,prsc:2|A-6073-OUT,B-6726-OUT;n:type:ShaderForge.SFN_Add,id:5265,x:32199,y:33128,varname:node_5265,prsc:2|A-6172-UVOUT,B-3020-OUT;n:type:ShaderForge.SFN_TexCoord,id:6172,x:31987,y:32989,varname:node_6172,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:9694,x:32438,y:33128,ptovrint:False,ptlb:wenli1,ptin:_wenli1,varname:node_9694,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5265-OUT;n:type:ShaderForge.SFN_Multiply,id:5681,x:32366,y:32965,varname:node_5681,prsc:2|A-9401-R,B-9694-R;n:type:ShaderForge.SFN_Tex2d,id:9401,x:32038,y:32818,ptovrint:False,ptlb:node_9401,ptin:_node_9401,varname:node_9401,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2738-OUT;n:type:ShaderForge.SFN_Multiply,id:8730,x:32605,y:32930,varname:node_8730,prsc:2|A-9091-R,B-5681-OUT;proporder:4448-9091-8163-3295-3199-9486-3533-8832-1481-9694-9401;pass:END;sub:END;*/

Shader "zt/effect_moveadd" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _wenli ("wenli", 2D) = "white" {}
        _light ("light", Float ) = 1
        _color ("color", Color) = (0.5,0.5,0.5,1)
        _XY ("XY", Vector) = (0,0,0,0)
        _speed ("speed", Float ) = 0
        _power ("power", Float ) = 1
        _XY1 ("XY1", Vector) = (0,0,0,0)
        _speed1 ("speed1", Float ) = 0
        _wenli1 ("wenli1", 2D) = "white" {}
        _node_9401 ("node_9401", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _wenli; uniform float4 _wenli_ST;
            uniform float _light;
            uniform float4 _XY;
            uniform float _speed;
            uniform float4 _color;
            uniform float _power;
            uniform float4 _XY1;
            uniform float _speed1;
            uniform sampler2D _wenli1; uniform float4 _wenli1_ST;
            uniform sampler2D _node_9401; uniform float4 _node_9401_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _wenli_var = tex2D(_wenli,TRANSFORM_TEX(i.uv0, _wenli));
                float4 node_6800 = _Time + _TimeEditor;
                float2 node_2738 = (i.uv0+(_XY.rgb.rg*(_speed*node_6800.g)));
                float4 _node_9401_var = tex2D(_node_9401,TRANSFORM_TEX(node_2738, _node_9401));
                float4 node_8978 = _Time + _TimeEditor;
                float2 node_5265 = (i.uv0+(_XY1.rgb.rg*(_speed1*node_8978.g)));
                float4 _wenli1_var = tex2D(_wenli1,TRANSFORM_TEX(node_5265, _wenli1));
                float3 emissive = ((_color.rgb*pow(((_MainTex_var.a*(_wenli_var.r*(_node_9401_var.r*_wenli1_var.r)))*_light),_power))*i.vertexColor.rgb*i.vertexColor.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
