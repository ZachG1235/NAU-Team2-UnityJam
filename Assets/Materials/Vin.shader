Shader "Custom/VignetteOpposite"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,0.7)
        _Radius("Circle Radius", Float) = 0.7
        _CenterLeft("Left Center", Vector) = (0.35,0.5,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

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
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Radius;
            float4 _CenterLeft;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Adjust for elliptical shape if desired
                float aspectY = 1.3;
                float2 adjUV = float2(uv.x, (uv.y - _CenterLeft.y) / aspectY + _CenterLeft.y);

                float dist = distance(adjUV, _CenterLeft.xy);

                // Instead of fading INWARD, we fade OUTWARD
                float edge = smoothstep(_Radius - 0.75, _Radius, dist);

                // edge = 0 near center (inside circle)
                // edge = 1 at edges (outside circle)
                // We multiply _Color.a by edge to only darken the outside
                return float4(_Color.rgb, _Color.a * edge);
            }
            ENDCG
        }
    }
}
