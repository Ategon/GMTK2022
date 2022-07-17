using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using System.IO;

public class PixelPerfectRenderPass : ScriptableRenderPass
{
    ProfilingSampler m_ProfilingSampler;
    PipeLineContext pipeLineContext;

    public PixelPerfectRenderPass(PixelPerfectRender.PixelPerfectRenderSettings settings)
    {
        m_ProfilingSampler = new ProfilingSampler("PixelPerfectRender");
        this.renderPassEvent = settings.Event;
        pipeLineContext = new PipeLineContext(settings);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("PixelPerfectRender");
        DrawingSettingsBuilder drawingSettingsBuilder = new DrawingSettingsBuilder(this, pipeLineContext);
        RenderPassContext renderPassContext = new RenderPassContext(context, cmd, drawingSettingsBuilder, ref renderingData);
        PipeLine pipeLine = new PipeLine(pipeLineContext, renderPassContext, ref renderingData);
        using (new ProfilingScope(cmd, m_ProfilingSampler))
        {
            pipeLine.SetPixelRenderTextureDimensions();
            pipeLine.MakeAndSetPixelRenderTextureToTarget();
            pipeLine.RenderToTarget();
            pipeLine.SetCameraRenderTargetAndBlit();
            pipeLine.Clean();
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
    }



    private class DrawingSettingsBuilder
    {
        ScriptableRenderPass renderPass;
        List<ShaderTagId> shaderTagIdList;
        SortingCriteria sortingCriteria;

        public DrawingSettingsBuilder(ScriptableRenderPass renderPass, PipeLineContext pipeLineContext)
        {
            this.renderPass = renderPass;
            this.shaderTagIdList = pipeLineContext.shaderTagIdList;
            this.sortingCriteria = pipeLineContext.sortingCriteria;
        }

        public void Build(ref RenderingData renderingData, out DrawingSettings drawingSettings)
        {
            drawingSettings = renderPass.CreateDrawingSettings(shaderTagIdList, ref renderingData, sortingCriteria);
        }
    }

    private class RenderPassContext
    {
        private DrawingSettings drawingSettings;

        public ScriptableRenderContext context { private set; get; }
        public CommandBuffer cmd { private set; get; }
        public ref DrawingSettings drawingSettingsRef { get { return ref drawingSettings; } }

        public RenderPassContext(ScriptableRenderContext context, CommandBuffer cmd, DrawingSettingsBuilder drawingSettingsBuilder, ref RenderingData renderingData)
        {
            drawingSettingsBuilder.Build(ref renderingData, out this.drawingSettings);
            this.context = context;
            this.cmd = cmd;
        }
    }

    private class PipeLineContext
    {
        private RenderStateBlock renderStateBlock;
        private FilteringSettings filteringSettings;
        private PixelPerfectRender.PixelPerfectRenderSettings settings;

        public ref RenderStateBlock renderStateBlockRef { get { return ref renderStateBlock; } }
        public ref FilteringSettings filteringSettingsRef { get { return ref filteringSettings; } }
        public List<ShaderTagId> shaderTagIdList { private set; get; }
        public ref PixelPerfectRender.PixelPerfectRenderSettings settingsRef { get { return ref settings; } }
        public SortingCriteria sortingCriteria { private set; get; }


        public PipeLineContext(PixelPerfectRender.PixelPerfectRenderSettings settings)
        {
            this.settings = settings;

            filteringSettings = new FilteringSettings(RenderQueueRange.opaque, settings.LayerMask);

            shaderTagIdList = new List<ShaderTagId>();
            shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            shaderTagIdList.Add(new ShaderTagId("LightweightForward"));
            shaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));

            renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
            this.sortingCriteria = SortingCriteria.CommonTransparent;
        }
    }

    private class PipeLine
    {
        static int pixelTexID = Shader.PropertyToID("_PixelTexture");
        static int pixelDepthID = Shader.PropertyToID("_DepthTex");
        static int cameraID = Shader.PropertyToID("_CameraColorTexture");
        static int cameraDepthID = Shader.PropertyToID("_DestDepthTex");


        PipeLineContext pipeLineContext;
        RenderPassContext renderPassContext;

        Camera camera;
        ScriptableRenderer cameraRenderer;
        CullingResults cullResults;
        DrawingSettings drawingSettings;

        int pixelPerTaxel;
        int renderTextureWidth;
        int renderTextureHeight;
        //Material paletteMaterial;
        Material blitMaterial;
        // Material simpleBlitMaterial;

        public PipeLine(PipeLineContext pipeLineContext, RenderPassContext renderPassContext, ref RenderingData renderingData)
        {
            this.pipeLineContext = pipeLineContext;
            this.renderPassContext = renderPassContext;
            this.camera = renderingData.cameraData.camera;
            this.cameraRenderer = renderingData.cameraData.renderer;
            this.cullResults = renderingData.cullResults;

            this.pixelPerTaxel = pipeLineContext.settingsRef.PixelPerTaxel;
            this.blitMaterial = pipeLineContext.settingsRef.BlitMaterial;
            // this.simpleBlitMaterial = pipeLineContext.settingsRef.SimpleBlitMaterial;
        }

        public void SetPixelRenderTextureDimensions()
        {
            renderTextureWidth = (int)(camera.pixelWidth / pixelPerTaxel);
            renderTextureHeight = (int)(camera.pixelHeight / pixelPerTaxel);
        }

        public void MakeAndSetPixelRenderTextureToTarget()
        {
            CommandBuffer cmd = renderPassContext.cmd;


            cmd.GetTemporaryRT(pixelTexID, renderTextureWidth, renderTextureHeight, 0, FilterMode.Point);
            cmd.GetTemporaryRT(pixelDepthID, renderTextureWidth, renderTextureHeight, 24, FilterMode.Point, RenderTextureFormat.Depth);
            cmd.SetRenderTarget(pixelTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                                pixelDepthID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(true, true, Color.clear);
            renderPassContext.context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        public void RenderToTarget()
        {
            renderPassContext.context.DrawRenderers(cullResults, ref renderPassContext.drawingSettingsRef, ref pipeLineContext.filteringSettingsRef, ref pipeLineContext.renderStateBlockRef);
        }

        public void SetCameraRenderTargetAndBlit()
        {
            CommandBuffer cmd = renderPassContext.cmd;
            cmd.BeginSample("Pixel Blit");
            cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle.nameID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                                cameraRenderer.cameraDepthTargetHandle.nameID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
            // cmd.SetRenderTarget(cameraRenderer.cameraColorTargetHandle.nameID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
            // cmd.Blit(new RenderTargetIdentifier(pixelTexID), BuiltinRenderTextureType.CurrentActive, blitMaterial);
            // cmd.SetGlobalTexture(cameraDepthID, cameraRenderer.cameraDepthTargetHandle);
            // cmd.SetGlobalTexture(pixelDepthID, cameraRenderer.cameraDepthTargetHandle);
            // cmd.SetGlobalTexture(cameraDepthID, cameraDepthID);
            // cmd.SetGlobalTexture(cameraDepthID, cameraRenderer.cameraDepthTargetHandle);
            // cmd.SetGlobalTexture(pixelDepthID, pixelDepthID);
            renderPassContext.context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            cmd.Blit(new RenderTargetIdentifier(pixelTexID), cameraRenderer.cameraColorTargetHandle.nameID, blitMaterial);
            cmd.EndSample("Pixel Blit");
            // renderPassContext.context.ExecuteCommandBuffer(cmd);
            // cmd.Clear();
        }

        public void Clean()
        {
            CommandBuffer cmd = renderPassContext.cmd;
            cmd.ReleaseTemporaryRT(pixelTexID);
            cmd.ReleaseTemporaryRT(pixelDepthID);

            renderPassContext.context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
    }
}
