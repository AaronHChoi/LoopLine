void KeyGray_half(float keyPressed, float3 primaryColor, float3 secondaryColor, out float3 Out)
{
    if (keyPressed > 0.5)
    {
        Out = secondaryColor;
    }
    else
    {
        Out = primaryColor;
    }
}
