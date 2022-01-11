Shader "Skybox/Cosmos Animated"{
	Properties {
		[Hdr]_Color ("Color", Color) = (3, 5, 6, 3)
		[Header(Main Texture)]
		[NoScaleOffset]_MainTex ("", Cube) = "white" {}
		_Colorize ("Colorize", Range(0,2)) = 0.6

		[Space(20)]
		[Header(Detail 1)]
		[NoScaleOffset]_Detail1 ("", 2D) = "white" {}
		_D1I ("Intensity", Range(0,2)) = 1
		_D1Scale ("Scale", FLOAT) = 2
		_D1Distort ("Distortion", FLOAT) = 0.1
        _D1Speed ("Speed", FLOAT) = 5
		[Space(20)]
		[Header(Detail 2)]
		[NoScaleOffset]_Detail2 ("", 2D) = "white" {}
		_D2I ("Intensity", Range(0,2)) = 0.8
		_D2Scale ("Scale", FLOAT) = 5
		_D2Distort ("Distortion", FLOAT) = 0.1
        _D2Speed ("Speed", FLOAT) = 3
	}

	SubShader {
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
		Cull Off ZWrite Off Fog { Mode Off }

		Pass {
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata_t {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
            	UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
            	UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}
			
			fixed4 _Color;
			samplerCUBE _MainTex;
			half _Colorize;
			sampler2D _Detail2;
			half _D2I;
			half _D2Scale;
			half _D2Speed;
			half _D2Distort;
			sampler2D _Detail1;
			half _D1I;
			half _D1Scale;
			half _D1Speed;
			half _D1Distort;

			fixed4 triplanar(sampler2D tex,float3 coord, half scale){
				fixed4 c = 
				     tex2D(tex, coord.xy * scale) * abs(coord.z);
				c += tex2D(tex, coord.yz * scale) * abs(coord.x);
				c += tex2D(tex, coord.zx * scale) * abs(coord.y);
				return c/(abs(coord.x) + abs(coord.y) + abs(coord.z));
			}

			fixed4 saturate(fixed4 col,half sat){
				return lerp(
					(col.r + col.g + col.b)/3,
					col, sat
				);
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 cosmos = texCUBE(_MainTex, i.texcoord)*2-1;

				fixed blend = frac(_Time.x*_D1Speed);
				fixed detail1_0 = triplanar(_Detail1, i.texcoord + cosmos.xyz*_D1Distort*blend, _D1Scale).r;
				fixed detail1_1 = triplanar(_Detail1, i.texcoord + cosmos.xyz*_D1Distort*frac(blend+0.5), _D1Scale).r;
				fixed detail1 = lerp(detail1_0,detail1_1, abs(blend*2-1));

				blend = frac(_Time.x*_D2Speed);
				fixed detail2_0 = triplanar(_Detail2, i.texcoord + cosmos.xyz * _D2Distort * blend, _D2Scale).r;
				fixed detail2_1 = triplanar(_Detail2, i.texcoord + cosmos.xyz * _D2Distort * frac(blend+0.5), _D2Scale).r;
				fixed detail2 = lerp(detail2_0,detail2_1, abs(blend*2-1));

				return fixed4(
					saturate(cosmos*0.5+0.5,_Colorize) * 
					_Color.rgb * 
					lerp(1,detail1,_D1I)*
					lerp(1,detail2,_D2I),1);
			}
			ENDCG 
		}
	} 	
	Fallback Off
}

