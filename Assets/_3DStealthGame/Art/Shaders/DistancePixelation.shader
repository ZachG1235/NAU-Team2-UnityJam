
Shader "Custom/DistancePixelation"
{
    Properties
    {
        _NearPixelSize ("Near Pixel Size", Range(1, 16)) = 8
        _FarPixelSize ("Far Pixel Size", Range(1, 8)) = 1
        _NearDistance ("Near Distance", Float) = 10
        _FarDistance ("Far Distance", Float) = 40
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _NearPixelSize;
                float _FarPixelSize;
                float _NearDistance;
                float _FarDistance;
            CBUFFER_END

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

                // Get the distance to the visible scene surface.
                float rawDepth = SampleSceneDepth(uv);
                float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                // 0 near, 1 far.
                float distanceFactor = saturate(
                    (eyeDepth - _NearDistance) /
                    max(_FarDistance - _NearDistance, 0.001)
                );

                // Nearby = larger pixel blocks.
                // Distant = smaller pixel blocks.
                float pixelSize = lerp(
                    _NearPixelSize,
                    _FarPixelSize,
                    distanceFactor
                );

                float2 textureSize = _BlitTexture_TexelSize.zw;
                float2 blockCount = max(
                    floor(textureSize / max(pixelSize, 1.0)),
                    float2(1.0, 1.0)
                );

                float2 pixelUV =
                    (floor(uv * blockCount) + 0.5) / blockCount;

                return SAMPLE_TEXTURE2D_X_LOD(
                    _BlitTexture,
                    sampler_LinearClamp,
                    pixelUV,
                    _BlitMipLevel
                );
            }

            ENDHLSL
        }
    }
}
