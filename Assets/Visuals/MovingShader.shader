Shader "Unlit/MovingShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1)
        _A("A", Float) = 25
        _B("B", FLoat) = 0.04
    }
        SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
        }

        GrabPass{}

        Pass
        {
            //Cull Off
            //ZWrite Off
            //Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 uvgrab :TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float4 _MainTex_TexelSize, _MainTex_ST;
            float _A, _B;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                fixed4 vertexUV = o.vertex;

#if UNITY_UV_STARTS_AT_TOP
                vertexUV.y *= -sign(_MainTex_TexelSize.y);
#endif

                o.uvgrab.xy = (float2(vertexUV.x, vertexUV.y) + vertexUV.w) * 0.5;
                //o.uvgrab.xy = o.uvgrab.xy + cos((o.uvgrab.xy * 25) + _Time.y) * 0.04;
                o.uvgrab.zw = vertexUV.zw;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //float offset = cos(_Time.y * 5) * 0.5;
                i.uvgrab.x = i.uvgrab.x + cos((i.uvgrab.x * _A) + _Time.y) * _B;
                i.uvgrab.y = i.uvgrab.y + cos((i.uvgrab.y * _A) + _Time.y) * _B;
                // sample the texture
                fixed4 col = tex2Dproj(_GrabTexture, i.uvgrab);

                //float3 normal = normalize(i.worldNormal);
                //float3 viewDir = normalize(i.viewDir);
                //float4 rimDot = 1 - dot(viewDir, normal);
                //rimDot = smoothstep(0, 1, rimDot);
                //col.a = rimDot;
                col.r = (col.r + _Color.r) * 0.5f;
                col.g = (col.g + _Color.g) * 0.5f;
                col.b = (col.b + _Color.b) * 0.5f;

                return col;
            }
            ENDCG
        }
    }
}
