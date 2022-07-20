Shader "Unlit/Palettizer Blit"
{
    Properties
    {
        [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
        // [HideInInspector] _DestDepthTex ("Destination (RGB)", 2D) = "black" {}
        _PaletteTex ("Palette (RGB)", 2D) = "white" {}
        _Length ("Length", int) = 0
        [Toggle(_Palettize)] _Palettize ("Palettize", Float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
    }
    SubShader
    {
        pass
        {
            Tags { "RenderType"="Opaque" }
            LOD 200

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest Less

            HLSLPROGRAM
            
            #pragma target 2.0
            #pragma shader_feature _Palettize
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Palette.hlsl"
            
            TEXTURE2D(_MainTex);
            TEXTURE2D(_DepthTex);
            // TEXTURE2D(_DestDepthTex);
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_DepthTex);
            // SAMPLER(sampler_DestDepthTex);
            SAMPLER(sampler_CameraDepthTexture);
         
      
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
            
            float SampleSourceDepth(float2 uv)
            {
                return SAMPLE_DEPTH_TEXTURE(_DepthTex, sampler_DepthTex, uv);
            }

            float SampleDestDepth(float2 uv)
            {
                return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
            }

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
                float dest = SampleDestDepth(input.uv);
                // clip(source - dest);
                
                depth = source;

                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                #if defined(_Palettize)
                color = GetPalettizeColor(color);
                #endif
                // return float4(saturate(dest*10000), 0, 0, 1);
                return color;
            }
            
            ENDHLSL
        }
    }
}
