Shader "Unlit/HpBar"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Hp ("Hp", Range(0,1)) = 1
    }
    SubShader
    {
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha  // Alpha blendi


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float  _Hp;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            float InverseLerp(float a, float b, float v) { return (v - a) / (b - a); }

            float4 frag (Interpolators i) : SV_Target
            {
                float4 outColor = float4(1,1,1,1);
                float HpMask = _Hp > i.uv.x;
                outColor = tex2D(_MainTex, float2(_Hp, i.uv.y));
                return outColor * HpMask;

                float tHealthColor = saturate(InverseLerp(0.2, 0.8, _Hp));
                float3 HpColor = lerp(float3(1,0,0), float3(0,1,0), tHealthColor);
                float3 bgColor = float3(0,0,0);
                //clip(HpMask - 0.5);

                //float4 outColor = float4(lerp(bgColor, HpColor, HpMask), HpMask);
                outColor = float4(HpColor, HpMask * 0.5);
                return outColor;
            }
            ENDCG
        }
    }
}
