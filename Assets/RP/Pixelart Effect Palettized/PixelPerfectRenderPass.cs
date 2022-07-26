using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectRenderPass : ScriptableRenderPass
{
    RenderStateBlock m_RenderStateBlock;
    List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
    FilteringSettings m_FilteringSettings;

    static int pixelTexID = Shader.PropertyToID("_PixelTexture");
    static int fullTexID = Shader.PropertyToID("_FullTexture");
    static int pixelDepthID = Shader.PropertyToID("_DepthTex");
    static int pixelInitDepthTexID = Shader.PropertyToID("_InitDepth");
    static int inputDepthTexID = Shader.PropertyToID("_InputDepthTex");
    static int cameraDepthAttachmentID = Shader.PropertyToID("_CameraDepthAttachment");
    static int ZWriteID = Shader.PropertyToID("_ZWrite");
    static int ZTestID = Shader.PropertyToID("_ZTest");
    static int cameraID = Shader.PropertyToID("_CameraColorTexture");

    RTHandle oldColorTarget;
    RTHandle oldDepthTarget;
    RTHandle colorPixels;
    RTHandle depthPixels;
    float pixelPerTaxel;
    Material blitMaterial;
    static Material m_CopyDepthMaterial;


    public PixelPerfectRenderPass(PixelPerfectRender.PixelPerfectRenderSettings settings)
    {
        profilingSampler = new ProfilingSampler("PixelPerfectRender");
        this.renderPassEvent = settings.Event;

        oldColorTarget = colorAttachmentHandle;
        oldDepthTarget = depthAttachmentHandle;
        blitMaterial = settings.BlitMaterial;
        if (m_CopyDepthMaterial == null)
        {
            m_CopyDepthMaterial = CoreUtils.CreateEngineMaterial("Hidden/Universal Render Pipeline/CopyDepth");
        }
        pixelPerTaxel = settings.PixelPerTaxel;

        if (settings.FilterSettings.RenderQueueType == RenderQueueType.Opaque)
        {
            m_FilteringSettings = new FilteringSettings(RenderQueueRange.opaque, settings.FilterSettings.LayerMask);
        }
        else if (settings.FilterSettings.RenderQueueType == RenderQueueType.Transparent)
        {
            m_FilteringSettings = new FilteringSettings(RenderQueueRange.transparent, settings.FilterSettings.LayerMask);
        }

        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
        m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
        m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));

        m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor desc)
    {
        // colorPixels = RTHandles.Alloc(Vector2.one / pixelPerTaxel, filterMode: FilterMode.Point, dimension: TextureDimension.Tex2D, name: "ColorPixelCamera");
        // depthPixels = RTHandles.Alloc(Vector2.one / pixelPerTaxel, depthBufferBits: DepthBits.Depth32, filterMode: FilterMode.Point, dimension: TextureDimension.Tex2D, name: "DepthPixelCamera");
        // ConfigureTarget(colorPixels, depthPixels);


        //RTHandleProperties rtHandleProperties = RTHandles.rtHandleProperties;
        // Debug.Log(colorPixels.rtHandleProperties.rtHandleScale);
        // blitMaterial.SetVector("_RTHandleScale", colorPixels.rtHandleProperties.rtHandleScale);

        // Blitter.Initialize(blitShader, blitShader);

        // int pixelWidth = (int)(desc.width / pixelPerTaxel);
        // int pixelHeight = (int)(desc.height / pixelPerTaxel);

        // cmd.GetTemporaryRT(pixelTexID, pixelWidth, pixelHeight, 0, FilterMode.Point);
        // cmd.GetTemporaryRT(pixelDepthID, pixelWidth, pixelHeight, 24, FilterMode.Point, RenderTextureFormat.Depth);

        // ConfigureTarget(pixelTexID, pixelDepthID);
        // ConfigureClear(ClearFlag.All, Color.clear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (m_FilteringSettings.renderQueueRange == RenderQueueRange.opaque)
        {
            ExecuteOpaque(context, ref renderingData);
        }
        else
        {
            ExecuteTransparent(context, ref renderingData);
        }
        // ExecuteOpaque(context, ref renderingData);
    }

    public void ExecuteOpaque(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = SortingCriteria.CommonTransparent;
        DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
        ref CameraData cameraData = ref renderingData.cameraData;
        Camera camera = cameraData.camera;
        Rect pixelRect = camera.pixelRect;
        int pixelWidth = (int)(camera.pixelWidth / pixelPerTaxel);
        int pixelHeight = (int)(camera.pixelHeight / pixelPerTaxel);
        CommandBuffer cmd = CommandBufferPool.Get("PixelPerfectRender");
        using (new ProfilingScope(cmd, profilingSampler))
        {
            cmd.GetTemporaryRT(pixelTexID, pixelWidth, pixelHeight, 0, FilterMode.Point);
            cmd.GetTemporaryRT(pixelDepthID, pixelWidth, pixelHeight, 32, FilterMode.Point, RenderTextureFormat.Depth);
            cmd.SetRenderTarget(pixelTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                                pixelDepthID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(true, true, Color.clear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

            ScriptableRenderer cameraRenderer = renderingData.cameraData.renderer;
            // ConfigureTarget(cameraRenderer.cameraColorTargetHandle, cameraRenderer.cameraDepthTargetHandle);
            cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                                    cameraRenderer.cameraDepthTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);

            cmd.Blit(pixelTexID, BuiltinRenderTextureType.CurrentActive, blitMaterial, 0);
            // Blit(cmd, new RenderTargetIdentifier(pixelTexID), BuiltinRenderTextureType.CurrentActive, blitMaterial, 0);
            // Blitter.BlitColorAndDepth(cmd, colorPixels, depthPixels, Vector4.one, 0, true);
            cmd.ReleaseTemporaryRT(pixelTexID);
            cmd.ReleaseTemporaryRT(pixelDepthID);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void ExecuteTransparent(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = SortingCriteria.CommonTransparent;
        DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
        ScriptableRenderer cameraRenderer = renderingData.cameraData.renderer;
        ref CameraData cameraData = ref renderingData.cameraData;
        Camera camera = cameraData.camera;
        Rect pixelRect = camera.pixelRect;
        int pixelWidth = (int)(camera.pixelWidth / pixelPerTaxel);
        int pixelHeight = (int)(camera.pixelHeight / pixelPerTaxel);
        CommandBuffer cmd = CommandBufferPool.Get("PixelPerfectRender");
        using (new ProfilingScope(cmd, profilingSampler))
        {
            cmd.GetTemporaryRT(pixelInitDepthTexID, pixelWidth, pixelHeight, 32, FilterMode.Point, RenderTextureFormat.Depth);
            cmd.GetTemporaryRT(pixelTexID, pixelWidth, pixelHeight, 0, FilterMode.Point);
            cmd.GetTemporaryRT(pixelDepthID, pixelWidth, pixelHeight, 32, FilterMode.Point, RenderTextureFormat.Depth);

            bool yflip = (cameraData.IsCameraProjectionMatrixFlipped());
            float flipSign = yflip ? -1.0f : 1.0f;
            Vector4 scaleBiasRt = (flipSign < 0.0f)
                ? new Vector4(flipSign, 1.0f, -10.0f, 1.0f)
                : new Vector4(flipSign, 0.0f, 1.0f, 1.0f);
            cmd.SetGlobalVector(Shader.PropertyToID("_ScaleBiasRt"), scaleBiasRt);
            cmd.SetGlobalTexture(cameraDepthAttachmentID, cameraRenderer.cameraDepthTargetHandle);
            cmd.SetRenderTarget(pixelInitDepthTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_CopyDepthMaterial);
            // cmd.ClearRenderTarget(false, true, Color.clear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            cmd.SetGlobalTexture(cameraDepthAttachmentID, pixelInitDepthTexID);
            cmd.SetRenderTarget(pixelTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                                pixelDepthID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(true, true, Color.clear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

            cmd.SetGlobalTexture(cameraDepthAttachmentID, cameraRenderer.cameraDepthTargetHandle);
            cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                                cameraRenderer.cameraDepthTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);

            cmd.Blit(pixelTexID, cameraRenderer.cameraColorTargetHandle, blitMaterial, 0);

            cmd.ReleaseTemporaryRT(pixelTexID);
            cmd.ReleaseTemporaryRT(pixelDepthID);
            cmd.ReleaseTemporaryRT(pixelInitDepthTexID);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        // Blitter.Cleanup();
        // colorPixels.Release();
        // depthPixels.Release();
        // cmd.ReleaseTemporaryRT(pixelTexID);
        // cmd.ReleaseTemporaryRT(pixelDepthID);
    }
}
