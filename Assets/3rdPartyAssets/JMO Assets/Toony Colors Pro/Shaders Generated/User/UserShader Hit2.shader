// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Toony Colors Pro 2/User/Shader Hit2"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		[HDR] _BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		[TCP2HeaderHelp(Main Directional Light)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		[TCP2HeaderHelp(Other Lights)]
		_RampThresholdOtherLights ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothingOtherLights ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCountOtherLights ("Bands Count", Range(1,20)) = 4
		[Space]
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] [HDR] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		 _SpecularColor2 ("Specular Color Float", Float) = 1
		 [NoScaleOffset] _SpecularColor1 ("Specular Color Texture", 2D) = "white" {}
		_SpecularShadowAttenuation ("Specular Shadow Attenuation", Float) = 0.25
		_SpecularSmoothness ("Smoothness", Float) = 0.2
		_SpecularToonBands ("Specular Bands", Float) = 3
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[NoScaleOffset] _EmissionText ("Emission Texture", 2D) = "white" {}
		 [TCP2ColorNoAlpha] [HDR] _EmissionColor ("Emission Color", Color) = (1,1,1,1)
		[TCP2Separator]
		_FresnelMin ("Fresnel Min", Range(0,2)) = 0
		_FresnelMax ("Fresnel Max", Range(0,2)) = 1.5
		
		[TCP2HeaderHelp(MatCap)]
		[Toggle(TCP2_MATCAP)] _UseMatCap ("Enable MatCap", Float) = 0
		[NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "black" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		 [NoScaleOffset] _MatCapColor2 ("MatCap Color Texture", 2D) = "white" {}
		 [TCP2ColorNoAlpha] _MatCapColor1 ("MatCap Color Color", Color) = (1,1,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[Toggle(_NORMALMAP)] _UseNormalMap ("Enable Normal Mapping", Float) = 0
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1
		[TCP2Separator]
		
		[Toggle(TCP2_TEXTURED_THRESHOLD)] _UseTexturedThreshold ("Enable Textured Threshold", Float) = 0
		_StylizedThreshold ("Stylized Threshold", 2D) = "gray" {}
		[TCP2Separator]
		
		[TCP2HeaderHelp(Sketch)]
		[Toggle(TCP2_SKETCH)] _UseSketch ("Enable Sketch Effect", Float) = 0
		_SketchTexture ("Sketch Texture", 2D) = "black" {}
		_SketchMin ("Sketch Min", Range(0,1)) = 0
		_SketchMax ("Sketch Max", Range(0,1)) = 1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Wind)]
		[Toggle(TCP2_WIND)] _UseWind ("Enable Wind", Float) = 0
		_WindDirection ("Direction", Vector) = (1,0,0,0)
		_WindStrength ("Strength", Range(0,20)) = 1
		_WindSpeed ("Speed", Range(0,20)) = 2.5
		
		[TCP2HeaderHelp(Outline)]
		_OutlineWidth ("Width", Range(0,8)) = 1
		_OutlineMinWidth ("Min Width", Range(0,20)) = 1
		[HDR] _OutlineColorVertex ("Color", Color) = (0,0,0,1)
		// Outline Normals
		[TCP2MaterialKeywordEnumNoPrefix(Regular, _, Vertex Colors, TCP2_COLORS_AS_NORMALS, Tangents, TCP2_TANGENT_AS_NORMALS, UV1, TCP2_UV1_AS_NORMALS, UV2, TCP2_UV2_AS_NORMALS, UV3, TCP2_UV3_AS_NORMALS, UV4, TCP2_UV4_AS_NORMALS)]
		_NormalsSource ("Outline Normals Source", Float) = 0
		[TCP2MaterialKeywordEnumNoPrefix(Full XYZ, TCP2_UV_NORMALS_FULL, Compressed XY, _, Compressed ZW, TCP2_UV_NORMALS_ZW)]
		_NormalsUVType ("UV Data Type", Float) = 0
		[TCP2Separator]

		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Opaque"
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#if UNITY_VERSION >= 202020
			#define URP_10_OR_NEWER
		#endif

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						TEXTURE2D(tex); SAMPLER(sampler##tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							TEXTURE2D(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			SAMPLE_TEXTURE2D(tex, sampler##samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	SAMPLE_TEXTURE2D_LOD(tex, sampler##samplertex, coord, lod)

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		// Uniforms

		// Shader Properties
		TCP2_TEX2D_WITH_SAMPLER(_BumpMap);
		TCP2_TEX2D_WITH_SAMPLER(_BaseMap);
		TCP2_TEX2D_WITH_SAMPLER(_EmissionText);
		TCP2_TEX2D_WITH_SAMPLER(_MatCapColor2);
		TCP2_TEX2D_WITH_SAMPLER(_StylizedThreshold);
		TCP2_TEX2D_WITH_SAMPLER(_SketchTexture);
		TCP2_TEX2D_WITH_SAMPLER(_SpecularColor1);

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float _WindSpeed;
			float4 _WindDirection;
			float _WindStrength;
			float _OutlineMinWidth;
			float _OutlineWidth;
			half4 _OutlineColorVertex;
			float _BumpScale;
			float4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _EmissionColor;
			fixed4 _MatCapColor;
			fixed4 _MatCapColor1;
			float4 _StylizedThreshold_ST;
			float _RampThreshold;
			float _RampSmoothing;
			float _BandsCount;
			float4 _SketchTexture_ST;
			float4 _SketchTexture_TexelSize;
			float _SketchMin;
			float _SketchMax;
			fixed4 _SColor;
			fixed4 _HColor;
			float _SpecularSmoothness;
			float _SpecularToonBands;
			float _SpecularShadowAttenuation;
			half4 _SpecularColor;
			float _SpecularColor2;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _BandsCountOtherLights;
			float _FresnelMin;
			float _FresnelMax;
			sampler2D _MatCapTex;
		CBUFFER_END

		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		// Outline Include
		HLSLINCLUDE

		#pragma multi_compile_fog

		struct appdata_outline
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			#if TCP2_UV1_AS_NORMALS
			float4 texcoord0 : TEXCOORD0;
		#elif TCP2_UV2_AS_NORMALS
			float4 texcoord1 : TEXCOORD1;
		#elif TCP2_UV3_AS_NORMALS
			float4 texcoord2 : TEXCOORD2;
		#elif TCP2_UV4_AS_NORMALS
			float4 texcoord3 : TEXCOORD3;
		#endif
			fixed4 vertexColor : COLOR;
			float4 tangent : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f_outline
		{
			float4 vertex : SV_POSITION;
			float4 vcolor : TEXCOORD0;
			float4 pack1 : TEXCOORD1; /* pack1.xyz = worldPos  pack1.w = fogFactor */
			float4 pack2 : TEXCOORD2; /* pack2.xyz = worldNormal  pack2.w = ndl */
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f_outline vertex_outline (appdata_outline v)
		{
			v2f_outline output = (v2f_outline)0;

			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_TRANSFER_INSTANCE_ID(v, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			float3 worldNormalUv = mul(unity_ObjectToWorld, float4(v.normal, 1.0)).xyz;
			// Shader Properties Sampling
			float __windTimeOffset = ( v.vertexColor.g );
			float __windSpeed = ( _WindSpeed );
			float __windFrequency = ( 1.0 );
			float4 __windSineScale2 = ( float4(2.3,1.7,1.4,1.2) );
			float __windSineStrength2 = ( .6 );
			float3 __windDirection = ( _WindDirection.xyz );
			float3 __windMask = ( v.vertexColor.rrr );
			float __windStrength = ( _WindStrength );
			float __outlineLightingWrapFactorVertex = ( 1.0 );
			float __outlineMinWidth = ( _OutlineMinWidth );
			float __outlineWidth = ( _OutlineWidth );
			float4 __outlineColorVertex = ( _OutlineColorVertex.rgba );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			#if defined(TCP2_WIND)
			// Wind Animation
			float windTimeOffset = __windTimeOffset;
			float windSpeed = __windSpeed;
			float3 windFrequency = worldPos.xyz * __windFrequency;
			float windPhase = (_Time.y + windTimeOffset) * windSpeed;
			float3 windFactor = sin(windPhase + windFrequency);
			float4 windSin2scale = __windSineScale2;
			float windSin2strength = __windSineStrength2;
			windFactor += sin(windPhase.xxx * windSin2scale.www + windFrequency * windSin2scale.xyz) * windSin2strength;
			float3 windDir = normalize(__windDirection);
			float3 windMask = __windMask;
			float windStrength = __windStrength;
			worldPos.xyz += windDir * windFactor * windMask * windStrength;
			#endif
			v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
			output.pack1.xyz = worldPos;
			output.pack2.xyz = worldNormalUv;
			float3 objSpaceLight = normalize(mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz);
			half lightWrap = __outlineLightingWrapFactorVertex;
			half ndl = max(0, (dot(v.normal.xyz, objSpaceLight.xyz) + lightWrap) / (1 + lightWrap));
			output.pack2.w = ndl;
		
		#ifdef TCP2_COLORS_AS_NORMALS
			//Vertex Color for Normals
			float3 normal = (v.vertexColor.xyz*2) - 1;
		#elif TCP2_TANGENT_AS_NORMALS
			//Tangent for Normals
			float3 normal = v.tangent.xyz;
		#elif TCP2_UV1_AS_NORMALS || TCP2_UV2_AS_NORMALS || TCP2_UV3_AS_NORMALS || TCP2_UV4_AS_NORMALS
			#if TCP2_UV1_AS_NORMALS
				#define uvChannel texcoord0
			#elif TCP2_UV2_AS_NORMALS
				#define uvChannel texcoord1
			#elif TCP2_UV3_AS_NORMALS
				#define uvChannel texcoord2
			#elif TCP2_UV4_AS_NORMALS
				#define uvChannel texcoord3
			#endif
		
			#if TCP2_UV_NORMALS_FULL
			//UV for Normals, full
			float3 normal = v.uvChannel.xyz;
			#else
			//UV for Normals, compressed
			#if TCP2_UV_NORMALS_ZW
				#define ch1 z
				#define ch2 w
			#else
				#define ch1 x
				#define ch2 y
			#endif
			float3 n;
			//unpack uvs
			v.uvChannel.ch1 = v.uvChannel.ch1 * 255.0/16.0;
			n.x = floor(v.uvChannel.ch1) / 15.0;
			n.y = frac(v.uvChannel.ch1) * 16.0 / 15.0;
			//- get z
			n.z = v.uvChannel.ch2;
			//- transform
			n = n*2 - 1;
			float3 normal = n;
			#endif
		#else
			float3 normal = v.normal;
		#endif
		
		#if TCP2_ZSMOOTH_ON
			//Correct Z artefacts
			normal = UnityObjectToViewPos(normal);
			normal.z = -_ZSmooth;
		#endif
			float size = 1;
		
		#if !defined(SHADOWCASTER_PASS)
			output.vertex = UnityObjectToClipPos(v.vertex.xyz);
			normal = mul(unity_ObjectToWorld, float4(normal, 0)).xyz;
			float2 clipNormals = normalize(mul(UNITY_MATRIX_VP, float4(normal,0)).xy);
			half2 screenRatio = half2(1.0, _ScreenParams.x / _ScreenParams.y);
			half2 outlineWidth = max(
				(__outlineMinWidth * output.vertex.w) / (_ScreenParams.xy / 2.0),
				(__outlineWidth / 100) * screenRatio
			);
			output.vertex.xy += clipNormals.xy * outlineWidth;
			
		#else
			v.vertex = v.vertex + float4(normal,0) * __outlineWidth * size * 0.01;
		#endif
		
			output.vcolor.xyzw = __outlineColorVertex;
			float4 clipPos = output.vertex;

			// Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.pack1.w = ComputeFogFactor(output.vertex.z);

			return output;
		}

		float4 fragment_outline (v2f_outline input) : SV_Target
		{

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

			float3 positionWS = input.pack1.xyz;
			float3 normalWS = input.pack1.xyz;

			// Shader Properties Sampling
			float4 __outlineColor = ( float4(1,1,1,1) );

			half4 outlineColor = __outlineColor * input.vcolor.xyzw;
			outlineColor *= input.pack2.w;
			outlineColor.rgb = MixFog(outlineColor.rgb, input.pack1.w);

			return outlineColor;
		}

		ENDHLSL
		// Outline Include End
		Pass
		{
			Name "Main"
			Tags
			{
				"LightMode"="UniversalForward"
			}
			ZWrite On

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			#pragma shader_feature_local _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK

			// -------------------------------------
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature_local_fragment TCP2_SPECULAR
			#pragma shader_feature_local TCP2_MATCAP
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local_fragment TCP2_SKETCH
			#pragma shader_feature_local_fragment TCP2_TEXTURED_THRESHOLD
			#pragma shader_feature_local_vertex TCP2_WIND

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
				float2 uvLM         : TEXCOORD1;
				half4 vertexColor   : COLOR;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex output / fragment input
			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float4 worldPosAndFog : TEXCOORD0;
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord    : TEXCOORD1; // compute shadow coord per-vertex for the main light
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				half3 vertexLights : TEXCOORD2;
			#endif
				float4 screenPosition : TEXCOORD3;
				float4 pack1 : TEXCOORD4; /* pack1.xyz = tangent  pack1.w = fogFactor */
				float3 pack2 : TEXCOORD5; /* pack2.xyz = bitangent */
				float4 pack3 : TEXCOORD6; /* pack3.xy = texcoord0  pack3.zw = uvLM */
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings Vertex(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				// Texture Coordinates
				output.pack3.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				// Shader Properties Sampling
				float __windTimeOffset = ( input.vertexColor.g );
				float __windSpeed = ( _WindSpeed );
				float __windFrequency = ( 1.0 );
				float4 __windSineScale2 = ( float4(2.3,1.7,1.4,1.2) );
				float __windSineStrength2 = ( .6 );
				float3 __windDirection = ( _WindDirection.xyz );
				float3 __windMask = ( input.vertexColor.rrr );
				float __windStrength = ( _WindStrength );

				output.pack3.zw = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				#if defined(TCP2_WIND)
				// Wind Animation
				float windTimeOffset = __windTimeOffset;
				float windSpeed = __windSpeed;
				float3 windFrequency = worldPos.xyz * __windFrequency;
				float windPhase = (_Time.y + windTimeOffset) * windSpeed;
				float3 windFactor = sin(windPhase + windFrequency);
				float4 windSin2scale = __windSineScale2;
				float windSin2strength = __windSineStrength2;
				windFactor += sin(windPhase.xxx * windSin2scale.www + windFrequency * windSin2scale.xyz) * windSin2strength;
				float3 windDir = normalize(__windDirection);
				float3 windMask = __windMask;
				float windStrength = __windStrength;
				worldPos.xyz += windDir * windFactor * windMask * windStrength;
				#endif
				input.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif
				float4 clipPos = vertexInput.positionCS;

				float4 screenPos = ComputeScreenPos(clipPos);
				output.screenPosition.xyzw = screenPos;

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal, input.tangent);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// Computes fog factor per-vertex
				output.worldPosAndFog.w = ComputeFogFactor(vertexInput.positionCS.z);

				// normal
				output.normal = normalize(vertexNormalInput.normalWS);

				// tangent
				output.pack1.xyz = vertexNormalInput.tangentWS;
				output.pack2.xyz = vertexNormalInput.bitangentWS;

				// clip position
				output.positionCS = vertexInput.positionCS;

				return output;
			}

			// Copy of LWRP's GetAdditionalLight() with different falloff calculation
			Light GetAdditionalLight_BypassFalloff(int i, float3 positionWS)
			{
				int perObjectLightIndex = GetPerObjectLightIndex(i);

				float3 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex].xyz;
				half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
				half4 spotDirection = _AdditionalLightsSpotDir[perObjectLightIndex];

				float3 lightVector = lightPositionWS - positionWS;
				float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);

				half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
				// original line:
				//half attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy);
				half attenuation = saturate(1 - distanceSqr * distanceAndSpotAttenuation.x);
				attenuation *= AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw);

				Light light;
				light.direction = lightDirection;
				light.distanceAttenuation = attenuation;
				light.shadowAttenuation = AdditionalLightRealtimeShadow(perObjectLightIndex, positionWS);
				light.color = _AdditionalLightsColor[perObjectLightIndex].rgb;

				return light;
			}

			half4 Fragment(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = normalize(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half3 tangentWS = input.pack1.xyz;
				half3 bitangentWS = input.pack2.xyz;
				#if defined(_NORMALMAP)
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);
				#endif

				//Screen Space UV
				float2 screenUV = input.screenPosition.xyzw.xy / input.screenPosition.xyzw.w;
				
				// Shader Properties Sampling
				float4 __normalMap = ( TCP2_TEX2D_SAMPLE(_BumpMap, _BumpMap, input.pack3.xy).rgba );
				float __bumpScale = ( _BumpScale );
				float4 __albedo = ( TCP2_TEX2D_SAMPLE(_BaseMap, _BaseMap, input.pack3.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __ambientIntensity = ( 1.0 );
				float3 __emission = ( float3(1,1,1) * TCP2_TEX2D_SAMPLE(_EmissionText, _EmissionText, input.pack3.xy).rgb * _EmissionColor.rgb * 1.0 );
				float3 __matcapColor = ( _MatCapColor.rgb * TCP2_TEX2D_SAMPLE(_MatCapColor2, _MatCapColor2, input.pack3.xy).rgb * _MatCapColor1.rgb );
				float __stylizedThreshold = ( TCP2_TEX2D_SAMPLE(_StylizedThreshold, _StylizedThreshold, input.pack3.xy * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
				float __stylizedThresholdScale = ( 1.0 );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float __bandsCount = ( _BandsCount );
				float __bandsCrispSmoothing = ( 2.0 );
				float3 __sketchTexture = ( TCP2_TEX2D_SAMPLE(_SketchTexture, _SketchTexture, screenUV * _SketchTexture_TexelSize.xy * _ScreenParams.xy * _SketchTexture_ST.xy + _SketchTexture_ST.zw).aaa );
				float __sketchAntialiasing = ( 20.0 );
				float __sketchThresholdScale = ( 1.0 );
				float __sketchMin = ( _SketchMin );
				float __sketchMax = ( _SketchMax );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float3 __sketchColor = ( float3(0,0,0) );
				float __specularSmoothness = ( _SpecularSmoothness );
				float __specularToonBands = ( _SpecularToonBands );
				float __specularShadowAttenuation = ( _SpecularShadowAttenuation );
				float3 __specularColor = ( _SpecularColor.rgb * __specularShadowAttenuation.xxx * __specularSmoothness.xxx * _SpecularColor2 * TCP2_TEX2D_SAMPLE(_SpecularColor1, _SpecularColor1, input.pack3.xy).rgb );
				float __rampThresholdOtherLights = ( _RampThresholdOtherLights );
				float __rampSmoothingOtherLights = ( _RampSmoothingOtherLights );
				float __bandsCountOtherLights = ( _BandsCountOtherLights );
				float __fresnelMin = ( _FresnelMin );
				float __fresnelMax = ( _FresnelMax );

				#if defined(_NORMALMAP)
				half4 normalMap = __normalMap;
				half3 normalTS = UnpackNormalScale(normalMap, __bumpScale);
					#if defined(_NORMALMAP)
				normalWS = normalize( mul(normalTS, tangentToWorldMatrix) );
					#endif
				#endif

				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;

				half3 emission = half3(0,0,0);
				
				albedo *= __mainColor.rgb;

				// main light: direction, color, distanceAttenuation, shadowAttenuation
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord = input.shadowCoord;
			#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
			#else
				float4 shadowCoord = float4(0, 0, 0, 0);
			#endif

			#if defined(URP_10_OR_NEWER)
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
					half4 shadowMask = SAMPLE_SHADOWMASK(input.pack3.zw);
				#elif !defined (LIGHTMAP_ON)
					half4 shadowMask = unity_ProbesOcclusion;
				#else
					half4 shadowMask = half4(1, 1, 1, 1);
				#endif

				Light mainLight = GetMainLight(shadowCoord, positionWS, shadowMask);
			#else
				Light mainLight = GetMainLight(shadowCoord);
			#endif

				// ambient or lightmap
			#ifdef LIGHTMAP_ON
				// Normal is required in case Directional lightmaps are baked
				half3 bakedGI = SampleLightmap(input.pack3.zw, normalWS);
				MixRealtimeAndBakedGI(mainLight, normalWS, bakedGI, half4(0, 0, 0, 0));
			#else
				// Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
				// are also defined in case you want to sample some terms per-vertex.
				half3 bakedGI = SampleSH(normalWS);
			#endif
				half occlusion = 1;

				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;
				emission += __emission;

				//MatCap
				#if defined(TCP2_MATCAP)
				half2 capCoord = mul((float3x3)UNITY_MATRIX_V, normalWS).xy * 0.5 + 0.5;
				fixed3 matcap = tex2D(_MatCapTex, capCoord).rgb * __matcapColor;
				emission += matcap;
				#endif

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				#if defined(TCP2_TEXTURED_THRESHOLD)
				float stylizedThreshold = __stylizedThreshold;
				stylizedThreshold -= 0.5;
				stylizedThreshold *= __stylizedThresholdScale;
				ndl += stylizedThreshold;
				#endif
				half3 ramp;
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				half bandsCount = __bandsCount;
				ndl = saturate(ndl);
				half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
				half gradientLength = fwidth(ndl);
				half bandsSmooth = gradientLength * (__bandsCrispSmoothing + bandsCount);
				ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

				// apply attenuation
				ramp *= atten;

				// Sketch
				#if defined(TCP2_SKETCH)
				half3 sketch = __sketchTexture;
				half sketchThresholdWidth = __sketchAntialiasing * fwidth(ndl);
				sketch = smoothstep(sketch - sketchThresholdWidth, sketch, clamp(saturate(ramp * __sketchThresholdScale), __sketchMin, __sketchMax));
				#endif
				#if defined(TCP2_SKETCH)
				indirectDiffuse.rgb *= sketch.rgb;
				#endif

				// highlight/shadow colors
				ramp = lerp(__shadowColor, __highlightColor, ramp);
				
				// output color
				half3 color = half3(0,0,0);
				color += albedo * lightColor.rgb * ramp;
				#if defined(TCP2_SKETCH)
				color.rgb *= lerp(__sketchColor, half3(1,1,1), sketch.rgb);
				#endif

				#if defined(TCP2_SPECULAR)
				//Blinn-Phong Specular
				half3 h = normalize(lightDir + viewDirWS);
				float ndh = max(0, dot (normalWS, h));
				float spec = pow(ndh, __specularSmoothness * 128.0);
				spec = floor(spec * __specularToonBands) / __specularToonBands;
				spec *= ndl;
				spec *= saturate(atten * ndl + __specularShadowAttenuation);
				
				//Apply specular
				emission.rgb += spec * lightColor.rgb * __specularColor;
				#endif

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				uint additionalLightsCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
				{
					Light light = GetAdditionalLight_BypassFalloff(lightIndex, positionWS);
					half atten = light.shadowAttenuation;
					half3 lightDir = light.direction;
					half3 lightColor = light.color.rgb;

					half ndl = dot(normalWS, lightDir);
					ndl *= light.distanceAttenuation;
					#if defined(TCP2_TEXTURED_THRESHOLD)
					float stylizedThreshold = __stylizedThreshold;
					stylizedThreshold -= 0.5;
					stylizedThreshold *= __stylizedThresholdScale;
					ndl += stylizedThreshold;
					#endif
					half3 ramp;
					
					half rampThreshold = __rampThresholdOtherLights;
					half rampSmooth = __rampSmoothingOtherLights * 0.5;
					half bandsCount = __bandsCountOtherLights;
					ndl = saturate(ndl);
					half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
					half gradientLength = fwidth(ndl);
					half bandsSmooth = gradientLength * (__bandsCrispSmoothing + bandsCount);
					ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					// apply highlight color
					ramp = lerp(half3(0,0,0), __highlightColor, ramp);
					
					// output color
					color += albedo * lightColor.rgb * ramp;

					#if defined(TCP2_SPECULAR)
					//Blinn-Phong Specular
					half3 h = normalize(lightDir + viewDirWS);
					float ndh = max(0, dot (normalWS, h));
					float spec = pow(ndh, __specularSmoothness * 128.0);
					spec = floor(spec * __specularToonBands) / __specularToonBands;
					spec *= ndl;
					spec *= saturate(atten * ndl + __specularShadowAttenuation);
					
					//Apply specular
					emission.rgb += spec * lightColor.rgb * __specularColor;
					#endif
				}
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				// apply ambient
				color += indirectDiffuse;

				half3 reflections = half3(0, 0, 0);
				half fresnelMin = __fresnelMin;
				half fresnelMax = __fresnelMax;
				half fresnelTerm = smoothstep(fresnelMin, fresnelMax, 1 - ndvRaw);
				reflections *= fresnelTerm;
				color.rgb += reflections;

				color += emission;

				// Mix the pixel color with fogColor. You can optionally use MixFogColor to override the fogColor with a custom one.
				float fogFactor = input.worldPosAndFog.w;
				color = MixFog(color, fogFactor);

				return half4(color, alpha);
			}
			ENDHLSL
		}

		// Outline
		Pass
		{
			Name "Outline"
			Tags { "LightMode" = "Outline" }
			Tags
			{
			}
			Cull Front
			Blend Off

			HLSLPROGRAM
			#pragma vertex vertex_outline
			#pragma fragment fragment_outline
			#pragma target 3.0
			#pragma multi_compile _ TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV1_AS_NORMALS TCP2_UV2_AS_NORMALS TCP2_UV3_AS_NORMALS TCP2_UV4_AS_NORMALS
			#pragma multi_compile _ TCP2_UV_NORMALS_FULL TCP2_UV_NORMALS_ZW
			#pragma multi_compile_instancing
			ENDHLSL
		}
		// Depth & Shadow Caster Passes
		HLSLINCLUDE

		#if defined(SHADOW_CASTER_PASS) || defined(DEPTH_ONLY_PASS)

			#define fixed half
			#define fixed2 half2
			#define fixed3 half3
			#define fixed4 half4

			float3 _LightDirection;
			float3 _LightPosition;

			struct Attributes
			{
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				half4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float3 pack0 : TEXCOORD1; /* pack0.xyz = positionWS */
				float2 pack1 : TEXCOORD2; /* pack1.xy = texcoord0 */
			#if defined(DEPTH_ONLY_PASS)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			#endif
			};

			float4 GetShadowPositionHClip(Attributes input)
			{
				float3 positionWS = TransformObjectToWorld(input.vertex.xyz);
				float3 normalWS = TransformObjectToWorldNormal(input.normal);

				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				return positionCS;
			}

			Varyings ShadowDepthPassVertex(Attributes input)
			{
				Varyings output = (Varyings)0;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				float3 worldNormalUv = mul(unity_ObjectToWorld, float4(input.normal, 1.0)).xyz;

				// Texture Coordinates
				output.pack1.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				// Shader Properties Sampling
				float __windTimeOffset = ( input.vertexColor.g );
				float __windSpeed = ( _WindSpeed );
				float __windFrequency = ( 1.0 );
				float4 __windSineScale2 = ( float4(2.3,1.7,1.4,1.2) );
				float __windSineStrength2 = ( .6 );
				float3 __windDirection = ( _WindDirection.xyz );
				float3 __windMask = ( input.vertexColor.rrr );
				float __windStrength = ( _WindStrength );

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				#if defined(TCP2_WIND)
				// Wind Animation
				float windTimeOffset = __windTimeOffset;
				float windSpeed = __windSpeed;
				float3 windFrequency = worldPos.xyz * __windFrequency;
				float windPhase = (_Time.y + windTimeOffset) * windSpeed;
				float3 windFactor = sin(windPhase + windFrequency);
				float4 windSin2scale = __windSineScale2;
				float windSin2strength = __windSineStrength2;
				windFactor += sin(windPhase.xxx * windSin2scale.www + windFrequency * windSin2scale.xyz) * windSin2strength;
				float3 windDir = normalize(__windDirection);
				float3 windMask = __windMask;
				float windStrength = __windStrength;
				worldPos.xyz += windDir * windFactor * windMask * windStrength;
				#endif
				input.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);

				// Screen Space UV
				float4 screenPos = ComputeScreenPos(vertexInput.positionCS);
				output.normal = normalize(worldNormalUv);
				output.pack0.xyz = vertexInput.positionWS;

				#if defined(DEPTH_ONLY_PASS)
					output.positionCS = TransformObjectToHClip(input.vertex.xyz);
				#elif defined(SHADOW_CASTER_PASS)
					output.positionCS = GetShadowPositionHClip(input);
				#else
					output.positionCS = float4(0,0,0,0);
				#endif

				return output;
			}

			half4 ShadowDepthPassFragment(Varyings input) : SV_TARGET
			{
				#if defined(DEPTH_ONLY_PASS)
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				#endif

				float3 positionWS = input.pack0.xyz;
				float3 normalWS = normalize(input.normal);

				// Shader Properties Sampling
				float4 __albedo = ( TCP2_TEX2D_SAMPLE(_BaseMap, _BaseMap, input.pack1.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );

				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				half3 albedo = half3(1,1,1);
				half alpha = __alpha;
				half3 emission = half3(0,0,0);

				return 0;
			}

		#endif
		ENDHLSL

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile SHADOW_CASTER_PASS

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature_local_vertex TCP2_WIND

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "DepthOnly"
			Tags
			{
				"LightMode" = "DepthOnly"
			}

			ZWrite On
			ColorMask 0

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature_local_vertex TCP2_WIND

			ENDHLSL
		}

		// Used for Baking GI. This pass is stripped from build.
		UsePass "Universal Render Pipeline/Lit/Meta"

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.3.7f1";ver:"2.5.1";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","SPECULAR","RIM_SHADER_FEATURE","RIM_LIGHTMASK","BUMP","BUMP_SHADER_FEATURE","BUMP_SCALE","WIND_ANIM_SIN","WIND_ANIM","WIND_SHADER_FEATURE","ZWRITE","WIND_SIN_2","RIM_VERTEX","TEXTURED_THRESHOLD","SKETCH_SHADER_FEATURE","TT_SHADER_FEATURE","RIM_DIR","FOG","REFLECTION_FRESNEL","REFL_ROUGH","REFLECTION_SHADER_FEATURE","RAMP_MAIN_OTHER","RAMP_SEPARATED","BYPASS_LIGHT_FALLOFF","RAMP_BANDS_CRISP","OUTLINE_URP_FEATURE","SPEC_LEGACY","SKETCH_GRADIENT","EMISSION","UNITY_2020_1","MATCAP_ADD","MATCAP","MATCAP_PERSPECTIVE_CORRECTION","MATCAP_PIXEL","MATCAP_SHADER_FEATURE","OUTLINE","OUTLINE_CLIP_SPACE","OUTLINE_MIN_WIDTH","SPECULAR_TOON_BAND","SPECULAR_NO_ATTEN","SPECULAR_SHADER_FEATURE","ENABLE_LIGHTMAP","ENABLE_META_PASS","SHADOW_COLOR_MAIN_DIR","SKETCH_AMBIENT","OUTLINE_OPAQUE","OUTLINE_LIGHTING_WRAP","OUTLINE_LIGHTING_VERT","OUTLINE_LIGHTING","UNITY_2019_4","TEMPLATE_LWRP"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[,sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:True;cc:4;chan:"RGBA";prop:"_BaseColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"2f0a77a7-a2a7-4f36-94d0-25da9f804dd6";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,,,,,,,sp(name:"Specular Color";imps:list[imp_mp_color(def:RGBA(0.5, 0.5, 0.5, 1);hdr:True;cc:3;chan:"RGB";prop:"_SpecularColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"c28cb412-a2c6-48b4-a3b1-a3656499fc54";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:0),imp_spref(cc:3;chan:"XXX";lsp:"Specular Shadow Attenuation";guid:"de47a031-e730-4b52-9a87-123b9c3026ac";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:-1),imp_spref(cc:3;chan:"XXX";lsp:"Specular Smoothness";guid:"25b0cd1f-4431-4280-9cad-92e27b3c0aa8";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:-1),imp_mp_float(def:1;prop:"_SpecularColor2";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"d4e1542e-272b-40e2-9ac1-ee11bd16b880";op:Multiply;lbl:"Specular Color Float";gpu_inst:False;locked:False;impl_index:-1),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_SpecularColor1";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"a868bb53-a742-49ee-af04-3ca5da331dca";op:Multiply;lbl:"Specular Color Texture";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,sp(name:"Emission";imps:list[imp_constant(type:color;fprc:float;fv:1;f2v:(1, 1);f3v:(1, 1, 1);f4v:(1, 1, 1, 1);cv:RGBA(1, 1, 1, 1);guid:"522c3dc3-8eef-4b74-bb2c-0f240e1bdf4c";op:Multiply;lbl:"Emission Color";gpu_inst:False;locked:False;impl_index:-1),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_EmissionText";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"c9e7dc91-cd4f-4943-bc31-de420c794107";op:Multiply;lbl:"Emission Texture";gpu_inst:False;locked:False;impl_index:-1),imp_customcode(prepend_type:Disabled;prepend_code:"";prepend_file:"";prepend_file_block:"";preprend_params:dict[];code:"";guid:"3bd283b7-fcf2-4f00-9eac-b7a96a571b06";op:Multiply;lbl:"Emission";gpu_inst:False;locked:False;impl_index:-1),imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:True;cc:3;chan:"RGB";prop:"_EmissionColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"7e5fd49f-b549-47fb-9bf8-c1f22e2f4cd8";op:Multiply;lbl:"Emission Color";gpu_inst:False;locked:False;impl_index:-1),imp_constant_float(fprc:float;fv:1;guid:"b23c58a6-cb36-4038-b4e0-0e6a2a27d54e";op:Multiply;lbl:"Emission";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,sp(name:"MatCap Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:3;chan:"RGB";prop:"_MatCapColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"34fe55b5-a175-46af-8b28-414d123899d2";op:Multiply;lbl:"MatCap Color";gpu_inst:False;locked:False;impl_index:0),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_MatCapColor2";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"b225b025-7d16-4462-95c4-d5c47b77d3b6";op:Multiply;lbl:"MatCap Color Texture";gpu_inst:False;locked:False;impl_index:-1),imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:3;chan:"RGB";prop:"_MatCapColor1";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"5077d1d6-bfba-46f7-a7e1-e9bfd5f0f4f5";op:Multiply;lbl:"MatCap Color Color";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,sp(name:"Sketch Texture";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:True;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"black";locked_uv:False;uv:4;cc:3;chan:"AAA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:ScreenSpace;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_SketchTexture";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"b35ea7eb-5b16-462e-ab42-d9d164dd429c";op:Multiply;lbl:"Sketch Texture";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,sp(name:"Outline Width";imps:list[imp_mp_range(def:1;min:0;max:8;prop:"_OutlineWidth";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"13abb586-c53f-46b7-b604-d09d57060eb0";op:Multiply;lbl:"Width";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,sp(name:"Outline Color Vertex";imps:list[imp_mp_color(def:RGBA(0, 0, 0, 1);hdr:True;cc:4;chan:"RGBA";prop:"_OutlineColorVertex";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"b5340b7a-ade3-4f81-9346-a878243de726";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,sp(name:"Wind Speed";imps:list[imp_mp_range(def:2.5;min:0;max:20;prop:"_WindSpeed";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"fdeca372-5269-4695-a428-3e757b68a358";op:Multiply;lbl:"Speed";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,sp(name:"Wind Strength";imps:list[imp_mp_range(def:1;min:0;max:20;prop:"_WindStrength";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"e1208f82-aad3-4d06-b65b-f506cbef9793";op:Multiply;lbl:"Strength";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),,,,,,,,,,,,,,,,,,,,sp(name:"Reflection Cubemap Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:True;cc:3;chan:"RGB";prop:"_ReflectionCubemapColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"cb49a83c-d128-48f2-9582-28e7930652a8";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_ReflectionCubemapColor1";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"015f082d-c4a4-4cba-8e4b-2a862c239178";op:Multiply;lbl:"Reflection Cubemap Color Texture";gpu_inst:False;locked:False;impl_index:-1),imp_constant(type:color;fprc:float;fv:1;f2v:(1, 1);f3v:(1, 1, 1);f4v:(1, 1, 1, 1);cv:RGBA(1, 1, 1, 1);guid:"732854c0-f518-413a-ad57-b499d1e0528e";op:Multiply;lbl:"Reflection Cubemap Color";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Outline ZSmooth";imps:list[imp_mp_range(def:0;min:-20;max:20;prop:"_OutlineZSmooth";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"c9583dfb-4e4d-47b2-8d3a-f2643feee05d";op:Multiply;lbl:"Z Correction";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Rim Color";imps:list[imp_mp_color(def:RGBA(0.8, 0.8, 0.8, 0.5);hdr:True;cc:3;chan:"RGB";prop:"_RimColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"d563a20c-2cc0-4c4c-9ad9-4cadcb8b2acb";op:Multiply;lbl:"Rim Color";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Rim Dir Vert";imps:list[imp_mp_vector(def:(0, 0, 1, 0);fp:float;cc:3;chan:"XYZ";prop:"_RimDirVert";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"77b278df-327a-4d41-8646-3720693ce037";op:Multiply;lbl:"Rim Direction";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];clones:dict[];isClone:False),sp(name:"Outline Lighting Wrap Factor Fragment";imps:list[imp_spref(cc:1;chan:"R";lsp:"Outline Color";guid:"5bb3436f-d2d2-4bd5-9f67-f8138198b979";op:Multiply;lbl:"Wrap Factor";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH cef49a57d5e88d72395f68c606265be3 */
