Shader "Custom/Binoculars"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,0.7)
        _Radius("Circle Radius", Float) = 0.5
        _CenterLeft("Left Center", Vector) = (0.35,0.5,0,0)
        _CenterRight("Right Center", Vector) = (0.65,0.5,0,0)
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Radius;
            float4 _CenterLeft;
            float4 _CenterRight;

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

                // Control how tall the circles appear
                float aspectY = 1.3; // smaller = taller (stretches vertically)

                // Scale the Y component before measuring distance
                float2 uvLeft = float2(uv.x, (uv.y - _CenterLeft.y) / aspectY + _CenterLeft.y);
                float2 uvRight = float2(uv.x, (uv.y - _CenterRight.y) / aspectY + _CenterRight.y);

                float distLeft = distance(uvLeft, _CenterLeft.xy);
                float distRight = distance(uvRight, _CenterRight.xy);

                // If inside either circle (ellipse), make transparent
                if (distLeft < _Radius || distRight < _Radius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
