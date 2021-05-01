Shader "Unlit/BodyShader"
{
Properties {
    _Color ("_Color", Color) = (1,1,1,1)
}
 
SubShader {
    Color [_Color]
    Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (_Color.b > 0) {
                    return _Color;
                } else {
                    return _Color * ((sin(_Time.y * 10) * 0.5) + 1);    
                }
            }
            ENDCG
    }
}
}
