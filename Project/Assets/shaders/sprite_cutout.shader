Shader "Custom/Sprite Cutout"
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
            "Queue"="Geometry" 
            "IgnoreProjector"="True" 
            "RenderType"="Opaque" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	

            #pragma multi_compile _ PIXELSNAP_ON
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;

			};

			sampler2D _MainTex;
			
            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;

                #if defined(PIXELSNAP_ON)
                o.vertex = UnityPixelSnap (v.vertex);
                #endif

                return o;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D (_MainTex, i.uv);

                clip(col.a - 0.5);

				return col;
			}
			ENDCG
		}
      
	}
}
