Shader "Universal Render Pipeline/VertexColoredTransparent" {
    Properties {
        _Color ("Color", Color) = (1,1,1,0.)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            fixed4 _Color;
            sampler2D _MainTex;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            half _Glossiness;
            half _Metallic;

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 c = tex2D(_MainTex, IN.uv) * IN.color;
                fixed4 finalColor = c * _Color;
                return finalColor;
            }
            ENDCG
        }
    }
}
