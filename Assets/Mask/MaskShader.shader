Shader "Smatch/MaskShader"
{
    Properties {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]_MaskBlur("Mask Blur", Float) = 1.0
        [HideInInspector]_MaskSquareness("Mask Squareness", Float) = 0.5
        [HideInInspector]_MaskWidth("Mask Width", Float) = 1.0
        [HideInInspector]_MaskHeight("Mask Height", Float) = 1.0
        [HideInInspector]_MaskRotation("Mask Rotation", Float) = 0.0
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define PI 3.14159265359

            sampler2D _MainTex;
            float _MaskBlur;
            float _MaskSquareness;
            float _MaskWidth;
            float _MaskHeight;
            float _MaskRotation;

            float2 rotate (float2 st, float t)
            {
                float c = cos(t), s = sin(t);
                return mul(float2x2(c, -s, s, c), st);
            }

            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 p = i.uv * 2 - 1;

                // Distort
                p.x *= _ScreenParams.x / _ScreenParams.y;
                p = rotate(p, _MaskRotation / 360.0 * PI);
                p.x /= _MaskWidth;
                p.y /= _MaskHeight;
                p.x /= _ScreenParams.x / _ScreenParams.y;
                p = pow(abs(p), max(_MaskSquareness - 1.0, 1.0));

                float l = length(p);
                float mask = 1.0 - smoothstep(1.0 / (_MaskBlur + 1.0), 1., l);

                return fixed4(tex2D(_MainTex, i.uv).rgb * mask, 1.0);
            }

            ENDCG
        }
    }
}
