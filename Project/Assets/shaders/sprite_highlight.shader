Shader "Custom/Sprite Highlight"
{
	Properties
	{
	    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

        Pass {
            Tags {"RenderType"="Opaque"}
            ZWrite On
            ZTest LEqual
            Fog { Mode Off }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ PIXELSNAP_ON

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

            fixed4 _Color;
            sampler2D _MainTex;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

                #if defined(PIXELSNAP_ON)
                o.vertex = UnityPixelSnap (v.vertex);
                #endif

                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : COLOR {

                fixed4 col = tex2D(_MainTex, i.uv);

                return step(0.5, col.a) * _Color;
            }

            ENDCG
	}
    }
    Fallback Off
}
