Shader "Hidden/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_MaskTex ("_MaskTex", 2D) = "white" {}
        _Size("Size", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Vetical"
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
            float4 _MaskTex_TexelSize;

            //float4 _MainTex_TexelSize;
            float _Size;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MaskTex, i.uv);
                fixed4 color = tex2D(_MainTex, i.uv);
                fixed sum = mask.a; 

                    
                    sum += tex2D(_MaskTex, i.uv + _MaskTex_TexelSize.xy * float2(0, _Size)).a;
                    sum += tex2D(_MaskTex, i.uv + _MaskTex_TexelSize.xy * float2(0,-_Size)).a;
                    sum += tex2D(_MaskTex, i.uv + _MaskTex_TexelSize.xy * float2(_Size, 0)).a;
                    sum += tex2D(_MaskTex, i.uv + _MaskTex_TexelSize.xy * float2(-_Size,0)).a;


                    fixed4 finalColor = fixed4(color.r, color.g, color.b, saturate(sum));
                    //clip(finalColor.a - 0.001);
                return finalColor;
            }
            ENDCG
        }
    }
}
