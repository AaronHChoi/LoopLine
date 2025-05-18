void KeyTarget_half(
    float keyPressed,
    float3 primaryColor,
    float3 secondaryColor,
    float3 primaryTexture,
    float3 secondaryTexture,
    out float3 Out)
{
    float3 primaryResult = length(primaryTexture) > 0.01 ? primaryTexture : primaryColor;
    float3 secondaryResult = length(secondaryTexture) > 0.01 ? secondaryTexture : secondaryColor;

    Out = keyPressed > 0.5 ? secondaryResult : primaryResult;
}
