Shader "Custom/Invisible"
{
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        ColorMask 0
        ZWrite Off
        Pass {}
    }
}
