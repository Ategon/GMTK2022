#ifndef __LAB_COLORSPACE__
#define __LAB_COLORSPACE__


/* 
 * ##############################################
 * # SECTION 0 : RGB to XYZ to LAB Color Spaces #
 * ##############################################
 * Copied from: https://gist.github.com/mattatz/44f081cac87e2f7c8980
 */

/*
 * Conversion between RGB and LAB colorspace.
 * Import from flowabs glsl program : https://code.google.com/p/flowabs/source/browse/glsl/?r=f36cbdcf7790a28d90f09e2cf89ec9a64911f138
 */

float3 rgb2xyz( float3 c ) {
    float3 tmp;
    tmp.x = ( c.r > 0.04045 ) ? pow( ( c.r + 0.055 ) / 1.055, 2.4 ) : c.r / 12.92;
    tmp.y = ( c.g > 0.04045 ) ? pow( ( c.g + 0.055 ) / 1.055, 2.4 ) : c.g / 12.92,
    tmp.z = ( c.b > 0.04045 ) ? pow( ( c.b + 0.055 ) / 1.055, 2.4 ) : c.b / 12.92;
    const float3x3 mat = float3x3(
		0.4124, 0.3576, 0.1805,
        0.2126, 0.7152, 0.0722,
        0.0193, 0.1192, 0.9505 
	);
    return 100.0 * mul(tmp, mat);
}

float3 xyz2lab( float3 c ) {
    float3 n = c / float3(95.047, 100, 108.883);
    float3 v;
    v.x = ( n.x > 0.008856 ) ? pow( n.x, 1.0 / 3.0 ) : ( 7.787 * n.x ) + ( 16.0 / 116.0 );
    v.y = ( n.y > 0.008856 ) ? pow( n.y, 1.0 / 3.0 ) : ( 7.787 * n.y ) + ( 16.0 / 116.0 );
    v.z = ( n.z > 0.008856 ) ? pow( n.z, 1.0 / 3.0 ) : ( 7.787 * n.z ) + ( 16.0 / 116.0 );
    return float3(( 116.0 * v.y ) - 16.0, 500.0 * ( v.x - v.y ), 200.0 * ( v.y - v.z ));
}

float3 rgb2lab( float3 c ) {
    float3 lab = xyz2lab( rgb2xyz( c ) );
    return float3( lab.x / 100.0, 0.5 + 0.5 * ( lab.y / 127.0 ), 0.5 + 0.5 * ( lab.z / 127.0 ));
}

float3 lab2xyz( float3 c ) {
    float fy = ( c.x + 16.0 ) / 116.0;
    float fx = c.y / 500.0 + fy;
    float fz = fy - c.z / 200.0;
    return float3(
         95.047 * (( fx > 0.206897 ) ? fx * fx * fx : ( fx - 16.0 / 116.0 ) / 7.787),
        100.000 * (( fy > 0.206897 ) ? fy * fy * fy : ( fy - 16.0 / 116.0 ) / 7.787),
        108.883 * (( fz > 0.206897 ) ? fz * fz * fz : ( fz - 16.0 / 116.0 ) / 7.787)
    );
}

float3 xyz2rgb( float3 c ) {
	const float3x3 mat = float3x3(
        3.2406, -1.5372, -0.4986,
        -0.9689, 1.8758, 0.0415,
        0.0557, -0.2040, 1.0570
	);
    float3 v = mul(c / 100.0, mat);
    float3 r;
    r.x = ( v.r > 0.0031308 ) ? (( 1.055 * pow( v.r, ( 1.0 / 2.4 ))) - 0.055 ) : 12.92 * v.r;
    r.y = ( v.g > 0.0031308 ) ? (( 1.055 * pow( v.g, ( 1.0 / 2.4 ))) - 0.055 ) : 12.92 * v.g;
    r.z = ( v.b > 0.0031308 ) ? (( 1.055 * pow( v.b, ( 1.0 / 2.4 ))) - 0.055 ) : 12.92 * v.b;
    return r;
}

float3 lab2rgb( float3 c ) {
    return xyz2rgb( lab2xyz( float3(100.0 * c.x, 2.0 * 127.0 * (c.y - 0.5), 2.0 * 127.0 * (c.z - 0.5)) ) );
}

/* 
 * #################################
 * # SECTION 1 : CIE ~ CIEDE delta #
 * #################################
 * Equation from: https://en.wikipedia.org/wiki/Color_difference
 */

#define SQUARE(x) ((x) * (x))
#define POW_SEVEN(x) ((x) * (x) * (x) * (x) * (x) * (x) * (x))
//Constances [using graphic arts weighting]
#define CIE_KL 1.0
#define CIE_KC 1.0
#define CIE_KH 1.0
#define CIE_K1 0.0045
#define CIE_K2 0.0015
#define CIE_25POW7 6103515625.0
//Reciprocal Constances
#define CIE_INV_KL 1.0
#define CIE_INV_KC 1.0
#define CIE_INV_KH 1.0
#define CIE_INV_K1 222.222222222
#define CIE_INV_K2 666.666666667

// Changes -pi to pi range to 0 to 360
// pi is 180 going counter clock wise
// -pi is 180 going clock wise (or -180 clock wise)
// This function maps the angles to on go
// counter clock wise starting from 0 to 360;
float fixDgrees(float rad)
{
    float2 three_sixty = float2(0.00277777777, 360.0);
    float degree = degrees(rad) + 360;
    return degree - (floor((degree * three_sixty.x)) * three_sixty.y);
}

float labDeltaEab(float3 lab1, float3 lab2)
{
    float deltaL2 = SQUARE(lab2.x - lab1.x);
    float deltaA2 = SQUARE(lab2.y - lab1.y);
    float deltaB2 = SQUARE(lab2.z - lab1.z);
    float deltaEab = sqrt(deltaL2 + deltaA2 + deltaB2);
    return deltaEab;
}

float labDeltaE94(float3 lab1, float3 lab2)
{
    float deltaL = lab1.x - lab2.x;
    
    float c1 = sqrt(SQUARE(lab1.y) + SQUARE(lab1.z));
    float c2 = sqrt(SQUARE(lab2.y) + SQUARE(lab2.z));
    float deltaCab = c1 - c2;

    float deltaA = lab1.y - lab2.y;
    float deltaB = lab1.z - lab2.z;
    float deltaHab = sqrt(SQUARE(deltaA) + SQUARE(deltaB) - SQUARE(deltaCab));

    float sl = 1;
    float sc = 1 + (CIE_K1 * c1);
    float sh = 1 + (CIE_K2 * c1);
    float inv_sl = 1;
    float inv_sc = 1/sc;
    float inv_sh = 1/sh;

    float product1 = deltaL * CIE_INV_KL * inv_sl;
    float product2 = deltaCab * CIE_INV_KC * inv_sc;
    float product3 = deltaHab * CIE_INV_KH * inv_sh;
    float deltaE94 = sqrt(SQUARE(product1) + SQUARE(product2) + SQUARE(product3));
    return deltaE94;
}

float labDeltaE00(float3 lab1, float3 lab2)
{
    float deltaLprim = lab2.x - lab1.x;
    float _L = (lab1.x + lab2.x)*(0.5);
    
    float c1 = sqrt(SQUARE(lab1.y) + SQUARE(lab1.z));
    float c2 = sqrt(SQUARE(lab2.y) + SQUARE(lab2.z));
    float _C = (c1 + c2)*(0.5);

    float _C7 = POW_SEVEN(_C);
    
    float aCoefficient = sqrt( _C7 / (_C7 + CIE_25POW7));
    float a1prim = lab1.y + ((lab1.y * 0.5) * (1 - aCoefficient));
    float a2prim = lab2.y + ((lab2.y * 0.5) * (1 - aCoefficient));

    float c1prim = sqrt(SQUARE(a1prim) + SQUARE(lab1.z));
    float c2prim = sqrt(SQUARE(a2prim) + SQUARE(lab2.z));
    float deltaCprim = c2prim - c1prim;
    float _Cprim = (c1prim + c2prim) * 0.5;

    float h1prim = fixDgrees(atan2(lab1.y, a1prim));
    float h2prim = fixDgrees(atan2(lab2.y, a2prim));
    float absDeltahprim = abs(h1prim - h2prim);
    float deltahprim = 0;

    if (absDeltahprim <= 180)
    {
        deltahprim = h2prim - h1prim;
    }else if (absDeltahprim > 180 && h2prim <= h1prim)
    {
        deltahprim = h2prim - h1prim + 360;
    }else if (absDeltahprim > 180 && h2prim > h1prim)
    {
        deltahprim = h2prim - h1prim - 360;
    }
    deltahprim = radians(deltahprim);

    float deltaHprim = 2 * sqrt(c1prim * c2prim) * sin(deltahprim * 0.5);
    float _Hprim = 0;
    float totalhprim = h1prim + h2prim;

    if (absDeltahprim <= 180)
    {
        _Hprim = totalhprim * 0.5;
    }else if (absDeltahprim > 180 && totalhprim < 360)
    {
        _Hprim = (totalhprim + 360) * 0.5;
    }else if (absDeltahprim > 180 && totalhprim >= 360)
    {
        _Hprim = (totalhprim - 360) * 0.5;
    }

    float t = 1 - (0.17 * cos(radians(_Hprim - 30))) + (0.24 * cos(radians(2 * _Hprim))) + (0.32 * cos(radians(3 * _Hprim + 6))) - (0.2 * cos(radians(4 * _Hprim - 63)));
    float L50 =_L - 50.0;
    float squareL50 = SQUARE(L50);
    float sl = 1 + ((0.015 * squareL50)/(sqrt(20 + squareL50)));
    float sc = 1 + 0.045 * _Cprim;
    float sh = 1 + 0.015 * _Cprim * t;
    float _Hprim275 = ((_Hprim - 275) * 0.04);
    float rt = -2 * sqrt(aCoefficient) * sin(radians(60 * exp(-SQUARE(_Hprim275))));

    float inv_sl = 1/sl;
    float inv_sc = 1/sc;
    float inv_sh = 1/sh;

    float product1 = deltaLprim * CIE_INV_KL * inv_sl;
    float product2 = deltaCprim * CIE_INV_KC * inv_sc;
    float product3 = deltaHprim * CIE_INV_KH * inv_sh;
    float term2 = rt * (deltaCprim * CIE_INV_KC * inv_sc) * (deltaHprim * CIE_INV_KH * inv_sh);
    float deltaE00 = sqrt(SQUARE(product1) + SQUARE(product2) + SQUARE(product3) + term2);
    return deltaE00;
}

float rgbDeltaE00(float3 rgb1, float3 rgb2)
{
    float3 lab1 = rgb2lab(rgb1);
    float3 lab2 = rgb2lab(rgb2);
    return labDeltaE00(lab1, lab2);
}

#endif