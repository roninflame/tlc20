// Toony Colors Pro+Mobile 2
// (c) 2014-2020 Jean Moreno

Shader "Toony Colors Pro 2/User/Shader Hit Alpha"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		_Alpha ("Alpha", Range(0,1)) = 1
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		[Header(Main Directional Light)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		[Header(Other Lights)]
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
		_SpecularToonSize ("Toon Size", Range(0,1)) = 0.25
		_SpecularToonSmoothness ("Toon Smoothness", Range(0.001,0.5)) = 0.05
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[NoScaleOffset] _EmissionText ("Emission Texture", 2D) = "white" {}
		 [TCP2ColorNoAlpha] [HDR] _EmissionColor ("Emission Color", Color) = (1,1,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] [HDR] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		//Rim Direction
		_RimDirVert ("Rim Direction", Vector) = (0,0,1,0)
		[TCP2Separator]

		[TCP2HeaderHelp(Reflections)]
		[Toggle(TCP2_REFLECTIONS)] _UseReflections ("Enable Reflections", Float) = 0
		
		[NoScaleOffset] _Cube ("Reflection Cubemap", Cube) = "black" {}
		[TCP2ColorNoAlpha] [HDR] _ReflectionCubemapColor ("Color", Color) = (1,1,1,1)
		_ReflectionCubemapRoughness ("Cubemap Roughness", Range(0,1)) = 0.5
		_FresnelMin ("Fresnel Min", Range(0,2)) = 0
		_FresnelMax ("Fresnel Max", Range(0,2)) = 1.5
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
		
		[Header(Wind)]
		[Toggle(TCP2_WIND)] _UseWind ("Enable Wind", Float) = 0
		_WindDirection ("Direction", Vector) = (1,0,0,0)
		_WindStrength ("Strength", Range(0,20)) = 1
		_WindSpeed ("Speed", Range(0,20)) = 2.5
		
		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjectors"="True"
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		
		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		Pass
		{
			Name "Main"
			Tags { "LightMode"="UniversalForward" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			//#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

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
			#pragma shader_feature TCP2_SPECULAR
			#pragma shader_feature TCP2_RIM_LIGHTING
			#pragma shader_feature TCP2_REFLECTIONS
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature TCP2_SKETCH
			#pragma shader_feature TCP2_TEXTURED_THRESHOLD
			#pragma shader_feature TCP2_WIND

			// Uniforms
			CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float _WindSpeed;
			float4 _WindDirection;
			float _WindStrength;
			float4 _RimDirVert;
			float _RimMinVert;
			float _RimMaxVert;
			sampler2D _BumpMap;
			float _BumpScale;
			sampler2D _BaseMap;
			float4 _BaseMap_ST;
			float _Alpha;
			fixed4 _BaseColor;
			sampler2D _EmissionText;
			half4 _EmissionColor;
			sampler2D _StylizedThreshold;
			float4 _StylizedThreshold_ST;
			float _RampThreshold;
			float _RampSmoothing;
			float _BandsCount;
			half4 _RimColor;
			float _SpecularToonSize;
			float _SpecularToonSmoothness;
			float _SpecularShadowAttenuation;
			half4 _SpecularColor;
			float _SpecularColor2;
			sampler2D _SpecularColor1;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _BandsCountOtherLights;
			sampler2D _SketchTexture;
			float4 _SketchTexture_ST;
			float4 _SketchTexture_TexelSize;
			float _SketchMin;
			float _SketchMax;
			fixed4 _SColor;
			fixed4 _HColor;
			float _ReflectionCubemapRoughness;
			half4 _ReflectionCubemapColor;
			float _FresnelMin;
			float _FresnelMax;
			samplerCUBE _Cube;
			CBUFFER_END

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
				float4 pack2 : TEXCOORD5; /* pack2.xyz = bitangent  pack2.w = rim */
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
				float3 __rimDirVert = ( _RimDirVert.xyz );
				float __rimMinVert = ( _RimMinVert );
				float __rimMaxVert = ( _RimMaxVert );

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
				output.normal = NormalizeNormalPerVertex(vertexNormalInput.normalWS);

				// tangent
				output.pack1.xyz = vertexNormalInput.tangentWS;
				output.pack2.xyz = vertexNormalInput.bitangentWS;

				// clip position
				output.positionCS = vertexInput.positionCS;

				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - vertexInput.positionWS);

				#if defined(TCP2_RIM_LIGHTING)
				half3 rViewDir = viewDirWS;
				half3 rimDir = __rimDirVert;
				rViewDir = normalize(UNITY_MATRIX_V[0].xyz * rimDir.x + UNITY_MATRIX_V[1].xyz * rimDir.y + UNITY_MATRIX_V[2].xyz * rimDir.z);
				half rim = 1.0f - saturate(dot(rViewDir, input.normal.xyz));
				rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
				output.pack2.w = rim;
				#endif

				return output;
			}

			half4 Fragment(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = NormalizeNormalPerPixel(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half3 tangentWS = input.pack1.xyz;
				half3 bitangentWS = input.pack2.xyz;
				#if defined(_NORMALMAP)
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);
				#endif

				//Screen Space UV
				float2 screenUV = input.screenPosition.xyzw.xy / input.screenPosition.xyzw.w;
				
				// Shader Properties Sampling
				float4 __normalMap = ( tex2D(_BumpMap, input.pack3.xy.xy).rgba );
				float __bumpScale = ( _BumpScale );
				float4 __albedo = ( tex2D(_BaseMap, input.pack3.xy.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( _Alpha * __mainColor.a );
				float __ambientIntensity = ( 1.0 );
				float3 __emission = ( float3(1,1,1) * tex2D(_EmissionText, input.pack3.xy.xy).rgb * _EmissionColor.rgb * 1.0 );
				float __stylizedThreshold = ( tex2D(_StylizedThreshold, input.pack3.xy.xy * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
				float __stylizedThresholdScale = ( 1.0 );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float __bandsCount = ( _BandsCount );
				float3 __rimColor = ( _RimColor.rgb );
				float __rimStrength = ( 1.0 );
				float __specularToonSize = ( _SpecularToonSize );
				float __specularToonSmoothness = ( _SpecularToonSmoothness );
				float __specularShadowAttenuation = ( _SpecularShadowAttenuation );
				float3 __specularColor = ( _SpecularColor.rgb * _SpecularColor2 * tex2D(_SpecularColor1, input.pack3.xy.xy).rgb );
				float __rampThresholdOtherLights = ( _RampThresholdOtherLights );
				float __rampSmoothingOtherLights = ( _RampSmoothingOtherLights );
				float __bandsCountOtherLights = ( _BandsCountOtherLights );
				float3 __sketchTexture = ( tex2D(_SketchTexture, screenUV * _SketchTexture_TexelSize.xy * _ScreenParams.xy * _SketchTexture_ST.xy + _SketchTexture_ST.zw).aaa );
				float __sketchAntialiasing = ( 20.0 );
				float __sketchThresholdScale = ( 1.0 );
				float __sketchMin = ( _SketchMin );
				float __sketchMax = ( _SketchMax );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float3 __sketchColor = ( float3(0,0,0) );
				float __reflectionCubemapRoughness = ( _ReflectionCubemapRoughness );
				float3 __reflectionCubemapColor = ( _ReflectionCubemapColor.rgb * float3(1,1,1) );
				float __fresnelMin = ( _FresnelMin );
				float __fresnelMax = ( _FresnelMax );

				#if defined(_NORMALMAP)
				
				// Normal Mapping
				half4 normalMap = __normalMap;
				half3 normalTS = UnpackNormalScale(normalMap, __bumpScale);
				normalWS = mul(normalTS, tangentToWorldMatrix);
				#endif

				half ndv = max(0, dot(viewDirWS, normalWS));
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
				Light mainLight = GetMainLight(shadowCoord);

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
				#if defined(TCP2_SKETCH)
				#endif
				emission += __emission;

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
				ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
				ramp = (round(ramp * bandsCount) / bandsCount) * step(ndl, 1);

				// apply attenuation
				ramp *= atten;

				half3 color = half3(0,0,0);
				// Rim Lighting
				#if defined(TCP2_RIM_LIGHTING)
				half rim = input.pack2.w;
				rim = ( rim );
				half3 rimColor = __rimColor;
				half rimStrength = __rimStrength;
				//Rim light mask
				color.rgb += ndl * atten * rim * rimColor * rimStrength;
				#endif
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

				#if defined(TCP2_SPECULAR)
				//Blinn-Phong Specular
				half3 h = normalize(lightDir + viewDirWS);
				float ndh = max(0, dot (normalWS, h));
				float spec = smoothstep(__specularToonSize + __specularToonSmoothness, __specularToonSize - __specularToonSmoothness,1 - (ndh / (1+__specularToonSmoothness)));
				spec *= ndl;
				spec *= saturate(atten * ndl + __specularShadowAttenuation);
				
				//Apply specular
				color.rgb += spec * lightColor.rgb * __specularColor;
				#endif

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				uint additionalLightsCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
				{
					Light light = GetAdditionalLight(lightIndex, positionWS);
					half atten = light.shadowAttenuation * light.distanceAttenuation;
					half3 lightDir = light.direction;
					half3 lightColor = light.color.rgb;

					half ndl = dot(normalWS, lightDir);
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
					ramp = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
					ramp = (round(ramp * bandsCount) / bandsCount) * step(ndl, 1);

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColors += ramp * lightColor.rgb;

					#if defined(TCP2_SPECULAR)
					//Blinn-Phong Specular
					half3 h = normalize(lightDir + viewDirWS);
					float ndh = max(0, dot (normalWS, h));
					float spec = smoothstep(__specularToonSize + __specularToonSmoothness, __specularToonSize - __specularToonSmoothness,1 - (ndh / (1+__specularToonSmoothness)));
					spec *= ndl;
					spec *= saturate(atten * ndl + __specularShadowAttenuation);
					
					//Apply specular
					color.rgb += spec * lightColor.rgb * __specularColor;
					#endif
					#if defined(TCP2_RIM_LIGHTING)
					// Rim light mask
					half3 rimColor = __rimColor;
					half rimStrength = __rimStrength;
					color.rgb += ndl * atten * rim * rimColor * rimStrength;
					#endif
				}
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				accumulatedRamp = saturate(accumulatedRamp);
				
				// Sketch
				#if defined(TCP2_SKETCH)
				half3 sketch = __sketchTexture;
				half sketchThresholdWidth = __sketchAntialiasing * fwidth(ndl);
				sketch = smoothstep(sketch - sketchThresholdWidth, sketch, clamp(saturate(accumulatedRamp * __sketchThresholdScale), __sketchMin, __sketchMax));
				#endif
				half3 shadowColor = (1 - accumulatedRamp.rgb) * __shadowColor;
				accumulatedRamp = accumulatedColors.rgb * __highlightColor + shadowColor;
				color += albedo * accumulatedRamp;
				#if defined(TCP2_SKETCH)
				color.rgb *= lerp(__sketchColor, half3(1,1,1), sketch);
				#endif

				// apply ambient
				color += indirectDiffuse;

				half3 reflections = half3(0, 0, 0);
				
				#if defined(TCP2_REFLECTIONS)
				//Reflection cubemap
				half3 reflectVector = reflect(-viewDirWS, normalWS);
				reflections.rgb += texCUBElod(_Cube, half4(reflectVector.xyz, __reflectionCubemapRoughness * 10.0)).rgb;
				reflections.rgb *= __reflectionCubemapColor;
				#endif
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

		// Depth & Shadow Caster Passes
		HLSLINCLUDE
		#if defined(SHADOW_CASTER_PASS) || defined(DEPTH_ONLY_PASS)

			#define fixed half
			#define fixed2 half2
			#define fixed3 half3
			#define fixed4 half4

			float3 _LightDirection;

			CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float _WindSpeed;
			float4 _WindDirection;
			float _WindStrength;
			float4 _RimDirVert;
			float _RimMinVert;
			float _RimMaxVert;
			sampler2D _BumpMap;
			float _BumpScale;
			sampler2D _BaseMap;
			float4 _BaseMap_ST;
			float _Alpha;
			fixed4 _BaseColor;
			sampler2D _EmissionText;
			half4 _EmissionColor;
			sampler2D _StylizedThreshold;
			float4 _StylizedThreshold_ST;
			float _RampThreshold;
			float _RampSmoothing;
			float _BandsCount;
			half4 _RimColor;
			float _SpecularToonSize;
			float _SpecularToonSmoothness;
			float _SpecularShadowAttenuation;
			half4 _SpecularColor;
			float _SpecularColor2;
			sampler2D _SpecularColor1;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _BandsCountOtherLights;
			sampler2D _SketchTexture;
			float4 _SketchTexture_ST;
			float4 _SketchTexture_TexelSize;
			float _SketchMin;
			float _SketchMax;
			fixed4 _SColor;
			fixed4 _HColor;
			float _ReflectionCubemapRoughness;
			half4 _ReflectionCubemapColor;
			float _FresnelMin;
			float _FresnelMax;
			CBUFFER_END

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
				float4 screenPosition : TEXCOORD0;
				float4 pack1 : TEXCOORD1; /* pack1.xyz = positionWS  pack1.w = rim */
				float2 pack2 : TEXCOORD2; /* pack2.xy = texcoord0 */
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
				positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
			#else
				positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
			#endif

				return positionCS;
			}

			Varyings ShadowDepthPassVertex(Attributes input)
			{
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				// Texture Coordinates
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				// Shader Properties Sampling
				float __windTimeOffset = ( input.vertexColor.g );
				float __windSpeed = ( _WindSpeed );
				float __windFrequency = ( 1.0 );
				float4 __windSineScale2 = ( float4(2.3,1.7,1.4,1.2) );
				float __windSineStrength2 = ( .6 );
				float3 __windDirection = ( _WindDirection.xyz );
				float3 __windMask = ( input.vertexColor.rrr );
				float __windStrength = ( _WindStrength );

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - vertexInput.positionWS);
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

				//Screen Space UV
				float4 screenPos = ComputeScreenPos(vertexInput.positionCS);
				output.screenPosition.xyzw = screenPos;
				output.normal = NormalizeNormalPerVertex(vertexNormalInput.normalWS);
				output.pack1.xyz = vertexInput.positionWS;

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

				float3 positionWS = input.pack1.xyz;

				// Shader Properties Sampling
				float4 __albedo = ( tex2D(_BaseMap, input.pack2.xy.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( _Alpha * __mainColor.a );

				float3 normalWS = NormalizeNormalPerPixel(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half ndv = max(0, dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				half3 albedo = __albedo.rgb;
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				return 0;
			}

		#endif
		ENDHLSL

		Pass
		{
			Name "ShadowCaster"
			Tags{"LightMode" = "ShadowCaster"}

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile SHADOW_CASTER_PASS

			// -------------------------------------
			// Material Keywords
			//#pragma shader_feature _ALPHATEST_ON
			//#pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment
			
			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature TCP2_WIND

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}

			ZWrite On
			ColorMask 0

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// -------------------------------------
			// Material Keywords
			// #pragma shader_feature _ALPHATEST_ON
			// #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment
			
			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature TCP2_WIND

			ENDHLSL
		}

		// Depth prepass
		// UsePass "Universal Render Pipeline/Lit/DepthOnly"

		// Used for Baking GI. This pass is stripped from build.
		UsePass "Universal Render Pipeline/Lit/Meta"
	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2019.4.11f1";ver:"2.5.1";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","SPEC_LEGACY","SPECULAR","RIM","RIM_SHADER_FEATURE","RIM_LIGHTMASK","BUMP","BUMP_SHADER_FEATURE","BUMP_SCALE","WIND_ANIM_SIN","WIND_ANIM","WIND_SHADER_FEATURE","ZWRITE","WIND_SIN_2","RIM_VERTEX","TEXTURED_THRESHOLD","SKETCH_SHADER_FEATURE","TT_SHADER_FEATURE","RIM_DIR","RIM_DIR_PERSP_CORRECTION","RAMP_BANDS_CRISP_NO_AA","SKETCH_GRADIENT","FOG","ENABLE_LIGHTMAP","SPECULAR_NO_ATTEN","ENABLE_META_PASS","REFLECTION_CUBEMAP","REFLECTION_FRESNEL","REFL_ROUGH","REFLECTION_SHADER_FEATURE","SPECULAR_TOON","SPECULAR_SHADER_FEATURE","EMISSION","RAMP_MAIN_OTHER","RAMP_SEPARATED","TEMPLATE_LWRP","ALPHA_BLENDING","SHADER_BLENDING"];flags:list[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[,,sp(name:"Alpha";imps:list[imp_mp_range(def:1;min:0;max:1;prop:"_Alpha";md:"";custom:False;refs:"";guid:"1ad0aa80-b63d-4c82-81db-bf4e3d805eea";op:Multiply;lbl:"Alpha";gpu_inst:False;locked:False;impl_index:-1),imp_spref(cc:1;chan:"A";lsp:"Main Color";guid:"dc72e473-1b6b-4278-a948-273d4cf45777";op:Multiply;lbl:"Alpha";gpu_inst:False;locked:False;impl_index:1)]),,,,,,,,,,sp(name:"Specular Color";imps:list[imp_mp_color(def:RGBA(0.500, 0.500, 0.500, 1.000);hdr:True;cc:3;chan:"RGB";prop:"_SpecularColor";md:"";custom:False;refs:"";guid:"c28cb412-a2c6-48b4-a3b1-a3656499fc54";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:0),imp_mp_float(def:1;prop:"_SpecularColor2";md:"";custom:False;refs:"";guid:"d4e1542e-272b-40e2-9ac1-ee11bd16b880";op:Multiply;lbl:"Specular Color Float";gpu_inst:False;locked:False;impl_index:-1),imp_mp_texture(uto:False;tov:"";gto:False;sbt:False;scr:False;scv:"";gsc:False;roff:False;goff:False;notile:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_SpecularColor1";md:"";custom:False;refs:"";guid:"a868bb53-a742-49ee-af04-3ca5da331dca";op:Multiply;lbl:"Specular Color Texture";gpu_inst:False;locked:False;impl_index:-1)]),,,,sp(name:"Rim Color";imps:list[imp_mp_color(def:RGBA(0.800, 0.800, 0.800, 0.500);hdr:True;cc:3;chan:"RGB";prop:"_RimColor";md:"";custom:False;refs:"";guid:"d563a20c-2cc0-4c4c-9ad9-4cadcb8b2acb";op:Multiply;lbl:"Rim Color";gpu_inst:False;locked:False;impl_index:0)]),,,,sp(name:"Rim Dir Vert";imps:list[imp_mp_vector(def:(0.0, 0.0, 1.0, 0.0);fp:float;cc:3;chan:"XYZ";prop:"_RimDirVert";md:"";custom:False;refs:"";guid:"77b278df-327a-4d41-8646-3720693ce037";op:Multiply;lbl:"Rim Direction";gpu_inst:False;locked:False;impl_index:0)]),,sp(name:"Emission";imps:list[imp_constant(type:color;fprc:float;fv:1;f2v:(1.0, 1.0);f3v:(1.0, 1.0, 1.0);f4v:(1.0, 1.0, 1.0, 1.0);cv:RGBA(1.000, 1.000, 1.000, 1.000);guid:"522c3dc3-8eef-4b74-bb2c-0f240e1bdf4c";op:Multiply;lbl:"Emission Color";gpu_inst:False;locked:False;impl_index:-1),imp_mp_texture(uto:False;tov:"";gto:False;sbt:False;scr:False;scv:"";gsc:False;roff:False;goff:False;notile:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_EmissionText";md:"";custom:False;refs:"";guid:"c9e7dc91-cd4f-4943-bc31-de420c794107";op:Multiply;lbl:"Emission Texture";gpu_inst:False;locked:False;impl_index:-1),imp_customcode(prepend_type:Disabled;prepend_code:"";prepend_file:"";prepend_file_block:"";preprend_params:dict[];code:"";guid:"3bd283b7-fcf2-4f00-9eac-b7a96a571b06";op:Multiply;lbl:"Emission";gpu_inst:False;locked:False;impl_index:-1),imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:True;cc:3;chan:"RGB";prop:"_EmissionColor";md:"";custom:False;refs:"";guid:"7e5fd49f-b549-47fb-9bf8-c1f22e2f4cd8";op:Multiply;lbl:"Emission Color";gpu_inst:False;locked:False;impl_index:-1),imp_constant_float(fprc:float;fv:1;guid:"b23c58a6-cb36-4038-b4e0-0e6a2a27d54e";op:Multiply;lbl:"Emission";gpu_inst:False;locked:False;impl_index:-1)]),sp(name:"Reflection Cubemap Color";imps:list[imp_mp_color(def:RGBA(1.000, 1.000, 1.000, 1.000);hdr:True;cc:3;chan:"RGB";prop:"_ReflectionCubemapColor";md:"";custom:False;refs:"";guid:"cb49a83c-d128-48f2-9582-28e7930652a8";op:Multiply;lbl:"Color";gpu_inst:False;locked:False;impl_index:0),imp_constant(type:color;fprc:float;fv:1;f2v:(1.0, 1.0);f3v:(1.0, 1.0, 1.0);f4v:(1.0, 1.0, 1.0, 1.0);cv:RGBA(1.000, 1.000, 1.000, 1.000);guid:"732854c0-f518-413a-ad57-b499d1e0528e";op:Multiply;lbl:"Reflection Cubemap Color";gpu_inst:False;locked:False;impl_index:-1)]),,,,,,,,sp(name:"Sketch Texture";imps:list[imp_mp_texture(uto:True;tov:"";gto:False;sbt:True;scr:False;scv:"";gsc:False;roff:False;goff:False;notile:False;def:"black";locked_uv:False;uv:4;cc:3;chan:"AAA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:ScreenSpace;uv_chan:"XZ";uv_shaderproperty:__NULL__;prop:"_SketchTexture";md:"";custom:False;refs:"";guid:"b35ea7eb-5b16-462e-ab42-d9d164dd429c";op:Multiply;lbl:"Sketch Texture";gpu_inst:False;locked:False;impl_index:0)]),,,,,,,sp(name:"Wind Speed";imps:list[imp_mp_range(def:2.5;min:0;max:20;prop:"_WindSpeed";md:"";custom:False;refs:"";guid:"fdeca372-5269-4695-a428-3e757b68a358";op:Multiply;lbl:"Speed";gpu_inst:False;locked:False;impl_index:0)]),,sp(name:"Wind Strength";imps:list[imp_mp_range(def:1;min:0;max:20;prop:"_WindStrength";md:"";custom:False;refs:"";guid:"e1208f82-aad3-4d06-b65b-f506cbef9793";op:Multiply;lbl:"Strength";gpu_inst:False;locked:False;impl_index:0)])];customTextures:list[]) */
/* TCP_HASH 156492bcd25144e8c94659e50acbd0f7 */
