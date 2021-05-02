using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    // Add to the PostProcessOutline class.
    public IntParameter scale = new IntParameter { value = 1 };
    // Add to the PostProcessOutline class.
    public FloatParameter depthThreshold = new FloatParameter { value = 0.2f };
    // Add to the PostProcessOutline class.
    [Range(0, 1)]
    public FloatParameter normalThreshold = new FloatParameter { value = 0.4f };
    // Add to PostProcessOutlineRenderer.
    [Range(0, 1)]
    public FloatParameter depthNormalThreshold = new FloatParameter { value = 0.5f };
    public FloatParameter depthNormalThresholdScale = new FloatParameter { value = 7 };
    public ColorParameter color = new ColorParameter { value = Color.white };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        
 
        // Add to the Render method in the PostProcessOutlineRenderer class, just below var sheet declaration.
        sheet.properties.SetFloat("_Scale", settings.scale);
        // Add below the line setting _Scale.
        sheet.properties.SetFloat("_DepthThreshold", settings.depthThreshold);
        // Add to the Render method in the PostProcessOutlineRenderer class.
        sheet.properties.SetFloat("_NormalThreshold", settings.normalThreshold);
        sheet.properties.SetFloat("_DepthNormalThreshold", settings.depthNormalThreshold);
        sheet.properties.SetFloat("_DepthNormalThresholdScale", settings.depthNormalThresholdScale);
        sheet.properties.SetColor("_Color", settings.color);
        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}