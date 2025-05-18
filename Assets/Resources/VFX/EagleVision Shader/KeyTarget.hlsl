void KeyTarget_half(
    float keyPressed,
    float3 primaryColor,
    float3 secondaryColor,
    float3 primaryTexture,
    float3 secondaryTexture,
    float emission,
    out float3 Out)
{
    float3 primaryResult = length(primaryTexture) > 0.01 ? primaryTexture : primaryColor;
    float3 secondaryResult = length(secondaryTexture) > 0.01 ? secondaryTexture : secondaryColor;

    if (keyPressed > 0.5)
    {
        Out = secondaryResult * emission;
    }
    else
    {
        Out = primaryResult;
    }
}
