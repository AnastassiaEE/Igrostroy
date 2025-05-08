Shader "Custom/RadialTransparencyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 0.5, 0, 1)
        _WaveRadius ("Wave Radius", Range(0, 1)) = 0.0
        _WaveWidth ("Wave Width", Range(0.01, 1)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _WaveRadius;
            float _WaveWidth;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                float inner = _WaveRadius - (_WaveWidth * 0.5);
                float outer = _WaveRadius + (_WaveWidth * 0.5);

                // Гладкий градиент по кольцу (альфа по гауссу)
                float wave = smoothstep(inner, _WaveRadius, dist) * (1.0 - smoothstep(_WaveRadius, outer, dist));

                // Повышаем контраст — волна будет мягче по краям, но ярче в центре
                wave = pow(wave, 0.8); // попробуй 0.5–0.8 для мягкости

                float4 texColor = tex2D(_MainTex, i.uv);
                float4 finalColor = texColor * _Color;
                finalColor.a *= wave;

                return finalColor;
            }
            ENDCG
        }
    }
}
