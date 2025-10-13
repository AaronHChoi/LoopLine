Shader "Custom/MeltDissolve"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise (RGB)", 2D) = "gray" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _DistortionStrength ("Distortion Strength", Range(0,1)) = 0.1
        _FadeColor ("Fade Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Name "MeltDissolve"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _NoiseTex;

            float _DissolveAmount;
            float _DistortionStrength;
            float4 _FadeColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = v.vertex; // Already in clip space if using full-screen quad with [-1,1] verts
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 noise = tex2D(_NoiseTex, uv * 3.0).rg - 0.5;
                uv += noise * _DistortionStrength;

                float4 col = tex2D(_MainTex, uv);

                float gradient = 1.0 - i.uv.y;
                float dissolve = saturate((gradient - _DissolveAmount) * 10.0);

                col.rgb = lerp(_FadeColor.rgb, col.rgb, dissolve);
                col.a = dissolve;

                return col;
            }
            ENDHLSL
        }
    }
}
