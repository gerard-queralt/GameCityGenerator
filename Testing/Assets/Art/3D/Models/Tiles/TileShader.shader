Shader "Custom/TileShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _TileSet_LV0 ("TileSet 0", 2D) = "white" {}
        _TileSet_LV1 ("TileSet 1", 2D) = "white" {}
        _TileSet_LV2 ("TileSet 2", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _TileSet_LV0;
        sampler2D _TileSet_LV1;
        sampler2D _TileSet_LV2;

        uniform float _TileOffset_LV0_X = 0.0f;
        uniform float _TileOffset_LV0_Y = 0.0f;
        uniform float _TileOffset_LV1_X = 0.0f;
        uniform float _TileOffset_LV1_Y = 0.0f;
        uniform float _TileOffset_LV2_X = 0.0f;
        uniform float _TileOffset_LV2_Y = 0.0f;

        struct Input
        {
            float2 uv_TileSet_LV0;
            float2 uv2_TileSet_LV1;
            float2 uv3_TileSet_LV2;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 color0 = tex2D (_TileSet_LV0, (IN.uv_TileSet_LV0 + float2(_TileOffset_LV0_X, _TileOffset_LV0_Y)) / 4.0);
            fixed4 color1 = tex2D (_TileSet_LV1, (IN.uv2_TileSet_LV1 + float2(_TileOffset_LV1_X, _TileOffset_LV1_Y)) / 4.0);
            fixed4 color2 = tex2D (_TileSet_LV2, (IN.uv3_TileSet_LV2 + float2(_TileOffset_LV2_X, _TileOffset_LV2_Y)) / 4.0);
            o.Albedo = color2.a * color2 + (1.0 - color2.a) * (color1.a * color1 + (1.0 - color1.a) * color0);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
