#ifndef PALETTE_INCLUDED
#define PALETTE_INCLUDED

TEXTURE2D(_PaletteTex);
#define PIXEL_SAMPLER sampler_point_repeat
SAMPLER(PIXEL_SAMPLER);

int _Length;
float4 _PaletteTex_TexelSize;

float modulo(float a, float2 b)
{
    //b.x = 1.0/b
    //b.y = b
    return a - (floor((a * b.x)) * b.y);
}

float2 GetPaletteUV(int index)
{
    float2 uvWithTopLeftOringin; 
    uvWithTopLeftOringin.x = (modulo(index, _PaletteTex_TexelSize.xz) + 0.5) * _PaletteTex_TexelSize.x;
    uvWithTopLeftOringin.y = 1.0 - (floor(index * _PaletteTex_TexelSize.x) + 0.5) * _PaletteTex_TexelSize.y;
    return uvWithTopLeftOringin;
}

half4 GetColorFromPalette(int index)
{
    float2 uv = GetPaletteUV(index);
    return SAMPLE_TEXTURE2D(_PaletteTex, PIXEL_SAMPLER, uv);
}

float GetColorDistance(half3 in1, half3 in2)
{
    float satDistance = distance(in1, in2);
    half3 col1 = RgbToHsv(in1);
    half3 col2 = RgbToHsv(in2);
    float hueDistance = distance(col1, col2);

    return lerp(satDistance, hueDistance, 0.5);

    // return rgbDeltaE00(in1, in2);
}

half4 GetPalettizeColor(half4 color)
{
    half4 paletteColor = color;
    
    if (_Length > 0)
    {
        paletteColor = GetColorFromPalette(0);
        float smallestDistance = GetColorDistance(color.xyz, paletteColor.xyz);
        float dis;
        for (int i = 1; i < _Length; i++)
        {
            half4 nextColor = GetColorFromPalette(i);
            dis = GetColorDistance(color.xyz, nextColor.xyz);
            if (dis < smallestDistance)
            {
                paletteColor = nextColor;
                smallestDistance = dis;
            }
        }
        paletteColor = half4(paletteColor.xyz, color.w);
    }
    return paletteColor;
}
#endif