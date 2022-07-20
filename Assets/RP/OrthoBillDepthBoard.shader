Shader "Unlit/OrthoBillDepthBoard"
{    
    Properties
    { 
        _MainTex("Texture", 2D) = "white" {}
        _Slope("Slope", Range(0, 1)) = 0
    }

    SubShader
    {        //Opaque
        Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalRenderPipeline" }
        ZWrite On
        Cull Off
        LOD 300
        

        Pass
        {            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"        

            TEXTURE2D(_MainTex);
            #define PIXEL_SAMPLER sampler_point_repeat
            SAMPLER(PIXEL_SAMPLER); 
            TEXTURE2D(_DepthTex);
            SAMPLER(sampler_DepthTex);
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            // UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
                //UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
                // UNITY_DEFINE_INSTANCED_PROP(float, _ScaleX)
                // UNITY_DEFINE_INSTANCED_PROP(float, _ScaleY)
            // UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

            float _Slope;

            struct Attributes
            {
                float4 positionOS : POSITION; 
                float2 uv : TEXCOORD0;                
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float depth : VAR_DEPTH;
                float4 billBoardPositionHCS : SV_POSITION;
            };

            float SampleDepth(float2 uv)
            {
                return SAMPLE_DEPTH_TEXTURE(_DepthTex, sampler_DepthTex, uv);
            }

            float SampleCamDepth(float2 uv)
            {
                return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
            }

            // float ComputeXAngle(float4 tangent)
            // {
            //     half3 worldTangent = UnityObjectToWorldDir(tangent);
            //     half3 viewTangent = mul((float3x3) UNITY_MATRIX_V, worldTangent);
            //     float angleRad = atan2(viewTangent.z, viewTangent.y); 
            //     return angleRad;
            // }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.uv = IN.uv.xy;

                //Billboarding by tralnslating just the origin
                float3 vertexInWorld = mul((float3x3)unity_ObjectToWorld, IN.positionOS.xyz);
                float4 originCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23, 1);
                float4 originOffset = mul(UNITY_MATRIX_V, originCoord);
                float4 vertexInView = originOffset + float4(vertexInWorld, 0);
                OUT.billBoardPositionHCS = mul(UNITY_MATRIX_P, vertexInView);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(originCoord.xyz);
                OUT.depth = saturate((vertexInput.positionCS.z / vertexInput.positionCS.w) + _Slope * IN.uv.y);

                return OUT;
            }
        
            half4 frag(Varyings IN, out float depth : SV_Depth) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, PIXEL_SAMPLER, IN.uv);
                clip(color.a - 0.001);
                
                float screenDepth = SampleDepth(IN.uv);
                //clip(IN.depth - screenDepth);
                depth = IN.depth;
                //return float4(depth, 0, 0, 1);
                return color;
            }
            ENDHLSL
        }

//         Pass
//         {		
//             Name "ShadowCaster"
//             Tags { "LightMode"="ShadowCaster" }
        
//             ZWrite On
//             ZTest LEqual
        
//             HLSLPROGRAM
//             // Required to compile gles 2.0 with standard srp library
//             #pragma prefer_hlslcc gles
//             #pragma exclude_renderers d3d11_9x gles
//             //#pragma target 4.5
        
//             // Material Keywords
//             #pragma shader_feature _ALPHATEST_ON
//             #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
        
//             // GPU Instancing
//             #pragma multi_compile_instancing
//             #pragma multi_compile _ DOTS_INSTANCING_ON
                    
//             #pragma vertex ShadowPassVertex
//             #pragma fragment ShadowPassFragment
            
//             #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
//             #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

//             Varyings vert(Attributes input) {
//                 Varyings output;
//                 UNITY_SETUP_INSTANCE_ID(input);
            
//                 // Example Displacement
//                 input.positionOS += float4(0, _SinTime.y, 0, 0);
            
//                 output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
//                 output.positionCS = GetShadowPositionHClip(input);
//                 return output;
//             }
// }
        
//             ENDHLSL
//         }
    }
}
