Shader "Hidden/ApplyMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_MaskTex ("Mask", 2D) = "white" {}
        //_Min("Min", Float) = 0
        //_//Max("Max", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform sampler2D _MaskTex;
            float _Min;
            float _Max;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                col.a = mask.a;
                //col.a = smoothstep(_Min, _Max, mask.a);
                //clip(col.a - 0.001);
                return col;
            }
            ENDCG
        }
    }
}
