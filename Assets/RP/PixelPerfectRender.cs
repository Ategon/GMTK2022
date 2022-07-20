using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class PixelPerfectRender : ScriptableRendererFeature
{
    [System.Serializable]
    public class PixelPerfectRenderSettings
    {
        public RenderPassEvent Event = RenderPassEvent.BeforeRenderingTransparents;
        public LayerMask LayerMask = 0;

        [Range(1, 100)]
        public int PixelPerTaxel = 16;
        public Material BlitMaterial;
        // public Material SimpleBlitMaterial;
    }

    public PixelPerfectRenderSettings settings = new PixelPerfectRenderSettings();

    PixelPerfectRenderPass pass;

    public override void Create()
    {
        pass = new PixelPerfectRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
