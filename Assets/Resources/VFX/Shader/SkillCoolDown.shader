Shader "Custom/SkillCoolDown"
{
    Properties
    {
        _MainTex ("主纹理", 2D) = "white" {}
        _Color ("颜色", Color) = (1,1,1,1)
        _Cooldown ("冷却进度", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"      = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color  : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Cooldown;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                o.color  = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                float2 center = float2(0.5, 0.5);
                float2 dir    = i.uv - center;

                float angle = atan2(dir.x, -dir.y);
                if (angle < 0) angle += 6.283185; 

                float normalizedAngle = angle / 6.283185;

                if (normalizedAngle < _Cooldown)
                    discard;

                return col;
            }
            ENDCG
        }
    }
}