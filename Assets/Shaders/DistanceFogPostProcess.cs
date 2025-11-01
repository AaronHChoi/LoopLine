// DistanceFogPostProcess.cs
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.RenderPipelines.Core.Runtime; // Correct namespace for CoreUtils in Unity 6

// This attribute makes it show up in the Volume "Add Override" menu
[Serializable, VolumeComponentMenu("Post-processing/Custom/Distance Fog")]
public sealed class DistanceFogPostProcess : CustomPostProcessVolumeComponent
{
    // This connects to the "Fog Color" property in your Shader Graph
    [Tooltip("The color of the fog.")]
    public ColorParameter fogColor = new ColorParameter(Color.magenta); // Default value

    // This connects to the "Fog Start" property in your Shader Graph
    [Tooltip("Distance from camera to start the fog.")]
    public ClampedFloatParameter fogStart = new ClampedFloatParameter(10f, 0f, 1000f);

    // This connects to the "Fog End" property in your Shader Graph
    [Tooltip("Distance from camera where fog is at full strength.")]
    public ClampedFloatParameter fogEnd = new ClampedFloatParameter(100f, 0f, 1000f);

    private Material m_Material;

    // --- WE HAVE REMOVED THE 'IsActive()' METHOD ENTIRELY ---
    // The compiler errors imply it's not an abstract method we need to implement.
    // The effect will just be "on" as long as it's enabled in the Volume.

    // This is where HDRP finds your shader
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;
    
    // Links the shader and C# script
    public override void Setup()
    {
        // REMINDER: Make sure this path matches your Shader Graph's "Path"
        string shaderPath = "Shader Graphs/FullscreenFog_Shader"; 
        
        if (Shader.Find(shaderPath) != null)
            m_Material = new Material(Shader.Find(shaderPath));
        else
            Debug.LogError($"[DistanceFogPostProcess] Unable to find shader '{shaderPath}'. Check Graph Inspector path.");
    }

    // This is the Render method the compiler DEMANDED
    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null) // This check keeps it safe
            return;

        m_Material.SetColor("_FogColor", fogColor.value);
        m_Material.SetFloat("_FogStart", fogStart.value);
        m_Material.SetFloat("_FogEnd", fogEnd.value);
        
        // Renders the effect
        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}