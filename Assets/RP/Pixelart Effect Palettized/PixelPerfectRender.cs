using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectRender : ScriptableRendererFeature
{
    [System.Serializable]
    public class PixelPerfectRenderSettings
    {
        public RenderPassEvent Event = RenderPassEvent.BeforeRenderingTransparents;
        public FilterSettings FilterSettings = new FilterSettings();
        [Range(1, 100)]
        public float PixelPerTaxel = 4;
        public Material BlitMaterial;
    }

    [System.Serializable]
    public class FilterSettings
    {
        public RenderQueueType RenderQueueType;
        public LayerMask LayerMask;

        public FilterSettings()
        {
            RenderQueueType = RenderQueueType.Opaque;
            LayerMask = 0;
        }
    }


    public PixelPerfectRenderSettings settings = new PixelPerfectRenderSettings();

    PixelPerfectRenderPass pass;

    public override void Create()
    {
        pass = new PixelPerfectRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // pass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
        renderer.EnqueuePass(pass);
    }
}
