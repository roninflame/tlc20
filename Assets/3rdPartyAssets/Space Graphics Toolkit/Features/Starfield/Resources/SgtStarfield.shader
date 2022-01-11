Shader "Hidden/SgtStarfield"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Tex", 2D) = "white" {}
		_Scale("Scale", Float) = 1
		_ScaleRecip("Scale Recip", Float) = 1
		_CameraRollAngle("Camera Roll Angle", Float) = 0

		_StretchDirection("Stretch Direction", Vector) = (0,0,0)
		_StretchVector("Stretch Vector", Float) = 0

		_NearTex("Near Tex", 2D) = "white" {}
		_NearScale("Near Scale", Float) = 0

		_PulseOffset("Pulse Offset", Float) = 1

		_SrcMode("Src Mode", Float) = 1 // 1 = One
		_DstMode("Dst Mode", Float) = 1 // 1 = One
		_ZWriteMode("ZWrite Mode", Float) = 8 // 8 = Always

		_ClampSizeMin("Clamp Size Min", Float) = 0
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
			Blend [_SrcMode][_DstMode]
			ZWrite[_ZWriteMode]
			ZTest LEqual
			Cull Off

			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile __ SGT_A // Alpha Test
			#pragma multi_compile __ SGT_B // PowerRGB
			#pragma multi_compile __ LIGHT_2 // Clamp Size
			#pragma multi_compile __ SGT_C // Stretch
			#pragma multi_compile __ SGT_D // Near
			#pragma multi_compile __ LIGHT_1 // Pulse (avoid using SGT_F)
			#include "UnityCG.cginc"

			float4    _Color;
			sampler2D _MainTex;
			float     _Scale;
			float     _ScaleRecip;
			float     _CameraRollAngle;

			float3 _StretchDirection;
			float3 _StretchVector;

			sampler2D _NearTex;
			float     _NearScale;

			float _ClampSizeMin;
			float _ClampSizeScale;

			float _PulseOffset;

			struct a2v
			{
				float4 vertex    : POSITION;
				float4 color     : COLOR;
				float3 normal    : NORMAL; // xy = corner offset, z = angle
				float3 tangent   : TANGENT; // x = pulse offset, y = pulse speed, z = pulse scale
				float2 texcoord0 : TEXCOORD0; // uv
				float2 texcoord1 : TEXCOORD1; // x = radius, y = back or front [-0.5 .. 0.5]

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex    : SV_POSITION;
				float4 color     : COLOR;
				float2 texcoord0 : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1; // mvpos

				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct f2g
			{
				float4 color : SV_TARGET;
			};

			float2 Rotate(float2 v, float a)
			{
				float s = sin(a);
				float c = cos(a);
				return float2(c * v.x - s * v.y, s * v.x + c * v.y);
			}

			void Vert(a2v i, out v2f o)
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float radius = i.texcoord1.x * _Scale;
#if LIGHT_2 // Clamp Size
				float radiusMin = abs(UnityObjectToViewPos(i.vertex).z * (_ClampSizeMin / _ScreenParams.y * _ClampSizeScale));
				float scale     = saturate(radius / radiusMin);
				i.color *= scale; // Darken by shrunk amount
				radius /= scale;
#endif
#if SGT_C // Stretch
				float4 vertexM  = mul(unity_ObjectToWorld, i.vertex);
				float3 up       = cross(_StretchDirection, normalize(vertexM.xyz - _WorldSpaceCameraPos));

				// Uncomment below if you want the stars to be stretched based on their size too
				vertexM.xyz += _StretchVector * i.texcoord1.y; // * radius;
				vertexM.xyz += up * i.normal.y * radius;

				o.vertex    = mul(UNITY_MATRIX_VP, vertexM);
				o.texcoord1 = mul(UNITY_MATRIX_V, vertexM);
#else
#if LIGHT_1 // Pulse
				radius *= 1.0f + sin(i.tangent.x * 3.141592654f + _PulseOffset * i.tangent.y) * i.tangent.z;
#endif
				float3 vertexMV = UnityObjectToViewPos(i.vertex);
				float4 cornerMV = float4(vertexMV, 1.0f);
				float  angle    = _CameraRollAngle + i.normal.z * 3.141592654f;

				i.normal.xy = Rotate(i.normal.xy, angle);

				cornerMV.xy += i.normal.xy * radius;

				o.vertex    = mul(UNITY_MATRIX_P, cornerMV);
				o.texcoord1 = cornerMV.xyz;
#endif
				o.color     = i.color;
				o.texcoord0 = i.texcoord0;
			}

			void Frag(v2f i, out f2g o)
			{
				float dist = length(i.texcoord1);
				o.color = tex2D(_MainTex, i.texcoord0);
#if SGT_B // PowerRGB
				o.color.rgb = pow(o.color.rgb, float3(1.0f, 1.0f, 1.0f) + (1.0f - i.color.rgb) * 10.0f);
#else
				o.color *= i.color;
#endif
				o.color *= _Color;
				o.color.a = saturate(o.color.a);
				o.color *= i.color.a;
#if SGT_D // Near
				float2 near = dist * _NearScale;
				o.color *= tex2D(_NearTex, near);
#endif
				o.color.a = saturate(o.color.a);
#if SGT_A // Alpha Test
				if (o.color.a < 0.5f)
				{
					o.color.a = 0.0f; discard;
				}
				else
				{
					o.color.a = 1.0f;
				}
#endif
			}
			ENDCG
		} // Pass
	} // SubShader
} // Shader