// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Toony Colors Pro 2/User/shader hit colors"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[HideInInspector] __BeginGroup_ShadowHSV ("Shadow HSV", Float) = 0
		_Shadow_HSV_H ("Hue", Range(-180,180)) = 0
		_Shadow_HSV_S ("Saturation", Range(-1,1)) = 0
		_Shadow_HSV_V ("Value", Range(-1,1)) = 0
		[HideInInspector] __EndGroup ("Shadow HSV", Float) = 0
		_BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		[TCP2HeaderHelp(Main Directional Light)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		_BandsSmoothing ("Bands Smoothing", Range(0.001,1)) = 0.1
		[TCP2HeaderHelp(Other Lights)]
		_RampThresholdOtherLights ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothingOtherLights ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCountOtherLights ("Bands Count", Range(1,20)) = 4
		_BandsSmoothingOtherLights ("Bands Smoothing", Range(0.001,1)) = 0.1
		[Space]
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularSmoothness ("Smoothness", Float) = 0.2
		_SpecularToonBands ("Specular Bands", Float) = 3
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[Toggle(_NORMALMAP)] _UseNormalMap ("Enable Normal Mapping", Float) = 0
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1
		[TCP2Separator]
		
		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		//Avoid compile error if the properties are ending with a drawer
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

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		// Uniforms

		// Shader Properties
		sampler2D _BumpMap;
		sampler2D _BaseMap;

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float _BumpScale;
			float4 _BaseMap_ST;
			fixed4 _BaseColor;
			float _RampThreshold;
			float _RampSmoothing;
			float _BandsCount;
			float _BandsSmoothing;
			float _SpecularSmoothness;
			float _SpecularToonBands;
			fixed4 _SpecularColor;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _BandsCountOtherLights;
			float _BandsSmoothingOtherLights;
			float _Shadow_HSV_H;
			float _Shadow_HSV_S;
			float _Shadow_HSV_V;
			fixed4 _SColor;
			fixed4 _HColor;
		CBUFFER_END

		//--------------------------------
		// HSV HELPERS
		// source: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
		
		float3 rgb2hsv(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
		
			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}
		
		float3 hsv2rgb(float3 c)
		{
			c.g = max(c.g, 0.0); //make sure that saturation value is positive
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}
		
		float3 ApplyHSV_3(float3 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return hsv2rgb(hsv);
		}
		float3 ApplyHSV_3(float color, float h, float s, float v) { return ApplyHSV_3(color.xxx, h, s ,v); }
		
		float4 ApplyHSV_4(float4 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return float4(hsv2rgb(hsv), color.a);
		}
		float4 ApplyHSV_4(float color, float h, float s, float v) { return ApplyHSV_4(color.xxxx, h, s, v); }
		
		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		Pass
		{
			Name "Main"
			Tags
			{
				"LightMode"="UniversalForward"
			}

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			#pragma shader_feature _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK

			// -------------------------------------

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature TCP2_SPECULAR
			#pragma shader_feature _NORMALMAP

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
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
				float3 pack0 : TEXCOORD3; /* pack0.xyz = tangent */
				float3 pack1 : TEXCOORD4; /* pack1.xyz = bitangent */
				float2 pack2 : TEXCOORD5; /* pack2.xy = texcoord0 */
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
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal, input.tangent);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// normal
				output.normal = NormalizeNormalPerVertex(vertexNormalInput.normalWS);

				// tangent
				output.pack0.xyz = vertexNormalInput.tangentWS;
				output.pack1.xyz = vertexNormalInput.bitangentWS;

				// clip position
				output.positionCS = vertexInput.positionCS;

				return output;
			}

			half4 Fragment(Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				// Texture Coordinates
				
				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = NormalizeNormalPerPixel(input.normal);
				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
				half3 tangentWS = input.pack0.xyz;
				half3 bitangentWS = input.pack1.xyz;
				#if defined(_NORMALMAP)
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);
				#endif

				// Shader Properties Sampling
				float4 __normalMap = ( tex2D(_BumpMap, input.pack2.xy).rgba );
				float __bumpScale = ( _BumpScale );
				float4 __albedo = ( tex2D(_BaseMap, input.pack2.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __ambientIntensity = ( 1.0 );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float __bandsCount = ( _BandsCount );
				float __bandsSmoothing = ( _BandsSmoothing );
				float __specularSmoothness = ( _SpecularSmoothness );
				float __specularToonBands = ( _SpecularToonBands );
				float3 __specularColor = ( _SpecularColor.rgb );
				float __rampThresholdOtherLights = ( _RampThresholdOtherLights );
				float __rampSmoothingOtherLights = ( _RampSmoothingOtherLights );
				float __bandsCountOtherLights = ( _BandsCountOtherLights );
				float __bandsSmoothingOtherLights = ( _BandsSmoothingOtherLights );
				float3 __maxLightIntensity = ( float3(1,1,1) );
				float __shadowHue = ( _Shadow_HSV_H );
				float __shadowSaturation = ( _Shadow_HSV_S );
				float __shadowValue = ( _Shadow_HSV_V );
				float3 __highlightColor = ( _HColor.rgb );
				float3 __shadowColor = ( _SColor.rgb * __highlightColor.rgb );

				#if defined(_NORMALMAP)
				half4 normalMap = half4(0,0,0,0);
				normalMap = __normalMap;
				half3 normalTS = UnpackNormalScale(normalMap, __bumpScale);
				normalWS = normalize( mul(normalTS, tangentToWorldMatrix) );
				#endif

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
					half4 shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
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
				// Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
				// are also defined in case you want to sample some terms per-vertex.
				half3 bakedGI = SampleSH(normalWS);
				half occlusion = 1;

				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				// apply attenuation
				ndl *= atten;
				half3 ramp;
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				half bandsCount = __bandsCount;
				half bandsSmoothing = __bandsSmoothing;
				ndl = saturate(ndl);
				half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
				half bandsSmooth = bandsSmoothing * 0.5;
				ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

				half3 color = half3(0,0,0);
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

				#if defined(TCP2_SPECULAR)
				//Blinn-Phong Specular
				half3 h = normalize(lightDir + viewDirWS);
				float ndh = max(0, dot (normalWS, h));
				float spec = pow(ndh, __specularSmoothness * 128.0);
				spec = floor(spec * __specularToonBands) / __specularToonBands;
				spec *= ndl;
				spec *= atten;
				
				//Apply specular
				emission.rgb += spec * lightColor.rgb * __specularColor;
				#endif

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				half3 accumulatedColorsAdditional = half3(0, 0, 0);
				uint additionalLightsCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
				{
					#if defined(URP_10_OR_NEWER)
						Light light = GetAdditionalLight(lightIndex, positionWS, shadowMask);
					#else
						Light light = GetAdditionalLight(lightIndex, positionWS);
					#endif
					half atten = light.shadowAttenuation * light.distanceAttenuation;
					half3 lightDir = light.direction;
					half3 lightColor = light.color.rgb;

					half ndl = dot(normalWS, lightDir);
					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ndl *= atten;
					half3 ramp;
					
					half rampThreshold = __rampThresholdOtherLights;
					half rampSmooth = __rampSmoothingOtherLights * 0.5;
					half bandsCount = __bandsCountOtherLights;
					half bandsSmoothing = __bandsSmoothingOtherLights;
					ndl = saturate(ndl);
					half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
					half bandsSmooth = bandsSmoothing * 0.5;
					ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColorsAdditional += ramp * lightColor.rgb;

					#if defined(TCP2_SPECULAR)
					//Blinn-Phong Specular
					half3 h = normalize(lightDir + viewDirWS);
					float ndh = max(0, dot (normalWS, h));
					float spec = pow(ndh, __specularSmoothness * 128.0);
					spec = floor(spec * __specularToonBands) / __specularToonBands;
					spec *= ndl;
					spec *= atten;
					
					//Apply specular
					emission.rgb += spec * lightColor.rgb * __specularColor;
					#endif
				}
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				accumulatedRamp = saturate(accumulatedRamp);
			#ifdef _ADDITIONAL_LIGHTS
				accumulatedColorsAdditional = min(accumulatedColorsAdditional, __maxLightIntensity);
				accumulatedColors += accumulatedColorsAdditional;
			#endif
				
				//Shadow HSV
				float3 albedoShadowHSV = ApplyHSV_3(albedo, __shadowHue, __shadowSaturation, __shadowValue);
				albedo = lerp(albedoShadowHSV, albedo, accumulatedRamp);
				half3 shadowColor = (1 - accumulatedRamp.rgb) * __shadowColor;
				accumulatedRamp = accumulatedColors.rgb * __highlightColor + shadowColor;
				color += albedo * accumulatedRamp;

				// apply ambient
				color += indirectDiffuse;

				color += emission;

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

			struct Attributes
			{
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
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
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				// Texture Coordinates
				output.pack1.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
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

				// Shader Properties Sampling
				float4 __albedo = ( tex2D(_BaseMap, input.pack1.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );

				half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);
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

			ENDHLSL
		}

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.3.7f1";ver:"2.7.1";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2020_1","SPECULAR_SHADER_FEATURE","BUMP","BUMP_SCALE","BUMP_SHADER_FEATURE","RAMP_MAIN_OTHER","RAMP_SEPARATED","CLAMP_LIGHTS_INTENSITY_ADD","SPECULAR","SPEC_LEGACY","SPECULAR_TOON_BAND","ATTEN_AT_NDL","SHADOW_HSV","TEMPLATE_LWRP","RAMP_BANDS"];flags:list[];flags_extra:dict[pragma_gpu_instancing=list[]];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",GPU_INSTANCING_MAX_COUNT_VALUE="50"];shaderProperties:list[,,,,,,,sp(name:"Shadow Color";imps:list[imp_mp_color(def:RGBA(0.2, 0.2, 0.2, 1);hdr:False;cc:3;chan:"RGB";prop:"_SColor";md:"";custom:False;refs:"";guid:"0fd90713-33cd-4df7-a22e-441978584045";op:Multiply;lbl:"Shadow Color";gpu_inst:False;locked:False;impl_index:0),imp_spref(cc:3;chan:"RGB";lsp:"Highlight Color";guid:"f86e445c-7fc4-4902-978c-1ce16240ed03";op:Multiply;lbl:"Shadow Color";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 0333cd782d076d85af08be3ee99b293c */
