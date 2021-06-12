Shader "Unlit/BackgroundShader" {
	Properties {
		[NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#define PI 3.1415

			struct MeshData {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			Interpolators vert (MeshData v) {
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (Interpolators i) : SV_Target {
				float yCoord = i.uv.y * 8;
				yCoord += _Time.y * 0.2;
				yCoord = sin( (frac(yCoord)+0.25) * PI * 2) + 0.5;
				

				// return float4(yCoord,abs(yCoord),0,1);

				float2 coord = float2(i.uv.x + yCoord * 0.01, i.uv.y);

				fixed4 col = tex2D(_MainTex, coord);
				return col;
			}
			ENDCG
		}
	}
}
