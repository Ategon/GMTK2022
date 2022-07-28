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


    float pixelPerTaxel;
    Material blitMaterial;
    static Material m_CopyDepthMaterial;
    static Material m_CopyColorMaterial;


    public PixelPerfectRenderPass(PixelPerfectRender.PixelPerfectRenderSettings settings)
    {
        this.renderPassEvent = settings.Event;


        blitMaterial = settings.BlitMaterial;
        if (m_CopyDepthMaterial == null)
        {
            profilingSampler = new ProfilingSampler("PixelPerfectRender");
            m_CopyDepthMaterial = CoreUtils.CreateEngineMaterial("Hidden/Universal Render Pipeline/CopyDepth");
        }
        if (m_CopyColorMaterial == null)
        {
            profilingSampler = new ProfilingSampler("PixelPerfectTransparentRender");
            m_CopyColorMaterial = CoreUtils.CreateEngineMaterial("Hidden/SimpleBlit");
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
    }

    public void ExecuteOpaque(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = SortingCriteria.CommonOpaque;
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
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);

            ScriptableRenderer cameraRenderer = renderingData.cameraData.renderer;
            cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                                    cameraRenderer.cameraDepthTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);

            cmd.Blit(pixelTexID, BuiltinRenderTextureType.CurrentActive, blitMaterial, 0);

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
        CommandBuffer cmd = CommandBufferPool.Get("PixelPerfectTransparentRender");
        using (new ProfilingScope(cmd, profilingSampler))
        {
            cmd.GetTemporaryRT(fullTexID, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Point);
            cmd.GetTemporaryRT(pixelTexID, pixelWidth, pixelHeight, 0, FilterMode.Point);

            cmd.SetRenderTarget(pixelTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(false, true, Color.clear);

            cmd.SetRenderTarget(fullTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                                cameraRenderer.cameraDepthTargetHandle, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(false, true, Color.clear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);

            cmd.Blit(fullTexID, pixelTexID, blitMaterial, 0);

            cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                                cameraRenderer.cameraDepthTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
            cmd.Blit(pixelTexID, cameraRenderer.cameraColorTargetHandle, m_CopyColorMaterial);

            cmd.ReleaseTemporaryRT(pixelTexID);
            cmd.ReleaseTemporaryRT(fullTexID);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
