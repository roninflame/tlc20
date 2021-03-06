Shader "Hidden/SgtAtmosphereInner"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_DepthTex("Depth Tex", 2D) = "white" {}
		_HorizonLengthRecip("Horizon Length Recip", Float) = 0
		_InnerRatio("Inner Ratio", Float) = 0
		_InnerScale("Inner Scale", Float) = 1

		_LightingTex("Lighting Tex", 2D) = "white" {}
		_AmbientColor("Ambient Color", Color) = (0,0,0)
		_ScatteringTex("Scattering Tex", 2D) = "white" {}
		_ScatteringMie("Scattering Mie", Float) = 0.5
		_ScatteringRayleigh("Scattering Rayleigh", Float) = 0.5

		_Light1Color("Light 1 Color", Color) = (0,0,0)
		_Light1Scatter("Light 1 Scatter", Color) = (0,0,0)
		_Light1Position("Light 1 Position", Vector) = (0,0,0)
		_Light1Direction("Light 1 Direction", Vector) = (0,0,0)

		_Light2Color("Light 2 Color", Color) = (0,0,0)
		_Light2Scatter("Light 2 Scatter", Color) = (0,0,0)
		_Light2Position("Light 2 Position", Vector) = (0,0,0)
		_Light2Direction("Light 2 Direction", Vector) = (0,0,0)

		_Shadow1Texture("Shadow 1 Texture", 2D) = "white" {}
		_Shadow1Ratio("Shadow 1 Ratio", Float) = 1

		_Shadow2Texture("Shadow 2 Texture", 2D) = "white" {}
		_Shadow2Ratio("Shadow 2 Ratio", Float) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue"           = "Transparent"
			"RenderType"      = "Transparent"
			"IgnoreProjector" = "True"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha, One One
			Cull Back
			Lighting Off
			Offset 0, -1
			ZWrite Off

			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile __ SGT_B // Scattering
			#pragma multi_compile __ LIGHT_0 LIGHT_1 LIGHT_2 // Lights
			#pragma multi_compile __ SHADOW_1 SHADOW_2 // Shadows
			#include "SgtLight.cginc"
			#include "SgtShadow.cginc"
			#include "UnityCG.cginc"

			float4    _Color;
			sampler2D _DepthTex;
			sampler2D _LightingTex;
			float3    _AmbientColor;
			sampler2D _ScatteringTex;
			float     _ScatteringMie;
			float     _ScatteringRayleigh;
			float     _HorizonLengthRecip;
			float     _InnerRatio;
			float     _InnerScale;
			float4x4  _WorldToLocal;
			float4x4  _LocalToWorld;

			struct a2v
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex    : SV_POSITION;
				float2 texcoord0 : TEXCOORD0; // 0..1 depth, 0..1 alt
#if LIGHT_1 || LIGHT_2
				float4 texcoord1 : TEXCOORD1; // xyz = light 1 to vertex/pixel, w = light 1 theta
	#if LIGHT_2
				float4 texcoord2 : TEXCOORD2; // xyz = light 2 to vertex/pixel, w = light 2 theta
	#endif
	#if SGT_B
				float3 texcoord4 : TEXCOORD4; // camera to vertex/pixel
	#endif
#endif
#if SHADOW_1 || SHADOW_2
				float4 texcoord6 : TEXCOORD6; // near world position
#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct f2g
			{
				float4 color : SV_TARGET;
			};

			float GetOutsideDistance(float3 ray, float3 rayD)
			{
				float B = dot(ray, rayD);
				float C = dot(ray, ray) - 1.0f;
				float D = B * B - C;
				return -B - sqrt(max(D, 0.0f));
			}

			void Vert(a2v i, out v2f o)
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 wPos = mul(unity_ObjectToWorld, i.vertex);
				float3 far  = mul(_WorldToLocal, wPos).xyz;
				float3 near = mul(_WorldToLocal, float4(_WorldSpaceCameraPos, 1.0f)).xyz;

				float3 nearFar = far - near;
				float3 dir     = normalize(nearFar);
				float  depthA  = length(nearFar);
				float  depthB  = GetOutsideDistance(near, dir);
				near += dir * max(depthB, 0.0f);
				float depth = length(near - far);

				float alt01 = (length(far) - _InnerRatio) * _InnerScale;

				o.vertex      = UnityObjectToClipPos(i.vertex);
				o.texcoord0.x = depth * _HorizonLengthRecip;
				o.texcoord0.y = 1.0f - alt01;
#if LIGHT_1 || LIGHT_2
				float3 nearD = normalize(near);

				o.texcoord1 = dot(nearD, _Light1Direction) * 0.5f + 0.5f;
	#if LIGHT_2
				o.texcoord2 = dot(nearD, _Light2Direction) * 0.5f + 0.5f;
	#endif
	#if SGT_B // Scattering
				o.texcoord4 = _WorldSpaceCameraPos - wPos.xyz;
				o.texcoord1.xyz = _Light1Position.xyz - _WorldSpaceCameraPos;
		#if LIGHT_2
				o.texcoord2.xyz = _Light2Position.xyz - _WorldSpaceCameraPos;
		#endif
	#endif
#endif
#if SHADOW_1 || SHADOW_2
				o.texcoord6 = mul(_LocalToWorld, wPos);
#endif
			}

			void Frag(v2f i, out f2g o)
			{
				float4 depth = tex2D(_DepthTex, i.texcoord0.xx);
				float4 main  = depth * _Color;

				//main.a *= tex2D(_DepthTex, i.texcoord0.zz).a;
				float alt = smoothstep(0.0f, 1.0f, saturate(i.texcoord0.y));
				main.a *= alt;

				o.color = main;
#if LIGHT_0 || LIGHT_1 || LIGHT_2
				o.color.rgb *= _AmbientColor;
	#if LIGHT_1 || LIGHT_2
				i.texcoord0.x = i.texcoord1.w;

				float4 lighting = tex2D(_LightingTex, i.texcoord0.xx) * main * _Light1Color;
		#if SGT_B // Scattering
				i.texcoord4 = normalize(-i.texcoord4);

				float  angle      = dot(i.texcoord4, normalize(i.texcoord1.xyz));
				//float  phase      = MieRayleighPhase(angle, _ScatteringMie, _ScatteringRayleigh);
				float  phase = RayleighPhase(angle, _ScatteringRayleigh);
				float4 scattering = tex2D(_ScatteringTex, i.texcoord0.xx) * _Light1Scatter * phase;
			#if LIGHT_2
				angle       = dot(i.texcoord4, normalize(i.texcoord2.xyz));
				//phase       = MieRayleighPhase(angle, _ScatteringMie, _ScatteringRayleigh);
				phase = RayleighPhase(angle, _ScatteringRayleigh);
				scattering += tex2D(_ScatteringTex, i.texcoord0.xx) * _Light2Scatter * phase;
			#endif
				scattering *= o.color.a; // Fade scattering out according to optical depth
				scattering *= saturate(1.0f - (o.color + lighting)); // Only scatter into remaining rgba

				lighting += scattering;
		#endif
		#if SHADOW_1 || SHADOW_2
				//lighting *= ShadowColor(i.texcoord6);
				lighting *= lerp(ShadowColor(i.texcoord6), 1.0f, o.color.a);
		#endif
				o.color += lighting;
				o.color.a = saturate(o.color.a);
	#endif
#endif
			}
			ENDCG
		} // Pass
	} // SubShader
} // Shader