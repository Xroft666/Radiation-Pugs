﻿Shader "Custom/Highlight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "black" {}
		_OccludeMap ("Occlusion Map", 2D) = "black" {}
	}
	
	SubShader {

		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		
		
		// OVERLAY GLOW
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;             
			
				fixed4 frag(v2f_img IN) : COLOR 
				{
					return tex2D (_MainTex, IN.uv) + tex2D(_OccludeMap, IN.uv);
				}
			ENDCG
		}
		
		// OVERLAY SOLID
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;
				
				fixed4 _Color;
			
				fixed4 frag(v2f_img IN) : COLOR 
				{
					fixed4 mCol = tex2D (_MainTex, IN.uv);
					fixed oCol = tex2D (_OccludeMap, IN.uv).r;
					
					fixed solid = step (1.0 - _Color.a, oCol);
					return mCol + solid * fixed4(_Color.rgb, 1.0);
				}
			ENDCG
		}
	
		
		// OCCLUSION
		
		Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;
			
				fixed4 frag(v2f_img IN) : COLOR 
				{
                    fixed4 mainCol = tex2D (_MainTex, IN.uv);
                    fixed4 occlCol = tex2D(_OccludeMap, IN.uv);

                    return mainCol - occlCol;
				}
			ENDCG
		}
		
		Pass {
        
           	Tags {"RenderType"="Opaque"}
        	ZWrite On
        	ZTest LEqual
        	Fog { Mode Off }
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float4 vert(float4 v:POSITION) : POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }

            fixed4 frag() : COLOR {
                return fixed4(0, 1, 0, 1);
            }

            ENDCG
        }	
    	
        Pass {        	
           	Tags {"Queue"="Transparent"}
            Cull Back
            Lighting Off
            ZWrite Off
            ZTest LEqual
            ColorMask RGBA
            Blend OneMinusDstColor One

        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _CameraDepthTexture;
            
            struct v2f {
                float4 vertex : POSITION;
                float4 projPos : TEXCOORD1;
            };
     
            v2f vert( float4 v : POSITION ) {        
                v2f o;
                o.vertex = mul( UNITY_MATRIX_MVP, v );
                o.projPos = ComputeScreenPos(o.vertex);             
                return o;
            }

            fixed frag( v2f i ) : COLOR {          
                float depthVal = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
                float zPos = i.projPos.z;
                
                return step( zPos, depthVal );         
            }
            ENDCG
        }

        Pass {
        
            Tags {"RenderType"="Opaque"}
            ZWrite On
            ZTest LEqual
            Fog { Mode Off }
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

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
            fixed4 _PugColor;

            sampler2D _MainTex;



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : COLOR {

                fixed4 col = tex2D(_MainTex, i.uv);

                return step(0.5, col.a) * _PugColor;
            }

            ENDCG
        }  
	} 
}
