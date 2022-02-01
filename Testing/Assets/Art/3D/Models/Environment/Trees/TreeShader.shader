Shader "Custom/TreeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _TreeSet ("TreeSet", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _TreeSet;

        uniform float _TreeOffset_X = 0.0f;
        uniform float _TreeOffset_Y = 0.0f;

        struct Input
        {
            float2 uv_TreeSet;
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
            fixed4 color = tex2D(_TreeSet, (IN.uv_TreeSet + float2(_TreeOffset_X, _TreeOffset_Y + 1.0)) / 4.0);
            o.Albedo = color.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = color.a;
            clip(color.a - 0.5f);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
