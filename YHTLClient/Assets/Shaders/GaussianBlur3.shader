Shader "Effect/GaussianBlur3" 
{
    Properties
    {
        //主纹理
        _MainTex("Base (RGB)", 2D) = "white" {}
        //模糊系数
        _BlurSize("Blur Size", float) = 1.0
    }
    

    SubShader 
    {
        CGINCLUDE
        #include "Unitycg.cginc"
        sampler2D _MainTex;
        uniform half4 _MainTex_TexelSize;
        uniform float _BlurSize;
        //模糊层级的系数
        static const half weight[4] = {0.0205, 0.0855, 0.232, 0.324};
        //纹理采样偏移值
        static const half4 coordOffset = half4(1.0, 1.0, -1.0, -1.0);
        //片面结构体
        struct v2f
        {
            //顶点位置
            float4 pos:SV_POSITION;
            //主纹理
            half2 uv:TEXCOORD0;
            //偏移纹理数组
            half4 uvoff[3]:TEXCOORD1;
        };
        
        v2f vertBlurVertical(appdata_img v) 
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord.xy;
            half2 offs = _MainTex_TexelSize.xy * half2(0,1) *  _BlurSize;
            o.uvoff[0] = v.texcoord.xyxy + offs.xyxy * coordOffset * 3;
            o.uvoff[1] = v.texcoord.xyxy + offs.xyxy * coordOffset * 2;
            o.uvoff[2] = v.texcoord.xyxy + offs.xyxy * coordOffset;

            return o;
        }
        
        v2f vertBlurHorizontal(appdata_img v) 
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord.xy;
            half2 offs = _MainTex_TexelSize.xy * half2(1,0) *  _BlurSize;
            o.uvoff[0] = v.texcoord.xyxy + offs.xyxy * coordOffset * 3;
            o.uvoff[1] = v.texcoord.xyxy + offs.xyxy * coordOffset * 2;
            o.uvoff[2] = v.texcoord.xyxy + offs.xyxy * coordOffset;

            return o;
        }
        fixed4 fragBlur(v2f i) : SV_Target 
        {
            //获取主纹理RGB色值
            fixed4 c = tex2D(_MainTex, i.uv) * weight[3];
            //主纹理叠加偏移纹理
            for(int idx = 0; idx < 3; idx ++)
            {
                c += tex2D(_MainTex, i.uvoff[idx].xy) * weight[idx];
                c += tex2D(_MainTex, i.uvoff[idx].zw) * weight[idx];
            }
            return c;
        } 
        ENDCG
        Cull Off ZWrite Off
        //通道1
        pass
        {
            ZTest Always 
            Name "GUASSIAN_BLUR_VERTICAL"
            CGPROGRAM
            #pragma vertex vertBlurVertical
            #pragma fragment fragBlur
            ENDCG
        }
        //通道2
        pass
        {
            ZTest Always 
            Name "GUASSIAN_BLUR_HORIZONTAL"
            CGPROGRAM
            #pragma vertex vertBlurHorizontal
            #pragma fragment fragBlur
            ENDCG
        }
        
        
    }
    Fallback Off
}