Shader "Unlit/Palettizer Blit"
{
    Properties
    {
        [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
        
        _PaletteTex ("Palette (RGB)", 2D) = "white" {}
        _Length ("Length", int) = 0
        [Toggle(_Palettize)] _Palettize ("Palettize", Float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", Float) = 0
        [Toggle(_ExternalZTest)] _ExternalZTest("Manual ZTest", Float) = 0.0
    }
    SubShader
    {
        pass
        {
            Tags { "RenderType"="Opaque" }
            LOD 200

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]

            HLSLPROGRAM
            
            #pragma target 2.0
            #pragma shader_feature _Palettize
            #pragma shader_feature _ExternalZTest
            // #pragma shader_feature _Transparent
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "LABColorSpace.hlsl"
            #include "Palette.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_DepthTex);
            SAMPLER(sampler_DepthTex);

            float SampleSourceDepth(float2 uv)
            {
                return SAMPLE_DEPTH_TEXTURE(_DepthTex, sampler_DepthTex, uv);
            }
            
        #if defined(_ExternalZTest)
            #if defined(_DEPTH_MSAA_2)
                #define MSAA_SAMPLES 2
            #elif defined(_DEPTH_MSAA_4)
                #define MSAA_SAMPLES 4
            #elif defined(_DEPTH_MSAA_8)
                #define MSAA_SAMPLES 8
            #else
                #define MSAA_SAMPLES 1
            #endif

            #if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
            #define DEPTH_TEXTURE_MS(name, samples) Texture2DMSArray<float, samples> name
            #define DEPTH_TEXTURE(name) TEXTURE2D_ARRAY_FLOAT(name)
            #define LOAD(uv, sampleIndex) LOAD_TEXTURE2D_ARRAY_MSAA(_CameraDepthAttachment, uv, unity_StereoEyeIndex, sampleIndex)
            #define SAMPLE(uv) SAMPLE_TEXTURE2D_ARRAY(_CameraDepthAttachment, sampler_CameraDepthAttachment, uv, unity_StereoEyeIndex).r
            #else
            #define DEPTH_TEXTURE_MS(name, samples) Texture2DMS<float, samples> name
            #define DEPTH_TEXTURE(name) TEXTURE2D_FLOAT(name)
            #define LOAD(uv, sampleIndex) LOAD_TEXTURE2D_MSAA(_CameraDepthAttachment, uv, sampleIndex)
            #define SAMPLE(uv) SAMPLE_DEPTH_TEXTURE(_CameraDepthAttachment, sampler_CameraDepthAttachment, uv)
            #endif

            #if MSAA_SAMPLES == 1
                DEPTH_TEXTURE(_CameraDepthAttachment);
                SAMPLER(sampler_CameraDepthAttachment);
            #else
                DEPTH_TEXTURE_MS(_CameraDepthAttachment, MSAA_SAMPLES);
                float4 _CameraDepthAttachment_TexelSize;
            #endif

            #if UNITY_REVERSED_Z
                #define DEPTH_DEFAULT_VALUE 1.0
                #define DEPTH_OP min
            #else
                #define DEPTH_DEFAULT_VALUE 0.0
                #define DEPTH_OP max
            #endif

            float SampleDestDepth(float2 uv)
            {
            #if MSAA_SAMPLES == 1
                return SAMPLE(uv);
            #else
                int2 coord = int2(uv * _CameraDepthAttachment_TexelSize.zw);
                float outDepth = DEPTH_DEFAULT_VALUE;

                UNITY_UNROLL
                for (int i = 0; i < MSAA_SAMPLES; ++i)
                    outDepth = DEPTH_OP(LOAD(coord, i), outDepth);
                return outDepth;
            #endif
            }
        #endif

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = input.uv;
                return output;
            }
            
            half4 frag (Varyings input, out float depth : SV_Depth) : SV_Target 
            {
                float source = SampleSourceDepth(input.uv);
            #if defined(_ExternalZTest)
                float dest = SampleDestDepth(input.uv);
                clip(source - dest);
            #endif
                depth = source;

                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
            #if defined(_Palettize)
                color = GetPalettizeColor(color);
            #endif
            
                return color;
            }
            
            ENDHLSL
        }
    }
}
